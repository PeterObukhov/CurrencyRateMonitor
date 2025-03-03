using Quartz.Impl;
using Quartz;

namespace CurrencyRateMonitor
{
    internal class CurrencyScheduler
    {
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<CurrencySaver>()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithCronSchedule("0 0 12 * * ?")
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
