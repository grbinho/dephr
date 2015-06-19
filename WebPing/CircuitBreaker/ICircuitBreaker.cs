using System;
using System.Threading.Tasks;

namespace WebPing.CircuitBreaker
{
    public interface ICircuitBreaker
    {
        TResult Execute<TResult>(Func<TResult> action);
        Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
        void Execute(Action action);
        Task ExecuteAsync(Func<Task> action);
    }
}
