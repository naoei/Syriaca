namespace Syriaca.Plugin.Bp.Patterns
{
    public abstract class PatternHandler
    {
        public abstract PatternType Type { get; }

        public uint Motor { get; set; }

        private bool isOpposite;

        protected PatternHandler(uint motor, bool isOpposite)
        {
            Motor = motor;
            this.isOpposite = isOpposite;
        }

        public abstract double Execute(BpPlugin plugin);
    }
    
    public enum PatternType
    {
        None = -1,
        LevelProgression = 0,
        Attempts = 1,
        Height = 2
    }
}