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
        public static void Main(string[] args)
        {
            var client = new TelegramBotClient("6665919797:AAEnyMmw4orBJy1IAfAHLaJp-5ypXY_Ux6E");
            client.StartReceiving(Update, Error);
            Console.ReadKey();
        }

        private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message?.Text != null)
            {
                Console.WriteLine($"{message.Chat.Id} | {message.Text}");
                if (message?.Text == null) return;
                var keyboard1 = new ReplyKeyboardMarkup(new[]
                {
                new[]
                {
                    new KeyboardButton("Вернуться в меню 🔙")
                }
            });
                var keyboard = new ReplyKeyboardMarkup(new[]
                {
                new[]
                {
                    new KeyboardButton("Добавить слово✏"),
                    new KeyboardButton("Учить слова‍🎓"),
                    new KeyboardButton("Повторить слова🎯‍")

                },
                 new[]
                {
                    new KeyboardButton("Помощь❔"),
                    new KeyboardButton("Немецкое радио📢"),
                },
            });
                switch (message.Text)
                {
                    case "/start":
                    case "Вернуться в меню 🔙":
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                                    "Привет! Я был создан чтобы помогать запоминать тебе новые слова ⭐ Нажми Помощь❔ если не знаешь, что делать 😉", replyMarkup: keyboard);
                        break;
                    case "Помощь❔":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Бот для изучения немецких слов!\n Чтобы изучить новое нажми Учить слова‍🎓\n Чтобы повторить изученное нажми Повторить слова🎯\n Чтобы добавить свое слово в словарь нажми Добавить слово✏");
                        break;
                    case "Добавить слово✏":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Напиши слово на немецком языке");
                        break;
                    case "Учить слова‍🎓":
                        int randomid = LearnWord.GetRandomId();
                        IEnumerable<FirstLevel> filteredWord = LearnWord.GetWordById(randomid);
                        foreach (FirstLevel firstLevel in filteredWord)
                        {
                            Console.WriteLine($"Name: {firstLevel.Germany} {firstLevel.Russian}");
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Запомни слово и его перевод");
                            await botClient.SendTextMessageAsync(message.Chat.Id, firstLevel.Germany);
                            await botClient.SendTextMessageAsync(message.Chat.Id, $"Перевод: {firstLevel.Russian}");

                            string word = firstLevel.Germany;
                            if (!LearnWord.WordExistsInRepeat(word/*, (int)message.Chat.Id)*/))
                            {
                                RepeatWord newRepeat = new RepeatWord
                                {
                                    id = (LearnWord.GetMaxId() + 1),
                                    chatid = (int)message.Chat.Id,
                                    wordde = firstLevel.Germany,
                                    wordru = firstLevel.Russian,
                                    worddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                };
                                LearnWord.Insert(newRepeat);
                            }
                        }
                        break;
                    case "Повторить слова🎯‍":
                        var repeatWord = LearnWord.GetOldDateWord((int)message.Chat.Id);
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Повторите:");
                        await botClient.SendTextMessageAsync(message.Chat.Id, repeatWord.wordde);
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Перевод: {repeatWord.wordru}");
                        LearnWord.UpdateDateWord((int)message.Chat.Id);
                        break;
                    case "Немецкое радио📢":
                        var inlineKeyboard = new InlineKeyboardMarkup(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithUrl("Немецкое радио📢", "https://www.de-online.ru/nemeckoe_radio_online")
        }
    });
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Слушай немецкое радио и повышай свой скилл 💪", replyMarkup: inlineKeyboard);
                        break;
                }
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
