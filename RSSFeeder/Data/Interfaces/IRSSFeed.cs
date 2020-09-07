using System.Collections.Generic;
using RSSFeeder.Models;

namespace RSSFeeder.Data.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с лентами
    /// </summary>
    public interface IRSSFeed
    {
        /// <summary>
        /// Возвращает все ленты
        /// </summary>
        IEnumerable<MainRSSFeedModel> GetAllFeeds();

        /// <summary>
        /// Получение ленты по ID
        /// </summary>
        /// <param name="id">Идентификатор требуемой ленты</param>
        /// <returns>Запрашиваемая лента</returns>
        MainRSSFeedModel GetFeedById(int id);

        /// <summary>
        /// Получение всех названий для лент с их ID
        /// </summary>
        /// <returns>Список названий лент</returns>
        IEnumerable<MainRSSFeedModel> GetAllFeedNames();
    }
}