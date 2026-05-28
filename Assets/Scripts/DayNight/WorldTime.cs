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

        [SerializeField]
        private int _startHour = 6;  // Giờ bắt đầu (6 = 6:00 sáng)

        [SerializeField]
        private int _startMinute = 0; // Phút bắt đầu

        private TimeSpan _currentTime;
        private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

        private void Start()
        {
            // Khởi tạo thời gian bắt đầu
            _currentTime = new TimeSpan(_startHour, _startMinute, 0);

            // Gọi event để cập nhật hiển thị và light ngay khi start
            WorldTimeChanged?.Invoke(this, _currentTime);

            // Bắt đầu vòng lặp thời gian
            StartCoroutine(AddMinute());
        }

        private IEnumerator AddMinute()
        {
            yield return new WaitForSeconds(_minuteLength);

            _currentTime = _currentTime.Add(TimeSpan.FromMinutes(1));
            WorldTimeChanged?.Invoke(this, _currentTime);

            StartCoroutine(AddMinute());
        }

        // Optional: Method để set thời gian thủ công
        public void SetTime(int hour, int minute)
        {
            _currentTime = new TimeSpan(hour, minute, 0);
            WorldTimeChanged?.Invoke(this, _currentTime);
        }
    }
}