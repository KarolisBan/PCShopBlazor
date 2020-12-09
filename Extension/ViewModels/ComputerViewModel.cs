using AutoMapper;
using PCShopBlazor.Variables.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCShopBlazor.Extension.ViewModels
{
    public class ComputerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public float Price { get; set; }
        public int CreatorId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public ICollection<Part> Parts { get; set; }


        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Computer, OrderViewModel>();
        }
    }
}
