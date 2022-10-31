using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPF.Internal
{
    internal class MemberPathTokenizer
    {
        private MemberPathTokenizer(string memberPath)
        {
            _memberPath = string.IsNullOrWhiteSpace(memberPath) ? string.Empty : memberPath.Trim();
        }

        private const string TokenSeparators = ".[]";
        private readonly string _memberPath;
        private int _currentReadingIndex;

        private char CurrentChar
        {
            get
            {
                if (_currentReadingIndex < _memberPath.Length)
                {
                    return _memberPath[_currentReadingIndex];
                }

                return char.MinValue;
            }
        }

        private bool HasMoreCharacters
        {
            get { return _currentReadingIndex < _memberPath.Length; }
        }

        private void MoveNext()
        {
            _currentReadingIndex++;
        }

        public static List<IMemberPathToken> GetTokens(string memberPath)
        {
            var instance = new MemberPathTokenizer(memberPath);

            return instance.GetTokens();
        }

        private List<IMemberPathToken> GetTokens()
        {
            var tokens = new List<IMemberPathToken>();

            if (_memberPath.Length == 0) return tokens;

            while (HasMoreCharacters)
            {
                // CurrentChar wird in Variable geschrieben, damit der Getter nicht 3 mal ausgeführt wird
                var currentChar = CurrentChar;

                if (char.IsWhiteSpace(currentChar) || currentChar == '.')
                {
                    MoveNext();
                }
                else
                {
                    IMemberPathToken token;

                    if (currentChar != '[') token = CreatePropertyToken();
                    else token = CreateIndexerToken();

                    if (token != null) tokens.Add(token);
                }
            }

            return tokens;
        }

        // Erstellt einen Token für eine normale Property
        private PropertyToken CreatePropertyToken()
        {
            var startIndex = _currentReadingIndex;

            while (HasMoreCharacters)
            {
                MoveNext();

                // Wenn wir bei einem der möglichen Separatoren ankommen, können wir aufhören weiter zu lesen
                if (TokenSeparators.Contains(CurrentChar)) break;
            }

            var propertyName = _memberPath.Substring(startIndex, _currentReadingIndex - startIndex).Trim();

            if (string.IsNullOrWhiteSpace(propertyName)) return null;

            var token = new PropertyToken(propertyName);

            return token;
        }

        // Erstellt einen Token für einen Indexer, wie z.B. für List<T>
        private IndexerToken CreateIndexerToken()
        {
            var indexerArguments = new List<object>();
            var argumentBuilder = new StringBuilder();
            var nestedBracketsLevel = 0;

            do
            {
                switch (CurrentChar)
                {
                    case '[':
                    {
                        if (nestedBracketsLevel > 0) argumentBuilder.Append('[');

                        nestedBracketsLevel++;
                        break;
                    }
                    case ']':
                    {
                        nestedBracketsLevel--;

                        if (argumentBuilder.Length == 0) break;

                        if (nestedBracketsLevel > 0) argumentBuilder.Append(']');
                        else
                        {
                            var indexerArgument = argumentBuilder.ToString();
                            argumentBuilder.Length = 0;
                            indexerArguments.Add(indexerArgument);
                        }

                        break;
                    }
                    case ',':
                    {
                        indexerArguments.Add(argumentBuilder.ToString());
                        argumentBuilder.Length = 0;
                        break;
                    }
                    default:
                    {
                        argumentBuilder.Append(CurrentChar);
                        break;
                    }
                }

                MoveNext();
            }
            while (nestedBracketsLevel != 0 && HasMoreCharacters);

            var token = new IndexerToken(indexerArguments);

            return token;
        }
    }
}