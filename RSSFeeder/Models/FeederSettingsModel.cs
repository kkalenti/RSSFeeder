using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RSSFeeder.Models
{
    /// <summary>
    /// Класс для хранения настроек приложения
    /// </summary>
    public class FeederSettingsModel
    {
        /// <summary>
        /// Настройки для каждой ленты
        /// </summary>
        public List<SingleFeedSettingsModel> FeedSettings { get; set; }

        /// <summary>
        /// Возвращает true, если описание статьи выводится
        /// в форматированном формате
        /// </summary>
        [DisplayName("Форматировать описание")]
        public bool IsDescriptionFormatted { get; set; }

        /// <summary>
        /// Время обновления фидов
        /// </summary>
        [DisplayName("Время обновления ленты (в минутах)")]
        public int UpdateTime { get; set; }
    }
}