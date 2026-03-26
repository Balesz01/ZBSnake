using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBSnake.Models
{
    public class SimulationTime
    {
        /// <summary>
        /// A játék alapvető sebessége másodpercben (mennyi idő telik el két lépés között).
        /// Kisebb szám = gyorsabb játék.
        /// </summary>
        public double TimeRate { get; set; }

        public SimulationTime()
        {
            // Kezdeti sebesség beállítása (pl. 0.2 másodpercenként lép egyet a kígyó)
            TimeRate = 0.2;
        }

        /// <summary>
        /// Ezt a metódust akkor hívhatod meg, ha a kígyó megevett egy almát, 
        /// és szeretnéd, hogy a játék egy kicsit gyorsuljon.
        /// </summary>
        public void IncreaseSpeed()
        {
            // Ne engedjük, hogy a játék játszhatatlanul gyors legyen (pl. 0.05 alá ne menjen)
            if (TimeRate > 0.05)
            {
                // Minden evésnél csökkentjük az időközt 0.01 másodperccel
                TimeRate -= 0.01;
            }
        }
    }
}
