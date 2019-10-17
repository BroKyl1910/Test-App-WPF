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
    /// Interaction logic for ViewTestLecturerWindow.xaml
    /// </summary>
    public partial class ViewTestLecturerWindow : Window
    {
        TestAppEntities db = new TestAppEntities();

        Test test;
        User user;

        public ViewTestLecturerWindow(Test test, User user)
        {
            InitializeComponent();

            this.test = test;
            this.user = user;

            // Display list of results in bound listview. ViewModel used so I can get specific data from the database objects to display
            lstResults.ItemsSource = test.Results.Select(r => new ResultsViewModel()
            {
                User = r.User,
                FirstName = r.User.FirstName,
                Surname = r.User.Surname,
                Result = (double)r.ResultPercentage,
                Test = r.Test
            }).OrderBy(r => r.FirstName);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(user).Show();
            this.Hide();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnViewStudentTest_Click(object sender, RoutedEventArgs e)
        {
            // Show selected learners test
            ResultsViewModel vm = (ResultsViewModel)((Button)sender).DataContext;
            new ViewMemoWindow(user, vm.User, vm.Test).Show();
            this.Hide();
        }

        private void btnViewMemo_Click(object sender, RoutedEventArgs e)
        {
            new ViewMemoLecturerWindow(user, test).Show();
            this.Hide();
        }

        private void btnTests_Click(object sender, RoutedEventArgs e)
        {
            new ViewTestsLecturerWindow(user).Show();
            this.Hide();
        }
    }
}
