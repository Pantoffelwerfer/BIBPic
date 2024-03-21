using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIBPic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BIBPic.Model.Tests
{
    [TestClass()]
    public class BinarySearchBoundsTests
    {
        

        [TestMethod()]
        public void SearchBoundsBinaryTest()
        {
            
            

            Assert.Fail();
        }

        private static Bitmap CreateBitmapOfSize(int width, int height)
        {
            return new Bitmap(width, height);
        }

        [TestMethod()]
        public void ImageUnderTargetSize_ReturnsSameImage()
        {
            // Arrange
            Bitmap image = CreateBitmapOfSize(100, 100);

            // Act
            Bitmap result = BinarySearchBounds.SearchBoundsBinary(image, "TestImage.jpg");

            // Assert
            Assert.AreSame(image, result);
        }

        [TestMethod()]
        public void ImageAlreadyAtTargetSize_ReturnsSameImage()
        {
            // Arrange
            Bitmap image = CreateBitmapOfSize(200, 200);

            // Act
            Bitmap result = BinarySearchBounds.SearchBoundsBinary(image, "TestImage.jpg");

            // Assert
            Assert.AreSame(image, result);
        }

        [TestMethod()]
        public void ImageAboveTargetSize_ScalesImage()
        {
            // Arrange
            Bitmap image = CreateBitmapOfSize(1000, 1000);

            // Act
            Bitmap result = BinarySearchBounds.SearchBoundsBinary(image, "TestImage.jpg");

            // Assert
            Assert.AreNotSame(image, result);
            Assert.IsTrue(result.Width < image.Width && result.Height < image.Height);
        }
    }
}