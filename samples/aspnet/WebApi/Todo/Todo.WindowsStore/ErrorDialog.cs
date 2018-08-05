using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Todo.WindowsStore
{
    public static class ErrorDialog
    {
        public static async Task ShowErrorAsync(string message)
        {
            MessageDialog messageDialog = new MessageDialog(message);
            await messageDialog.ShowAsync();
        }

        public static async Task ShowErrorsAsync(IList<string> errors)
        {
            if (errors.Count > 0)
            {
                await ShowErrorAsync(errors.Aggregate((s, next) => s + '\n' + next));
            }
        }

        public static async Task ShowErrorsAsync(string message, IEnumerable<string> errors)
        {
            List<string> allErrors = new List<string>();
            allErrors.Add(message);
            allErrors.AddRange(errors);
            await ShowErrorsAsync(allErrors);
        }
    }
}
