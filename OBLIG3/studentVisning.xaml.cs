using Microsoft.EntityFrameworkCore.ChangeTracking;
using OBLIG3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OBLIG3
{
    
    public partial class studentVisning : Window
    {
        private Dat154Context dx { get; set; }
        private readonly LocalView<Student> studenter;
        private readonly LocalView<Course> kurs;
        private readonly LocalView<Grade> karakterer;
        private string soekefelt;
        public studentVisning()
        {
            InitializeComponent();
        }

        public studentVisning(Dat154Context dx, string soekefelt)
        {
            InitializeComponent();
            this.dx = dx;

            studenter = dx.Students.Local;
            kurs = dx.Courses.Local;
            karakterer = dx.Grades.Local;
           
            var filtrerteStudenter = dx.Students.Where(s => s.Studentname.Contains(soekefelt)).OrderBy(s => s.Studentname).ToList();

            studentList.ItemsSource = filtrerteStudenter;

            
           
        }

        
    }
}
