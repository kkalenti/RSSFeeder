using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Extensions.Options;
using RSSFeeder.Data.Interfaces;
using RSSFeeder.Models;

namespace RSSFeeder.Data.Repositories
{
    /// <summary>
    /// Класс-репозиторий, для реализации возможности работы с лентами
    /// </summary>
    public class RSSFeedRepository : IRSSFeed
    {
        /// <summary>
        /// Настройки приложения
        /// </summary>
        private IFeederSettings _feederSettings { get;}

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="feederSettings"></param>
        public RSSFeedRepository(IFeederSettings feederSettings)
        {
            _feederSettings = feederSettings;
        }

        /// <summary>
        /// Возвращает все ленты
        /// </summary>
        public IEnumerable<MainRSSFeedModel> GetAllFeeds()
        {
            var setting = _feederSettings.GetAllSettings();

            return setting.FeedSettings.Select(feed => LoadRssFeed(feed)).ToList();
        }

        /// <summary>
        /// Получение ленты по ID
        /// </summary>
        /// <param name="id">Идентификатор требуемой ленты</param>
        /// <returns>Запрашиваемая лента</returns>
        public MainRSSFeedModel GetFeedById(int id)
        {
            var setting = _feederSettings.GetFeedSettingsById(id);

            return LoadRssFeed(setting);
        }

        /// <summary>
        /// Получение всех названий для лент с их ID
        /// </summary>
        /// <returns>Список названий лент</returns>
        public IEnumerable<MainRSSFeedModel> GetAllFeedNames()
        {
            return _feederSettings.GetAllSettings().FeedSettings
                .Select(x => new MainRSSFeedModel()
                {
                    Id = x.Id,
                    FeedName = x.Name
                });
        }

        /// <summary>
        /// Загрузка лент по RSS адресу
        /// </summary>
        /// <param name="feed">Настройки ленты</param>
        /// <returns></returns>
        private static MainRSSFeedModel LoadRssFeed(SingleFeedSettingsModel feed)
        {
            if (feed == null) return null;
            var reader = XmlReader.Create(feed.RSSUrl);

            var formatter = new Rss20FeedFormatter();
            formatter.ReadFrom(reader);
            reader.Close();

            return new MainRSSFeedModel()
            {
                Id = feed.Id,
                FeedName = feed.Name,
                FeedItems = formatter.Feed.Items.Select(x => new RSSFeedItemModel()
                {
                    Title = x.Title.Text,
                    Link = x.Links.FirstOrDefault().Uri.ToString(),
                    PublishDate = x.PublishDate.DateTime,
                    Description = x.Summary.Text
                }).ToList()
            };
        }
    }
}