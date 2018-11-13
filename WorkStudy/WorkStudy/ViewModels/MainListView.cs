﻿using System.Collections.ObjectModel;
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

        static int studyNumber = 1;
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
                    Observed = "",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Adam The Riveter",
                    Observed = "",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Paul the Polisher",
                    Observed = "",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Jake The Painter",
                    Observed = "",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Sarah the Packer",
                    Observed = "",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Julie the Stacker",
                    Observed = "",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Ted the Driver",
                    Observed = "",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Philip the Broom",
                    Observed = "",
                    Isvisible = false
                },
                new Product
                {
                    Title = "Colin the Stamper",
                    Observed = "",
                    Isvisible = false
                }
            };

        }

        public void ShoworHiddenProducts(Product product)
        {
            if (_oldProduct == product)
            {
                product.Isvisible = !product.Isvisible;
                product.Observed = "OBSERVED";
                UpDateProducts(product);
            }
            else
            {
                if (_oldProduct != null)
                {
                    _oldProduct.Isvisible = false;
                    _oldProduct.Observed = "OBSERVED";
                    UpDateProducts(_oldProduct);

                }

                product.Isvisible = true;
                product.Observed = "";
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