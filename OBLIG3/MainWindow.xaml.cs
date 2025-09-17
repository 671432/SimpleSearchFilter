using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OBLIG3.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OBLIG3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dat154Context dx = new(); //DBContext
        
        private readonly LocalView<Student> studenter;
        private readonly LocalView<Course> kurs;
        private readonly LocalView<Grade> karakterer; 
       

        public MainWindow()
        {
            InitializeComponent();

            studenter = dx.Students.Local; //Laster de ned lokalt(cache)
            dx.Students.Load();

            kurs = dx.Courses.Local;
            dx.Courses.Load();

            karakterer = dx.Grades.Local;
            dx.Grades.Load();

            kursListe.ItemsSource = kurs.OrderBy(k => k.Coursename);

            karakterListe.ItemsSource = new List<string> {"A", "B", "C", "D", "E", "F", "Alle med stryk" };

            //karakterListe.ItemsSource = karakterer.OrderBy(k => k.Grade1);
        }

        private void soekeKnapp_Click(object sender, RoutedEventArgs e)
        {
            studentVisning studentvisning = new(dx, soekeFelt.Text);
            studentvisning.Show();
        }

        private void CRUD_Click(object sender, RoutedEventArgs e)
        {
            CRUD crud = new(dx);
            crud.Show();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            Course valgtKurs = comboBox.SelectedItem as Course;

            if(valgtKurs != null)
            {
                ValgtKurs vK = new(dx, valgtKurs);
                vK.Show();
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            var valgtKarakter = comboBox.SelectedItem as String;

            if(valgtKarakter !=null && valgtKarakter == "Alle med stryk")
            {
                ValgtKurs vK = new(dx, true);
                vK.Show();
            }else 
            {
                ValgtKurs vK = new(dx, valgtKarakter);
                vK.Show();
            }
        }
    }
}