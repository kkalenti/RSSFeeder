using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using RSSFeeder.Data.Interfaces;
using RSSFeeder.Models;
using RSSFeeder.ViewModels;

namespace RSSFeeder.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRSSFeed _rssFeed;

        private readonly IFeederSettings _feederSettings;

        /// <summary>
        /// Bootstrap класс для изображений, добавляется для форматирования
        /// описания статей в ленте
        /// </summary>
        private const string _textToAdd = " class=\"img-fluid\"";

        public HomeController(IRSSFeed rssFeed, IFeederSettings feederSettings)
        {
            _rssFeed = rssFeed;
            _feederSettings = feederSettings;
        }

        /// <summary>
        /// Контроллер основной формы, на которой выводится RSS лента
        /// </summary>
        /// <param name="id">Идентификатор ленты, которую следует выводить</param>
        /// <returns></returns>
        public IActionResult Index(int id)
        {
            var feedNameList = _rssFeed.GetAllFeedNames();

            var viewModel = new IndexViewModel()
            {
                MainRssFeed = FormFeedView(id),
                FeedNameList = feedNameList
            };

            var allSettings = _feederSettings.GetAllSettings();
            ViewBag.IsFormatted = allSettings.IsDescriptionFormatted;
            ViewBag.UpdateTime = TimeSpan.FromMinutes(allSettings.UpdateTime).TotalMilliseconds;

            return View(viewModel);
        }

        /// <summary>
        /// Контроллер для обновления ленты
        /// </summary>
        /// <param name="id">Идентификатор ленты, которую следует выводить</param>
        /// <returns></returns>
        public IActionResult FeedItemList(int id)
        {
            ViewBag.IsFormatted = _feederSettings.GetAllSettings().IsDescriptionFormatted;
            return PartialView("Home/_FeedContent", FormFeedView(id));
        }

        /// <summary>
        /// Контроллер настроек лент(название, ссылка на ленту)
        /// </summary>
        /// <returns></returns>
        public IActionResult FeedSettings()
        {
            var setting = _feederSettings.GetAllSettings();

            return View(setting.FeedSettings);
        }

        /// <summary>
        /// Контроллер настроек приложения
        /// </summary>
        /// <returns></returns>
        public IActionResult ApplicationSettings()
        {
            var setting = _feederSettings.GetAllSettings();
            var viewMode = new ApplicationSettingViewModel()
            {
                UpdateTime = setting.UpdateTime,
                IsDescriptionFormatted = setting.IsDescriptionFormatted
            };

            return View(viewMode);
        }

        /// <summary>
        /// POST метод, который получает данные об изменении настроек приложения
        /// и сохраняет их
        /// </summary>
        /// <param name="updatedSettings">Новые параметры приложения</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ApplicationSettings(ApplicationSettingViewModel updatedSettings)
        {
            _feederSettings.UpdateApplicationSettings(new FeederSettingsModel()
            {
                IsDescriptionFormatted = updatedSettings.IsDescriptionFormatted,
                UpdateTime = updatedSettings.UpdateTime
            });

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Еонтроллер, для вывода формы для добавления новой ленты
        /// </summary>
        /// <returns></returns>
        public IActionResult AddNewFeed()
        {
            return View();
        }

        /// <summary>
        /// POST метод, который получает данные о добавляемой ленте
        /// </summary>
        /// <param name="newSetting">Настройки для добавляемой ленты</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddNewFeed(SingleFeedSettingsModel newSetting)
        {
            //Проверка на подключение к введенному пользователем адресу
            try
            {
                var reader = XmlReader.Create(newSetting.RSSUrl);
                //Проверка на возможность работы с полученной xml разметкой
                try
                {
                    var formatter = new Rss20FeedFormatter();
                    formatter.ReadFrom(reader);
                    reader.Close();
                }
                catch (XmlException)
                {
                    reader.Close();
                    ModelState.TryAddModelError("", "Введенный RSS не доступен");
                    return View();
                }

                _feederSettings.AddFeedSettings(newSetting);

                return RedirectToAction("FeedSettings");
            }
            catch (Exception)
            {
                ModelState.TryAddModelError("", "Введенный RSS адрес не корректен");
                return View();
            }
        }

        /// <summary>
        /// Контроллер для удаления выбранной ленты
        /// </summary>
        /// <param name="id">Идентификатор ленты</param>
        /// <returns></returns>
        public IActionResult DeleteFeed(int id)
        {
            _feederSettings.DeleteFeedSettings(id);
            return RedirectToAction("FeedSettings");
        }

        /// <summary>
        /// Контроллер для вывода формы редактирования настроек ленты
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditFeed(int id)
        {
            var setting = _feederSettings.GetAllSettings();

            return View(setting.FeedSettings.FirstOrDefault(x => x.Id == id));
        }

        /// <summary>
        /// POST метод, который получает данные об изменениях в ленте
        /// </summary>
        /// <param name="updatedSetting">Настройки для изменяемой ленты</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditFeed(SingleFeedSettingsModel updatedSetting)
        {
            //Проверка на подключение к введенному пользователем адресу
            try
            {
                var reader = XmlReader.Create(updatedSetting.RSSUrl);
                //Проверка на возможность работы с полученной xml разметкой
                try
                {
                    var formatter = new Rss20FeedFormatter();
                    formatter.ReadFrom(reader);
                    reader.Close();
                }
                catch (XmlException)
                {
                    reader.Close();
                    ModelState.TryAddModelError("", "Введенный RSS не доступен");
                    return View();
                }

                _feederSettings.UpdateFeedSettings(updatedSetting);
                return RedirectToAction("FeedSettings");
            }
            catch (Exception)
            {
                ModelState.TryAddModelError("", "Введенный RSS адрес не корректен");
                return View();
            }
        }

        /// <summary>
        /// Метод для формирования ленты для вывода
        /// </summary>
        /// <param name="feedId">Идентификатор ленты</param>
        /// <returns></returns>
        private MainRSSFeedModel FormFeedView(int feedId)
        {
            var rssToView = feedId < 1 ? _rssFeed.GetAllFeeds().FirstOrDefault() : _rssFeed.GetFeedById(feedId);

            if (rssToView != null)
            {
                foreach (var item in rssToView.FeedItems)
                {
                    var text = item.Description;
                    var r = new Regex(@"\w*<img\w*");
                    var m = r.Matches(text).ToList();


                    for (var i = 0; i < m.Count; i++)
                    {
                        item.Description = item.Description
                            .Insert(m[i].Index + m[i].Length + _textToAdd.Length * i, _textToAdd);
                    }
                }
            }

            return rssToView;
        }
    }
}
