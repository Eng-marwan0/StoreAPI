using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace StoreAPI.DTOs
{
    // ============================================
    // Order (Read Only DTO)
    // ============================================
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string? OrderNumber { get; set; }

        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public string? DeliveryStatus { get; set; }

        public string? CouponCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderItemDTO> Items { get; set; } = new();
    }

    // ============================================
    // Order Item (Included inside OrderDTO)
    // ============================================
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }

        public int ProductId { get; set; }
        public string? ProductNameAr { get; set; }
        public string? ProductNameEn { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    // ============================================
    // Create Order From Cart
    // ============================================
    public class CreateOrderDTO
    {
        public int AddressId { get; set; }
        public string PaymentMethod { get; set; }
        public string? Notes { get; set; }

        // GPS
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    // ============================================
    // Update Order Status (Admin Only)
    // ============================================
    public class UpdateOrderStatusDTO
    {
        public string Status { get; set; }
        public string? DeliveryStatus { get; set; }
    }

    // ============================================
    // Update Payment Method
    // ============================================
    public class UpdatePaymentMethodDTO
    {
        public string NewPaymentMethod { get; set; }
    }

    // ============================================
    // Apply Coupon Code
    // ============================================
    public class ApplyCouponDTO
    {
        public string CouponCode { get; set; }
    }
}