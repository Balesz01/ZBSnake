using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace ZBSnake.View
{
    public class DrawGame
    {
        private ImageBrush appleBrush;
        private ImageBrush snakeBrush;

        // Konstruktor: ez fut le legelőször, amikor a DrawGame létrejön
        public DrawGame()
        {
            // 1. Az alma kép betöltése
            appleBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/alma.png")));

            snakeBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/snakehead.png")));

        }

        public void Draw(Canvas gameCanvas, int[,] map, int cellSize)
        {
            gameCanvas.Children.Clear();

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    // --- 1. LÉPÉS: A PÁLYA (MAP) HÁTTERÉNEK MEGRAJZOLÁSA ---
                    Rectangle bgRect = new Rectangle
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    // Sakktábla minta: ha X+Y páros, sötétzöld, különben sima zöld
                    if ((x + y) % 2 == 0)
                    {
                        bgRect.Fill = Brushes.DarkGreen;
                    }
                    else
                    {
                        bgRect.Fill = Brushes.ForestGreen;
                    }

                    Canvas.SetLeft(bgRect, x * cellSize);
                    Canvas.SetTop(bgRect, y * cellSize);
                    gameCanvas.Children.Add(bgRect);


                    // --- 2. LÉPÉS: A KÍGYÓ ÉS AZ ALMA RÁRAJZOLÁSA A FŰRE ---
                    int cellValue = map[y, x];

                    // Ha nem 0, akkor van ott valami
                    if (cellValue != 0)
                    {
                        Rectangle entityRect = new Rectangle
                        {
                            Width = cellSize,
                            Height = cellSize,
                            // Egy kis margó, hogy a kígyó kockái elkülönüljenek egymástól
                            Margin = new Thickness(1)
                        };

                        if (cellValue == 1) // Kígyó
                        {
                            // Ha sikerült betölteni a képet, azt használjuk, különben zöld színű lesz
                            if (snakeBrush != null) entityRect.Fill = snakeBrush;
                            else entityRect.Fill = Brushes.LimeGreen;
                        }
                        else if (cellValue == 2) // Alma
                        {
                            // Ha sikerült betölteni a képet, azt használjuk, különben piros színű lesz
                            if (appleBrush != null) entityRect.Fill = appleBrush;
                            else entityRect.Fill = Brushes.Red;
                        }

                        Canvas.SetLeft(entityRect, x * cellSize);
                        Canvas.SetTop(entityRect, y * cellSize);
                        gameCanvas.Children.Add(entityRect);
                    }
                }
            }

        }
    }
}
