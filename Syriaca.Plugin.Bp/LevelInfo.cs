﻿using Syriaca.Client.Memory;

namespace Syriaca.Plugin.Bp
{
    public class LevelInfo
    {
        public long Id { get; private set; }
        public float Length { get; private set; }

        private GdReader reader;

        public LevelInfo(GdReader reader)
        {
            this.reader = reader;
            
            Update();
        }

        public void Update()
        {
            try
            {
                Id = reader.Read<int>("Level ID");
                Length = reader.Read<float>("Level Length");
            }
            catch
            {
                Id = -882;
            }
        }
    }

    public class PlayerInfo
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public int CurrentAttempt { get; private set; }

        private GdReader reader;

        public PlayerInfo(GdReader reader)
        {
            this.reader = reader;
        }

        public void Update()
        {
            try
            {
                X = reader.Read<float>("Player X");
                Y = reader.Read<float>(0x164, 0x224, 0x38);
                CurrentAttempt = reader.Read<int>(0x164, 0x4A8);
            }
            catch
            {
                // we may have ended up back at the main menu, so it's best to set the X to 0.
                X = 0;
            }
        }
    }
}