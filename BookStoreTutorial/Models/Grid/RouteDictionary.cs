using BookStoreTutorial.Models.DataTransferObjects;
using BookStoreTutorial.Models.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.Grid {
    public class RouteDictionary : Dictionary<string, string> {
        public int PageNumber {
            get => Get(nameof(GridDTO.PageNumber)).ToInt();
            set => this[nameof(GridDTO.PageNumber)] = value.ToString();
        }

        public int PageSize {
            get => Get(nameof(GridDTO.PageSize)).ToInt();
            set => this[nameof(GridDTO.PageSize)] = value.ToString();
        }

        public string SortField {
            get => Get(nameof(GridDTO.SortField));
            set => this[nameof(GridDTO.SortField)] = value;
        }

        public string SortDirection {
            get => Get(nameof(GridDTO.SortDirection));
            set => this[nameof(GridDTO.SortDirection)] = value;
        }
        
        public void SetSortAndDirection(string fieldName, RouteDictionary currentRoute) {
            if (currentRoute.SortField.EqualNoCase(fieldName) && currentRoute.SortDirection == "asc") {
                this[nameof(GridDTO.SortField)] = "desc";
            }
            else {
                this[nameof(GridDTO.SortField)] = "asc";
            }
        }

        public RouteDictionary Clone() {
            var clone = new RouteDictionary();

            foreach (var key in Keys) {
                clone.Add(key, this[key]);
            }

            return clone;
        }

        private string Get(string key) => Keys.Contains(key) ? this[key] : null;
    }
}
