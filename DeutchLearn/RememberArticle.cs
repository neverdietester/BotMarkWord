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
    public class RememberArticle
    {
        private ITelegramBotClient _botClient;
        private Chat _chat;
        int randomidart = LearnArticle.GetRandomId();

        public RememberArticle(ITelegramBotClient botClient, Chat chat)
        {
            _botClient = botClient;
            _chat = chat;
        }
        public bool IsFinished { get; private set; }

        public async Task OnAnswer(Message message)
        {
            switch (message.Text)
            {
                case "runLearn":
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
                        InlineKeyboardButton.WithCallbackData("Поехали😎", "runLearn")
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
            
            Console.WriteLine(randomidart);
            switch (callbackQuery.Data)
            {
                case "runLearn":
                    InlineKeyboardMarkup inlineKeyboard = new(
                        new[] {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Das", " das"),
                                InlineKeyboardButton.WithCallbackData("Die", " die"),
                                InlineKeyboardButton.WithCallbackData("Der", " der")

                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Перевод", "translate"),
                                InlineKeyboardButton.WithCallbackData("Ответ", "answer")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Finish", "stop")
                            }
                        }
                    );

                    await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "");
                    IEnumerable<ArticleBegin> articleFiltredGe = LearnArticle.GetWordByArticle(randomidart);
                    foreach (ArticleBegin articleBegin in articleFiltredGe)
                    {
                        await _botClient.SendTextMessageAsync(_chat.Id, articleBegin.Germany);
                    }
                        await _botClient.SendTextMessageAsync(_chat.Id,
                            $"Выберите вариант",
                            replyMarkup: inlineKeyboard);
                    break;

                case " das":
                case " die":
                case " der":
                    IEnumerable<ArticleBegin> articlefiltred = LearnArticle.GetWordByArticle(randomidart);
                    foreach (ArticleBegin articleBegin in articlefiltred)
                    {
                        if (callbackQuery.Data == articleBegin.Article)
                        { await _botClient.SendTextMessageAsync(_chat.Id, "Верно!"); }
                        else await _botClient.SendTextMessageAsync(_chat.Id, "Попробуй еще раз!");
                    }
                    break;
                case "translate":
                    IsFinished = true;
                    IEnumerable<ArticleBegin> articleFiltredRu = LearnArticle.GetWordByArticle(randomidart);
                    foreach (ArticleBegin articleBegin in articleFiltredRu)
                    {
                        await _botClient.SendTextMessageAsync(_chat.Id, articleBegin.Russian);
                    }
                    break;
                case "answer":
                    IsFinished = true;
                    IEnumerable<ArticleBegin> articleFiltred = LearnArticle.GetWordByArticle(randomidart);
                    foreach (ArticleBegin articleBegin in articleFiltred)
                    {
                        await _botClient.SendTextMessageAsync(_chat.Id, articleBegin.Article);
                    }
                    break;
            }
        }
    }
}
