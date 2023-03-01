using Dapper.Paging.Demo.Configuration;
using Dapper.Paging.Demo.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Dapper.Paging.Demo.Services.Repositories
{
    /// <summary>
    /// Persons repository
    /// </summary>
    public class PersonRepository : IPersonRepository
    {
        private readonly SqlServerOptions _sqlServerOptions;

        public PersonRepository(IOptions<SqlServerOptions> sqlServerOptions)
        {
            _sqlServerOptions = sqlServerOptions.Value;
        }

        public async Task<PagedResults<Person>> GetAsync(string searchString = "", int pageNumber = 1, int pageSize = 10)
        {
            using (var conn = new SqlConnection(_sqlServerOptions.SqlServerConnection))
            {
                await conn.OpenAsync();

                // Set first query
                var whereStatement = string.IsNullOrWhiteSpace(searchString) ? "" : $"WHERE [FirstName] LIKE '{searchString}'";
                var queries = @$"
                SELECT
                    [BusinessEntityID],
                    [PersonType],
                    [Title],
                    [FirstName],
                    [MiddleName],
                    [LastName],
                    [Suffix],
                    [ModifiedDate] FROM [Person].[Person] (NOLOCK)
                {whereStatement}
                ORDER BY [BusinessEntityID]
                OFFSET @PageSize * (@PageNumber - 1) ROWS
                FETCH NEXT @PageSize ROWS ONLY;";

                // Set second query, separated with semi-colon
                queries += "SELECT COUNT(*) AS TotalItems FROM [Person].[Person] (NOLOCK);";

                // Execute multiple queries with Dapper in just one step
                using var multi = await conn.QueryMultipleAsync(queries,
                    new
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    });

                // Fetch Items by OFFSET-FETCH clause
                var items = await multi.ReadAsync<Person>().ConfigureAwait(false);

                // Fetch Total items count
                var totalItems = await multi.ReadFirstAsync<int>().ConfigureAwait(false);

                // Create paged result
                var result = new PagedResults<Person>(totalItems, pageNumber, pageSize)
                {
                    Items = items
                };
                return result;
            }
        }
    }
}
