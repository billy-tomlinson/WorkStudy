using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{

    //public class Employee
    //{
    //    public string Name
    //    {
    //        get;
    //        set;
    //    }
    //}

    //public class GroupOfEmployees
    //{
    //    public Employee EmployeeOne
    //    {
    //        get;
    //        set;
    //    }
    //    public Employee EmployeeTwo
    //    {
    //        get;
    //        set;
    //    }

    //    public Employee EmployeeThree
    //    {
    //        get;
    //        set;
    //    }
    //}
    public class MainViewModel : INotifyPropertyChanged
    {
        
        //private List<Employee> _employees;
        //private List<GroupOfEmployees> _groupEmployees;


        //public List<Employee> Employees
        //{
        //    get => _employees;
        //    set
        //    {
        //        _employees = value;
        //        OnPropertyChanged();
        //    }
        //}


        //public List<GroupOfEmployees> GroupEmployees
        //{
        //    get => BuildGroupOfEmployees();
        //    set
        //    {
        //        _groupEmployees = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private List<GroupOfEmployees> BuildGroupOfEmployees()
        //{
        //    int counter = 0;
        //    GroupOfEmployees groupOfEmployee = new GroupOfEmployees();
        //    var groupedEmployees = new List<GroupOfEmployees>();
        //    for (int i = 0; i < Employees.Count; i++)
        //    {

        //        if (counter == 0)
        //        {
        //            groupOfEmployee.EmployeeOne = Employees[i];
        //            counter++;
        //        }

        //        else if (counter == 1)
        //        {
        //            groupOfEmployee.EmployeeTwo = Employees[i];
        //            counter++;
        //        }

        //        else if (counter == 2)
        //        {
        //            groupOfEmployee.EmployeeThree = Employees[i];
        //            groupedEmployees.Add(groupOfEmployee);
        //            groupOfEmployee = new GroupOfEmployees();
        //            counter = 0;
        //        }


        //    }

        //    return groupedEmployees;
        //}

        public ICommand SelectStudentCommand => new Command(student =>
        {

        });

        public MainViewModel()
        {

            //Employees = new List<Employee>
            //{
            //    new Employee{Name = "pilly"},
            //    new Employee{Name = "silly"},
            //    new Employee{Name = "dilly"},
            //    new Employee{Name = "filly"},
            //    new Employee{Name = "gilly"},
            //    new Employee{Name = "milly"}
            //};
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
