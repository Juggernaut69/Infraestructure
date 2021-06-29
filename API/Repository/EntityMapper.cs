using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using AutoMapper;
using Domain;

namespace API.Repository
{
    public class EntityMapper<TSource, TDestination> where TSource : class where TDestination : class
    {

        public EntityMapper()
        {
            Mapper.CreateMap<CustomerModel, Customer>();
            Mapper.CreateMap<Customer, CustomerModel>();
            Mapper.CreateMap<ProductModel, Product>();
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<OrderModel, Order>();
            Mapper.CreateMap<Order, OrderModel>();
            Mapper.CreateMap<OrderDetailModel, OrderDetail>();
            Mapper.CreateMap<OrderDetail, OrderDetailModel>();
        }

        public TDestination Translate(TSource obj)
        {
            return Mapper.Map<TDestination>(obj);
        }

    }

}