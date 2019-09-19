using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for ViewTestsWindow.xaml
    /// </summary>
    public partial class ViewTestsWindow : Window
    {
        TestAppEntities db = new TestAppEntities();
        User user;

        public ViewTestsWindow(User user)
        {
            InitializeComponent();

            this.user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            List<Course> studentCourses = user.StudentAssignments.Select(sa => sa.Course).ToList();
            List<Module> studentModules = studentCourses.SelectMany(sc => sc.ModuleCourses.Select(mc => mc.Module)).ToList();

            cmbModule.Items.Clear();
            foreach (var module in studentModules)
            {
                cmbModule.Items.Add(module);
            }

            cmbModule.SelectedIndex = 1;

        }

        private void CmbModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayTests();
        }

        private void DisplayTests()
        {
            stckMain.Children.Clear();
            List<Test> tests = ((Module)cmbModule.SelectedItem).Tests.OrderBy(t=>t.DueDate).ToList();
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

                    Button btnAction = new Button();
                    btnAction.Margin = new Thickness(612, 20, 10, 0);
                    btnAction.Height = 34;
                    bool takenTest = db.Results.Any(r => r.TestID == test.TestID && r.Username.Equals(user.Username));
                    if (takenTest)
                    {
                        btnAction.Style = (Style)System.Windows.Application.Current.Resources["FlatButton"];
                        btnAction.Content = "View Result";
                        btnAction.Name = "btn"+test.TestID;
                        btnAction.Click += BtnViewResult_Click;
                    }
                    else
                    {
                        btnAction.Style = (Style)System.Windows.Application.Current.Resources["DarkButton"];
                        btnAction.Content = "Take Test";
                        btnAction.Name = "btn" + test.TestID;
                        btnAction.Click += BtnTakeTest_Click;
                    }

                    grid.Children.Add(lblTestTitle);
                    grid.Children.Add(lblModule);
                    grid.Children.Add(lblDueDate);
                    grid.Children.Add(btnAction);

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

        private void BtnTakeTest_Click(object sender, RoutedEventArgs e)
        {
            int testID = Convert.ToInt16(((Button)sender).Name.Substring(3));
            new TakeTestWindow(user, db.Tests.First(t => t.TestID == testID)).Show();
            this.Hide();
        }

        private void BtnViewResult_Click(object sender, RoutedEventArgs e)
        {
            int testID = Convert.ToInt16(((Button)sender).Name.Substring(3));
            new ViewMemoWindow(user, db.Tests.First(t=>t.TestID==testID)).Show();
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
    }
}
