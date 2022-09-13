using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBient.Tasks
{
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

        public abstract bool HasTasks();

        public abstract void AddTask(IEnumerator task);

        public abstract void AddTasks(params IEnumerator[] tasks);

        protected abstract bool TryGetNextTask(out IEnumerator nextTask);


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
