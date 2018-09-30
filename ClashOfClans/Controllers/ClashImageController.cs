using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ClashOfClans.Controllers
{
    public class ClashImageController : Controller
    {
        Dictionary<string, byte[]> cache = new Dictionary<string, byte[]>();

        [HttpGet("/images/coc/{*path}")]
        public async Task<IActionResult> Index(string path)
        {
            byte[] val;
            if (!cache.TryGetValue(path, out val))
            {
                using (var client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync($"https://api-assets.clashofclans.com/{path}");
                    cache[path] = bytes;
                    val = bytes;
                }
            }

            return File(val, "image/png");
        }
    }
}