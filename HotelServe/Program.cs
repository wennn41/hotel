using System.Data;
using HotelServe.Repositories;
using HotelServe.Services;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace HotelServe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // 設定資料庫連線字串
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // 註冊 Dapper 的資料庫連線
            builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

            // 註冊 Repository 和 Service
            builder.Services.AddScoped<LoginRepository>();
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddScoped<LineService>();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<INewsRepository, NewsRepository>();
            builder.Services.AddScoped<IContactUsRepository, ContactUsRepository>();
            builder.Services.AddScoped<IContactUsService, ContactUsService>();
			builder.Services.AddScoped<RoomTypeRepository>();
			builder.Services.AddScoped<RoomTypeService>();
            builder.Services.AddScoped<MemberRepository>();
            builder.Services.AddScoped<MemberService>();





            // 註冊 CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:8080") // 前端 URL
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // 如果需要 cookies-based 認證
                });
            });

            //配置 JWT 認證
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],       // 從配置文件讀取
                        ValidAudience = builder.Configuration["Jwt:Audience"],   // 從配置文件讀取
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // 從配置文件讀取密鑰
                    };
                });

            //            配置 Swagger 支援 JWT
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "請輸入 JWT Token，例如：Bearer {token}",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        }
                                    },
                                    Array.Empty<string>() // 不需要特定範圍
                                }
                });
            });




            builder.Services.AddControllers();

            // Swagger/OpenAPI 設定
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // 解決 Cross-Origin-Opener-Policy 問題
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Remove("Cross-Origin-Opener-Policy");
                context.Response.Headers.Remove("Cross-Origin-Embedder-Policy");
                await next();
            });

            // 啟用 CORS
            app.UseCors();

            //啟用 HTTPS
            app.UseHttpsRedirection();

            //            啟用身份驗證
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

