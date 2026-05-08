using System.Net;
using System.Text.Json;
using ErpAcademico.Application.Exceptions;

namespace ErpAcademico.WebApi.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<
        ErrorHandlingMiddleware>
        _logger;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<
            ErrorHandlingMiddleware>
            logger)
    {
        _next = next;

        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro não tratado");

            await TratarExcecaoAsync(
                context,
                ex);
        }
    }

    private static async Task
        TratarExcecaoAsync(
            HttpContext context,
            Exception excecao)
    {
        context.Response.ContentType =
            "application/json";

        var (statusCode, mensagem) =
            excecao switch
            {
                NegocioException =>
                    (
                        HttpStatusCode.BadRequest,
                        excecao.Message
                    ),

                KeyNotFoundException =>
                    (
                        HttpStatusCode.NotFound,
                        "Recurso não encontrado."
                    ),

                UnauthorizedAccessException =>
                    (
                        HttpStatusCode.Unauthorized,
                        "Acesso não autorizado."
                    ),

                _ =>
                    (
                        HttpStatusCode.InternalServerError,
                        "Erro interno do servidor."
                    )
            };

        context.Response.StatusCode =
            (int)statusCode;

        var resposta = new
        {
            StatusCode =
                context.Response.StatusCode,

            Mensagem = mensagem,

            Timestamp =
                DateTime.UtcNow
        };

        var json =
            JsonSerializer.Serialize(
                resposta,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy =
                        JsonNamingPolicy
                            .CamelCase
                });

        await context.Response
            .WriteAsync(json);
    }
}