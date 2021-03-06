using System;
using System.Linq;
using System.Collections.Generic;
using Play.Catalog.Service.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Play.Catalog.Service.Controllers {
    [ApiController]  // Automatically routes into the routes defined below or handles the bad routes
    [Route("items")]  // It handles the route /items where the base URL is https://localhost:7278
    public class ItemsController : ControllerBase {
        public static readonly List<ItemDto> items = new() {
            new ItemDto(
                Guid.NewGuid(),
                "Potion",
                "Restores 10% of HP",
                5,
                DateTimeOffset.UtcNow
            ),
            new ItemDto(
                Guid.NewGuid(),
                "Antidote",
                "Cures Poison",
                7,
                DateTimeOffset.UtcNow
            ),
            new ItemDto(
                Guid.NewGuid(),
                "Bronze Sword",
                "Deals damage equal to 10% of enemy HP",
                20,
                DateTimeOffset.UtcNow
            ),
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get() {
            return items;
        }

        [HttpGet("{Id}")]
        public ItemDto GetById(Guid Id) {
            return items.Where(item => item.Id == Id).SingleOrDefault();
        }

        // This method allows us to return different types
        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto) {
            var item = new ItemDto(
                Guid.NewGuid(),
                createItemDto.Name,
                createItemDto.Description,
                createItemDto.Price,
                DateTimeOffset.UtcNow
            );
            items.Add(item);
            return CreatedAtAction(
                nameof(GetById),
                new {
                    id = item.Id
                },
                item
            );
        }

        // This method IActionResult doesn't return anything. This is logical as we update the record and we don't reutrn anything.
        // This is for updating the details of an existing item
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto) {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
            var updatedItem = existingItem with {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price,
            };
            var index = items.FindIndex(item => item.Id == id);
            items[index] = updatedItem;

            return NoContent();
        }
    }
}