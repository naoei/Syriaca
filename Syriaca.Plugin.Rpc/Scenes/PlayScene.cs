using System.Collections.Generic;
using DiscordRPC;
using Syriaca.Client.Information;
using Syriaca.Client.Memory;

namespace Syriaca.Plugin.Rpc.Scenes
{
    public partial class PlayScene : RpcScene
    {
        public override IEnumerable<GameScene> Scenes =>
            new[] { GameScene.Play, GameScene.OfficialLevel, GameScene.TheChallenge };

        private readonly LevelInfo levelInfo;
        private readonly PlayerInfo playerInfo;
        private double playerProgress;

        public PlayScene(RpcClient client, Dictionary<string, object> sceneProperties, GdReader reader)
            : base(client, sceneProperties, reader)
        {
            client.Presence.WithTimestamps(Timestamps.Now);

            levelInfo = new LevelInfo(reader);
            playerInfo = new PlayerInfo();
            levelInfo.TrueDifficulty = CalculateDifficulty();
        }

        public override void Pulse()
        {
            levelInfo.Update(Reader);
            playerInfo.Update(Reader);
            
            if (levelInfo.Id == -882)
            {
                Client.ChangeStatus(s =>
                {
                    s.Details = string.Empty;
                    s.State = "In menus";
                    s.Timestamps = null;
                    s.Buttons = null;
                });
                return;
            }

            playerProgress = playerInfo.X / levelInfo.Length * 100;

            if (playerProgress <= levelInfo.CompletionProgress)
                playerProgress = levelInfo.CompletionProgress;

            Client.ChangeStatus(s =>
            {
                s.Details = levelInfo.ToString();

                s.State =
                    $"{playerProgress:N2}% | {getCoinString()} {(levelInfo.Id != 0 ? $"Score: {CalculateScore():N0} ({CalculatePerformance():N} pp)" : "")}";

                if (levelInfo.Id != 0)
                {
                    s.Buttons = new[]
                    {
                        new Button
                        {
                            Label = "View level",
                            Url = $"https://gdbrowser.com/{levelInfo.Id}"
                        }
                    };
                }
            });
        }

        public override void Dispose()
        {
            Client.ChangeStatus(s =>
            {
                s.Timestamps = null;
                s.Buttons = null;
            });
        }

        private string getCoinString()
        {
            var result = string.Empty;

            for (var i = 0; i < levelInfo.MaxCoins; i++)
                result += levelInfo.CoinsGrabbed[i] ? "C" : "-";

            if (!string.IsNullOrEmpty(result) && levelInfo.Id != 0)
                result += " |";

            return result;
        }
    }
}