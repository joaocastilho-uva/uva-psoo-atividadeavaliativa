namespace ArteConexao.Repositories.Interfaces
{
    public interface IImagemRepository
    {
        Task<string> UploadAsync(IFormFile imagem);
    }
}
