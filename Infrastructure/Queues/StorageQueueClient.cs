using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace Infrastructure.Queues
{
    public class StorageQueueClient : IStorageQueueClient
    {
        private readonly ILogger<IStorageQueueClient> _logger;
        private readonly QueueClient _cloudQueues;

        public StorageQueueClient(ILogger<IStorageQueueClient> logger, QueueClient cloudQueues)
        {
            _logger = logger;
            _cloudQueues = cloudQueues;
        }
        public async Task AddMessageAsync(UserDetailsCommand message)
        {
            var serialisedbody = JsonConvert.SerializeObject(message);


            using (_logger.BeginScope(new { QueueName = _cloudQueues.Name }))
            {
                try
                {
                    _logger.LogInformation("About to publish Message to Queue");
                    var response = await _cloudQueues.SendMessageAsync(serialisedbody);
                    _logger.LogInformation("Message published message id : {messageId}", response.Value.MessageId);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error in publishing the message to Queue");
                    throw;
                }


            }
        }
    }
}
