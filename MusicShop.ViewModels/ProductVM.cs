
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MusicShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicShop.ViewModels
{
    public class ProductVM
    {

        

        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

    }
}
