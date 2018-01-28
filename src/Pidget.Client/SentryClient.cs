﻿using Pidget.Client.DataModels;
using System;
using System.Threading.Tasks;

namespace Pidget.Client
{
    public abstract class SentryClient
    {
        public const string Name = "pidget";

        public static string Version => VersionNumber.Get();

        public Dsn Dsn { get; }

        protected SentryClient(Dsn dsn)
        {
            Assert.ArgumentNotNull(dsn, nameof(dsn));

            Dsn = dsn;
        }

        public Task<SentryResponse> CaptureAsync(
            Action<SentryEventBuilder> builderAccessor)
        {
            var builder = new SentryEventBuilder();
            builderAccessor(builder);

            return SendEventAsync(builder.Build());
        }

        public abstract Task<SentryResponse> SendEventAsync(
            SentryEventData sentryEvent);
    }
}
