using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicShop.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Range(1,1000,ErrorMessage ="Please Enter A Value Between 1 and 1000")]
        public int Count {  get; set; }
        
        public int ProductId {  get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ValidateNever]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]

        [ValidateNever]

        public ApplicationUser ApplicationUser { get; set; }


    }
}
