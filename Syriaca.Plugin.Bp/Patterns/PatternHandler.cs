namespace Syriaca.Plugin.Bp.Patterns
{
    public abstract class PatternHandler
    {
        public abstract PatternType Type { get; }
        public uint Motor { get; }
        public bool IsOpposite { get; }

        protected PatternHandler(uint motor, bool isOpposite)
        {
            Motor = motor;
            IsOpposite = isOpposite;
        }

        public abstract double GetValue(BpPlugin plugin);
    }
    
    public enum PatternType
    {
        None = -1,
        LevelProgression = 0,
        Attempts = 1,
        Height = 2
    }
}