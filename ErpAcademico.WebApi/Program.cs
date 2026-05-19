using ErpAcademico.Infrastructure.Data;
using ErpAcademico.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using ErpAcademico.Application.Validators;
using ErpAcademico.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using ErpAcademico.Application.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ProdutoService>();

builder.Services.AddScoped<VendaService>();

builder.Services.AddScoped<OrcamentoService>();

builder.Services.AddScoped<DashboardService>();

builder.Services.AddScoped<EstoqueService>();

builder.Services.AddScoped<MovimentacaoEstoqueService>();

builder.Services.AddScoped<RelatorioService>();

builder.Services.AddScoped<AuthService>();

builder.Services
    .AddControllers();

builder.Services
    .AddFluentValidationAutoValidation();

builder.Services
    .AddValidatorsFromAssemblyContaining<
        EntradaEstoqueValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<
    ProdutoDtoValidator>();

builder.Services
    .Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory =
            context =>
            {
                var erros =
                    context.ModelState
                        .Values
                        .SelectMany(v =>
                            v.Errors)
                        .Select(e =>
                            e.ErrorMessage)
                        .ToList();
            
                var response =
                    new ResponseDto<object>
                    {
                        Sucesso = false,

                        Mensagem =
                            "Erro de validação.",

                        Erros = erros
                    };

                return new BadRequestObjectResult(
                    response);
            };
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddScoped<RelatorioService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "ERP Acadêmico API",
            Version = "v1"
        });

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Digite apenas o token JWT"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        });
});

var jwtKey =
    builder.Configuration["Jwt:Key"];

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey!))
            };
    });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://172.16.22.28:3000"
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors();

app.UseSwagger();

app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();