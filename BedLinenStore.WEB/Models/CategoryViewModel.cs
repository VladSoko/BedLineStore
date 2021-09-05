using System.Collections.Generic;
using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Models
{
    public class CategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public int BedLineId { get; set; }
    }
}