﻿using System;
using System.Diagnostics;
using System.IO;
using Syriaca.Client.Information;
using Syriaca.Client.Utils;

namespace Syriaca.Client.Memory
{
    public class GdReader : MemoryReader
    {
        public const int GAME_MANAGER_ADDRESS = 0x3222D0;
        public const int ACCOUNT_MANAGER_ADDRESS = 0x3222D8;
        
        private readonly IntPtr gameManager;
        private readonly IntPtr accountManager;
        
        public GdReader(Process process)
            : base(process)
        {
            gameManager = Memory.Read<IntPtr>((IntPtr) GAME_MANAGER_ADDRESS);
            accountManager = Memory.Read<IntPtr>((IntPtr) ACCOUNT_MANAGER_ADDRESS);
            
            Logger.Info($"GameManager address: 0x{gameManager:X}");
            Logger.Info($"AccountManager address: 0x{accountManager:X}");
        }

        protected override AddressDictionary CreateAddressDictionary() => AddressDictionary.Parse(File.ReadAllText("Addresses.txt"));

        public GameScene ReadCurrentScene() 
            => (GameScene) Read<int>("Current Scene", gameManager);

        public (int user, int account) ReadPlayerIds() 
            => (Read<int>("User ID", gameManager), Read<int>("Account ID", accountManager));

        public T Read<T>(string addressEntryName)
            where T : struct
            => Read<T>(addressEntryName, gameManager);
        
        public T Read<T>(Address entry)
            where T : struct
            => Read<T>(entry, gameManager);
        
        public T Read<T>(params int[] offsets)
            where T : struct
            => Read<T>(gameManager, offsets);
        
        public string ReadString(string addressEntryName)
            => ReadString(addressEntryName, gameManager);

        public string ReadString(Address entry)
            => ReadString(entry, gameManager);

        public string ReadString(params int[] offsets)
            => ReadString(gameManager, offsets);
    }
}