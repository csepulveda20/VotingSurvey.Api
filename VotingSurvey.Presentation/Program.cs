using VotingSurvey.Application.Settings;
using VotingSurvey.Infrastructure.Settings;
using VotingSurvey.Presentation.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Presentation().Infrastructure(builder.Configuration).Application();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
