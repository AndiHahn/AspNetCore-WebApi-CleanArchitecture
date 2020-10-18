using System;
using RimDev.Automation.StorageEmulator;

namespace CleanArchitecture.Tests.Shared.AzureStorageEmulator
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
