using System;
using System.Collections.Generic;
using System.Diagnostics;
using Syriaca.Client.Utils;

namespace Syriaca.Client
{
    /// <summary>
    /// A scheduler to run actions at a set interval.
    /// </summary>
    public class Scheduler
    {
        private readonly Queue<Action> runQueue = new();

        /// <summary>
        /// The delay to wait after each pulse.
        /// </summary>
        public int Delay { get; }
        
        /// <summary>
        /// The name of this scheduler.
        /// </summary>
        public string Name { get; }

        public readonly Stopwatch Stopwatch = new();

        public Scheduler(int delay)
        {
            Delay = delay;
            Name = DebugUtils.GetCallingClass();
        }

        /// <summary>
        /// Executes all actions in the run queue and restarts the timer.
        /// </summary>
        public void Pulse()
        {
            Stopwatch.Restart();

            foreach (var action in runQueue)
                action();
        }

        /// <summary>
        /// Adds an action to the run queue
        /// </summary>
        /// <param name="action">The action to add.</param>
        public void Add(Action action)
        {
            Logger.Debug($"Enqueueing action: {action.Method.Name} for {Name}");
            runQueue.Enqueue(action);
        }

        /// <summary>
        /// Clears the run queue.
        /// </summary>
        public void Clear()
        {
            runQueue.Clear();
        }
    }
}