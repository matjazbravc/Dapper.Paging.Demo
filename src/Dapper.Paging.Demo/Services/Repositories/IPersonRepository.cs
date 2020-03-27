using Dapper.Paging.Demo.Models;
using Dapper.Paging.Demo.Services;
using System.Threading.Tasks;

namespace Dapper.Razor.Demo.Services.Repositories
{
    public interface IPersonRepository
    {
        Task<PagedResults<Person>> GetAsync(string searchString = "", int page = 1, int pageSize = 10);
    }
}
