using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Application.DTOs.CreateUser;
using UsersAPI.Application.UseCases.CreateUser;

namespace UsersAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ICreateUserUseCase _createUserUseCase;

    public UsersController(ICreateUserUseCase createUserUseCase)
    {
        _createUserUseCase = createUserUseCase;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await _createUserUseCase.ExecuteAsync(request);

        if (!result.IsSuccess)
        {
            return result.Error!.Code switch
            {
                "validation_error" => BadRequest(result.Error),
                "domain_validation_error" => BadRequest(result.Error),
                "email_already_exists" => Conflict(result.Error),
                _ => StatusCode(500, result.Error)
            };
        }

        return CreatedAtAction(
            nameof(Create),
            new { id = result.Value!.UserId },
            result.Value
        );
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new { Message = "Authenticated!" });
    }

}
