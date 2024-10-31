using System.Reflection;
using MediatR;
using SC4013.Application.Common.Exceptions;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Security;

namespace SC4013.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public AuthorizationBehaviour(IUser user, IIdentityService identityService)
    {
        _user = user;
        _identityService = identityService;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();
        if (authorizeAttributes.Any())
        {
            if (_user.Id == null)
            {
                throw new UnauthorizedAccessException();
            }
            
            // Role-base authorization
            var authorizationAttributesWithRoles = authorizeAttributes
                .Where(x => !string.IsNullOrWhiteSpace(x.Roles))
                .ToList();
            
            if (authorizationAttributesWithRoles.Any())
            {
                var authorized = false;

                foreach (var roles in authorizationAttributesWithRoles.Select(x => x.Roles.Split(',')))
                {
                    foreach (var role in roles)
                    {
                        var isInRole = await _identityService.IsInRoleAsync(_user.Id, role.Trim());
                        if (isInRole)
                        {
                            authorized = true;
                            break;
                        }
                    }
                }
                
                if (!authorized)
                {
                    throw new ForbiddenAccessException();
                }
            }
            
        }
        
        return await next();
    }
}