using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Infrastructure.Queues;
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
        private readonly IStorageQueueClient _storageQueueClient;

        public UserDetailsFunction(ILogger<UserDetailsFunction> logger, IStorageQueueClient storageQueueClient)
        {
            _logger = logger;
            _storageQueueClient = storageQueueClient;
        }

        [FunctionName(nameof(UserDetailsFunction))]
        public async Task<IActionResult> HandleEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/{username}/userDetails")]
            HttpRequest req,
            [Description("The Customer UserName")] string username,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserDetailsFunction)} invoked");
                var email = req.Query["email"];
                var firstName = req.Query["firstname"];
                var lastname = req.Query["lastname"];
                DateTime.TryParse(req.Query["dob"], out DateTime dob);
                var userDetails = new UserDetailsCommand(username, firstName, lastname, email, dob);
                await _storageQueueClient.AddMessageAsync(userDetails);
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new InternalServerErrorResult();
            }
        }
    }
}
