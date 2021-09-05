using BedLinenStore.WEB.Enums;

namespace BedLinenStore.WEB.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public CartLine CartLine { get; set; }
    }
}
