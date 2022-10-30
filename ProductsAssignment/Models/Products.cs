using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAssignment
{
    public class Products
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImgURL { get; set; }
        public virtual Category Category { get; set; }
       
    }
}
