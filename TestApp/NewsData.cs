using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class NewsData
    {
        private static List<News> _newsList = new List<News>();
        private static List<News> _favoriteList = new List<News>();
        private static HashSet<int> _hiddenList = new HashSet<int>();

        public NewsData()
        {
    
        }

        public List<News> GetNews() 
        {
            return _newsList;
        }

        public List<News> GetFavoriteNews() 
        {
            return _favoriteList;
        }

        public async Task LoadDataAsync()
        {
            _newsList.Clear();
            await GetNewsAPI();
        }

        private async static Task GetNewsAPI()
        {
            try
            {
                var result = new List<News>();
                var uri = new Uri("http://frontappapi.dock7.66bit.ru/api/news/get?count=20&page=1");

                var webClient = new WebClient();
                var json = await webClient.DownloadStringTaskAsync(uri);

                var jArray = JArray.Parse(json);
                foreach (var item in jArray)
                {
                    var news = new News
                    {
                        Id = (int)item["id"],
                        Title = (string)item["title"],
                        Content = (string)item["content"]
                    };
                    if (!_hiddenList.Contains(news.Id))
                    {
                        _newsList.Add(news);
                    }                   
                }
                _newsList.Reverse();
            }
            catch
            {
                _newsList = new List<News>();
            }
        }

        public void AddFavorite(News news)
        {
            if (!_favoriteList.Contains(news))
                _favoriteList.Add(news);
        }

        public void HideNews(News news)
        {
            _hiddenList.Add(news.Id);
            for (var i = 0; i < _newsList.Count; i++)
            {
                if (_newsList[i].Id == news.Id)
                {
                    _newsList.RemoveAt(i);
                }
            }
            for (var i = 0; i < _favoriteList.Count; i++)
            {
                if (_favoriteList[i].Id == news.Id)
                {
                    _favoriteList.RemoveAt(i);
                }
            }
            
        }
    }
}
