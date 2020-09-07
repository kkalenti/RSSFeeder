using System.Collections.Generic;
using RSSFeeder.Models;

namespace RSSFeeder.Data.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с настройками приложения
    /// </summary>
    public interface IFeederSettings
    {   
        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        /// <returns>Возвращает переменную с настройками приложения</returns>
        FeederSettingsModel GetAllSettings();

        /// <summary>
        /// Получить настройки ленты по ID
        /// </summary>
        /// <param name="id">Идентификатор требуемой настройки</param>
        /// <returns>Возвращает переменную с настройками требуемой ленты</returns>
        SingleFeedSettingsModel GetFeedSettingsById(int id);

        /// <summary>
        /// Обновить настройки приложения
        /// </summary>
        /// <param name="settings">Настройки для приложения</param>
        void UpdateApplicationSettings(FeederSettingsModel settings);

        /// <summary>
        /// Обновить настройки ленты
        /// </summary>
        /// <param name="settings">Настройки для обновляемой ленты</param>
        void UpdateFeedSettings(SingleFeedSettingsModel settings);

        /// <summary>
        /// Удаление данных о ленте
        /// </summary>
        /// <param name="id">Идентификатор удаляемой ленты</param>
        void DeleteFeedSettings(int id);

        /// <summary>
        /// Добавление настроек для новой ленты
        /// </summary>
        /// <param name="settings">Настройки для добавляемой ленты</param>
        void AddFeedSettings(SingleFeedSettingsModel settings);
    }
}