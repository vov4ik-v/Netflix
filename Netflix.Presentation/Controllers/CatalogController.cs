using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Netflix.BusinessLogic.Interfaces;
using Netflix.Presentation.DTOs;

namespace Netflix.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;
    private readonly IMapper _mapper;

    public CatalogController(ICatalogService catalogService, IMapper mapper)
    {
        _catalogService = catalogService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContentDto>>> GetAll()
    {
        var contents = await _catalogService.GetAllContentAsync();
        return Ok(_mapper.Map<IEnumerable<ContentDto>>(contents));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContentDetailDto>> GetById(int id)
    {
        var content = await _catalogService.GetContentDetailsAsync(id);
        if (content == null)
            return NotFound();

        var reviews = await _catalogService.GetReviewsForContentAsync(id);
        var ratings = await _catalogService.GetRatingsForContentAsync(id);

        content.Reviews = reviews.ToList();
        content.Ratings = ratings.ToList();

        return Ok(_mapper.Map<ContentDetailDto>(content));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ContentDto>>> Search([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Title query parameter is required.");

        var results = await _catalogService.SearchContentByTitleAsync(title);
        return Ok(_mapper.Map<IEnumerable<ContentDto>>(results));
    }

    [HttpGet("genre/{genreName}")]
    public async Task<ActionResult<IEnumerable<ContentDto>>> GetByGenre(string genreName)
    {
        var results = await _catalogService.GetContentByGenreAsync(genreName);
        return Ok(_mapper.Map<IEnumerable<ContentDto>>(results));
    }

    [HttpGet("genres")]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAllGenres()
    {
        var genres = await _catalogService.GetAllGenresAsync();
        return Ok(_mapper.Map<IEnumerable<GenreDto>>(genres));
    }
}
