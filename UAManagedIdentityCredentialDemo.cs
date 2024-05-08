using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Azure_ManagedIdentity_demo_FunctionApp
{
    public class UAManagedIdentityCredentialDemo : AzureCredentialDemo<UAManagedIdentityCredentialDemo>
    {
        public UAManagedIdentityCredentialDemo(ILogger<UAManagedIdentityCredentialDemo> logger)
            : base(logger)
        {
        }

        [Function("UAManagedIdentityCredentialDemo")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("*** UserAssigned ManagedIdentityCredential demo ***");

            var UserAssignedIdentityId = Environment.GetEnvironmentVariable("MI_test_user_assigned_identity_id");
            if (string.IsNullOrEmpty(UserAssignedIdentityId))
                throw new Exception("Environment variable 'MI_test_user_assigned_identity_id' is not set.");
            var cred = new ManagedIdentityCredential(UserAssignedIdentityId);

            return await GetDetailsAndFormatResponse(cred);
        }
    }
}
