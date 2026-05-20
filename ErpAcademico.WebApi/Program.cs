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
        policy.AllowAnyOrigin()
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

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider
//        .GetRequiredService<AppDbContext>();
//
//    db.Database.Migrate();
//}
// Endpoint de Health Check para o Render saber que a API está online
app.MapGet("/", () => Results.Ok(new { 
    status = "Healthy", 
    projeto = "BikeFlow ERP", 
    ambiente = "Production" 
}));

// Opcional: Se quiser dar suporte ao método HEAD que o Render usou
app.MapMethods("/", new[] { "HEAD" }, () => Results.Ok());
app.Run();