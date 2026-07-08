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
    public class GiftCardsController : ControllerBase
    {
        private readonly IGiftCardService _giftCardService;

        public GiftCardsController(IGiftCardService giftCardService)
        {
            _giftCardService = giftCardService;
        }

        /// <summary>
        /// Create a new gift card
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GiftCardResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<GiftCardResponse>> CreateGiftCard([FromBody] CreateGiftCardRequest request)
        {
            try
            {
                var response = await _giftCardService.CreateGiftCardAsync(request);
                return CreatedAtAction(nameof(GetGiftCard), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get gift card by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GiftCardResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GiftCardResponse>> GetGiftCard(Guid id)
        {
            try
            {
                var response = await _giftCardService.GetGiftCardAsync(id);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get gift card by code
        /// </summary>
        [HttpGet("code/{code}")]
        [ProducesResponseType(typeof(GiftCardResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GiftCardResponse>> GetGiftCardByCode(string code)
        {
            try
            {
                var response = await _giftCardService.GetGiftCardByCodeAsync(code);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all gift cards
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GiftCardResponse>), 200)]
        public async Task<ActionResult<IEnumerable<GiftCardResponse>>> GetAllGiftCards()
        {
            var response = await _giftCardService.GetAllGiftCardsAsync();
            return Ok(response);
        }

        /// <summary>
        /// Update a gift card
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GiftCardResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<GiftCardResponse>> UpdateGiftCard(Guid id, [FromBody] UpdateGiftCardRequest request)
        {
            try
            {
                var response = await _giftCardService.UpdateGiftCardAsync(id, request);
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
        /// Delete a gift card
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGiftCard(Guid id)
        {
            var result = await _giftCardService.DeleteGiftCardAsync(id);
            if (!result)
                return NotFound(new { message = $"Gift card with ID {id} not found" });

            return NoContent();
        }

        /// <summary>
        /// Get gift card balance
        /// </summary>
        [HttpGet("{id}/balance")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetBalance(Guid id)
        {
            try
            {
                var balance = await _giftCardService.GetBalanceAsync(id);
                return Ok(new { balance });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
