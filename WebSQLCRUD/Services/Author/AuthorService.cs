using Microsoft.EntityFrameworkCore;
using WebSQLCRUD.Data;
using WebSQLCRUD.DTO.Author;
using WebSQLCRUD.Exceptions;
using WebSQLCRUD.Models;

namespace WebSQLCRUD.Services.Author
{
    public class AuthorService : IAuthorInterface
    {
        private readonly AppDbContext _context;
        public AuthorService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel<List<AuthorModel>>> ListAuthors(int page=1, int pageSize=10)
        {
            ResponseModel<List<AuthorModel>> response = new ResponseModel<List<AuthorModel>>();
            try
            {
                var skipCount = (page - 1) * pageSize;
                var authors = await _context.Authors.OrderBy(e => e.id)
                    .Skip(skipCount)
                    .Take(pageSize)
                    .ToListAsync();

                response.data = authors;
                response.message = "Authors retrieved successfully.";

                return response;
            }
            catch (DatabaseException ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<AuthorModel>> GetAuthorId(int idAuthor)
        {
            ResponseModel<AuthorModel> response = new ResponseModel<AuthorModel>();
            try
            {
                var author = await _context.Authors.FirstOrDefaultAsync(authorBank => authorBank.id == idAuthor);
            
                if(author == null)
                {
                    response.message = "Author not found.";
                    return response;
                }

                response.data = author;
                response.message = "Author retrieved successfully.";

                return response;
            }
            catch (ResourceNotFoundException ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<AuthorModel>> GetAuthorBookId(int idBook)
        {
            ResponseModel<AuthorModel> response = new ResponseModel<AuthorModel>();
            try
            {
                var book = await _context.Books.Include(a => a.author).
                    FirstOrDefaultAsync(bookBank => bookBank.id == idBook);

                if (book == null)
                {
                    response.message = "Author not found for the given book ID.";
                    return response;
                }

                response.data = book.author;
                response.message = "Author retrieved successfully for the given book ID.";
                return response;

            }
            catch (ResourceNotFoundException ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<AuthorModel>>> CreateAuthor(CreateAuthorDTO createAuthorDTO)
        {
            ResponseModel<List<AuthorModel>> response = new ResponseModel<List<AuthorModel>>();

            try
            {
                var author = new AuthorModel
                {
                    name = createAuthorDTO.name,
                    surname = createAuthorDTO.surname
                };

                _context.Add(author);
                await _context.SaveChangesAsync();

                response.data = await _context.Authors.ToListAsync();
                response.message = "Author created successfully.";

                return response;
            }
            catch(DatabaseException ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<AuthorModel>>> UpdateAuthor(UpdateAuthorDTO updateAuthorDTO)
        {
            ResponseModel<List<AuthorModel>> response = new ResponseModel<List<AuthorModel>>();

            try
            {
                var author = await _context.Authors.FirstOrDefaultAsync(authorBank => authorBank.id == updateAuthorDTO.id);

                if (author == null)
                {
                    response.message = "Author not found.";
                    return response;
                }

                author.name = updateAuthorDTO.name;
                author.surname = updateAuthorDTO.surname;

                _context.Update(author);
                await _context.SaveChangesAsync();
                
                response.data = await _context.Authors.ToListAsync();
                response.message = "Author updated successfully.";
                return response;
            }
            catch (DatabaseException ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<AuthorModel>>> DeleteAuthor(int idAuthor)
        {
            ResponseModel<List<AuthorModel>> response = new ResponseModel<List<AuthorModel>>();

            try
            {
                var author = await _context.Authors.FirstOrDefaultAsync(authorBank => authorBank.id == idAuthor);

                if (author == null)
                {
                    response.message = "Author not found.";
                    return response;
                }

                _context.Remove(author);
                await _context.SaveChangesAsync();

                response.data = await _context.Authors.ToListAsync();
                response.message = "Author deleted successfully.";
                return response;
            }
            catch (DatabaseException ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }
    }
}
