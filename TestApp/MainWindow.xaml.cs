using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TestAppEntities db = new TestAppEntities();
        BackgroundWorker bw = new BackgroundWorker();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            btnLogin.Visibility = Visibility.Hidden;
            prgLoading.Visibility = Visibility.Visible;

            bw.DoWork += (obj, ev) => AuthenticateUser(username, password, out user);
            bw.RunWorkerCompleted += (obj, ev) => OutputAuthentication(user);

            bw.RunWorkerAsync();



        }

        private void OutputAuthentication(User user)
        {
            if (user == null)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Nope";
            }
            else
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Ok den.";
            }

            prgLoading.Visibility = Visibility.Hidden;
            btnLogin.Visibility = Visibility.Visible;

        }

        private void AuthenticateUser(string username, string password, out User user)
        {
            user = db.Users.FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower()) && u.Password.Equals(password));
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
