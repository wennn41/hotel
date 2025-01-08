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

            // �]�w��Ʈw�s�u�r��
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // ���U Dapper ����Ʈw�s�u
            builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

            // ���U Repository �M Service
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





            // ���U CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:8080") // �e�� URL
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // �p�G�ݭn cookies-based �{��
                });
            });

            //�t�m JWT �{��
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],       // �q�t�m���Ū��
                        ValidAudience = builder.Configuration["Jwt:Audience"],   // �q�t�m���Ū��
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // �q�t�m���Ū���K�_
                    };
                });

            //            �t�m Swagger �䴩 JWT
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "�п�J JWT Token�A�Ҧp�GBearer {token}",
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
                                    Array.Empty<string>() // ���ݭn�S�w�d��
                                }
                });
            });




            builder.Services.AddControllers();

            // Swagger/OpenAPI �]�w
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

            // �ѨM Cross-Origin-Opener-Policy ���D
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Remove("Cross-Origin-Opener-Policy");
                context.Response.Headers.Remove("Cross-Origin-Embedder-Policy");
                await next();
            });

            // �ҥ� CORS
            app.UseCors();

            //�ҥ� HTTPS
            app.UseHttpsRedirection();

            //            �ҥΨ�������
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

