using ApiAuth.API.DtoS;
using ApiAuth.API.Infrastructure.Abstraction.Commands.AppUser;
using ApiAuth.API.Infrastructure.Extensions;
using ApiAuth.API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiAuth.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _mediator.Send(new RegisterUserCommand(dto, Request.Scheme));
            if (!result.Succeeded)
            {
                ModelState.AddErrors(result);
                return BadRequest(ModelState);
            }

            return Ok(new ResponseViewModel("Your account has been created; please check your inbox!"));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _mediator.Publish(new RemoveUserEvent(User.Identity.Name));
            return Ok(new ResponseViewModel("User deleted successfully!"));
        }


        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var result = await _mediator.Send(new ChangePasswordCommand(User.Identity.Name, dto));
            if (!result.Succeeded)
            {
                ModelState.AddErrors(result);
                return BadRequest(ModelState);
            }

            return Ok(new ResponseViewModel("Your password has been changed!"));
        }

        [HttpPost("setPassword")]
        public async Task<IActionResult> SetPassword(SetPasswordDto dto)
        {
            var result = await _mediator.Send(new SetPasswordCommand(User.Identity.Name, dto));
            if (!result.Succeeded)
            {
                ModelState.AddErrors(result);
                return BadRequest(ModelState);
            }

            return Ok(new ResponseViewModel("Your password has been reset!"));
        }

        [HttpPost("sendVerificationEmail")]
        public async Task<IActionResult> SendVerificationEmail()
        {
            await _mediator.Publish(new SendVerificationEmailEvent(User.Identity.Name, Request.Scheme));
            return Ok(new ResponseViewModel("Verification email sent. Please check your email."));
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
            await _mediator.Publish(new UpdateUserEvent(User.Identity.Name, dto));
            return Ok(new ResponseViewModel("Your profile has been updated"));
        }
    }
}