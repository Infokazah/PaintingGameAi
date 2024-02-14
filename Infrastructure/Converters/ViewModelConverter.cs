using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PaintGameMVVM.Infrastructure.Converters
{
    class ViewModelConverter
    {
        public ViewModelConverter() { }

        public Color StringToColorConvert(string str)
        {
            if (str is string colorString && colorString.StartsWith("#") && colorString.Length == 9)
            {
                try
                {
                    byte a = Convert.ToByte(colorString.Substring(1, 2), 16);
                    byte r = Convert.ToByte(colorString.Substring(3, 2), 16);
                    byte g = Convert.ToByte(colorString.Substring(5, 2), 16);
                    byte b = Convert.ToByte(colorString.Substring(7, 2), 16);
                    return Color.FromArgb(a, r, g, b);
                }
                catch (Exception)
                {
                    return Colors.Black; 
                }
            }
            return Colors.Black; 
        }

        public byte[] InkCanvasToByteArr(InkCanvas canvas)
        {
            if (canvas is InkCanvas inkCanvas)
            {

                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                    (int)inkCanvas.ActualWidth,
                    (int)inkCanvas.ActualHeight,
                    96,
                    96,
                    PixelFormats.Pbgra32);
                renderTargetBitmap.Render(inkCanvas);

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    encoder.Save(memoryStream);

                    return memoryStream.ToArray();
                }
            }
            return null;
        }
    }
}
