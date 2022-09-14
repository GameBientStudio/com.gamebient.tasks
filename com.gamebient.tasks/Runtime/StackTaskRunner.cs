using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBient.Tasks
{
    /// <summary>
    /// TaskRunner which implements use of a Stack (Runs tasks in FIFO order).
    /// </summary>
    public class StackTaskRunner : TaskRunner
    {
        readonly Stack<IEnumerator> tasks;

        StackTaskRunner()
        {
            tasks = new();
        }

        public override bool HasTasks() => tasks.Count > 0;

        public override void AddTask(IEnumerator task)
        {
            tasks.Push(task);
        }

        public override void AddTasks(params IEnumerator[] tasks)
        {
            foreach (var task in tasks)
                AddTask(task);
        }

        protected override bool TryGetNextTask(out IEnumerator nextTask)
        {
            return tasks.TryPop(out nextTask);
        }
    }
}