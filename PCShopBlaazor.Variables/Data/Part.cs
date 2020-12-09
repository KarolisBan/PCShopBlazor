using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCShopBlazor.Variables.Data
{
    public class Part
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Model { get; set; }
        public float Value { get; set; }
        public int CreatorId { get; set; }

        public int ComputerId { get; set; }
        public Computer Computer { get; set; }
    }
}
