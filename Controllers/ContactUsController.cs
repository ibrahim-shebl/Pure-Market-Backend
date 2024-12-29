using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Dtos;
using test.DTOs;
using test.Models;
using test.Services;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUs _contactus;
        private readonly IMapper _mapper;
        public ContactUsController(IContactUs contactus, IMapper mapper)
        {
            _contactus = contactus;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var contacts = await _contactus.GetAll();
            
            var data = _mapper.Map<IEnumerable<ContactusDetails>>(contacts);


            return Ok(data);
        }
       

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ContactUsDto dto)
        {
            
            var contact = _mapper.Map<Contact>(dto);
            _contactus.Add(contact);

            return Ok(contact);
        }

         
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var contact = await _contactus.GetById(id);
            if (contact == null)
                return NotFound($"No product was found with ID {id}");
            

            _contactus.Delete(contact);

            return Ok(contact);
        }
    }
}
