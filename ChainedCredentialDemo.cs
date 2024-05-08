using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Azure_ManagedIdentity_demo_FunctionApp
{
    public class ChainedTokenCredentialDemo : AzureCredentialDemo<ChainedTokenCredentialDemo>
    {
        public ChainedTokenCredentialDemo(ILogger<ChainedTokenCredentialDemo> logger)
            : base(logger)
        {
        }

        [Function("ChainedTokenCredentialDemo")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("*** ChainedTokenCredential demo ***");

            var miCred = new ManagedIdentityCredential();
            var cliCred = new AzureCliCredential();
            var cred = new ChainedTokenCredential(miCred, cliCred);

            return await GetDetailsAndFormatResponse(cred);
        }
    }
}
