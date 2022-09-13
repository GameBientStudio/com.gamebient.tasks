using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBient.Tasks
{
    public interface IRunner
    {
        bool HasTasks();
        void AddTask(IEnumerator task);
        bool TryGetNextTask(out IEnumerator nextTask);
    }
}