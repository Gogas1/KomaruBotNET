using KomaruBotASPNET.Configuration;
using KomaruBotASPNET.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Controllers
{
    [Route("bot")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly BotConfiguration _botConfiguration;
        private readonly UpdateService _updateService;

        public BotController(IOptions<BotConfiguration> botConfiguration, UpdateService updateService)
        {
            _botConfiguration = botConfiguration.Value;
            _updateService = updateService;
        }

        [HttpGet("setWebhook")]
        public async Task<string> SetWebhook([FromServices]ITelegramBotClient botClient, CancellationToken ct)
        {
            var webhookUrl = _botConfiguration.WebhookUrl.AbsoluteUri;
            await botClient.DeleteWebhookAsync();
            await botClient.SetWebhookAsync(webhookUrl, allowedUpdates: [], secretToken: _botConfiguration.SecretKey, cancellationToken: ct);
            return $"Webhook set to {webhookUrl}";
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient bot, CancellationToken ct)
        {
            if (Request.Headers["X-Telegram-Bot-Api-Secret-Token"] != _botConfiguration.SecretKey)
            {
                return Forbid();
            }

            try
            {
                await _updateService.HandleUpdateAsync(bot, update, ct);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
