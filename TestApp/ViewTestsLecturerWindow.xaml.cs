using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for ViewTestsLecturerWindow.xaml
    /// </summary>
    public partial class ViewTestsLecturerWindow : Window
    {
        TestAppEntities db = new TestAppEntities();
        User user;

        public ViewTestsLecturerWindow(User user)
        {
            InitializeComponent();

            this.user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Module> lecturerModules = db.LecturerAssignments.Where(la => la.Username.Equals(user.Username)).Select(la => la.Module).ToList();

            cmbModule.Items.Clear();
            foreach (var module in lecturerModules)
            {
                cmbModule.Items.Add(module);
            }

            cmbModule.SelectedIndex = 0;
        }

        private void CmbModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayTests();
        }

        private void DisplayTests()
        {
            stckMain.Children.Clear();
            List<Test> tests = ((Module)cmbModule.SelectedItem).Tests.Where(t => t.Username.Equals(user.Username)).OrderBy(t => t.DueDate).ToList();
            if (tests.Any())
            {
                foreach (Test test in tests)
                {
                    Card card = new Card();
                    card.Margin = new Thickness(8, 8, 8, 8);
                    card.Background = (Brush)System.Windows.Application.Current.Resources["PrimaryColorTransparent"];

                    Grid grid = new Grid();
                    grid.Margin = new Thickness(8, 8, 8, 8);

                    TextBlock lblTestTitle = new TextBlock();
                    lblTestTitle.HorizontalAlignment = HorizontalAlignment.Left;
                    lblTestTitle.FontSize = 28;
                    lblTestTitle.Margin = new Thickness(23, 20, 0, 0);
                    lblTestTitle.TextWrapping = TextWrapping.Wrap;
                    lblTestTitle.Text = test.Title;
                    lblTestTitle.VerticalAlignment = VerticalAlignment.Top;
                    lblTestTitle.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];

                    TextBlock lblModule = new TextBlock();
                    lblModule.HorizontalAlignment = HorizontalAlignment.Left;
                    lblModule.FontSize = 13;
                    lblModule.TextWrapping = TextWrapping.Wrap;
                    lblModule.VerticalAlignment = VerticalAlignment.Top;
                    lblModule.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                    lblModule.Text = test.Module.ToString();
                    lblModule.Margin = new Thickness(23, 59, 29, 0);

                    TextBlock lblDueDate = new TextBlock();
                    lblDueDate.HorizontalAlignment = HorizontalAlignment.Left;
                    lblDueDate.FontSize = 13;
                    lblDueDate.TextWrapping = TextWrapping.Wrap;
                    lblDueDate.VerticalAlignment = VerticalAlignment.Top;
                    lblDueDate.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                    lblDueDate.Text = test.DueDate.ToShortDateString();
                    lblDueDate.Margin = new Thickness(23, 80, 29, 40);

                    Button btnViewTest = new Button();
                    btnViewTest.Margin = new Thickness(615, 20, 29, 0);
                    btnViewTest.Height = 34;
                    btnViewTest.Style = (Style)System.Windows.Application.Current.Resources["DarkButton"];
                    btnViewTest.Content = "View Test";
                    btnViewTest.Name = "btnView_" + test.TestID;
                    btnViewTest.Click += btnViewTest_Click;
                    btnViewTest.Padding = new Thickness(8, 8, 8, 8);

                    PackIcon icnEdit = new PackIcon();
                    icnEdit.Margin = new Thickness(652, 20, 62, 0);
                    icnEdit.Height = 40;
                    icnEdit.Name = "icnEdit_" + test.TestID;
                    icnEdit.MouseUp += icnEdit_Click;
                    icnEdit.Kind = PackIconKind.Edit;
                    icnEdit.Cursor = Cursors.Hand;

                    PackIcon icnDelete = new PackIcon();
                    icnDelete.Margin = new Thickness(685, 20, 29, 0);
                    icnDelete.Height = 40;
                    icnDelete.Name = "icnDelete_" + test.TestID;
                    icnDelete.MouseUp += icnDelete_Click;
                    icnDelete.Kind = PackIconKind.Delete;
                    icnDelete.Cursor = Cursors.Hand;

                    grid.Children.Add(lblTestTitle);
                    grid.Children.Add(lblModule);
                    grid.Children.Add(lblDueDate);
                    grid.Children.Add(btnViewTest);
                    grid.Children.Add(icnEdit);
                    grid.Children.Add(icnDelete);

                    card.Content = grid;
                    stckMain.Children.Add(card);
                }
            }
            else
            {
                Card card = new Card();
                card.Margin = new Thickness(8, 8, 8, 8);
                card.Background = (Brush)System.Windows.Application.Current.Resources["PrimaryColorTransparent"];

                Grid grid = new Grid();
                grid.Margin = new Thickness(8, 8, 8, 8);

                TextBlock lblNoTests = new TextBlock();
                lblNoTests.HorizontalAlignment = HorizontalAlignment.Left;
                lblNoTests.FontSize = 24;
                lblNoTests.Margin = new Thickness(30, 20, 0, 30);
                lblNoTests.TextWrapping = TextWrapping.Wrap;
                lblNoTests.Text = "No tests";
                lblNoTests.VerticalAlignment = VerticalAlignment.Top;
                lblNoTests.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];

                grid.Children.Add(lblNoTests);

                card.Content = grid;
                stckMain.Children.Add(card);
            }
        }

        private void icnDelete_Click(object sender, MouseButtonEventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to delete this test?",
                                    "Delete Test",
                                    System.Windows.MessageBoxButton.YesNo);
            if (confirmResult == System.Windows.MessageBoxResult.No) return;

            int testID = Convert.ToInt32(((PackIcon)sender).Name.Substring(10));

            Test test = db.Tests.First(t => t.TestID == testID);

            test.Answers.ToList().ForEach(a => db.Entry(a).State = EntityState.Deleted);
            test.Questions.ToList().ForEach(q => db.Entry(q).State = EntityState.Deleted);

            db.Tests.Remove(test);
            db.SaveChanges();

            DisplayTests();

        }

        private void icnEdit_Click(object sender, MouseButtonEventArgs e)
        {
            int testID = Convert.ToInt32(((PackIcon)sender).Name.Substring(8));
            new EditTestWindow(user, db.Tests.First(t => t.TestID == testID)).Show();
            this.Hide();

        }

        private void btnViewTest_Click(object sender, RoutedEventArgs e)
        {
            int testID = Convert.ToInt32(((Button)sender).Name.Substring(8));
            new ViewTestLecturerWindow(db.Tests.Single(t => t.TestID == testID), user).Show();
            this.Hide();
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

        private void btnNewTest_Click(object sender, RoutedEventArgs e)
        {
            new CreateTestWindow(user).Show();
            this.Hide();
        }

        private void btnTests_Click(object sender, RoutedEventArgs e)
        {
            new ViewTestsLecturerWindow(user).Show();
            this.Hide();
        }
    }
}
