using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataLayer
{
    public static class QueryExtensions
    {
        public static IQueryable<T> PageBy<T>(this IQueryable<T> items, int pageNumber, int pageSize)
        {
            //Skip pages and return remaining up to pageSize
            return items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
