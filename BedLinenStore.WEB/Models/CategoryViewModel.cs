using BedLinenStore.WEB.Models.Entities;
using System.Collections.Generic;

namespace BedLinenStore.WEB.Models
{
    public class CategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public int BedLineId { get; set; }
    }
}