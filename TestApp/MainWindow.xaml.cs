﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User user;
     

        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;


            lblWelcome.Text = "Welcome, " + user.FirstName + " " + user.Surname;
        }

        private void BtnTests_Click(object sender, RoutedEventArgs e)
        {
            if (user.UserType == (int)UserType.LECTURER)
            {
                new ViewTestsLecturerWindow(user).Show();
            }
            else
            {
                new ViewTestsStudentWindow(user).Show();
            }
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new LoginWindow().Show();
        }
    }
}
