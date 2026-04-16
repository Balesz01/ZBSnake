using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using ZBSnake.Models;
using ZBSnake.View;
using ZBSnake.Controller;

namespace ZBSnake
{
    public enum Direction { Up, Down, Left, Right }

    public partial class MainWindow : Window
    {
        public DispatcherTimer simTimer;
        public SimulationTime Time = new SimulationTime();
        private DrawGame renderer = new DrawGame();
        private Movement movement;
        private Scoretodatabase db;

        private Direction currentDirection = Direction.Right;
        private Direction nextDirection = Direction.Right;

        private int[,] map = new int[20, 20];
        private int cellSize = 40;

        private TextBlock scoreText;
        private bool gameOverHandled = false;

        public MainWindow()
        {
            InitializeComponent();

            try { db = new Scoretodatabase(); }
            catch (Exception ex)
            {
                MessageBox.Show($"Adatbázis hiba: {ex.Message}\nA játék adatbázis nélkül fut.",
                                "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            scoreText = new TextBlock
            {
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Text = "Pontszám: 0"
            };
            Canvas.SetLeft(scoreText, 10);
            Canvas.SetTop(scoreText, 10);
            Panel.SetZIndex(scoreText, 10);
            GameCanvas.Children.Add(scoreText);

            SetupGame();

            simTimer = new DispatcherTimer();
            simTimer.Interval = TimeSpan.FromSeconds(Time.TimeRate);
            simTimer.Tick += SimTimer_Tick;
            simTimer.Start();
        }

        private void SimTimer_Tick(object sender, EventArgs e)
        {
            currentDirection = nextDirection;

            Time.IncreaseSpeed();
            movement.MoveSnake(map, currentDirection);

            if (movement.IsGameOver)
            {
                simTimer.Stop();
                // Utolsó keret kirajzolása
                renderer.Draw(GameCanvas, map, cellSize, movement.SnakeBody);
                UpdateScore();

                if (!gameOverHandled)
                {
                    gameOverHandled = true;
                    HandleGameOver();
                }
                return;
            }

            // Normál keret: átadjuk a snakeBody-t a helyes irányú rajzoláshoz
            renderer.Draw(GameCanvas, map, cellSize, movement.SnakeBody);
            UpdateScore();

            // Pontszám szöveg mindig felül marad
            GameCanvas.Children.Remove(scoreText);
            GameCanvas.Children.Add(scoreText);
            Panel.SetZIndex(scoreText, 999);
        }

        private void HandleGameOver()
        {
            string nev = Microsoft.VisualBasic.Interaction.InputBox(
                $"Játék vége! Pontszámod: {movement.Score}\n\nAdd meg a neved a mentéshez:",
                "Eredmény mentése",
                "Játékos");

            bool isNewRecord = false;

            if (!string.IsNullOrWhiteSpace(nev) && db != null)
            {
                try { isNewRecord = db.TrySaveScore(nev.Trim(), movement.Score); }
                catch (Exception ex)
                {
                    MessageBox.Show($"Mentési hiba: {ex.Message}", "Hiba",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            ShowGameOver(isNewRecord);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    if (currentDirection != Direction.Down) nextDirection = Direction.Up; break;
                case Key.S:
                case Key.Down:
                    if (currentDirection != Direction.Up) nextDirection = Direction.Down; break;
                case Key.A:
                case Key.Left:
                    if (currentDirection != Direction.Right) nextDirection = Direction.Left; break;
                case Key.D:
                case Key.Right:
                    if (currentDirection != Direction.Left) nextDirection = Direction.Right; break;
                case Key.R:
                    RestartGame(); break;
            }
        }

        private void SetupGame()
        {
            Array.Clear(map, 0, map.Length);
            movement = new Movement(20, 20);
            movement.InitSnake(map, 10, 10);
            movement.SpawnApple(map);
            currentDirection = Direction.Right;
            nextDirection = Direction.Right;
            gameOverHandled = false;
        }

        private void RestartGame()
        {
            SetupGame();
            Time = new SimulationTime();
            simTimer.Interval = TimeSpan.FromSeconds(Time.TimeRate);
            simTimer.Start();
        }

        private void UpdateScore()
        {
            scoreText.Text = $"Pontszám: {movement.Score}";
        }

        private void ShowGameOver(bool isNewRecord = false)
        {
            string recordText = isNewRecord ? "\n🏆 ÚJ REKORD!" : "";

            var overlay = new TextBlock
            {
                Text = $"GAME OVER!{recordText}\nPontszám: {movement.Score}\n\nNyomj R-t az újraindításhoz",
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)),
                TextAlignment = TextAlignment.Center,
                Padding = new Thickness(20),
                TextWrapping = TextWrapping.Wrap
            };

            Canvas.SetLeft(overlay, 150);
            Canvas.SetTop(overlay, 300);
            Panel.SetZIndex(overlay, 1000);
            GameCanvas.Children.Add(overlay);
        }
    }
}