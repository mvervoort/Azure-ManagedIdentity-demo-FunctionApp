using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Azure_ManagedIdentity_demo_FunctionApp
{
    public class DefaultAzureCredentialDemo : AzureCredentialDemo<DefaultAzureCredentialDemo>
    {
        public DefaultAzureCredentialDemo(ILogger<DefaultAzureCredentialDemo> logger)
            : base(logger)
        {
        }

        [Function("DefaultAzureCredentialDemo")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("*** DefaultAzureCredential demo ***");

            var cred = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeVisualStudioCredential = true });
                
            return await GetDetailsAndFormatResponse(cred);
        }
    }
}
