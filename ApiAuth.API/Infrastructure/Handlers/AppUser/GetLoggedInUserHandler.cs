﻿using ApiAuth.API.Infrastructure.Abstraction.Queries.AppUser;
using ApiAuth.API.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Handlers.AppUser
{
    public class GetLoggedInUserHandler : IRequestHandler<GetLoggedInUserQuery, AppUserViewModel>
    {
        private readonly ILogger<GetLoggedInUserHandler> _logger;
        private readonly UserManager<Models.AppUser> _userManager;
        private readonly IMapper _mapper;

        public GetLoggedInUserHandler(
            ILogger<GetLoggedInUserHandler> logger,
            UserManager<Models.AppUser> userManager,
            IMapper mapper
            )
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AppUserViewModel> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting {nameof(GetLoggedInUserHandler)}");

                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    throw new NullReferenceException("User not found!");
                }

                var userViewModel = _mapper.Map<AppUserViewModel>(user);

                var userRoles = await _userManager.GetRolesAsync(user);
                userViewModel.Roles = userRoles.ToList();

                _logger.LogInformation($"Finished {nameof(GetLoggedInUserHandler)}");

                return userViewModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
