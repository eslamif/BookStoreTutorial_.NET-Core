using BookStoreTutorial.Models.DataTransferObjects;
using BookStoreTutorial.Models.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.Grid {
    public class GridBuilder {
        private const string RouteKey = "currentroute";
        public RouteDictionary Routes { get; set; }
        public ISession Session { get; set; }
        public RouteDictionary CurrentRoute => Routes;

        public GridBuilder(ISession session) {
            this.Session = session;
            Routes = session.GetObject<RouteDictionary>(RouteKey) ?? new RouteDictionary();
        }

        public GridBuilder(ISession session, GridDTO grid, string defaultSortField) {
            this.Session = session;
            
            Routes = new RouteDictionary();
            Routes.PageNumber = grid.PageNumber;
            Routes.PageSize = grid.PageSize;
            Routes.SortField = grid.SortField ?? defaultSortField;

            SaveRouteSegment();
        }

        public void SaveRouteSegment() => Session.SetObject<RouteDictionary>(RouteKey, Routes);

        public int GetTotalPages(int count) {
            int size = Routes.PageSize;
            return (count + size - 1) / size;
        }

    }
}
