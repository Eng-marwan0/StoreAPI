using System.ComponentModel.DataAnnotations;

namespace StoreAPI.DTOs
{
    // ======================================================
    //  المستخدم الأساسي (للرجوع للمستخدم في الطلبات، السلة، ...)
    // ======================================================
    public class UserDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string? Token { get; set; }  // فقط بعد تسجيل الدخول
    }

    // ======================================================
    //  إنشاء حساب جديد (Register)
    // ======================================================
    public class RegisterDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    // ======================================================
    //  تسجيل الدخول (Login)
    // ======================================================
    public class LoginDTO
    {
        [Required]
        public string EmailOrPhone { get; set; }

        [Required]
        public string Password { get; set; }
    }

    // ======================================================
    //  تحديث بيانات المستخدم
    // ======================================================
    public class UpdateUserDTO
    {
        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? NewPassword { get; set; }
    }

    // ======================================================
    //  User Summary — للاستخدام في الطلبات أو السلة إن لزم الأمر
    // ======================================================
    public class UserSummaryDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
}