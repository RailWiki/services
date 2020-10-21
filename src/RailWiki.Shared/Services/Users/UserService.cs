using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using RailWiki.Shared.Configuration;
using RailWiki.Shared.Data;
using RailWiki.Shared.Models.Users;

namespace RailWiki.Shared.Services.Users
{
    public class UserService : IUserService
    {
        private readonly OktaClient _oktaClient;
        private readonly OktaConfig _oktaConfig;
        private readonly IRepository<Entities.Users.User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<Entities.Users.User> userRepository,
            IOptions<OktaConfig> oktaOptions,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _oktaConfig = oktaOptions.Value;

            _oktaClient = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _oktaConfig.OktaDomain,
                Token = _oktaConfig.ApiToken
            });
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<Entities.Users.User> GetUserByIdAsync(int id) => _userRepository.GetByIdAsync(id);

        public async Task<GetUserModel> GetUserBySlugAsync(string slug)
        {
            var user = await _userRepository.TableNoTracking
                .FirstOrDefaultAsync(x => x.Slug == slug);

            var userModel = _mapper.Map<GetUserModel>(user);

            return userModel;
        }

        public async Task<Entities.Users.User> RegisterUserAsync(RegisterUserRequest request)
        {
            IUser oktaUser = null;
            try
            {
                var oktaUserRequest = new CreateUserWithPasswordOptions
                {
                    Activate = true,
                    Profile = new UserProfile
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Login = request.Email
                    },
                    Password = request.Password
                };

                oktaUser = await _oktaClient.Users.CreateUserAsync(oktaUserRequest);
                _logger.LogDebug($"Registered user {request.Email} with Okta. Okta ID: {oktaUser.Id}");

                if (!string.IsNullOrEmpty(_oktaConfig.AppGroupName))
                {
                    var appGroup = await _oktaClient.Groups.FirstOrDefaultAsync(x => x.Profile.Name == _oktaConfig.AppGroupName);
                    if (appGroup != null)
                    {
                        await _oktaClient.Groups.AddUserToGroupAsync(appGroup.Id, oktaUser.Id);
                    }

                    _logger.LogDebug($"Added user {request.Email} to Okta group {_oktaConfig.AppGroupName}");
                }
            }
            catch(OktaApiException oktaEx)
            {
                _logger.LogError(oktaEx, $"Error creating user {request.Email} in Okta. Error Code: {oktaEx.ErrorCode}. Summary: {oktaEx.ErrorSummary}");

                // TODO: throw a more friendly error message
                throw new Exception(oktaEx.ErrorSummary, oktaEx);
            }

            try
            {  
                var user = new Entities.Users.User
                {
                    SubjectId = oktaUser.Id,
                    EmailAddress = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    RegisteredOn = DateTime.UtcNow
                };

                await CreateUserAsync(user);

                _logger.LogDebug($"Registered user {request.Email}");

                return user;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error registering user {request.Email}");
                throw ex;
            }
        }

        public async Task CreateUserAsync(Entities.Users.User user)
        {
            await _userRepository.CreateAsync(user);
        }
    }
}
