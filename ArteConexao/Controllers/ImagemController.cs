using ArteConexao.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ArteConexao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagemController : Controller
    {
        private readonly IImagemRepository imagemRepository;

        public ImagemController(IImagemRepository imagemRepository)
        {
            this.imagemRepository = imagemRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile imagem)
        {
            var imagemUrl = await imagemRepository.UploadAsync(imagem);

            if (imagemUrl == null)
            {
                return Problem("Não foi possível carregar a imagem.", null, (int)HttpStatusCode.InternalServerError);
            }

            return Json(new { link = imagemUrl });
        }
    }
}
