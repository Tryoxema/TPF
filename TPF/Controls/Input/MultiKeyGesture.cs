using System.Windows.Input;

namespace TPF.Controls
{
    public class MultiKeyGesture : InputGesture
    {
        public MultiKeyGesture(params KeyCombination[] keys)
        {
            KeyCombinations = keys;
        }

        public KeyCombination[] KeyCombinations { get; }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            if (inputEventArgs is KeyEventArgs keyEventArgs && KeyCombinations != null)
            {
                for (int i = 0; i < KeyCombinations.Length; i++)
                {
                    var keyCombination = KeyCombinations[i];

                    if (keyCombination.Key == keyEventArgs.Key && keyCombination.Modifiers == Keyboard.Modifiers) return true;
                }
            }

            return false;
        }
    }

    public class KeyCombination
    {
        public KeyCombination(Key key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public Key Key { get; }

        public ModifierKeys Modifiers { get; }

        public static implicit operator KeyCombination(Key key) => new KeyCombination(key, ModifierKeys.None);
    }
}