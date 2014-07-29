using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Core
{
    /// <summary>
    /// An argument class used to hold values during execution of commands. 
    /// Doesn't contain any AST-related content.
    /// </summary>
    public class ValueArgument : IArgument
    {

        /// <summary>
        /// Get the name of the argument, if any.
        /// </summary>
        public string ArgumentName { get; private set; }
        
        /// <summary>
        /// Get the position of the argument in the parsed string.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Get the value of the argument.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Get the type of the argument value.
        /// </summary>
        public Type ValueType { get; private set; }

        /// <summary>
        /// Get the value of this argument as a specific type. 
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>The value as the requested type if possible.</returns>
        public TValue GetValue<TValue>()
        {
            if (Value is TValue)
                return (TValue)Value;
            else
                throw new InvalidCastException("You cannot get the value of this C3L argument as a " + typeof(TValue).FullName);
        }

        /// <summary>
        /// Try to get the value of this argument as a specific type, and return true if it succeeds. 
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="theValue">The output of the value if it success. 
        /// If it doesn't succeed the output the the default value of the given type.</param>
        /// <returns>Whether the try was succesful or not.</returns>
        public bool TryGetValue<TValue>(out TValue theValue)
        {
            if (Value is TValue)
            {
                theValue = (TValue)Value;
                return true;
            }
            else
            {
                theValue = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// A constructor for an argument
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        public ValueArgument(object value, Type type)
            : this(null, value, type)
        {
        }

        /// <summary>
        /// A constructor for an argument
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        public ValueArgument(string name, object value, Type type)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Argument value cannot be null");

            ArgumentName = name;
            Value = value;
            Position = -1;
            ValueType = type;
        }

    }
}
