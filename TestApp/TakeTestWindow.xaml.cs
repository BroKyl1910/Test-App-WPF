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
        User user;
        Test test;
        List<Question> questions;
        int questionIndex;
        int[] answerIndices;
        RadioButton[] answerRadioButtons;


        public TakeTestWindow(User user, Test test)
        {
            InitializeComponent();

            questions = test.Questions.ToList();
            answerIndices = questions.Select(q => -1).ToArray();
            this.user = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Set up UI
            lblTestTitle.Text = test.Title + " - " + test.User.FirstName + " " + test.User.Surname;
            lblModule.Text = test.Module.ToString();
            lblDueDate.Text = "Due by - "+test.DueDate.ToShortDateString();

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
            if (questionIndex == 0)
            {
                btnPrev.IsEnabled = false;
                btnNext.IsEnabled = true;
            }
            else if (questionIndex == questions.Count-1)
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
            lblQuestionNumber.Text = "Question " + (questionIndex + 1) +" of "+questions.Count;
            //Display question
            lblA.Text = questions[questionIndex].Answer1;
            lblB.Text = questions[questionIndex].Answer2;
            lblC.Text = questions[questionIndex].Answer3;
            lblQuestion.Text = questions[questionIndex].QuestionText;
            rdioA.IsChecked = false;
            rdioB.IsChecked = false;
            rdioC.IsChecked = false;
            if (answerIndices[questionIndex] != -1)
            {
                answerRadioButtons[answerIndices[questionIndex]].IsChecked = true;
            }
        }

        private void BtnSaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            //Check that answer is selected
            if (answerRadioButtons.Any(r => r.IsChecked == true))
            {
                crdError.Visibility = Visibility.Hidden;
                answerIndices[questionIndex] = Array.FindIndex(answerRadioButtons, r => r.IsChecked == true);
                UpdateNextPrevButtons();
            }
            else
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please select an answer";
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            questionIndex++;
            UpdateQuestionDisplay();
            UpdateNextPrevButtons();

        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            questionIndex--;
            UpdateQuestionDisplay();
            UpdateNextPrevButtons();
        }

        private void BtnSaveTest_Click(object sender, RoutedEventArgs e)
        {
            if(answerIndices.Any(i => i == -1))
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please complete all questions";
                questionIndex = Array.FindIndex(answerIndices, i => i == -1);
                UpdateQuestionDisplay();
                UpdateNextPrevButtons();
            }
            else
            {
                List<Answer> answers = new List<Answer>();

                int attemptNumber = db.Results.Count(r => r.TestID == test.TestID && r.Username.Equals(user.Username))+1;

                for (int i = 0; i < questions.Count; i++)
                {
                    Answer answer = new Answer();
                    answer.AttemptNumber = attemptNumber;
                    answer.QuestionID = questions[i].QuestionID;
                    answer.Username = user.Username;
                    answer.TestID = test.TestID;
                    answer.UserAnswer = answerIndices[i];
                    answer.Correct = answerIndices[i] == questions[i].CorrectAnswer;

                    db.Answers.Add(answer);
                }

                db.SaveChanges();

                int numCorrect = db.Answers.Count(a => a.TestID == test.TestID && a.Username.Equals(user.Username) && a.AttemptNumber == attemptNumber && a.Correct);

                Result result = new Result();
                result.TestID = test.TestID;
                result.Username = user.Username;
                result.AttemptNumber = attemptNumber;
                result.UserResult = numCorrect;
                result.ResultPercentage = (decimal) ((double) numCorrect) / questions.Count * 100;

                db.Results.Add(result);

                db.SaveChanges();


                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Result - "+numCorrect+"/"+questions.Count;


            }
        }

        private void LblA_MouseUp(object sender, MouseButtonEventArgs e)
        {
            rdioA.IsChecked = true;
        }

        private void LblB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            rdioB.IsChecked = true;
        }

        private void LblC_MouseUp(object sender, MouseButtonEventArgs e)
        {
            rdioC.IsChecked = true;
        }

        private void BtnTests_Click(object sender, RoutedEventArgs e)
        {
            new ViewTestsWindow(user).Show();
            this.Hide();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Hide();
        }
    }
}
