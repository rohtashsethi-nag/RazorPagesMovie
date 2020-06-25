using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;
using System.Linq;

namespace RazorPagesMovie.Pages_Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }
        public SelectList Genres { get; set; }

        public async Task OnGetAsync()
        {
            var movies = from m in _context.Movie 
                        select m;
            IQueryable<string> genreQuery = from m in _context.Movie orderby m.Genre select m.Genre;

            if (!string.IsNullOrEmpty(SearchString)) {
                movies = movies.Where(m => m.Title.ToLower().Contains(SearchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(MovieGenre)) {
                movies = movies.Where(m => m.Genre == MovieGenre);
            }

            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
