using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Recipedia.Data;
using Recipedia.Models;
using Recipedia.Repositories;
using Recipedia.Services;

namespace Recipedia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            ConfigurationManager configuration = builder.Configuration;


            // Add services to the container.

            builder.Services.AddControllers();

            // Setup database context and connection string here
            builder.Services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Adding Microsoft identity with config settings
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                // Password Requirements
                options.Password.RequiredLength = 6; // Change to 8
                options.Password.RequireLowercase = false; // Change to true
                options.Password.RequireUppercase = false; // Change to true
                options.Password.RequireDigit = false; // Change to true
                options.Password.RequireNonAlphanumeric = false; // Change to true

                // Ensure email is confirmed
                options.SignIn.RequireConfirmedEmail = false; // Change to true

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 8;
            })
            .AddEntityFrameworkStores<ApplicationContext>() // Connects identity to the database giving its method ability to access it
            .AddDefaultTokenProviders();

            // Add Mailkit email config (not used atm)
            //builder.Services.Configure<MailKitSettings>(configuration.GetSection("MailKitSettings"));

            // Adding authentication
            builder.Services.AddAuthentication(options =>
            {
                // JWT options
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                // Add JWT Bearer
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true; // Allows the server to save the token for the duration of the request
                    options.RequireHttpsMetadata = true; // Enforces HTTPS so tokens aren't transfered over unsecure connections
                    options.TokenValidationParameters = new TokenValidationParameters // The rules of which authorization will check
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])) // Symetric key lets the system know the same secret is used for both signing and verifying the JWT. Then encodes it into bytes
                    };
                });

            // Add authorization
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();

            // Add to scope
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
            builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
            builder.Services.AddScoped<IShoppinglistRepository, ShoppinglistRepository>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<DtoMapperService>();
            builder.Services.AddSingleton(provider =>
                new JwtRepository(provider.GetRequiredService<IConfiguration>()));

            // Configure Swagger to include JWT authorization input
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Define the Bearer Authentication scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization", // Name of the header
                    In = ParameterLocation.Header, // Location of the header
                    Type = SecuritySchemeType.Http, // Type of the security
                    Scheme = "bearer" // Scheme name
                });

                // Make sure Swagger UI requires a Bearer token to be specified
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer" // Must match the scheme name defined in AddSecurityDefinition
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
            });


            // Add to scope
            // scopes...


            var app = builder.Build();

            // Used for the controllers configuration
            app.UseRouting();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }    
}