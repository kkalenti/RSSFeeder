using System.Collections.Generic;
using RSSFeeder.Models;

namespace RSSFeeder.ViewModels
{
    public class IndexViewModel
    {
        /// <summary>
        /// Список названий лент
        /// </summary>
        public IEnumerable<MainRSSFeedModel> FeedNameList { get; set; }

        /// <summary>
        /// Фид для отображения
        /// </summary>
        public MainRSSFeedModel MainRssFeed { get; set; }
    }
}