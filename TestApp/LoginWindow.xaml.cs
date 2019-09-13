﻿using System;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        TestAppEntities db = new TestAppEntities();

        public LoginWindow()
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

            // Background worker used because reading from the database for the first time, especially if it's in the cloud
            // is really slow
            BackgroundWorker bw = new BackgroundWorker();

            //Authenticate and return user
            bw.DoWork += (obj, ev) => AuthenticateUser(username, password, out user);
            //Display error if not found or go to next screen if found
            bw.RunWorkerCompleted += (obj, ev) => OutputAuthentication(user);

            bw.RunWorkerAsync();



        }

        //Display error if not found or go to next screen if found
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

        //Authenticate and return user
        private void AuthenticateUser(string username, string password, out User user)
        {
            user = null;
            foreach (User u in db.Users)
            {
                if (u.Username.ToLower().Equals(username.ToLower()) && BCrypt.CheckPassword(password, u.Password))
                {
                    user = u;
                    break;
                }
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
            this.Hide();
        }
    }
}