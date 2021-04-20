using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Input;
using ShoppingApp_DS_VK_JMK_SNA.Entities;

namespace ShoppingApp_DS_VK_JMK_SNA.ViewModels
{
    class ProductViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        //public List<Product> listOfProducts;

        private ObservableCollection<Product> allStoreProducts;

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products 
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private ObservableCollection<Product> cartProducts;
        public ObservableCollection<Product> CartProducts
        {
            get => cartProducts;
            set
            {
                cartProducts = value;
                OnPropertyChanged(nameof(CartProducts));
            }
        }

        public int SelectedIndex { get; set; } = -1;

        public IList SelectedProducts { get; set; }

        public DelegateCommand AddCommand { get; }
        public DelegateCommand CheckOutCommand { get; }

        public DelegateCommand RemoveCommand { get; }

        public DelegateCommand UpdateCommand { get; }

        public DelegateCommand ClearFiltersCommand { get; }

        public DelegateCommand GetTotalCommand { get; }

        public DelegateCommand AddProductCommand { get; }

        private string categoryFilter;
        public string CategoryFilter 
        {
            get => categoryFilter;

            set
            {
                categoryFilter = value;
                OnPropertyChanged(nameof(CategoryFilter));
                UpdateFilter();

            }
        }

        private string nameFilter;
        public string NameFilter
        {
            get => nameFilter;

            set
            {
                nameFilter = value;
                OnPropertyChanged(nameof(NameFilter));
                UpdateFilter();
            }
        }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                if (!(selectedProduct is null))
                {
                    selectedProduct.PropertyChanged -= ProductPropertyChanged;
                }

                selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));

                UpdateSelectedProductQuantity();


                if(!(selectedProduct is null))
                {
                    selectedProduct.PropertyChanged += ProductPropertyChanged;
                }
            }
        }

        private string selectedProductAvailability;

        public String SelectedProductAvailability
        {
            get => selectedProductAvailability;

            set
            {
                selectedProductAvailability = value;
                OnPropertyChanged(nameof(SelectedProductAvailability));
            }
        }

        public ProductViewModel()
        {
            Products = allStoreProducts = new ObservableCollection<Product>
            {
                new Product("Laptop","Electronics","i3",0,1000,2),
                new Product("Polo T-shirt","Clothing","Denim",0,89,3),
                new Product("Woodland Sandals","Footwear","Soft",0,179,2),
                new Product("iPhone","Electronics","11 Pro Max",0,1514,3)
            };

            CartProducts = new ObservableCollection<Product>();

            AddCommand = new DelegateCommand(AddSelectedProduct);
            CheckOutCommand = new DelegateCommand(CheckOutProducts);
            ClearFiltersCommand = new DelegateCommand(ClearFilters);
            RemoveCommand = new DelegateCommand(RemoveSelectedProduct);
            UpdateCommand = new DelegateCommand(UpdateSelectedProduct);
            GetTotalCommand = new DelegateCommand(UpdateCartTotal);
            AddProductCommand = new DelegateCommand(AddProduct);

        }
        private void ProductPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Product.Quantity)))
            {
                UpdateSelectedProductQuantity();
            }
        }
        private void UpdateSelectedProductQuantity()
        {
            if (selectedProduct is null)
                SelectedProductAvailability = null;
            else if (selectedProduct.Quantity > 0)
                SelectedProductAvailability = $"Quantity {selectedProduct.Quantity}";
            else
                SelectedProductAvailability = "Available";
        }

        private void UpdateCartTotal(object Parameter)
        {
            decimal total=0;
            decimal? discountTotal = 0;
            decimal? grandTotal = 0;
            if (CartProducts is null)
            {
                SubTotal = 0;
            }
            else if (CartProducts.Count > 0)
            {
                foreach(Product product in CartProducts)
                {
                    total += product.Price;
                    discountTotal += product.Discount;
                }
                grandTotal = total - discountTotal;
                SubTotal = (decimal)grandTotal;
            }
        }

        

        private void UpdateFilter()
        {
            if (ShouldApplyFilter())
                ApplyFilter();
            else
                ClearFilters(null);
        }
        private bool ShouldApplyFilter()
        {
            bool categoryEmpty = string.IsNullOrWhiteSpace(CategoryFilter);
            bool nameEmpty = string.IsNullOrWhiteSpace(NameFilter);
            return !(categoryEmpty && nameEmpty);
        }

        public void AddSelectedProduct(object parameter)
        {
            if (CanAddSelectedProduct(parameter))
            {
                CartProducts.Add(new Product(SelectedProduct.Name,SelectedProduct.Category
                    ,SelectedProduct.Description,SelectedProduct.Discount
                    ,SelectedProduct.Price));
                SelectedProduct = null;
            }

        }
        private void AddProduct(object Parameter)
        {
            Product product = new Product(ProductName, ProductCategory, ProductDescription,
                ProductDiscount, ProductPrice, ProductQuantity);
            Products.Add(product);
        }

        private bool CanAddSelectedProduct(object parameter)
        {
            return SelectedProduct != null && SelectedProduct.Quantity>0;
        }

        public void RemoveSelectedProduct(object parameter)
        {
            ObservableCollection<Product> updatedProducts = new ObservableCollection<Product>();
            foreach(Product product in CartProducts)
            {
                updatedProducts.Add(product);
            }
            if (updatedProducts.Count == 0)
                CartProducts = null;
            else
            {
                updatedProducts.Remove(SelectedProduct);
            }
            CartProducts = updatedProducts;

        }

        private bool CanRemoveSelectedProduct(object parameter)
        {
            return SelectedProduct != null;
        }

        public void UpdateSelectedProduct(object parameter)
        {
            ObservableCollection<Product> updatedProducts = new ObservableCollection<Product>();
            foreach(Product product in CartProducts)
            {
                updatedProducts.Add(product);
            }
            if (CanUpdateSelectedProduct(parameter))
            {
                Product product = CartProducts.ElementAt(SelectedIndex+1);
                updatedProducts.Remove(product);
                product.SelectQuantity = SelectProductQuantity;
                updatedProducts.Add(product);
                CartProducts = updatedProducts;
            }

        }

        private bool CanUpdateSelectedProduct(object parameter)
        {
            return SelectedProduct != null;
        }

        public void CheckOutProducts(object parameter)
        {
            if (CanCheckOutSelectedProduct(parameter))
            {
                CartProducts.Clear();
            }

        }

        private  bool CanCheckOutSelectedProduct(object parameter)
        {
            return CartProducts.Count> 0;
        }

        private void ApplyFilter()
        {
            ObservableCollection<Product> filteredProducts = new ObservableCollection<Product>();
            foreach(Product product in allStoreProducts)
            {
                if (ProductMatchesFilters(product))
                    filteredProducts.Add(product);
            }

            Products = filteredProducts;
        }

        private bool ProductMatchesFilters(Product product)
        {

            if (!string.IsNullOrWhiteSpace(CategoryFilter) &&
                !product.Category.ToLower().StartsWith(CategoryFilter.ToLower()))
            {
                return false;
            }
                
            else if (!string.IsNullOrWhiteSpace(NameFilter) 
                && !product.Name.ToLower().StartsWith(NameFilter.ToLower()))
            {
                return false;
            }
                
            return true;
        }

        private void ClearFilters(object parameter)
        {
            categoryFilter = null;
            nameFilter = null;
            OnPropertyChanged(categoryFilter);
            OnPropertyChanged(NameFilter);
            Products = allStoreProducts;
        }

        private int selectProductQuantity;

        public int SelectProductQuantity
        {
            get => selectProductQuantity;

            set
            {
                selectProductQuantity = value;
                OnPropertyChanged(nameof(SelectProductQuantity));
            }
        }

        private decimal subTotal;

        public decimal SubTotal
        {
            get => subTotal;

            set
            {
                subTotal = value;
                OnPropertyChanged(nameof(SubTotal));
            }
        }

        private decimal? totalDiscount;

        public decimal? TotalDiscount
        {
            get => totalDiscount;

            set
            {
                totalDiscount = value;
                OnPropertyChanged(nameof(TotalDiscount));
            }
        }

        private decimal? totalPrice;

        public decimal? TotalPrice
        {
            get => totalPrice;

            set
            {
                totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        private string productName;

        public string ProductName
        {
            get => productName;

            set
            {
                productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        private string productCategory;

        public string ProductCategory
        {
            get => productCategory;

            set
            {
                productCategory = value;
                OnPropertyChanged(nameof(productCategory));
            }
        }

        private string productDescription;

        public string ProductDescription
        {
            get => productDescription;

            set
            {
                productDescription = value;
                OnPropertyChanged(nameof(ProductDescription));
            }
        }

        private decimal? productDiscount;

        public decimal? ProductDiscount
        {
            get => productDiscount;

            set
            {
                productDiscount = value;
                OnPropertyChanged(nameof(ProductDiscount));
            }
        }

        private decimal productPrice;

        public decimal ProductPrice
        {
            get => productPrice;

            set
            {
                productPrice = value;
                OnPropertyChanged(nameof(ProductPrice));
            }
        }

        private int productQuantity;

        public int ProductQuantity
        {
            get => productQuantity;

            set
            {
                productQuantity = value;
                OnPropertyChanged(nameof(ProductQuantity));
            }
        }
    }
}
