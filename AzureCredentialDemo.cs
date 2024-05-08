using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs.Models;
using Azure.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Azure_ManagedIdentity_demo_FunctionApp
{
    public abstract class AzureCredentialDemo<TParent>
    {
        internal readonly ILogger<TParent> _logger;

        public AzureCredentialDemo(ILogger<TParent> logger)
        {
            _logger = logger;
        }

        public async Task<ObjectResult> GetDetailsAndFormatResponse(TokenCredential cred)
        {
            var responseMessage = new List<string>();
            try
            {
                var tokenDetails = await GetTokenDetails(cred);

                responseMessage.Add("Token details:\r\n" + string.Join("\r\n", tokenDetails.Select(claim => $"- {claim.Type} = {claim.Value}")));
            }
            catch (Exception ex)
            {
                responseMessage.Add("Token details failed:\r\n" + ex.Message);
            }

            try
            {
                var containers = await GetContainers(cred);

                responseMessage.Add("Fetched containers:\r\n" + string.Join("\r\n", containers.Select(container => $"- {container}")));
            }
            catch (Exception ex)
            {
                responseMessage.Add("Fetching containers failed:\r\n" + ex.Message);
            }

            return new OkObjectResult(string.Join("\r\n\r\n", responseMessage));
        }

        public async Task<List<string>> GetContainers(TokenCredential cred)
        {
            _logger.LogInformation("Setup blob client...");
            var storageAccountName = Environment.GetEnvironmentVariable("MI_test_storage_account_name");
            if (string.IsNullOrEmpty(storageAccountName))
                throw new Exception("Environment variable 'MI_test_storage_account_name' is not set.");
            var blobServiceClient = new BlobServiceClient(new Uri($"https://{storageAccountName}.blob.core.windows.net/"), cred);

            _logger.LogInformation("Fetch containers...");
            var resultSegment = blobServiceClient.GetBlobContainersAsync().AsPages();

            var containers = new List<string>();
            await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
            {
                foreach (BlobContainerItem containerItem in containerPage.Values)
                {
                    Console.WriteLine("Container name: {0}", containerItem.Name);
                    containers.Add(containerItem.Name);
                }
            }

            return containers;
        }

        public async Task<IEnumerable<Claim>> GetTokenDetails(TokenCredential cred)
        {
            string[] scopes = { "https://graph.microsoft.com/.default" };
            var token = await cred.GetTokenAsync(new TokenRequestContext(scopes), default);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token.Token) as JwtSecurityToken;
            var interestingClaimTypes = new string[] { "name", "app_displayname", "unique_name", "appid", "oid" };
            var interestingClaims = jsonToken.Claims.Where(c => interestingClaimTypes.Contains(c.Type));
            return interestingClaims;
        }
    }
}
