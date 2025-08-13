using Microsoft.EntityFrameworkCore;
using WebSQLCRUD.Data;
using WebSQLCRUD.DTO.Author;
using WebSQLCRUD.DTO.Book;
using WebSQLCRUD.Models;

namespace WebSQLCRUD.Services.Book
{
    public class BookService : IBookInterface
    {
        private readonly AppDbContext _context;
        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<BookModel>>> ListBooks(int page = 1, int pageSize = 10)
        {
            ResponseModel<List<BookModel>> response = new ResponseModel<List<BookModel>>();
            try
            {
                var skipCount = (page - 1) * pageSize;
                var books = await _context.Books.OrderBy(e => e.id)
                    .Skip(skipCount)
                    .Take(pageSize)
                    .Include(a => a.author)
                    .ToListAsync();

                response.data = books;
                response.message = "Books retrieved successfully.";

                return response;
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<BookModel>> GetBookId(int idBook)
        {
            ResponseModel<BookModel> response = new ResponseModel<BookModel>();
            try
            {
                var book = await _context.Books.Include(a => a.author).FirstOrDefaultAsync(bookBank => bookBank.id == idBook);

                if (book == null)
                {
                    response.message = "Book not found.";
                    return response;
                }

                response.data = book;
                response.message = "Book retrieved successfully.";

                return response;
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<BookModel>>> GetBookAuthorId(int idAuthor)
        {
            ResponseModel<List<BookModel>> response = new ResponseModel<List<BookModel>>();
            try
            {
                var book = await _context.Books.Include(a => a.author)
                    .Where(bookBank => bookBank.author.id == idAuthor).ToListAsync();

                if (book == null)
                {
                    response.message = "Book not found for the given author ID.";
                    return response;
                }

                response.data = book;
                response.message = "Book retrieved successfully for the given author ID.";
                return response;

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<BookModel>>> CreateBook(CreateBookDTO createBookDTO)
        {
            ResponseModel<List<BookModel>> response = new ResponseModel<List<BookModel>>();

            try
            {
                var author = await _context.Authors
                    .FirstOrDefaultAsync(authorBank => authorBank.id == createBookDTO.author.id);

                if(author == null)
                {
                    response.message = "Author not found.";
                    return response;
                }

                var book = new BookModel
                {
                    title = createBookDTO.title,
                    author = author
                };

                _context.Add(book);
                await _context.SaveChangesAsync();

                response.data = await _context.Books.Include(a => a.author).ToListAsync();

                return response;

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<BookModel>>> UpdateBook(UpdateBookDTO updateBookDTO)
        {
            ResponseModel<List<BookModel>> response = new ResponseModel<List<BookModel>>();

            try
            {
                var book = await _context.Books.Include(a => a.author)
                    .FirstOrDefaultAsync(bookBank => bookBank.id == updateBookDTO.id);  
                var author = await _context.Authors
                    .FirstOrDefaultAsync(authorBank => authorBank.id == updateBookDTO.author.id);

                if (author == null)
                {
                    response.message = "Author not found.";
                    return response;
                }

                if (book == null)
                {
                    response.message = "Book not found.";
                    return response;
                }

                book.title = updateBookDTO.title;
                book.author = author;

                _context.Update(book);
                await _context.SaveChangesAsync();

                response.data = await _context.Books.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<BookModel>>> DeleteBook(int idBook)
        {
            ResponseModel<List<BookModel>> response = new ResponseModel<List<BookModel>>();

            try
            {
                var book = await _context.Books.Include(a => a.author)
                    .FirstOrDefaultAsync(bookBank => bookBank.id == idBook);

                if (book == null)
                {
                    response.message = "Book not found.";
                    return response;
                }

                _context.Remove(book);
                await _context.SaveChangesAsync();

                response.data = await _context.Books.ToListAsync();
                response.message = "Book deleted successfully.";
                return response;
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

    }
}
