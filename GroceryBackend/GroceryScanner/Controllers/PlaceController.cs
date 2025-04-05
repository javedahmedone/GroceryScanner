using GroceryScanner.Business.Service.Place;
using Microsoft.AspNetCore.Mvc;


    [ApiController]
    [Route("api/Place")]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _placeService;
        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet("PlacesBySearchItem")]
        public async Task<IActionResult> PlacesBySearchItemAsync(string searchItem)
        {
            var response = await _placeService.GetPlaceBySearchItem(searchItem);
            return Ok(response);
        }

        [HttpGet("PlaceById")]
        public async Task<IActionResult> PlacesByPlaceIdAsync(string placeId)
        {
            var response = await _placeService.PlacesByPlaceIdAsync(placeId);
            return Ok(response);
        }

        [HttpGet("PlaceByLonAndLat")]
        public async Task<IActionResult> PlaceByLonAndLatAsync(string longtitude, string latitude)
        {
            var response = await _placeService.PlaceByLonAndLatAsync(longtitude,latitude);
            return Ok(response);
        }
    }

