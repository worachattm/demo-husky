using webapi.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/{id}/user", (string id) => $"Hello {id}!");
app.MapHealthEndpoints();


app.Run();


public partial class Program { }
