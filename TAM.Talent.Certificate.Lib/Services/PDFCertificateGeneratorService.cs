using Microsoft.AspNetCore.Hosting;
using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using TAM.Talent.Certificate.Lib.Models;

namespace TAM.Talent.Certificate.Lib
{
    public class PDFCertificateGeneratorService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public PDFCertificateGeneratorService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// export pdf into stream data type
        /// </summary>
        /// <returns></returns>
        public Stream ExportPdfStreamData(PdfDataModel model)
        {
            var pdfBytes = new byte[0];
            using (var stream = new SKDynamicMemoryWStream())
            {
                var certificatePDFMetaData = new SKDocumentPdfMetadata
                {
                    Author = "System",
                    Creation = DateTime.Now,
                    Creator = "System",
                    Title = $@"test - { DateTime.Now.ToString("dd-MM-yyyy") }"
                };

                SetupCertificatePage(stream, certificatePDFMetaData, model);

                pdfBytes = stream.CopyToData().ToArray();
            }

            var pdfStream = new MemoryStream(pdfBytes);

            return pdfStream;
            //File.WriteAllBytes(@"D:\talent certificate\asd.pdf", test);
        }


        private void SetupCertificatePage(SKWStream stream, SKDocumentPdfMetadata certificatePDFMetaData, PdfDataModel model)
        {
            string projectRootPath = _hostingEnvironment.ContentRootPath;

            var certifBytes = System.IO.File.ReadAllBytes(projectRootPath + @"\wwwroot\blue_with_underline.png");
            //var certifBytes = System.IO.File.ReadAllBytes(@"C:\Web Applications\iTalent\wwwroot\blue_with_underline.png");
            if (model.ProgramTypeId == 1)
            {
                //certifBytes = System.IO.File.ReadAllBytes(@"C:\Web Applications\iTalent\wwwroot\blue_with_underline.png");
                certifBytes = System.IO.File.ReadAllBytes(projectRootPath + @"\wwwroot\blue_with_underline.png");
            }
            else if (model.ProgramTypeId == 2)
            {
                //certifBytes = System.IO.File.ReadAllBytes(@"C:\Web Applications\iTalent\wwwroot\red_with_underline.png");
                certifBytes = System.IO.File.ReadAllBytes(projectRootPath + @"\wwwroot\red_with_underline.png");
            }
            else if (model.ProgramTypeId == 3)
            {
                //certifBytes = System.IO.File.ReadAllBytes(@"C:\Web Applications\iTalent\wwwroot\purple_with_underlinepng");
                certifBytes = System.IO.File.ReadAllBytes(projectRootPath + @"\wwwroot\purple_with_underline.png");
            }
            else if (model.ProgramTypeId == 4)
            {
                //certifBytes = System.IO.File.ReadAllBytes(@"C:\Web Applications\iTalent\wwwroot\gold_with_underline.png");
                certifBytes = System.IO.File.ReadAllBytes(projectRootPath + @"\wwwroot\gold_with_underline.png");
            }

            var certifBackground = SKBitmap.Decode(certifBytes);

            var certifWidth = certifBackground.Width;
            var certifHeight = certifBackground.Height;

            //FontFamily nunito = new FontFamily(@"/assets/Nunito/Nunito-Black.ttf");
            //FontFamily nunitoFont = new FontFamily("https://fonts.googleapis.com/css?family=Nunito");
            var nunito = "Nunito";

            using (var certificatePDFDocument = SKDocument.CreatePdf(stream, certificatePDFMetaData))
            {
                using (var certificateCanvas = certificatePDFDocument.BeginPage(certifWidth, certifHeight))
                {
                    //BackgroundImage
                    certificateCanvas.DrawBitmap(certifBackground, 0, 0);

                    //Certificate Number
                    //var certifNoText = $@"No.456789012345";
                    var certifNoText = $@"" + model.CertificateNumber;
                    var certifNoPaint = new SKPaint
                    {
                        TextSize = 24,
                        Color = SKColors.Black,
                        TextAlign = SKTextAlign.Center,
                        Typeface = SKTypeface.FromFamilyName(
                            nunito,
                            SKFontStyleWeight.SemiBold,
                            SKFontStyleWidth.Normal,
                            SKFontStyleSlant.Upright)
                    };

                    certificateCanvas.DrawText(certifNoText, certifWidth / 2, 115, certifNoPaint);

                    var headerName = $@"CERTIFICATE OF COMPLETION";
                     if(model.EnumCertificate != "Default"){
                        headerName =  $@"" + model.EnumCertificate.ToUpper();
                    }
                    var headerPaint = new SKPaint
                    {
                        TextSize = 31,
                        Color = SKColors.Black,
                        TextAlign = SKTextAlign.Center,
                        Typeface = SKTypeface.FromFamilyName(
                            nunito,
                            SKFontStyleWeight.SemiBold,
                            SKFontStyleWidth.Normal,
                            SKFontStyleSlant.Upright)
                    };

                    certificateCanvas.DrawText(headerName, certifWidth / 2, 170, headerPaint);


                    //Host Name
                    //var hostNameText = $@"Toyota Training Center";
                    var hostNameText = $@"" + model.Host;
                    var hostNamePaint = new SKPaint
                    {
                        TextSize = 30,
                        Color = SKColors.Black,
                        TextAlign = SKTextAlign.Center,
                        Typeface = SKTypeface.FromFamilyName(
                            nunito,
                            SKFontStyleWeight.SemiBold,
                            SKFontStyleWidth.Normal,
                            SKFontStyleSlant.Upright)
                    };

                    certificateCanvas.DrawText(hostNameText, certifWidth / 2, 210, hostNamePaint);


                    //Name
                    var nameText = $@"" + model.EmployeeName;
                    var namePaint = new SKPaint
                    {
                        TextSize = 32,
                        Color = SKColors.Black,
                        TextAlign = SKTextAlign.Center,
                        Typeface = SKTypeface.FromFamilyName(
                            nunito,
                            SKFontStyleWeight.SemiBold,
                            SKFontStyleWidth.Normal,
                            SKFontStyleSlant.Upright)
                    };

                    certificateCanvas.DrawText(nameText, certifWidth / 2, 285, namePaint);


                    //Training
                    var trainingNameText = $@"" + model.TrainingName;

                    /* if(model.EnumCertificate != "Default"){
                        trainingNameText =  $@"" + model.EnumCertificate;
                    }*/
                    var trainingNamePaint = new SKPaint
                    {
                        TextSize = 28,
                        Color = SKColors.Black,
                        TextAlign = SKTextAlign.Center,
                        Typeface = SKTypeface.FromFamilyName(
                            nunito,
                            SKFontStyleWeight.SemiBold,
                            SKFontStyleWidth.Normal,
                            SKFontStyleSlant.Upright)
                    };

                    certificateCanvas.DrawText(trainingNameText, certifWidth / 2, 350, trainingNamePaint);


                    //Place and Date
                    var dates = model.EventDate == null ? "" : model.EventDate.Value.ToString("MM/dd/yyyy");
                    var placeAndDateText = $@"" + model.Location + ", " + dates;
                    var placeAndDatePaint = new SKPaint
                    {
                        TextSize = 24,
                        Color = SKColors.Black,
                        TextAlign = SKTextAlign.Center,
                        Typeface = SKTypeface.FromFamilyName(
                            nunito,
                            SKFontStyleWeight.Normal,
                            SKFontStyleWidth.Normal,
                            SKFontStyleSlant.Upright)
                    };

                    certificateCanvas.DrawText(placeAndDateText, certifWidth / 5, 500, placeAndDatePaint);


                    //digital signature
                    //string path = @"../TAM.Talent.Certificate.Lib/assets/99e3a499-05d5-4f84-b913-f4fdf9462452.png";
                    //var getImage = Resize(path, 150, 100);
                    //var signatureImg = SKBitmap.Decode(getImage);


                    if (model.SignatureBytes != null)
                    {
                        var signatureStream = new MemoryStream(model.SignatureBytes);
                        var signatureImg = SKBitmap.Decode(signatureStream);

                        SKImageInfo resizeInfo = new SKImageInfo(signatureImg.Width, signatureImg.Height, signatureImg.ColorType, signatureImg.AlphaType, signatureImg.ColorSpace)
                        {
                            Height = 100,
                            Width = 150
                        };

                        var resizedBitMap = signatureImg.Resize(resizeInfo, SKFilterQuality.High);

                        certificateCanvas.DrawBitmap(resizedBitMap, (int)(certifWidth - (certifWidth / 3.6)), 565);
                    }


                    //name under signature
                    var signatureName = $@"" + model.SignatureEmployeeName;
                    var name2Paint = new SKPaint
                    {
                        TextSize = 24,
                        Color = SKColors.White,
                        TextAlign = SKTextAlign.Center,
                        Typeface = SKTypeface.FromFamilyName(
                            nunito,
                            SKFontStyleWeight.Normal,
                            SKFontStyleWidth.Normal,
                            SKFontStyleSlant.Upright)
                    };

                    certificateCanvas.DrawText(signatureName, (int)(certifWidth - (certifWidth / 5.5)), 615, name2Paint);


                    //position
                    var positionText = $@"" + model.Position;
                    var positionPaint = new SKPaint
                    {
                        TextSize = 24,
                        Color = SKColors.White,
                        TextAlign = SKTextAlign.Center,
                        Typeface = SKTypeface.FromFamilyName(
                            nunito,
                            SKFontStyleWeight.Normal,
                            SKFontStyleWidth.Normal,
                            SKFontStyleSlant.Upright)
                    };

                    certificateCanvas.DrawText(positionText, (int)(certifWidth - (certifWidth / 5.5)), 645, positionPaint);

                    certificatePDFDocument.EndPage();
                }

                certificatePDFDocument.Close();
            }

        }

        private byte[] Resize(string srcPath, int width, int height)
        {
            Image image = Image.FromFile(srcPath);
            Bitmap resultImage = Resize(image, width, height);
            //resultImage.Save(srcPath.Replace(".png", "_" + width + "x" + height + ".png"));
            //return resultImage;

            //ImageConverter converter = new ImageConverter();
            //return (byte[])converter.ConvertTo(resultImage, typeof(byte[]));
            using (var stream = new MemoryStream())
            {
                resultImage.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private Bitmap Resize(Image image, int width, int height)
        {

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

    }
}
