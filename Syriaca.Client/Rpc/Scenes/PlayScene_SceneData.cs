using System;
using System.Collections.Generic;
using Syriaca.Client.Memory;

namespace Syriaca.Client.Rpc.Scenes
{
    public partial class PlayScene
    {
        private class LevelInfo
        {
            public int Id { get; private set; }
            public string Title { get; private set; }
            public string Author { get; private set; }
            public int Stars { get; private set; }
            public bool Demon { get; private set; }
            public bool Auto { get; private set; }
            public int Difficulty { get; private set; }
            public int DemonDifficulty { get; private set; }
            public int CompletionProgress { get; private set; }
            public int TotalAttempts { get; private set; }
            public int Jumps { get; private set; }
            public int MaxCoins { get; private set; }
            public float Length { get; private set; }
            public bool[] CoinsGrabbed { get; } = new bool[3];
            public double TrueDifficulty { get; set; }

            public LevelInfo(MemoryReader reader, IReadOnlyDictionary<string, object> sceneData)
            {
                UpdateInformation(reader, sceneData);
            }

            public void UpdateInformation(MemoryReader reader, IReadOnlyDictionary<string, object> sceneData)
            {
                Id = (int) sceneData.GetValueOrDefault("Level ID", -882);

                if ((int) sceneData.GetValueOrDefault("Level Title Length", 0) >= 15)
                {
                    //var titleAddress = reader.Read<IntPtr>("Level Title");
                    Title = reader.ReadString((IntPtr) sceneData["Level Title"]);
                }
                else
                {
                    Title = (string) sceneData.GetValueOrDefault("Level Title");
                }

                Author = (string) sceneData.GetValueOrDefault("Level Author");
                Stars = (int) sceneData.GetValueOrDefault("Level Stars", 0);
                Demon = (bool) sceneData.GetValueOrDefault("Is Demon", false);
                Auto = (bool) sceneData.GetValueOrDefault("Is Auto", false);
                Difficulty = (int) sceneData.GetValueOrDefault("Level Difficulty", 0);
                DemonDifficulty = (int) sceneData.GetValueOrDefault("Demon Difficulty", 0);
                CompletionProgress = (int) sceneData.GetValueOrDefault("Completion Progress", 0);
                TotalAttempts = (int) sceneData.GetValueOrDefault("Attempts", 0);
                Jumps = (int) sceneData.GetValueOrDefault("Jumps", 0);
                MaxCoins = (int) sceneData.GetValueOrDefault("Max Coins", 0);
                Length = (float) sceneData.GetValueOrDefault("Level Length", 0f);

                if (MaxCoins >= 1)
                    for (var i = 0; i < 3; i++)
                        CoinsGrabbed[i] = (int) sceneData.GetValueOrDefault($"Coin {i} Grabbed", 0) == 1;
            }

            public override string ToString()
                => $"{Title} - {Author}{GetDifficultyString()} {GetIdString()}";

            private string GetDifficultyString()
                => TrueDifficulty == 0 ? string.Empty : $" [{TrueDifficulty:N1}*]";

            private string GetIdString()
                => Id == 0 ? "(Local level)" : $"(ID: {Id})";
        }

        private class PlayerInfo
        {
            public float X { get; private set; }

            public void Update(GdReader reader)
            {
                try
                {
                    X = reader.Read<float>("Player X");
                }
                catch
                {
                    // we may have ended up back at the main menu, so it's best to set the X to 0.
                    X = 0;
                }
            }
        }
    }
}