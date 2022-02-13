using System;
using System.Linq;
using System.Reflection;
using Syriaca.Client.Utils;
using Syriaca.Plugin.Bp.Tcp.Handlers;

namespace Syriaca.Plugin.Bp.Tcp
{
    public static class TcpManager
    {
        public static TcpHandler FindSuitableHandler(OpCodes code)
        {
            var opEnum = Assembly.GetAssembly(typeof(TcpHandler))!
               .GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(TcpHandler)))
               .Select(t => (TcpHandler) Activator.CreateInstance(t));
            
            Logger.Info($"{code} handled.");

            return opEnum.First(o => o?.OpCode == code);
        }
    }
}