using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicStore.Components
{
    [ViewComponent(Name = "Recommandation")]
    public class RecommandationComponent : ViewComponent
    {
        public RecommandationComponent(MusicStoreContext dbContext)
        {
            DbContext = dbContext;
        }

        private MusicStoreContext DbContext { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recommandations = await DbContext.Albums.Take(3).ToListAsync();

            return View(recommandations);
        }

    }
}