using System.Text.Json;
using HRPolicyChatbotRAG.Models.Requests;
using HRPolicyChatbotRAG.Models.Responses;
using HRPolicyChatbotRAG.Interfaces;


namespace HRPolicyChatbotRAG.Services
{
    public class HRChatbotService : IHRChatbotService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HRChatbotService> _logger;

        public HRChatbotService(HttpClient httpClient, ILogger<HRChatbotService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ChatResponse> GetChatResponseAsync(ChatPrompt prompt)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(prompt);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/HR-chatbot", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw Python response: {responseBody}");
                return JsonSerializer.Deserialize<ChatResponse>(responseBody)?? throw new InvalidOperationException("Empty response from Python");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Python backend");
                throw; 
            }
        }
    }
}
