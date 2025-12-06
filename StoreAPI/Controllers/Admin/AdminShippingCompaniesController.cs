using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/shipping-companies")]
    [Authorize(Roles = "Admin")]
    public class ShippingCompaniesController : ControllerBase
    {
        private readonly IShippingCompanyService _service;

        public ShippingCompaniesController(IShippingCompanyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) =>
            Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(CreateShippingCompanyDTO dto) =>
            Ok(await _service.CreateAsync(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateShippingCompanyDTO dto) =>
            Ok(await _service.UpdateAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) =>
            Ok(await _service.DeleteAsync(id));
    }
}