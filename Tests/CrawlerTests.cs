using System.Threading.Tasks;
using Scrubber;
using Xunit;

namespace Tests
{
    public class CrawlerTests
    {
        [Fact]
        public async Task Run()
        {
            var thread = "http://fuckcombustion.com/threads/volcano-hybrid.36986/";

            await Crawler.Start(thread);

            Assert.True(true);
        }
    }
}
