using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using ShoppingApp_DS_VK_JMK_SNA.ViewModels;

namespace ShoppingApp_DS_VK_JMK_SNA.Entities
{
    class Product : Entity, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        private string name;

        public string Name
        {
            get => name;

            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string category;

        public string Category
        {
            get => category;

            set
            {
                category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        private string description;

        public string Description
        {
            get => description;

            set
            {
                description = value;
                OnPropertyChanged(nameof(value));
            }
        }

        private decimal? discount;

        public decimal? Discount
        {
            get => discount;
            set
            {
                discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }

        private decimal price;

        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private int quantity;

        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private bool isAvailable;

        public bool IsAvailable
        {
            get => isAvailable;

            set
            {
                isAvailable = value;
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        private int selectQuantity;

        public int SelectQuantity
        {
            get => selectQuantity;

            set
            {
                selectQuantity = value;
                OnPropertyChanged(nameof(SelectQuantity));
            }
        }

        private int currentQuantity;

        public int CurrentQuantity
        {
            get => currentQuantity;

            set
            {
                currentQuantity = value;
                OnPropertyChanged(nameof(currentQuantity));
            }
        }
        public ObservableCollection<Product> AddToCart { get;}

        public Product() { }

        public Product(string name, string category, string description, decimal? discount, decimal price,int quantity)
            : this(default, default, default, name, category, description, discount, price,quantity) { }

        public Product(string name, string category, string description, decimal? discount, decimal price)
            : this(default, default, default, name, category, description, discount, price) { }

        public Product(long id, DateTime dateCreated, DateTime dateModified, string name,
            string category, string description, decimal? discount, decimal price)
            : base(id, dateCreated, dateModified)
        {
            Name = name;
            Category = category;
            Description = description;
            Discount = discount;
            Price = price;
            AddToCart = new ObservableCollection<Product>();
            SelectQuantity = 1;
            CurrentQuantity = 1;
        }
        public Product(long id, DateTime dateCreated, DateTime dateModified, string name,
            string category, string description, decimal? discount, decimal price,int quantity)
            : base(id, dateCreated, dateModified)
        {
            Name = name;
            Category = category;
            Description = description;
            Discount = discount;
            Price = price;
            Quantity = quantity;
            AddToCart = new ObservableCollection<Product>();
            SelectQuantity = 1;
            CurrentQuantity = 1;
        }

        public bool AddProduct(int quantity)
        {
            bool success = Quantity > 0 && Quantity>=SelectQuantity;
            if (success)
            {
                Quantity -= SelectQuantity;
                CurrentQuantity += SelectQuantity;
                AddToCart.Add(new Product(name, category, description, discount, price,currentQuantity));
            }

            return success;
        }

        public bool RemoveProduct()
        {
           
            bool success = AddToCart.Count > 0 && CurrentQuantity>=SelectQuantity;
            if (success)
            {
                Quantity += SelectQuantity;
                CurrentQuantity -= SelectQuantity;
                if (CurrentQuantity == 0)
                {
                    //AddToCart = AddToCart[AddToCart.Count - 1];
                }
            }

            return success;
        }
    }
}
