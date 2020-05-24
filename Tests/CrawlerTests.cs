using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scrubber;
using Xunit;

namespace Tests
{
    public class CrawlerTests
    {
        [Fact]
        public void Run()
        {
            var threads = new List<string>
            {
                "http://fuckcombustion.com/threads/i-just-saw-the-moon.21835/",
                "http://fuckcombustion.com/threads/volcano-hybrid.36986/"
            };

            var tasks = new List<Task>();

            foreach (var thread in threads)
            {
                tasks.Add(Crawler.Start(thread));
            }

            Task.WaitAll(tasks.Cast<Task>().ToArray());

            Assert.True(true);
        }
    }
}
