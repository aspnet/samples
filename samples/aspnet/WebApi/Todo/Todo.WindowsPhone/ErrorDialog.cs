using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Todo.WindowsPhone
{
    public static class ErrorDialog
    {
        public static void ShowError(string message)
        {
            MessageBox.Show(message);
        }

        public static void ShowErrors(IList<string> errors)
        {
            if (errors.Count > 0)
            {
                ShowError(errors.Aggregate((s, next) => s + '\n' + next));
            }
            else
            {
                ShowError("An unknown error has occurred.");
            }
        }

        public static void ShowErrors(string message, IEnumerable<string> errors)
        {
            List<string> allErrors = new List<string>();
            allErrors.Add(message);
            allErrors.AddRange(errors);
            ShowErrors(allErrors);
        }
    }
}
