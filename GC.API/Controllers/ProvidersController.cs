using AutoMapper;
using GlobalConnect.Application.Modules.Provider.Interfaces;
using GlobalConnect.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProvidersController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProvider(int id)
        {
            var providerDto = await _providerService.GetProviderByIdAsync(id);
            if (providerDto == null)
            {
                return NotFound(new { message = $"Provider with ID {id} not found." });
            }

            return Ok(providerDto);
        }

        // POST: api/Books
        //[HttpPost]
        //public ActionResult AddBook(BookToAddDto book)
        //{
        //    var newBook = _mapper.Map<BookToAddDto, User>(book);
        //    _bookRepository.Add(newBook);
        //    return Ok("Book added successfully");
        //}

        //// POST: api/Books/2
        //[HttpPut("{id}")]
        //public ActionResult<BookToReturnDto> UpdateBook(int id,BookToAddDto updatedBook)
        //{
        //    var newBookData = _mapper.Map<BookToAddDto, User>(updatedBook);
        //    _bookRepository.Update(id, newBookData);
        //    return _mapper.Map<User, BookToReturnDto>(newBookData);
        //}

        //// DELETE: api/Books 
        //[HttpDelete("{id}")]
        //public ActionResult DeleteBook(int id)
        //{
        //    _bookRepository.Delete(id);
        //    return Ok();
        //}
    }
}
