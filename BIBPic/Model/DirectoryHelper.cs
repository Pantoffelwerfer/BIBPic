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
        

        //Get the files from the source folder and save them to the destination folder.
        public void GetFiles()
        {
            string? sourceFilePath = this.OriginFolderPath;
            
            string[] searchPatterns = { "*.jpg", "*.jpeg", "*.png" };
            DirectoryInfo destinationDirectory = this.PrepareDestinationDirectory();

            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceFilePath);

            if (sourceDirectory.Exists)
            {
                foreach (var searchPattern in searchPatterns)
                {
                    foreach (FileInfo fileInfo in sourceDirectory.GetFiles(searchPattern, searchOption: SearchOption.TopDirectoryOnly))
                    {
                    
                        //Get Login from the file name.
                        string fileName = fileInfo.Name; //ToDo: Get the file name from the DB by ID
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
                        //SendExceptionMail(); // ToDo: Send an email to the admin
                        //File.Move(filePath, destSave, overwrite: true);
                        //fileImage.Save(destSave, ImageFormat.Jpeg);
                    
                        fileImage.Dispose();
                    }
                }
            }
        }

        //Get the class names from the excel file.
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
    
        //Create the destination directory if it does not exist.
        public DirectoryInfo PrepareDestinationDirectory()
        {
            string? tempPath = this.TargetFolderPath;
            DirectoryInfo directoryInfo = new DirectoryInfo(tempPath);
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

        //Move the directory to the target folder for the original file if needed.
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
