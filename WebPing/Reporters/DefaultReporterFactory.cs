namespace WebPing.Reporters
{
    public static class DefaultReporterFactory
    {
        public static IHearthBeatReporter CreateReporter(DefaultReporters reporter)
        {
            switch (reporter)
            {
                case DefaultReporters.Trace: return new TraceReporter();
                case DefaultReporters.SignalR: return new SignalRReporter();
                default: return new TraceReporter();
            }
        }
    }
}
