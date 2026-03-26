using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ZBSnake.View
{
    public class DrawGame
    {
        // Készítünk egy metódust, ami paraméterként megkapja a szükséges dolgokat
        public void Draw(Canvas gameCanvas, List<Point> snakeParts, Point apple, int snakeSize)
        {
            // 1. Töröljük a vásznat
            gameCanvas.Children.Clear();

            // 2. Kirajzoljuk a kígyót
            foreach (Point part in snakeParts)
            {
                Rectangle snakeSquare = new Rectangle
                {
                    Width = snakeSize,
                    Height = snakeSize,
                    Fill = Brushes.LimeGreen,
                    Margin = new Thickness(1)
                };

                Canvas.SetLeft(snakeSquare, part.X);
                Canvas.SetTop(snakeSquare, part.Y);
                gameCanvas.Children.Add(snakeSquare);
            }

            // 3. Kirajzoljuk az almát
            Rectangle appleSquare = new Rectangle
            {
                Width = snakeSize,
                Height = snakeSize,
                Fill = Brushes.Red
            };

            Canvas.SetLeft(appleSquare, apple.X);
            Canvas.SetTop(appleSquare, apple.Y);
            gameCanvas.Children.Add(appleSquare);
        }
    }
}
