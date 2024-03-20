using Microsoft.Graph.Models.ExternalConnectors;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BIBPic.Ressources;
using OfficeOpenXml;
using System.Reflection;

namespace BIBPic.Model
{
    internal class DirectoryHelper
    {
        public string OriginFolderPath { get; private set; }
        public string TargetFolderPath { get; private set; }

        public void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        public void GetFiles()
        {
            string? sourceFilePath = this.OriginFolderPath;
            string searchPattern = "*.jpg";
            DirectoryInfo destinationDirectory = this.PrepareDestinationDirectory();

#pragma warning disable CS8604 // Possible null reference argument.
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceFilePath);

            if (sourceDirectory.Exists)
            {
                foreach (FileInfo fileInfo in sourceDirectory.GetFiles(searchPattern, searchOption: SearchOption.TopDirectoryOnly))
                {
                    
                    //Get Login from the file name.
                    string fileName = fileInfo.Name;
                    string login = fileInfo.Name.Replace(fileInfo.Extension, "");

                    Student studentName; //ToDo: Search for the user in the DB

                    string destSave = destinationDirectory.FullName + fileName;
                    string filePath = Path.Combine(sourceFilePath, fileName);
                    Bitmap fileImage = new Bitmap(filePath);

                    if (login == null)
                    {
                        SendExceptionMail(fileName);
                        continue;
                    }


                    var newImage = BinarySearchBounds.SearchBoundsBinary(fileImage, fileName);
                            newImage.Dispose();
                            fileImage.Dispose();
                            //SendExceptionMail();
                        File.Move(filePath, destSave, overwrite: true);
                     fileImage.Save(destSave, ImageFormat.Jpeg);
                    
                    fileImage.Dispose();
                }
            }
        }

        public List<ClassNames> GetClassNamesFromExcel()
        {
            string filePath = "BIBPic.Ressources.ClassNames.xlsx";
            List<ClassNames> classNames = new List<ClassNames>();

            // Prüfen, ob die Datei existiert
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filePath))
            {
                if (stream != null)
                {
                    // Verwendung der EPPlus-Bibliothek zum Lesen der Excel-Datei
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Annahme: Die Klassennamen befinden sich im ersten Arbeitsblatt

                        int rowCount = worksheet.Dimension.Rows;

                        // Annahme: Die Klassennamen befinden sich in der ersten Spalte (Spalte A)
                        for (int row = 2; row <= rowCount; row++)
                        {
                            string className = worksheet.Cells[row, 1].Value?.ToString(); // Lesen des Klassennamens aus der ersten Spalte

                            if (!string.IsNullOrEmpty(className))
                            {
                                classNames.Add(new ClassNames(className)); // Ausgabe des Klassennamens (Sie können es speichern, wie Sie möchten)
                            }
                        }
                    }
                }
                else
                {
                    //Todo : Log the error
                }
            }
            return classNames;
        }
    

        public DirectoryInfo PrepareDestinationDirectory()
        {
            string? tempPath = this.TargetFolderPath;
#pragma warning disable CS8604 // Possible null reference argument.
            DirectoryInfo directoryInfo = new DirectoryInfo(tempPath);
#pragma warning restore CS8604 // Possible null reference argument.
            try
            {
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }
            catch (ArgumentNullException ex)
            {
                //Log.Error($"Argument is null or empty: {ex.Message}");
            }

            return directoryInfo;
        }

        public void MoveDirectory(string sourcePath, string destinationPath)
        {
            if (System.IO.Directory.Exists(sourcePath))
            {
                System.IO.Directory.Move(sourcePath, destinationPath);
            }
        }

        public static void SendExceptionMail(string fileName)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            //TODO: Add the email address to send the exception mail

            message.Subject = $"Fotoupload fehlgeschlagen!";
            message.From = new MailAddress("dennis.razlaw@edu.bib.de");
            message.Body += string.Format("Prozess von {0} fehlgeschlagen! ", fileName);
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("username", "password");
            smtp.EnableSsl = true;
            smtp.Send(message);
        }
    }
}
