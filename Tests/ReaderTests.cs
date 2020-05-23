using System;
using System.Threading.Tasks;
using Scrubber;
using Xunit;

namespace Tests
{
    public class ReaderTests
    {
        private readonly string _testSiteUrl = "http://fuckcombustion.com/threads/volcano-hybrid.36986/";

        [Fact]
        public void ReturnsString()
        {
            Assert.IsType<string>(Reader.GetPage(new Uri(_testSiteUrl)));
        }

        [Fact]
        public async Task ExtractCss()
        {
            var html = await Reader.GetPage(new Uri(_testSiteUrl)).Result.Content.ReadAsStringAsync();
            var baseUrl = _testSiteUrl.Substring(0, _testSiteUrl.IndexOf("/threads", StringComparison.Ordinal));

            var css = Reader.ExtractCss(html, baseUrl);

            Assert.IsType<string>(css);
        }
    }
}
