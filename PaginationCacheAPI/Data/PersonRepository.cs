using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaginationCacheAPI
{
    public class PersonRepository
    {
        AppDbContext Context;
        public PersonRepository(AppDbContext Context)
        {
            this.Context = Context;
        }

        public async Task<IQueryable<Person>> GetQueryAble()
        {
            return Context.Person.AsQueryable();
        }


        public async Task<(IEnumerable<Person> records, int total)> GetPaginationAsync(QueryResource pagination, Person filterResource)
        {
            var queryable = ConditionFilter(filterResource);

            var total = await queryable.CountAsync();

            var records = await queryable.AsNoTracking()
                .AsSplitQuery()
                .OrderBy(x => x.Id)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (records, total);
        }


        private IQueryable<Person> ConditionFilter(Person filterResource)
        {
            var queryable = Context.Person.AsQueryable();
            //if (!string.IsNullOrWhiteSpace(filterResource.StaffId))
            //{
            //    queryable.Where(x => x.StaffId.Equals(filterResource.StaffId));
            //} 

            if (filterResource != null)
            {
                if (!string.IsNullOrEmpty(filterResource.FirstName))
                    queryable = queryable.Where(x => x.FirstName.Contains(filterResource.FirstName.RemoveSpaceCharacter()));

                //if (!string.IsNullOrEmpty(filterResource.FirstName))
                //{
                //    string fullName = filterResource.FirstName.RemoveSpaceCharacter().ToLower();
                //    queryable = queryable.Where(x => x.FirstName.Contains(fullName));
                //}

                //if (!string.IsNullOrEmpty(filterResource.LastName))
                //{
                //    string fullName = filterResource.LastName.RemoveSpaceCharacter().ToLower();
                //    queryable = queryable.Where(x => x.LastName.Contains(fullName));
                //}
            }

            return queryable;
        }

    }
}
