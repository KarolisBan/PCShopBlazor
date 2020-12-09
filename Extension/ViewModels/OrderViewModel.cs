using AutoMapper;
using PCShopBlazor.Variables.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCShopBlazor.Extension.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public float OrderPrice { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public ICollection<Computer> Computers { get; set; }


        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Order, OrderViewModel>();
        }
    }
}
