using Microsoft.Graph.Models.ExternalConnectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BIBPic.Model
{
    internal class DirectoryHelper
    {
        public void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        public void GetFiles(string path, string searchPattern)
        {
            System.IO.Directory.GetFiles(path, searchPattern);
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
