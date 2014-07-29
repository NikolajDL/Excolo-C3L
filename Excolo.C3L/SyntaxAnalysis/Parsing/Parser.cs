using Excolo.C3L.Exceptions;
using Excolo.C3L.Interfaces.AST;
using Excolo.C3L.SyntaxAnalysis.AST;
using Excolo.C3L.SyntaxAnalysis.Scanning;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.Parsing
{
    /// <summary>
    /// A parser class used to parse a given string in the Excolo C3L language and return an AST.
    /// </summary>
    public class Parser : Excolo.C3L.Interfaces.IShellParser
    {

        private Excolo.C3L.Interfaces.IScanner _scanner;
        private Token _currentToken;
        private List<ParserException> Errors { get; set; }

        /// <summary>
        /// Get whether the parser is in panic mode.
        /// </summary>
        public bool Panic { get; private set; }

        /// <summary>
        /// A constructor for the parser
        /// </summary>
        /// <param name="panicMode">Set whether panic mode is on. Defualt: On</param>
        public Parser(bool panicMode = true)
        {
            Panic = panicMode;
        }

        /// <summary>
        /// A method to parse the given input string and return an AST accordingly.
        /// </summary>
        /// <param name="input">The input to parse.</param>
        /// <returns>An abstract syntax tree (AST) representing the parsed input.</returns>
        public IProgramSequence Parse(string input)
        {
            Errors = new List<ParserException>();

            ProgramSequence result = null;

            try
            {
                _scanner = new Scanner(input);
                _currentToken = _scanner.Scan();

                try
                {
                    result = ParseProgramSequence();
                }
                catch (ParserException e)
                {
                    Errors.Add(e);
                }
            }
            catch (ScannerException e)
            {
                Errors.Add(new ParserException("Invalid input, discarded by scanner. See inner exception", e));
            }
            catch (Exception e)
            {
                Errors.Add(new ParserException("Unknown error. See inner exception", e));
                throw;
            }
            finally
            {
                if (result == null)
                    result = new ProgramSequence(null, null);
            }


            result.Errors = Errors;
            return result;
        }

        #region Parse Methods


        private ProgramSequence ParseProgramSequence()
        {
            ICommand command = ParseCommand();
            IProgramSequence follow = ParseProgramSequenceFollow();
            
            if (!IsEOF(_currentToken))
            {
                Errors.Add(new IllegalTokenException(_currentToken, "Should have encountered end of file", TokenKind.EOF));
            }

            return new ProgramSequence(command, follow);;
        }

        private IProgramSequence ParseProgramSequenceFollow()
        {
            IProgramSequence result = null;
            if (_currentToken.Kind == TokenKind.COMMAND_SEPARATOR)
            {
                Accept(TokenKind.COMMAND_SEPARATOR);
                ICommand command = ParseCommand();
                IProgramSequence follow = ParseProgramSequenceFollow();
                result = new ProgramSequence(command, follow);
            }
            else
            {
                result = new ProgramSequenceEmpty(_currentToken.Position);
            }
            return result;
        }

        private ICommand ParseCommand()
        {
            string name = null;
            int position = -1;
            IArgumentSequence args = null;

            // Check for a word token - as the commandname must be a word.
            if (_currentToken.Kind != TokenKind.WORD)
            {
                return GoPanic(ParseCommand, TokenKind.WORD);
            }
            // If everything is alright, get the name
            name = _currentToken.Spelling;
            position = _currentToken.Position;
            AcceptIt();

            // Parse arguments
            args = ParseArgumentSequence();

            // Create command node
            return new Command(position, name, args);
        }


        private IArgumentSequence ParseArgumentSequence()
        {
            IArgumentSequence result = null;

            // Check for end-of-file or end of command - if so the argument sequence is empty.
            if (IsEOF(_currentToken) || IsCommand(_currentToken))
            {
                result = new ArgumentSequenceEmpty(_currentToken.Position);
            }
            else
            {
                Excolo.C3L.Interfaces.AST.IArgument arg = ParseArgument();
                IArgumentSequence args = ParseArgumentSequence();

                result = new ArgumentSequence(arg, args);
            }

            return result;
        }

        private IArgument ParseArgument()
        {
            IArgument result = null;
            int position = _currentToken.Position;

            switch (_currentToken.Kind)
            {
                case TokenKind.INTEGER:
                case TokenKind.REAL:
                case TokenKind.TRUE:
                case TokenKind.FALSE:
                case TokenKind.STRING:
                    result = ParseArgumentValue(_currentToken);
                    AcceptIt();
                    break;
                case TokenKind.WORD:
                    var spelling = _currentToken.Spelling;
                    AcceptIt();
                    if (_currentToken.Kind == TokenKind.ARGUMENT_SEPARATOR)
                    {
                        AcceptIt();
                        result = new Argument(position, spelling, ParseArgumentValue(_currentToken).Value, typeof(string));
                        AcceptIt();
                    }
                    else
                    {
                        result = new Argument(position, spelling, typeof(string));
                    }
                    break;
                default:
                    GoPanic(ParseCommand, TokenKind.INTEGER, TokenKind.REAL,
                        TokenKind.TRUE, TokenKind.FALSE, TokenKind.STRING, TokenKind.WORD);
                    break;
            }
            return result;
        }

        private IArgument ParseArgumentValue(Token token)
        {
            IArgument result = null;
            int position = _currentToken.Position;

            switch (token.Kind)
            {
                case TokenKind.INTEGER:
                    int valueInt;
                    bool isNumeric = int.TryParse(token.Spelling, NumberStyles.Number, CultureInfo.InvariantCulture, out valueInt);
                    if (!isNumeric)
                        GoPanic(ParseCommand,
                            () => new MalformedTokenException(_currentToken, "Could not parse token as integer as expected."));
                    result = new Argument(position, valueInt, typeof(int));
                    break;
                case TokenKind.REAL:
                    float valueFloat;
                    bool isFloat = float.TryParse(token.Spelling, NumberStyles.Number, CultureInfo.InvariantCulture, out valueFloat);
                    if (!isFloat)
                        GoPanic(ParseCommand,
                            () => new MalformedTokenException(_currentToken, "Could not parse token as float as expected."));
                    result = new Argument(position, valueFloat, typeof(float));
                    break;
                case TokenKind.TRUE:
                    result = new Argument(position, true, typeof(bool));
                    break;
                case TokenKind.FALSE:
                    result = new Argument(position, false, typeof(bool));
                    break;
                case TokenKind.STRING:
                    result = new Argument(position, token.Spelling, typeof(string));
                    break;
                case TokenKind.WORD:
                    result = new Argument(position, token.Spelling, typeof(string));
                    break;
            }

            return result;
        }

        #endregion

        #region Token Test Methods

        private bool IsEOF(Token t)
        {
            return t.Kind == TokenKind.EOF;
        }

        private bool IsCommand(Token t)
        {
            return t.Kind == TokenKind.COMMAND_SEPARATOR;
        }

        private bool IsWord(Token t)
        {
            return t.Kind == TokenKind.WORD;
        }

        #endregion

        #region Helper Methods

        private TResult GoPanic<TResult>(Func<TResult> startOver, params TokenKind[] expectedTokens)
        {
            return GoPanic(startOver, () => new IllegalTokenException(_currentToken, expectedTokens));
            /*
            // If not in panic mode, throw exception.
            if (!Panic)
                throw new IllegalTokenException(_currentToken, expectedTokens);

            // If in panic mode, add to list of exceptions and skip tokens until a command-separator is found
            Errors.Add(new IllegalTokenException(_currentToken, expectedTokens));
            do
            {
                AcceptIt();
            }
            while ((_currentToken.Kind != TokenKind.COMMAND_SEPARATOR) && Panic);

            // Start over parsing
            return startOver.Invoke();
             */
        }

        private TResult GoPanic<TResult, TException>(Func<TResult> startOver, Func<TException> exception)
            where TException : ParserException
        {
            // If not in panic mode, throw exception.
            if (!Panic)
                throw exception.Invoke();

            // If in panic mode, add to list of exceptions and skip tokens until a command-separator is found
            Errors.Add(exception.Invoke());
            do
            {
                AcceptIt();
            }
            while ((_currentToken.Kind != TokenKind.COMMAND_SEPARATOR) && Panic);

            // Start over parsing
            return startOver.Invoke();
        }

        private void Accept(TokenKind expectedKind)
        {
            if (_currentToken.Kind != expectedKind)
            {
                Errors.Add(new IllegalTokenException(_currentToken, expectedKind));
                do
                {
                    AcceptIt();
                }
                while ((_currentToken.Kind != expectedKind) && Panic);
            }
            AcceptIt();
        }

        private void AcceptIt()
        {
            try
            {
                if (_currentToken.Kind == TokenKind.EOF)
                {
                    throw new IllegalTokenException(_currentToken, TokenKind.COMMAND_SEPARATOR, TokenKind.WORD);
                }
                _currentToken = _scanner.Scan();
            }
            catch (ScannerException e)
            {
                if (Panic)
                {
                    Errors.Add(new ParserException("Invalid input, discarded by scanner. See inner exception", e));
                }
                else
                {
                    throw;
                }
            }
        }
        
        #endregion
    }
}
