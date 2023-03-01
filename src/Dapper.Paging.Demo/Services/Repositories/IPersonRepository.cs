using Dapper.Paging.Demo.Models;
using System.Threading.Tasks;

namespace Dapper.Paging.Demo.Services.Repositories
{
    public interface IPersonRepository
    {
        Task<PagedResults<Person>> GetAsync(string searchString = "", int page = 1, int pageSize = 10);
    }
}
