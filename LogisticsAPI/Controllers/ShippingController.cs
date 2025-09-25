using AutoMapper.Configuration.Conventions;
using LogisticsAPI.Models.DTOs.Shipment;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using LogisticsAPI.Models.DTOs;
using LogisticsAPI.Models.DTOs.Shipping;

namespace LogisticsAPI.Controllers
{
    [ApiController]
    [Route("api/shipping")]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _service;

        public ShippingController(IShippingService service)
        {
            _service = service;
        }

        [HttpPost("availability")]
        [SwaggerOperation(Summary = "Check if a destination is serviceable for shipping.")]
        [ProducesResponseType(typeof(AvailabilityResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AvailabilityResponseDTO>> CheckAvailability([FromBody] AvailabilityRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.CheckAvailabilityAsync(dto);

                if (result == null)
                    return BadRequest(new { error = "Unable to validate destination." });

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.ParamName + " is invalid." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }

        }

    }
}
