namespace Syriaca.Plugin.Bp.Patterns
{
    public class LevelProgress : PatternHandler
    {
        public override PatternType Type => PatternType.LevelProgression;

        public LevelProgress(uint motor, bool isOpposite) 
            : base(motor, isOpposite)
        {
        }

        public override double Execute(BpPlugin plugin)
        {
            return 0;
        }
    }
}