using HotelListing.API.Models.Country;

namespace HotelListing.API.Models.Hotel
{
    public class HotelWtihDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public double Rating { get; set; }

        public int CountryId { get; set; }

        public BaseCountryDto baseCountryDto { get; set; }
    }
}
