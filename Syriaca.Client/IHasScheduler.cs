namespace Syriaca.Client
{
    public interface IHasScheduler
    {
        public Scheduler Scheduler { get; }

        public void StartScheduler();
    }
}