using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIBPic.Model
{
    public class FileExplorerService
    {
        public string OpenFileDialog()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Wählen Sie einen Ordner aus.";
                dialog.RootFolder = Environment.SpecialFolder.Desktop;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }

            return null;
        }
    }
}
