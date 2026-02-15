using System.Text;
using JobTracker.API.Data;
using JobTracker.API.Interfaces;
using JobTracker.API.Repositories;
using JobTracker.API.Repositories.Implementations;
using JobTracker.API.Services;
using JobTracker.API.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JobTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<DBHelper>();

            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailServices, EmailServices>();
            builder.Services.AddScoped<IQueriesAndFeedbacksServices, QueriesAndFeedbackServices>();
            builder.Services.AddScoped<IInterviewQuestionOptionDdlService, InterviewQuestionOptionDdlService>();

            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IQueriesAndFeedbackRepository, QueriesAndFeedbackRepository>();
            builder.Services.AddScoped<IInterviewQuestionOptionDdlRepository, InterviewQuestionOptionDdlRepository>();

            builder.Services.AddHttpClient<IInterviewAiService, InterviewAiService>();
            builder.Services.AddHttpClient();

          
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact", policy =>
                {
                    policy
                        .WithOrigins("https://jobtracker-indol.vercel.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
          
            var groqApiKey = Environment.GetEnvironmentVariable("GROQ_APIKEY");
            if (string.IsNullOrWhiteSpace(groqApiKey))
                throw new Exception("GROQ_APIKEY environment variable is missing");

            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEYJOBTRACKER");
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("JWT_KEYJOBTRACKER environment variable is missing");

      
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)
                        ),

                        ClockSkew = TimeSpan.Zero
                    };
                });

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseCors("AllowReact");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
