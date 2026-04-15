using System;
using System.Diagnostics;

namespace ZBSnake.Models
{
    /// <summary>
    /// Stopwatch alapú időzítő – a DispatcherTimer pontatlanságát kiváltja.
    /// A mozgás akkor történik, ha legalább TimeRate másodperc eltelt az utolsó lépés óta.
    /// </summary>
    public class SimulationTime
    {
        private Stopwatch stopwatch = new Stopwatch();
        private double accumulated = 0;

        /// <summary>
        /// Két lépés között eltelő idő másodpercben. Kisebb = gyorsabb.
        /// </summary>
        public double TimeRate { get; set; } = 0.3;

        public SimulationTime()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Meghívandó minden UI tick-nél (pl. 16ms-es DispatcherTimer).
        /// Visszatér true-val, ha a kígyónak lépnie kell.
        /// </summary>
        public bool ShouldStep()
        {
            double elapsed = stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();

            accumulated += elapsed;

            if (accumulated >= TimeRate)
            {
                accumulated -= TimeRate;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Alma evésekor gyorsítás.
        /// </summary>
        public void IncreaseSpeed()
        {
            if (TimeRate > 0.05)
                TimeRate -= 0.01;
        }

        public void Reset()
        {
            TimeRate = 0.2;
            accumulated = 0;
            stopwatch.Restart();
        }
    }
}