namespace WebPing.Utils
{
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
