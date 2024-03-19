using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIBPic.Ressources;

namespace BIBPic.ViewModel
{
    internal class MainViewModel
    {
		private static List<ClassNames> _classNames;

		public static List<ClassNames> ClassNamesList
		{
			get { return _classNames; }
			set { _classNames = value; }
		}

		private List<Students> _students;

		public List<Students> Students
		{
			get { return _students; }
			set { _students = value; }
		}


	}
}
