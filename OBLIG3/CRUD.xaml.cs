using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client.NativeInterop;
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
    /// <summary>
    /// Interaction logic for CRUD.xaml
    /// </summary>
    public partial class CRUD : Window
    {
        public Dat154Context dx {  get; set; } //Denne brukes for å bruke delt cache
        private readonly LocalView<Course> kurs;
        private Boolean error = false;
        private Course kursValg;
        public CRUD()
        {
            InitializeComponent();
        }
        public void InitializeDataContext()
        {
            kursListe.DataContext = dx.Courses;
            
        }

        public CRUD(Dat154Context sharedContext)
        {
            InitializeComponent();
            dx = sharedContext;
            dx.Courses.Load(); 
            kurs = dx.Courses.Local; 
            kursListe.ItemsSource = kurs.OrderBy(k => k.Coursename); 
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Add student
        {
            Student s; Grade g;
            if(sname != null && sage != null && sid != null && skarakter != null && kursValg != null)
            {
                int id = int.Parse(sid.Text);
                if (!dx.Students.Any(s => s.Id == id))
                {
                        s = new()
                    {
                        Studentname = sname.Text,
                        Studentage = int.Parse(sage.Text),
                        Id = int.Parse(sid.Text)
                    };

                        g = new()
                    {
                        Studentid = id,
                        Coursecode = kursValg.Coursecode,
                        Grade1 = skarakter.Text
                    };
                    MessageBox.Show(s.Studentname + " opprettet! Lagt til i " + g.Coursecode);
                    
                    try
                    {
                        dx.Students.Add(s);
                        dx.Grades.Add(g);
                        dx.SaveChanges();
                        error = false;
                    } catch(Exception ex){}

                }
                else
                {
                    errorText.Text = "Denne IDen eksisterer allerede!";
                    errorText.Foreground = new SolidColorBrush(Colors.Red);
                    error = true;
                    errorText.Visibility = Visibility.Visible;
                }


                sname.Text = sage.Text = sid.Text =skarakter.Text = null; //For å blanke ut 
            }
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) //Delete student, kan ikke slette studenter knyttet til et fag enda
        {
            if(int.TryParse(sid.Text, out int id))
            {

                Student? s = dx.Students.Where(sid => sid.Id == id).FirstOrDefault();

                if (s != null)
                {
                    var karakterer = dx.Grades.Where(k => k.Studentid == id).ToList();
                    foreach (var karakter in karakterer)
                    {
                        dx.Grades.Remove(karakter);
                    }
                    dx.Students.Remove(s);
                    dx.SaveChanges();
                }

                sname.Text = sage.Text = sid.Text = null;
            } else
            {
                MessageBox.Show("Du må angi et studentnummer");
            }

            
        }


        



        private void infoText_GotFocus(object sender, RoutedEventArgs e)
        {
  
        }

        private void sid_GotFocus(object sender, RoutedEventArgs e)
        {
            if(error)
            {
                errorText.Text = "";
                errorText.Foreground = new SolidColorBrush(Colors.Red);
                error = false;
                errorText.Visibility = Visibility.Collapsed;
            }
        }

        private void kursListe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            Course kurs = comboBox.SelectedItem as Course;

            if (kurs != null) this.kursValg = kurs;
        }
    }
}
