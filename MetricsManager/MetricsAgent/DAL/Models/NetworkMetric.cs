﻿using System;

namespace MetricsAgent
{
    public class NetworkMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}
