using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BedLinenStore.WEB.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public ICollection<Product> Products { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Zip { get; set; }

        public string Email { get; set; }
    }
}
