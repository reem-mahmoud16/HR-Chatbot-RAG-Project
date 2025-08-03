using HRPolicyChatbotRAG.Models.Requests;
using HRPolicyChatbotRAG.Models.Responses;

namespace HRPolicyChatbotRAG.Interfaces
{
    public interface IHRChatbotService
    {
        Task<ChatResponse> GetChatResponseAsync(ChatPrompt prompt);
    }

}
