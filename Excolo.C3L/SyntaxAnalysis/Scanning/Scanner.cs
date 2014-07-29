using Excolo.C3L.Exceptions;
using Excolo.C3L.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.Scanning
{
    /// <summary>
    /// A scanner class used to scan a given string for tokens defined by the C3L language. 
    /// </summary>
    public class Scanner : IScanner
    {
        private string _workingString;
        private int _position;
        private StringBuilder _currentSpelling;
        private TokenKind _currentKind;

        /// <summary>
        /// Get the full string this scanner is scanning.
        /// </summary>
        public virtual string WorkingString
        {
            get { return _workingString; }
        }

        /// <summary>
        /// A method to return a two-line string showing where in the input an error occured.
        /// </summary>
        /// <param name="position">The position of the error starting at 1</param>
        /// <returns></returns>
        public virtual string GetErrorPositionString(int position, int? consoleWidth = null)
        {
            if(!consoleWidth.HasValue)
            {
                try { consoleWidth = Console.WindowWidth; }
                catch { consoleWidth = 80; }
            }

            var startChar = 0;
            var subString = _workingString;

            if (position > consoleWidth)
                startChar = position - (consoleWidth.Value / 2);

            var pos = (position > 1 ? position : 1) - startChar;


            if (startChar > 0)
            {
                subString = "..." + subString.Substring(startChar);
                pos += 3;
            }

            if (subString.Length > consoleWidth)
                subString = subString.Substring(0, consoleWidth.Value - 3) + "...";

            return subString + "\n" + "^".PadLeft(pos, ' ');
        }

        /// <summary>
        /// A constructor for the scanner.
        /// </summary>
        /// <param name="input">The input string to scan for tokens.</param>
        public Scanner(string input)
        {

            if (String.IsNullOrEmpty(input))
                throw new ScannerException("Input null or empty string");

            _workingString = input;
            _position = 0;
            _currentKind = TokenKind.EOF;
        }

        /// <summary>
        /// A method to scan for the next token in the string giving during initialization.
        /// </summary>
        public virtual Token Scan()
        {
            // Remove all separators from the beginning.
            while (IsSeparator(CurrentChar))
            {
                ScanSeparator();
            }

            _currentSpelling = new StringBuilder();
            int initPosition = _position + 1;

            ScanToken();

            // Test for separator after token
            if (IsSeparator(CurrentChar) || IsSymbol(CurrentChar) || IsEof(CurrentChar) 
                || _currentKind == TokenKind.ARGUMENT_SEPARATOR
                || _currentKind == TokenKind.COMMAND_SEPARATOR)
                return new Token(initPosition, _currentSpelling.ToString(), _currentKind);

            throw new ScannerException(_position + 1, "Unexpected end of token: " + _currentKind.ToString());
        }

        #region Scan Methods

        private void ScanSeparator()
        {
            switch (CurrentChar)
            {
                case ' ':
                case '\n':
                case '\r':
                case '\t':
                    Skip();
                    return;
            }
        }


        private void ScanToken()
        {
            if (ScanSymbolTable())
            { /* We've already done it */ }
            else if (IsStartOfWord(CurrentChar))
            {
                _currentKind = TokenKind.WORD;
                TakeIt();
                while (IsInsideOfWord(CurrentChar))
                {
                    TakeIt();
                }
            }
            else if (CurrentChar == '"' || CurrentChar == '\'')
            {
                _currentKind = TokenKind.STRING;
                char initialQuote = CurrentChar;
                Skip();
                while (CurrentChar != initialQuote)
                {
                    if (CurrentChar == (char)0)
                    {
                        throw new ScannerException(_position + 1, "Premature EOF. ");
                    }

                    TakeIt();
                }
                Skip();
            }
            else if (IsStartOfNumeric(CurrentChar))
            {
                var separatorCount = 0;

                if (CurrentChar == '.')
                {
                    separatorCount++;
                    _currentKind = TokenKind.REAL;
                }
                else
                    _currentKind = TokenKind.INTEGER;

                TakeIt();

                while (IsInsideNumeric(CurrentChar))
                {
                    if (CurrentChar == '.')
                    {
                        _currentKind = TokenKind.REAL;
                        separatorCount++;
                    }

                    if (separatorCount > 1)
                    {
                        throw new ScannerException(_position + 1, "Too many decimal-separators in real number: " + separatorCount);
                    }

                    TakeIt();
                }

                if (_currentKind == TokenKind.INTEGER)
                { 
                    int n;
                    if (!int.TryParse(_currentSpelling.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out n))
                    {
                        throw new ScannerException(_position + 1, "The spelling of the token of type INTEGER, could not be parsed as an integer.");
                    }
                }else if (_currentKind == TokenKind.REAL)
                { 
                    float n;
                    if (!float.TryParse(_currentSpelling.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out n))
                    {
                        throw new ScannerException(_position + 1, "The spelling of the token of type REAL, could not be parsed as a real number.");
                    }
                }

            }
            else if (CurrentChar == (char)0)
            {
                _currentKind = TokenKind.EOF;
            }
            else
            {
                char unknow = CurrentChar;
                TakeIt();
                throw new UnknownCharacterException(_position, unknow, "Unknown character encountered.");
            }

            return;
        }

        private bool ScanSymbolTable()
        {
            if (CurrentChar == ';')
            {
                _currentKind = TokenKind.COMMAND_SEPARATOR;
                TakeIt();
            }
            else if (CurrentChar == ':')
            {
                _currentKind = TokenKind.ARGUMENT_SEPARATOR;
                TakeIt();
            }
            else if (CurrentString(4).Equals("true") || CurrentString(4).Equals("True"))
            {
                _currentKind = TokenKind.TRUE;
                TakeIt(4);
            }
            else if (CurrentString(5).Equals("false") || CurrentString(5).Equals("False"))
            {
                _currentKind = TokenKind.FALSE;
                TakeIt(5);
            }
            else
            {
                return false;
            }
            return true;
        }
        
        #endregion

        #region Lexical Test Methods

        private bool IsSymbol(char c)
        {
            return c == ':' || c == ';';
        }

        private bool IsEof(char c)
        {
            return c == (char)0;
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private bool IsSeparator(char c)
        {
            return c == ' ' || c == '\n' || c == '\t' || c == '\r';
        }

        private bool IsSign(char c)
        {
            return c == '-';
        }

        private bool IsRealSeparator(char c)
        {
            return c == '.';
        }

        private bool IsStartOfWord(char c)
        {
            return IsLetter(c) || c == '_';
        }

        private bool IsInsideOfWord(char c)
        {
            return IsDigit(c) || IsLetter(c) || c == '_' || c == '-';
        }

        private bool IsStartOfNumeric(char c)
        {
            return IsSign(c) || IsDigit(c) || IsRealSeparator(c);
        }

        private bool IsInsideNumeric(char c)
        {
            return IsDigit(c) || IsRealSeparator(c);
        }
        
        #endregion

        #region Helper Methods

        private char CurrentChar
        {
            get
            {
                if (_position < _workingString.Length)
                {
                    return _workingString[_position];
                }
                return (char)0;
            }
        }

        private string CurrentString(int length)
        {
            if (length < 1)
            {
                throw new System.ArgumentException("Non-positive length received: " + length.ToString());
            }
            if (length + _position > _workingString.Length)
            {
                return new string((char)0, length);
            }
            return _workingString.Substring(_position, length);
        }

        private void Take(char c)
        {
            if (CurrentChar == c)
            {
                TakeIt();
            }
            else
            {
                throw new IllegalCharacterException(_position, c, CurrentChar);
            }
        }

        private void TakeIt(int num = 1)
        {
            _currentSpelling.Append(CurrentString(num));
            Skip(num);
        }

        private void Skip(int num = 1)
        {
            _position += num;
        }

        private void Swap<T>(ref T one, ref T two)
        {
            T temp = one;
            one = two;
            two = temp;
        }
        
        #endregion

    }
}
