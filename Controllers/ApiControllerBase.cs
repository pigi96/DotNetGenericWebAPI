using Microsoft.AspNetCore.Mvc;

namespace GenericWebAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class ApiControllerBase : Controller
{
    
}