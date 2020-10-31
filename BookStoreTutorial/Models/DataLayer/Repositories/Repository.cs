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

        public virtual void Delete(T entity) => dbSet.Remove(entity);

        public virtual T Get(QueryOptions<T> options) {
            IQueryable<T> query = BuildQuery(options);
            return query.FirstOrDefault();
        }

        public virtual T Get(int id) => dbSet.Find(id);

        public virtual T Get(string id) => dbSet.Find(id);

        public virtual void Insert(T entity) => dbSet.Add(entity);

        public virtual IEnumerable<T> List(QueryOptions<T> options)
        {
            IQueryable<T> query = BuildQuery(options);
            return query.ToList();
        }

        public virtual void Save() => context.SaveChanges();

        public virtual void Update(T entity) => dbSet.Update(entity);

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
