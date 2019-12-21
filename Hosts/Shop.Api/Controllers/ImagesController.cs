using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ImagesController : BaseController
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet("{imagePath}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage([FromRoute]string imagePath)
        {
            var image = await _imageRepository.GetImage(imagePath).ConfigureAwait(false);

            var extension = Path.GetExtension(imagePath);
            return File(image, ContentType(extension));
        }

        private string ContentType(string extension)
        {
            switch (extension)
            {
                case ".jpg":
                    return "image/jpeg";

                case ".png":
                    return "image/png";

                default:
                    return "image/jpeg";
            }
        }
    }
}
