using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Azure_ManagedIdentity_demo_FunctionApp
{
    public class RawManagedIdentityCredentialDemo : AzureCredentialDemo<RawManagedIdentityCredentialDemo>
    {
        public RawManagedIdentityCredentialDemo(ILogger<RawManagedIdentityCredentialDemo> logger)
            : base(logger)
        {
        }

        [Function("RawManagedIdentityCredentialDemo")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("*** Raw ManagedIdentity demo ***");

            try
            {
                _logger.LogInformation("Fetch raw Managed Identity token...");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Metadata", "true");
                var response = await client.GetAsync("http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=https://management.azure.com/");
                return new OkObjectResult(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Exception: " + ex.Message);
            }
        }
    }
}
