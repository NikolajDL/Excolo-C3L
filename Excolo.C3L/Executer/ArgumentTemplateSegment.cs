using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Executer
{
    /// <summary>
    /// A class defining a segment of the argument template string. 
    /// </summary>
    public class ArgumentTemplateSegment
    {
        /// <summary>
        /// Get the name of the argument segment.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Get whether this argument segment is a catch-all segment.
        /// </summary>
        public bool IsCatchAll { get; private set; }

        /// <summary>
        /// A constructor for the ArgumentTemplateSegment.
        /// </summary>
        /// <param name="argumentTemplateSegment">The string template segment, 
        /// to checked for different flags (e.g. the catch-all flag '*')</param>
        public ArgumentTemplateSegment(string argumentTemplateSegment)
        {
            if (String.IsNullOrEmpty(argumentTemplateSegment))
                throw new Excolo.C3L.Exceptions.ArgumentException(
                    "ArgumentTemplateSegment cannot be null or empty string.");

            Name = argumentTemplateSegment.TrimEnd('*');

            if (argumentTemplateSegment.Last() == '*')
                IsCatchAll = true;
        }

    }
}
