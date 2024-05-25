using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModelLayer.EmailModel;
using ModelLayer.JWTModel;
using NLog.Web;
using RepositoryLayer.Contexts;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System.Text;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Logger
        builder.Logging.AddDebug();
        var logpath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        NLog.GlobalDiagnosticsContext.Set("LogDirectory", logpath);
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Host.UseNLog();
        builder.Services.AddLogging();

        // Dapper context
        builder.Services.AddSingleton<DapperContext>();

        // Email configuration
        builder.Services.Configure<EmailSettingModel>(builder.Configuration.GetSection("SmtpSettings"));
        builder.Services.AddScoped(sp => sp.GetRequiredService<IOptions<EmailSettingModel>>().Value);

        // Services
        builder.Services.AddScoped<IAuthorizationRL, AuthorizationServiceRL>();
        builder.Services.AddScoped<IRegisterationRL, RegisterationServiceRL>();
        builder.Services.AddScoped<IRegisterationBL, RegisterationServiceBL>();
        builder.Services.AddScoped<ILoginRL, LoginServiceRL>();
        builder.Services.AddScoped<ILoginBL, LoginServiceBL>();
        builder.Services.AddScoped<ILabelRL, LabelServiceRL>();
        builder.Services.AddScoped<ILabelBL, LabelServiceBL>();
        builder.Services.AddScoped<INoteRL, NoteServiceRL>();
        builder.Services.AddScoped<INoteBL, NoteServiceBL>();
        builder.Services.AddScoped<ICollaborationRL, CollaborationServiceRL>();
        builder.Services.AddScoped<ICollaborationBL, CollaborationServiceBL>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthorization();

        // JWT configuration
        var jwtSettings = builder.Configuration.GetSection("JwtSetting").Get<JwtSettingModel>();
        if (jwtSettings != null)
        {
            builder.Services.AddSingleton(jwtSettings);

            if(jwtSettings.SecretKey != null)

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
        }

        builder.Services.AddSwaggerGen(c =>
        { 
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization", 
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http, 
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                     new OpenApiSecurityScheme
                     {
                        Reference = new OpenApiReference
                        {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer" 
                        }
                     },
                     Array.Empty<string>()
                }
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}