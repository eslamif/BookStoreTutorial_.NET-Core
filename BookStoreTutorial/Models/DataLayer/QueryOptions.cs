using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataLayer
{
    public class QueryOptions<T>
    {
        public Expression<Func<T, Object>> OrderBy { get; set; }
        public string OrderByDirection { get; set; } = "asc";
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string Includes
        {
            set => includes = value.Replace(" ", "").Split(",");
        }

        public WhereClauses<T> WhereClauses { get; set; }

        public Expression<Func<T, bool>> Where
        {
            set
            {
                if (WhereClauses == null)
                {
                    WhereClauses = new WhereClauses<T>();
                }
                WhereClauses.Add(value);
            }
        }

        public bool HasWhere => WhereClauses != null;
        public bool HasOrderBy => OrderBy != null;
        public bool HasPaging => PageNumber > 0 && PageSize > 0;

        private string[] includes;

        public string[] GetIncludes() => includes ?? new string[0];
    }
}
