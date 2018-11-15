using System.Collections.ObjectModel;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AddOperatorsViewModel : BaseViewModel
    {
        readonly IBaseRepository<Operator> operatorRepo;
        public Command SaveOperator { get; set; }
        public AddOperatorsViewModel()
        {
            SaveOperator = new Command(SaveOperatorDetails);
            operatorRepo = new BaseRepository<Operator>(App.DatabasePath);
            Name = string.Empty;
        }

        ObservableCollection<string> operators;
        public ObservableCollection<string> Operators => Utilities.Activities;

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        void SaveOperatorDetails()
        {
            if (operators == null)
            {
                operators = new ObservableCollection<string>();
                operators = Utilities.Activities;
            }

            operators.Add(Name);
            Utilities.Activities = operators;
            Name = string.Empty;
        }

        public override void SubmitDetailsAndNavigate()
        {
            foreach (var element in Operators)
            {
                var operator1 = new Operator() { Name = element };
                operatorRepo.SaveItem(operator1);
            }

            Utilities.Navigate(new StudyStartPAge());
        }
    }
}

