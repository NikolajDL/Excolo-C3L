using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Extensions
{
    /// <summary>
    /// A static class to extend the functionality of the generic <see cref="IEnumerable"/>
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// A method to retrieve the second element of the sequence that satisfies a condition
        /// or a default value if that element doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="self">A reference to the calling collection.</param>
        /// <param name="predicate">A predicate defining the condition.</param>
        public static T SecondOrDefault<T>(this IEnumerable<T> self, Func<T, bool> predicate = null)
        {
            return self.Skip(1).FirstOrDefault(predicate);
        }

        /// <summary>
        /// A method to retrieve the third element of the sequence that satisfies a condition
        /// or a default value if that element doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="self">A reference to the calling collection.</param>
        /// <param name="predicate">A predicate defining the condition.</param>
        public static T ThirdOrDefault<T>(this IEnumerable<T> self, Func<T, bool> predicate = null)
        {
            return self.Skip(1).SecondOrDefault(predicate);
        }
    }
}
