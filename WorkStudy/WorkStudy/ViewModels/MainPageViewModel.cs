using System.Collections.ObjectModel;
using WorkStudy.Model;
using WorkStudy.Services;

namespace WorkStudy.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private Operator oldOperator;
        public ObservableCollection<Operator> Operators { get; set; }
        readonly IBaseRepository<Operator> operatorRepo;

        static int _studyNumber = 1;
        public int StudyNumber
        {
            get => _studyNumber;
            set
            {
                _studyNumber = value;
                OnPropertyChanged();
            }
        }
       
        public MainPageViewModel()
        {
            operatorRepo = new BaseRepository<Operator>(App.DatabasePath);
            Operators = (ObservableCollection<Operator>)operatorRepo.GetItems();
        }

        public void ShowOrHideOperators(Operator value)
        {
            if (oldOperator == value)
            {
                value.Isvisible = !value.Isvisible;
                value.Observed = "OBSERVED";
                UpdateOperators(value);
            }
            else
            {
                if (oldOperator != null)
                {
                    oldOperator.Isvisible = false;
                    oldOperator.Observed = "OBSERVED";
                    UpdateOperators(oldOperator);

                }

                value.Isvisible = true;
                value.Observed = "";
                UpdateOperators(value);
            }

            oldOperator = value;
        }

        public void UpdateStudyNumber()
        {
            StudyNumber = StudyNumber + 1;
        }
        private void UpdateOperators(Operator value)
        {
            var index = Operators.IndexOf(value);
            Operators.Remove(value);
            Operators.Insert(index, value);
        }

    }
}
