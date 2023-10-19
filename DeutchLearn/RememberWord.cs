using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;



namespace DeutchLearn
{
    public class RememberWord
    {
        private ITelegramBotClient _botClient;
        private Chat _chat;

        public RememberWord(ITelegramBotClient botClient, Chat chat)
        {
            _botClient = botClient;
            _chat = chat;
        }

        public bool IsFinished { get; private set; }

        public async Task OnAnswer(Message message)
        {
            switch (message.Text)
            {
                case "run":
                    break;

                case "stop":
                    IsFinished = true;
                    break;
            }
        }

        public async Task StartAsync()
        {
            InlineKeyboardMarkup inlineKeyboard = new(
                new[] 
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Поехали😎", "run")
                    }
                }
                );

            await _botClient.SendTextMessageAsync(_chat.Id,
                    $"Жми⬇",
                    replyMarkup: inlineKeyboard);
        }

        public void Stop()
        {
            IsFinished = true;
        }

        internal async Task OnAnswer(CallbackQuery callbackQuery)
        {
            switch (callbackQuery.Data)
            {
                case "run":
                    InlineKeyboardMarkup inlineKeyboard = new(
                        new[] {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Учить новое", "learnNew"),
                                InlineKeyboardButton.WithCallbackData("Повторить изученое", "repeatOld")
                            }
                        }
                    );

                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "");
                    await _botClient.SendTextMessageAsync(_chat.Id,
                            $"Выберите вариант",
                            replyMarkup: inlineKeyboard);
                    break;
                case "learnNew":
                    int randomid = LearnWord.GetRandomId();
                    IEnumerable<FirstLevel> filteredWord = LearnWord.GetWordById(randomid);
                    foreach (FirstLevel firstLevel in filteredWord)
                    {
                        Console.WriteLine($"Name: {firstLevel.Germany} {firstLevel.Russian}");
                        await _botClient.SendTextMessageAsync(_chat.Id, $"✅Запомни слово: {firstLevel.Germany}");
                        await _botClient.SendTextMessageAsync(_chat.Id, $"✅Перевод: {firstLevel.Russian}");

                        string word = firstLevel.Germany;
                        if (!LearnWord.WordExistsInRepeat(word))
                        {
                            RepeatWord newRepeat = new RepeatWord
                            {
                                id = (LearnWord.GetMaxId() + 1),
                                chatid = (int)_chat.Id,
                                wordde = firstLevel.Germany,
                                wordru = firstLevel.Russian,
                                worddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            };
                            LearnWord.Insert(newRepeat);
                        }
                    }
                    break;
                case "repeatOld":
                    var repeatWord = LearnWord.GetOldDateWord((int)_chat.Id);
                    await _botClient.SendTextMessageAsync(_chat.Id, $"❕Повторите:{repeatWord.wordde}");
                    await _botClient.SendTextMessageAsync(_chat.Id, $"❕Перевод: {repeatWord.wordru}");
                    LearnWord.UpdateDateWord((int)_chat.Id);
                    break;
            }
        }
    }
}
