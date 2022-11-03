using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddHttpClient("MyHttpClient", (opts) =>
{
    // base address, etc.
}).AddPolicyHandler(GetMyRetryPolicy());

app.MapGet("/", () => "Hello World!");

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetMyRetryPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}