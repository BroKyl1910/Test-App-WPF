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
    /// Interaction logic for CreateTestWindow.xaml
    /// </summary>
    public partial class CreateTestWindow : Window
    {
        TestAppEntities db = new TestAppEntities();

        Test test;
        List<Question> questions;
        int questionIndex;
        RadioButton[] answerRadioButtons;

        public CreateTestWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            test = new Test();
            questions = new List<Question>();
            questionIndex = 0;
            answerRadioButtons = new RadioButton[] { rdioA, rdioB, rdioC };

            // Set up GUI
            User user = db.Users.First(u => u.Username.Equals("BroKyl1910"));
            List<Module> lecturerModules = user.LecturerAssignments.Select(a => a.Module).ToList();
            DateTime today = DateTime.Today;

            //Add modules to combobox
            cmbModule.Items.Clear();
            foreach(Module module in lecturerModules)
            {
                cmbModule.Items.Add(module);
            }

            dtpDueDate.SelectedDate = today;

            UpdateQuestionDisplay();
            UpdateNextPrevButtons();
        }

        private void btnSaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                Question question = new Question();
                question.TestID = test.TestID;
                question.QuestionText = txtQuestion.Text;
                question.Answer1 = txtA.Text;
                question.Answer2 = txtB.Text;
                question.Answer3 = txtC.Text;


                question.CorrectAnswer = Array.FindIndex(answerRadioButtons, r => r.IsChecked == true);
                if (questionIndex == questions.Count)
                {
                    //new question
                    questions.Add(question);
                }
                else
                {
                    //update existing
                    questions[questionIndex] = question;
                }

                UpdateNextPrevButtons();
            }
        }

        private void UpdateNextPrevButtons()
        {
            // index starts at 0
            // only updated when next is clicked

            //On start
            if (questions.Count == 0)
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

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            questionIndex++;
            UpdateQuestionDisplay();
            UpdateNextPrevButtons();
        }

        private void UpdateQuestionDisplay()
        {
            System.Diagnostics.Trace.WriteLine("Question Index: " +questionIndex);
            System.Diagnostics.Trace.WriteLine("Length of Questions: " +questions.Count);
            lblQuestionNumber.Text = "Question " + (questionIndex + 1);
            if (questionIndex == questions.Count)
            {
                //Empty fields
                txtA.Text = "";
                txtB.Text = "";
                txtC.Text = "";
                txtQuestion.Text = "";
                rdioA.IsChecked = false;
                rdioB.IsChecked = false;
                rdioC.IsChecked = false;
            }
            else
            {
                //Display question
                txtA.Text = questions[questionIndex].Answer1;
                txtB.Text = questions[questionIndex].Answer2;
                txtC.Text = questions[questionIndex].Answer3;
                txtQuestion.Text = questions[questionIndex].QuestionText;
                rdioA.IsChecked = false;
                rdioB.IsChecked = false;
                rdioC.IsChecked = false;
                answerRadioButtons[questions[questionIndex].CorrectAnswer].IsChecked = true;
            }
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            questionIndex--;
            UpdateQuestionDisplay();
            UpdateNextPrevButtons();
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

            crdError.Visibility = Visibility.Hidden;
            return true;
        }

        private bool AllFieldsFilled()
        {
            return !(txtQuestion.Text.Equals("") || txtA.Text.Equals("") || txtB.Text.Equals("") || txtC.Text.Equals("") || Array.FindIndex(answerRadioButtons, r=> r.IsChecked==true) == -1);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(questions.Count == 0)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Test must have at least one question";
                return;
            }
            crdError.Visibility = Visibility.Hidden;
            questions.RemoveAt(questionIndex);
            if (questionIndex >= questions.Count)
            {
                questionIndex = questions.Count - 1;
            }

            UpdateQuestionDisplay();
        }
    }
}
