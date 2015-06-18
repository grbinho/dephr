namespace WebPing.Utils
{
    public interface ICircuitBreakerConfiguration
    {
        int BreakOnNumberOfExceptions { get; }
        int BreakCircuitForSeconds { get; }
    }
}
