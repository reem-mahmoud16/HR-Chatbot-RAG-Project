using HRPolicyChatbotRAG.Interfaces;
using HRPolicyChatbotRAG.Services;
using Microsoft.EntityFrameworkCore;
using HRPolicyChatbotRAG.data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChatDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services
builder.Services.AddHttpClient<IHRChatbotService, HRChatbotService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8001/");
});

builder.Services.AddScoped<IPromptDBService, PromptDBService>();  // Add this line

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
