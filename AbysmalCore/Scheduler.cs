using AbysmalCore.Debugging;

namespace AbysmalCore
{
    /// <summary>
    /// A helper class to schedule functions to execute
    /// </summary>
    [DebugInfo("abysmal core standard event schedule system")]
    public class Scheduler
    {
        /// <summary>
        /// Keep track so garbage collection doesnt kill "unused" timers
        /// </summary>
        private List<Timer> _timers = new();
        private readonly object _lock = new object();

        /// <summary>
        /// Schedules an event to occur every (n)ms
        /// </summary>
        /// <param name="ms">The interval</param>
        /// <param name="ev">The event to fire every interval</param>
        /// <remarks>
        /// IMPORTANT: The event will be unscheduled if it returns true.
        /// This method is not thread-safe
        /// </remarks>
        public void ScheduleInterval(int ms, Func<bool> ev)
        {
            // needs to be outside the callbakc
            Timer? timer = null;

            TimerCallback callback = state =>
            {
                Timer currentTimer = (Timer)state!;

                if (ev())
                {
                    currentTimer.Dispose();
                    lock (_lock) { _timers.Remove(currentTimer); }
                }
            };

            // start and pass stat (timer)
            // timer is null, so we pass null
            timer = new Timer(
                callback,
                null, // placeholder
                Timeout.Infinite,
                ms
            );

            // timer is no longer null, we can start
            timer.Change(0, ms);

            // prevent gc
            lock (_lock) { _timers.Add(timer); }
        }

        /// <summary>
        /// Schedules <paramref name="ev"/> to occur in (<paramref name="ms"/>)ms without blocking the current thread
        /// </summary>
        /// <param name="ms">The delay</param>
        /// <param name="ev">The event to fire</param>
        public void Schedule(int ms, Action ev)
        {
            Task.Run(async () => await ScheduleAsync(ms, ev));
        }

        /// <summary>
        /// Schedules <paramref name="ev"/> to occur in (<paramref name="ms"/>)ms
        /// </summary>
        /// <param name="ms">The delay</param>
        /// <param name="ev">The event to fire</param>
        public async Task ScheduleAsync(int ms, Action ev)
        {
            AbysmalDebug.Log(this, $"Scheduled event {ev.Method.Name} to occur in {ms}ms");
            await Task.Delay(ms);
            AbysmalDebug.Log(this, $"Scheduled event {ev.Method.Name} fired");
            ev();
        }

        /// <summary>
        /// Schedules <paramref name="ev"/> to fire at <paramref name="time"/>
        /// </summary>
        /// <param name="time"></param>
        /// <param name="ev"></param>
        public void ScheduleAt(DateTime time, Action ev)
        {
            // delay
            TimeSpan delay = time - DateTime.Now;
            int ms = (int)delay.TotalMilliseconds;

            if (ms < 0)
            {
                AbysmalDebug.Warn(this, $"Target time {time} is in the past ({ms}ms). Firing immediately");
                ms = 0;
            }

            Schedule(ms, ev);
        }
    }
}
