using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSQLCRUD.Models;
using WebSQLCRUD.Services.Author;

namespace WebSQLCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorInterface _authorInterface;
        public AuthorController(IAuthorInterface autorInterface)
        {
            _authorInterface = autorInterface;
        }

        [HttpGet("ListAuthors")]
        public async Task<ActionResult<ResponseModel<List<AuthorModel>>>> ListAuthors()
        {
            var authors = await _authorInterface.ListAuthors();
            return Ok(authors);
        }

        [HttpGet("GetAuthorId/{idAuthor}")]
        public async Task<ActionResult<ResponseModel<AuthorModel>>> GetAuthorId(int idAuthor)
        {
            var author = await _authorInterface.GetAuthorId(idAuthor);
            return Ok(author);
        }
    }
}
