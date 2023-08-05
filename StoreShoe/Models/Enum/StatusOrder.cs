using System.ComponentModel.DataAnnotations;

namespace StoreShoe.Models.Enum
{
    public enum StatusOrder : int
    {
        [Display(Name = "Đang Duyệt")]
        Pending = 0,

        [Display(Name = "Đang Chờ Giao Hàng")]
        WaitingForDelivery = 1,

        [Display(Name = "Đã Giao Hàng")]
        Delivered = 2
    }

}
