using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Azure_ManagedIdentity_demo_FunctionApp
{
    public class ManagedIdentityCredentialDemo : AzureCredentialDemo<ManagedIdentityCredentialDemo>
    {
        public ManagedIdentityCredentialDemo(ILogger<ManagedIdentityCredentialDemo> logger)
            : base(logger)
        {
        }

        [Function("ManagedIdentityCredentialDemo")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("*** ManagedIdentityCredential demo ***");

            var cred = new ManagedIdentityCredential();

            return await GetDetailsAndFormatResponse(cred);
        }
    }
}
