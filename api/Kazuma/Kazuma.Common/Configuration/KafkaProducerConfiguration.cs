﻿using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaghetti.Common.Configuration
{
    public class KafkaProducerConfiguration : ProducerConfig
    {
        public int? CommitTimeoutMs { get; set; }
    }
}
