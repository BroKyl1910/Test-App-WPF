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
        TestAppEntities db = new TestAppEntities();
        User student;
        Test test;
        List<Answer> answers;
        List<Question> questions;
        int questionIndex;
        RadioButton[] answerRadioButtons;


        public TakeTestWindow()
        {
            InitializeComponent();
            test = db.Tests.First();
            questions = test.Questions.ToList();
            answers = questions.Select(q=>new Answer()).ToList();
            student = db.Users.First(u => u.UserType == (int)UserType.STUDENT);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Set up UI
            lblTestTitle.Text = test.Title + " - " + test.User.FirstName + " " + test.User.Surname;
            lblModule.Text = test.Module.ToString();
            lblDueDate.Text = test.DueDate.ToShortDateString();

            answerRadioButtons = new RadioButton[] { rdioA, rdioB, rdioC };
            foreach(RadioButton rb in answerRadioButtons)
            {
                rb.Checked += AnswerSelection_Checked;
            }

            questionIndex = 0;

            UpdateQuestionDisplay();
            UpdateNextPrevButtons();
        }

        private void AnswerSelection_Checked(object sender, RoutedEventArgs e)
        {
            btnNext.IsEnabled = false;
            btnPrev.IsEnabled = false;
        }

        private void UpdateNextPrevButtons()
        {


            //On start
            if (answers.Count == 0)
            {
                btnPrev.IsEnabled = false;
                btnNext.IsEnabled = false;
            }
            else if (questionIndex == 0)
            {
                btnPrev.IsEnabled = false;
                btnNext.IsEnabled = true;
            }
            else if (questionIndex == questions.Count)
            {
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = false;
            }
            else
            {
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
            }
        }

        private void UpdateQuestionDisplay()
        {
            System.Diagnostics.Trace.WriteLine("Question Index: " + questionIndex);
            System.Diagnostics.Trace.WriteLine("Length of Questions: " + questions.Count);
            lblQuestionNumber.Text = "Question " + (questionIndex + 1);
            //Display question
            lblA.Text = questions[questionIndex].Answer1;
            lblB.Text = questions[questionIndex].Answer2;
            lblC.Text = questions[questionIndex].Answer3;
            lblQuestion.Text = questions[questionIndex].QuestionText;
            rdioA.IsChecked = false;
            rdioB.IsChecked = false;
            rdioC.IsChecked = false;
        }

        private void BtnSaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            //Check that answer is selected
            if (answerRadioButtons.Any(r => r.IsChecked == true))
            {
                crdError.Visibility = Visibility.Hidden;
                UpdateNextPrevButtons();
            }
            else
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please select an answer";
            }
        }
    }
}
