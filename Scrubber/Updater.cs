using System;
using System.Linq;
using HtmlAgilityPack;

namespace Scrubber
{
    public static class Updater
    {
        public static string UpdateCssLinks(string html, string css)
        {
            var document = new HtmlDocument();

            document.LoadHtml(html);

            document.DocumentNode
                .SelectNodes(".//link[@rel='stylesheet']")
                .ToList()
                .ForEach(n => { n.Remove(); });

            document.DocumentNode.SelectSingleNode(".//head")
                .AppendChild(HtmlNode.CreateNode($"<link ref=\"stylesheet\" href=\"main.css\" type=\"text/css\" >{Environment.NewLine}"));

            //            document.DocumentNode.SelectSingleNode(".//head")
            //                .AppendChild(HtmlNode.CreateNode($"<style>{Environment.NewLine}{css}{Environment.NewLine}</style>{Environment.NewLine}"));

            return document.DocumentNode.OuterHtml;
        }

        public static string UpdateThreadLinks(string html, string threadName)
        {
            var document = new HtmlDocument();
            var baseHref = $"threads/{threadName}/";

            document.LoadHtml(html);

            document.DocumentNode
                .SelectNodes("(.//span[contains(@class, 'pageNavHeader')])[1]/following-sibling::nav/descendant::a")
                .ToList()
                .ForEach(n =>
                {
                    var href = n.GetAttributeValue("href", string.Empty);

                    if (!string.IsNullOrWhiteSpace(href))
                    {
                        if (href.Length == baseHref.Length)
                        {
                            n.SetAttributeValue("href", "page-1.html");
                        }
                        else
                        {
                            n.SetAttributeValue("href", href.Remove(0, baseHref.Length) + ".html");
                        }
                    }
                });

            document.DocumentNode
                .SelectNodes("(.//span[contains(@class, 'pageNavHeader')])[2]/following-sibling::nav/descendant::a")
                .ToList()
                .ForEach(n =>
                {
                    var href = n.GetAttributeValue("href", string.Empty);

                    if (!string.IsNullOrWhiteSpace(href))
                    {
                        if (href.Length == baseHref.Length)
                        {
                            n.SetAttributeValue("href", "page-1.html");
                        }
                        else
                        {
                            n.SetAttributeValue("href", href.Remove(0, baseHref.Length) + ".html");
                        }
                    }
                });

            return document.DocumentNode.OuterHtml;
        }
    }
}
