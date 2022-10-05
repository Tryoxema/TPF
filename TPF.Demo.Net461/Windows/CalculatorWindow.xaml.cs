using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPF.Controls;
using TPF.Demo.Net461.Controls;

namespace TPF.Demo.Net461.Windows
{
    [SearchMetadata("Rechner")]
    [WindowMetadata("Rechner")]
    public partial class CalculatorWindow : ChromelessWindow
    {
        public CalculatorWindow()
        {
            InitializeComponent();

            MinWidth = Calculator.MinWidth;
            MaxWidth = Calculator.MaxWidth;
            MinHeight = Calculator.MinHeight + 20;
            MaxHeight = Calculator.MaxHeight + 20;
        }
    }
}