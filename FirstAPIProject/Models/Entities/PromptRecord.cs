using System.ComponentModel.DataAnnotations;

namespace HRPolicyChatbotRAG.Models.Entities
{
    public class PromptRecord
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(1000)]
        public string Prompt { get; set; } = string.Empty;


        [Required]
        public string Answer { get; set; } = string.Empty;


        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
