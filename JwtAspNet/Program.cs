using JwtAspNet;
using JwtAspNet.Extentions;
using JwtAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TokenService>();

builder.Services.AddAuthentication(x => 
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x => 
    {
        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(x => 
{
    x.AddPolicy("Admin", p => p.RequireRole("admin"));
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", (TokenService service) => 
{
    return service.CreateToken(new JwtAspNet.Models.User(1, "Yuri Moyses", "yuri@gmail.com", "https://yuri.io/", "1234", new[] { "student", "admin" }));
});

app.MapGet("/restrito", (ClaimsPrincipal user) => new 
    {
        id = user.Id(),
        name = user.Name(),
        givenName = user.GivenName(),
        image = user.Image(),
    }).RequireAuthorization();

app.MapGet("/admin", () => "Voce tem acesso")
    .RequireAuthorization("admin");

app.Run();
