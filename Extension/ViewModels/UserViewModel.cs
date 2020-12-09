using AutoMapper;
using PCShopBlazor.Variables.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCShopBlazor.Extension.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<User, UserViewModel>();
        }
    }
}
