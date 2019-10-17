using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestApp
{
    public class ResultsViewModel
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public User User{ get; set; }
        public Test Test { get; set; }
        public double Result { get; set; }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ResultsViewModel> resultsViewModels;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            this.ResultsViewModels = new ObservableCollection<ResultsViewModel>();
        }

        public ViewModel(ObservableCollection<ResultsViewModel> resultsViewModels)
        {
            this.ResultsViewModels = resultsViewModels;
        }

        public ObservableCollection<ResultsViewModel> ResultsViewModels
        {
            get { return resultsViewModels; }
            set
            {
                resultsViewModels = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("NewResults");
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
                MessageBox.Show(name);
            }
        }
    }
}
