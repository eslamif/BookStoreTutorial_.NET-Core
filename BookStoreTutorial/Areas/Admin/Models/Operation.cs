using BookStoreTutorial.Models.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Areas.Admin.Models
{
    public static class Operation
    {
        public static bool IsAdd(string action) => action.EqualNoCase("add");

        public static bool IsDelete(string action) => action.EqualNoCase("delete");
    }
}
