using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataLayer
{
    public class WhereClauses<T> : List<Expression<Func<T, bool>>> { }
}
