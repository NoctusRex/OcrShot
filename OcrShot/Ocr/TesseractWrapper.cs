using System.Drawing;
using System.IO;
using Tesseract;

namespace OcrShot.Ocr
{
    public sealed class TesseractWrapper
    {
        /// <summary>
        /// https://github.com/tesseract-ocr/tessdata
        /// https://stackoverflow.com/questions/10947399/how-to-implement-and-do-ocr-in-a-c-sharp-project
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string DoMagic(Bitmap image)
        {
            if (!Directory.Exists(@"./tessdata")) Directory.CreateDirectory(@"./tessdata");

            using TesseractEngine engine = new("./tessdata", "eng");
            using Pix pix = new BitmapToPixConverter().Convert(image);
            using Page page = engine.Process(pix);

            return page.GetText();
        }
    }

}
