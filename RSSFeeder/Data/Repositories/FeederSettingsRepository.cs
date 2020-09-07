using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RSSFeeder.Data.Interfaces;
using RSSFeeder.Models;

namespace RSSFeeder.Data.Repositories
{
    /// <summary>
    /// Класс-репозиторий, для реализации возможностей работы с настройками приложения
    /// </summary>
    public class FeederSettingsRepository : IFeederSettings
    {
        /// <summary>
        /// Настройки приложения
        /// </summary>
        private FeederSettingsModel _settings { get;}

        /// <summary>
        /// Счетчик для идентификаторв лент
        /// </summary>
        private int _lastFeedId = 0;

        /// <summary>
        /// Функция предполагает использования идентификатора для ленты,
        /// ведет счет для идентификаторов ленты
        /// </summary>
        /// <returns>Последний доступный идентификатор</returns>
        private int UseFeedId()
        {
            _lastFeedId += 1;
            return _lastFeedId;
        }

        /// <summary>
        /// Конструктор класса, подготавливает настройки программы,
        /// получая данные из конфиг файла
        /// </summary>
        /// <param name="feederConfig"></param>
        public FeederSettingsRepository(IConfiguration feederConfig)
        {
            var config = feederConfig;

            var rss = config.GetSection("FeederSettings").GetSection("RssFeed").Get<SingleFeedSettingsModel>();
            rss.Id = UseFeedId();

            var updateTime = config.GetSection("FeederSettings").GetSection("UpdateTime").Value;

            _settings = new FeederSettingsModel
            {
                IsDescriptionFormatted = true,
                UpdateTime = int.Parse(updateTime),
                FeedSettings = new List<SingleFeedSettingsModel>
                {
                    rss
                }
            };
        }

        /// <summary>
        /// Получить настройки приложения
        /// </summary>
        /// <returns>Возвращает переменную с настройками приложения</returns>
        public FeederSettingsModel GetAllSettings()
        {
            return _settings;
        }

        /// <summary>
        /// Получить настройки ленты по ID
        /// </summary>
        /// <param name="id">Идентификатор требуемой настройки</param>
        /// <returns>Возвращает переменную с настройками приложения</returns>
        public SingleFeedSettingsModel GetFeedSettingsById(int id)
        {
            return _settings.FeedSettings.FirstOrDefault(setting => setting.Id == id);
        }

        /// <summary>
        /// Обновить настройки приложения
        /// </summary>
        /// <param name="settings">Настройки для приложения</param>
        public void UpdateApplicationSettings(FeederSettingsModel settings)
        {
            _settings.IsDescriptionFormatted = settings.IsDescriptionFormatted;
            _settings.UpdateTime = settings.UpdateTime;
        }

        /// <summary>
        /// Обновить настройки ленты
        /// </summary>
        /// <param name="settings">Настройки для обновляемой ленты</param>
        public void UpdateFeedSettings(SingleFeedSettingsModel setting)
        {
            var updatedItem = _settings.FeedSettings
                .FirstOrDefault(x => x.Id == setting.Id);

            if (updatedItem == null) return;

            updatedItem.Name = setting.Name;
            updatedItem.RSSUrl = setting.RSSUrl;
        }

        /// <summary>
        /// Удаление данных о ленте
        /// </summary>
        /// <param name="id">Идентификатор удаляемой ленты</param>
        public void DeleteFeedSettings(int id)
        {
            var deletedItem = _settings.FeedSettings
                .FirstOrDefault(x => x.Id == id);

            if (deletedItem == null) return;

            _settings.FeedSettings.Remove(deletedItem);
        }

        /// <summary>
        /// Добавление настроек для новой ленты
        /// </summary>
        /// <param name="settings">Настройки для добавляемой ленты</param>
        public void AddFeedSettings(SingleFeedSettingsModel settings)
        {
            _settings.FeedSettings.Add(new SingleFeedSettingsModel()
            {
                Id = UseFeedId(),
                Name = settings.Name,
                RSSUrl = settings.RSSUrl
            });
        }
    }
}