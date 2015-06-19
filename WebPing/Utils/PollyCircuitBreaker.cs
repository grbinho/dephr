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
            if (_policy == null)
            {
                return action.Invoke();
            }
            else
            {
                try
                {
                    return _policy.Execute(action);
                }
                catch (BrokenCircuitException ex)
                {
                    //TODO: need additional information about from where did this happen

                    Trace.WriteLine(string.Format("Circuit was broken. Exception: {0}", ex.ToString()));
                    throw new CircuitBrokenException();
                }
            }
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            if (_policyAsync == null)
            {
                await action.Invoke();
            }
            else
            {
                try
                {
                    await _policyAsync.ExecuteAsync(action);
                }
                catch (BrokenCircuitException ex)
                {
                    //TODO: need additional information about from where did this happen

                    Trace.WriteLine(string.Format("Circuit was broken. Exception: {0}", ex.ToString()));
                    throw new CircuitBrokenException();
                }
            }
        }

        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            if (_policyAsync == null)
            {
                return await action.Invoke();
            }
            else
            {
                try
                {
                    return await _policyAsync.ExecuteAsync(action);
                }
                catch (BrokenCircuitException ex)
                {
                    //TODO: need additional information about from where did this happen

                    Trace.WriteLine(string.Format("Circuit was broken. Exception: {0}", ex.ToString()));
                    throw new CircuitBrokenException();
                }
            }
        }
    }
}
