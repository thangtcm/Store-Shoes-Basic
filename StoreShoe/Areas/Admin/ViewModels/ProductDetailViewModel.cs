using Microsoft.AspNetCore.Mvc.Rendering;
using StoreShoe.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreShoe.Areas.Admin.ViewModels
{
    public class ProductDetailViewModel
    {
        [Required]
        [DisplayName("Tên sản phẩm")]
        public string? ProductName { get; set; }

        [Required]
        [DisplayName("Mô tả sản phẩm")]
        public string? ProductDescription { get; set; }

        [Required]
        [DisplayName("Giá sản phẩm")]
        public float ProductPrice { get; set; }

        [DisplayName("Giảm giá sản phẩm")]
        public float ProductSale { get; set; }

        [Required]
        [DisplayName("Số lượng tồn kho")]
        public int Inventory { get; set; }

        [DisplayName("Hình ảnh sản phẩm")]
        public string? ProductImage { get; set; }

        [DisplayName("Màu sắc")]
        public List<int>? SelectedColorIds { get; set; }

    }
}
