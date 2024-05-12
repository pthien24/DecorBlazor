using Decor_Models;
using System.ComponentModel.DataAnnotations;

namespace DecorWeb_client.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM() { 
            ProductPrice = new ();
            Count = 1;
        }
        [Range(1,int.MaxValue,ErrorMessage ="please enter a value greater than 0")]
        public int Count { get; set; }
        [Required]
        public int SelectedProductPriceID { get; set; }
        public ProductPriceDTO ProductPrice{ get; set; }
    }
}
