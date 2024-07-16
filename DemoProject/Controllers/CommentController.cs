using DemoProject.Dtos.Comment;
using DemoProject.Interfaces;
using DemoProject.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDtos = comments.Select(c => c.ToCommentDto());
            return Ok(commentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) { 
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromRoute]int stockId, CreateCommentRequestDto createCommentDto)
        {
            if(!await _stockRepository.StockExist(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = createCommentDto.ToCommentFromCreateDto(stockId);
            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(Get), new {id=commentModel.Id},commentModel.ToCommentDto());

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateCommentRequestDto updateCommentRequestDto)
        {
            //var commentModel = await _commentRepository.UpdateAsync(id, updateCommentRequestDto.ToCommentFromUpdateDto());
            var commentModel = await _commentRepository.UpdateAsync(id, updateCommentRequestDto);
            if (commentModel == null)
            {
                return NotFound("Comment is not found");
            }

            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentRepository.DeleteAsync(id);
            if (comment == null)
            {
                return NotFound("Comment is not found");
            }
            return NoContent();
        }
    }
}
