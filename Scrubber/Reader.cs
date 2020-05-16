using System.Net.Http;
using System.Threading.Tasks;

namespace Scrubber
{
    public class Reader : IReader
    {
        private readonly string _url;

        public Reader(string url)
        {
            _url = url;
        }

        public virtual async Task<string> Read()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
