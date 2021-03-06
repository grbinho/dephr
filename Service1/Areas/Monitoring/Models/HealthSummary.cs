﻿using System;
using Dephr;

namespace Service1.Areas.Monitoring.Models
{
    public class HealthSummary
    {
        public string Name { get; private set; }
        public string Endpoint { get; set; }
        public long LastResponseTime { get; private set; }
        public bool Up { get; private set; }

        public long AvgResponseTime
        {
            get
            {
                return (long)Math.Ceiling(TotalResponseTime / (decimal)TotalHearthBeats);
            }
        }

        public long MaxResponseTime { get; private set; }
        public long MinResponseTime { get; private set; }

        public decimal PercentUp
        {
            get
            {
                return TotalUpHearthBeats / (decimal)TotalHearthBeats * 100;
            }
        }

        public long TotalHearthBeats { get; private set; }
        public long TotalUpHearthBeats { get; private set; }
        public long TotalResponseTime { get; private set; }

        public HealthSummary(string name)
        {
            Name = name;
            MinResponseTime = long.MaxValue;
        }

        public void AddHearthBeat(HearthBeat beat)
        {
            if (!beat.ServiceName.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            LastResponseTime = beat.ResponseTime;
            TotalResponseTime += beat.ResponseTime;
            Up = beat.Up;
            TotalHearthBeats++;

            if (beat.Up)
            {
                TotalUpHearthBeats++;
            }

            if (beat.ResponseTime > MaxResponseTime)
            {
                MaxResponseTime = beat.ResponseTime;
            }

            if (beat.ResponseTime < MinResponseTime)
            {
                MinResponseTime = beat.ResponseTime;
            }
        }
    }
}
