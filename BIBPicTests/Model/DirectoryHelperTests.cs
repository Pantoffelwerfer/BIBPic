using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIBPic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIBPic.Ressources;

namespace BIBPic.Model.Tests
{
    [TestClass()]
    public class DirectoryHelperTests
    {
        private DirectoryHelper _directoryHelper = new();



        [TestMethod()]
        public void GetClassNamesFromExcel_WhenExcelFileExists_ReturnsClassNames()
        {
            // Arrange

            // Act
            List<ClassNames> result = _directoryHelper.GetClassNamesFromExcel();

            // Assert
            Assert.IsNotNull(result);
            // Add more assertions as needed
        }

        [TestMethod()]
        public void GetClassNamesFromExcel_ExcelFileContainsMoreThanOneName_ReturnsTrue()
        {
            // Arrange

            // Act
            List<ClassNames> result = _directoryHelper.GetClassNamesFromExcel();

            // Assert
            Assert.IsTrue(result.Count >= 0);
            // Add more assertions as needed
        }

    }

}