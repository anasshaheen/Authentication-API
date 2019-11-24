using ApiAuth.API.DtoS;
using ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser;
using ApiAuth.API.Infrastructure.Abstraction.Queries.AppUser;
using ApiAuth.API.Infrastructure.Extensions;
using ApiAuth.API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiAuth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me/providers")]
        public async Task<IActionResult> GetUserProviders()
        {
            var result = await _mediator.Send(new GetUserProvidersQuery(User.Identity.Name));
            return Ok(new ResponseViewModel(result));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUser()
        {
            var result = await _mediator.Send(new GetLoggedInUserQuery(User.Identity.Name));
            return Ok(new ResponseViewModel(result));
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _mediator.Publish(new ForgotPasswordEvent(User.Identity.Name, dto, Request.Scheme));
            return Ok(new ResponseViewModel("Please, check your inbox."));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _mediator.Send(new JwtLoginCommand(dto));
            return Ok(new ResponseViewModel(result));
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            await _mediator.Publish(new ConfirmEmailEvent(userId, code));
            return Ok(new ResponseViewModel("Please, check your inbox."));
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(dto));
            if (!result.Succeeded)
            {
                ModelState.AddErrors(result);
                return BadRequest(ModelState);
            }

            return Ok(new ResponseViewModel("Your password has been reset."));
        }
    }
}
