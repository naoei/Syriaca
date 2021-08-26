using System;
using System.Diagnostics;
using System.IO;
using osu.Framework.Bindables;
using Syriaca.Client.Information;

namespace Syriaca.Client.Memory
{
    public class GdReader : MemoryReader
    {
        public const int GAME_MANAGER_ADDRESS = 0x3222D0;
        public const int ACCOUNT_MANAGER_ADDRESS = 0x3222D8;
        
        //TODO: Eventually make our own bindable where we can manually call out if we have changes or not.
        private readonly Bindable<GdProcessState> state;
        private readonly IntPtr gameManager;
        private readonly IntPtr accountManager;
        
        public GdReader(Process process, Bindable<GdProcessState> processState)
            : base(process)
        {
            gameManager = Memory.Read<IntPtr>((IntPtr) GAME_MANAGER_ADDRESS);
            accountManager = Memory.Read<IntPtr>((IntPtr) ACCOUNT_MANAGER_ADDRESS);
            
            state.BindTo(processState);
        }

        protected override AddressDictionary CreateAddressDictionary() => AddressDictionary.Parse(File.ReadAllText("Addresses.txt"));

        public void UpdateScene()
        {
            state.Value.Scene = (GameScene) Read<int>("Current Scene", gameManager);
        }

        public void ReadPlayerIds()
        {
            state.Value.PlayerState.UserId = Read<int>("User ID", gameManager);
            state.Value.PlayerState.AccountId = Read<int>("Account ID", accountManager);
        }
    }
}