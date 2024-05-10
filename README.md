# Azure ManagedIdentity demo using a Azure Functions

Let yourself be amazed by the shear wonders of __Managed Identities__ in Azure.

This is a simple demo to show how to use Azure ManagedIdentity in an Azure Function to access an Azure Storage Account.

It will show the use of:

- [DefaultAzureCredential](https://learn.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential?view=azure-dotnet)
- [ManagedIdentityCredential](https://learn.microsoft.com/en-us/dotnet/api/azure.identity.managedidentitycredential?view=azure-dotnet) (for system-assigned and user-assigned MI)
- [ChainedTokenCredential](https://learn.microsoft.com/en-us/dotnet/api/azure.identity.chainedtokencredential?view=azure-dotnet)
- [AzureCliCredential](https://learn.microsoft.com/en-us/dotnet/api/azure.identity.azureclicredential?view=azure-dotnet)

## Solution design

![Solution-design.drawio.png](docs/Solution-design.drawio.png)

## Prerequisites

- Access to an Azure Subscription
- Azure CLI installed (Solution is developed/tested with v2.59)
- Azure Functions Core Tools installed (for runing local in VS-Code)
- Azurite storage emulator installed (for runing local in VS-Code)

## Setup

Update the input variables in [Setup.ps1](Setup.ps1) to your needs, and run [Setup.ps1](Setup.ps1). This will:

1. Create a Resource Group
2. Create a Storage Account
3. Assign permissions for current user
4. Create blob containers
5. Create a Function App
6. Create a System Assigned Managed Idenity
7. Create User Assigned Managed Identity
8. Update settings locally
9. Update settings in deployed Function App
10. Publishing the Function App

## Usage

1. Run locally using the 'Azure Functions Core Tools' and 'Azurite storage emulator'.
2. Or test the Functions in the Azure Portal (after deploying using [Setup.ps1](Setup.ps1))

## Result

Below is a table of all the results of local development (in Function simulator)
and Function App (Test/Run of the Function in the Function App).

| Credentials | Local development | Function App |
| ----------- | ----------------- | ------------ |
| DefaultAzureCredential | Token details:<br>- app_displayname = **Microsoft Azure CLI**<br>- appid = &lt;guid&gt;<br>- name = **Marco Vervoort**<br>- oid = &lt;guid&gt;<br>- unique_name = &lt;e-mail address&gt; | Token details:<br>- app_displayname = **mvrs-mi-demo2-func**<br>- appid = &lt;guid&gt;<br>- oid = &lt;guid&gt; |
| System Assigned<br>ManagedIdentityCredential | <span style="color:red">Token details failed:<br>ManagedIdentityCredential authentication unavailable.<br>Multiple attempts failed to obtain a token from the managed identity endpoint.</span> | Token details:<br>- app_displayname = **mvrs-mi-demo2-func**<br>- appid = &lt;guid&gt;<br>- oid = &lt;guid&gt; |
| User Assigned<br>ManagedIdentityCredential | <span style="color:red">Token details failed:<br>ManagedIdentityCredential authentication unavailable.<br>Multiple attempts failed to obtain a token from the managed identity endpoint.<span> | Token details:<br>- app_displayname = **mvrs-mi-demo2-umi**<br>- appid = &lt;guid&gt;<br>- oid = &lt;guid&gt; |
| ChainedTokenCredential<br>(ManagedIdentityCredential + <br>AzureCliCredential) | Token details:<br>- app_displayname = **Microsoft Azure CLI**<br>- appid = &lt;guid&gt;<br>- name = **Marco Vervoort**<br>- oid = &lt;guid&gt;<br>- unique_name = &lt;e-mail address&gt; | Token details:<br>- app_displayname = **mvrs-mi-demo2-func**<br>- appid = &lt;guid&gt;<br>- oid = &lt;guid&gt; |

And all of the succesful calls will also show the fetched containers, like:
```
Fetched containers:
- azure-webjobs-hosts
- azure-webjobs-secrets
- container1
- container2
- container3
- container4
```

## Links

- [Use managed identities on a virtual machine to acquire access token - Managed identities for Azure resources | Microsoft Learn](https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/how-to-use-vm-token)