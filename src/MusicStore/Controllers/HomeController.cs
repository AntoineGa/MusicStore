using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MusicStore.Models;
using Microsoft.Extensions.Options;
using MusicStore.Extensions;


namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {

        private IOptions<AppSettings> AppSettings;

        public HomeController(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings; 
        }
        
        //
        // GET: /Home/
        public async Task<IActionResult> Index(
            [FromServices] MusicStoreContext dbContext,
            [FromServices] IMemoryCache cache)
        {
            // Get most popular albums
            var cacheKey = "topselling";
            List<Album> albums;
            if (!cache.TryGetValueExt(cacheKey, out albums, AppSettings.Value.CacheTimeout))
            {
                albums = await GetTopSellingAlbumsAsync(dbContext, 6);

                if (albums != null && albums.Count > 0)
                {
                    // Refresh it every 10 minutes.
                    // Let this be the last item to be removed by cache if cache GC kicks in.
                    cache.SetExt(
                        cacheKey,
                        albums,
                        new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(AppSettings.Value.CacheTimeout > 0 ? AppSettings.Value.CacheTimeout : 1))
                            .SetPriority(CacheItemPriority.High),
                        AppSettings.Value.CacheTimeout);
                }
            }

            return View(albums);
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        public IActionResult StatusCodePage()
        {
            return View("~/Views/Shared/StatusCodePage.cshtml");
        }

        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

        private async Task<List<Album>> GetTopSellingAlbumsAsync(MusicStoreContext dbContext, int count)
        {
            // Group the order details by album and return
            // the albums with the highest count

            return await dbContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count)
                .Take(count)
                .ToListAsync();
        }
    }
}