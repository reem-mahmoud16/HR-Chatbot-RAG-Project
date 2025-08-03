using HRPolicyChatbotRAG.Models.Entities;

namespace HRPolicyChatbotRAG.Interfaces
{
    public interface IPromptDBService
    {
        Task SavePromptAsync(string prompt, string answer);
        Task<List<PromptRecord>> GetHistoryAsync(int count = 10);
    }
}
