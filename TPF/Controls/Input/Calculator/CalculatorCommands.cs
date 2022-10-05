using System;
using System.Windows.Input;

namespace TPF.Controls
{
    public static class CalculatorCommands
    {
        static CalculatorCommands()
        {
            var type = typeof(CalculatorCommands);

            UpdateInput = new RoutedCommand(nameof(UpdateInput), type);
            Delete = new RoutedCommand(nameof(Delete), type);
            AddOperator = new RoutedCommand(nameof(AddOperator), type);
            FinishCalculation = new RoutedCommand(nameof(FinishCalculation), type);
            ExecuteFunction = new RoutedCommand(nameof(ExecuteFunction), type);
            ClearAll = new RoutedCommand(nameof(ClearAll), type);
            Clear = new RoutedCommand(nameof(Clear), type);
            MemoryClear = new RoutedCommand(nameof(MemoryClear), type);
            MemoryRead = new RoutedCommand(nameof(MemoryRead), type);
            MemoryStore = new RoutedCommand(nameof(MemoryStore), type);
            MemoryPlus = new RoutedCommand(nameof(MemoryPlus), type);
            MemoryMinus = new RoutedCommand(nameof(MemoryMinus), type);
        }

        public static ICommand UpdateInput { get; private set; }

        public static ICommand Delete { get; private set; }

        public static ICommand AddOperator { get; private set; }

        public static ICommand FinishCalculation { get; private set; }

        public static ICommand ExecuteFunction { get; private set; }

        public static ICommand ClearAll { get; private set; }

        public static ICommand Clear { get; private set; }

        public static ICommand MemoryClear { get; private set; }

        public static ICommand MemoryRead { get; private set; }

        public static ICommand MemoryStore { get; private set; }

        public static ICommand MemoryPlus { get; private set; }

        public static ICommand MemoryMinus { get; private set; }
    }
}