using Excolo.C3L.Exceptions;
using Excolo.C3L.SyntaxAnalysis.Scanning;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Tests.SyntaxAnalysis.Scanning
{
    [TestFixture]
    public class ScannerTests
    {
        
        #region Command Tests
		
        [Test]
        public void Can_Scan_Single_Command()
        {
            ScanAssert
                .Scanner<Scanner>("create name:'nikolaj'")
                .TestTokens(
                    new Token("create", TokenKind.WORD), 
                    new Token("name", TokenKind.WORD),
                    new Token(":", TokenKind.ARGUMENT_SEPARATOR),
                    new Token("nikolaj", TokenKind.STRING),
                    new Token("", TokenKind.EOF)
                );

        }

        [Test]
        public void Can_Scan_Multiple_Commands()
        {
            ScanAssert
                .Scanner<Scanner>("create user; update id:10")
                .TestTokens(
                    new Token("create", TokenKind.WORD),
                    new Token("user", TokenKind.WORD),
                    new Token(";", TokenKind.COMMAND_SEPARATOR),
                    new Token("update", TokenKind.WORD),
                    new Token("id", TokenKind.WORD),
                    new Token(":", TokenKind.ARGUMENT_SEPARATOR),
                    new Token("10", TokenKind.INTEGER),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Multi_Argument_Command()
        {
            ScanAssert
                .Scanner<Scanner>("update id:-10 moderator")
                .TestTokens(
                    new Token("update", TokenKind.WORD),
                    new Token("id", TokenKind.WORD),
                    new Token(":", TokenKind.ARGUMENT_SEPARATOR),
                    new Token("-10", TokenKind.INTEGER),
                    new Token("moderator", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Named_Argument()
        {
            ScanAssert
                .Scanner<Scanner>("id:10")
                .TestTokens(
                    new Token("id", TokenKind.WORD),
                    new Token(":", TokenKind.ARGUMENT_SEPARATOR),
                    new Token("10", TokenKind.INTEGER),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Named_Argument_With_Separator()
        {
            ScanAssert
                .Scanner<Scanner>("id  :    10")
                .TestTokens(
                    new Token("id", TokenKind.WORD),
                    new Token(":", TokenKind.ARGUMENT_SEPARATOR),
                    new Token("10", TokenKind.INTEGER),
                    new Token("", TokenKind.EOF)
                );
        }

	    #endregion

        #region Word Tests

        [Test]
        public void Can_Scan_Word()
        {
            ScanAssert
                .Scanner<Scanner>("doaction")
                .TestTokens(
                    new Token("doaction", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Word_With_Beginning_Underscore()
        {
            ScanAssert
                .Scanner<Scanner>("_doaction")
                .TestTokens(
                    new Token("_doaction", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Word_With_Ending_Underscore()
        {
            ScanAssert
                .Scanner<Scanner>("doaction_")
                .TestTokens(
                    new Token("doaction_", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Word_Center_Underscore()
        {
            ScanAssert
                .Scanner<Scanner>("do_action")
                .TestTokens(
                    new Token("do_action", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Word_Center_Hyphen()
        {
            ScanAssert
                .Scanner<Scanner>("do-action")
                .TestTokens(
                    new Token("do-action", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Word_Ending_Hyphen()
        {
            ScanAssert
                .Scanner<Scanner>("do-action-")
                .TestTokens(
                    new Token("do-action-", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Word_With_Numbers()
        {
            ScanAssert
                .Scanner<Scanner>("do4actions")
                .TestTokens(
                    new Token("do4actions", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }
        
        #endregion

        #region String Tests

        [Test]
        public void Can_Scan_Strings()
        {
            ScanAssert
                .Scanner<Scanner>("echo 'Hello World!'")
                .TestTokens(
                    new Token("echo", TokenKind.WORD),
                    new Token("Hello World!", TokenKind.STRING),
                    new Token("", TokenKind.EOF)
                );

            ScanAssert
                .Scanner<Scanner>("echo \"Hello World!\"")
                .TestTokens(
                    new Token("echo", TokenKind.WORD),
                    new Token("Hello World!", TokenKind.STRING),
                    new Token("", TokenKind.EOF)
                );

        }

        [Test]
        public void Can_Scan_Nested_Strings()
        {
            ScanAssert
                .Scanner<Scanner>("echo '\"Hello World!\"'")
                .TestTokens(
                    new Token("echo", TokenKind.WORD),
                    new Token("\"Hello World!\"", TokenKind.STRING),
                    new Token("", TokenKind.EOF)
                );

            ScanAssert
                .Scanner<Scanner>("echo '\"Hello World!\"'")
                .TestTokens(
                    new Token("echo", TokenKind.WORD),
                    new Token("\"Hello World!\"", TokenKind.STRING),
                    new Token("", TokenKind.EOF)
                );

            ScanAssert
                .Scanner<Scanner>("echo '\"Hello\" this is \"World!\"'")
                .TestTokens(
                    new Token("echo", TokenKind.WORD),
                    new Token("\"Hello\" this is \"World!\"", TokenKind.STRING),
                    new Token("", TokenKind.EOF)
                );


            ScanAssert
                .Scanner<Scanner>("echo 'Hello\" World!'")
                .TestTokens(
                    new Token("echo", TokenKind.WORD),
                    new Token("Hello\" World!", TokenKind.STRING),
                    new Token("", TokenKind.EOF)
                );
        }
        
        #endregion

        #region Number Tests

        [Test]
        public void Can_Scan_Integer()
        {
            ScanAssert
                .Scanner<Scanner>("sum 10 4")
                .TestTokens(
                    new Token("sum", TokenKind.WORD),
                    new Token("10", TokenKind.INTEGER),
                    new Token("4", TokenKind.INTEGER),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Signed_Integer()
        {
            ScanAssert
                .Scanner<Scanner>("pow -10 2")
                .TestTokens(
                    new Token("pow", TokenKind.WORD),
                    new Token("-10", TokenKind.INTEGER),
                    new Token("2", TokenKind.INTEGER),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Real_Number()
        {
            ScanAssert
                .Scanner<Scanner>("sum 10.023 4")
                .TestTokens(
                    new Token("sum", TokenKind.WORD),
                    new Token("10.023", TokenKind.REAL),
                    new Token("4", TokenKind.INTEGER),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Signed_Real_Number()
        {
            ScanAssert
                .Scanner<Scanner>("sum -10.032 4")
                .TestTokens(
                    new Token("sum", TokenKind.WORD),
                    new Token("-10.032", TokenKind.REAL),
                    new Token("4", TokenKind.INTEGER),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Real_Number_With_Inferred_Zero()
        {
            ScanAssert
                .Scanner<Scanner>("sum .0431 4")
                .TestTokens(
                    new Token("sum", TokenKind.WORD),
                    new Token(".0431", TokenKind.REAL),
                    new Token("4", TokenKind.INTEGER),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Signed_Real_Number_With_Inferred_Zero()
        {
            ScanAssert
                .Scanner<Scanner>("sum -.0431 4")
                .TestTokens(
                    new Token("sum", TokenKind.WORD),
                    new Token("-.0431", TokenKind.REAL),
                    new Token("4", TokenKind.INTEGER),
                    new Token("", TokenKind.EOF)
                );
        }
        
        #endregion

        #region Boolean Tests

        [Test]
        public void Can_Scan_Boolean_True()
        {
            ScanAssert
                .Scanner<Scanner>("usehash true")
                .TestTokens(
                    new Token("usehash", TokenKind.WORD),
                    new Token("true", TokenKind.TRUE),
                    new Token("", TokenKind.EOF)
                );
            ScanAssert
                .Scanner<Scanner>("usehash True")
                .TestTokens(
                    new Token("usehash", TokenKind.WORD),
                    new Token("True", TokenKind.TRUE),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_Boolean_False()
        {
            ScanAssert
                .Scanner<Scanner>("usehash false")
                .TestTokens(
                    new Token("usehash", TokenKind.WORD),
                    new Token("false", TokenKind.FALSE),
                    new Token("", TokenKind.EOF)
                );
            ScanAssert
                .Scanner<Scanner>("usehash False")
                .TestTokens(
                    new Token("usehash", TokenKind.WORD),
                    new Token("False", TokenKind.FALSE),
                    new Token("", TokenKind.EOF)
                );
        }
        
        #endregion

        #region Separator Tests

        [Test]
        public void Can_Scan_With_Beginning_Separators()
        {
            ScanAssert
                .Scanner<Scanner>("\t\n\r                     doaction")
                .TestTokens(
                    new Token("doaction", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_With_Separators_Between_Tokens()
        {
            ScanAssert
                .Scanner<Scanner>("doaction      \t\n\r                     True")
                .TestTokens(
                    new Token("doaction", TokenKind.WORD),
                    new Token("True", TokenKind.TRUE),
                    new Token("", TokenKind.EOF)
                );
        }

        [Test]
        public void Can_Scan_With_Ending_Separators()
        {
            ScanAssert
                .Scanner<Scanner>("doaction      \t\n\r              ")
                .TestTokens(
                    new Token("doaction", TokenKind.WORD),
                    new Token("", TokenKind.EOF)
                );
        }
        
        #endregion



        #region Word Exception Tests

        [Test]
        public void Cannot_Scan_Word_With_Starting_Number()
        {
            ScanAssert
                .Scanner<Scanner>("2doaction")
                .TestException<ScannerException>(2);
        }


        [Test]
        public void Cannot_Scan_Word_With_Starting_Hyphens()
        {
            ScanAssert
                .Scanner<Scanner>("-doaction")
                .TestException<ScannerException>(2);
        }

        #endregion

        #region Number Exception Tests

        [Test]
        public void Cannot_Scan_Integer_With_Ending_Letter()
        {
            ScanAssert
                .Scanner<Scanner>("100a")
                .TestException<ScannerException>(4);
        }
        [Test]
        public void Cannot_Scan_Integer_With_Inside_Letter()
        {
            ScanAssert
                .Scanner<Scanner>("10a04")
                .TestException<ScannerException>(3);
        }
        [Test]
        public void Cannot_Scan_Signed_Integer_With_Ending_Letter()
        {
            ScanAssert
                .Scanner<Scanner>("-100a")
                .TestException<ScannerException>(5);
        }
        [Test]
        public void Cannot_Scan_Integer_With_Ending_Hyphens()
        {
            ScanAssert
                .Scanner<Scanner>("100-")
                .TestException<ScannerException>(4);
        }

        [Test]
        public void Cannot_Scan_Real_Number_With_Double_Decimal_Points()
        {
            ScanAssert
                .Scanner<Scanner>("10.021.243")
                .TestException<ScannerException>(7);
        }

        [Test]
        public void Cannot_Scan_Real_Number_With_Comma_As_Decimal_Separator()
        {
            ScanAssert
                .Scanner<Scanner>("10,021")
                .TestException<ScannerException>(3);
        }

        [Test]
        public void Cannot_Scan_Number_With_Plus_Sign()
        {
            ScanAssert
                .Scanner<Scanner>("+10")
                .TestException<UnknownCharacterException>(1);
        }

        #endregion

        #region String Exception Tests

        [Test]
        public void Cannot_Scan_String_Without_Ending_Quote()
        {
            ScanAssert
                .Scanner<Scanner>("'dawdaw")
                .TestException<ScannerException>();
        }
        [Test]
        public void Cannot_Scan_String_Without_Starting_Quote()
        {
            ScanAssert
                .Scanner<Scanner>("dawdaw'")
                .TestException<ScannerException>();
        }

        [Test]
        public void Cannot_Scan_String_Without_Ending_DoubleQuote()
        {
            ScanAssert
                .Scanner<Scanner>("\"dawdaw")
                .TestException<ScannerException>();
        }

        [Test]
        public void Cannot_Scan_String_Without_Starting_DoubleQuote()
        {
            ScanAssert
                .Scanner<Scanner>("dawdaw\"")
                .TestException<ScannerException>();
        }

        [Test]
        public void Cannot_Scan_String_With_Mixed_Quotes()
        {
            ScanAssert
                .Scanner<Scanner>("'dawdaw\"")
                .TestException<ScannerException>();
            ScanAssert
                .Scanner<Scanner>("\"dawdaw'")
                .TestException<ScannerException>();
        }

        [Test]
        public void Cannot_Scan_String_With_Missing_Nested_Ending()
        {
            ScanAssert
                .Scanner<Scanner>("'\"'dawdaw\"'")
                .TestException<ScannerException>();
            ScanAssert
                .Scanner<Scanner>("\"'\"dawdaw'\"")
                .TestException<ScannerException>();
        }

        #endregion


        [Test]
        public void Cannot_Scan_With_Unknown_Character()
        {
            ScanAssert
                .Scanner<Scanner>("$ thisislegal")
                .TestException<UnknownCharacterException>();
        }
    }

    /// <summary>
    /// Test helper for the scanner class.
    /// </summary>
    public class ScanAssert : Assert
    {
        public static ScanTester Scanner<TScanner>(string input) where TScanner : Scanner
        {
            var scanner = (TScanner)Activator.CreateInstance(typeof(TScanner), new object[] { input });

            return new ScanTester(scanner);
        }

        public class ScanTester
        {
            public ScanTester(Scanner scanner)
            {
                Scanner = scanner;
            }

            public Scanner Scanner { get; private set; }

            public void TestTokens(params Token[] expectedTokens)
            {
                var count = expectedTokens.Length;

                for (int i = 0; i < count; ++i)
                {
                    var token = Scanner.Scan();
                    Console.WriteLine(token);
                    Assert.That(token, Is.EqualTo(expectedTokens[i]));
                }
            }

            public void TestException<TExc>(string message) where TExc : ShellException
            {
                TestException<TExc>(null, message);
            }
            public void TestException<TExc>(int? expectedPosition = null, string message = null) where TExc : ShellException
            { 

                var exc = Assert.Throws<TExc>(() => {                     
                    Token token;
                    do
                    {
                        token = Scanner.Scan();
                        Console.WriteLine(token);
                    } while (token.Kind != TokenKind.EOF);
                }, message);

                if (expectedPosition.HasValue)
                    Assert.That(exc.Position, Is.EqualTo(expectedPosition.Value));
                Console.WriteLine("Exception: \n" + exc.Message);
                Console.WriteLine(Scanner.GetErrorPositionString(exc.Position));
            }
        }
    }
}
