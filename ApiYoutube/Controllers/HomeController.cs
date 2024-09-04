using ApiYoutube.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApiYoutube.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var videos = await BuscarVideosCanal();

            return View(videos);
        }

        private async Task<List<VideoDetalhes>> BuscarVideosCanal()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyCEv1bJ7DzoQJx2Ou4m0SuvNkD4u_9pph0",
                ApplicationName = "ApiVideoYoutube",


            });

            var serachRequest = youtubeService.Search.List("snippet");
            serachRequest.ChannelId = "UCxDYiN3jS_3EuPSjhj1utbg";
            serachRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            serachRequest.MaxResults = 20;

            var searchResponse = await serachRequest.ExecuteAsync();

            List<VideoDetalhes> videoList = searchResponse.Items.Select(item =>
                                new VideoDetalhes
                                {
                                    Title = item.Snippet.Title,
                                    Description = item.Snippet.Description,
                                    ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url,
                                    Link = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                                    PublishedAt = item.Snippet.PublishedAt
                                }

            ).OrderByDescending(video => video.PublishedAt)
             .ToList();

            return videoList;
        }
    }
}
