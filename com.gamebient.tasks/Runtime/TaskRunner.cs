using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBient.Tasks
{
    /// <summary>
    /// Abstract wrapper class for enumeration tasks to run in a MonoBehaviour.
    /// </summary>
    public abstract class TaskRunner
    {
        public System.Action OnTasksFinished;
        public System.Action<object> OnStatusChanged;

        readonly float minimumTimeToRun;
        float startTime;


        public TaskRunner()
        {
            minimumTimeToRun = 0;
        }

        public TaskRunner(float minimumTimeToRun) : this()
        {
            this.minimumTimeToRun = minimumTimeToRun;
        }

        /// <summary>
        /// <returns>Returns true if there are any tasks.</returns>
        /// </summary>
        public abstract bool HasTasks();

        /// <summary>
        /// Adds one task.
        /// </summary>
        public abstract void AddTask(IEnumerator task);

        /// <summary>
        /// Adds one or more tasks.
        /// </summary>
        public abstract void AddTasks(params IEnumerator[] tasks);

        /// <summary>
        /// Returns true if a next task was found.
        /// <param name="nextTask">The next task</param>
        /// </summary>
        protected abstract bool TryGetNextTask(out IEnumerator nextTask);

        /// <summary>
        /// Starts work on the first task. If there is none, OnTaskFinished is called.
        /// </summary>
        public IEnumerator StartWork()
        {
            startTime = Time.unscaledTime;
            if (TryGetNextTask(out IEnumerator firstTask))
            {
                yield return RunTask(firstTask);
            }
            else
            {
                OnTasksFinished?.Invoke();
                yield break;
            }
        }

        float GetWaitTime()
        {
            return Mathf.Max(0, minimumTimeToRun - (Time.unscaledTime - startTime));
        }

        IEnumerator RunTask(IEnumerator task)
        {
            while (task.MoveNext())
            {
                object current = task.Current;
                OnStatusChanged?.Invoke(current);
                yield return current;
            }

            if (TryGetNextTask(out IEnumerator nextTask))
                yield return RunTask(nextTask);
            else
            {
                yield return new WaitForSecondsRealtime(GetWaitTime());
            }
            OnTasksFinished?.Invoke();
        }
    }
}
