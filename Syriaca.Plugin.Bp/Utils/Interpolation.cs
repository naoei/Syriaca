namespace Syriaca.Plugin.Bp.Utils
{
    public static class Interpolation
    {
        public static double ValueAt(double time, double val1, double val2, double startTime, double endTime)
        {
            if (val1 == val2)
                return val1;

            var current = time - startTime;
            var duration = endTime - startTime;

            if (current == 0)
                return val1;
            if (duration == 0)
                return val2;

            var t = current / duration;
            return val1 + t * (val2 - val1);
        }
    }
}