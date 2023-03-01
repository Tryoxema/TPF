using System;
using System.Linq;
using System.Text;
using System.Security;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class PasswordBox : WatermarkTextBox
    {
        static PasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(typeof(PasswordBox)));
        }

        public PasswordBox()
        {
            CommandManager.AddPreviewCanExecuteHandler(this, OnPreviewCanExecuteCommand);
            DataObject.AddPastingHandler(this, OnDataPasting);

            // Password sollte niemals null sein, also vorbelegen
            Password = string.Empty;

            // Undo-Funktion deaktivieren
            IsUndoEnabled = false;
            UndoLimit = 0;
        }

        #region PasswordChanged RoutedEvent
        public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent("PasswordChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(PasswordBox));

        public event RoutedEventHandler PasswordChanged
        {
            add { AddHandler(PasswordChangedEvent, value); }
            remove { RemoveHandler(PasswordChangedEvent, value); }
        }
        #endregion

        #region PasswordChar DependencyProperty
        public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register("PasswordChar",
            typeof(char),
            typeof(PasswordBox),
            new PropertyMetadata('●', OnPasswordCharChanged));

        static void OnPasswordCharChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (PasswordBox)sender;

            instance.SyncDisplayText(instance.CaretIndex);
        }

        public char PasswordChar
        {
            get { return (char)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }
        #endregion

        #region ShowPasswordButtonVisibility DependencyProperty
        public static readonly DependencyProperty ShowPasswordButtonVisibilityProperty = DependencyProperty.Register("ShowPasswordButtonVisibility",
            typeof(Visibility),
            typeof(PasswordBox),
            new PropertyMetadata(Visibility.Collapsed));

        public Visibility ShowPasswordButtonVisibility
        {
            get { return (Visibility)GetValue(ShowPasswordButtonVisibilityProperty); }
            set { SetValue(ShowPasswordButtonVisibilityProperty, value); }
        }
        #endregion

        private int _caretIndex = -1;

        public string Password
        {
            [SecurityCritical]
            get
            {
                string passwordString;

                var stringPtr = Marshal.SecureStringToBSTR(SecurePassword);

                try
                {
                    passwordString = Marshal.PtrToStringUni(stringPtr);
                }
                finally
                {
                    Marshal.ZeroFreeBSTR(stringPtr);
                }

                return passwordString;
            }
            [SecurityCritical]
            set
            {
                if (value == null) value = string.Empty;

                SecurePassword = new SecureString();

                for (int i = 0; i < value.Length; ++i)
                {
                    SecurePassword.AppendChar(value[i]);
                }

                SyncDisplayText(_caretIndex);

                RaiseEvent(new RoutedEventArgs(PasswordChangedEvent, this));
            }
        }

        private void SetPassword(string password, int caretIndex)
        {
            _caretIndex = caretIndex;
            Password = password;
            _caretIndex = -1;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SecureString SecurePassword { get; private set; }

        internal Button ShowPasswordButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ShowPasswordButton != null)
            {
                ShowPasswordButton.IsPressedChanged -= ShowPasswordButton_IsPressedChanged;
            }

            ShowPasswordButton = GetTemplateChild("PART_ShowPasswordButton") as Button;

            if (ShowPasswordButton != null)
            {
                ShowPasswordButton.IsPressedChanged += ShowPasswordButton_IsPressedChanged;
            }
        }

        private void ShowPasswordButton_IsPressedChanged(object sender, EventArgs e)
        {
            SyncDisplayText(CaretIndex);
        }

        [SecuritySafeCritical]
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            // OnPreviewKeyDown kümmert sich um \r, deshalb hier ignorieren
            if (e.Text != "\r") PasswordInsert(e.Text, CaretIndex);

            e.Handled = true;

            base.OnPreviewTextInput(e);
        }

        [SecuritySafeCritical]
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                {
                    e.Handled = true;
                    break;
                }
                case Key.Space:
                {
                    PasswordInsert(" ", CaretIndex);
                    e.Handled = true;
                    break;
                }
                case Key.Back:
                {
                    PasswordRemove((SelectedText.Length > 0) ? CaretIndex : CaretIndex - 1);
                    e.Handled = true;
                    break;
                }
                case Key.Delete:
                {
                    PasswordRemove(CaretIndex);
                    e.Handled = true;
                    break;
                }
                case Key.V:
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        if (Clipboard.ContainsText())
                        {
                            PasswordInsert(Clipboard.GetText(), CaretIndex);
                            e.Handled = true;
                        }
                    }
                    break;
                }
                case Key.Enter:
                {
                    if (AcceptsReturn)
                    {
                        PasswordInsert("\r", CaretIndex);
                    }
                    break;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (Text.Length != Password.Length)
            {
                if (Text == "") SetPassword("", 0);
                else SyncDisplayText(Password.Length);
            }
        }

        [SecuritySafeCritical]
        private void OnDataPasting(object sender, DataObjectPastingEventArgs e)
        {
            // Abbrechen wenn etwas anderes als Text eingefügt wird
            if (!e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true)) return;

            if (e.SourceDataObject.GetData(DataFormats.UnicodeText) is string text) PasswordInsert(text, CaretIndex);

            e.CancelCommand();
        }

        private void OnPreviewCanExecuteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            // Sicherstellen, dass Password nicht kopiert werden kann, auch nicht wärend Peek
            if (e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Cut || e.Command == ApplicationCommands.Undo)
            {
                e.CanExecute = false;
                e.Handled = true;
            }
        }

        private void SyncDisplayText(int nextCarretIndex)
        {
            if (ShowPasswordButton != null && ShowPasswordButton.IsPressed)
            {
                Text = Password;
            }
            else
            {
                var stringBuilder = new StringBuilder();

                Text = stringBuilder.Append(Enumerable.Repeat(PasswordChar, Password.Length).ToArray()).ToString();
            }

            CaretIndex = Math.Max(nextCarretIndex, 0);
        }

        [SecurityCritical]
        private void PasswordInsert(string text, int index)
        {
            if (text == null) return;

            var newPassword = Password;

            if ((index < 0) || (index > newPassword.Length)) return;

            if (SelectedText.Length > 0) PasswordRemove(index);

            for (int i = 0; i < text.Length; ++i)
            {
                if ((MaxLength == 0) || (newPassword.Length < MaxLength))
                {
                    newPassword = newPassword.Insert(index++, text[i].ToString());
                }
            }
            SetPassword(newPassword, index);
        }

        [SecurityCritical]
        private void PasswordRemove(int index)
        {
            var newPassword = Password;

            if ((index < 0) || (index >= newPassword.Length)) return;

            if (SelectedText.Length > 0)
            {
                for (int i = 0; i < SelectedText.Length; ++i)
                {
                    newPassword = newPassword.Remove(index, 1);
                }

                SetPassword(newPassword, index);
            }
            else
            {
                newPassword = newPassword.Remove(index, 1);

                SetPassword(newPassword, index);
            }
        }
    }
}