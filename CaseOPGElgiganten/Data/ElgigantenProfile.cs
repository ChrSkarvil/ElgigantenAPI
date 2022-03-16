using AutoMapper;
using CaseOPGElgiganten.Data.Entities;
using CaseOPGElgiganten.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseOPGElgiganten.Data
{
    public class ElgigantenProfile : Profile
    {
        public ElgigantenProfile()
        {
            this.CreateMap<ProductInfo, ProductInfoModel>()
                .ReverseMap();


            this.CreateMap<Product, ProductModel>()
                .ReverseMap();
                //.ForMember(t => t.ProductInfo, opt => opt.Ignore());

        }
    }
}
