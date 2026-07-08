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
    public class RedemptionsController : ControllerBase
    {
        private readonly IRedemptionService _redemptionService;

        public RedemptionsController(IRedemptionService redemptionService)
        {
            _redemptionService = redemptionService;
        }

        /// <summary>
        /// Redeem a gift card
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(RedemptionResponse), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RedemptionResponse>> RedeemGiftCard([FromBody] CreateRedemptionRequest request)
        {
            try
            {
                var response = await _redemptionService.RedeemGiftCardAsync(request);
                return CreatedAtAction(nameof(GetRedemption), new { id = response.Id }, response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get redemption by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RedemptionResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RedemptionResponse>> GetRedemption(Guid id)
        {
            try
            {
                var response = await _redemptionService.GetRedemptionAsync(id);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all redemptions
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RedemptionResponse>), 200)]
        public async Task<ActionResult<IEnumerable<RedemptionResponse>>> GetAllRedemptions()
        {
            var response = await _redemptionService.GetAllRedemptionsAsync();
            return Ok(response);
        }

        /// <summary>
        /// Get redemptions by gift card ID
        /// </summary>
        [HttpGet("gift-card/{giftCardId}")]
        [ProducesResponseType(typeof(IEnumerable<RedemptionResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<RedemptionResponse>>> GetRedemptionsByGiftCard(Guid giftCardId)
        {
            try
            {
                var response = await _redemptionService.GetRedemptionsByGiftCardAsync(giftCardId);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
