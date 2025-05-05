using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WebApi_Coris.DataContext;
using WebApi_Coris.Infrastructure.Auth;
using WebApi_Coris.Service;
using WebApi_Coris.Service.Abstractions;
using WebApi_Coris.Service.AuthenticationService;
using WebApi_Coris.Service.InsuredService;
using WebApi_Coris.Service.UserService;
using WebApi_Coris.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using WebApi_Coris.Validators;

var builder = WebApplication.CreateBuilder(args);

// 1) Validations
builder.Services.AddValidatorsFromAssemblyContaining<InsuredDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserDtoValidator>();
builder.Services.AddFluentValidationAutoValidation()
               .AddFluentValidationClientsideAdapters();

// 2) Application services
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<IInsuredInterface, InsuredService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// 3) Password hashers
builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();

// 4) HttpContextAccessor and AbilityHandler
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorizationHandler, AbilityHandler>();

// 5) JWT Authentication
var jwtKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var payload = JsonSerializer.Serialize(new { message = "Não está autenticado" });
            await context.Response.WriteAsync(payload);
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            var payload = JsonSerializer.Serialize(new { message = "Você não tem permissão para acessar este recurso" });
            await context.Response.WriteAsync(payload);
        }
    };
});

// 6) Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FullAccess", policy =>
        policy.Requirements.Add(new AbilityRequirement("*")));
    options.AddPolicy("ReadOnly", policy =>
        policy.RequireAuthenticatedUser());
});

// 7) Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Description = "Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            }, new string[] { }
        }
    });
});

// 8) Controllers
builder.Services.AddControllers();

// 9) DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();