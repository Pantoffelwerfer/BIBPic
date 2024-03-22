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
using BIBPic.ViewModel;

namespace BIBPic.Model
{
    public class DirectoryHelper
    {
        public string OriginFolderPath { get; set; }
        public string TargetFolderPath { get; set; }
        

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

            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceFilePath);

            if (sourceDirectory.Exists)
            {
                foreach (FileInfo fileInfo in sourceDirectory.GetFiles(searchPattern, searchOption: SearchOption.TopDirectoryOnly))
                {
                    
                    //Get Login from the file name.
                    string fileName = fileInfo.Name;
                    string login = fileInfo.Name.Replace(fileInfo.Extension, "");

                    Student studentName; //ToDo: Search for the user in the DB

                    string destSave = destinationDirectory.FullName+ "\\" +fileName;
                    string filePath = Path.Combine(sourceFilePath, fileName);
                    Bitmap fileImage = new Bitmap(filePath);

                    if (login == null)
                    {
                        SendExceptionMail(fileName);
                        continue;
                    }


                    var newImage = BinarySearchBounds.SearchBoundsBinary(fileImage, fileName);
                    newImage.Save(destSave, ImageFormat.Jpeg);
                            newImage.Dispose();
                            fileImage.Dispose();
                            //SendExceptionMail();
                        //File.Move(filePath, destSave, overwrite: true);
                     //fileImage.Save(destSave, ImageFormat.Jpeg);
                    
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
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // ClassNames.xlsx only has one worksheet.

                        int rowCount = worksheet.Dimension.Rows;

                        // First class name is in the second row.
                        for (int row = 2; row <= rowCount; row++)
                        {
                            string className = worksheet.Cells[row, 1].Value?.ToString(); // Read the class name from the first column.

                            if (!string.IsNullOrEmpty(className))
                            {
                                classNames.Add(new ClassNames(className)); // ClassNames added to the list.
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
            message.From = new MailAddress("exampleMail.com");
            message.Body += string.Format("Prozess von {0} fehlgeschlagen! ", fileName);
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("username", "password");
            smtp.EnableSsl = true;
            smtp.Send(message);
        }
    }
}
