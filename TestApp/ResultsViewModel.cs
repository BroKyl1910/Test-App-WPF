using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class ResultsViewModel
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public User User{ get; set; }
        public Test Test { get; set; }
        public double Result { get; set; }
    }
}
