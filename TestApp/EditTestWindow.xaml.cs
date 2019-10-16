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
    /// Interaction logic for EditTestWindow.xaml
    /// </summary>
    public partial class EditTestWindow : Window
    {
        TestAppEntities db = new TestAppEntities();

        User lecturer;
        Test test;
        List<Question> questions;
        int questionIndex;
        RadioButton[] answerRadioButtons;

        public EditTestWindow(User lecturer, Test test)
        {
            InitializeComponent();
            //this.lecturer = lecturer;
            this.lecturer = lecturer;
            this.test = test;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            questions = test.Questions.ToList();
            questionIndex = 0;
            answerRadioButtons = new RadioButton[] { rdioA, rdioB, rdioC };

            // Set up GUI
            List<Module> lecturerModules = lecturer.LecturerAssignments.Select(a => a.Module).ToList();

            //Add modules to combobox
            cmbModule.Items.Clear();
            foreach (Module module in lecturerModules)
            {
                cmbModule.Items.Add(module);
            }
            txtTestTitle.Text = test.Title;
            cmbModule.SelectedItem = test.Module;
            dtpDueDate.SelectedDate = test.DueDate;

            UpdateQuestionDisplay();
            UpdateNextPrevButtons();
        }

        private void UpdateQuestionDisplay()
        {
            System.Diagnostics.Trace.WriteLine("Question Index: " + questionIndex);
            System.Diagnostics.Trace.WriteLine("Length of Questions: " + questions.Count);
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

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            new ViewTestsLecturerWindow(lecturer).Show();
            this.Hide();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Ensure user can't be left in a situation where they have no questions
            if (questions.Count <= 1 && questionIndex == 0)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Test must have at least one question";
                return;
            }

            crdError.Visibility = Visibility.Hidden;
            if (questionIndex == questions.Count)
            {
                questionIndex--;
                UpdateQuestionDisplay();
                UpdateNextPrevButtons();
                return;
            }
            questions.RemoveAt(questionIndex);

            if (questionIndex >= questions.Count)
            {
                questionIndex = questions.Count - 1;
            }

            UpdateQuestionDisplay();
            UpdateNextPrevButtons();
        }

        private void BtnSaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateQuestionForm())
            {
                Question question = new Question();
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

        private bool ValidateQuestionForm()
        {
            //Check all fields are filled
            if (!AllQuestionFieldsFilled())
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please complete all question fields";
                return false;
            }

            crdError.Visibility = Visibility.Hidden;
            return true;
        }

        private bool AllQuestionFieldsFilled()
        {
            return !(txtQuestion.Text.Equals("") || txtA.Text.Equals("") || txtB.Text.Equals("") || txtC.Text.Equals("") || Array.FindIndex(answerRadioButtons, r => r.IsChecked == true) == -1);
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
            if (ValidateQuestionForm() && ValidateTestForm())
            {
                Test dbTest = db.Tests.Single(t => t.TestID == test.TestID);
                dbTest.ModuleID = ((Module)cmbModule.SelectedItem).ModuleID;
                dbTest.DueDate = (DateTime)dtpDueDate.SelectedDate;
                dbTest.Title = txtTestTitle.Text;

                //Remove all answers that contain this test's questions
                //var answersToDelete = db.Answers.Where(a => a.Test.TestID == test.TestID).ToList();
                //foreach (Answer answer in answersToDelete)
                //{
                //    db.Answers.Remove(answer);
                //}
                db.SaveChanges();
                dbTest.Questions.Clear();
                foreach (Question q in questions)
                {
                    q.TestID = dbTest.TestID;
                    //q.Test = dbTest;
                    dbTest.Questions.Add(q);

                }

                //dbTest.Questions = questions;

                //questions.ForEach(q => q.TestID = newTest.TestID);


                ////Remove questions
                //questions.ForEach(q => db.Questions.RemoveRange(db.Questions.Where(qu=> q.QuestionID!= -1 && qu.QuestionID==q.QuestionID)));

                ////Add 
                //questions.ForEach(q => db.Questions.Add(q));
                //db.SaveChanges();

                ////Repopulate test's questions

                try
                {
                    db.SaveChanges();

                    MessageBox.Show("Saved");

                    new MainWindow(lecturer).Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }
        }

        private bool ValidateTestForm()
        {
            //Check all fields are filled
            if (!AllTestFieldsFilled())
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Please complete all test fields";
                return false;
            }

            //Ensure test has questions
            if (questions.Count == 0)
            {
                crdError.Visibility = Visibility.Visible;
                lblError.Text = "Test has no questions";
                return false;
            }

            crdError.Visibility = Visibility.Hidden;
            return true;
        }

        private bool AllTestFieldsFilled()
        {
            return !(txtTestTitle.Text.Equals("") || dtpDueDate.SelectedDate == null || cmbModule.SelectedIndex == -1);
        }

    }
}
