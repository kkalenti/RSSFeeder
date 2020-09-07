using System;
using System.ComponentModel;

namespace RSSFeeder.Models
{
    /// <summary>
    /// Модель для хранении информации об
    /// элементе RSS ленты
    /// </summary>
    public class RSSFeedItemModel
    {
        /// <summary>
        /// Название статьи
        /// </summary>
        [DisplayName("Название")]
        public string Title { get; set; }

        /// <summary>
        /// Ссылка на статью
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Краткое описание статьи
        /// </summary>
        [DisplayName("Описание статьи")]
        public string Description { get; set; }

        /// <summary>
        /// Дата публикации статьи
        /// </summary>
        [DisplayName("Дата публикации")]
        public DateTime PublishDate { get; set; }
    }
}