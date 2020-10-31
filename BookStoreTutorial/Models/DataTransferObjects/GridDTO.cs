using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataTransferObjects {
    public class GridDTO {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; } = "asc";
    }
}
