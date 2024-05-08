# Azure ManagedIdentity demo using a Azure Functions

This is a simple demo to show how to use Azure ManagedIdentity in a Azure Function to access an Azure Storage Account.

It will show the use of:

- DefaultAzureCredential
- ManagedIdentityCredential (for system-assigned and user-assigned MI)
- ChainedTokenCredential
- AzureCliCredential

## Prerequisites

- Azure Subscription
- Azure CLI
- Azure Functions Core Tools
- Azure Storage Account
- Azure Function App
- Azure Managed Identity

## Setup

1. Create a new Azure Function App
```pwsh
az functionapp create --resource-group <resource-group> --name <function-app-name> --consumption-plan-location <location> --runtime dotnet --functions-version 4
```
2. Create a new Azure Storage Account
```pwsh
az storage account create --name <storage-account-name> --resource-group <resource-group> --location <location> --sku Standard_LRS
```
3. Create a new Azure Managed Identity
```pwsh
az identity create --name <managed-identity-name> --resource-group <resource-group>
```
4. Assign the Managed Identity to the Azure Function App
```pwsh
az functionapp identity assign --name <function-app-name> --resource-group <resource-group> --identities <managed-identity-id>
```
5. Assign the Managed Identity to the Azure Storage Account
```pwsh
az role assignment create --role "Storage Blob Data Contributor" --assignee <managed-identity-id> --scope /subscriptions/<subscription-id>/resourceGroups/<resource-group>/providers/Microsoft.Storage/storageAccounts/<storage-account-name>
```
6. Deploy this project to the Azure Function App
```pwsh
func azure functionapp publish <function-app-name>
```

```pwsh
az functionapp create --resource-group <resource-group> --name <function-app-name> --consumption-plan-location <location> --runtime dotnet --functions-version 4
az storage account create --name <storage-account-name> --resource-group <resource-group> --location <location> --sku Standard_LRS
az identity create --name <managed-identity-name> --resource-group <resource-group>
az functionapp identity assign --name <function-app-name> --resource-group <resource-group> --identities <managed-identity-id>
az role assignment create --role "Storage Blob Data Contributor" --assignee <managed-identity-id> --scope /subscriptions/<subscription-id>/resourceGroups/<resource-group>/providers/Microsoft.Storage/storageAccounts/<storage-account-name>
func azure functionapp publish <function-app-name>
```

## Usage

1. Test the Functions in the Azure Portal
2. Or run locally using the Azure Functions Core Tools
