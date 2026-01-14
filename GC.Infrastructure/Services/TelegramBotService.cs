using GlobalConnect.Application.Modules.Provider.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Text;
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

        private static ConcurrentDictionary<long, List<int>> _userSearches = new();
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
            
            // 1. Check if the user is replying with a Number (Selection)
            if (int.TryParse(messageText, out int selection) && _userSearches.ContainsKey(chatId))
            {
                var resultList = _userSearches[chatId];

                // Validate number (1-based index)
                if (selection > 0 && selection <= resultList.Count)
                {
                    int selectedProviderId = resultList[selection - 1]; // Convert 1-based to 0-based

                    // Construct the link to your Angular App
                    string link = $"http://localhost:4200/view/{selectedProviderId}";

                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"✅ اضغط الرابط التالي لعرض التفاصيل والحجز والتواصل:\n\n🔗 {link}",
                        cancellationToken: cancellationToken
                    );
                }
                else
                {
                    await botClient.SendMessage(chatId, "❌ رقم غير صحيح، الرجاء اختيار رقم من القائمة.", cancellationToken: cancellationToken);
                }
                return;
            }
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
                        text: "No providers  found / لم يتم العثور على اخصائيين",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                // Save IDs to Cache
                var providerIds = providers.Select(p => p.Id).ToList();
                _userSearches[chatId] = providerIds;

                // Build the Single Message List
                var sb = new StringBuilder();
                sb.AppendLine($"🔍 تم ايجاد {providers.Count} اخصيائيين '{query}':");
                sb.AppendLine("________________________");

                int index = 1;
                foreach (var p in providers)
                {
                    sb.AppendLine($"{index}. 👨‍⚕️ {p.Name} - {p.Specialty}");
                    index++;
                }

                sb.AppendLine("________________________");
                sb.AppendLine("👇 *ارسل رقم الاخصائي الذي تريد عرض تفاصيله.*");
                sb.AppendLine("(مثلاً: ارسل '1')");

                await botClient.SendMessage(
                    chatId: chatId,
                    text: sb.ToString(),
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken
                );
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