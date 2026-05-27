using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

namespace WorldTime
{
    public class WorldTimeWatcher : MonoBehaviour
    {
        [SerializeField]
        private WorldTime _worldTime;
        [SerializeField]
        private List<Schedule> _schedule;

        private void Start()
        {
            _worldTime.WorldTimeChanged += CheckSchedules;
        }
        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= CheckSchedules;
        }
        private void CheckSchedules(object sender, TimeSpan newTime)
        {
            var schedule =
                 _schedule.FirstOrDefault(s =>
                 s.Hour == newTime.Hours &&
                 s.Minute == newTime.Minutes);

            schedule?._action?.Invoke();
        }
        [Serializable]
        public class Schedule
        {
            public int Hour;
            public int Minute;
            public UnityEvent _action;
        }
    }
}
