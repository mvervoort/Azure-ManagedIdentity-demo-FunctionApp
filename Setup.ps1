$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot
Clear-Host

# Input variables
$resourceGroup = "MI-demo-rg"
$location = "northeurope"
$functionAppName = "mvrs-mi-demo2-func"
$storageAccountName = "mvrsmidemo2sa"
$managedIdentityName = "mvrs-mi-demo2-umi"


Write-Host "Creating resource group..."
az group create --name $resourceGroup --location $location --output none

Write-Host "Creating storage account..."
$sa = az storage account create --name $storageAccountName --resource-group $resourceGroup --location $location --sku Standard_LRS --only-show-errors --output json | ConvertFrom-Json
$saKey = (az storage account keys list --account-name $storageAccountName --resource-group $resourceGroup --output json | ConvertFrom-Json)[0].value

Write-Host "Assign permissions for current user..."
$signedInUser = az ad signed-in-user show | ConvertFrom-Json
Write-Host " displayName = $($signedInUser.displayName)"
az role assignment create --role "Storage Blob Data Contributor" --assignee $signedInUser.id --scope $sa.id --output none

Write-Host "Creating blob containers..."
foreach ($count in 1..4) {
    az storage container create --name "container$count" --account-name $sa.name --account-key $saKey --output none
}

Write-Host "Creating function app..."
az functionapp create --resource-group $resourceGroup --name $functionAppName --consumption-plan-location $location --storage-account $sa.name --runtime dotnet --functions-version 4 --disable-app-insights true --output none

Write-Host "Creating System Assigned Managed Idenity..."
$smi = az functionapp identity assign --name $functionAppName --resource-group $resourceGroup | ConvertFrom-Json
az role assignment create --role "Storage Blob Data Contributor" --assignee $smi.principalId --scope $sa.id --output none

Write-Host "Creating User Assigned Managed Identity..."
$umi = az identity create --name $managedIdentityName --resource-group $resourceGroup | ConvertFrom-Json
az functionapp identity assign --name $functionAppName --resource-group $resourceGroup --identities $umi.id --output none
az role assignment create --role "Storage Blob Data Contributor" --assignee $umi.principalId --scope $sa.id --output none

Write-HOst "Update settings local..."
$localSettings = Get-Content 'local.settings.json' -raw | ConvertFrom-Json
$localSettings.Values.MI_test_storage_account_name = $sa.name
$localSettings.Values.MI_test_user_assigned_identity_id = $umi.clientId
$localSettings | ConvertTo-Json -depth 32| set-content 'local.settings.json'

Write-HOst "Update settings in function app..."
az functionapp config appsettings set --resource-group $resourceGroup --name $functionAppName --settings MI_test_storage_account_name=$($sa.name) MI_test_user_assigned_identity_id=$($umi.clientId) --output none

Write-HOst "Publishing function app..."
func azure functionapp publish $functionAppName
