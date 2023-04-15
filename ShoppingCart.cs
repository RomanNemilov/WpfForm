using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfForm
{
    internal class ShoppingCart
    {
        public List<ProductInOrder> Products { get; private set; }
        public double TotalCost
        {
            get
            {
                double sum = 0;
                foreach (ProductInOrder product in Products)
                {
                    sum += product.Cost * product.Quantity;
                }
                return sum;
            }
        }
        public ShoppingCart()
        {
            Products = new List<ProductInOrder>();
        }
        public void AddProduct(Product product)
        {
            ProductInOrder productInOrder = new ProductInOrder(product);
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Name == productInOrder.Name)
                {
                    Products[i].Quantity += 1;
                    return;
                }
            }
            Products.Add(productInOrder);
        }
        public void AddProduct(String name)
        {
            ProductInOrder product = Products.Find(x => x.Name.Equals(name));
            product.Quantity += 1;
        }
        public void RemoveProduct(String name)
        {
            ProductInOrder product = Products.Find(x => x.Name.Equals(name));
            product.Quantity -= 1;
            if(product.Quantity == 0)
            {
                Products.Remove(product);
            }
        }
    }
}
