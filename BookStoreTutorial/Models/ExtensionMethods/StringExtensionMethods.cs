using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.ExtensionMethods {
    public static class StringExtension {

        public static string Slug(this string s) {
            var sb = new StringBuilder();

            foreach (char c in s) {
                if (c == '-' || !char.IsPunctuation(c)) {
                    sb.Append(c);
                }
            }
            return sb.ToString().Replace(' ', '-');
        }

        public static bool EqualNoCase(this string s, string toCompare) => s?.ToLower() == toCompare?.ToLower();

        public static int ToInt(this string s) {
            int.TryParse(s, out int id);
            return id;
        }

        public static string Capitalize(this string s) => s?.Substring(0, 1)?.ToUpper() + s?.Substring(1);

    }
}
