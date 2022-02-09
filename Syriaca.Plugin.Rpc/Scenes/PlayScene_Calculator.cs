using System;

namespace Syriaca.Plugin.Rpc.Scenes
{
    public partial class PlayScene
    {
        public int CalculateScore()
        {
            var res = Math.Pow(Math.Pow(playerProgress / 100d, 1 + CalculateDifficulty() / 14d * 0.5d),
                          1 - (Math.Log10(levelInfo.Length) - 5) * 0.1) * 1_000_000;

            // jump bonus
            res += Math.Pow(levelInfo.Jumps, 1.1f);

            return (int) res;
        }

        public float CalculatePerformance()
        {
            var res = Math.Pow(
                Math.Pow(CalculateDifficulty() / 14d, 0.4d) * (CalculateScore() / 1_000_000d) *
                Math.Pow(levelInfo.Length, CalculateDifficulty() / 25d), 1.2d);

            if (playerProgress >= 100)
                res *= 1.05f;

            foreach (var coin in levelInfo.CoinsGrabbed)
                if (coin)
                    res *= .1 / levelInfo.MaxCoins + 1; // should at least have 1.1 bonus max.*/

            if (levelInfo.Demon)
                res += Math.Pow(levelInfo.DemonDifficulty * (playerProgress / 100), 1.1f);
            else
                res += 1 + 1 / (levelInfo.Difficulty == 0 ? 1 : levelInfo.Difficulty);

            // uniformly calculate difficulty bonus
            //res *= Math.Pow(CalculateDifficulty(), (Stars * 0.95) / 1e7);

            // length nerf
            //res *= Math.Pow(Length, -(Length * 20) / 1e7) * (Length / (Length * (CalculateDifficulty() / 10))) *
            //       CalculateDifficulty() * 0.075;

            double attemptPenalty;

            if (levelInfo.Demon && levelInfo.TotalAttempts < 500 * levelInfo.DemonDifficulty)
                attemptPenalty = Math.Pow(levelInfo.TotalAttempts, -(levelInfo.TotalAttempts * 0.75 / 1e7)); // demon, low attempts
            else
                attemptPenalty = Math.Pow(levelInfo.TotalAttempts, -(levelInfo.TotalAttempts * 0.85 / 1e7)); // everything else

            res *= attemptPenalty;

            return (float) res;
        }

        public double CalculateDifficulty()
        {
            double diff = levelInfo.Stars;

            if (levelInfo.Auto)
                return 0;

            if (!levelInfo.Demon)
                return diff;

            diff = levelInfo.DemonDifficulty switch
            {
                3 => 10,
                4 => 11,
                5 => 13,
                6 => 14,
                _ => 12
            };

            // precursor length balancing
            //diff += Math.Pow(Length, -(Length * 20) / 1e7) * (Length / (Length * (diff / 10))) *
            //        Math.Pow(diff, -(diff * 0.175) / 1e7);

            // length bonus
            //diff += Math.Pow(Length, -(diff / Math.Pow(Length, -(Length * 20) / 1e7))) * 1.75;

            return diff;
        }
    }
}