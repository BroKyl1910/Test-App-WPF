﻿using System;
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
            //Auto-select module
            cmbModule.SelectedIndex = lecturerModules.FindIndex(lm => lm.ModuleID == test.Module.ModuleID);
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
            else if (questionIndex == questions.Count - 1)
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
                SaveCurrentQuestion();
            }
        }

        private void SaveCurrentQuestion()
        {
            // Overwrite question with new data
            questions[questionIndex].QuestionText = txtQuestion.Text;
            questions[questionIndex].Answer1 = txtA.Text;
            questions[questionIndex].Answer2 = txtB.Text;
            questions[questionIndex].Answer3 = txtC.Text;
            questions[questionIndex].CorrectAnswer = Array.FindIndex(answerRadioButtons, r => r.IsChecked == true);

            UpdateNextPrevButtons();
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
                SaveCurrentQuestion();

                //Get saved test
                Test dbTest = db.Tests.Single(t => t.TestID == test.TestID);
                //Edit saved test
                dbTest.ModuleID = ((Module)cmbModule.SelectedItem).ModuleID;
                dbTest.DueDate = (DateTime)dtpDueDate.SelectedDate;
                dbTest.Title = txtTestTitle.Text;

                db.SaveChanges();

                foreach (Question q in questions)
                {
                    // Get saved question with same ID
                    Question dbQuestion = db.Questions.Single(dbQ => dbQ.QuestionID == q.QuestionID);
                    //Edit saved question
                    dbQuestion.QuestionText = q.QuestionText;
                    dbQuestion.Answer1 = q.Answer1;
                    dbQuestion.Answer2 = q.Answer2;
                    dbQuestion.Answer3 = q.Answer3;
                    dbQuestion.CorrectAnswer = q.CorrectAnswer;

                }

                //Ensure test is published if it was unpublished
                dbTest.Published = true;

                try
                {
                    db.SaveChanges();

                    MessageBox.Show("Saved");

                    new ViewTestsLecturerWindow(lecturer).Show();
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
