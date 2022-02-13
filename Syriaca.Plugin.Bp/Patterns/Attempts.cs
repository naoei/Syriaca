namespace Syriaca.Plugin.Bp.Patterns
{
    public class Attempts : PatternHandler
    {
        public override PatternType Type => PatternType.Attempts;

        private int threshold;

        public Attempts(uint motor, bool isOpposite, int threshold) 
            : base(motor, isOpposite)
        {
            this.threshold = threshold;
        }

        public override double Execute(BpPlugin plugin)
        {
            return 0;
        }
    }
}