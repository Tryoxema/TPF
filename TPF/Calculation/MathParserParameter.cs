namespace TPF.Calculation
{
    public class MathParserParameter
    {
        public MathParserParameter(string token, double value)
        {
            Token = token;
            Value = value;
        }

        public string Token { get; }

        public double Value { get; }
    }
}