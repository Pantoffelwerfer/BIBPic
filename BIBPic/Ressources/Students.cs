using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBPic.Ressources
{
    internal class Students
    {
        //Properties of the Students class.
        public string Name { get; set; }
        public string Surname { get; set; }
        public int StudentID { get; set; }

        //Constructor of the Students class.
        public Students(string name, string surname, int studentID)
        {
            Name = name;
            Surname = surname;
            StudentID = studentID;
        }

    }
}
