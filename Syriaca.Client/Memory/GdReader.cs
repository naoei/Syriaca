using System;
using System.Diagnostics;
using System.IO;
using Syriaca.Client.Information;
using Syriaca.Client.Utils;

namespace Syriaca.Client.Memory
{
    /// <summary>
    /// A memory reader specifically for Geometry Dash.
    /// </summary>
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

        /// <summary>
        /// Reads the current scene.
        /// </summary>
        public GameScene ReadCurrentScene()
            => (GameScene) Read<int>("Current Scene", gameManager);

        /// <summary>
        /// Reads the player ids
        /// </summary>
        public PlayerState ReadPlayerIds()
            => (Read<int>("User ID", gameManager), Read<int>("Account ID", accountManager));

        /// <summary>
        /// Reads any type of value in Geometry Dash's memory region, starting from <see cref="gameManager"/>.
        /// </summary>
        /// <param name="addressEntryName">The address name to fetch offset data from.</param>
        /// <typeparam name="T">
        /// The type to read the value in-memory to.
        /// Note that if you want to read a string, please use the <see cref="ReadString(string)" /> method.
        /// </typeparam>
        /// <returns>The value in memory.</returns>
        public T Read<T>(string addressEntryName)
            where T : struct
            => Read<T>(addressEntryName, gameManager);

        /// <summary>
        /// Reads any type of value in Geometry Dash's memory region, starting from <see cref="gameManager"/>.
        /// </summary>
        /// <param name="entry">A custom address entry to read the offsets from.</param>
        /// <typeparam name="T">
        /// The type to read the value in-memory to.
        /// Note that if you want to read a string, please use the <see cref="ReadString(string)" /> method.
        /// </typeparam>
        /// <returns>The value in memory.</returns>
        public T Read<T>(Address entry)
            where T : struct
            => Read<T>(entry, gameManager);

        /// <summary>
        /// Reads any type of value in Geometry Dash's memory region, starting from <see cref="gameManager"/>.
        /// </summary>
        /// <param name="offsets">The offsets to read to get to the value in-memory.</param>
        /// <typeparam name="T">
        /// The type to read the value in memory to.
        /// Note that if you want to read a string, please use the <see cref="ReadString(string)" /> method.
        /// </typeparam>
        /// <returns>The value in memory.</returns>
        public T Read<T>(params int[] offsets)
            where T : struct
            => Read<T>(gameManager, offsets);

        /// <summary>
        /// Reads a <see cref="string" /> in UTF8 encoding in Geometry Dash's memory region, starting from <see cref="gameManager"/>.
        /// </summary>
        /// <param name="addressEntryName">The address name to fetch offset data from.</param>
        /// <returns>The string value in memory.</returns>
        public string ReadString(string addressEntryName)
            => ReadString(addressEntryName, gameManager);

        /// <summary>
        /// Reads a <see cref="string" /> in UTF8 encoding in Geometry Dash's memory region, starting from <see cref="gameManager"/>.
        /// </summary>
        /// <param name="entry">A custom address entry to read the offsets from.</param>
        /// <returns>The string value in memory.</returns>
        public string ReadString(Address entry)
            => ReadString(entry, gameManager);

        /// <summary>
        /// Reads a <see cref="string" /> in UTF8 encoding in Geometry Dash's memory region, starting from <see cref="gameManager"/>.
        /// </summary>
        /// <param name="offsets">The offsets to read to get to the value in-memory.</param>
        /// <returns>The string value in memory.</returns>
        public string ReadString(params int[] offsets)
            => ReadString(gameManager, offsets);
    }
}