using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using ZBSnake.Controller;

namespace ZBSnake.View
{
    public class DrawGame
    {
        private BitmapImage appleImg;
        private BitmapImage headImg;
        private BitmapImage bodyImg;
        private BitmapImage tailImg;

        public DrawGame()
        {
            appleImg = new BitmapImage(new Uri("pack://application:,,,/Images/alma.png"));
            headImg = new BitmapImage(new Uri("pack://application:,,,/Images/snakehead.png"));
            bodyImg = new BitmapImage(new Uri("pack://application:,,,/Images/snakebody.png"));
            tailImg = new BitmapImage(new Uri("pack://application:,,,/Images/snakeend.png"));
        }

        /// <summary>
        /// WPF RotateTransform az óramutató járásával forgat (CW).
        /// A képek alapból BALRA néznek, ezért:
        ///   Balra  =   0°  (nincs forgatás)
        ///   Jobbra = 180°
        ///   Fel    = -90°  (= 270° CW, de -90 is működik WPF-ben)
        ///   Le     =  90°
        /// </summary>
        private double GetAngle((int row, int col) from, (int row, int col) to)
        {
            int dr = to.row - from.row;
            int dc = to.col - from.col;

            if (dc == -1 && dr == 0) return 180;    // Balra
            if (dc == 1 && dr == 0) return 0;  // Jobbra
            if (dr == -1 && dc == 0) return -90;  // Fel   (CW: -90 = CCW 90)
            if (dr == 1 && dc == 0) return 90;   // Le

            return 0;
        }

        private Rectangle MakeImageRect(BitmapImage img, int cellSize, double angleDeg)
        {
            var rect = new Rectangle
            {
                Width = cellSize,
                Height = cellSize,
                Fill = new ImageBrush(img) { Stretch = Stretch.UniformToFill },
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(angleDeg)
            };
            return rect;
        }

        public void Draw(Canvas gameCanvas, int[,] map, int cellSize,
                         LinkedList<(int row, int col)> snakeBody = null)
        {
            gameCanvas.Children.Clear();

            int rowCount = map.GetLength(0);
            int colCount = map.GetLength(1);

            // 1. Háttér sakktábla
            for (int y = 0; y < rowCount; y++)
                for (int x = 0; x < colCount; x++)
                {
                    var bg = new Rectangle
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Fill = (x + y) % 2 == 0 ? Brushes.DarkGreen : Brushes.ForestGreen
                    };
                    Canvas.SetLeft(bg, x * cellSize);
                    Canvas.SetTop(bg, y * cellSize);
                    gameCanvas.Children.Add(bg);
                }

            // 2. Alma
            for (int y = 0; y < rowCount; y++)
                for (int x = 0; x < colCount; x++)
                    if (map[y, x] == 2)
                    {
                        var apple = MakeImageRect(appleImg, cellSize, 0);
                        Canvas.SetLeft(apple, x * cellSize);
                        Canvas.SetTop(apple, y * cellSize);
                        gameCanvas.Children.Add(apple);
                    }

            // 3. Kígyó – csak ha van snakeBody
            if (snakeBody == null || snakeBody.Count == 0) return;

            var segs = new List<(int row, int col)>(snakeBody);
            int n = segs.Count;

            for (int i = 0; i < n; i++)
            {
                var seg = segs[i];
                double angle;
                BitmapImage img;

                if (i == 0)
                {
                    // FEJ: nyak → fej iránya = mozgás iránya
                    img = headImg;
                    angle = n > 1 ? GetAngle(segs[1], segs[0]) : 0;
                }
                else if (i == n - 1)
                {
                    // FAROK VÉGE: előző szegmens → farok iránya
                    img = tailImg;
                    angle = GetAngle(segs[i - 1], segs[i]);
                }
                else
                {
                    // TEST: előző szegmens → aktuális iránya
                    img = bodyImg;
                    angle = GetAngle(segs[i - 1], segs[i]);
                }

                var rect = MakeImageRect(img, cellSize, angle);
                Canvas.SetLeft(rect, seg.col * cellSize);
                Canvas.SetTop(rect, seg.row * cellSize);
                gameCanvas.Children.Add(rect);
            }
        }
    }
}