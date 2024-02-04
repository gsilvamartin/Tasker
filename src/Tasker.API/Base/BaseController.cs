using Microsoft.AspNetCore.Mvc;

namespace Tasker.API.Base;

public class BaseController : Controller
{
    protected IActionResult Success(object? data = null)
    {
        return Ok(new { success = true, data });
    }

    protected IActionResult Error(Exception ex, string errorMessage)
    {
        return BadRequest(new
        {
            success = false,
            error = new
            {
                message = errorMessage,
                details = ex.Message,
                stackTrace = ex.StackTrace
            }
        });
    }

    protected IActionResult NotFound(string message = "Register not found")
    {
        return NotFound(new
        {
            success = false,
            error = new
            {
                message
            }
        });
    }
}