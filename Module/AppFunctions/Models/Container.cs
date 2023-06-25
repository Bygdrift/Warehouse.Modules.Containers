using Azure.ResourceManager.ContainerInstance.Models;
using System;
using System.Collections.Generic;

namespace Module.AppFunctions.Models
{
    public class Container
    {
        public Container(string containerName, string image, Uri? vaultUri, double memoryInGB, double cpu, (string Name, string Variable)[] variables)
        {
            ContainerName = containerName;
            Image = image;
            VaultUri = vaultUri;

            MemoryInGB = memoryInGB;
            Cpu = cpu;

            if (variables != null)
                foreach (var item in variables)
                    Variables.Add(new ContainerEnvironmentVariable(item.Name) { Value = item.Variable });

            Variables.Add(new ContainerEnvironmentVariable("VaultUri") { Value = vaultUri?.ToString() });

        }

        public Uri? VaultUri { get; }
        public string ContainerName { get; }
        public string Image { get; }
        public double MemoryInGB { get; }
        public double Cpu { get; }
        public List<ContainerEnvironmentVariable> Variables { get; set; } = new();
    }
}
