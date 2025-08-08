using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSQLCRUD.DTO.Author;
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

        [HttpGet("GetAuthorBookId/{idBook}")]
        public async Task<ActionResult<ResponseModel<AuthorModel>>> GetAuthorBookId(int idBook)
        {
            var author = await _authorInterface.GetAuthorBookId(idBook);
            return Ok(author);
        }

        [HttpPost("CreateAuthor")]
        public async Task<ActionResult<ResponseModel<List<AuthorModel>>>> CreateAuthor(CreateAuthorDTO createAuthorDTO)
        {
            var authors = await _authorInterface.CreateAuthor(createAuthorDTO);
            return Ok(authors);
        }

        [HttpPut("UpdateAuthor")] 
        public async Task<ActionResult<ResponseModel<List<AuthorModel>>>> UpdateAuthor(UpdateAuthorDTO updateAuthorDTO)
        {
            var authors = await _authorInterface.UpdateAuthor(updateAuthorDTO);
            return Ok(authors);
        }

        [HttpDelete("DeleteAuthor")]
        public async Task<ActionResult<ResponseModel<List<AuthorModel>>>> DeleteAuthor(int idAuthor)
        {
            var authors = await _authorInterface.DeleteAuthor(idAuthor);
            return Ok(authors);
        }
    }
}
