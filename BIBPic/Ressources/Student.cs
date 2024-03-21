using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBPic.Ressources
{
    public class Student
    {
        //Properties of the Student class.
        public string Name { get; set; }
        public string Surname { get; set; }
        public int StudentID { get; set; }
        public string ClassID { get; set; }

        //Constructor of the Student class.
        public Student(string name, string surname, string classID, int studentID)
        {
            Name = name;
            Surname = surname;
            StudentID = studentID;
            ClassID = classID;
        }

    }
}
