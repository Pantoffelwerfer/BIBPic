using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBPic.Model
{
    public class BinarySearchBounds
    {
        private readonly static int targetSize = 100000;
        public static Bitmap SearchBoundsBinary(Bitmap fileImage, string fileName)
        {
            MemoryStream ms = new MemoryStream();
            fileImage.Save(ms, ImageFormat.Jpeg);
            
            var newMemory = ms.Length;
            float minScaleFactor = 0.01f; // Mindestwert für den Skalierungsfaktor
            float maxScaleFactor = 1.0f; // Maximalwert für den Skalierungsfaktor

            while (minScaleFactor <= maxScaleFactor)
            {
                // Mittleren Skalierungsfaktor berechnen
                float scaleFactor = minScaleFactor + (minScaleFactor + maxScaleFactor) / 2;

                // Größe des skalierten Bildes berechnen
                int newWidth = (int)(fileImage.Width * scaleFactor);
                int newHeight = (int)(fileImage.Height * scaleFactor);

                // Überprüfen, ob das skalierte Bild innerhalb der maximalen Größenbeschränkungen liegt
                if (newMemory > targetSize)
                {
                    // Temporäres Bild mit dem skalierten Bild erstellen
                    Image resizedImage = new Bitmap(newWidth, newHeight);
                    using (Graphics graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(fileImage, 0, 0, newWidth, newHeight);
                    }

                    // Dateigröße des skalierten Bildes überprüfen
                    long fileSize;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        resizedImage.Save(memoryStream, ImageFormat.Jpeg);
                        fileSize = memoryStream.Length;
                    }

                    // Überprüfen, ob die Dateigröße nahe bei der Zielgröße liegt
                    if (Math.Abs(fileSize - targetSize) <= 1024) // Toleranz von 1 KB
                    {
                        return (Bitmap)resizedImage; // Rückgabe des skalierten Bildes
                    }
                    else if (fileSize > targetSize)
                    {
                        // Dateigröße ist zu groß, den oberen Bereich der binären Suche ändern
                        maxScaleFactor = scaleFactor - 0.01f;
                    }
                    else
                    {
                        // Dateigröße ist zu klein, den unteren Bereich der binären Suche ändern
                        minScaleFactor = scaleFactor + 0.01f;
                    }
                }
                else
                {
                    // Das Bild überschreitet die maximalen Größenbeschränkungen, die Suche abbrechen
                    break;
                }
            }
            DirectoryHelper.SendExceptionMail(fileName);
            // Rückgabe des Originalbildes, wenn keine passende Größe gefunden wurde
            return fileImage;
        }

    }
    
}
