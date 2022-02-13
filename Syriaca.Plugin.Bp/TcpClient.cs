using System.Linq;
using SimpleTcp;
using Syriaca.Plugin.Bp.Tcp;

namespace Syriaca.Plugin.Bp
{
    public class TcpClient : SimpleTcpClient
    {
        public TcpClient()
            : base("127.0.0.1:9090")
        {
        }
        
        public void Start()
        {
            Logger += log => Client.Utils.Logger.Log(sanitizeMessage(log));
            Events.Connected  += logConnected;
            Events.Disconnected  += logDisconnected;
            Events.DataReceived += onDataReceived;
            
            ConnectWithRetries(10);
        }

        private void logConnected(object o, ConnectionEventArgs args)
        {
            Client.Utils.Logger.Info($"[[{args.IpPort}]] client connected.");
        }
        
        private void logDisconnected(object o, ConnectionEventArgs args)
        {
            Client.Utils.Logger.Warn($"[[{args.IpPort}]] client disconnected: {args.Reason}.");
        }
        
        private void onDataReceived(object o, DataReceivedEventArgs args)
        {
            var op = (OpCodes) args.Data[0];
            var operation = TcpManager.FindSuitableHandler(op);
            
            operation.HandleData(args.Data.Skip(1).ToArray());
        }

        private string sanitizeMessage(string message)
            => message.Replace("[", "[[").Replace("]", "]]");
    }
}