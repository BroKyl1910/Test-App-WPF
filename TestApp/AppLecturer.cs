using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class AppLecturer : AppUser
    {
        public string LecturerCode { get; set; }

        public AppLecturer(string lecturerCode, string firstName, string surname, string username, UserType userType) : base(firstName, surname, username, userType)
        {
            LecturerCode = lecturerCode;
        }
    }
}
