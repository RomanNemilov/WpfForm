using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfForm
{
    internal class ProductInOrder : Product
    {
        public int Quantity { get; set; }
        public ProductInOrder(Product product)
        {
            Name = product.Name;
            Cost = product.Cost;
            Photo = product.Photo;
            Quantity = 1;
        }
    }
}
