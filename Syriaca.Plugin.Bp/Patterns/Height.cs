namespace Syriaca.Plugin.Bp.Patterns
{
    public class Height : PatternHandler
    {
        public override PatternType Type => PatternType.Height;

        private float multiplier;

        public Height(uint motor, bool isOpposite, float multiplier) 
            : base(motor, isOpposite)
        {
            this.multiplier = multiplier;
        }

        public override double Execute(BpPlugin plugin)
        {
            return 0;
        }
    }
}


/*
Jumping and hitting the circles or how fast you move vertically
Probably also when moving through
portals
and each player mode probably needs different 'feels' for jumping
like the plane would be more constant
and the spider would be more spiky
*/