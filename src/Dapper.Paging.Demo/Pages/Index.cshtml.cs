using Dapper.Paging.Demo.Models;
using Dapper.Paging.Demo.Services;
using Dapper.Razor.Demo.Services.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Dapper.Paging.Demo.Pages
{
    /// <summary>
    /// Index Page Model
    /// </summary>
    public class IndexModel : PageModel
    {

        readonly IPersonRepository _personRepository;

        public IndexModel(IPersonRepository personRepository)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        }

        [BindProperty]
        public PagedResults<Person> Persons { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        /// <summary>
        /// Initializes any state needed for the page, in our case Persons List
        /// </summary>
        public async Task OnGetAsync(int pageNumber = 1)
        {
            Persons = await _personRepository.GetAsync(SearchString, pageNumber).ConfigureAwait(false);
        }
    }
}