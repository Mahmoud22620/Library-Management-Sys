using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library_Management_Sys.Models;
using Library_Management_Sys.Repositories.Interfaces;
using AutoMapper;
using Library_Management_Sys.Models.DTOs;

namespace Library_Management_Sys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IMapper _mapper;

        public PublishersController(IGenericRepository<Publisher> publisherRepo, IMapper mapper)
        {
            _publisherRepository = publisherRepo;
            _mapper = mapper;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetPublishers()
        {
            var publishersList = await _publisherRepository.GetAllAsync();
            return Ok(_mapper.Map<List<PublisherDTO>>(publishersList));
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDTO>> GetPublisher(int id)
        {
            var publisher = await _publisherRepository.GetAsync(p => p.PublisherId == id);

            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PublisherDTO>(publisher));
        }

        // PUT: api/Publishers
        [HttpPut]
        public async Task<IActionResult> PutPublisher(PublisherDTO publisherDto)
        {
            var publisher = await PublisherExists(publisherDto.PublisherId);
            if (publisher != null)
            {
                await _publisherRepository.UpdateAsync(_mapper.Map<Publisher>(publisherDto));
                return Ok(new { message = "Publisher updated successfully" });
            }
            else
            {
                return NotFound(new { message = "Publisher Not Found" });
            }
        }

        // POST: api/Publishers
        [HttpPost]
        public async Task<ActionResult<PublisherDTO>> PostPublisher(PublisherDTO publisherDto)
        {
            if (publisherDto == null)
            {
                return BadRequest();
            }
            await _publisherRepository.CreateAndSaveAsync(_mapper.Map<Publisher>(publisherDto));
            return Ok(new { message = "Publisher created successfully" });
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _publisherRepository.GetAsync(p => p.PublisherId == id);
            if (publisher == null)
            {
                return NotFound();
            }

            await _publisherRepository.RemoveAsync(publisher);
            await _publisherRepository.SaveAsync();
            return Ok(new { message = "Publisher deleted successfully" });
        }

        private async Task<Publisher> PublisherExists(int id)
        {
            return await _publisherRepository.GetAsync(p => p.PublisherId == id);
        }
    }
}
