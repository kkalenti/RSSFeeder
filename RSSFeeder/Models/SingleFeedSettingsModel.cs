using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSSFeeder.Models
{
    /// <summary>
    /// Модель представляющаю настройки конкретной ленты
    /// </summary>
    public class SingleFeedSettingsModel
    {
        /// <summary>
        /// Идентификатор для настройки конкретной ленты
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название ленты
        /// </summary>
        [DisplayName("Название")]
        [Required(ErrorMessage = "Пожалуйста, введите Названи")]
        [MaxLength(30, ErrorMessage = "Максимальная длина названия - 30 символов")]
        public string Name { get; set; }

        /// <summary>
        /// RSS адрес ленты
        /// </summary>
        [DisplayName("RSS адресс")]
        [Required(ErrorMessage = "Пожалуйста, введите RSS адресс")]
        [DataType(DataType.Url, ErrorMessage = "Пожалуйста, введите верный RSS адресс")]
        public string RSSUrl { get; set; }
    }
}