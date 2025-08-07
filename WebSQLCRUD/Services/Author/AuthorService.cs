using Microsoft.EntityFrameworkCore;
using WebSQLCRUD.Data;
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
        public async Task<ResponseModel<List<AuthorModel>>> ListAuthors()
        {
            ResponseModel<List<AuthorModel>> response = new ResponseModel<List<AuthorModel>>();
            try
            {
                var authors = await _context.Authors.ToListAsync();

                response.data = authors;
                response.message = "Authors retrieved successfully.";

                return response;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = false;
                return response;
            }
        }

        public async Task<ResponseModel<AuthorModel>> GetAuthorBookId(int idLivro)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
    }
}
