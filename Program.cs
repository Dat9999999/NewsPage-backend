
using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.repositories.interfaces;
using NewsPage.repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NewsPage.helpers;
using StackExchange.Redis;

namespace NewsPage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var isDevelopment = builder.Environment.IsDevelopment();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

            // connect to Redis // xử lý mã otp 
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
                .Connect(builder.Configuration["Redis:ConnectionString"]));


            // 🔹 Lấy thông tin từ appsettings.json
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = !isDevelopment,
                        ValidateAudience = !isDevelopment,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization();

            // Add services to the container.
            //Auth service
            builder.Services.AddScoped<IUserAccountRepository, UserAccountsRepository>();
            builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();
            //JWT token
            builder.Services.AddScoped<JwtHelper>();
            //crypt password
            builder.Services.AddTransient<PasswordHelper>();
            //uploads file
            builder.Services.AddScoped<FileHelper>();

            //Send email
            builder.Services.AddSingleton<MailHelper>();

            //Generate OTP
            builder.Services.AddSingleton<OtpHelper>();



            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
