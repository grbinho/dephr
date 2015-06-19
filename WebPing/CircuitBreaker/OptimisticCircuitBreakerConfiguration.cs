namespace WebPing.CircuitBreaker
{
    /// <summary>
    /// Breaks circuit for 30 seconds and tolerates 3 exceptions.
    /// </summary>
    public class OptimisticCircuitBreakerConfiguration : ICircuitBreakerConfiguration
    {
        public int BreakCircuitForSeconds
        {
            get
            {
                return 30;
            }
        }

        public int BreakOnNumberOfExceptions
        {
            get
            {
                return 3;
            }
        }
    }
}
