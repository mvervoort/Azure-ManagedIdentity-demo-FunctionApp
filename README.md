# Azure ManagedIdentity demo using a Azure Functions

This is a simple demo to show how to use Azure ManagedIdentity in an Azure Function to access an Azure Storage Account.

It will show the use of:

- DefaultAzureCredential
- ManagedIdentityCredential (for system-assigned and user-assigned MI)
- ChainedTokenCredential
- AzureCliCredential

## Solution design

![docs\Solution-design.drawio.png](docs\Solution-design.drawio.png)

## Prerequisites

- Azure Subscription
- Azure CLI installed
- Azure Functions Core Tools installed
- Azurite storage emulator installed

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

## Links

- [Use managed identities on a virtual machine to acquire access token - Managed identities for Azure resources | Microsoft Learn](https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/how-to-use-vm-token)