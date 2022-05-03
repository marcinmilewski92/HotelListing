using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Models.Hotel;
using HotelListing.API.Models.Country;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHotelsRepository _repository;

        public HotelsController(IMapper mapper, IHotelsRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels = await _repository.GetAllAsync();
            var hotelsDto = _mapper.Map<List<HotelDto>>(hotels);
            return Ok(hotelsDto);
        }


        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelWtihDetailsDto>> GetHotel(int id)
        {
            var hotel = await _repository.GetDetails(id);

            var hotelDto = _mapper.Map<HotelWtihDetailsDto>(hotel);
            hotelDto.baseCountryDto = _mapper.Map<BaseCountryDto>(hotel.Country);
            
            if (hotel == null)
            {
                return NotFound();
            }

            return hotelDto;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            var hotel = await _repository.GetAsync(id);

            if(hotel == null)
            {
                return NotFound();
            }

            _mapper.Map(hotelDto, hotel);

            try
            {
                await _repository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
        {
            var hotel = _mapper.Map<Hotel>(hotelDto);
            await _repository.AddAsync(hotel);
           

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _repository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id); 
            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _repository.Exists(id);
        }
    }
}
