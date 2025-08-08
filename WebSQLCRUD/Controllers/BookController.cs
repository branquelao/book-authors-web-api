using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSQLCRUD.DTO.Book;
using WebSQLCRUD.Models;
using WebSQLCRUD.Services.Book;
using WebSQLCRUD.Services.Book;

namespace WebSQLCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookInterface _bookInterface;
        public BookController(IBookInterface autorInterface)
        {
            _bookInterface = autorInterface;
        }

        [HttpGet("ListBooks")]
        public async Task<ActionResult<ResponseModel<List<BookModel>>>> ListBooks()
        {
            var books = await _bookInterface.ListBooks();
            return Ok(books);
        }

        [HttpGet("GetBookId/{idBook}")]
        public async Task<ActionResult<ResponseModel<BookModel>>> GetBookId(int idBook)
        {
            var book = await _bookInterface.GetBookId(idBook);
            return Ok(book);
        }

        [HttpGet("GetBookAuthorId/{idAuthor}")]
        public async Task<ActionResult<ResponseModel<BookModel>>> GetBookAuthorId(int idAuthor)
        {
            var book = await _bookInterface.GetBookAuthorId(idAuthor);
            return Ok(book);
        }

        [HttpPost("CreateBook")]
        public async Task<ActionResult<ResponseModel<List<BookModel>>>> CreateBook(CreateBookDTO createBookDTO)
        {
            var books = await _bookInterface.CreateBook(createBookDTO);
            return Ok(books);
        }

        [HttpPut("UpdateBook")]
        public async Task<ActionResult<ResponseModel<List<BookModel>>>> UpdateBook(UpdateBookDTO updateBookDTO)
        {
            var books = await _bookInterface.UpdateBook(updateBookDTO);
            return Ok(books);
        }

        [HttpDelete("DeleteBook")]
        public async Task<ActionResult<ResponseModel<List<BookModel>>>> DeleteBook(int idBook)
        {
            var books = await _bookInterface.DeleteBook(idBook);
            return Ok(books);
        }
    }
}
