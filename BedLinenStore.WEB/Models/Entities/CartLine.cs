using System.Collections.Generic;

namespace BedLinenStore.WEB.Models.Entities
{
    public class CartLine
    {
        public int Id { get; set; }

        public ICollection<Product> Products { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}