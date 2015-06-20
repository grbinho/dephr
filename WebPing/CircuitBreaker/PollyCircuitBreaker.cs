using System;
using System.Threading.Tasks;
using Polly;
using System.Diagnostics;
using Polly.CircuitBreaker;

namespace Dephr.CircuitBreaker
{
    public class PollyCircuitBreaker : ICircuitBreaker
    {
        private readonly ICircuitBreakerConfiguration _config;
        private readonly Policy _policy;
        private readonly Policy _policyAsync;

        /// <summary>
        /// Circuit breaker based on Polly library (https://github.com/michael-wolfenden/Polly)
        /// </summary>
        /// <param name="config">Circuit breaker configuration</param>
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
        /// <summary>
        /// Execute action in circuit breaker.
        /// </summary>
        /// <param name="action">Action to execute.</param>
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
        /// <summary>
        /// Execute action in circuit breaker.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="action">Action to execute.</param>
        /// <returns>Action result.</returns>
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
        /// <summary>
        /// Execute action in circuit breaker.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <returns>Task.</returns>
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
        /// <summary>
        /// Execute action in circuit breaker
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="action">Action to execute.</param>
        /// <returns>Action result.</returns>
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
