using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Netflix.BusinessLogic.Interfaces;
using Netflix.Presentation.DTO;

namespace Netflix.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController(ICatalogService catalogService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContentDto>>> GetAll()
    {
        var contents = await catalogService.GetAllContentAsync();
        return Ok(mapper.Map<IEnumerable<ContentDto>>(contents));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContentDetailDto>> GetById(int id)
    {
        var content = await catalogService.GetContentDetailsAsync(id);
        if (content == null)
            return NotFound();

        var reviews = await catalogService.GetReviewsForContentAsync(id);
        var ratings = await catalogService.GetRatingsForContentAsync(id);

        content.Reviews = reviews.ToList();
        content.Ratings = ratings.ToList();

        return Ok(mapper.Map<ContentDetailDto>(content));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ContentDto>>> Search([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Title query parameter is required.");

        var results = await catalogService.SearchContentByTitleAsync(title);
        return Ok(mapper.Map<IEnumerable<ContentDto>>(results));
    }

    [HttpGet("genre/{genreName}")]
    public async Task<ActionResult<IEnumerable<ContentDto>>> GetByGenre(string genreName)
    {
        var results = await catalogService.GetContentByGenreAsync(genreName);
        return Ok(mapper.Map<IEnumerable<ContentDto>>(results));
    }

    [HttpGet("genres")]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAllGenres()
    {
        var genres = await catalogService.GetAllGenresAsync();
        return Ok(mapper.Map<IEnumerable<GenreDto>>(genres));
    }
}
