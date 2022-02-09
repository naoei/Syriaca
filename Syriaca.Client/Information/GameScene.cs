namespace Syriaca.Client.Information
{
    /// <summary>
    /// All of the valid scenes in Geometry Dash.
    /// </summary>
    public enum GameScene
    {
        /// <summary>
        /// The main menu.
        /// </summary>
        MainMenu = 0,
        
        /// <summary>
        /// The level select scene.
        /// </summary>
        Select = 1,
        
        /// <summary>
        /// The level play scene.
        /// </summary>
        Play = 3,
        
        /// <summary>
        /// The level search scene.
        /// </summary>
        Search = 4,
        
        /// <summary>
        /// The leaderboard scene.
        /// </summary>
        Leaderboard = 6,
        
        /// <summary>
        /// The online section of the game. 
        /// </summary>
        Online = 7,
        
        /// <summary>
        /// The listing of official levels.
        /// </summary>
        OfficialLevelListing = 8,
        
        /// <summary>
        /// The level play scene, except for official levels.
        /// </summary>
        OfficialLevel = 9,
        
        /// <summary>
        /// The.
        /// </summary>
        TheChallenge = 12,
        
        /// <summary>
        /// Unknow
        /// </summary>
        Unknown = 63
    }
}