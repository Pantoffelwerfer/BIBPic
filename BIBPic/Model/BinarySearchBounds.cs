using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            
            var memory = ms.Length;
            float minScaleFactor = 0.01f; // Lower bound for the scaling factor.
            float maxScaleFactor = 1.0f; // Higher bound for the scaling factor.

            if (memory > targetSize)
            {
                long fileSize = memory;
                while (minScaleFactor <= maxScaleFactor)
                {
                    // Middle of the binary search bounds.
                    float scaleFactor = minScaleFactor + (maxScaleFactor - minScaleFactor) / 2;

                    // Size of the new image.
                    int newWidth = (int)(fileImage.Width * scaleFactor);
                    int newHeight = (int)(fileImage.Height * scaleFactor);

                    // Check if the file size is within the target size.
                    if (Math.Abs(fileSize - targetSize) >= 1024)
                    {
                        // Temporary resized image.
                        Image resizedImage = new Bitmap(newWidth, newHeight);
                        using (Graphics graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.DrawImage(fileImage, 0, 0, newWidth, newHeight);
                        }

                        // Length of the resized image.
                        
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            resizedImage.Save(memoryStream, ImageFormat.Jpeg);
                            fileSize = memoryStream.Length;
                        }

                        // Check if the file size is within the target size.
                        if (Math.Abs(fileSize - targetSize) <= 1024) // Tolerance of 1KB.
                        {
                            return (Bitmap)resizedImage; 
                        }
                        else if (fileSize > targetSize)
                        {
                            // Length of the resized image is too large, adjust the upper bound of the binary search.
                            maxScaleFactor = scaleFactor - 0.01f;
                        }
                        else
                        {
                            // Length of the resized image is too small, adjust the lower bound of the binary search.
                            minScaleFactor = scaleFactor + 0.01f;
                        }
                    }
                    else
                    {
                        
                        break;
                    }
                }
            }
            //DirectoryHelper.SendExceptionMail(fileName);
            // Return the original image if the file size is within the target size.
            return fileImage;
        }

    }
    
}
