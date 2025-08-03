using HRPolicyChatbotRAG.data;
using HRPolicyChatbotRAG.Interfaces;
using HRPolicyChatbotRAG.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRPolicyChatbotRAG.Services
{
    public class PromptDBService : IPromptDBService
    {
        private readonly ChatDBContext _context;
        public PromptDBService(ChatDBContext context)
        {
            _context = context;
        }

        public async Task SavePromptAsync(string prompt, string answer)
        {
            var record = new PromptRecord
            {
                Prompt = prompt,
                Answer = answer,
            };

            _context.PromptRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PromptRecord>> GetHistoryAsync(int count = 10)
        {
            return await _context.PromptRecords
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}

