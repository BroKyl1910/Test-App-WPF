using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        TestAppEntities db = new TestAppEntities();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            //    BackgroundWorker bw = new BackgroundWorker();
            //    bw.DoWork += (obj, ev) => RegisterUser();
            //    bw.RunWorkerCompleted += (obj, ev) =>
            //    {
            //        prgLoading.Visibility = Visibility.Hidden;
            //        btnRegister.Visibility = Visibility.Visible;
            //    };
            //    prgLoading.Visibility = Visibility.Visible;
            //    btnRegister.Visibility = Visibility.Hidden;
            //    bw.RunWorkerAsync();

            RegisterUser();

        }

        private void RegisterUser()
        {
            if (ValidateForm())
            {
                User user = new User();
                user.Username = txtUsername.Text;
                user.Password = HashPassword(txtPassword.Password);
                user.FirstName = txtFirstName.Text;
                user.Surname = txtSurname.Text;
                user.UserType = cmbUserType.SelectedIndex;
                user.UniversityIdentification = txtIdentification.Text;

                db.Users.Add(user);
                db.SaveChanges();

                MessageBox.Show("User Registered");

                //Show next page
            }
        }

        private bool ValidateForm()
        {
            //Check all fields are filled
            if (!AllFieldsFilled())
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please complete all fields";
                return false;
            }

            //Check username isn't already used
            if (db.Users.Any(u => u.Username.ToLower().Equals(txtUsername.Text.ToLower())))
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Username already taken";
                return false;
            }

            //Check Identification isn't used
            if (db.Users.Any(u => u.UniversityIdentification.ToLower().Equals(txtIdentification.Text.ToLower())))
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Identification number already used";
                return false;
            }

            //Password must be at least 8 characters and have 1 uppercase and 1 lowercase and 1 digit
            char[] passwordChars = txtPassword.Password.ToCharArray();
            if (!(passwordChars.Length >= 8 && passwordChars.Where(c => Char.IsUpper(c)).ToList().Count >= 1 && passwordChars.Where(c => Char.IsLower(c)).ToList().Count >= 1 && passwordChars.Where(c => Char.IsDigit(c)).ToList().Count >= 1))
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Password must be at least 8 characters and contain 1 uppercase character, 1 lowercase character and 1 digit";
                return false;
            }

            //Check passwords match
            if (!txtPassword.Password.Equals(txtConfirmPassword.Password))
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Passwords do not match";
                return false;
            }


            return true;
        }

        private bool AllFieldsFilled()
        {
            return !(txtFirstName.Text.Equals("") || txtSurname.Text.Equals("") || txtUsername.Text.Equals("") || txtPassword.Password.Equals("") || txtConfirmPassword.Password.Equals("") || cmbUserType.SelectedIndex == -1 || txtIdentification.Text.Equals(""));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbUserType.Items.Add("Student");
            cmbUserType.Items.Add("Lecturer");
            cmbUserType.SelectedIndex = 0;
            txtFirstName.Focus();
        }

        private void CmbUserType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbUserType.SelectedIndex == 0)
            {
                lblIdentificationType.Text = "Student Number";
            } else if (cmbUserType.SelectedIndex == 1)
            {
                lblIdentificationType.Text = "Lecturer Code";
            }
        }

        private string HashPassword(string password)
        {
            string salt = BCrypt.GenerateSalt();
            string hash = BCrypt.HashPassword(password, salt);
            return hash;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
