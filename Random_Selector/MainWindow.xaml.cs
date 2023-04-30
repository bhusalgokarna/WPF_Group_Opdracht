using Random_Selector.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Random_Selector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IList<string> readLine = new List<string>();
        public IList<Student> studentsMet1 = new List<Student>();
        public IList<Student> studentsAnders = new List<Student>();
        public IList<Student>students = new List<Student>();
        public string path = @"C:\Intec\Data\RandomSelecting.txt";
      
        public MainWindow()
        {
            InitializeComponent();
            GetOrCreateFile();
            GetAllStudents();
            PopulateLstBox();
        }
        private async void GetOrCreateFile()
        {
            Task task = Task.Run(() => {

            if (File.Exists(path))
            {
                return;
            }
            else
            {
                File.Create(path);
                MessageBox.Show($"File has been created on {File.GetCreationTime(path)}");
            }
        }); await task;
        }
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Group is making....");
            MessageBox.Show($"Group of {txtGroupStudent.Text} students");
            if (!string.IsNullOrEmpty(txtGroupStudent.Text)) 
            {
                GenerateGroup();

            }
            else
            {
                MessageBox.Show("Give the correct number to make group..");
            }
        }
        private void GetAllStudents()
        {
                readLine = File.ReadAllLines(path).ToList();
                char[] delimeter = new char[] { ',', ' ', ';', '?' };
                foreach (var line in readLine)
                {

                    string[] entries = line.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                    Student student = new Student();
                    student.ID = int.Parse(entries[0]);
                    student.Level = int.Parse(entries[1]);
                    student.FirstName = entries[2];
                    student.LastName = entries[3];
                    if (student.Level == 1)
                    {
                        studentsMet1.Add(student);
                        students.Add(student);
                    }
                    else
                    {
                        studentsAnders.Add(student);
                        students.Add(student);
                    }
                }
            Task.Delay(1000);
        }
        private void GenerateGroup()
        {
           
              IList<Student> SelectedStudents = new List<Student>();
              int numberOfstudents = int.Parse(txtGroupStudent.Text);
               Random random1 = new Random();
            if (studentsMet1.Count>=1 && studentsAnders.Count>=numberOfstudents-1)
            {
                int stdIndex = random1.Next(1, studentsMet1.Count);
                SelectedStudents.Add(studentsMet1[stdIndex]);
                studentsMet1.Remove(studentsMet1[stdIndex]);
                numberOfstudents--;

                for (int i = 0; i < numberOfstudents; i++)
                {
                    stdIndex = random1.Next(0, studentsAnders.Count);
                    for (int j = 0; j < studentsAnders.Count; j++)
                    {
                        if (studentsAnders[stdIndex] == studentsAnders[j])
                        {
                            SelectedStudents.Add(studentsAnders[j]);
                            studentsAnders.Remove(studentsAnders[j]);
                        }
                    }
                }
            }
            else
            {
                if (MessageBox.Show("We dont have enogh students to make group, do you still want the last group?") == MessageBoxResult.Yes)
                {
                    lstGroup.Items.Clear();
                    foreach (var item in studentsMet1)
                    {
                        lstGroup.Items.Add(item);
                        lstAllStudents.Items.Remove(item);
                    }
                    
                    foreach (var item in studentsAnders)
                    {
                        lstGroup.Items.Add(item);
                        lstAllStudents.Items.Remove(item);
                    }
                    FillLstGroup(SelectedStudents);
                }
            }
            lstGroup.ItemsSource = SelectedStudents;
        }

        private void FillLstGroup(IList<Student>selected)
        {   
            lstGroup.Items.Clear();
            foreach (var student in selected)
            {
                lstGroup.Items.Add(student);
                lstAllStudents.Items.Remove(student);
            }
            txtGroupStudent.Clear();
        }
         private void btmKRUD_Click(object sender, RoutedEventArgs e)
        {
            CRUD student = new CRUD();
            student.Show();
        }
        private async void btnFillListBox_Click(object sender, RoutedEventArgs e)
        { 
            if (!lstAllStudents.Items.IsEmpty)
            {
                MessageBox.Show("ListBox is already filled!");
            }
            else
            {
                Task task = new Task(GetAllStudents);
                task.Start();
                MessageBox.Show("Reading the File..........wait 5 second");
                await task;
                PopulateLstBox();
            }
        }
        private void PopulateLstBox()
        {
            lstAllStudents.Items.Clear();
            foreach (var student in students)
            {
                lstAllStudents.Items.Add(student);
            }

        }

        private void btmRefresh_Click(object sender, RoutedEventArgs e)
        {
            students.Clear();
            GetAllStudents();
            PopulateLstBox();
        }
    }
}
