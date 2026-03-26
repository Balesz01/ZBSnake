using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ZBSnake.Models;
using ZBSnake.View;
using ZBSnake.Controller;

namespace ZBSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public partial class MainWindow : Window
    {
        public DispatcherTimer simTimer;
        public SimulationTime Time = new SimulationTime();
        private DrawGame renderer = new DrawGame();
        private Direction currentDirection = Direction.Right;
        private int[,] map = new int[20, 20];
        private int cellSize = 40;

        public MainWindow()
        {
            InitializeComponent();

            SetupGame();
            simTimer = new DispatcherTimer();
            // A TimeRate határozza meg, milyen gyors a játék (pl. 0.1 vagy 0.2 másodperc)
            simTimer.Interval = TimeSpan.FromSeconds(Time.TimeRate);
            simTimer.Tick += SimTimer_Tick;

            simTimer.Start();
        }

        private void SimTimer_Tick(object sender, EventArgs e)
        {

            // MoveSnake(); 

            // CheckCollisions();

            // 3. Evés vizsgálat (Megevett egy almát?)
            // Ha igen, nő a pontszám, hosszabbodik a kígyó, és esetleg gyorsul a timer:
            // simTimer.Interval = TimeSpan.FromSeconds(UjGyorsabbIdo);

            // 4. Képernyő (UI) frissítése / Kígyó újrarajzolása
            renderer.Draw(GameCanvas, map, cellSize);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // A gombnyomás CSAK az irányt változtatja meg.
            // Extra szabály: a kígyó nem fordulhat meg 180 fokban önmagába!
            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    if (currentDirection != Direction.Down)
                        currentDirection = Direction.Up;
                    break;

                case Key.S:
                case Key.Down:
                    if (currentDirection != Direction.Up)
                        currentDirection = Direction.Down;
                    break;

                case Key.A:
                case Key.Left:
                    if (currentDirection != Direction.Right)
                        currentDirection = Direction.Left;
                    break;

                case Key.D:
                case Key.Right:
                    if (currentDirection != Direction.Left)
                        currentDirection = Direction.Right;
                    break;
            }
        }

        private void SetupGame()
        {
            // Biztos ami biztos, letöröljük az egész pályát (csupa 0 lesz)
            Array.Clear(map, 0, map.Length);

            // A kígyó feje (1-es) kerüljön a 10. sor 10. oszlopába (pont a pálya közepe)
            map[10, 10] = 1;

            // Az alma (2-es) kerüljön mondjuk az 5. sor 15. oszlopába
            map[5, 15] = 2;
        }

    }
}