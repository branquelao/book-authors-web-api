using WebSQLCRUD.DTO.Author;
using WebSQLCRUD.Models;

namespace WebSQLCRUD.Services.Author
{
    public interface IAuthorInterface
    {
        Task<ResponseModel<List<AuthorModel>>> ListAuthors(int page=1, int pageSize=10);
        Task<ResponseModel<AuthorModel>> GetAuthorId(int idAuthor);
        Task<ResponseModel<AuthorModel>> GetAuthorBookId(int idLivro);
        Task<ResponseModel<List<AuthorModel>>> CreateAuthor(CreateAuthorDTO createAuthorDTO);
        Task<ResponseModel<List<AuthorModel>>> UpdateAuthor(UpdateAuthorDTO updateAuthorDTO);
        Task<ResponseModel<List<AuthorModel>>> DeleteAuthor(int idAuthor);
    }
}
