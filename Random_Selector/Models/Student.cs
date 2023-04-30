using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random_Selector.Models
{
    public class Student
    {
        public  int ID { get; set; }
        public int Level { get; set; }
        public string FirstName { get; set; }    
        public string LastName { get; set; }
        //public bool IsAssigned { get; set; }


        public override string ToString()
        {
            return $"{ID} {Level} {FirstName} {LastName}";

        }
    }
}
