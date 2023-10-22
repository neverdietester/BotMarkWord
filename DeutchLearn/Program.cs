using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace DeutchLearn
{
    class Program
    {
        private static RememberArticle _article;
        private static RememberWord _word;

        public static void Main(string[] args)
        {
            var client = new TelegramBotClient("6665919797:AAEnyMmw4orBJy1IAfAHLaJp-5ypXY_Ux6E");
            client.StartReceiving(HandleUpdateAsync, HandleErrorAsync);
            Console.ReadKey();
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            Console.WriteLine(exception);
            return Task.CompletedTask;
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        await BotOnMessageReceived(botClient, update.Message);
                        break;
                    case UpdateType.CallbackQuery:
                        if (_word != null)
                        {
                            await _word.OnAnswer(update.CallbackQuery);
                        }
                        if (_article != null)
                        {
                            await _article.OnAnswer(update.CallbackQuery);
                        }
                        else return;
                            break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message?.Text != null)
            {
                Console.WriteLine($"{message.Chat.Id} | {message.Text}");
                if (message?.Text == null) return;
                var keyboard = new ReplyKeyboardMarkup(new[]
                {
                            new[]
                            {
                                new KeyboardButton("Учить слова‍🎓"),
                                new KeyboardButton("Учить артикли🎓")
                            },
                             new[]
                            {
                                new KeyboardButton("Помощь❔"),
                                new KeyboardButton("Немецкое радио📢"),
                            }
                        });

                switch (message.Text)
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                                    "Привет! Я был создан чтобы помогать запоминать тебе немецкие слова ⭐ Нажми Помощь❔ если не знаешь, что делать 😉", replyMarkup: keyboard);
                        break;
                    case "Помощь❔":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Бот для изучения немецких слов!\n Чтобы изучить новые слова нажми Учить слова‍🎓\n Чтобы изучить артикли нажми Учить артикли‍🎯");
                        break;
                    case "Учить артикли🎓":
                        _article = new RememberArticle(botClient, message.Chat);
                        await _article.StartAsync();
                        break;
                    case "Учить слова‍🎓":
                        _word = new RememberWord(botClient, message.Chat);
                        await _word.StartAsync();
                        break;
                    case "Немецкое радио📢":
                        var radioKeyboard = new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Немецкое радио📢", "https://www.de-online.ru/nemeckoe_radio_online")
                            }
                        });
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Слушай немецкое радио и повышай свой скилл 💪", replyMarkup: radioKeyboard);
                        break;
                    default:
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Я не знаю данную команду. Выбери один из пунктов меню, если не знаешь, что делать жми Помощь❔");
                        break;
                }
            }
        }
    }
}
