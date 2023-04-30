using Random_Selector.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Random_Selector
{
    /// <summary>
    /// Interaction logic for CRUDstudent.xaml
    /// </summary>
    public partial class CRUD : Window
    {
        MainWindow main = new MainWindow();
        IList<Student> students=new List<Student>();
        static  string path = @"C:\Intec\Data\RandomSelecting.txt";
        IList<string> readFile = File.ReadAllLines(path).ToList();
        bool isUpdating = false;
        public CRUD()
        {
            InitializeComponent();
            ReadFile();
            PopulateListBox();

        }
        private void PopulateListBox()
        {
            lstPrint.Items.Clear();
            foreach (var item in students) 
            {
                lstPrint.Items.Add(item);
            }
        }
        public void ReadFile()
        {
            IEnumerable<string>readFile = File.ReadAllLines(path).ToList();
            char[] delimeter = new char[] { ',', ' ', ';', '?' };
            foreach (var line in readFile)
            {
                string[] entries = line.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                Student student = new Student();
                student.ID = int.Parse(entries[0]);
                student.Level = int.Parse(entries[1]);
                student.FirstName = entries[2];
                student.LastName = entries[3];
                students.Add(student);
            }

            }
        private void btnInsert1_Click(object sender, RoutedEventArgs e)
        {
            Student std=new Student();
            if (!string.IsNullOrEmpty(txtLName1.Text))
            {
            std.ID=students.Last().ID+1;
            std.Level = int.Parse(txtLevel1.Text);
            std.FirstName = txtFName1.Text;
            std.LastName=txtLName1.Text;
            InsertStudent(std);
            MessageBox.Show("Your data inserted Successfully..");
            
            }
            else
            {
                MessageBox.Show("Your Text box is empty...");
            }
            ClearTextBox();
        }
        private void InsertStudent(Student newStd)
        {
                using (StreamWriter st = File.AppendText(path))
                {
                    //st.WriteLine();
                    st.WriteLine(newStd.ID.ToString() + ", " + newStd.Level.ToString() + ", " + newStd.FirstName.ToString() + ", " + newStd.LastName.ToString());

                }   
        }

        private void lstPrint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUpdating==false)
            {
            var selectedStudent=lstPrint.SelectedItem as Student;
            txtID1.Text= selectedStudent.ID.ToString();
            txtLevel1.Text= selectedStudent.Level.ToString();
            txtFName1.Text= selectedStudent.FirstName.ToString();
            txtLName1.Text= selectedStudent.LastName.ToString();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Student updatedStudent=new Student();
            updatedStudent.ID=int.Parse(txtID1.Text);
            updatedStudent.Level=int.Parse(txtLevel1.Text);
            updatedStudent.FirstName = txtFName1.Text;
            updatedStudent.LastName = txtLName1.Text;
            StudentUpdate(updatedStudent);            
            MessageBox.Show("Record Updated Succesfully...");
        }

        private void StudentUpdate(Student updatedStudent)
        {
                StringBuilder newLine = new StringBuilder();

                foreach (var item in readFile)
                {
                    string[] entries = item.Split(',');
                    if (entries[0] == updatedStudent.ID.ToString())
                    {
                        string temp = entries[0].Replace(entries[1], updatedStudent.ID.ToString());
                        string temp1 = entries[1].Replace(entries[1], updatedStudent.Level.ToString());
                        string temp2 = entries[2].Replace(entries[2], updatedStudent.FirstName.ToString());
                        string temp3 = entries[3].Replace(entries[3], updatedStudent.LastName.ToString());
                        newLine.Append(temp + ", " + temp1 + ", " + temp2 + ", " + temp3 + "\n");
                    }
                    else
                    {
                        newLine.Append(item.ToString() + "\n");
                    }
                File.WriteAllText(path, newLine.ToString());
            }           
            isUpdating = true;
            ClearTextBox();
        }
        private void ClearTextBox()
        {
            txtID1.Clear();
            txtLevel1.Clear();
            txtFName1.Clear();
            txtLName1.Clear();
            txtLevel1.Clear();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            students.Clear();
            ReadFile();
            PopulateListBox();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Student std=new Student();
            var selectedStudent = lstPrint.SelectedItem as Student;
            std.ID = selectedStudent.ID;
            DeleteStudent(std);
        }
        public void DeleteStudent(Student student)
        {
            StringBuilder newLine = new StringBuilder();

            foreach (var item in readFile)
            {
                string[] entries = item.Split(',');
                if (entries[0] == student.ID.ToString())
                {
                    File.Delete(entries[0]);
                    File.Delete(entries[1]);
                    File.Delete(entries[2]);
                    File.Delete(entries[3]);
                   
                }
                else
                {
                    
                    newLine.Append(item.ToString() + "\n");
                }               
            }
            File.WriteAllText(path, newLine.ToString());
            isUpdating = true;
            ClearTextBox();
           
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        { 
            main.Show();
            
        }
    }
}
