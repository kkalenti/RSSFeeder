using System.Collections.Generic;

namespace RSSFeeder.Models
{
    /// <summary>
    /// Модель, представляющая одну ленту
    /// </summary>
    public class MainRSSFeedModel
    {
        /// <summary>
        /// Идентификатор ленты
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Элементы ленты
        /// </summary>
        public List<RSSFeedItemModel> FeedItems { get; set; }

        /// <summary>
        /// Название ленты
        /// </summary>
        public string FeedName { get; set; }
    }
}