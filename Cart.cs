using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingApp_DS_VK_JMK_SNA.Entities
{
    class Cart
    {
        private List<Product> listOfProducts;
        public void AddProduct(Product product)
        {
            listOfProducts.Add(product);
        }
    }
}
