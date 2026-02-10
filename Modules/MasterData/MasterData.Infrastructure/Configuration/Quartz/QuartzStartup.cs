using MasterData.Infrastructure.Configuration.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace MasterData.Infrastructure.Configuration.Quartz;

public static class QuartzServiceCollectionExtensions
{
    public static void AddQuartzScheduler(
        this IServiceCollection services,
        long? internalProcessingPoolingInterval = null)
    {
        Console.WriteLine("\n=== Configuring Quartz Scheduler ===");

        var scheduler = CreateAndStartScheduler();

        ScheduleOutboxJob(scheduler, internalProcessingPoolingInterval);

        services.AddSingleton(scheduler);
    }

    /// <summary>
    /// Create and start the Quartz scheduler
    /// </summary>
    private static IScheduler CreateAndStartScheduler()
    {
        var schedulerConfiguration = new System.Collections.Specialized.NameValueCollection
            { { "quartz.scheduler.instanceName", "MasterDataScheduler" } };

        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
        var               scheduler        = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
        scheduler.Start().GetAwaiter().GetResult();

        return scheduler;
    }

    private static void ScheduleOutboxJob(IScheduler scheduler, long? intervalMs)
    {
        var outboxJobKey = new JobKey("ProcessOutboxJob", "MasterData");
        var outboxJob = JobBuilder.Create<ProcessOutboxJob>()
            .WithIdentity(outboxJobKey)
            .Build();
        var outboxTrigger = CreateTrigger("ProcessOutboxTrigger", intervalMs);

        scheduler.ScheduleJob(outboxJob, outboxTrigger).GetAwaiter().GetResult();
    }

    private static ITrigger CreateTrigger(string triggerName, long? intervalMs)
    {
        ITrigger trigger;

        if (intervalMs.HasValue)
        {
            trigger = TriggerBuilder
                .Create()
                .WithIdentity(triggerName, "MasterData")
                .StartNow()
                .WithSimpleSchedule(x => x
                                        .WithInterval(TimeSpan.FromMilliseconds(intervalMs.Value))
                                        .RepeatForever())
                .Build();
        }
        else
        {
            trigger = TriggerBuilder
                .Create()
                .WithIdentity(triggerName, "MasterData")
                .StartNow()
                // .WithCronSchedule("0/2 * * ? * *")
                .WithCronSchedule("0/5 * * ? * *")
                .Build();
        }

        return trigger;
    }
}