using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace StoreAPI.DTOs
{
    public class ProductUpdateDTO
    {
        public int CategoryId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int Stock { get; set; }

        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}