using Excolo.C3L.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.AST
{
    /// <summary>
    /// An abstract base class, defining minimum information about a node in the AST for Excolo C3L.
    /// </summary>
    public abstract class BaseAST
    {
        /// <summary>
        /// Get the current character position.
        /// </summary>
        public virtual int Position { get; protected set; }


        public BaseAST() : this(-1) 
        { }
        /// <summary>
        /// A constructor for the base AST class.
        /// </summary>
        /// <param name="position">The position in the string represented by this AST node.</param>
        public BaseAST(int position)
        {
            Position = position;
        }

        /// <summary>
        /// A method to visit/accept a visitor, for the visitor pattern.
        /// </summary>
        /// <param name="visitor">The Visitor being accepted.</param>
        /// <param name="obj">The object calling the visit method - null if this is the root.</param>
        public abstract object Visit(IVisitor visitor, Object obj);
    }
}
