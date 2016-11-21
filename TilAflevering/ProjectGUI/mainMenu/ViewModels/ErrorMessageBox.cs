using System.Windows;

namespace mainMenu.ViewModels
{
    public class ErrorMessageBox : IMessageBox
    {
        public void OpenMessageBox(string message)
        {
            MessageBox.Show(message);
        }
    }
}