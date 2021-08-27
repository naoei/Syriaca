using System;
using System.Collections.Generic;
using System.Diagnostics;
using Syriaca.Client.Utils;

namespace Syriaca.Client
{
    public class Scheduler
    {
        private readonly Queue<Action> runQueue = new();

        public int Delay { get; }
        public string Name { get;  }

        public readonly Stopwatch Stopwatch = new();

        public Scheduler(int delay)
        {
            Delay = delay;
            Name = DebugUtils.GetCallingClass();
        }

        public void Pulse()
        {
            Stopwatch.Restart();

            foreach (var action in runQueue)
                action();
        }

        public void Add(Action action)
        {
            Logger.Debug($"Enqueueing action: {action.Method.Name} for {Name}");
            runQueue.Enqueue(action);
        }

        public void Clear()
        {
            runQueue.Clear();
        }
    }
}