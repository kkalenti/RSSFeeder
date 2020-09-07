using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RSSFeeder.ViewModels
{
    public class ApplicationSettingViewModel
    {
        /// <summary>
        /// Возвращает true, если описание статьи выводится
        /// в форматированном формате
        /// </summary>
        [DisplayName("Форматировать описание статьи в HTML")]
        public bool IsDescriptionFormatted { get; set; }

        /// <summary>
        /// Время обновления фидов
        /// </summary>
        [DisplayName("Время обновления ленты")]
        [Required(ErrorMessage = "Введите время обновления ленты")]
        [Range(1,120, ErrorMessage = "Время обновления не может быть меньше 1 минуты и больше 120 минут")]
        public int UpdateTime { get; set; }
    }
}