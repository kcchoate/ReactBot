using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReactBot.App.Services
{
    public class DiscordClientTokenRetriever
    {
        public async Task<string> GetTokenAsync()
        {
            var secretName = "Discord-Socket-Client-Token";
            var keyVaultUri = Environment.GetEnvironmentVariable("VaultUri");
            var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
            var secret = await client.GetSecretAsync(secretName);
            return secret.Value.Value;
        }
    }
}
