using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCShopBlazor.Variables.Data
{
    public class Order
    {
        public Order()
        {
            Computers = new HashSet<Computer>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderNumber { get; set; }
        [Required]
        public float OrderPrice { get; set; }
        [Required]
        public int BuyerId { get; set; }
        [Required]
        public string BuyerName { get; set; }


        public ICollection<Computer> Computers { get; set; }
    }
}
