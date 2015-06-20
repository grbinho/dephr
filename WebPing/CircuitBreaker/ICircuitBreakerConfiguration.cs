namespace Dephr.CircuitBreaker
{
    public interface ICircuitBreakerConfiguration
    {
        int BreakOnNumberOfExceptions { get; }
        int BreakCircuitForSeconds { get; }
    }
}
