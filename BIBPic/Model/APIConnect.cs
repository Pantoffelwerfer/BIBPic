using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graph;
using Microsoft.Graph.Authentication;
using Microsoft.Identity.Client;
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Identity;

namespace BIBPic.Model
{
    internal class APIConnect 
    {
        static async Task Main(string[] args)
        {
            string[] scopes = new string[] { "User.ReadWrite.All" };

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            string tenantId = "common";

            // Value from app registration
            string clientId = "YOUR_CLIENT_ID";

            // using Azure.Identity;
            var options = new DeviceCodeCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ClientId = clientId,
                TenantId = tenantId,
                // Callback function that receives the user prompt
                // Prompt contains the generated device code that user must
                // enter during the auth process in the browser
                DeviceCodeCallback = (code, cancellation) =>
                {
                    Console.WriteLine(code.Message);
                    return Task.FromResult(0);
                },
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.devicecodecredential
            var deviceCodeCredential = new DeviceCodeCredential(options);

            var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);

            // Pfad zum neuen Foto
            string newPhotoPath = "PathToNewPhoto.jpg";

            // Laden des neuen Fotos als Binärdaten
            byte[] photoBytes = File.ReadAllBytes(newPhotoPath);

            try
            {
                // Hochladen des neuen Fotos
                await graphClient.Users[clientId].Photo.Content.PutAsync(new MemoryStream(photoBytes));

                Console.WriteLine("Foto erfolgreich aktualisiert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aktualisieren des Fotos: {ex.Message}");
            }
        }
    }
}
