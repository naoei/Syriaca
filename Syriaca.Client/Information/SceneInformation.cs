using System.Collections.Generic;

namespace Syriaca.Client.Information
{
    /// <summary>
    /// Holds information about a scene. 
    /// </summary>
    public class SceneInformation
    {
        /// <summary>
        /// The scene to hold information for.
        /// </summary>
        public GameScene Scene { get; }

        /// <summary>
        /// The information for this scene.
        /// </summary>
        public Dictionary<string, object> SceneData { get; }

        public SceneInformation()
            : this(GameScene.Unknown)
        {
        }

        public SceneInformation(GameScene scene)
            : this(scene, new Dictionary<string, object>())
        {
        }

        public SceneInformation(GameScene scene, Dictionary<string, object> sceneData)
        {
            Scene = scene;
            SceneData = sceneData;
        }
    }
}