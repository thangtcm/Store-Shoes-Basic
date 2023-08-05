using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreShoe.ViewModels
{
    public class NewProductVM
    {
        public int Id { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Không bỏ trống mục này")]
        public string? ProductName { get; set; }

        [DisplayName("Mô tả sản phẩm")]
        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "Không bỏ trống mục này")]
        [DisplayName("Giá sản phẩm")]
        public float ProductPrice { get; set; }

        [DisplayName("Giảm giá sản phẩm")]
        public float ProductSale { get; set; }

        [Required(ErrorMessage = "Không bỏ trống mục này")]
        [DisplayName("Số lượng tồn kho")]
        public int Inventory { get; set; }

        [Required(ErrorMessage = "Không bỏ trống mục này")]
        [DisplayName("Kích cỡ")]
        public int Size { get; set; }

        public string? ProductImage { get; set; }
    }
}
