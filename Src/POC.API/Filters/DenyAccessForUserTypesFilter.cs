using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POC.API.Filters
{
public class DenyAccessForUserTypesFilter : IActionFilter
    {

        public DenyAccessForUserTypesFilter(string[] usersTypes)
        {
            this.UserTypes = usersTypes ?? throw new ArgumentNullException(nameof(usersTypes));
        }


        protected string[] UserTypes { get; }

        /// <inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //string[] AllowedUser = new string[] { "one", "two", "three" };
            //Get user type from db/header etc
            var userTypeFromHeader = "BasicUser";
            if (this.UserTypes != null)
            {
                if (this.UserTypes.Any(x => x == userTypeFromHeader) )
                {
                    context.Result = new NotFoundObjectResult(null);
                }
            }
        }
    }
}
