using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureQueueExample.Controller.v1
{
    public class UserDetailsFunction
    {
        private readonly ILogger<UserDetailsFunction> _logger;

        public UserDetailsFunction(ILogger<UserDetailsFunction> logger)
        {
            _logger = logger;
        }

        [FunctionName(nameof(UserDetailsFunction))]
        public async Task<IActionResult> HandleEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/userDetails")]
            HttpRequest req,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserDetailsFunction)} invoked");

                await Task.FromResult("");
                return new OkObjectResult("");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new InternalServerErrorResult();
            }
        }
    }
}
