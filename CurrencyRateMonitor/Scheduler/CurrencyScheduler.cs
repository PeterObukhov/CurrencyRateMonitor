using Quartz.Impl;
using Quartz;
using CurrencyRateMonitor.Handlers;

namespace CurrencyRateMonitor.Scheduler
{
    internal class CurrencyScheduler
    {
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<CurrencySaver>()
                .Build();

            var cronTime = ReadTimeFromConfig();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithCronSchedule($"{cronTime} * * ?")
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        private static string ReadTimeFromConfig()
        {
            var time = ConfigurationHandler.Сonfiguration.GetSection("CronTime").Value.Split(":");
            if (time.Length == 2)
            {
                return $"0 {time[1]} {time[0]}";
            }
            else if (time.Length == 3)
            {
                return $"{time[2]} {time[1]} {time[0]}";
            }
            else return "0 0 12";
        }
    }
}
