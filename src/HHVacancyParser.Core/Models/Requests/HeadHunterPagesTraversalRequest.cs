using System;

namespace HHVacancyParser.Core.Models.Requests
{
    public class HeadHunterPagesTraversalRequest
    {
        public HeadHunterRequest Request { get; set; } = null!;

        public int[] Pages { get; set; } = null!;

        /// <summary>
        /// Delay between requests in milliseconds.
        /// </summary>
        public HeadHunterDelayOptions DelayOptions { get; set; } = new HeadHunterDelayOptions();
    }

    public class HeadHunterDelayOptions
    {
        public bool UseRandomDelay { get; set; } = true;

        public int MaxDelay { get; set; } = 1000;
        public int MinDelay { get; set; } = 100;
        public int StaticDelay { get; set; } = 50;

        private readonly Random _random = new Random();

        public int GetDelay()
        {
            if (UseRandomDelay)
            {
                var randomDelay = _random.Next(MinDelay, MaxDelay + 1);

                return randomDelay;
            }

            return StaticDelay;
        }
    }
}
