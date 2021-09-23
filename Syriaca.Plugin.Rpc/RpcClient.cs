using System;
using DiscordRPC;
using DiscordRPC.Message;
using Syriaca.Client.Utils;

namespace Syriaca.Plugin.Rpc
{
    public class RpcClient : IDisposable
    {
        private const string client_id = "419568479172034561";
        private readonly DiscordRpcClient client;
        public readonly RichPresence Presence = new();

        public event Action OnReady;

        public RpcClient()
        {
            client = new DiscordRpcClient(client_id);

            client.OnReady += onReady;
            client.OnConnectionFailed += onConnectionFailed;
            client.OnError += onError;
            
            client.Initialize();
        }

        private void onError(object _, ErrorMessage args)
        {
            Logger.Error(args.Message);
        }

        private void onConnectionFailed(object _, ConnectionFailedMessage args)
        {
            Logger.Error($"Failed to connect to Discord's RPC sever.");
            client.Deinitialize();
        }

        private void onReady(object _, ReadyMessage ready)
        {
            Logger.Log($"RPC Ready! Running v{ready.Version} for {ready.User}");
            
            OnReady?.Invoke();
            client.SetPresence(Presence);
        }

        public void ChangeStatus(Action<RichPresence> newStatus)
        {
            newStatus(Presence);
        }

        public void Update()
        {
            client.SetPresence(Presence);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}