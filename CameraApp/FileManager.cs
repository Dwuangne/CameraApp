using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraApp
{
    public class FileManager
    {
        public static void SaveImage(Bitmap image, string initialDirectory)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = initialDirectory;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    image.Save(saveFileDialog.FileName);
                }
            }
        }

        public static void OpenFile(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file: {ex.Message}");
            }
        }
    }
}
