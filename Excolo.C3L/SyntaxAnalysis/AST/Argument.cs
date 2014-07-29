using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.AST
{
    /// <summary>
    /// A class defining the argument of a command in the AST.
    /// </summary>
    public class Argument : BaseAST, IArgument
    {

        /// <summary>
        /// Get the name of the argument, if any.
        /// </summary>
        public string ArgumentName { get; private set; }

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
        public Argument(object value, Type type)
            : this(-1, null, value, type)
        {
        }
        /// <summary>
        /// A constructor for an argument
        /// </summary>
        /// <param name="position">The position of this argument in the parsed string.</param>
        /// <param name="value">The value of the argument.</param>
        public Argument(int position, object value, Type type)
            : this(position, null, value, type)
        {
        }

        /// <summary>
        /// A constructor for an argument
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        public Argument(string name, object value, Type type)
            : this(-1, name, value, type)
        {
        }

        /// <summary>
        /// A constructor for an argument
        /// </summary>
        /// <param name="position">The position of this argument in the parsed string.</param>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        public Argument(int position, string name, object value, Type type)
            : base(position)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Argument value cannot be null");
            if (type == null)
                throw new ArgumentNullException("type", "Argument valuetype cannot be null");

            ArgumentName = name;
            Value = value;
            ValueType = type;
        }

        /// <summary>
        /// A method to visit/accept a visitor, for the visitor pattern.
        /// </summary>
        /// <param name="visitor">The Visitor being accepted.</param>
        /// <param name="obj">The object calling the visit method - null if this is the root.</param>
        public override object Visit(Interfaces.IVisitor visitor, object obj)
        {
            return visitor.Visit(this, obj);
        }



    }
}
