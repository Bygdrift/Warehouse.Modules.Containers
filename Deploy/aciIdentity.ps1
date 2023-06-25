Clear-Host
Write-Host '====================================================' -ForegroundColor Yellow

# az login

# https://learn.microsoft.com/en-us/azure/container-instances/container-instances-managed-identity?ref=zimmergren.net&WT.mc_id=tozimmergren&utm_campaign=zimmergren&utm_medium=blog&utm_source=zimmergren
# Get the resource ID of the resource group

# az group show --name example --query id --output tsv
# $aa = /subscriptions/7ce9b23e-5a96-4e30-b494-ed4f5d2cc7a6/resourceGroups/example

# $group = 'example'
# $RG_ID=$(az group show --name $group --query id --output tsv)

# Create container group with system-managed identity
# az container create -g $group -n mycontainer --image mcr.microsoft.com/azure-cli --assign-identity --scope $RG_ID  --command-line "tail -f /dev/null"

$SP_ID=$(az container show --resource-group example --name mycontainer --query identity.principalId --out tsv)

# az bicep decompile --file main.json