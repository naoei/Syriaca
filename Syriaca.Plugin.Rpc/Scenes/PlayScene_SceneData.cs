using Syriaca.Client.Memory;

namespace Syriaca.Plugin.Rpc.Scenes
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

            public LevelInfo(GdReader reader)
            {
                Update(reader);
            }

            public void Update(GdReader reader)
            {
                try
                {
                    Id = reader.Read<int>("Level ID");

                    // TODO: Figure out why this portion of the code is fucking up.
                    // I already have my theory and believe that it's due to the string encoding,
                    // will have to eventually support custom string encoding.
                    /*if (reader.Read<int>("Level Title Length") >= 15)
                    {
                        var titleAddress = reader.Read<IntPtr>("Level Title");
                        Title = reader.ReadString(titleAddress);
                    }
                    else
                    {*/
                        Title = reader.ReadString("Level Title");
                    //}

                    Author = reader.ReadString("Level Author");
                    Stars = reader.Read<int>("Level Stars");
                    Demon = reader.Read<bool>("Is Demon");
                    Auto = reader.Read<bool>("Is Auto");
                    Difficulty = reader.Read<int>("Level Difficulty");
                    DemonDifficulty = reader.Read<int>("Demon Difficulty");
                    CompletionProgress = reader.Read<int>("Completion Progress");
                    TotalAttempts = reader.Read<int>("Attempts");
                    Jumps = reader.Read<int>("Jumps");
                    MaxCoins = reader.Read<int>("Max Coins");
                    Length = reader.Read<float>("Level Length");

                    if (MaxCoins >= 1)
                        for (var i = 0; i < 3; i++)
                            CoinsGrabbed[i] = reader.Read<int>($"Coin {i} Grabbed") == 1;
                }
                catch
                {
                    Id = -882;
                }
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