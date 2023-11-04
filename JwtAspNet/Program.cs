using JwtAspNet.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TokenService>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", (TokenService service) => 
{
    return service.CreateToken(new JwtAspNet.Models.User(1, "Yuri Moyses", "yuri@gmail.com", "https://yuri.io/", "1234", new[] { "student", "admin" }));
}).RequireAuthorization();

app.Run();
