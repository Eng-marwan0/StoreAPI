using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;

        public AddressController(IAddressService service)
        {
            _service = service;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirst("id")!.Value);

        [HttpGet]
        public async Task<IActionResult> GetMyAddresses()
        {
            int userId = GetUserId();
            return Ok(await _service.GetUserAddressesAsync(userId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            int userId = GetUserId();
            return Ok(await _service.GetByIdAsync(userId, id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAddressDTO dto)
        {
            int userId = GetUserId();
            return Ok(await _service.CreateAsync(userId, dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAddressDTO dto)
        {
            int userId = GetUserId();
            return Ok(await _service.UpdateAsync(userId, id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = GetUserId();
            return Ok(await _service.DeleteAsync(userId, id));
        }
    }
}