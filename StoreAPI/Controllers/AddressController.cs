using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]   // المستخدم العادي
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // هيلبر لجلب userId من الـ JWT
        private int GetUserId()
        {
            var idClaim = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(idClaim))
                throw new Exception("التوكن لا يحتوي على معرف المستخدم.");

            if (!int.TryParse(idClaim, out int userId))
                throw new Exception("معرف المستخدم في التوكن غير صالح.");

            return userId;
        }

        // ================================
        // GET: api/Addresses
        // ================================
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<AddressDTO>>>> GetMyAddresses()
        {
            int userId = GetUserId();
            var result = await _addressService.GetUserAddressesAsync(userId);
            return Ok(result);
        }

        // ================================
        // GET: api/Addresses/5
        // ================================
        [HttpGet("{addressId}")]
        public async Task<ActionResult<ApiResponse<AddressDTO>>> GetById(int addressId)
        {
            int userId = GetUserId();
            var result = await _addressService.GetByIdAsync(userId, addressId);
            return Ok(result);
        }

        // ================================
        // POST: api/Addresses
        // ================================
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AddressDTO>>> Create([FromBody] CreateAddressDTO dto)
        {
            int userId = GetUserId();
            var result = await _addressService.CreateAsync(userId, dto);
            return Ok(result);
        }

        // ================================
        // PUT: api/Addresses/5
        // ================================
        [HttpPut("{addressId}")]
        public async Task<ActionResult<ApiResponse<AddressDTO>>> Update(int addressId, [FromBody] UpdateAddressDTO dto)
        {
            int userId = GetUserId();
            var result = await _addressService.UpdateAsync(userId, addressId, dto);
            return Ok(result);
        }

        // ================================
        // DELETE: api/Addresses/5
        // ================================
        [HttpDelete("{addressId}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int addressId)
        {
            int userId = GetUserId();
            var result = await _addressService.DeleteAsync(userId, addressId);
            return Ok(result);
        }
    }
}