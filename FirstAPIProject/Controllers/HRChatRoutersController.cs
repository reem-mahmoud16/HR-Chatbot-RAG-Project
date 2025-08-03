using HRPolicyChatbotRAG.Interfaces;
using HRPolicyChatbotRAG.Models.Entities;
using HRPolicyChatbotRAG.Models.Requests;
using HRPolicyChatbotRAG.Models.Responses;
using HRPolicyChatbotRAG.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRPolicyChatbotRAG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRChatRoutersController : ControllerBase
    {
        private readonly IHRChatbotService _hRChatbotHandler;
        private readonly IPromptDBService _promptDBService;

        public HRChatRoutersController(IHRChatbotService hRChatbotHandler, IPromptDBService promptDBService)
        {
            _hRChatbotHandler = hRChatbotHandler;
            _promptDBService = promptDBService;
        }

        [HttpPost("HR-Policy-Chatbot-ask")]
        public async Task<ActionResult<ChatResponse>> PostHRChatbotPrompting([FromBody] ChatPrompt prompt)
        {
            try
            {
                var response = await _hRChatbotHandler.GetChatResponseAsync(prompt);
                await _promptDBService.SavePromptAsync(prompt.Prompt, response.Answer);
                return Ok(response);
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, "Bad Gateway: Error communicating with Python backend");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpGet("HR-Policy-Chatbot-GetHistory")]
        public async Task<ActionResult<ChatResponse>> GetHRChatbotHistory()
        {
            try
            {
                List<PromptRecord>  RequestedHistory = await _promptDBService.GetHistoryAsync(10);
                return Ok(RequestedHistory);
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, "Bad Gateway: Error communicating with Python backend");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
