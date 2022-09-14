using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBient.Tasks
{
    /// <summary>
    /// TaskRunner which implements use of a Queue (Runs tasks in FILO order).
    /// </summary>
    public class QueueTaskRunner : TaskRunner
    {
        readonly Queue<IEnumerator> tasks;

        QueueTaskRunner()
        {
            tasks = new();
        }

        public override bool HasTasks() => tasks.Count > 0;

        public override void AddTask(IEnumerator task)
        {
            tasks.Enqueue(task);
        }

        public override void AddTasks(params IEnumerator[] tasks)
        {
            foreach (var task in tasks)
                AddTask(task);
        }

        protected override bool TryGetNextTask(out IEnumerator nextTask)
        {
            return tasks.TryDequeue(out nextTask);
        }
    }
}