using WebSQLCRUD.DTO.Author;
using WebSQLCRUD.DTO.Book;
using WebSQLCRUD.Models;

namespace WebSQLCRUD.Services.Book
{
    public interface IBookInterface
    {
        Task<ResponseModel<List<BookModel>>> ListBooks();
        Task<ResponseModel<BookModel>> GetBookId(int idBook);
        Task<ResponseModel<List<BookModel>>> GetBookAuthorId(int idLivro);
        Task<ResponseModel<List<BookModel>>> CreateBook(CreateBookDTO createBookDTO);
        Task<ResponseModel<List<BookModel>>> UpdateBook(UpdateBookDTO updateBookDTO);
        Task<ResponseModel<List<BookModel>>> DeleteBook(int idAuthor);
    }
}
