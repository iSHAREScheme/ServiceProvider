using iSHARE.Api.Controllers;
using iSHARE.Api.Filters;
using iSHARE.Api.Swagger;
using iSHARE.Models;
using Microsoft.AspNetCore.Mvc;

namespace iSHARE.ServiceProvider.Api.Controllers
{
    public class PersonController : SchemeAuthorizedController
    {
        /// <summary>
        /// Mock service - Requests access for user
        /// </summary>
        /// <remarks>
        /// Fictional service. Client can send random data to this endpoint,
        /// and the service will simply respond with "true".
        /// The request body will not be evaluated in any manner by the server.
        /// Used as a mock service, endpoint will for example be provided with user info,
        /// indicating a boom access request for this user.
        /// </remarks>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [HttpPost, Route("boom_access")]
        [ApiExplorerSettings(GroupName = SwaggerGroups.TestSpec)]
        [SignResponse("boom_access_token", "boom_access_content", "BoomAccess")]
        public ActionResult<BoomAccess> BoomAccess()
        {
            return Ok(new BoomAccess { Valid = true });
        }
    }
}
