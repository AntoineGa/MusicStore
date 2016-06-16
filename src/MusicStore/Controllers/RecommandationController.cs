using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class RecommandationController : Controller
    {
        public RecommandationController(MusicStoreContext dbContext)
        {
            DbContext = dbContext;
        }

        public MusicStoreContext DbContext { get; }

        //
        // GET: /Store/
        public async Task<IActionResult> Index()
        {
            var albums = await DbContext.Albums.ToListAsync();

            return View(albums);
        }
        
    }
}