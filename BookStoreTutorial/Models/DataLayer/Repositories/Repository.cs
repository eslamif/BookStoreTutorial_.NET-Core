using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private BookstoreContext context;
        private DbSet<T> dbSet;
        private int? count;

        public Repository(BookstoreContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public int Count => count ?? dbSet.Count();

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public T Get(QueryOptions<T> options)
        {
            throw new NotImplementedException();
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public T Get(string id)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> List(QueryOptions<T> options)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        private IQueryable<T> BuildQuery(QueryOptions<T> options)
        {
            IQueryable<T> query = dbSet;

            //Includes
            foreach (string include in options.GetIncludes())
            {
                query = query.Include(include);
            }

            //Where clauses
            if (options.HasWhere)
            {
                foreach (var clause in options.WhereClauses)
                {
                    query = query.Where(clause);
                }
                count = query.Count();  //filtered count
            }

            //Order by
            if (options.HasOrderBy)
            {
                if (options.OrderByDirection == "asc")
                {
                    query = query.OrderBy(options.OrderBy);
                }
                else
                {
                    query = query.OrderByDescending(options.OrderBy);
                }
            }

            //Paging
            if (options.HasPaging)
            {
                query = query.PageBy(options.PageNumber, options.PageSize);
            }

            return query;
        }
    }
}
