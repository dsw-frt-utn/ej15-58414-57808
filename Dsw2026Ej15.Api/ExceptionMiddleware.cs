namespace Dsw2026Ej15.Api;

using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Deja que la petición siga su curso normal
                await _next(context);
            }
            catch (ValidationException ex)
            {
                // Inciso i.i: Retornar 400 Bad Request
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                // Inciso i.ii: Retornar 500 Problem
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Problem",
                    Detail = "Ocurrió un error inesperado en el servidor."
                };
                
                var result = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(result);
            }
        }
    }
}