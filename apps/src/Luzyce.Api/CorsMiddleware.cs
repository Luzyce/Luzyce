namespace Luzyce.Api;

public class CorsMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate next = next;


    public async Task InvokeAsync(HttpContext context)
    {
        IHeaderDictionary headers = context.Response.Headers!;

        headers.Append("Access-Control-Allow-Origin", "*");
        headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization");
        headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");

        if (context.Request.Method == "OPTIONS")
        {
            context.Response.StatusCode = 200;
            await context.Response.CompleteAsync();
            context.ToString();
        }
        else
        {
            await next(context);
            context.ToString();
        }
    }
}
