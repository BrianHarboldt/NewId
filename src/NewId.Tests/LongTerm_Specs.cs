﻿namespace NewId.Tests
{
    using System;
    using NUnit.Framework;
    using Providers;

    [TestFixture]
    public class Generating_ids_over_time
    {
        [Test]
        public void Should_keep_them_ordered_for_sql_server()
        {
            var generator = new NewIdGenerator(new DefaultNetworkIdProvider(), new TimeLapseTickProvider());
            generator.Next();

            int limit = 1024;

            var ids = new NewId[limit];
            for (int i = 0; i < limit; i++)
            {
                ids[i] = generator.Next();
            }

            for (int i = 0; i < limit - 1; i++)
            {
                Assert.AreNotEqual(ids[i], ids[i + 1]);
                Assert.Less(ids[i], ids[i + 1]);
                if (i%16 == 0)
                    Console.WriteLine(ids[i]);
            }
        }

        class TimeLapseTickProvider :
            TickProvider
        {
            DateTime _previous = DateTime.UtcNow;
            TimeSpan _interval = TimeSpan.FromSeconds(2);
            public long Ticks
            {
                get
                {
                    _previous = _previous + _interval;
                    _interval = TimeSpan.FromSeconds((long) _interval.TotalSeconds + 30);
                    return _previous.Ticks;
                }
            }
        }
    }
}