using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GiftCardSystem.Application.DTOs;
using GiftCardSystem.Application.Services;

namespace GiftCardSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            try
            {
                var response = await _customerService.CreateCustomerAsync(request);
                return CreatedAtAction(nameof(GetCustomer), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get customer by ID with their gift cards
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDetailResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CustomerDetailResponse>> GetCustomer(Guid id)
        {
            try
            {
                var response = await _customerService.GetCustomerAsync(id);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerResponse>), 200)]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetAllCustomers()
        {
            var response = await _customerService.GetAllCustomersAsync();
            return Ok(response);
        }

        /// <summary>
        /// Update a customer
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CustomerResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CustomerResponse>> UpdateCustomer(Guid id, [FromBody] CreateCustomerRequest request)
        {
            try
            {
                var response = await _customerService.UpdateCustomerAsync(id, request);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a customer
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result)
                return NotFound(new { message = $"Customer with ID {id} not found" });

            return NoContent();
        }
    }
}
