﻿using Azure.Core;
using Azure.ResourceManager.ContainerInstance;
using Azure.ResourceManager.ContainerInstance.Models;
using System.Collections.Generic;

namespace Module.AppFunctions.Models
{
    public class ContainerGroup
    {
        public ContainerGroup(string moduleName, ContainerInstanceOperatingSystemType operatingSystem, List<Container> containers)
        {
            ModuleName = moduleName;
            OperatingSystem = operatingSystem;
            Containers = containers;
        }

        public string ModuleName { get; set; } = string.Empty;

        public AzureLocation Location { get; set; } = AzureLocation.WestEurope;

        public ContainerInstanceOperatingSystemType OperatingSystem { get; set; } = ContainerInstanceOperatingSystemType.Linux;

        public List<Container> Containers { get; set; } = new List<Container>();

        public ContainerGroupData GetContainerGroupData(bool includeContainers = true)
        {
            var containers = new List<ContainerInstanceContainer>();
            if (includeContainers)
                foreach (var item in Containers)
                {
                    var requirements = new ContainerResourceRequirements(new ContainerResourceRequestsContent(item.MemoryInGB, item.Cpu));
                    var container = new ContainerInstanceContainer(item.ContainerName, item.Image, requirements);
                    item.Variables.Add(new ContainerEnvironmentVariable("ModuleName") { Value = ModuleName });
                    foreach (var variable in item.Variables)
                        container.EnvironmentVariables.Add(variable);

                    containers.Add(container);
                }

            return new ContainerGroupData(AzureLocation.WestEurope, containers, OperatingSystem)
            {
                RestartPolicy = new ContainerGroupRestartPolicy("OnFailure"),
                Identity = new Azure.ResourceManager.Models.ManagedServiceIdentity(new Azure.ResourceManager.Models.ManagedServiceIdentityType("SystemAssigned")) //https://azuresdkdocs.blob.core.windows.net/$web/dotnet/Microsoft.Azure.Management.Blueprint/0.14.0-preview/api/Microsoft.Azure.Management.Blueprint.Models/Microsoft.Azure.Management.Blueprint.Models.ManagedServiceIdentity.html
            };
        }
    }
}
