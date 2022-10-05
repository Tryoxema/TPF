namespace TPF.Controls.Specialized.Calculator
{
    public abstract class Operation
    {
        public string DisplayText { get; set; }

        public OperationType Type { get; set; }
    }
}