using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Interfaces.AST
{
    /// <summary>
    /// An interface describing the Argument sub-tree of C3L AST.
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// Get the name of the argument, if any.
        /// </summary>
        string ArgumentName { get; }

        /// <summary>
        /// Get the position of the argument in the parsed string.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Get the value of the argument.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Get the type of the argument value.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Get the value of this argument as a specific type. 
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>The value as the requested type if possible.</returns>
        TValue GetValue<TValue>();

        /// <summary>
        /// Try to get the value of this argument as a specific type, and return true if it succeeds. 
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="theValue">The output of the value if it success. 
        /// If it doesn't succeed the output the the default value of the given type.</param>
        /// <returns>Whether the try was succesful or not.</returns>
        bool TryGetValue<TValue>(out TValue theValue);
    }
}
