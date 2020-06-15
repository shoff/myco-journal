namespace Myco.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Hosting.Internal;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MycoHost : IHost, IAsyncDisposable
    {
        private readonly ILogger<MycoHost> logger;
        private readonly IHostLifetime hostLifetime;
        private readonly ApplicationLifetime applicationLifetime;
        private readonly HostOptions options;
        private IEnumerable<IHostedService> hostedServices;


        public MycoHost(IServiceProvider services, IHostApplicationLifetime applicationLifetime, ILogger<MycoHost> logger,
            IHostLifetime hostLifetime, IOptions<HostOptions> options)
        {
            this.Services = services ?? throw new ArgumentNullException(nameof(services));
            this.applicationLifetime = (applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime))) as ApplicationLifetime;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IServiceProvider Services { get; }

        public void Dispose() => DisposeAsync().GetAwaiter().GetResult();

        public async ValueTask DisposeAsync()
        {
            switch (this.Services)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync();
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            using var combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource
                (cancellationToken, this.applicationLifetime.ApplicationStopping);
            var combinedCancellationToken = combinedCancellationTokenSource.Token;

            await this.hostLifetime.WaitForStartAsync(combinedCancellationToken);

            combinedCancellationToken.ThrowIfCancellationRequested();
            this.hostedServices = this.Services.GetService<IEnumerable<IHostedService>>();

            foreach (var hostedService in this.hostedServices)
            {
                // Fire IHostedService.Start
                await hostedService.StartAsync(combinedCancellationToken).ConfigureAwait(false);
            }

            // Fire IHostApplicationLifetime.Started
            this.applicationLifetime?.NotifyStarted();
        }

        public async Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            using (var cts = new CancellationTokenSource(this.options.ShutdownTimeout))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken))
            {
                var token = linkedCts.Token;
                // Trigger IHostApplicationLifetime.ApplicationStopping
                this.applicationLifetime?.StopApplication();

                IList<Exception> exceptions = new List<Exception>();
                if (this.hostedServices != null) // Started?
                {
                    foreach (var hostedService in this.hostedServices.Reverse())
                    {
                        token.ThrowIfCancellationRequested();
                        try
                        {
                            await  hostedService.StopAsync(token).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(ex);
                        }
                    }
                }

                token.ThrowIfCancellationRequested();
                await this.hostLifetime.StopAsync(token);

                // Fire IHostApplicationLifetime.Stopped
                this.applicationLifetime?.NotifyStopped();

                if (exceptions.Count > 0)
                {
                    var ex = new AggregateException("One or more hosted services failed to stop.", exceptions);
                    this.logger.LogError(ex, ex.Message);
                    throw ex;
                }
            }
        }

    }
}