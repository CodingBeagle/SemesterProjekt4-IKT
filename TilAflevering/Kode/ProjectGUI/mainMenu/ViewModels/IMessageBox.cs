using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;

namespace mainMenu.ViewModels
{
    public interface IMessageBox
    {
        void OpenMessageBox(string message);
    }
}