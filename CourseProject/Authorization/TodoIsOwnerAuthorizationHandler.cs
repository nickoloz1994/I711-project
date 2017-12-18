using CourseProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Authorization
{
    public class TodoIsOwnerAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, TodoItem>
    {
        private UserManager<ApplicationUser> _userManager;

        public TodoIsOwnerAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   TodoItem resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.FromResult(0);
            }

            if (requirement.Name != Constants.CreateOperationName &&
               requirement.Name != Constants.ReadOperationName &&
               requirement.Name != Constants.UpdateOperationName &&
               requirement.Name != Constants.DeleteOperationName)
            {
                return Task.FromResult(0);
            }

            if (resource.OwnerID == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}