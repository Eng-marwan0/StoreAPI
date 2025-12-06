using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Models
{
    public class ShipmentItem
    {
        [Key]
        public int ShipmentItemId { get; set; }

        public int ShipmentId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // العلاقات
        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}