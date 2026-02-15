using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JobTracker.API.DTOs;
using JobTracker.API.Interfaces;
using JobTracker.API.Repositories;

namespace JobTracker.API.Services
{
    public class InterviewAiService : IInterviewAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
      

        public InterviewAiService(
            HttpClient httpClient,
            IConfiguration configuration
           )
        {
            _httpClient = httpClient;
            _configuration = configuration;
        
        }

        public async Task<InterviewAiResponsedto> GenerateInterviewAsync(InterviewAidto request)
        {
            var difficulty = request.Difficulty ?? "Intermediate";
            var category = request.Category ?? "Mixed type";
            var questionType = request.QuestionType ?? "Normal";
            var Minuetesperquetion = request.MinutesPerQuestion;
            var QuetionLimit = request.QuestionLimit ?? 10;
           
          
           
            var prompt = $@"You are an experienced technical interviewer.
                            Task: Generate {difficulty}-level {category} {questionType} interview questions strictly based on the job description provided below.Time Expectation: {Minuetesperquetion}
                            Instructions:
                            - Output only the interview questions.
                            - Do not provide answers.
                            - Do not provide explanations.
                            - Do not repeat or restate the job description.
                            - Do not include any commentary before or after the questions.
                            - Generate exactly {QuetionLimit} questions.
                            - Each question must be clear, specific, and directly relevant to the job description.
                            - Avoid generic, vague, or unrelated questions.
                            - Avoid unnecessary buzzwords.
                            - Focus on realistic questions that are commonly asked in actual interviews.
                            - Ensure the tone is natural and human-like.
                            Job Description:
                             {request.JobDescription}";

            var apiKey = _configuration["GROQ_APIKEY"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("GROQ_APIKEY environment variable is not configured.");
            }

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);


            var body = new
            {
                model = "llama-3.1-8b-instant",
                messages = new[]
                {
                new { role = "system", content = "You are a professional interview question generator." },
                new { role = "user", content = prompt }}
            };

            var json = JsonSerializer.Serialize(body);

            var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions",new StringContent(json, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) throw new Exception($"Groq API error: {content}");

            var doc = JsonDocument.Parse(content);

            var generatedText = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            if (string.IsNullOrWhiteSpace(generatedText))
            {
                return new InterviewAiResponsedto();
            }

            var lines = generatedText.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).Where(l => char.IsDigit(l.FirstOrDefault())).ToList();




            return new InterviewAiResponsedto
            {
                Questions = lines
            };


        }
    }
}
