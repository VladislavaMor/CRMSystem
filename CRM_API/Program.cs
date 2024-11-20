using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CRM_API;
using CRM_API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = int.MaxValue;
});
// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddDbContext<WebApiDBContext>
    (options => options.UseInMemoryDatabase(databaseName: "CRM_Helper"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthentificationOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthentificationOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthentificationOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies[".AspNetCore.Application.Id"];
    if (!string.IsNullOrEmpty(token))
        context.Request.Headers.Add("Authorization", "Bearer " + token);

    await next();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
