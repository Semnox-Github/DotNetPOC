/********************************************************************************************
 * Project Name - Products Controller/BarCodeController
 * Description  - Created to  Genrate Barcodes for products using GenCode128 dll
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.50        02-Feb-2019   Muhammed Mehraj          Created to Generate Barcodes for products.
 ****************************************************************************************************
 *2.50        18-Mar-2019   Akshay Gulaganji        Added Execution Context and Custom Generic Exception in response
 ***************************************************************************************************/
using GenCode128;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.CommonServices
{
    public class BarCodeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Genrates Barcodes for products 
        /// Convert a bitmap image to memorystream and send the response
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/CommonServices/Barcode/")]
        [Authorize]
        public HttpResponseMessage Get(string activityType, int productId = -1, string productName = null, int weight = -1, string barcodeText = null, bool showPrice = true, bool showDesrciption = true)
        {
            try
            {
                log.LogMethodEntry(productName, weight);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                string value = string.Empty;
                Image image = null;
                if (activityType.ToUpper().ToString() == "BARCODE")
                {
                    image = Code128Rendering.MakeBarcodeImage(productName, weight, true);

                    if (image != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                            value = System.Convert.ToBase64String(stream.ToArray());
                        }
                    }
                }
                else if (activityType.ToUpper().ToString() == "INVENTORYDETAILSGENERATEBARCODE")
                {
                    PrinterBL printerBL = new PrinterBL(executionContext);
                    image = printerBL.MakeBarcodeLibImage(weight, 40, BarcodeLib.TYPE.CODE128.ToString(), barcodeText);
                    ProductBL productBL = new ProductBL(executionContext, productId, false, false);

                    string productDescription = productBL.getProductDTO.Description;
                    double price = productBL.getProductDTO.Cost;
                    int yLocation = 0;
                    int width = 400;
                    int height = 85;
                    int fontSize = 8;

                    if (weight == 1)
                        fontSize = 8;
                    else if (weight == 2)
                        fontSize = 9;
                    else if (weight == 3)
                        fontSize = 10;
                    else if (weight >= 4)
                        fontSize = 12;

                    Bitmap bmp = new Bitmap(width, height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        using (Font myFont = new Font("Arial", fontSize, FontStyle.Regular))
                        {
                            Utilities utilities = new Utilities();
                            string str = price == -1 ? "" : price.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                            SizeF stringSize = new SizeF();
                            stringSize = g.MeasureString(str, myFont);
                            g.DrawString(barcodeText, myFont, Brushes.Black, 0, yLocation);
                            if (showPrice)
                                g.DrawString(str, myFont, Brushes.Black, image.Width - stringSize.Width, yLocation);
                            yLocation += Convert.ToInt32(stringSize.Height);
                            g.DrawImage(
                                        //Code128Rendering.MakeBarcodeImage(txtInput.Text, int.Parse(txtWeight.Text), true),
                                        printerBL.MakeBarcodeLibImage(weight, 40, BarcodeLib.TYPE.CODE128.ToString(), barcodeText),
                                        new Rectangle(0, yLocation, width, height),  // destination rectangle  
                                        0,
                                        0,           // upper-left corner of source rectangle
                                        width,       // width of source rectangle
                                        height,      // height of source rectangle
                                        GraphicsUnit.Pixel,
                                        null);
                            yLocation += image.Height;
                            if (showDesrciption)
                                g.DrawString(productDescription, myFont, Brushes.Black, 0, yLocation);
                        }
                    }
                    image = (Image)bmp;
                    using (var stream = new MemoryStream())
                    {
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        value = System.Convert.ToBase64String(stream.ToArray());
                    }
                }
                log.LogMethodExit(value);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = value, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
