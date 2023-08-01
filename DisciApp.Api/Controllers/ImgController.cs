using DisciApp.Api.DataBaseContext;
using DisciApp.Api.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DisciApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ImgController : ControllerBase
    {
        private readonly ImgDbContext _context;

        public ImgController(ImgDbContext context)
        {
            _context = context;
        }



        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var base64String = "";

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                // Convert byte[] to Base64 String
                base64String = Convert.ToBase64String(imageBytes);
            }

            var myEntity = new Img
            {
                ImageBase64 = base64String
                
            };

            _context.Imgs.Add(myEntity);
            
            await _context.SaveChangesAsync();
            

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var myEntity = await _context.Imgs.FindAsync(id);

            if (myEntity == null)
            {
                return NotFound();
            }

            var imageBytes = Convert.FromBase64String(myEntity.ImageBase64);

            return File(imageBytes, "image/jpeg");
        }
    }
}
