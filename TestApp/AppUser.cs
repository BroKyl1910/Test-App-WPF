using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class AppUser
    {
        public AppUser(string firstName, string surname, string username, UserType userType)
        {
            FirstName = firstName;
            Surname = surname;
            Username = username;
            UserType = userType;
        }

        public virtual string FirstName { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Username { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
