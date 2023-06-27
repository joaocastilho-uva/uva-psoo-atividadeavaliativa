using ArteConexao.Repositories.Interfaces;
using CloudinaryDotNet;
using System.Net;

namespace ArteConexao.Repositories
{
    public class ImagemRepository : IImagemRepository
    {
        private readonly Account conta;

        public ImagemRepository(IConfiguration configuration)
        {
            conta = new Account(configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]);
        }

        public async Task<string> UploadAsync(IFormFile imagem)
        {
            var cliente = new Cloudinary(conta);

            var uploadFileResponse = await cliente.UploadAsync(
                new CloudinaryDotNet.Actions.ImageUploadParams()
                {
                    File = new FileDescription(imagem.Name, imagem.OpenReadStream()),
                    DisplayName = imagem.Name
                });

            if (uploadFileResponse != null && uploadFileResponse.StatusCode == HttpStatusCode.OK)
            {
                return uploadFileResponse.SecureUri.ToString();
            }

            return null;
        }
    }
}
