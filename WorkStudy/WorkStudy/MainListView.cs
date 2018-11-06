using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WorkStudy
{
    public class MainListView : INotifyPropertyChanged
    {
        private Product _oldProduct;
        public ObservableCollection<Product> Products { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        int studyNumber;
        public int StudyNumber
        {
            get { return studyNumber; }
            set
            {
                studyNumber = value;
                OnPropertyChanged("StudyNumber");
            }
        }
       

        public MainListView()
        {
            Products = new ObservableCollection<Product>
            {
                new Product
                {
                    Title = "John The Welder",

                    Isvisible = false
                },
                new Product
                {
                    Title = "Adam The Riveter",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Paul the Polisher",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Jake The Painter",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Sarah the Packer",
                    Isvisible = false
                }
            };

        }

        public void ShoworHiddenProducts(Product product)
        {
            if (_oldProduct == product)
            {
                product.Isvisible = !product.Isvisible;
                UpDateProducts(product);
            }
            else
            {
                if (_oldProduct != null)
                {
                    _oldProduct.Isvisible = false;
                    UpDateProducts(_oldProduct);

                }

                product.Isvisible = true;
                UpDateProducts(product);
            }

            _oldProduct = product;
        }


        public void UpdateStudyNumber()
        {
            StudyNumber = StudyNumber + 1;
        }

        private void UpDateProducts(Product product)
        {

            var Index = Products.IndexOf(product);
            Products.Remove(product);
            Products.Insert(Index, product);

        }


    }
}
