using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LogisticsAPI.Models.DTOs.Address;
using LogisticsAPI.Models.Entities;
using LogisticsAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace LogisticsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;

        public AddressController(IAddressService service)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new Address.")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AddressCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.CreateAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.ParamName + " cannot be null" });
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all Addresss.")]
        [ProducesResponseType(typeof(IEnumerable<AddressReadDTO>, StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { erro = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieves a specific Address by ID.")]
        [ProducesResponseType(typeof(AddressReadDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                if (dto == null)
                    return NotFound();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Updates a Address record.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] AddressUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _service.UpdateAsync(dto);
                if (!success)
                    return NotFound();

                return Ok();
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

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a Address by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }

        }
    }
}