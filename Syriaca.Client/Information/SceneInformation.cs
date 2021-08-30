using System.Collections.Generic;

namespace Syriaca.Client.Information
{
    public class SceneInformation
    {
        public GameScene Scene { get; }

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