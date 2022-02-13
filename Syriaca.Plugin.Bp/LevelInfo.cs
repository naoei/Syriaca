using Syriaca.Client.Memory;

namespace Syriaca.Plugin.Bp
{
    public class LevelInfo
    {
        public long Id { get; set; }
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
            }
            catch
            {
                // we may have ended up back at the main menu, so it's best to set the X to 0.
                X = 0;
            }
        }
    }
}