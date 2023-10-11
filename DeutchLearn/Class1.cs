//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DeutchLearn
//{
//    class Class1
//    {

//botClient = new TelegramBotClient("6665919797:AAEnyMmw4orBJy1IAfAHLaJp-5ypXY_Ux6E");
//botClient.OnMessage += Bot_OnMessage;
//botClient.StartReceiving();

//Console.WriteLine("Bot is running...");
//Console.ReadLine();

//botClient.StopReceiving();


//        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
//        {
//            if (e.Message == null || e.Message.Type != MessageType.Text)
//                return;

//            switch (e.Message.Text)
//            {
//                case "/start":
//                    await SendMainMenu(e.Message.Chat.Id);
//                    break;
//                case "Учить слова":
//                    await SendLearnWordsMenu(e.Message.Chat.Id);
//                    break;
//                case "Добавить слово":
//                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Введите слово на немецком: перевод на русском");
//                    break;
//                case "Учить новое":
//                    await SendRandomWord(e.Message.Chat.Id);
//                    break;
//                case "Повторить":
//                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Функционал 'Повторить' еще не реализован");
//                    break;
//                case "Напоминания":
//                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Функционал 'Напоминания' еще не реализован");
//                    break;
//                case "Переводчик":
//                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Функционал 'Переводчик' еще не реализован");
//                    break;
//                case "Помощь":
//                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Функционал 'Помощь' еще не реализован");
//                    break;
//                default:
//                    await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Неизвестная команда");
//                    break;
//            }
//        }

//        private static async Task SendMainMenu(long chatId)
//        {
//            var keyboardMarkup = new ReplyKeyboardMarkup
//            {
//                Keyboard = new[]
//                {
//                    new[]
//                    {
//                        new KeyboardButton("Учить слова"),
//                        new KeyboardButton("Напоминания")
//                    },
//                    new[]
//                    {
//                        new KeyboardButton("Переводчик"),
//                        new KeyboardButton("Помощь")
//                    }
//                }
//            };

//            await botClient.SendTextMessageAsync(chatId, "Главное меню", replyMarkup: keyboardMarkup);
//        }

//        private static async Task SendLearnWordsMenu(long chatId)
//        {
//            var keyboardMarkup = new InlineKeyboardMarkup(new[]
//            {
//                new[]
//                {
//                    InlineKeyboardButton.WithCallbackData("Добавить слово"),
//                    InlineKeyboardButton.WithCallbackData("Учить новое"),
//                    InlineKeyboardButton.WithCallbackData("Повторить")
//                }
//            });

//            await botClient.SendTextMessageAsync(chatId, "Меню 'Учить слова'", replyMarkup: keyboardMarkup);
//        }

//        private static async Task SendRandomWord(long chatId)
//        {
//            // Здесь можно использовать любой способ получения случайного слова из словаря
//            string randomWord = GetRandomWord();

//            await botClient.SendTextMessageAsync(chatId, $"Введите перевод слова '{randomWord}'");
//        }

//        private static string GetRandomWord()
//        {
//            // Здесь должен быть код для получения случайного слова из словаря
//            // Верните случайное слово на немецком языке
//        }
//    }
//}
//    }
//}


//using System;
//using System.Threading.Tasks;
//using Telegram.Bot;
//using Telegram.Bot.Types;

//class Program
//{
//    private const string connectionString = "Host=localhost;Username=myuser;Password=mypassword;Database=mydatabase";

//    static async Task Main(string[] args)
//    {
//        var botClient = new TelegramBotClient("YOUR_BOT_TOKEN");

//        botClient.OnMessage += Bot_OnMessage;
//        botClient.StartReceiving();

//        Console.WriteLine("Bot is listening. Press ENTER to exit.");
//        Console.ReadLine();

//        botClient.StopReceiving();
//    }

//    private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
//    {
//        var message = e.Message;

//        if (message == null || message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
//            return;

//        switch (message.Text)
//        {
//            case "/add_word":
//                await botClient.SendTextMessageAsync(message.Chat.Id, "Введите слово на немецком языке");
//                break;

//            default:
//                // Handle other cases or commands
//                break;
//        }
//    }

//    private static async Task ProcessAddWordStepAsync(long chatId, string userwordde)
//    {
//        // Assuming you have a valid NpgsqlConnection named "connection" established earlier

//        // Prompt the user to enter the translation
//        await botClient.SendTextMessageAsync(chatId, "Введите перевод слова");
//        var response = await botClient.ReceiveTextMessageAsync();

//        if (response != null)
//        {
//            string userru = response.Text;

//            // Save the word and its translation to the database
//            using (var cmd = new Npgsql.NpgsqlCommand("INSERT INTO words (word_de, word_ru) VALUES (@word_de, @word_ru)", connection))
//            {
//                cmd.Parameters.AddWithValue("word_de", userwordde);
//                cmd.Parameters.AddWithValue("word_ru", userru);
//                await cmd.ExecuteNonQueryAsync();
//            }

//            await botClient.SendTextMessageAsync(chatId, "Слово и его перевод успешно добавлены в базу данных.");
//        }
//        else
//        {
//            await botClient.SendTextMessageAsync(chatId, "Введен некорректный перевод. Попробуйте снова.");
//        }
//    }
//}