using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class AppStudent : AppUser
    {
        public string StudentNumber { get; set; }
        public AppStudent(string studentNumber, string firstName, string surname, string username, UserType userType) : base(firstName, surname, username, userType)
        {
            StudentNumber = studentNumber;
        }

       
    }
}
