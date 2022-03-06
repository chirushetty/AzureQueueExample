using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Queues;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureQueueExample.Controller.v1
{
    public class QueueTriggerFunction
    {
        private readonly ILogger<QueueTriggerFunction> _logger;
        private readonly TelemetryClient _telemetryClient;

        public QueueTriggerFunction(ILogger<QueueTriggerFunction> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        [FunctionName(nameof(UserDetailsListener))]
        public async Task UserDetailsListener(
            [QueueTrigger("%UserDetailsQueueName%")]
            string retrievedMessage,
            string id, string dequeueCount)
        {
            try
            {
                // todo: We should review the "retrievedMessage" parameter in the future and try to receive it as a CloudQueueMessage.
                // Previous attempts to do so have failed to populate the fields in the CloudQueueMessage, which we assume is either an issue
                // in the encoding of the message when being put onto the queue, or an issue with the libraries used to write / read the queue.

                var queueMessage = JsonConvert.DeserializeObject<UserDetailsCommand>(retrievedMessage);
                using (_logger.BeginScope(new { MessageId = id, Email = queueMessage.Email }))
                {
                    _logger.LogInformation("Queue Message Received MessageId : {id}", id);
                }
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackMetric($"StorageQueueTrigger_user-details-inboxqueue_Failure".ToLower(), Convert.ToInt32(dequeueCount));

                throw ex;
            }

        }
    }
}
