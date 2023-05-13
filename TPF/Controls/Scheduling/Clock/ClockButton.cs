using System.Windows.Controls.Primitives;

namespace TPF.Controls
{
    public class ClockButton : ToggleButton
    {
        protected override void OnToggle()
        {
            // Es soll dem Benutzer nicht möglich sein einen geklickten Button wieder abzuwählen
            if (IsChecked != true) base.OnToggle();
        }
    }
}