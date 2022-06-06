using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaudiA.Foundation.Session.Business
{
    public static class StringExtensions
    {
        public static string ReplaceEmptyBracesAndHyphen(this string name)
        {
            return name.Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", "");
        }
    }
}