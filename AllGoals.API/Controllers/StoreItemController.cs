using AllGoals.Application.DTOs;
using AllGoals.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllGoals.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreItemController : ControllerBase
{
    private readonly IStoreItemService _storeItemService;
    
        public StoreItemController(IStoreItemService storeItemService)
        {
            _storeItemService = storeItemService;
        }

        /// <summary>
        /// Obtém todos os itens da loja.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StoreItemDtoResponse>), 200)]
        public async Task<IActionResult> GetStoreItems()
        {
            var items = await _storeItemService.ListAsync();
            return Ok(items);
        }

        /// <summary>
        /// Obtém um item específico da loja pelo Id.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(StoreItemDtoResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStoreItemById(int id)
        {
            var item = await _storeItemService.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        /// <summary>
        /// Cria um novo item na loja.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(StoreItemDtoResponse), 201)] // Created
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateStoreItem([FromBody] StoreItemDtoRequest createDto)
        {
            var newItemResponse = await _storeItemService.CreateAsync(createDto);

            return CreatedAtAction(
                nameof(GetStoreItemById), 
                new { id = newItemResponse.Id }, 
                newItemResponse
            );
        }

        /// <summary>
        /// Atualiza um item da loja existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateStoreItem(int id, [FromBody] StoreItemDtoRequest updateDto)
        {
            var success = await _storeItemService.UpdateAsync(id, updateDto);

            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Exclui um item da loja.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteStoreItem(int id)
        {
            var success = await _storeItemService.DeleteAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
}