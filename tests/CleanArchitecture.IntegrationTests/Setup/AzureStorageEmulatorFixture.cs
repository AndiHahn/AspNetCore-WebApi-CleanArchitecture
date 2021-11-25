using System;
using RimDev.Automation.StorageEmulator;

namespace CleanArchitecture.IntegrationTests.Setup
{
    public class AzureStorageEmulatorFixture : IDisposable
    {
        private readonly AzureStorageEmulatorAutomation emulatorAutomation;

        public AzureStorageEmulatorFixture()
        {
            emulatorAutomation = new AzureStorageEmulatorAutomation();
            emulatorAutomation.Start();
        }

        public void Dispose()
        {
            emulatorAutomation.Dispose();
        }
    }
}
