using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Models;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // =========================================
        // إنشاء طلب من السلة
        // =========================================

        public async Task<ApiResponse<OrderDTO>> CreateOrderFromCartAsync(int userId, CreateOrderDTO dto)
        {
            // 1) جلب السلة + المنتجات
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartItems == null || cart.CartItems.Count == 0)
                return ApiResponse<OrderDTO>.ErrorResponse("السلة فارغة.");

            // 2) جلب العنوان والتأكد أنه للمستخدم
            var address = await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.AddressId == dto.AddressId && a.UserId == userId);

            if (address == null)
                return ApiResponse<OrderDTO>.ErrorResponse("عنوان التوصيل غير صالح.");

            if (address.Latitude == null || address.Longitude == null)
                return ApiResponse<OrderDTO>.ErrorResponse("العنوان لا يحتوي على موقع جغرافي (GPS).");

            // 3) حساب الإجمالي وبناء عناصر الطلب
            decimal total = 0m;
            var orderItems = new List<OrderItem>();

            foreach (var item in cart.CartItems)
            {
                var product = item.Product;
                if (product == null)
                    continue;

                // الكمية (لو صفر أو أقل نخليها 1)
                int quantity = item.Quantity ?? 1;
                if (quantity <= 0)
                    quantity = 1;

                // السعر: نحاول نأخذ من السطر، ثم PriceAtTime، ثم سعر المنتج
                decimal unitPrice = 0m;

                // لو خصائصك nullable (decimal?) استخدم هذا:
                // unitPrice = item.UnitPrice
                //     ?? item.PriceAtTime
                //     ?? product.Price
                //     ?? 0m;

                // لو خصائصك ليست nullable (decimal) استخدم هذا الكود:
                unitPrice = item.UnitPrice ?? 0;
                if (unitPrice <= 0 && item.PriceAtTime > 0)
                    unitPrice = item.PriceAtTime ?? 0;
                if (unitPrice <= 0 && product.Price.HasValue && product.Price.Value > 0)
                    unitPrice = product.Price.Value;

                // جمع الإجمالي
                total += unitPrice * quantity;

                // إضافة عنصر طلب
                orderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = unitPrice * quantity
                });

                // خصم الكمية من المخزون لو كان stock nullable
                if (product.Stock.HasValue)
                    product.Stock = Math.Max(0, product.Stock.Value - quantity);
            }

            // 4) إنشاء الطلب
            var order = new Order
            {
                UserId = userId,
                DeliveryAddressId = dto.AddressId,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Pending",
                Status = "Pending",
                DeliveryStatus = "Pending",
                TotalAmount = total,
                CreatedAt = DateTime.UtcNow,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            // 5) مسح السلة
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            // 6) تجهيز النتيجة
            var result = new OrderDTO
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                DeliveryStatus = order.DeliveryStatus,
                Items = order.OrderItems.Select(i => new OrderItemDTO
                {
                    OrderItemId = i.OrderItemId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };

            return ApiResponse<OrderDTO>.SuccessResponse(result, "تم إنشاء الطلب بنجاح.");
        }

        // =========================================
        // جلب طلب واحد للمستخدم
        // =========================================
        public async Task<ApiResponse<OrderDTO>> GetOrderByIdAsync(int userId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Coupon)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
                return ApiResponse<OrderDTO>.ErrorResponse("الطلب غير موجود.");

            var dto = new OrderDTO
            {
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                DeliveryStatus = order.DeliveryStatus,
                CouponCode = order.Coupon?.Code,
                DiscountAmount = order.DiscountAmount,
                Items = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductNameAr = oi.Product?.NameAr,
                    ProductNameEn = oi.Product?.NameEn,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            };

            return ApiResponse<OrderDTO>.SuccessResponse(dto, "تم جلب الطلب بنجاح.");
        }

        // =========================================
        // جلب كل طلبات المستخدم
        // =========================================
        public async Task<ApiResponse<List<OrderDTO>>> GetUserOrdersAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Coupon)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var list = orders.Select(o => new OrderDTO
            {
                OrderId = o.OrderId,
                TotalAmount = o.TotalAmount,
                PaymentMethod = o.PaymentMethod,
                CreatedAt = o.CreatedAt,
                DeliveryStatus = o.DeliveryStatus,
                CouponCode = o.Coupon?.Code,
                DiscountAmount = o.DiscountAmount,
                Items = o.OrderItems.Select(oi => new OrderItemDTO
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductNameAr = oi.Product?.NameAr,
                    ProductNameEn = oi.Product?.NameEn,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            }).ToList();

            return ApiResponse<List<OrderDTO>>.SuccessResponse(list, "تم جلب الطلبات بنجاح.");
        }

        // =========================================
        // إلغاء الطلب
        // =========================================
        public async Task<ApiResponse<bool>> CancelOrderAsync(int userId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
                return ApiResponse<bool>.ErrorResponse("الطلب غير موجود.");

            if (order.DeliveryStatus != "Pending")
                return ApiResponse<bool>.ErrorResponse("لا يمكن إلغاء الطلب بعد بدء معالجته.");

            // إعادة الكميات إلى المخزون
            foreach (var item in order.OrderItems)
            {
                if (item.Product != null && item.Product.Stock.HasValue)
                    item.Product.Stock += item.Quantity;
            }

            order.DeliveryStatus = "Canceled";
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم إلغاء الطلب بنجاح.");
        }

        // =========================================
        // تحديث طريقة الدفع
        // =========================================
        public async Task<ApiResponse<bool>> UpdatePaymentMethodAsync(int userId, int orderId, string newMethod)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
                return ApiResponse<bool>.ErrorResponse("الطلب غير موجود.");

            if (order.DeliveryStatus != "Pending")
                return ApiResponse<bool>.ErrorResponse("لا يمكن تعديل طريقة الدفع بعد بدء التجهيز.");

            order.PaymentMethod = newMethod;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "تم تحديث طريقة الدفع.");
        }
    }
}