using Microsoft.EntityFrameworkCore;
using HRPolicyChatbotRAG.Models.Entities;

namespace HRPolicyChatbotRAG.data
{
    public class ChatDBContext : DbContext
    {
        public ChatDBContext(DbContextOptions<ChatDBContext> options): base(options) { }

        public DbSet<PromptRecord> PromptRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: Add indexes for performance
            modelBuilder.Entity<PromptRecord>()
                .HasIndex(p => p.CreatedAt);
        }
    }
}
