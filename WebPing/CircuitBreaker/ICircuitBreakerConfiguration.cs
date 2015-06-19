namespace WebPing.CircuitBreaker
{
    public interface ICircuitBreakerConfiguration
    {
        int BreakOnNumberOfExceptions { get; }
        int BreakCircuitForSeconds { get; }
    }
}
