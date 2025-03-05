using Quartz.Impl;
using Quartz;
using CurrencyRateMonitor.Handlers;

namespace CurrencyRateMonitor.Scheduler
{
    /// <summary>
    /// Класс расписания
    /// </summary>
    internal class CurrencyScheduler
    {
        /// <summary>
        /// Метод для старта сохранения данных по расписанию
        /// </summary>
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


        /// <summary>
        /// Метод для конвертации времени из конфига в нужный формат для Cron
        /// </summary>
        /// <returns>Строка в формате СС ММ ЧЧ </returns>
        private static string ReadTimeFromConfig()
        {
            var time = ConfigurationHandler.Сonfiguration.GetSection("CronTime").Value;
            if (TimeOnly.TryParse(time, out var check))
            {
                return $"{check.Second} {check.Minute} {check.Hour}";
            }
            else
            {
                Console.WriteLine("Неверный формат времени, выгрузка будет происходить ежедневно в 12:00");
                return "0 0 12";
            }
        }
    }
}
