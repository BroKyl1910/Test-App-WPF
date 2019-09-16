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

namespace TestApp
{
    /// <summary>
    /// Interaction logic for TakeTestWindow.xaml
    /// </summary>
    public partial class TakeTestWindow : Window
    {
        User student;
        Test test;
        List<Answer> answers;

        public TakeTestWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblTestTitle.Text = test.User.FirstName + test.User.Surname;
        }
    }
}
