using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCShopBlazor.Variables.Data
{
    public class Computer
    {
        public Computer()
        {
            Parts = new HashSet<Part>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        public float Price { get; set; }
        public int CreatorId { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public ICollection<Part> Parts { get; set; }
    }
}
