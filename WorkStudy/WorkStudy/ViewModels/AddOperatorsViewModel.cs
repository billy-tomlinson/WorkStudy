using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AddOperatorsViewModel : BaseViewModel
    {
        readonly IBaseRepository<Operator> operatorRepo;
        public Command SaveOperator { get; set; }
        public Operator Operator;

        public AddOperatorsViewModel()
        {
            SaveOperator = new Command(SaveOperatorDetails);
            operatorRepo = new BaseRepository<Operator>();
            Operators = new ObservableCollection<Operator>(operatorRepo.GetItems());
            Operator = new Operator();
            Name = string.Empty;
        }

        private ObservableCollection<Operator> operators;
        public ObservableCollection<Operator> Operators
        {
            get => operators;
            set
            {
                operators = value;
                OnPropertyChanged();
            }
        }

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
            List<Operator> duplicatesCheck = new List<Operator>(Operators);
            if (duplicatesCheck.Find(_ => _.Name.ToUpper() == Name.ToUpper().Trim()) == null)
                operatorRepo.SaveItem(new Operator { Name = Name.ToUpper().Trim() });
            Operators = new ObservableCollection<Operator>(operatorRepo.GetItems());
            Name = string.Empty;
        }

        public override void SubmitDetailsAndNavigate()
        {
            Utilities.Navigate(new StudyStartPAge());
        }

        public ICommand ItemClickedCommand
        {
            get { return ShowActivities(); }
        }

        Command ShowActivities()
        {
            return new Command((item) =>
            {
                Operator = item as Operator;
            });
        }
    }
}

