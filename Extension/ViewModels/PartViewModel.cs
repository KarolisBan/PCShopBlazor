using AutoMapper;
using PCShopBlazor.Variables.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCShopBlazor.Extension.ViewModels
{
    public class PartViewModel
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public float Value { get; set; }
        public int CreatorId { get; set; }

        public int  ComputerId { get; set; }
        public Computer Computer { get; set; }
        //public override string ToString()
        //{
        //    return $"{Country}, {City}, {Street} {Address}";
        //}

        //public string GetAddress()
        //{
        //    return $"{Street} {Address}, {City}";
        //}

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Part, PartViewModel>();
        }
    }
}
