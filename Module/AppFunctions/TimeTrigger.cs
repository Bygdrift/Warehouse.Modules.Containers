using System;
using Azure.Identity;
using Azure.ResourceManager.ContainerInstance.Models;
using Azure.ResourceManager.KeyVault;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Azure.ResourceManager.ContainerInstance;
using Azure.ResourceManager.KeyVault.Models;
using Azure;
using System.Threading.Tasks;
using Module.AppFunctions.Models;
using System.Linq;
using Bygdrift.Warehouse;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Azure.Core.Diagnostics;

namespace Module.AppFunctions
{
    public class TimeTrigger
    {
        public AppBase<Settings> App { get; }

        public TimeTrigger(ILogger<TimeTrigger> logger) => App = new AppBase<Settings>(logger);

        [FunctionName("TimeTrigger5")]
        public async Task Run([TimerTrigger("0 */5 * * * *"
#if DEBUG
            ,RunOnStartup = true
#endif
            )] TimerInfo myTimer)
        {
            //App.LoadedUtc = DateTime.UtcNow;

            App.Log.LogWarning($"Nr 3: {DateTime.Now}");

           



            string resourceGroupName = "example";
            //var armClient = new ArmClient(new ClientSecretCredential(tenant, clientId, clientSecret));  //https://github.com/Azure/azure-sdk-for-net/issues/29967

            TokenCredential credential = new DefaultAzureCredential();
            //TokenCredential credential = new ManagedIdentityCredential();
            var armClient = new ArmClient(credential);
            App.Log.LogWarning($"101");


            try
            {
                //var a = armClient.GetResourceGroupResource(new ResourceIdentifier("example"));
                //App.Log.LogWarning($"102" + a.Data.Id);
                SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();  //https://learn.microsoft.com/en-us/dotnet/azure/sdk/resource-management?tabs=PowerShell#management-sdk-cheat-sheet
                var subscriptions = armClient.GetSubscriptions();  //https://learn.microsoft.com/en-us/dotnet/azure/sdk/resource-management?tabs=PowerShell#management-sdk-cheat-sheet
                App.Log.LogWarning($"223" + subscriptions.Count());
                App.Log.LogWarning($"224" + subscriptions.First().Data.SubscriptionId);
                //App.Log.LogWarning($"223" + subscription.Data.SubscriptionId);
            }
            catch (Exception e)
            {
                App.Log.LogWarning($"Error: {e.Message}");
            }
            App.Log.LogWarning($"103");

            //var client = new SecretClient(new Uri("https://slet2.vault.azure.net/"), credential);  //https://learn.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme?view=azure-dotnet#authenticate-with-a-system-assigned-managed-identity


            //SubscriptionResource subscription = await armClient.GetDefaultSubscriptionAsync();  //https://learn.microsoft.com/en-us/dotnet/azure/sdk/resource-management?tabs=PowerShell#management-sdk-cheat-sheet
            //App.Log.LogInformation($"224" + subscription.Id.SubscriptionId);
            //Response<SubscriptionResource> subscription = await armClient.GetSubscriptions().GetAsync(subscriptionId).ConfigureAwait(false);




            //ResourceGroupResource rg = subscription.GetResourceGroup(resourceGroupName);
            ////var resourceGroupCollection = subscription.GetResourceGroups();
            ////var resourceGroup = await resourceGroupCollection.GetAsync(resourceGroupName).ConfigureAwait(false);

            //KeyVaultResource keyVault = rg.GetKeyVault("slet2");

            //var containers = new List<Container>() {
            //    new Container("mycontainerv2", "bygdrift/consoleappcontainerv1", keyVault.Data.Properties.VaultUri, 1, 1, new (string, string)[] { ("Test", "HAHA"), }),
            //    new Container("mycontainerv3", "bygdrift/consoleappcontainerv1", keyVault.Data.Properties.VaultUri, 1, 1, new (string, string)[] { ("Test", "HAHA"), })
            //};
            //var containerGroup = new ContainerGroup("ModuleName", ContainerInstanceOperatingSystemType.Linux, containers);
            //await CreateACI(rg, containerGroup, keyVault);
        }

        private static async Task CreateACI(ResourceGroupResource rg, ContainerGroup containerGroup, KeyVaultResource keyVault)
        {
            var containerGroupCollection = rg.GetContainerGroups();
            if (!containerGroupCollection.Any(o => o.Data.Name == containerGroup.ModuleName))  //It will fail completing because Keyvault isn't set yet. If they have the Bygdrift Warehouse component installed, the container will fail fast at first run.
            {
                var res = await containerGroupCollection.CreateOrUpdateAsync(WaitUntil.Completed, containerGroup.ModuleName, containerGroup.GetContainerGroupData());
                var principalId = res.Value.Data.Identity.PrincipalId.ToString() ?? "";
                await UpdateKeyVaultPolicies(keyVault, principalId);
            }
            else
            {
                var res = await containerGroupCollection.CreateOrUpdateAsync(WaitUntil.Completed, containerGroup.ModuleName, containerGroup.GetContainerGroupData());
                await res.Value.StartAsync(WaitUntil.Completed);
            }
        }

        private static async Task UpdateKeyVaultPolicies(KeyVaultResource keyVault, string principalId)
        {
            var identityPermissions = new IdentityAccessPermissions();
            identityPermissions.Secrets.Add(new IdentityAccessSecretPermission("get"));
            identityPermissions.Secrets.Add(new IdentityAccessSecretPermission("list"));
            Guid tenantId = keyVault.Data.Properties.TenantId;
            var keyVaultPolicy = new KeyVaultAccessPolicy(tenantId, principalId, identityPermissions);
            var keyVaultPolicies = new List<KeyVaultAccessPolicy> { keyVaultPolicy };
            var keyVaultPolicyProperties = new KeyVaultAccessPolicyProperties(keyVaultPolicies);
            var keyVaultParameters = new KeyVaultAccessPolicyParameters(keyVaultPolicyProperties);
            var res = await keyVault.UpdateAccessPolicyAsync(AccessPolicyUpdateKind.Add, keyVaultParameters);
        }
    }
}
