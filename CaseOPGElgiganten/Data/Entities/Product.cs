using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseOPGElgiganten.Data.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int ManufacturerID { get; set; }
        public int CategoryID { get; set; }
        public string ProductDescription { get; set; }
        public string NetDimensions { get; set; }
        public decimal NetWeight { get; set; }
        public string GrossDimensions { get; set; }
        public decimal GrossWeight { get; set; }
        public Nullable<long> EAN { get; set; }
        public Nullable<long> GTIN { get; set; }
        //public ProductInfo ProductInfo { get; set; }
        //public ICollection<ProductInfo> ProductInfos { get; set; }

        //public ICollection<ProductInfo> ProductInfos { get; set; }

    }
}
