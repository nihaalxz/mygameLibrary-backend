using apiWebpractice.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace apiWebpractice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();

             //❗ Commented out for debugging formatting error
             builder.Services.AddOpenApi();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Configure password requirements
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                // Configure user settings
                 options.User.RequireUniqueEmail = true;
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ngApp", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            builder.Services.AddIdentityApiEndpoints<Appuser>().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthentication(x=>
            {
                x.DefaultAuthenticateScheme =
                x.DefaultChallengeScheme =
                x.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!))
                };
            });


            var app = builder.Build();

            // Enable middleware
            if (app.Environment.IsDevelopment())
            {
                // ❗ Commented out for debugging formatting error
                app.MapScalarApiReference();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            // Apply CORS
            app.UseCors("ngApp");

            app.UseAuthentication();
            app.UseAuthorization();

            // Serve static files (Angular build goes into wwwroot)
            app.UseDefaultFiles(); // Looks for index.html
            app.UseStaticFiles();
            app.MapGroup("/api")
                .MapIdentityApi<Appuser>();

            // Map API controllers
            app.MapControllers();

            // Fallback to index.html for Angular routing
            app.MapFallbackToFile("index.html");
            app.MapPost("/api/signup",async (
                UserManager<Appuser> userManager,
                [FromBody] UserRegistrationModel userRegistrationModel

                )=>{
                    Appuser user = new Appuser()
                    {
                        UserName = userRegistrationModel.Email, // Use email as username
                        Email =userRegistrationModel.Email,
                        FullName=userRegistrationModel.FullName  
                    };
                  var result= await userManager.CreateAsync(user,userRegistrationModel.Password);
                    if(result.Succeeded)
                    {
                        return Results.Ok(result);
                       
                    }
                    else
                    {
                        return Results.BadRequest(result.Errors);
                    }
                });

            app.MapPost("/api/signin", async ( 
                UserManager<Appuser> userManager,
                [FromBody] Models.LoginModel loginModel) =>
            {

                var user = await userManager.FindByEmailAsync(loginModel.Email);
                if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var signInKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!));

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new System.Security.Claims.ClaimsIdentity(new[]
                        {
                            new Claim("UserID",user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(10),
                        SigningCredentials=new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Results.Ok(new { token });


                }
                else
                {
                    return Results.BadRequest(new {message="Username or password incorrect"});
                }
                 
            } );
                
               

            app.Run();
        }
    }
}
