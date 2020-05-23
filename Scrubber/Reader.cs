using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Polly;

namespace Scrubber
{
    public static class Reader
    {
        public static Task<HttpResponseMessage> GetPage(Uri uri)
        {
            var client = new HttpClient(new HttpRetryMessageHandler(new HttpClientHandler()));

            return client.GetAsync(uri);
        }

        public static string ExtractCss(string html, string basePath)
        {
            var links = GetStyleSheetLinks(html);
            var client = new HttpClient(new HttpRetryMessageHandler(new HttpClientHandler()));
            var responses = links.Select(link => client.GetAsync(new Uri($@"{basePath}/{link}"))).ToList();

            Task.WaitAll(responses.Cast<Task>().ToArray());

            var files = responses.Select(r => r.Result);
            var contentTasks = files.Select(f => f.Content.ReadAsStringAsync()).ToList();

            Task.WaitAll(contentTasks.Cast<Task>().ToArray());

            var css = string.Join(Environment.NewLine, contentTasks.Select(c => c.Result));

            return css;
        }

        private static IEnumerable<string> GetStyleSheetLinks(string html)
        {
            var document = new HtmlDocument();

            document.LoadHtml(html);

            return document.DocumentNode
                .SelectNodes(".//link[@rel='stylesheet']")
                .Select(n => n.GetAttributeValue("href", string.Empty))
                .Where(s => !string.IsNullOrWhiteSpace(s));
        }

        public static async Task<int> GetPageCount(Uri uri)
        {
            var html = await GetPage(uri).Result.Content.ReadAsStringAsync();

            var document = new HtmlDocument();

            document.LoadHtml(html);

            var pageNavHeaderText = document.DocumentNode.SelectNodes(".//span[contains(@class, 'pageNavHeader')]")[0].InnerHtml;

            return int.Parse(pageNavHeaderText.Split(' ')[3]);
        }
    }

    public class HttpRetryMessageHandler : DelegatingHandler
    {
        public HttpRetryMessageHandler(HttpClientHandler handler) : base(handler) { }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt)))
                .ExecuteAsync(() => base.SendAsync(request, cancellationToken));
    }
}
