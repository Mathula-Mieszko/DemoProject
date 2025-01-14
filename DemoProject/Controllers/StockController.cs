﻿using DemoProject.data;
using DemoProject.Dtos.Stock;
using DemoProject.Helpers;
using DemoProject.Interfaces;
using DemoProject.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStockRepository _stockRepository;
        public StockController(ApplicationDbContext dbContext, IStockRepository stockRepository)
        {
            _dbContext = dbContext;   
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult>Get([FromQuery]QueryObject query) {



            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stocks = await _stockRepository.GetAllAsync(query);

            var stockDtos = stocks.Select(s=>s.ToStockDto()).ToList();

            return Ok(stockDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute]int id) {
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null) {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockRequestDto)
        {
            var stockModel =  stockRequestDto.ToStockFromCreateDto();
            await _stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(Get), new {id = stockModel.Id},stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockRequestDto)
        {
            var stockModel = await _stockRepository.UpdateAsync(id, stockRequestDto);

            if (stockModel == null) {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepository.DeleteAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }
            return NoContent();

        }


    }
}
