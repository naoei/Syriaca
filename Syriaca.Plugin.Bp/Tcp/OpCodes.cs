namespace Syriaca.Plugin.Bp.Tcp
{
    public enum OpCodes
    {
        // SERVER | Always starts with 0x0#
        Heartbeat = 0x00,
        Configuration = 0x01,
        
        // CLIENT | Always starts with 0x1#
        SendCommand = 0x10
    }
}