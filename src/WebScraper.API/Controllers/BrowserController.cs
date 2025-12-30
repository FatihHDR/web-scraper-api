using Microsoft.AspNetCore.Mvc;
using WebScraper.API.Models;
using WebScraper.Domain.Interfaces;

namespace WebScraper.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrowserController : ControllerBase
{
    private readonly IBrowserService _browserService;

    public BrowserController(IBrowserService browserService)
    {
        _browserService = browserService;
    }

    [HttpGet("state")]
    public IActionResult GetState()
    {
        return Ok(_browserService.State);
    }

    [HttpPost("navigate")]
    public async Task<IActionResult> Navigate([FromBody] NavigateRequest request)
    {
        var result = await _browserService.NavigateAsync(request.Url);
        if (!result.Success) return BadRequest(result.ErrorMessage);
        
        return Ok(new { result.Message, _browserService.State });
    }

    [HttpGet("content")]
    public async Task<IActionResult> GetContent()
    {
        var result = await _browserService.GetPageContentAsync();
         if (!result.Success) return BadRequest(result.ErrorMessage);

        return Content(result.Data!, "text/html");
    }

    [HttpPost("click")]
    public async Task<IActionResult> Click([FromBody] ClickRequest request)
    {
        var result = await _browserService.ClickElementAsync(request.Selector);
        if (!result.Success) return BadRequest(result.ErrorMessage);

        return Ok(new { result.Message, _browserService.State });
    }

    [HttpPost("type")]
    public async Task<IActionResult> Type([FromBody] TypeRequest request)
    {
        var result = await _browserService.TypeTextAsync(request.Selector, request.Text);
        if (!result.Success) return BadRequest(result.ErrorMessage);
        
        return Ok(new { result.Message });
    }

    [HttpPost("screenshot")]
    public async Task<IActionResult> Screenshot()
    {
        var fileName = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        var result = await _browserService.TakeScreenshotAsync(fileName);
        
        if (!result.Success) return BadRequest(result.ErrorMessage);

        // In a real API we might return the file directly or a URL to it
        // For now, returning the path where it was saved locally is fine
        return Ok(new { result.Message, Path = fileName });
    }
    
    [HttpPost("scroll")]
    public async Task<IActionResult> Scroll([FromBody] ScrollRequest request)
    {
        var result = await _browserService.ScrollAsync(request.Direction, request.Pixels);
        if (!result.Success) return BadRequest(result.ErrorMessage);
        
        return Ok(new { result.Message });
    }

    [HttpPost("script")]
    public async Task<IActionResult> EvaluateScript([FromBody] ScriptRequest request)
    {
        var result = await _browserService.EvaluateScriptAsync(request.Script);
        if (!result.Success) return BadRequest(result.ErrorMessage);
        
        return Ok(new { result.Message, Result = result.Data });
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var result = await _browserService.RefreshAsync();
        if (!result.Success) return BadRequest(result.ErrorMessage);
        
        return Ok(new { result.Message });
    }
    
    [HttpPost("history/back")]
    public async Task<IActionResult> GoBack()
    {
       var result = await _browserService.GoBackAsync();
       if (!result.Success) return BadRequest(result.ErrorMessage);
       
       return Ok(new { result.Message, _browserService.State });
    }
    
    [HttpPost("history/forward")]
    public async Task<IActionResult> GoForward()
    {
        var result = await _browserService.GoForwardAsync();
        if (!result.Success) return BadRequest(result.ErrorMessage);
        
        return Ok(new { result.Message, _browserService.State });
    }
}
