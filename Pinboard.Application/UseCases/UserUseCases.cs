using Pinboard.Domain.Interfaces;
using Pinboard.Domain.Interfaces.Repositories;
using Pinboard.Domain.Interfaces.UseCases;

namespace Pinboard.Application.UseCases
{
    public class UserUseCases : IUserUseCases
    {
        private readonly IDataContext _dataContext;
        private readonly IAuthService _authService;
        private readonly IUserState _userState;

        public UserUseCases(IDataContext dataContext, IAuthService authService, IUserState userState)
        {
            _dataContext = dataContext;
            _authService = authService;
            _userState = userState;
        }

        public void DeleteCurrentUser()
        {
            var userId = _userState.Identifier;

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User identifier not found. Unable to delete user");
            }

            _authService.DeleteAccount(userId);

            _dataContext.NoteRepository.DeleteByAuthorId(userId);
        }
    }
}
