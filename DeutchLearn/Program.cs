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

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message?.Text != null)
            {
                Console.WriteLine($"{message.Chat.Username} | {message.Text}");
                switch (message.Text)
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(message.Chat.Id,
                                    "Привет! Я был создан чтобы помогать запоминать тебе новые слова 👩‍🏫! Я буду подстраиваться под твой уровень знания немецкого и оптимально выбирать упражнения! Введите /help , если не знаешь что делать 😉");
                        //await SendMainMenu(message.Chat.Id);
                        break;
                    case "/help":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Бот для изучения немецких слов! \n Чтобы добавить свое слово в словарь введите /add_word \n Чтобы выучить новое введите /learn_word \n Чтобы повторить изученное введите /repeat_word");
                        break;
                    case "/add_word":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите слово на немецком языке");
                        break;
                    case "/learn_word":
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
                    case "/repeat_word":
                        var repeatWord = LearnWord.GetOldDateWord((int)message.Chat.Id);
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Повторите:");
                        await botClient.SendTextMessageAsync(message.Chat.Id, repeatWord.wordde);
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Перевод: {repeatWord.wordru}");
                        LearnWord.UpdateDateWord((int)message.Chat.Id);
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
