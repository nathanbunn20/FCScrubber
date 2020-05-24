using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scrubber
{
    public static class Crawler
    {
        public static async Task Start(string threadUrl)
        {
            var baseThreadUri = GetBaseThreadUri(threadUrl);

            var pageCount = await Reader.GetPageCount(baseThreadUri);

            var uris = new List<Uri>();

            for (var i = 0; i < pageCount; i++)
            {
                var uri = new Uri($@"{baseThreadUri}page-{i + 1}");

                uris.Add(uri);
            }

            var htmlTasks = new List<Task<HttpResponseMessage>>();

            foreach (var uri in uris)
            {
                var task = Reader.GetPage(uri);

                htmlTasks.Add(task);
            }

            Task.WaitAll(htmlTasks.Cast<Task>().ToArray());

            var htmlReaders = htmlTasks.Select(t => t.Result.Content.ReadAsStringAsync()).ToList();

            Task.WaitAll(htmlReaders.Cast<Task>().ToArray());

            var htmlResults = htmlReaders.Select(r => r.Result).ToList();
            var css = Reader.ExtractCss(htmlResults.First(), $"http://{baseThreadUri.Authority}");
            var threadName = GetThreadName(baseThreadUri.AbsoluteUri);

            var htmls = new List<string>();

            foreach (var htmlResult in htmlResults)
            {
                var scriptsRemoved = Deleter.RemoveAllUnwanted(htmlResult);
                var cssUpdated = Updater.UpdateCssLinks(scriptsRemoved, css);
                var linksUpdated = Updater.UpdateThreadLinks(cssUpdated, threadName);

                htmls.Add(linksUpdated);
            }

            SaveThread(threadName, htmls, css);
        }

        private static void SaveThread(string threadName, IReadOnlyList<string> htmls, string css)
        {
            var targetPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\FuckCombustionThreads\\{threadName}";

            Directory.CreateDirectory(targetPath);

            for (var i = 0; i < htmls.Count; i++)
            {
                File.WriteAllText(Path.Combine(targetPath, $"page-{i + 1}.html"), htmls[i]);
            }

            File.WriteAllText(Path.Combine(targetPath, "main.css"), css);
        }

        private static string GetThreadName(string threadUrl)
        {
            var chunks = threadUrl.Split('/');

            return chunks[chunks.Length - 2];
        }

        private static Uri GetBaseThreadUri(string threadUrl)
        {
            if (threadUrl.ToLower().Contains("page"))
            {
                var chunks = threadUrl.Split('/');

                foreach (var chunk in chunks)
                {
                    if (chunk.ToLower().Contains("page"))
                    {
                        return new Uri(threadUrl.Remove(threadUrl.ToLower().IndexOf("page", StringComparison.Ordinal) + 1, chunk.Length));
                    }
                }
            }

            return new Uri(threadUrl);
        }
    }
}
