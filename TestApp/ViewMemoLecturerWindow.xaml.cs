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
    /// Interaction logic for LecturerViewMemoWindow.xaml
    /// </summary>
    
    // Use of a separate window is justified here in my opinion as there is quite a few different things shown on this window compared to the other memo window
    public partial class ViewMemoLecturerWindow : Window
    {
        User lecturer;
        Test test;
        public ViewMemoLecturerWindow(User lecturer, Test test)
        {
            InitializeComponent();

            this.lecturer = lecturer;
            this.test = test;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            int questionNumber = 1;

            lblTestTitle.Text = test.Title;
            lblDueDate.Text = test.DueDate.ToShortDateString();
            lblModule.Text = test.Module.ToString();


            //Make card foreach answer
            foreach (Question question in test.Questions)
            {
                Card card = new Card();
                card.Margin = new Thickness(8, 8, 8, 8);
                card.Background = (Brush)System.Windows.Application.Current.Resources["PrimaryColorTransparent"];

                Grid grid = new Grid();
                grid.Margin = new Thickness(8, 8, 8, 8);

                TextBlock lblQuestionNumber = new TextBlock();
                lblQuestionNumber.HorizontalAlignment = HorizontalAlignment.Left;
                lblQuestionNumber.FontSize = 28;
                lblQuestionNumber.Margin = new Thickness(23, 20, 0, 0);
                lblQuestionNumber.TextWrapping = TextWrapping.Wrap;
                lblQuestionNumber.Text = "Question " + questionNumber;
                lblQuestionNumber.VerticalAlignment = VerticalAlignment.Top;
                lblQuestionNumber.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];

                TextBlock lblQuestion = new TextBlock();
                lblQuestion.HorizontalAlignment = HorizontalAlignment.Left;
                lblQuestion.FontSize = 20;
                lblQuestion.TextWrapping = TextWrapping.Wrap;
                lblQuestion.VerticalAlignment = VerticalAlignment.Top;
                lblQuestion.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                lblQuestion.Text = question.QuestionText;
                lblQuestion.Margin = new Thickness(23, 72, 23, 0);

                Label lblAnswers = new Label();
                lblAnswers.FontSize = 13;
                lblAnswers.HorizontalAlignment = HorizontalAlignment.Left;
                lblAnswers.VerticalAlignment = VerticalAlignment.Top;
                lblAnswers.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                lblAnswers.Margin = new Thickness(23, 131, 0, 0);
                lblAnswers.Content = "Answers";

                RadioButton rdioA = new RadioButton();
                rdioA.HorizontalAlignment = HorizontalAlignment.Left;

                rdioA.VerticalAlignment = VerticalAlignment.Top;
                rdioA.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                rdioA.Content = "A.";
                rdioA.Margin = new Thickness(32, 174, 0, 0);

                RadioButton rdioB = new RadioButton();
                rdioB.HorizontalAlignment = HorizontalAlignment.Left;

                rdioB.VerticalAlignment = VerticalAlignment.Top;
                rdioB.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                rdioB.Content = "B.";
                rdioB.Margin = new Thickness(32, 222, 0, 0);

                RadioButton rdioC = new RadioButton();
                rdioC.HorizontalAlignment = HorizontalAlignment.Left;

                rdioC.VerticalAlignment = VerticalAlignment.Top;
                rdioC.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                rdioC.Content = "C.";
                rdioC.Margin = new Thickness(32, 269, 0, 0);

                TextBlock lblA = new TextBlock();
                lblA.HorizontalAlignment = HorizontalAlignment.Left;

                lblA.TextWrapping = TextWrapping.Wrap;
                lblA.VerticalAlignment = VerticalAlignment.Top;
                lblA.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                lblA.Text = question.Answer1;
                lblA.Margin = new Thickness(82, 176, 0, 0);

                TextBlock lblB = new TextBlock();
                lblB.HorizontalAlignment = HorizontalAlignment.Left;

                lblB.TextWrapping = TextWrapping.Wrap;
                lblB.VerticalAlignment = VerticalAlignment.Top;
                lblB.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                lblB.Text = question.Answer2;
                lblB.Margin = new Thickness(82, 224, 0, 0);

                TextBlock lblC = new TextBlock();
                lblC.HorizontalAlignment = HorizontalAlignment.Left;

                lblC.TextWrapping = TextWrapping.Wrap;
                lblC.VerticalAlignment = VerticalAlignment.Top;
                lblC.Foreground = (Brush)System.Windows.Application.Current.Resources["AccentGreyColor"];
                lblC.Text = question.Answer3;
                lblC.Margin = new Thickness(82, 271, 0, 0);

                Label lblCorrectAnswer = new Label();
                lblCorrectAnswer.HorizontalAlignment = HorizontalAlignment.Left;
                lblCorrectAnswer.VerticalAlignment = VerticalAlignment.Top;
                lblCorrectAnswer.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                lblCorrectAnswer.Margin = new Thickness(23, 299, 0, 9);
                lblCorrectAnswer.Content = "Correct answer: " + new char[] { 'A', 'B', 'C' }[question.CorrectAnswer];

                grid.Children.Add(lblQuestionNumber);
                grid.Children.Add(lblQuestion);
                grid.Children.Add(lblAnswers);
                grid.Children.Add(rdioA);
                grid.Children.Add(rdioB);
                grid.Children.Add(rdioC);
                grid.Children.Add(lblA);
                grid.Children.Add(lblB);
                grid.Children.Add(lblC);
                grid.Children.Add(lblCorrectAnswer);


                questionNumber++;

                card.Content = grid;
                stckMain.Children.Add(card);
            }

        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(lecturer).Show();
            this.Hide();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Hide();
        }

        private void BtnTests_Click(object sender, RoutedEventArgs e)
        {
            
            new ViewTestsLecturerWindow(lecturer).Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
