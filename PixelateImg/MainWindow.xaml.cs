using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Drawing.Imaging.ImageCodecInfo;

namespace PixelateImg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //var originalImage = new Bitmap($"D:/figma.jpg");
            var originalImage = new Bitmap($"{textBox.Text}");

            // Define the grid size, which can be customized as needed
            //const int gridWidth = 10;
            var gridWidth = Convert.ToInt32(textBox3.Text);
            var gridHeight = Convert.ToInt32(textBox4.Text);

            // Define the width of the grid lines
            var gridLineWidth = Convert.ToSingle(textBox5.Text);

            // Count the number of grids (divided from the middle)
            var numGridsX = originalImage.Width / gridWidth;
            var numGridsY = originalImage.Height / gridHeight;

            // Get the adjusted mesh size, ensuring left and right and up and down symmetry
            var adjustedGridWidth = originalImage.Width / numGridsX;
            var adjustedGridHeight = originalImage.Height / numGridsY;

            // Create a new image with the same size as the original image
            var newImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Iterate each grid
            for (var y = 0; y < numGridsY; y++)
            {
                for (var x = 0; x < numGridsX; x++)
                {
                    // Get the grid area (divided from the middle)
                    var gridX = x * adjustedGridWidth + (gridWidth - adjustedGridWidth) / 2;
                    var gridY = y * adjustedGridHeight + (gridHeight - adjustedGridHeight) / 2;
                    var gridRect = new System.Drawing.Rectangle(gridX, gridY, adjustedGridWidth, adjustedGridHeight);

                    // Count the number of times each color appears in the grid
                    var mostCommonColor = GetMostCommonColor(originalImage, gridRect);

                    // Fill the color inside the grid
                    using var g = Graphics.FromImage(newImage);
                    using (var brush = new SolidBrush(mostCommonColor))
                    {
                        g.FillRectangle(brush, gridRect);
                    }

                    // grid line
                    using (var pen = new Pen(Color.Black, gridLineWidth))
                    {
                        g.DrawRectangle(pen, gridRect);
                    }
                }
            }

            // Save the new image locally, specifying the JPEG encoding parameters
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] =
                new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L); // 100L indicates the highest quality JPEG
            var jpegCodec = GetEncoderInfo("image/jpeg");
            newImage.Save($"{textBox1.Text}", jpegCodec, encoderParams);

            // Make the picture black and white
            var blackAndWhiteImage = ConvertToBlackAndWhite(newImage);
            blackAndWhiteImage.Save($"{textBox2.Text}", jpegCodec, encoderParams);
        }
        private static Color GetMostCommonColor(Bitmap image, Rectangle area)
        {
            // Use a dictionary to count the number of color occurrences
            var colorCount = new Dictionary<Color, int>();
            for (var y = area.Top; y < area.Bottom; y++)
            {
                for (var x = area.Left; x < area.Right; x++)
                {
                    var pixelColor = image.GetPixel(x, y);
                    if (colorCount.ContainsKey(pixelColor))
                    {
                        colorCount[pixelColor]++;
                    }
                    else
                    {
                        colorCount[pixelColor] = 1;
                    }
                }
            }

            // Find the color that appears the most
            var maxCount = 0;
            var mostCommonColor = Color.Black;
            foreach (var pair in colorCount.Where(pair => pair.Value > maxCount))
            {
                maxCount = pair.Value;
                mostCommonColor = pair.Key;
            }

            return mostCommonColor;
        }

        private static Bitmap ConvertToBlackAndWhite(Bitmap image)
        {
            var blackAndWhiteImage = new Bitmap(image.Width, image.Height);
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var pixelColor = image.GetPixel(x, y);
                    // Convert the color to black and white
                    var grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    var newColor = Color.FromArgb(grayValue, grayValue, grayValue);
                    blackAndWhiteImage.SetPixel(x, y, newColor);
                }
            }

            return blackAndWhiteImage;
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var codecs = GetImageEncoders();
            return codecs.FirstOrDefault(codec => codec.MimeType == mimeType);
        }
    }
}
