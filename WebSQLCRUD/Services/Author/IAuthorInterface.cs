using WebSQLCRUD.Models;

namespace WebSQLCRUD.Services.Author
{
    public interface IAuthorInterface
    {
        Task<ResponseModel<List<AuthorModel>>> ListAuthors();
        Task<ResponseModel<AuthorModel>> GetAuthorId(int id);
        Task<ResponseModel<AuthorModel>> GetAuthorBookId(int idLivro);
    }
}
