using System;
using System.Collections;
using UnityEngine;


namespace WorldTime

{
    public class WorldTime : MonoBehaviour
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;
        [SerializeField]
        private float _dayLength;
        
        private TimeSpan _currentTime;
        private float _miunuteLength => _dayLength / WorldTimeConstants.MinutesInDay; // 1440 phút trong một ngày
        
        private void Start()
        {
            StartCoroutine(AddMinute());
        }
        private IEnumerator AddMinute()
        {
            _currentTime += TimeSpan.FromMinutes(1);
            WorldTimeChanged?.Invoke(this, _currentTime); 
            yield return new WaitForSeconds(_miunuteLength);
            StartCoroutine(AddMinute());
        }
    }
    
    
}
