using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LogManager
{
    public class TimeMaskedTextBox : TextBox
    {
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(
                "Time",
                typeof(string),
                typeof(TimeMaskedTextBox),
                new PropertyMetadata("00:00:00", OnTimeChanged));

        public string Time
        {
            get => (string)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public TimeMaskedTextBox()
        {
            PreviewTextInput += OnPreviewTextInput;
            PreviewKeyDown += OnPreviewKeyDown;
            TextChanged += OnTextChanged;
            Text = "00:00:00";
        }

        private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimeMaskedTextBox textBox)
            {
                textBox.Text = e.NewValue.ToString();
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
                return;
            }

            var text = Text;
            var caretIndex = CaretIndex;

            // Пропускаем двоеточия
            if (caretIndex == 2 || caretIndex == 5)
            {
                caretIndex++;
            }

            // Создаем новую строку с введенной цифрой
            var newText = text.Substring(0, caretIndex) + e.Text + text.Substring(caretIndex + 1);
            
            // Проверяем валидность времени
            if (IsValidTimeFormat(newText))
            {
                Text = newText;
                CaretIndex = caretIndex + 1;
            }
            
            e.Handled = true;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                var text = Text;
                var caretIndex = CaretIndex;

                // Пропускаем двоеточия при удалении
                if (caretIndex == 3 || caretIndex == 6)
                {
                    caretIndex--;
                }

                if (caretIndex > 0)
                {
                    var newText = text.Substring(0, caretIndex - 1) + "0" + text.Substring(caretIndex);
                    if (IsValidTimeFormat(newText))
                    {
                        Text = newText;
                        CaretIndex = caretIndex - 1;
                    }
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Delete)
            {
                var text = Text;
                var caretIndex = CaretIndex;

                // Пропускаем двоеточия при удалении
                if (caretIndex == 2 || caretIndex == 5)
                {
                    caretIndex++;
                }

                if (caretIndex < text.Length)
                {
                    var newText = text.Substring(0, caretIndex) + "0" + text.Substring(caretIndex + 1);
                    if (IsValidTimeFormat(newText))
                    {
                        Text = newText;
                        CaretIndex = caretIndex;
                    }
                    e.Handled = true;
                }
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsValidTimeFormat(Text))
            {
                Time = Text;
            }
        }

        private bool IsValidTimeFormat(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length != 8)
                return false;

            if (text[2] != ':' || text[5] != ':')
                return false;

            if (!int.TryParse(text.Substring(0, 2), out int hours) || hours > 23)
                return false;

            if (!int.TryParse(text.Substring(3, 2), out int minutes) || minutes > 59)
                return false;

            if (!int.TryParse(text.Substring(6, 2), out int seconds) || seconds > 59)
                return false;

            return true;
        }
    }
} 