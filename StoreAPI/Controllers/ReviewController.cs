using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAPI.DTOs;
using StoreAPI.Helpers;
using StoreAPI.Services.Interfaces;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirst("id")!.Value);

        [HttpPost]
        public async Task<IActionResult> AddReview(CreateReviewDTO dto) =>
            Ok(await _service.AddReviewAsync(GetUserId(), dto));

        [HttpPut("{reviewId}")]
        public async Task<IActionResult> Update(int reviewId, UpdateReviewDTO dto) =>
            Ok(await _service.UpdateReviewAsync(GetUserId(), reviewId, dto));

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete(int reviewId) =>
            Ok(await _service.DeleteReviewAsync(GetUserId(), reviewId));

        [AllowAnonymous]
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetReviews(int productId) =>
            Ok(await _service.GetProductReviewsAsync(productId));
    }
}