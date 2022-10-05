using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;
using TPF.Controls.Specialized.Calculator;
using TPF.Internal;

namespace TPF.Controls
{
    public class Calculator : Control
    {
        static Calculator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calculator), new FrameworkPropertyMetadata(typeof(Calculator)));

            RegisterCommands();
        }

        public Calculator()
        {
            // Werte initialisieren
            CalculationValue = new CalculatorValue(DecimalSeparator);
            InputValue = new CalculatorValue(DecimalSeparator);
        }

        #region ValueChanged RoutedEvent
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<decimal>),
            typeof(Calculator));

        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }
        #endregion

        #region DecimalSeparator DependencyProperty
        public static readonly DependencyProperty DecimalSeparatorProperty = DependencyProperty.Register("DecimalSeparator",
            typeof(string),
            typeof(Calculator),
            new PropertyMetadata(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

        public string DecimalSeparator
        {
            get { return (string)GetValue(DecimalSeparatorProperty); }
            set { SetValue(DecimalSeparatorProperty, value); }
        }
        #endregion

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(decimal),
            typeof(Calculator),
            new PropertyMetadata(0.0m, OnValueChanged));

        static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.OnValueChanged((decimal)e.OldValue, (decimal)e.NewValue);
        }

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region MemoryValue DependencyProperty
        public static readonly DependencyProperty MemoryValueProperty = DependencyProperty.Register("MemoryValue",
            typeof(decimal?),
            typeof(Calculator),
            new PropertyMetadata(null));

        public decimal? MemoryValue
        {
            get { return (decimal?)GetValue(MemoryValueProperty); }
            set { SetValue(MemoryValueProperty, value); }
        }
        #endregion

        #region History ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey HistoryPropertyKey = DependencyProperty.RegisterReadOnly("History",
            typeof(string),
            typeof(Calculator),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HistoryProperty = HistoryPropertyKey.DependencyProperty;

        public string History
        {
            get { return (string)GetValue(HistoryProperty); }
            protected set { SetValue(HistoryPropertyKey, value); }
        }
        #endregion

        #region DisplayedValue ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey DisplayedValuePropertyKey = DependencyProperty.RegisterReadOnly("DisplayedValue",
            typeof(string),
            typeof(Calculator),
            new PropertyMetadata(null));

        public static readonly DependencyProperty DisplayedValueProperty = DisplayedValuePropertyKey.DependencyProperty;

        public string DisplayedValue
        {
            get { return (string)GetValue(DisplayedValueProperty); }
            protected set { SetValue(DisplayedValuePropertyKey, value); }
        }
        #endregion

        #region HasError ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterReadOnly("HasError",
            typeof(bool),
            typeof(WatermarkTextBox),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty HasErrorProperty = HasErrorPropertyKey.DependencyProperty;

        public bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            protected set { SetValue(HasErrorPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        // Stellt den feststehenden Teil der Historie dar
        internal string FrozenHistory;
        // Das letzte Ergebnis der Kalkulation
        internal CalculatorValue CalculationValue;
        // Ein Wert der als Hilfe für die Historie der angewendeten Funktionen genutzt wird
        internal string FunctionHistoryValue;
        // Eine Liste aller auf den aktuellen Wert ausgeführten Funktionen, die als Hilfe für die Historie genutzt wird
        internal List<Operation> ExecutedFunctions = new List<Operation>();
        // Der ausstehende Operator
        internal TwoValueOperation PendingOperator;
        // Der aktuell im Display dargestellte Wert
        internal CalculatorValue InputValue;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CalculationValue = new CalculatorValue(DecimalSeparator);
            InputValue = new CalculatorValue(DecimalSeparator, Value);

            UpdateDisplay();
        }

        protected virtual void OnValueChanged(decimal oldValue, decimal newValue)
        {
            HasError = false;
            History = null;
            FrozenHistory = null;
            CalculationValue.Clear();
            ExecutedFunctions.Clear();
            FunctionHistoryValue = null;
            PendingOperator = null;
            InputValue = new CalculatorValue(DecimalSeparator, Value);

            UpdateDisplay();

            var eventArgs = new RoutedPropertyChangedEventArgs<decimal>(oldValue, newValue, ValueChangedEvent);

            RaiseEvent(eventArgs);
        }

        private void UpdateDisplay()
        {
            if (HasError)
            {
                History = null;
                DisplayedValue = "ERROR";
                return;
            }
            
            // History füllen anhand von FrozenHistory, ExecutedFunctions und InputValue
            var history = FrozenHistory;

            if (PendingOperator != null) history += PendingOperator.DisplayText;

            if (ExecutedFunctions.Count > 0)
            {
                var valueHistory = FunctionHistoryValue ?? InputValue.DisplayValue;

                for (int i = 0; i < ExecutedFunctions.Count; i++)
                {
                    var function = ExecutedFunctions[i];

                    if (function.Type != OperationType.Function || string.IsNullOrWhiteSpace(function.DisplayText)) continue;

                    valueHistory = $"{function.DisplayText}({valueHistory})";
                }

                history += valueHistory;
            }

            History = history;

            DisplayedValue = InputValue.DisplayValue;
        }

        private static void RegisterCommands()
        {
            var type = typeof(Calculator);

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.UpdateInput, OnUpdateInputCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.Delete, OnDeleteCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.AddOperator, OnAddOperatorCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.FinishCalculation, OnFinishCalculationCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.ExecuteFunction, OnExecuteFunctionCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.ClearAll, OnClearAllCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.Clear, OnClearCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.MemoryClear, OnMemoryClearCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.MemoryRead, OnMemoryReadCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.MemoryStore, OnMemoryStoreCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.MemoryPlus, OnMemoryPlusCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CalculatorCommands.MemoryMinus, OnMemoryMinusCommand));
            // InputBindings registrieren
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D1, Key.NumPad1)) { CommandParameter = 1 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D2, Key.NumPad2)) { CommandParameter = 2 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D3, Key.NumPad3)) { CommandParameter = 3 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D4, Key.NumPad4)) { CommandParameter = 4 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D5, Key.NumPad5)) { CommandParameter = 5 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D6, Key.NumPad6)) { CommandParameter = 6 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D7, Key.NumPad7)) { CommandParameter = 7 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D8, Key.NumPad8)) { CommandParameter = 8 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D9, Key.NumPad9)) { CommandParameter = 9 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new MultiKeyGesture(Key.D0, Key.NumPad0)) { CommandParameter = 0 });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.UpdateInput, new KeyGesture(Key.Decimal)) { CommandParameter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.Delete, new KeyGesture(Key.Back)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.AddOperator, new KeyGesture(Key.Add)) { CommandParameter = CalculatorOperations.Add });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.AddOperator, new KeyGesture(Key.Subtract)) { CommandParameter = CalculatorOperations.Subtract });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.AddOperator, new KeyGesture(Key.Multiply)) { CommandParameter = CalculatorOperations.Multiply });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.AddOperator, new KeyGesture(Key.Divide)) { CommandParameter = CalculatorOperations.Divide });
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.FinishCalculation, new KeyGesture(Key.Enter)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.ClearAll, new KeyGesture(Key.Escape)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.Clear, new KeyGesture(Key.Delete)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(CalculatorCommands.MemoryStore, new KeyGesture(Key.M, ModifierKeys.Control)));
            // KeyGesture lässt Shift+D5 nicht zu, deshalb muss das hier anders generiert werden
            CommandManager.RegisterClassInputBinding(type, new KeyBinding()
            {
                Command = CalculatorCommands.ExecuteFunction,
                CommandParameter = CalculatorOperations.Percent,
                Key = Key.D5,
                Modifiers = ModifierKeys.Shift
            });
        }

        private static void OnUpdateInputCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            if (instance.InputValue.Overwrite)
            {
                instance.FunctionHistoryValue = null;
                instance.ExecutedFunctions.Clear();
            }

            var parameter = e.Parameter?.ToString();

            if (parameter == instance.DecimalSeparator || parameter == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) instance.InputValue.AddDecimalSeparator();
            else if (int.TryParse(parameter, out var number)) instance.InputValue.AddNumber(number);

            instance.UpdateDisplay();
        }

        private static void OnDeleteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            instance.InputValue.RemoveLast();

            instance.UpdateDisplay();
        }

        private static void OnAddOperatorCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            if (e.Parameter is TwoValueOperation operation)
            {
                if (operation.Type != OperationType.Operator) return;
            }
            else return;

            var frozenHistory = instance.FrozenHistory;

            var error = false;

            if (frozenHistory == null)
            {
                var valueHistory = instance.FunctionHistoryValue ?? instance.InputValue.DisplayValue;

                for (int i = 0; i < instance.ExecutedFunctions.Count; i++)
                {
                    var function = instance.ExecutedFunctions[i];

                    if (function.Type != OperationType.Function || string.IsNullOrWhiteSpace(function.DisplayText)) continue;

                    valueHistory = $"{function.DisplayText}({valueHistory})";
                }

                // FrozenHistory ist jetzt der bisherige Input-Wert
                frozenHistory = valueHistory;
                // CalculationValue ist der bisherige Input-Wert
                instance.CalculationValue = new CalculatorValue(instance.DecimalSeparator, instance.InputValue.GetValue());
                // Ausstehenden Operator eintragen
                instance.PendingOperator = operation;
                // ExecutedFunctions leeren
                instance.ExecutedFunctions.Clear();
                instance.FunctionHistoryValue = null;
                // Input als überschreibbar markieren
                instance.InputValue.Overwrite = true;
            }
            else
            {
                if (instance.InputValue.Overwrite && instance.ExecutedFunctions.Count == 0)
                {
                    // Ausstehenden Operator austauschen
                    instance.PendingOperator = operation;
                }
                else
                {
                    try
                    {
                        // Ausstehende Operation ausführen
                        var value = instance.PendingOperator.Body(instance.CalculationValue.GetValue(), instance.InputValue.GetValue());

                        // Wert für CalculationValue vorbereiten
                        var calculationValue = new CalculatorValue(instance.DecimalSeparator, value);
                        // Operator an FrozenHistory anhängen
                        frozenHistory += instance.PendingOperator.DisplayText;

                        // Den InputValue-Text mit den ExecutedFunctions ummanteln
                        var valueHistory = instance.FunctionHistoryValue ?? instance.InputValue.DisplayValue;

                        for (int i = 0; i < instance.ExecutedFunctions.Count; i++)
                        {
                            var function = instance.ExecutedFunctions[i];

                            if (function.Type != OperationType.Function || string.IsNullOrWhiteSpace(function.DisplayText)) continue;

                            valueHistory = $"{function.DisplayText}({valueHistory})";
                        }
                        // Den InputValue-Text an FrozenHistory anhängen
                        frozenHistory += valueHistory;
                        // Wert in CalculationValue eintragen
                        instance.CalculationValue = calculationValue;
                        // ExecutedFunctions leeren
                        instance.ExecutedFunctions.Clear();
                        instance.FunctionHistoryValue = null;
                        // Ausstehenden Operator austauschen
                        instance.PendingOperator = operation;
                        // InputValue als Overwrite markieren
                        instance.InputValue = new CalculatorValue(instance.DecimalSeparator, value);
                    }
                    catch (Exception)
                    {
                        error = true;
                    }
                }
            }

            if (error)
            {
                instance.HasError = true;
            }
            else
            {
                instance.FrozenHistory = frozenHistory;
            }

            instance.UpdateDisplay();
        }

        private static void OnFinishCalculationCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            instance.History = null;
            instance.FrozenHistory = null;

            var error = false;

            // Gibt es etwas zu berechnen?
            if (instance.PendingOperator == null)
            {
                instance.Value = instance.InputValue.GetValue();
                instance.InputValue.Overwrite = true;
            }
            else
            {
                try
                {
                    // Berechnung ausführen
                    instance.Value = instance.PendingOperator.Body(instance.CalculationValue.GetValue(), instance.InputValue.GetValue());
                    instance.CalculationValue.Clear();
                    instance.InputValue.Overwrite = true;
                    instance.PendingOperator = null;
                }
                catch (Exception)
                {
                    error = true;
                }
            }

            if (error)
            {
                instance.HasError = true;
            }
            else
            {
                // Sichergehen, dass das Display korrekt aktualisiert wird
                instance.ExecutedFunctions.Clear();
                instance.FunctionHistoryValue = null;
                instance.InputValue = new CalculatorValue(instance.DecimalSeparator, instance.Value);
            }

            instance.UpdateDisplay();
        }

        private static void OnExecuteFunctionCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            var error = false;

            try
            {
                // Handelt es sich um eine Funktion mit einem Wert oder mit zwei Werten
                if (e.Parameter is SingleValueOperation singleValueOperation)
                {
                    // Funktion ausführen
                    var inputValue = singleValueOperation.Body(instance.InputValue.GetValue());

                    // Funktion in Liste eintragen
                    if (singleValueOperation.Type == OperationType.Function)
                    {
                        instance.ExecutedFunctions.Add(singleValueOperation);
                        if (instance.FunctionHistoryValue == null) instance.FunctionHistoryValue = instance.InputValue.DisplayValue;
                    }
                    else
                    {
                        instance.ExecutedFunctions.Clear();
                        instance.ExecutedFunctions.Add(singleValueOperation);
                        instance.FunctionHistoryValue = null;
                    }

                    instance.InputValue = new CalculatorValue(instance.DecimalSeparator, inputValue);
                    
                }
                else if (e.Parameter is TwoValueOperation twoValueOperation)
                {
                    // Funktion ausführen
                    var inputValue = twoValueOperation.Body(instance.CalculationValue.GetValue(), instance.InputValue.GetValue());

                    // Funktion in Liste eintragen
                    if (twoValueOperation.Type == OperationType.Function)
                    {
                        instance.ExecutedFunctions.Add(twoValueOperation);
                        if (instance.FunctionHistoryValue == null) instance.FunctionHistoryValue = instance.InputValue.DisplayValue;
                    }
                    else
                    {
                        instance.ExecutedFunctions.Clear();
                        instance.ExecutedFunctions.Add(twoValueOperation);
                        instance.FunctionHistoryValue = null;
                    }

                    instance.InputValue = new CalculatorValue(instance.DecimalSeparator, inputValue);
                }
            }
            catch (Exception)
            {
                error = true;
            }

            if (error) instance.HasError = true;

            instance.UpdateDisplay();
        }

        private static void OnClearAllCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            instance.HasError = false;
            instance.History = null;
            instance.FrozenHistory = null;
            instance.CalculationValue.Clear();
            instance.ExecutedFunctions.Clear();
            instance.FunctionHistoryValue = null;
            instance.PendingOperator = null;
            instance.InputValue.Clear();
            instance.Value = 0;
            instance.UpdateDisplay();
        }

        private static void OnClearCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError)
            {
                instance.HasError = false;
                instance.History = null;
                instance.FrozenHistory = null;
                instance.PendingOperator = null;
            }

            instance.InputValue.Clear();
            instance.ExecutedFunctions.Clear();
            instance.FunctionHistoryValue = null;
            instance.Value = 0;
            instance.UpdateDisplay();
        }

        private static void OnMemoryClearCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            instance.MemoryValue = null;
        }

        private static void OnMemoryReadCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            if (instance.MemoryValue != null) instance.Value = instance.MemoryValue.Value;
        }

        private static void OnMemoryStoreCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            instance.MemoryValue = instance.InputValue.GetValue();
        }

        private static void OnMemoryPlusCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            if (instance.MemoryValue == null) instance.MemoryValue = instance.InputValue.GetValue();
            else instance.MemoryValue += instance.InputValue.GetValue();
        }

        private static void OnMemoryMinusCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (Calculator)sender;

            instance.Focus();

            if (instance.HasError) return;

            if (instance.MemoryValue == null) instance.MemoryValue = -instance.InputValue.GetValue();
            else instance.MemoryValue -= instance.InputValue.GetValue();
        }
    }
}