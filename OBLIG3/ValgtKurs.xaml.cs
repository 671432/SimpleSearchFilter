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

    public partial class ValgtKurs : Window
    {
        private Dat154Context dx { get; set; }
        private readonly LocalView<Student> studenter;
        private readonly LocalView<Course> kurs;

        private Course valgtKurs;
        private String valgtkarakter;
        private Boolean stryk;
       
        public ValgtKurs()
        {
            InitializeComponent();
        }
   
        public ValgtKurs(Dat154Context dx, Course kurs)
        {
            InitializeComponent();
            this.dx = dx;
            this.valgtKurs = kurs;
            HentOgVisStudenter();
        }
        public ValgtKurs(Dat154Context dx, String karakter)
        {
            InitializeComponent();
            this.dx = dx;
            this.valgtkarakter = karakter;
            HentOgVisKarakterer();
        }
        public ValgtKurs(Dat154Context dx, Boolean stryk)
        {
            InitializeComponent();
            this.stryk = stryk;
            this.dx = dx;
            HentOgVisKarakterer();
        }

        public void HentOgVisKarakterer()
        {
            if(stryk)
            {
                var karaktererOgStudenterForKurs = dx.Grades
                .Where(g => g.Grade1 == "F")
                .Select(g => new
                {
                    Id = g.Studentid,
                    Studentname = g.Student.Studentname,
                    Studentage = g.Student.Studentage,
                    Grade = g.Grade1,
                    Coursename = g.Coursecode
                    })
                    .ToList();
                deltagerList.ItemsSource = karaktererOgStudenterForKurs;

            }
            else
            {
                var karaktererOgStudenterForKurs = dx.Grades
                    .Where(g => g.Grade1.CompareTo(valgtkarakter) <= 0)
                    .Select(g => new
                    {
                        Id = g.Studentid,
                        Studentname = g.Student.Studentname,
                        Studentage = g.Student.Studentage,
                        Grade = g.Grade1,
                        Coursename = g.Coursecode
                    })
                    .ToList();
                deltagerList.ItemsSource = karaktererOgStudenterForKurs;

            }

        }

        public void HentOgVisStudenter()
        {
            var karaktererOgStudenterForKurs = dx.Grades
                .Where(g => g.Coursecode == valgtKurs.Coursecode)
                .Select(g => new
                {
                    Id = g.Studentid,
                    Studentname = g.Student.Studentname, 
                    Studentage = g.Student.Studentage, 
                    Grade = g.Grade1,
                    Coursename = valgtKurs.Coursename
                })
                .ToList();

            
            deltagerList.ItemsSource = karaktererOgStudenterForKurs;
        }

    }
}
