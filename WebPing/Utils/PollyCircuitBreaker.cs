using System;
using System.Threading.Tasks;
using Polly;
using System.Diagnostics;
using Polly.CircuitBreaker;

namespace WebPing.Utils
{
    public class PollyCircuitBreaker : ICircuitBreaker
    {
        private readonly ICircuitBreakerConfiguration _config;
        private readonly Policy _policy;
        private readonly Policy _policyAsync;

        public PollyCircuitBreaker(ICircuitBreakerConfiguration config)
        {
            _config = config;

            try
            {
                _policyAsync = Policy.Handle<Exception>().CircuitBreakerAsync(config.BreakOnNumberOfExceptions, TimeSpan.FromSeconds(config.BreakCircuitForSeconds));
                _policy = Policy.Handle<Exception>().CircuitBreaker(config.BreakOnNumberOfExceptions, TimeSpan.FromSeconds(config.BreakCircuitForSeconds));
            }
            catch (Exception ex)
            {
                _policy = null;
                _policyAsync = null;

                Trace.WriteLine(string.Format("Error occured trying to create polly policies. Exception: {0}", ex.ToString()));
            }

            
        }

        public void Execute(Action action)
        {
            if(_policy == null)
            {
                action.Invoke();
            }
            else
            {
                try
                {
                    _policy.Execute(action);
                }
                catch(BrokenCircuitException ex)
                {
                    //TODO: need additional information about from where did this happen

                    Trace.WriteLine(string.Format("Circuit was broken. Exception: {0}", ex.ToString()));
                    throw new CircuitBrokenException();
                }
            }
            
        }

        public TResult Execute<TResult>(Func<TResult> action)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(Func<Task> action)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            throw new NotImplementedException();
        }
    }
}
