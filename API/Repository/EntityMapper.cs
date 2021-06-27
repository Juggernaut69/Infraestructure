using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Domain;

namespace API.Repository
{
    public class EntityMapper<TSource, TDestination> where TSource : class where TDestination : class
    {

        public EntityMapper()
        {
            Mapper.CreateMap<Models.Customer, CustomerDomain>();
            Mapper.CreateMap<CustomerDomain, Models.Customer>();
            Mapper.CreateMap<Models.Product, ProductDomain>();
            Mapper.CreateMap<ProductDomain, Models.Product>();
            Mapper.CreateMap<Models.Order, OrderDomain>();
            Mapper.CreateMap<OrderDomain, Models.Order>();
            Mapper.CreateMap<Models.OrderDetail, OrderDetailDomain>();
            Mapper.CreateMap<OrderDetailDomain, Models.OrderDetail>();
        }

        public TDestination Translate(TSource obj)
        {
            return Mapper.Map<TDestination>(obj);
        }

    }

}