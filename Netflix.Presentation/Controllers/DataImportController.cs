using Microsoft.AspNetCore.Mvc;
using Netflix.BusinessLogic.Interfaces;
using Netflix.Presentation.DTOs;

namespace Netflix.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataImportController(IDataImportService dataImportService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ImportResultDto>> Import([FromQuery] string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return BadRequest(new ImportResultDto { Message = "filePath query parameter is required." });

        try
        {
            await dataImportService.ImportFromCsvAsync(filePath);
            return Ok(new ImportResultDto { Message = "Import completed successfully." });
        }
        catch (FileNotFoundException)
        {
            return NotFound(new ImportResultDto { Message = $"File not found: {filePath}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ImportResultDto { Message = $"Import failed: {ex.Message}" });
        }
    }
}
