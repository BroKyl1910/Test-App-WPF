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
        int testNumber;

        public CreateTestWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            test = new Test();
            questions = new List<Question>();
            testNumber = 0;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Question question = new Question();
            question.TestID = test.TestID;
            question.QuestionID = "";
        }
    }
}
