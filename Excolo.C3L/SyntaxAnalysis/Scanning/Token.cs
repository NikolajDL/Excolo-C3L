using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.Scanning
{
    /// <summary>
    /// A token used to perform syntactical analysis.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Get the position of the token in the command.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Get the string that make up the token.
        /// </summary>
        public string Spelling { get; private set; }

        /// <summary>
        /// Get the kind of the token.
        /// </summary>
        public TokenKind Kind { get; private set; }

        /// <summary>
        /// A constructor for the token.
        /// </summary>
        /// <param name="spelling">The string that make up the token.</param>
        /// <param name="kind">The kind of the token.</param>
        public Token(string spelling, TokenKind kind)
            : this(-1, spelling, kind)
        {
        }
        /// <summary>
        /// A constructor for the token.
        /// </summary>
        /// <param name="position">The position of the token in the command.</param>
        /// <param name="spelling">The string that make up the token.</param>
        /// <param name="kind">The kind of the token.</param>
        public Token(int position, string spelling, TokenKind kind)
        {
            Position = position;
            Spelling = spelling;
            Kind = kind;
        }


        /// <summary>
        /// A method to check whether two tokens are equal.
        /// </summary>
        /// <param name="obj"></param>
        public virtual bool Equals(Token other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!Spelling.Equals(other.Spelling))
                return false;

            if (Kind != other.Kind)
                return false;

            return true;
        }

        /// <summary>
        /// A method to check whether two tokens are equal.
        /// </summary>
        /// <param name="other">The object to check equality against.</param>
        public override bool Equals(object other)
        {
            return Equals(other as Token);
        }

        /// <summary>
        /// A string representation of the token class,
        /// in this case the token spelling is returned.
        /// </summary>
        public override string ToString()
        {
            return Spelling + " : " + Kind.ToString();
        }
    }
}
