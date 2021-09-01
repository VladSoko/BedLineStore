using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BedLinenStore.WEB.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public MainInfo MainInfo { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public int MainInfoId { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<CartLine> CartLines { get; set; }
    }
}
