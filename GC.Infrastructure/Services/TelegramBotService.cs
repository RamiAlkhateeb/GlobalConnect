using GlobalConnect.Application.Modules.Provider.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace GlobalConnect.Infrastructure.Services
{
    public class TelegramBotService : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _botToken = "8599759300:AAEjySWRnkG21Cwyery5_YVxqD9tuwh8VcA";

        public TelegramBotService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _botClient = new TelegramBotClient(_botToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() };

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
            errorHandler: HandlePollingErrorAsync, // Named 'errorHandler' now
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            },
            cancellationToken: stoppingToken
            );

            await Task.Delay(-1, stoppingToken);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { Text: { } messageText } message) return;

            var chatId = message.Chat.Id;
            using var scope = _serviceProvider.CreateScope();
            var providerService = scope.ServiceProvider.GetRequiredService<IProviderService>();

            // 1. Logic for Search
            if (messageText.StartsWith("/search") || messageText.StartsWith("بحث"))
            {
                string query = messageText.Replace("/search", "").Replace("بحث", "").Trim();
                var providers = await providerService.SearchProvidersAsync(query);

                if (!providers.Any())
                {
                    // 2. Updated to SendMessage
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: "No doctors found / لم يتم العثور على أطباء",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                foreach (var p in providers.Take(5))
                {
                    string responseText = $"👨‍⚕️ *{p.Name}*\n" +
                                     $"Specialty: {p.Specialty}\n" +
                                     $"📍 {p.Nationality}\n\n" +
                                     $"[Book Appointment / احجز الآن]({p.GoogleBookingUrl})";

                    await botClient.SendMessage(
                        chatId: chatId,
                        text: responseText,
                        parseMode: ParseMode.Markdown,
                        cancellationToken: cancellationToken
                    );
                }
            }
            else
            {
                await botClient.SendMessage(
                chatId: chatId,
                text: "Welcome! Type /search [Specialty] to find a doctor.\nأهلاً بك! اكتب 'بحث' متبوعاً بالتخصص للعثور على طبيب.",
                cancellationToken: cancellationToken
            );
            }
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Telegram Error: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}