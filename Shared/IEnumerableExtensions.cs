using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class IEnumerableExtensions
    {
        public static T Middle<T>(this IEnumerable<T> enumerable)
        {
            var count = enumerable.Count();

            return enumerable.ElementAt(count / 2);
        }
    }
}
