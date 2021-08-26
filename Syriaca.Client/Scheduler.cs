using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Syriaca.Client
{
    public class Scheduler
    {
        private readonly Queue<Action> runQueue = new();

        public int Delay { get; }
        public string Name { get;  }

        public readonly Stopwatch Stopwatch = new();

        public Scheduler(int delay, MemberInfo type)
        {
            Delay = delay;
            Name = type.Name;
        }

        public void Pulse()
        {
            Stopwatch.Restart();

            foreach (var action in runQueue)
                action();
        }

        public void Add(Action action)
        {
            runQueue.Enqueue(action);
        }
    }
}