/***********************************************************************************************************************
 * Project Name - Printer BL
 * Description  - Business logic to handle Printer
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 ************************************************************************************************************************
 *1.00        17-Sep-2018      Mathew Ninan      Created
 *2.60        25-Mar-2019      Jagana Mohana Rao added constructors and SaveUpdatePrinterList() method in PrinterListBL class
 *            16-May-2019      Mushahid Faizan   Added SQL Transaction in SaveUpdatePrinterList() and created GetAllPrinterList() method.
 *2.70        5-Jul -2019      Girish Kundar     Modified : Passing execution context object as parameter to the Lookups constructor.
 *2.70.2      18-Jul-2019      Deeksha           Modifications as per 3 tier standard.
 *            3-Oct-2019       Rakesh Kumar      Added ReceiptImage() method
 *            10-Oct-2019      Rakesh Kumar      Added PrintTicketsToPrinter() method having return type Image
 *2.120       17-Apr-2021      Guru S A          Wristband printing flow enhancements
 *2.120       27-Apr-2021      Girish Kundar     Duplicate Wristband printing issue Fix
 *2.130.4     08-Mar-2022      Mathew Ninan      New Method SetupThePrinting to print to a file 
 *2.140       14-Sep-2021      Fiona             Modified: Issue fixes added sqlTransaction parameter for GetPrinterProducts
 *2.130.10    01-Sep-2022      Vignesh Bhat      Support for Reverse card number logic is missing in RFID printer card reads
 ********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Device;
using Semnox.Parafait.Customer;
using QRCoder;
using Semnox.Parafait.Communication;
using System.IO;
using System.Globalization;
using iTextSharp.text.pdf;
using System.Data;
using iTextSharp.text;
using Newtonsoft.Json;
using System.ComponentModel;
using Semnox.Parafait.Languages;
using System.Drawing.Imaging;
using Semnox.Parafait.Device.Printer.WristBandPrinters;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// BL class for Printer
    /// </summary>
    public class PrinterBL
    {
        private PrinterDTO printerDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// ProgressUpdates
        /// </summary>
        /// <param name="statusMessage"></param>
        public delegate void ProgressUpdates(string statusMessage);
        /// <summary>
        /// PrintProgressUpdates
        /// </summary>
        public ProgressUpdates PrintProgressUpdates;
        /// <summary>
        /// SetCardPrinterError
        /// </summary>
        /// <param name="errorValue"></param>
        public delegate void SetCardPrinterError(bool errorValue);
        /// <summary>
        /// SetCardPrinterErrorValue
        /// </summary>
        public SetCardPrinterError SetCardPrinterErrorValue;
        /// <summary>
        /// Parameterized constructor of PrinterBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public PrinterBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            printerDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Printer Id as the parameter
        /// Would fetch the Printer object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildren">LoadChildren</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public PrinterBL(ExecutionContext executionContext, int id, bool loadChildren, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildren, sqlTransaction);
            PrinterDataHandler printerDataHandler = new PrinterDataHandler(sqlTransaction);
            printerDTO = printerDataHandler.GetPrinter(id);
            //printerDTO.PrinterLocation = String.IsNullOrEmpty(printerDTO.PrinterLocation) ? printerDTO.PrinterName : printerDTO.PrinterLocation;
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            try
            {
                printerDTO.PrinterType = printerBuildBL.PopulatePrinterType(printerDTO);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executinng PopulatePrinterType() " + ex.Message);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Error fetching Printer Type. Check Printer set up. "));
            }
            if (loadChildren)
                printerDTO.PrintableProductIds = GetPrinterProducts(sqlTransaction);
            printerDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates PrinterBL object using the printerDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="printerDTO">printerDTO object</param>
        public PrinterBL(ExecutionContext executionContext, PrinterDTO printerDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, printerDTO);
            this.printerDTO = printerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Printer
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            PrinterDataHandler printerDataHandler = new PrinterDataHandler(sqlTransaction);
            if (printerDTO.PrinterId < 0)
            {
                printerDTO = printerDataHandler.InsertPrinters(printerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                printerDTO.AcceptChanges();
            }
            else
            {
                if (printerDTO.IsChanged)
                {
                    printerDTO = printerDataHandler.UpdatePrinters(printerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    printerDTO.AcceptChanges();
                }
            }
            if (printerDTO.PrinterDisplayGroups != null && printerDTO.PrinterDisplayGroups.Count != 0)
            {
                foreach (PrinterDisplayGroupDTO printerDisplayGroupDTO in printerDTO.PrinterDisplayGroups)
                {
                    if (printerDisplayGroupDTO.IsChanged)
                    {
                        PrinterDisplayGroupBL printerDisplayGroupBL = new PrinterDisplayGroupBL(executionContext, printerDisplayGroupDTO);
                        printerDisplayGroupBL.Save(sqlTransaction);
                    }
                }
            }
            if (printerDTO.PrintProductsList != null && printerDTO.PrintProductsList.Count != 0)
            {
                foreach (PrinterProductsDTO printerProductsDTO in printerDTO.PrintProductsList)
                {
                    if (printerProductsDTO.IsChanged)
                    {
                        PrinterProductsBL printerProductsBL = new PrinterProductsBL(executionContext, printerProductsDTO);
                        printerProductsBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get list of eligible products for the printer
        /// </summary>
        public List<int> GetPrinterProducts(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<int> lstProductIds = new List<int>();
            //get list of Printer display groups for specific printer object
            PrinterDisplayGroupListBL printerDisplayGroupList = new PrinterDisplayGroupListBL(executionContext);
            List<KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>(PrinterDisplayGroupDTO.SearchByParameters.PRINTER_ID, printerDTO.PrinterId.ToString()));
            searchParams.Add(new KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>(PrinterDisplayGroupDTO.SearchByParameters.ISACTIVE, "Y"));
            List<PrinterDisplayGroupDTO> lstPrinterDisplayGroupDTO = printerDisplayGroupList.GetPrinterDisplayGroupDTOList(searchParams, sqlTransaction);
            log.LogVariableState("List of Printer Display Groups", lstPrinterDisplayGroupDTO);

            //get list of Printer Products for specific printer object
            PrinterProductsListBL printerProductsList = new PrinterProductsListBL(executionContext);
            List<KeyValuePair<PrinterProductsDTO.SearchByParameters, string>> searchProductsParams = new List<KeyValuePair<PrinterProductsDTO.SearchByParameters, string>>();
            searchProductsParams.Add(new KeyValuePair<PrinterProductsDTO.SearchByParameters, string>(PrinterProductsDTO.SearchByParameters.PRINTERID, printerDTO.PrinterId.ToString()));
            searchProductsParams.Add(new KeyValuePair<PrinterProductsDTO.SearchByParameters, string>(PrinterProductsDTO.SearchByParameters.ISACTIVE, "Y"));
            List<PrinterProductsDTO> lstPrinterProductsDTO = printerProductsList.GetPrinterProductsDTOList(searchProductsParams, sqlTransaction);
            if (lstPrinterProductsDTO != null && lstPrinterProductsDTO.Count > 0)
                lstProductIds.AddRange(lstPrinterProductsDTO.Select(x => x.ProductId).ToList()); //get productids into the list
            List<int> printerProductsDisplayGroupIds = new List<int>();
            if (lstPrinterProductsDTO != null)
            {
                foreach (PrinterProductsDTO printerProductsDTO in lstPrinterProductsDTO)
                {
                    ProductsDisplayGroupList productsDisplayGrouplist = new ProductsDisplayGroupList(executionContext);
                    List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> SearchParameters = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                    SearchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, printerProductsDTO.ProductId.ToString()));
                    List<ProductsDisplayGroupDTO> productDisplayGroupList = productsDisplayGrouplist.GetAllProductsDisplayGroup(SearchParameters);
                    if (productDisplayGroupList != null && productDisplayGroupList.Count > 0)
                        printerProductsDisplayGroupIds.AddRange(productDisplayGroupList.Select(x => x.DisplayGroupId).Distinct().ToList());//get distinct list of display group ids
                }
                printerDTO.PrintProductsList = lstPrinterProductsDTO;
            }
            printerProductsDisplayGroupIds = printerProductsDisplayGroupIds.Distinct().ToList(); //final list of distinct display group ids
            log.LogVariableState("Display Group corresponding to Printer Products", printerProductsDisplayGroupIds);
            if (lstPrinterDisplayGroupDTO != null)
            {
                foreach (PrinterDisplayGroupDTO printerDisplayGroupDTO in lstPrinterDisplayGroupDTO)
                {
                    if (!printerProductsDisplayGroupIds.Exists(x => x.Equals(printerDisplayGroupDTO.DisplayGroupId)))
                    {
                        ProductsDisplayGroupList productsDisplayGrouplist = new ProductsDisplayGroupList(executionContext);
                        List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> SearchParameters = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                        SearchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID, printerDisplayGroupDTO.DisplayGroupId.ToString()));
                        List<ProductsDisplayGroupDTO> productDisplayGroupList = productsDisplayGrouplist.GetAllProductsDisplayGroup(SearchParameters);
                        if (productDisplayGroupList != null && productDisplayGroupList.Count > 0)
                            lstProductIds.AddRange(productDisplayGroupList.Select(x => x.ProductId).ToList());//get list of product ids for a display group
                    }
                }
                printerDTO.PrinterDisplayGroups = lstPrinterDisplayGroupDTO;
            }
            lstProductIds.Distinct().ToList();//Finalize unique list of product ids
            if (printerDTO != null)
            {
                printerDTO.PrintableProductIds = lstProductIds;
            }
            log.LogMethodExit(lstProductIds);
            return lstProductIds;
        }

        /// <summary>
        /// Setup Printing Document
        /// </summary>
        /// <param name="MyPrintDocument">MyPrintDocument</param>
        /// <param name="isClientServerApp">isClientServerApp</param>
        /// <param name="documentName">documentName</param>
        /// <returns>true or false</returns>
        public bool SetupThePrinting(PrintDocument MyPrintDocument, bool isClientServerApp, string documentName)
        {
            log.LogMethodEntry(MyPrintDocument, isClientServerApp, documentName);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            if (printerBuildBL.SetUpPrinting(MyPrintDocument, isClientServerApp, documentName, printerDTO))
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Setup Printing Document
        /// </summary>
        /// <param name="MyPrintDocument">MyPrintDocument</param>
        /// <param name="isClientServerApp">isClientServerApp</param>
        /// <param name="documentName">documentName</param>
        /// <returns>true or false</returns>
        public bool SetupThePrinting(PrintDocument MyPrintDocument, string filePath, string documentName)
        {
            log.LogMethodEntry(MyPrintDocument, filePath, documentName);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            if (printerBuildBL.SetUpPrinting(MyPrintDocument, filePath, documentName, printerDTO))
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Perform Paper cut by sending printer command
        /// </summary>
        /// <param name="printerLocation">Printer Location</param>
        public void CutPrinterPaper(string printerLocation)
        {
            log.LogMethodEntry(printerLocation);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            printerBuildBL.CutPrintPaper(printerLocation);
            log.LogMethodExit();
        }

        /// <summary>
        /// Issue command to open cash drawer
        /// </summary>
        public void OpenCashDrawer()
        {
            log.LogMethodEntry();
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            printerBuildBL.OpenCashDrawer();
            log.LogMethodExit();
        }

        /// <summary>
        /// Sending ticket object to Printer
        /// </summary>
        /// <param name="currentTicket">current ticket count</param>
        /// <param name="e">Print event</param>
        /// <param name="PrintTicketArray">Ticket List</param>
        /// <returns>True or false</returns>
        public bool PrintTicketsToPrinter(List<Printer.clsTicket> printTicketArray, int currentTicket, PrintPageEventArgs e)
        {
            log.LogMethodEntry(printTicketArray, currentTicket, e);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            bool status = printerBuildBL.PrintTicketsToPrinter(printTicketArray, currentTicket, e);
            log.LogMethodExit(status);
            return status;
        }

        /// <summary>
        /// Sending ticket object to Printer
        /// </summary>
        /// <param name="currentTicket">current ticket count</param>
        /// <param name="e">Print event</param>
        /// <param name="PrintTicketArray">Ticket List</param>
        /// <returns>True or false</returns>
        public bool PrintTicketsToPrinter(List<Printer.clsTicket> printTicketArray, int currentTicket, Graphics g)
        {
            log.LogMethodEntry(printTicketArray, currentTicket, g);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            bool status = printerBuildBL.PrintTicketsToPrinter(printTicketArray, currentTicket, g);
            log.LogMethodExit(status);
            return status;
        }

        /// <summary>
        /// Printing Card Object to RFID WB or Card Printer
        /// </summary>
        /// <param name="Ticket">Ticket</param>
        /// <returns>card number as string</returns>
        public string PrintCardToPrinter(Printer.clsTicket Ticket, WristBandPrinter wristBandPrinter)
        {
            log.LogMethodEntry(Ticket, "wristBandPrinter");

            if (printerDTO.PrinterType == PrinterDTO.PrinterTypes.CardPrinter)
            {
                PrintDialog pd = new PrintDialog();
                PrinterSettings ps = pd.PrinterSettings;
                ps.PrinterName = String.IsNullOrEmpty(printerDTO.PrinterLocation) ? printerDTO.PrinterName : printerDTO.PrinterLocation;
                Graphics g = ps.CreateMeasurementGraphics();
                IntPtr HDC = g.GetHdc();
                string returnValueNew = PrintCardToMagiCardPrinter(Ticket, HDC);
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            else if (printerDTO.PrinterType == PrinterDTO.PrinterTypes.RFIDWBPrinter)
            {
                string returnValueNew = PrintToRFIDWristBandPrinter(Ticket, wristBandPrinter);
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            else
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Not a card / wristband printer");
                SendPrintProgressUpdates("Not a card / wristband printer");
                SetCardPrinterErrorValue(true);
                throw new ApplicationException("Not a card / wristband printer");
            }
        }

        /// <summary>
        /// Issue print to Card Printer
        /// </summary>
        /// <param name="Ticket">Ticket</param>
        /// <param name="HDC">HDC</param>
        /// <returns>Card number string</returns>
        public string PrintCardToMagiCardPrinter(clsTicket ticket, IntPtr HDC)
        {
            log.LogMethodEntry(ticket, HDC);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            string printedCardNumber = printerBuildBL.printCardToMagiCardPrinter(ticket, HDC);
            log.LogMethodExit(printedCardNumber);
            return printedCardNumber;
        }

        /// <summary>
        /// Issue print to Card Printer.
        /// </summary>
        /// <param name="Ticket">Ticket</param>
        /// <returns>Card number string</returns>
        public string PrintToRFIDWristBandPrinter(clsTicket ticket, WristBandPrinter wristBandPrinter)
        {
            log.LogMethodEntry(ticket, "wristBandPrinter");
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            if (PrintProgressUpdates != null)
            {
                printerBuildBL.PrintProgressUpdates = new PrinterBuildBL.ProgressUpdates(PrintProgressUpdates);
            }
            if (SetCardPrinterErrorValue != null)
            {
                printerBuildBL.SetCardPrinterErrorValue = new PrinterBuildBL.SetCardPrinterError(SetCardPrinterErrorValue);
            }
            string printedCardNumber = printerBuildBL.printToRFIDWristBandPrinter(ticket, printerDTO, wristBandPrinter);
            log.LogMethodExit(printedCardNumber);
            return printedCardNumber;
        }

        /// <summary>
        /// Printing ticket using graphics object
        /// </summary>
        /// <param name="Ticket">cls Ticket class</param>
        /// <param name="graphics">Graphics object</param>
        public void PrintTicketElements(clsTicket Ticket, Graphics graphics)
        {
            log.LogMethodEntry(Ticket, graphics);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            printerBuildBL.printTicketElements(Ticket, graphics);
            log.LogMethodExit(graphics);
        }

        /// <summary>
        /// Barcode image is created and returned
        /// </summary>
        /// <param name="weight">Weight of Barcode</param>
        /// <param name="height">Height of Barcode</param>
        /// <param name="barCodeEncodeType">Barcode type</param>
        /// <param name="barCodeValue">Value of barcode</param>
        /// <returns>Barcode Image</returns>
        public System.Drawing.Image MakeBarcodeLibImage(int weight, int height, string barCodeEncodeType, string barCodeValue)
        {
            log.LogMethodEntry(weight, height, barCodeEncodeType, barCodeValue);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            System.Drawing.Image barcodeImage = printerBuildBL.MakeBarcodeLibImage(weight, height, barCodeEncodeType, barCodeValue);
            log.LogMethodExit(barcodeImage);
            return barcodeImage;
        }

        public string GenerateProductBarcode(int barWeight, string textToEncode, int productId, bool showPrice = true, bool showDescription = true)
        {
            string str1 = string.Empty;
            string productDescription = string.Empty;
            double productPrice = 0;
            ProductBL productBl = new ProductBL(this.executionContext, productId, false, false, (SqlTransaction)null);
            PrinterBL printerBl = new PrinterBL(this.executionContext);
            ProductDTO productDTO = productBl.getProductDTO;
            if (productBl != null)
            {
                productDescription = productBl.getProductDTO.Description;
                productPrice = productBl.getProductDTO.Cost;
            }

            PrinterBL printerBL = new PrinterBL(executionContext);
            System.Drawing.Image myimg = printerBL.MakeBarcodeLibImage(barWeight, 40, BarcodeLib.TYPE.CODE128.ToString(), textToEncode);
            int width = 400;
            int height = 40;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            int yLocation = 0;
            int fontSize = 8;
            if (Convert.ToInt32(barWeight) == 1)
                fontSize = 8;
            else if (Convert.ToInt32(barWeight) == 2)
                fontSize = 9;
            else if (Convert.ToInt32(barWeight) == 3)
                fontSize = 10;
            else if (Convert.ToInt32(barWeight) >= 4)
                fontSize = 12;

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (System.Drawing.Font myFont = new System.Drawing.Font("Arial", fontSize, FontStyle.Regular))
                {
                    Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();
                    string str = productPrice == -1 ? "" : productPrice.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                    SizeF stringSize = new SizeF();
                    stringSize = g.MeasureString(str, myFont);
                    g.DrawString(textToEncode, myFont, Brushes.Black, 0, yLocation);
                    if (showPrice)
                        g.DrawString(str, myFont, Brushes.Black, myimg.Width - stringSize.Width, yLocation);
                    yLocation += Convert.ToInt32(stringSize.Height);
                    g.DrawImage(
                                printerBL.MakeBarcodeLibImage(Convert.ToInt32(barWeight), 40, BarcodeLib.TYPE.CODE128.ToString(), textToEncode),
                                new System.Drawing.Rectangle(0, yLocation, width, height),  // destination rectangle  
                                0,
                                0,           // upper-left corner of source rectangle
                                width,       // width of source rectangle
                                height,      // height of source rectangle
                                GraphicsUnit.Pixel,
                                null);
                    yLocation += myimg.Height;
                    if (showDescription)
                        g.DrawString(productDescription, myFont, Brushes.Black, 0, yLocation);
                }
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bmp.Save((Stream)memoryStream, ImageFormat.Png);
                str1 = Convert.ToBase64String(memoryStream.ToArray());
            }
            return str1;
        }

        /// <summary>
        /// Formatting Receipt Class into Graphics 
        /// object for final printing
        /// </summary>
        /// <param name="transactionReceipt">transactionReceipt</param>
        /// <param name="receiptLineIndex">receiptLineIndex</param>
        /// <param name="e">Print Page event</param>
        /// <returns>True or False</returns>
        public bool PrintReceiptToPrinter(ReceiptClass TransactionReceipt, ref int receiptLineIndex, PrintPageEventArgs e)
        {
            log.LogMethodEntry(TransactionReceipt, receiptLineIndex, e);
            int pageHeight = 0;
            bool returnValue = PrintReceiptToPrinter(TransactionReceipt, ref receiptLineIndex, e.Graphics, e.MarginBounds, ref pageHeight);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Formatting Receipt Class into Graphics 
        /// object for final printing
        /// </summary>
        /// <param name="transactionReceipt">transactionReceipt</param>
        /// <param name="receiptLineIndex">receiptLineIndex</param>
        /// <param name="g">Graphics Object</param>
        /// <param name="marginBounds">marginBounds</param>
        /// <param name="pageHeight">pageHeight</param>
        /// <returns>true or false</returns>
        public bool PrintReceiptToPrinter(ReceiptClass transactionReceipt, ref int receiptLineIndex, Graphics g, System.Drawing.Rectangle marginBounds, ref int pageHeight)
        {
            log.LogMethodEntry(transactionReceipt, receiptLineIndex, g, marginBounds, pageHeight);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            bool returnValue = printerBuildBL.PrintReceiptToPrinter(transactionReceipt, ref receiptLineIndex, g, marginBounds, ref pageHeight);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Formatting Receipt Class into String 
        /// for final printing
        /// </summary>
        /// <param name="TransactionReceipt">TransactionReceipt</param>
        /// <param name="appendText">appendText</param>
        /// <returns>true or false</returns>
        public bool PrintReceiptToText(ReceiptClass TransactionReceipt, ref string appendText)
        {
            log.LogMethodEntry(TransactionReceipt, appendText);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            bool returnValue = printerBuildBL.PrintReceiptToText(TransactionReceipt, ref appendText);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// It will print the receipt
        /// </summary>
        /// <param name="shiftKey"></param>
        /// <param name="posMachine"></param>
        /// <param name="user"></param>
        /// <param name="shiftTime"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public string PrintShiftReceipt(int shiftKey, string posMachine, string user, Core.Utilities.Utilities utilities, DateTime? shiftTime = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(shiftKey, posMachine, user, shiftTime, sqlTransaction);
            PrinterBuildBL printerBuildBL = new PrinterBuildBL(executionContext);
            string absolutePath = printerBuildBL.PrintShiftReceipt(shiftKey, posMachine, user, utilities, sqlTransaction, shiftTime);
            if (absolutePath != null)
            {
                //fetch the file
                byte[] dataBytes = File.ReadAllBytes(absolutePath);
                // convert it to base64 string image
                string imagebase64String = Convert.ToBase64String(dataBytes);
                log.LogMethodExit(imagebase64String);
                return imagebase64String;
            }
            log.LogMethodExit();
            return null;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PrinterDTO PrinterDTO
        {
            get
            {
                return printerDTO;
            }
        }

        private void SendPrintProgressUpdates(string message)
        {
            log.LogMethodEntry(message);
            if (PrintProgressUpdates != null)
            {
                PrintProgressUpdates(message);
            }
            else
            {
                log.Info("PrintProgressUpdates is not defined. Hence no message sent back");
            }
            log.LogMethodExit();
        }

        private void SendCardPrinterErrorValue(bool errorValue)
        {
            log.LogMethodEntry(errorValue);
            if (SetCardPrinterErrorValue != null)
            {
                SetCardPrinterErrorValue(errorValue);
            }
            else
            {
                log.Info("SetCardPrinterErrorValue is not defined. Hence no error value sent back");
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Printers
    /// </summary>
    public class PrinterListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PrinterDTO> printerDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public PrinterListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.printerDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor with printerDTOList and executionContext
        /// </summary>
        /// <param name="printerDTOList"></param>
        /// <param name="executionContext"></param>
        public PrinterListBL(ExecutionContext executionContext, List<PrinterDTO> printerDTOList)
        {
            log.LogMethodEntry(executionContext, printerDTOList);
            this.printerDTOList = printerDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the Printer list
        /// </summary>
        /// <param name="searchParameters">SearchParameters</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <returns>printerDTOList</returns>
        public List<PrinterDTO> GetPrinterDTOList(List<KeyValuePair<PrinterDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PrinterDataHandler printerDataHandler = new PrinterDataHandler(sqlTransaction);
            List<PrinterDTO> printerDTOList = printerDataHandler.GetPrinterList(searchParameters);

            if (printerDTOList != null && printerDTOList.Any() && loadChildRecords)
            {
                PrinterDisplayGroupListBL printerDisplayGroupListBL = new PrinterDisplayGroupListBL(executionContext);
                List<KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>> SearchParams = new List<KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>(PrinterDisplayGroupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
                if (loadActiveChildRecords)
                {
                    SearchParams.Add(new KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>(PrinterDisplayGroupDTO.SearchByParameters.ISACTIVE, "1"));
                }
                List<PrinterDisplayGroupDTO> printerDisplayGroupDTOList = printerDisplayGroupListBL.GetPrinterDisplayGroupDTOList(SearchParams);

                List<KeyValuePair<PrinterProductsDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<PrinterProductsDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<PrinterProductsDTO.SearchByParameters, string>(PrinterProductsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
                if (loadActiveChildRecords)
                {
                    searchParameter.Add(new KeyValuePair<PrinterProductsDTO.SearchByParameters, string>(PrinterProductsDTO.SearchByParameters.ISACTIVE, "1"));
                }
                PrinterProductsListBL printerProductsListBL = new PrinterProductsListBL(executionContext);
                List<PrinterProductsDTO> printerProductsDTOList = printerProductsListBL.GetPrinterProductsDTOList(searchParameter);
                foreach (PrinterDTO printerDTO in printerDTOList)
                {
                    //For Include Display groups i.e Print These Display Groups
                    if (printerDisplayGroupDTOList != null && printerDisplayGroupDTOList.Any())
                    {
                        printerDTO.PrinterDisplayGroups = printerDisplayGroupDTOList.FindAll(m => m.PrinterId == printerDTO.PrinterId).ToList();
                    }
                    if (printerDTO.PrinterDisplayGroups != null && printerDTO.PrinterDisplayGroups.Any())
                    {
                        foreach (PrinterDisplayGroupDTO printerDisplayGroupDTO in printerDTO.PrinterDisplayGroups)
                        {
                            //Populate Print only these  Products                           
                            if (printerProductsDTOList != null && printerProductsDTOList.Any())
                            {
                                printerDTO.PrintProductsList = printerProductsDTOList.FindAll(m => m.PrinterId == printerDTO.PrinterId).ToList();
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(printerDTOList);
            return printerDTOList;
        }

        /// <summary>
        /// This method is used to save and update the printer list for Web Management Studio.
        /// </summary>
        public void SaveUpdatePrinterList()
        {
            log.LogMethodEntry();
            if (printerDTOList != null && printerDTOList.Any())
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (PrinterDTO printerDto in printerDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            PrinterBL printerBL = new PrinterBL(executionContext, printerDto);
                            printerBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                log.LogMethodExit();

            }
        }
    }


    /// <summary>
    /// Common logic within Printer BL
    /// </summary>
    public class PrinterBuildBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<string> paymentModes = new List<string>();
        private List<double> amountList = new List<double>();
        private readonly DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private int waitTillMsec = -1;
        private int sleepGapDuringWait = -1;
        private TagNumberParser tagNumberParser;
        /// <summary>
        /// ProgressUpdates
        /// </summary>
        /// <param name="statusMessage"></param>
        public delegate void ProgressUpdates(string statusMessage);
        /// <summary>
        /// PrintProgressUpdates
        /// </summary>
        public ProgressUpdates PrintProgressUpdates;
        /// <summary>
        /// SetCardPrinterError
        /// </summary>
        /// <param name="errorValue"></param>
        public delegate void SetCardPrinterError(bool errorValue);
        /// <summary>
        /// SetCardPrinterErrorValue
        /// </summary>
        public SetCardPrinterError SetCardPrinterErrorValue;

        /// <summary>
        /// Common logic within Printer BL
        /// <param name="executionContext">ExecutionContext</param>
        /// </summary>
        public PrinterBuildBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.tagNumberParser = new TagNumberParser(this.executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Common logic within Printer BL
        /// </summary>
        /// <param name="printerDTO">PrinterDTO</param>
        /// <returns>Returns DTO</returns>
        public PrinterDTO.PrinterTypes PopulatePrinterType(PrinterDTO printerDTO)
        {
            log.LogMethodEntry(printerDTO);
            LookupValuesDTO lookupValueDTO = (new LookupValues(executionContext, printerDTO.PrinterTypeId)).LookupValuesDTO;
            PrinterDTO.PrinterTypes printerTypes = new PrinterDTO.PrinterTypes();
            printerTypes = (PrinterDTO.PrinterTypes)Enum.Parse(typeof(PrinterDTO.PrinterTypes), lookupValueDTO.LookupValue, true);
            log.LogMethodExit(printerTypes);
            return printerTypes;
        }

        /// <summary>
        /// Common logic within Printer BL
        /// </summary>
        /// <param name="MyPrintDocument">myPrintDocument</param>
        /// <param name="isClientServerApp">IsClientServerApp</param>
        /// <param name="documentName">DocumentName</param>
        /// <param name="printerDTO">PrinterDTO</param>
        /// <returns>true or false</returns>
        public bool SetUpPrinting(PrintDocument MyPrintDocument, bool isClientServerApp, string documentName, PrinterDTO printerDTO)
        {
            log.LogMethodEntry(MyPrintDocument, isClientServerApp, documentName, printerDTO);

            PrintDialog MyPrintDialog = new PrintDialog();
            if (printerDTO.PrinterName != "Default")
                MyPrintDialog.PrinterSettings.PrinterName = String.IsNullOrEmpty(printerDTO.PrinterLocation) ? printerDTO.PrinterName : printerDTO.PrinterLocation;
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = true;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = true;
            MyPrintDialog.UseEXDialog = true;
            bool showPrintDialog = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SHOW_PRINT_DIALOG_IN_POS") == "Y";
            int printerPageLeftMargin = Convert.ToInt32(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PRINTER_PAGE_LEFT_MARGIN"));
            int printerPageRightMargin = Convert.ToInt32(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PRINTER_PAGE_RIGHT_MARGIN"));
            if (showPrintDialog && isClientServerApp && printerDTO.PrinterType != PrinterDTO.PrinterTypes.KOTPrinter)
            {
                if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }

            MyPrintDocument.DocumentName = documentName;
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            DateTime dateNow = DateTime.Now;

            MyPrintDocument.OriginAtMargins = true;
            MyPrintDocument.DefaultPageSettings.Margins = new Margins(printerPageLeftMargin, printerPageRightMargin, 10, 20);

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Common logic within Printer BL
        /// </summary>
        /// <param name="MyPrintDocument">myPrintDocument</param>
        /// <param name="filePath">Directory Path for saving the prn file</param>
        /// <param name="documentName">DocumentName</param>
        /// <param name="printerDTO">PrinterDTO</param>
        /// <returns>true or false</returns>
        public bool SetUpPrinting(PrintDocument MyPrintDocument, string filePath, string documentName, PrinterDTO printerDTO)
        {
            log.LogMethodEntry(MyPrintDocument, filePath, documentName, printerDTO);

            PrintDialog MyPrintDialog = new PrintDialog();
            if (printerDTO.PrinterName != "Default")
                MyPrintDialog.PrinterSettings.PrinterName = String.IsNullOrEmpty(printerDTO.PrinterLocation) ? printerDTO.PrinterName : printerDTO.PrinterLocation;
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = true;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = true;
            MyPrintDialog.PrinterSettings.PrintToFile = true;
            if (filePath.EndsWith("\\"))
                MyPrintDialog.PrinterSettings.PrintFileName = filePath + documentName;
            else
                MyPrintDialog.PrinterSettings.PrintFileName = filePath + "\\" + documentName;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = true;
            MyPrintDialog.UseEXDialog = true;
            //bool showPrintDialog = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SHOW_PRINT_DIALOG_IN_POS") == "Y";
            int printerPageLeftMargin = Convert.ToInt32(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PRINTER_PAGE_LEFT_MARGIN"));
            int printerPageRightMargin = Convert.ToInt32(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PRINTER_PAGE_RIGHT_MARGIN"));
            //if (showPrintDialog && isClientServerApp && printerDTO.PrinterType != PrinterDTO.PrinterTypes.KOTPrinter)
            //{
            //    if (MyPrintDialog.ShowDialog() != DialogResult.OK)
            //    {
            //        log.LogMethodExit(false);
            //        return false;
            //    }
            //}
            if (!System.IO.Directory.Exists(filePath))
            {
                log.LogMethodExit("File path does not exist: " + filePath);
                return false;
            }
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            DateTime dateNow = DateTime.Now;

            MyPrintDocument.OriginAtMargins = true;
            MyPrintDocument.DefaultPageSettings.Margins = new Margins(printerPageLeftMargin, printerPageRightMargin, 10, 20);

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Byte command to Printer to Open Cash Drawer
        /// </summary>
        public void OpenCashDrawer()
        {
            log.LogMethodEntry();
            byte[] bCashDrawerPrintString;
            try
            {
                string[] strCASH_DRAWER_PRINT_STRING = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CASH_DRAWER_PRINT_STRING").Split(',');
                bCashDrawerPrintString = new byte[strCASH_DRAWER_PRINT_STRING.Length];
                int i = 0;
                foreach (string str in strCASH_DRAWER_PRINT_STRING)
                    bCashDrawerPrintString[i++] = Convert.ToByte(Convert.ToInt32(str.Trim()));
            }
            catch (Exception ex)
            {
                log.Error("Unable to get the value of CASH_DRAWER_PRINT_STRING", ex);
                bCashDrawerPrintString = new byte[] { 27, 112, 0, 100, 250 };
            }
            IntPtr pBytes = Marshal.AllocHGlobal(Marshal.SizeOf(bCashDrawerPrintString[0]) * bCashDrawerPrintString.Length);
            try
            {
                Marshal.Copy(bCashDrawerPrintString, 0, pBytes, bCashDrawerPrintString.Length);
                RawPrinterHelper.SendBytesToPrinter(new PrinterSettings().PrinterName, pBytes, bCashDrawerPrintString.Length);
            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pBytes);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Print regular tickets to printer
        /// </summary>
        /// <param name="PrintTicketArray">List of clsTicket</param>
        /// <param name="currentTicket">count of current ticket</param>
        /// <param name="e">Print Page event</param>
        /// <returns>true or false</returns>
        public bool PrintTicketsToPrinter(List<Printer.clsTicket> PrintTicketArray, int currentTicket, PrintPageEventArgs e)
        {
            log.LogMethodEntry(PrintTicketArray, currentTicket, e);
            PrintTicketsToPrinter(PrintTicketArray, currentTicket, e.Graphics);
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Print regular tickets to printer
        /// </summary>
        /// <param name="PrintTicketArray">List of clsTicket</param>
        /// <param name="currentTicket">count of current ticket</param>
        /// <param name="e">Print Page event</param>
        /// <returns>true or false</returns>
        public bool PrintTicketsToPrinter(List<Printer.clsTicket> PrintTicketArray, int currentTicket, Graphics g)
        {
            log.LogMethodEntry(PrintTicketArray, currentTicket, g);

            ReplaceCardNumber(PrintTicketArray[currentTicket], PrintTicketArray[currentTicket].CardNumber);
            printTicketElements(PrintTicketArray[currentTicket], g);

            bool printTicketBorder = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PRINT_TICKET_BORDER") == "Y";
            if (printTicketBorder)
            {
                using (Pen pen = new Pen(Color.Black, PrintTicketArray[0].BorderWidth))
                    g.DrawRectangle(pen, PrintTicketArray[0].TicketBorder);
            }

            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// Printing data to RFID Wristband Printer
        /// </summary>
        /// <param name="Ticket">Ticket object</param>
        /// <param name="printer">Printer</param>
        public string printToRFIDWristBandPrinter(clsTicket Ticket, PrinterDTO printer, WristBandPrinter wristBandPrinter)
        {
            log.LogMethodEntry(Ticket, printer, "wristBandPrinter");
            string cardNumber = string.Empty; 
            cardNumber = GetCardNumberAndPrint(wristBandPrinter, Ticket, printer);
            log.LogMethodExit(cardNumber);
            return cardNumber;
        }

        private string GetCardNumberAndPrint(WristBandPrinter wristBandPrinter, clsTicket ticket, PrinterDTO printer)
        {
            log.LogMethodEntry(wristBandPrinter, ticket, printer);
            string cardNumber = string.Empty;
            if (wristBandPrinter != null)
            { 
                int retryCount = 0;
                const int maxRetries = 2;
                do
                {
                    try
                    {
                        log.Info(retryCount);
                        wristBandPrinter.CanPrint(executionContext);
                        cardNumber = wristBandPrinter.ReadRFIDTag();
                        SendPrintProgressUpdates(MessageContainerList.GetMessage(executionContext, 3005, cardNumber));
                        //"Wrist band number is &1" 
                        retryCount = 6;
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        log.Error("Error reading card number from Wrist band Printer", ex);
                        if (retryCount < maxRetries)
                        {
                            SendPrintProgressUpdates(MessageContainerList.GetMessage(executionContext, 3006));
                            // "Error while reading Wrist band number, retrying..."
                            log.Info("Faced error, try again after 50 msec");
                            System.Threading.Thread.Sleep(50);
                        }
                        else
                        {
                            SendPrintProgressUpdates(MessageContainerList.GetMessage(executionContext, 3007));
                            // "Unable to read Wrist band from Stima printer"
                            SendCardPrinterErrorValue(true);
                            throw;
                        }
                    }
                }
                while (retryCount < maxRetries);

                PrintDocument printDoc = new PrintDocument();
                try
                {
                    if (string.IsNullOrEmpty(cardNumber))
                    {
                        log.LogMethodEntry(null, MessageContainerList.GetMessage(executionContext, 3007));
                        SendCardPrinterErrorValue(true);
                        throw new ApplicationException(MessageContainerList.GetMessage(executionContext, 3007));
                    }
                    Semnox.Parafait.Customer.Accounts.AccountBL CheckCard = new Customer.Accounts.AccountBL(executionContext, cardNumber, false, false);
                    if (CheckCard.AccountDTO != null && CheckCard.GetAccountId() != -1)
                    {
                        string errorMsg = MessageContainerList.GetMessage(executionContext, 3008, cardNumber);
                        //"Wrist band# &1 on RFID printer is already issued. Please contact staff"
                        log.LogMethodEntry(null, errorMsg);
                        SendPrintProgressUpdates(errorMsg);
                        SendCardPrinterErrorValue(true);
                        throw new ApplicationException(errorMsg);
                    }

                    ReplaceCardNumber(ticket, cardNumber);
                    SendPrintProgressUpdates(MessageContainerList.GetMessage(executionContext, 3009, cardNumber));
                    log.Debug("Prepare Ticket For Printing ..");
                    PrepareTicketForPrinting(printDoc, printer);
                    log.Debug("Prepare Ticket For Printing completed");
                    printDoc.DefaultPageSettings.Margins = ticket.MarginProperty;
                    printDoc.PrintPage += (object sender, PrintPageEventArgs e) =>
                    {
                        printTicketElements(ticket, e.Graphics);
                    };
                    printDoc.Print();
                    log.Debug("PrintFileName :  " + printDoc.PrinterSettings.PrintFileName);
                    if (string.IsNullOrWhiteSpace(printDoc.PrinterSettings.PrintFileName) == false)
                    {
                        log.Debug("Send Ticket For Printing ");
                        wristBandPrinter.Print(File.ReadAllBytes(printDoc.PrinterSettings.PrintFileName));
                        log.Debug("Printing completed ");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    SendPrintProgressUpdates(ex.Message);
                    SendCardPrinterErrorValue(true);
                    throw;
                }
                finally
                {
                    try
                    {
                        log.Debug("deleting file ");
                        if (string.IsNullOrWhiteSpace(printDoc.PrinterSettings.PrintFileName) == false)
                        {
                            System.IO.File.Delete(printDoc.PrinterSettings.PrintFileName);
                            log.Debug(" file  deleted");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug(" Error while deleting file ");
                        log.Error(ex);
                    }
                }
                //}
                //finally
                //{
                //if (wristBandPrinter != null)
                //{
                //    try { wristBandPrinter.Close(); }
                //    catch (Exception ex)
                //    {
                //        log.Error("wristBandPrinter.Close()", ex);
                //    };
                //}
                //}
            }
            else
            {
                log.Error("Unable to initialize the Wristband printer");
            }
            log.LogMethodExit(cardNumber);
            return cardNumber;
        }

        /// <summary>
        /// PrepareTicketForPrinting to create PRN file
        /// </summary>
        /// <param name="printDoc"></param>
        /// <param name="printer"></param>
        /// <returns></returns>
        private bool PrepareTicketForPrinting(PrintDocument printDoc, PrinterDTO printer)
        {
            log.LogMethodEntry(printer);
            string filePath = System.IO.Path.GetTempFileName();
            if (SetUpPrinting(printDoc, filePath, printer))
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// SetUpPrinting  for ticket print 
        /// </summary>
        /// <param name="MyPrintDocument"></param>
        /// <param name="filePath"></param>
        /// <param name="printerDTO"></param>
        /// <returns></returns>
        public bool SetUpPrinting(PrintDocument MyPrintDocument, string filePath, PrinterDTO printerDTO)
        {
            log.LogMethodEntry(MyPrintDocument, filePath, printerDTO);

            PrintDialog MyPrintDialog = new PrintDialog();
            if (printerDTO.PrinterName != "Default")
                MyPrintDialog.PrinterSettings.PrinterName = String.IsNullOrEmpty(printerDTO.PrinterLocation) ? printerDTO.PrinterName : printerDTO.PrinterLocation;
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = true;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = true;
            MyPrintDialog.PrinterSettings.PrintToFile = true;
            MyPrintDialog.PrinterSettings.PrintFileName = filePath;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = true;
            MyPrintDialog.UseEXDialog = true;
            int printerPageLeftMargin = Convert.ToInt32(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PRINTER_PAGE_LEFT_MARGIN"));
            int printerPageRightMargin = Convert.ToInt32(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PRINTER_PAGE_RIGHT_MARGIN"));
            if (!System.IO.File.Exists(filePath))
            {
                log.LogMethodExit("File path does not exist: " + filePath);
                return false;
            }
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            DateTime dateNow = DateTime.Now;
            MyPrintDocument.OriginAtMargins = true;
            MyPrintDocument.DefaultPageSettings.Margins = new Margins(printerPageLeftMargin, printerPageRightMargin, 10, 20);
            log.LogMethodExit(true);
            return true;
        }


        private void SetWaitTile()
        {
            log.LogMethodEntry();
            if (waitTillMsec == -1)
            {
                try
                {
                    waitTillMsec = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "RFID_PRINTER_SLEEP_TIME", 1500);
                }
                catch { waitTillMsec = 1500; }
                try
                {
                    sleepGapDuringWait = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "RFID_PRINTER_SLEEP_WAIT_GAP", 300);
                }
                catch { sleepGapDuringWait = 300; }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to check for Print status on the Printer
        /// </summary>
        /// <param name="printServerName">printServerName</param>
        /// <param name="printerName">printerName</param>
        /// <param name="timeOut">timeOut</param>
        void WaitForPrintComplete(string printServerName, string printerName, int timeOut)
        {
            log.LogMethodEntry(printServerName, printerName, timeOut);
            System.Printing.PrintServer myPrintServer;
            System.Printing.PrintQueue myPrintQueue;
            bool printDone = false;

            try
            {
                using (myPrintServer = new System.Printing.PrintServer(@"\\" + printServerName))
                {
                    using (myPrintQueue = myPrintServer.GetPrintQueue(printerName))
                    {
                        DateTime waitTill = DateTime.Now.AddMilliseconds(timeOut);
                        while (!printDone && DateTime.Now < waitTill)
                        {
                            myPrintQueue.Refresh();
                            System.Printing.PrintJobInfoCollection pCollection = myPrintQueue.GetPrintJobInfoCollection();
                            if (pCollection.Count() == 0)
                            {
                                printDone = true;
                                break;
                            }
                            System.Threading.Thread.Sleep(100);
                            Application.DoEvents();
                        }

                        if (!printDone)
                        {
                            myPrintQueue.Refresh();
                            System.Printing.PrintJobInfoCollection pCollection1 = myPrintQueue.GetPrintJobInfoCollection();
                            foreach (System.Printing.PrintSystemJobInfo job in pCollection1)
                            {
                                job.Cancel();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to get print queue details for " + printServerName + "/" + printerName + ": " + ex.Message);
                throw new ApplicationException("Unable to get print queue details for " + printServerName + "/" + printerName + ": " + ex.Message);
            }
            if (!printDone)
            {
                log.Error("Unable to print wristband. Timeout occurred. Please retry...");
                throw new ApplicationException("Unable to print wristband. Timeout occurred. Please retry...");
            }
        }

        /// <summary>
        /// Printing Card template to MagiCard printer
        /// </summary>
        /// <param name="HDC">Pointer to Graphics device</param>
        /// <param name="Ticket">Ticket Class</param>
        public string printCardToMagiCardPrinter(clsTicket Ticket, IntPtr HDC)
        {
            log.LogMethodEntry(Ticket, HDC);

            Semnox.Parafait.Device.Printer.MagiCard magiCardPrinter = new Semnox.Parafait.Device.Printer.MagiCard(HDC);
            magiCardPrinter.OpenSession();
            try
            {
                magiCardPrinter.FeedCard();

                DeviceClass rfidReader = new Semnox.Parafait.Device.OmniKey();
                string cardNumberReceived = rfidReader.readCardNumber();
                if (string.IsNullOrEmpty(cardNumberReceived))
                {
                    magiCardPrinter.EjectCard();
                    log.LogMethodExit("Throwing ApplicationException - Unable to read card in printer");
                    throw new ApplicationException("Unable to read card in printer");
                }

                TagNumber tagNumber;
                if (this.tagNumberParser.TryParse(cardNumberReceived, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(cardNumberReceived);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    throw new ValidationException("Invalid Tag Number. " + message);
                }
                string cardNumber = tagNumber.Value;
                ReplaceCardNumber(Ticket, cardNumber);

                IntPtr frontCanvasHDC = magiCardPrinter.InitializeCanvas();
                Graphics gFront = Graphics.FromHdc(frontCanvasHDC);
                printTicketElements(Ticket, gFront);

                if (Ticket.Backside != null)
                {
                    IntPtr backCanvasHDC = magiCardPrinter.InitializeCanvas(true);
                    Graphics gBack = Graphics.FromHdc(backCanvasHDC);
                    printTicketElements(Ticket.Backside, gBack);
                }

                magiCardPrinter.PrintCard();
                magiCardPrinter.EjectCard();
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }
            finally
            {
                magiCardPrinter.CloseSession();
            }
        }


        /// <summary>
        /// Common logic within Printer BL
        /// </summary>
        /// <param name="printerLocation">Printer Location</param>
        public void CutPrintPaper(string printerLocation)
        {
            log.LogMethodEntry(printerLocation);
            byte[] bCutPaperPrintCommand;
            try
            {
                string[] strCUT_PAPER_PRINTER_COMMAND = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUT_PAPER_PRINTER_COMMAND").Split(',');
                bCutPaperPrintCommand = new byte[strCUT_PAPER_PRINTER_COMMAND.Length];
                int i = 0;
                foreach (string str in strCUT_PAPER_PRINTER_COMMAND)
                    bCutPaperPrintCommand[i++] = Convert.ToByte(Convert.ToInt32(str.Trim()));
            }
            catch (Exception ex)
            {
                log.Error("Unable to get the value of CUT_PAPER_PRINTER_COMMAND", ex);
                bCutPaperPrintCommand = new byte[] { 29, 86, 1 };
            }

            IntPtr pBytes = Marshal.AllocHGlobal(Marshal.SizeOf(bCutPaperPrintCommand[0]) * bCutPaperPrintCommand.Length);
            try
            {
                Marshal.Copy(bCutPaperPrintCommand, 0, pBytes, bCutPaperPrintCommand.Length);
                RawPrinterHelper.SendBytesToPrinter(printerLocation, pBytes, bCutPaperPrintCommand.Length);
            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(pBytes);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Printing ticket using graphics object
        /// </summary>
        /// <param name="Ticket">cls Ticket class</param>
        /// <param name="graphics">Graphics object</param>
        public void printTicketElements(clsTicket Ticket, Graphics graphics)
        {
            log.LogMethodEntry(Ticket, graphics);

            if (Ticket.BackgroundImage != null)
            {
                Bitmap img = new Bitmap(Ticket.BackgroundImage);

                float imgWidth;
                float imgHeight;
                float locationX, locationY;

                imgWidth = Math.Min(img.Width, Ticket.PaperSize.Width);
                imgHeight = imgWidth * img.Height / img.Width;

                locationX = 0;
                locationY = 0;

                graphics.DrawImage(img, locationX, locationY, imgWidth, imgHeight);
            }

            List<clsTicket.PrintObject> printObjectList = Ticket.PrintObjectList;
            foreach (clsTicket.PrintObject prn in printObjectList)
            {
                if (prn.Image != null)
                {
                    Bitmap img = new Bitmap(prn.Image);

                    float imgWidth;
                    float imgHeight;
                    float locationX, locationY;

                    if (prn.Rotate.Equals('C') || prn.Rotate.Equals('A'))
                    {
                        //rotate the picture by 90 degrees and re-save the picture as a Jpeg
                        if (prn.Rotate.Equals('C'))
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        else
                            img.RotateFlip(RotateFlipType.Rotate90FlipXY);

                        imgWidth = Math.Min(img.Width, prn.Width);
                        imgHeight = imgWidth * img.Height / img.Width;

                        locationX = prn.Location.X;
                        locationY = prn.Location.Y;
                    }
                    else
                    {
                        imgWidth = Math.Min(img.Width, prn.Width);
                        imgHeight = imgWidth * img.Height / img.Width;

                        if (prn.Alignment.Equals('C'))
                        {
                            locationX = prn.Location.X + (prn.Width - imgWidth) / 2;
                            locationY = prn.Location.Y;
                        }
                        else if (prn.Alignment.Equals('R'))
                        {
                            locationX = prn.Location.X + (prn.Width - imgWidth);
                            locationY = prn.Location.Y;
                        }
                        else
                        {
                            locationX = prn.Location.X;
                            locationY = prn.Location.Y;
                        }
                    }

                    graphics.DrawImage(img, locationX, locationY, imgWidth, imgHeight);
                }
                else if (prn.BarCode != null)
                {
                    Bitmap img = new Bitmap(prn.BarCode);

                    float imgWidth;
                    float imgHeight;
                    float locationX, locationY;

                    if (prn.Rotate.Equals('C') || prn.Rotate.Equals('A'))
                    {
                        //rotate the picture by 90 degrees and re-save the picture as a Jpeg
                        if (prn.Rotate.Equals('C'))
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        else
                            img.RotateFlip(RotateFlipType.Rotate90FlipXY);

                        imgWidth = (float)img.Width;
                        imgHeight = img.Height;

                        if (prn.Font.Size < 12)
                            imgWidth = imgWidth * 2;
                        else if (prn.Font.Size < 16)
                            imgWidth = (float)(imgWidth * 1.25);

                        locationX = prn.Location.X;
                        locationY = prn.Location.Y;
                    }
                    else
                    {
                        imgWidth = (float)img.Width;
                        imgHeight = img.Height;

                        if (prn.Font.Size < 12)
                            imgHeight = imgHeight * 2;
                        else if (prn.Font.Size < 16)
                            imgHeight = (float)(imgHeight * 1.25);

                        if (prn.Alignment.Equals('C'))
                        {
                            locationX = prn.Location.X + (prn.Width - imgWidth) / 2;
                            locationY = prn.Location.Y;
                        }
                        else if (prn.Alignment.Equals('R'))
                        {
                            locationX = prn.Location.X + (prn.Width - imgWidth);
                            locationY = prn.Location.Y;
                        }
                        else
                        {
                            locationX = prn.Location.X;
                            locationY = prn.Location.Y;
                        }
                    }

                    graphics.DrawImage(img, locationX, locationY, imgWidth, imgHeight);
                }

                if (!string.IsNullOrEmpty(prn.Text))
                {
                    int width, height;
                    StringFormat sf = new StringFormat();
                    sf.FormatFlags = StringFormatFlags.NoWrap;
                    if (prn.Rotate.Equals('C') || prn.Rotate.Equals('A'))
                    {
                        if (prn.Rotate.Equals('A'))
                        {
                            width = prn.Width;
                            height = 100;

                            System.Drawing.Drawing2D.Matrix matrix = graphics.Transform;
                            matrix.RotateAt(-180, new PointF(prn.Location.X + width / 2, prn.Location.Y));
                            matrix.RotateAt(90, new PointF(prn.Location.X, prn.Location.Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                            graphics.Transform = matrix;
                        }
                        else
                        {
                            sf.FormatFlags |= StringFormatFlags.DirectionVertical;
                            width = 100;
                            height = prn.Width;
                        }
                    }
                    else
                    {
                        width = prn.Width;
                        height = 100;
                    }

                    if (prn.Alignment.Equals('C'))
                        sf.Alignment = StringAlignment.Center;
                    else if (prn.Alignment.Equals('R'))
                        sf.Alignment = StringAlignment.Far;
                    else
                        sf.Alignment = StringAlignment.Near;

                    Color brushColor = Color.Black;
                    if (!string.IsNullOrEmpty(prn.Color))
                    {
                        Color color = getColor(prn.Color);
                        if (color != Color.FromArgb(0))
                            brushColor = color;
                    }

                    using (Brush brush = new SolidBrush(brushColor))
                    {
                        graphics.DrawString(prn.Text, prn.Font, brush, new System.Drawing.Rectangle(prn.Location.X, prn.Location.Y, width, height), sf);
                    }

                    if (prn.Rotate.Equals('A'))
                        graphics.ResetTransform();
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// GetColor - Convert string value to actual color object
        /// </summary>
        /// <param name="inColor">color value</param>
        /// <returns>Returns Color</returns>
        Color getColor(string inColor)
        {
            log.LogMethodEntry(inColor);

            try
            {
                Color color;
                if (inColor.Contains(",")) // RGB
                {
                    int p = inColor.IndexOf(',');
                    int r = Convert.ToInt32(inColor.Substring(0, p));
                    inColor = inColor.Substring(p + 1).Trim();

                    p = inColor.IndexOf(',');
                    int g = Convert.ToInt32(inColor.Substring(0, p));
                    inColor = inColor.Substring(p + 1).Trim();

                    int b = Convert.ToInt32(inColor);

                    color = Color.FromArgb(r, g, b);
                }
                else
                {
                    color = Color.FromName(inColor);
                    if (color.ToArgb() == 0)
                        color = Color.FromArgb(Int32.Parse(inColor, System.Globalization.NumberStyles.HexNumber));

                    if (color.ToArgb() == 0)
                    {
                        log.LogMethodExit(Color.FromArgb(0));
                        return Color.FromArgb(0);
                    }
                }

                log.LogMethodExit(color);
                return color;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while calculating the color", ex);
                log.LogMethodExit(Color.FromArgb(0));
                return Color.FromArgb(0);
            }
        }

        /// <summary>
        /// Replace Card Number in cls ticket class
        /// </summary>
        /// <param name="Ticket">cls class</param>
        /// <param name="CardNumber">Card Number from device</param>
        public void ReplaceCardNumber(clsTicket Ticket, string CardNumber)
        {
            log.LogMethodEntry(Ticket, CardNumber);

            foreach (clsTicket.PrintObject printObject in Ticket.PrintObjectList)
            {
                if (printObject.Text.Contains("@CardNumber"))
                    printObject.Text = printObject.Text.Replace("@CardNumber", string.IsNullOrEmpty(CardNumber) ? "" : CardNumber);

                if (printObject.Text.Contains("@BarCodeCardNumber"))
                {
                    printObject.Text = printObject.Text.Replace("@BarCodeCardNumber", "");
                    if (!string.IsNullOrEmpty(CardNumber))
                    {
                        int weight = 1;
                        if (printObject.Font.Size >= 16)
                            weight = 3;
                        else if (printObject.Font.Size >= 12)
                            weight = 2;
                        printObject.BarCode = MakeBarcodeLibImage(weight, printObject.BarCodeHeightProperty, printObject.BarCodeEncodeType, CardNumber);
                    }
                }

                if (printObject.Text.Contains("@QRCodeCardNumber"))
                {
                    printObject.Text = printObject.Text.Replace("@QRCodeCardNumber", "");
                    if (string.IsNullOrEmpty(CardNumber) == false)
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(CardNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        if (qrCode != null)
                        {
                            int pixelPerModule = 1;
                            if (printObject.Font.Size > 10)
                            {
                                pixelPerModule = Convert.ToInt32(printObject.Font.Size / 10);
                            }
                            printObject.BarCode = qrCode.GetGraphic(pixelPerModule);
                        }
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Bar code image is created and returned
        /// </summary>
        /// <param name="weight">Weight of Bar code</param>
        /// <param name="height">Height of Bar code</param>
        /// <param name="barCodeEncodeType">Bar code type</param>
        /// <param name="barCodeValue">Value of bar code</param>
        /// <returns>Bar code Image</returns>
        public System.Drawing.Image MakeBarcodeLibImage(int weight, int height, string barCodeEncodeType, string barCodeValue)
        {
            log.LogMethodEntry(weight, height, barCodeEncodeType, barCodeValue);
            BarcodeLib.Barcode barCodeObj = new BarcodeLib.Barcode();
            barCodeObj.BarWidth = weight;
            if (height > 0)
                barCodeObj.Height = height;
            else
                barCodeObj.Height = 24;
            if (!String.IsNullOrEmpty(barCodeEncodeType))
            {
                try { barCodeObj.EncodedType = (BarcodeLib.TYPE)Enum.Parse(typeof(BarcodeLib.TYPE), barCodeEncodeType); }
                catch { barCodeObj.EncodedType = BarcodeLib.TYPE.CODE128; }
            }
            else
                barCodeObj.EncodedType = BarcodeLib.TYPE.CODE128;
            System.Drawing.Image returnValue = barCodeObj.Encode(barCodeObj.EncodedType, barCodeValue);
            log.LogMethodExit(returnValue);
            return returnValue;

        }

        /// <summary>
        /// PrintReceiptToPrinter
        /// </summary>
        /// <param name="TransactionReceipt">Receipt Class - Transaction</param>
        /// <param name="receiptLineIndex">receiptLineIndex</param>
        /// <param name="g">Graphics Object</param>
        /// <param name="marginBounds">marginBounds</param>
        /// <param name="pageHeight">pageHeight</param>
        /// <returns>True or false</returns>
        public bool PrintReceiptToPrinter(ReceiptClass TransactionReceipt, ref int receiptLineIndex, Graphics g, System.Drawing.Rectangle marginBounds, ref int pageHeight)
        {
            log.LogMethodEntry(TransactionReceipt, receiptLineIndex, g, marginBounds, pageHeight);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.FormatFlags = StringFormatFlags.NoClip;
            int receiptWidth = marginBounds.Width;
            int xPos = 0;
            int lineHeight = 20;
            int heightOnPage = 0;
            int totalLines = TransactionReceipt.TotalLines;
            float[] colWidth = new float[5];
            int receiptLineHeight = 0;
            while (receiptLineIndex < totalLines)
            {
                if (TransactionReceipt.ReceiptLines[receiptLineIndex].LineHeight > 0)
                    receiptLineHeight = TransactionReceipt.ReceiptLines[receiptLineIndex].LineHeight;
                switch (TransactionReceipt.ReceiptLines[receiptLineIndex].colCount)
                {
                    case 1: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 1.0F; break;
                    case 2: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.5F; break;
                    case 3: colWidth[0] = receiptWidth * 0.5F; colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.25F; break;
                    case 4:
                        colWidth[0] = receiptWidth * .45F;
                        colWidth[1] = receiptWidth * .15F;
                        colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
                    case 5:
                        {
                            colWidth[0] = receiptWidth * .40F;
                            colWidth[1] = receiptWidth * .15F;
                            colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.15F; break;
                        }
                    default: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
                }

                float cumWidth = 0;
                lineHeight = (receiptLineHeight > 0) ? receiptLineHeight : 20;
                for (int j = 0; j < 5; j++)
                {
                    if (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                        continue;
                    lineHeight = Math.Max(lineHeight, (int)Math.Ceiling(g.MeasureString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], TransactionReceipt.ReceiptLines[receiptLineIndex].LineFont, (int)colWidth[j]).Height));
                }

                if (TransactionReceipt.ReceiptLines[receiptLineIndex].BarCode != null)
                {
                    Bitmap img = new Bitmap(TransactionReceipt.ReceiptLines[receiptLineIndex].BarCode);
                    if (TransactionReceipt.ReceiptLines[receiptLineIndex].BarCode.Tag != null
                        && TransactionReceipt.ReceiptLines[receiptLineIndex].BarCode.Tag.ToString() == "QRCode")
                    {
                        g.DrawImage(img, Math.Max(0, (receiptWidth - img.Width)) / 2, heightOnPage, Math.Min(img.Width, receiptWidth), img.Height);
                        heightOnPage += img.Height;
                    }
                    else
                    {
                        g.DrawImage(img, Math.Max(0, (receiptWidth - img.Width)) / 2, heightOnPage, Math.Min(img.Width, receiptWidth), img.Height * 2);
                        heightOnPage += img.Height * 2;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        if (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                            continue;
                        using (System.Drawing.Font f = new System.Drawing.Font(TransactionReceipt.ReceiptLines[receiptLineIndex].LineFont.FontFamily, 8))
                        {
                            g.DrawString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], f, Brushes.Black, (int)(receiptWidth - g.MeasureString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], f).Width) / 2, heightOnPage);
                            heightOnPage += (int)g.MeasureString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], f).Height;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                            continue;
                        switch (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j])
                        {
                            case "L": stringFormat.Alignment = StringAlignment.Near; break;
                            case "R": stringFormat.Alignment = StringAlignment.Far; break;
                            case "C": stringFormat.Alignment = StringAlignment.Center; break;
                            default: stringFormat.Alignment = StringAlignment.Near; break;
                        }
                        try
                        {
                            if (TransactionReceipt.ReceiptLines[receiptLineIndex + 1].Data[j].StartsWith("--")) // heading
                                stringFormat.FormatFlags = StringFormatFlags.NoClip;
                            else if (TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j].StartsWith("--")) // -- below heading
                                stringFormat.FormatFlags = StringFormatFlags.NoWrap;
                            else
                                stringFormat.FormatFlags = StringFormatFlags.NoClip;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while calculating the format flags", ex);
                        }

                        g.DrawString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], TransactionReceipt.ReceiptLines[receiptLineIndex].LineFont, Brushes.Black, new System.Drawing.Rectangle((int)(xPos + cumWidth), heightOnPage, (int)(colWidth[j]), lineHeight), stringFormat);
                        cumWidth += colWidth[j];
                    }
                }

                receiptLineIndex++;
                heightOnPage += lineHeight;
                if (receiptLineIndex < totalLines) // at least 1 more line to be printed. check if next line can be printed on same page
                {
                    lineHeight = (receiptLineHeight > 0) ? receiptLineHeight : 20;
                    for (int j = 0; j < 5; j++)
                    {
                        if (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                            continue;
                        lineHeight = Math.Max(lineHeight, (int)Math.Ceiling(g.MeasureString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], TransactionReceipt.ReceiptLines[receiptLineIndex].LineFont, (int)colWidth[j]).Height));
                    }
                    if (heightOnPage + lineHeight > marginBounds.Height)
                        break;
                }
            }

            pageHeight = heightOnPage;

            if (receiptLineIndex < totalLines)
            {
                log.LogVariableState("receiptLineIndex ,", receiptLineIndex);
                log.LogVariableState("pageHeight ,", pageHeight);
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogVariableState("receiptLineIndex ,", receiptLineIndex);
                log.LogVariableState("pageHeight ,", pageHeight);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Print Receipt to Text. Primarily for e-Journal process
        /// </summary>
        /// <param name="TransactionReceipt">Receipt Class</param>
        /// <param name="appendText">String Text which will have formatted value</param>
        /// <returns>true or false</returns>
        public bool PrintReceiptToText(ReceiptClass TransactionReceipt, ref string appendText)
        {
            log.LogMethodEntry(TransactionReceipt, appendText);
            try
            {
                int receiptWidth = 60;
                int receiptLineIndex = 0;
                int totalLines = TransactionReceipt.TotalLines;
                float[] colWidth = new float[5];
                StringBuilder sbText = new StringBuilder();
                sbText.AppendLine();
                while (receiptLineIndex < totalLines)
                {
                    switch (TransactionReceipt.ReceiptLines[receiptLineIndex].colCount)
                    {
                        case 1: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 1.0F; break;
                        case 2: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.5F; break;
                        case 3: colWidth[0] = receiptWidth * 0.5F; colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.25F; break;
                        case 4:
                            colWidth[0] = receiptWidth * .45F;
                            colWidth[1] = receiptWidth * .15F;
                            colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
                        case 5:
                            {
                                colWidth[0] = receiptWidth * .40F;
                                colWidth[1] = receiptWidth * .15F;
                                colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.15F; break;
                            }
                        default: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
                    }

                    if (TransactionReceipt.ReceiptLines[receiptLineIndex].BarCode == null)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                                continue;
                            switch (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j])
                            {
                                case "L":
                                    {
                                        sbText.AppendFormat("{0, -" + Convert.ToInt32(colWidth[j]) + "}", TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j]);
                                        break;
                                    }
                                case "R":
                                    {
                                        //if (TransactionReceipt.ReceiptLines[receiptLineIndex].TemplateSection == "DISCOUNTS")
                                        //    sbText.AppendFormat("{0, " + Convert.ToInt32(colWidth[j]) + "}", TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j]);
                                        //else
                                        sbText.AppendFormat("{0, " + Convert.ToInt32(colWidth[j]) + "}", TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j]);
                                        break;
                                    }
                                case "C":
                                    {
                                        sbText.AppendFormat("{0, " + Convert.ToInt32(colWidth[j] / 2 - TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j].Length / 2) + "}{1, -" + Convert.ToInt32(colWidth[j] / 2) + "}", " ", TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j]);
                                        break;
                                    }
                                default:
                                    {
                                        sbText.AppendFormat("{0, -" + Convert.ToInt32(colWidth[j]) + "}", TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j]);
                                        break;
                                    }
                            }
                        }
                        if ((TransactionReceipt.ReceiptLines[receiptLineIndex].TemplateSection == "PRODUCT"
                              && TransactionReceipt.ReceiptLines[receiptLineIndex + 1].TemplateSection != "PRODUCT"
                            )
                            ||
                            (TransactionReceipt.ReceiptLines[receiptLineIndex].TemplateSection == "HEADER"
                                 && TransactionReceipt.ReceiptLines[receiptLineIndex + 1].TemplateSection != "HEADER"
                            )
                            ||
                            (TransactionReceipt.ReceiptLines[receiptLineIndex].TemplateSection == "PRODUCTSUMMARY"
                                 && TransactionReceipt.ReceiptLines[receiptLineIndex + 1].TemplateSection != "PRODUCTSUMMARY"
                            )
                           )
                            sbText.AppendLine();
                    }
                    sbText.AppendLine();
                    receiptLineIndex++;
                }
                sbText.AppendLine(new string('-', receiptWidth));
                appendText = sbText.ToString();
                log.LogVariableState("Receipt Text", appendText);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
                log.LogMethodExit(false);
                return false;
            }
        }
        public System.Drawing.Image PrintTicketsToPrinter(List<Printer.clsTicket> PrintTicketArray, int currentTicket, int width, int height)
        {
            log.LogMethodEntry(PrintTicketArray, currentTicket, width, height);
            System.Drawing.Image image = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(image);
            ReplaceCardNumber(PrintTicketArray[currentTicket], PrintTicketArray[currentTicket].CardNumber);
            printTicketElements(PrintTicketArray[currentTicket], g);

            bool printTicketBorder = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PRINT_TICKET_BORDER") == "Y";
            if (printTicketBorder)
            {
                using (Pen pen = new Pen(Color.Black, PrintTicketArray[0].BorderWidth))
                    g.DrawRectangle(pen, PrintTicketArray[0].TicketBorder);
            }

            log.LogMethodExit(true);
            return image;
        }

        public System.Drawing.Image ReceiptImage(ReceiptClass transactionReceipt, int receiptLineIndex, System.Drawing.Rectangle marginBounds, int pageHeight)
        {
            log.LogMethodEntry(transactionReceipt, receiptLineIndex, marginBounds, pageHeight);
            System.Drawing.Image image = new Bitmap(300, 1000);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.FormatFlags = StringFormatFlags.NoClip;
            int receiptWidth = marginBounds.Width;
            int xPos = 0;
            int lineHeight = 20;
            int heightOnPage = 0;
            int totalLines = transactionReceipt.TotalLines;
            float[] colWidth = new float[5];
            int receiptLineHeight = 0;
            while (receiptLineIndex < totalLines)
            {
                if (transactionReceipt.ReceiptLines[receiptLineIndex].LineHeight > 0)
                    receiptLineHeight = transactionReceipt.ReceiptLines[receiptLineIndex].LineHeight;
                switch (transactionReceipt.ReceiptLines[receiptLineIndex].colCount)
                {
                    case 1: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 1.0F; break;
                    case 2: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.5F; break;
                    case 3: colWidth[0] = receiptWidth * 0.5F; colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.25F; break;
                    case 4:
                        colWidth[0] = receiptWidth * .45F;
                        colWidth[1] = receiptWidth * .15F;
                        colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
                    case 5:
                        {
                            colWidth[0] = receiptWidth * .40F;
                            colWidth[1] = receiptWidth * .15F;
                            colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.15F; break;
                        }
                    default: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
                }

                float cumWidth = 0;
                lineHeight = (receiptLineHeight > 0) ? receiptLineHeight : 20;
                for (int j = 0; j < 5; j++)
                {
                    if (transactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || transactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                        continue;
                    lineHeight = Math.Max(lineHeight, (int)Math.Ceiling(g.MeasureString(transactionReceipt.ReceiptLines[receiptLineIndex].Data[j], transactionReceipt.ReceiptLines[receiptLineIndex].LineFont, (int)colWidth[j]).Height));
                }

                if (transactionReceipt.ReceiptLines[receiptLineIndex].BarCode != null)
                {
                    Bitmap img = new Bitmap(transactionReceipt.ReceiptLines[receiptLineIndex].BarCode);
                    g.DrawImage(img, Math.Max(0, (receiptWidth - img.Width)) / 2, heightOnPage, Math.Min(img.Width, receiptWidth), img.Height * 2);
                    heightOnPage += img.Height * 2;
                    for (int j = 0; j < 5; j++)
                    {
                        if (transactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || transactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                            continue;
                        using (System.Drawing.Font f = new System.Drawing.Font(transactionReceipt.ReceiptLines[receiptLineIndex].LineFont.FontFamily, 8))
                        {
                            g.DrawString(transactionReceipt.ReceiptLines[receiptLineIndex].Data[j], f, Brushes.Black, (int)(receiptWidth - g.MeasureString(transactionReceipt.ReceiptLines[receiptLineIndex].Data[j], f).Width) / 2, heightOnPage);
                            heightOnPage += (int)g.MeasureString(transactionReceipt.ReceiptLines[receiptLineIndex].Data[j], f).Height;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (transactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || transactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                            continue;
                        switch (transactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j])
                        {
                            case "L": stringFormat.Alignment = StringAlignment.Near; break;
                            case "R": stringFormat.Alignment = StringAlignment.Far; break;
                            case "C": stringFormat.Alignment = StringAlignment.Center; break;
                            default: stringFormat.Alignment = StringAlignment.Near; break;
                        }
                        try
                        {
                            if (transactionReceipt.ReceiptLines[receiptLineIndex + 1].Data[j].StartsWith("--")) // heading
                                stringFormat.FormatFlags = StringFormatFlags.NoClip;
                            else if (transactionReceipt.ReceiptLines[receiptLineIndex].Data[j].StartsWith("--")) // -- below heading
                                stringFormat.FormatFlags = StringFormatFlags.NoWrap;
                            else
                                stringFormat.FormatFlags = StringFormatFlags.NoClip;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while calculating the format flags", ex);
                        }

                        g.DrawString(transactionReceipt.ReceiptLines[receiptLineIndex].Data[j], transactionReceipt.ReceiptLines[receiptLineIndex].LineFont, Brushes.Black, new System.Drawing.Rectangle((int)(xPos + cumWidth), heightOnPage, (int)(colWidth[j]), lineHeight), stringFormat);
                        cumWidth += colWidth[j];
                    }
                }

                receiptLineIndex++;
                heightOnPage += lineHeight;
                if (receiptLineIndex < totalLines) // at least 1 more line to be printed. check if next line can be printed on same page
                {
                    lineHeight = (receiptLineHeight > 0) ? receiptLineHeight : 20;
                    for (int j = 0; j < 5; j++)
                    {
                        if (transactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || transactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                            continue;
                        lineHeight = Math.Max(lineHeight, (int)Math.Ceiling(g.MeasureString(transactionReceipt.ReceiptLines[receiptLineIndex].Data[j], transactionReceipt.ReceiptLines[receiptLineIndex].LineFont, (int)colWidth[j]).Height));
                    }
                    if (heightOnPage + lineHeight > marginBounds.Height)
                        break;
                }
            }
            pageHeight = heightOnPage;
            log.LogMethodExit(image);
            return image;
        }

        /// <summary>
        /// It will Save the file in the Server Path
        /// </summary>
        public string PrintShiftReceipt(int shiftKey, string posMachine, string user, Core.Utilities.Utilities utilities, SqlTransaction sqlTransaction, DateTime? shiftTime = null)
        {
            log.LogMethodEntry(shiftKey, posMachine, user, utilities, shiftTime, sqlTransaction);
            try
            {
                int col1x = 0;
                int col2x = 80;
                int col3x = 160;
                int col4x = 240;
                int col5x = 320;
                int col6x = 480;
                int yLocation = 820;
                int yIncrement = 20;
                String loginTime = "";
                string logOutTime = "";
                DateTime lginTime = DateTime.MinValue;
                DateTime lgouttime = DateTime.MinValue;
                string systemAmount = "0";
                string systemCardCount = "0";
                string systemGameCardAmount = "0";
                string systemCheckAmt, systemCouponAmt, systemTicketNumber;
                systemCheckAmt = systemCouponAmt = systemTicketNumber = "0";
                string shift = "";
                string filePath = @"C:/Parafait Home/Reports/"; //server path
                string formattedDateTime = DateTime.Now.ToString("yyyy-MM-dd HHmmss", CultureInfo.InvariantCulture);
                string fileNamePostFix = Convert.ToString(shiftKey) + "-" + formattedDateTime;
                string fileName = "POS Shift Reports - " + posMachine + "-" + fileNamePostFix;
                string absolutePath = filePath + fileName + ".pdf";

                BaseFont defaultFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, false);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, false);

                if (shiftTime == null)
                {
                    shiftTime = DateTime.Now;
                }
                string getShiftQuery = @"Select * from shift where shift_key=@shiftkey";
                SqlParameter[] sqlParameters = new SqlParameter[1];
                sqlParameters[0] = new SqlParameter("@shiftkey", shiftKey);

                DataTable shiftDetailsDataTable = dataAccessHandler.executeSelectQuery(getShiftQuery, sqlParameters, sqlTransaction);
                DataTable otherShiftDataTable = new DataTable();

                using (FileStream fileStream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    Document document = new Document(PageSize.A4, 20, 20, 20, 20);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(document, fileStream);
                    document.Open();
                    PdfContentByte cb = pdfWriter.DirectContent;
                    cb.BeginText();

                    if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                    {
                        string getCloseShiftQuery = @"Select top 1 * from shift where pos_machine=@posmachine and shift_username=@user
                                                                 and shift_key>@shiftkey and shift_time>@shifttime and shift_action='Close'
                                                                 order by shift_key desc";
                        SqlParameter[] getCloseShiftParameter = new SqlParameter[4];
                        getCloseShiftParameter[0] = new SqlParameter("@shiftkey", shiftKey);
                        getCloseShiftParameter[1] = new SqlParameter("@posmachine", posMachine);
                        getCloseShiftParameter[2] = new SqlParameter("@user", user);
                        getCloseShiftParameter[3] = new SqlParameter("@shifttime", shiftTime);

                        otherShiftDataTable = dataAccessHandler.executeSelectQuery(getCloseShiftQuery, getCloseShiftParameter, sqlTransaction);
                        shift = utilities.MessageUtils.getMessage("Open Shift");
                    }
                    else
                    {
                        string getOpenShiftQuery = @"Select top 1 * from shift where pos_machine=@posmachine and shift_username=@user
                                                                 and shift_key<@shiftkey and shift_time<@shifttime and shift_action='Open'
                                                                 order by shift_key desc";

                        SqlParameter[] getOpenShiftParameter = new SqlParameter[4];
                        getOpenShiftParameter[0] = new SqlParameter("@shiftkey", shiftKey);
                        getOpenShiftParameter[1] = new SqlParameter("@posmachine", posMachine);
                        getOpenShiftParameter[2] = new SqlParameter("@user", user);
                        getOpenShiftParameter[3] = new SqlParameter("@shifttime", shiftTime);

                        otherShiftDataTable = dataAccessHandler.executeSelectQuery(getOpenShiftQuery, getOpenShiftParameter, sqlTransaction);

                        if (otherShiftDataTable.Rows.Count == 0)
                        {
                            log.Error(utilities.MessageUtils.getMessage("Open shift is not available. Can not print"));
                            log.LogMethodExit(null);
                            return null;
                        }

                        shift = utilities.MessageUtils.getMessage("Close Shift");
                    }

                    if (shiftDetailsDataTable.Rows.Count > 0)
                    {
                        cb.SetFontAndSize(baseFont, 10.0F);
                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("POS Shift Report") + " - " + shift, 30, yLocation, 0);
                        yLocation -= 30;
                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("POS Name") + ": " + shiftDetailsDataTable.Rows[0]["pos_machine"].ToString(), 30, yLocation, 0);
                        yLocation -= 30;
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Far;
                        cb.SetFontAndSize(defaultFont, 9.0F);

                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            lginTime = (DateTime)shiftDetailsDataTable.Rows[0]["shift_time"];
                            loginTime = ((DateTime)shiftDetailsDataTable.Rows[0]["shift_time"]).ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                        }
                        else
                        {
                            if (otherShiftDataTable.Rows.Count > 0)
                            {
                                lginTime = (DateTime)otherShiftDataTable.Rows[0]["shift_time"];
                                loginTime = ((DateTime)otherShiftDataTable.Rows[0]["shift_time"]).ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                            }
                        }

                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            if (otherShiftDataTable.Rows.Count > 0)
                            {
                                lgouttime = (DateTime)otherShiftDataTable.Rows[0]["shift_time"];
                                logOutTime = ((DateTime)otherShiftDataTable.Rows[0]["shift_time"]).ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                            }
                        }
                        else
                        {
                            lgouttime = (DateTime)shiftDetailsDataTable.Rows[0]["shift_time"];
                            logOutTime = ((DateTime)shiftDetailsDataTable.Rows[0]["shift_time"]).ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                        }

                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Cashier") + ": " + shiftDetailsDataTable.Rows[0]["shift_username"].ToString(), col1x, yLocation, 0);
                        yLocation -= yIncrement;
                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Login Time") + ": " + loginTime, col1x, yLocation, 0);
                        yLocation -= yIncrement;
                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Logout Time") + ": " + "", col1x, yLocation, 0);
                        }
                        else
                        {
                            cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Logout Time") + ": " + logOutTime, col1x, yLocation, 0);
                        }

                        yLocation -= yIncrement;
                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Cashier"), col2x, yLocation, 0);
                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("System"), col3x, yLocation, 0);
                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Net Shift"), col4x, yLocation, 0);

                        yLocation -= yIncrement;
                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Cash"), col1x, yLocation, 0);
                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["shift_amount"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, (0).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, (0).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col4x, yLocation, 0);
                        }
                        else
                        {
                            if (otherShiftDataTable.Rows.Count > 0)
                            {
                                systemAmount = otherShiftDataTable.Rows[0]["shift_amount"].ToString();
                            }
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["shift_amount"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, (Convert.ToDecimal(systemAmount == "" ? "0" : systemAmount) + Convert.ToDecimal(shiftDetailsDataTable.Rows[0]["actual_amount"].ToString())).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["actual_amount"].ToString(), col4x, yLocation, 0);
                        }
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Card Count") + ":", col1x, yLocation, 0);

                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["card_count"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, (0).ToString(utilities.ParafaitEnv.NUMBER_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, (0).ToString(utilities.ParafaitEnv.NUMBER_FORMAT), col4x, yLocation, 0);
                        }
                        else
                        {
                            if (otherShiftDataTable.Rows.Count > 0)
                            {
                                systemCardCount = otherShiftDataTable.Rows[0]["card_count"].ToString();
                            }
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["card_count"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, (Convert.ToDecimal(systemCardCount == "" ? "0" : systemCardCount) + Convert.ToDecimal(shiftDetailsDataTable.Rows[0]["actual_cards"].ToString())).ToString(utilities.ParafaitEnv.NUMBER_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["actual_cards"].ToString(), col4x, yLocation, 0);
                        }
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Credit Card") + ":", col1x, yLocation, 0);

                        cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["CreditCardamount"].ToString(), col2x, yLocation, 0);
                        yLocation -= yIncrement;

                        Array.Clear(paymentModes.ToArray(), 0, paymentModes.Count);
                        Array.Clear(amountList.ToArray(), 0, amountList.Count);

                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            logOutTime = loginTime;
                            lgouttime = lginTime;
                        }

                        GetPaymentModesForShiftReport(shiftDetailsDataTable.Rows[0]["pos_machine"].ToString(), lginTime, lgouttime, sqlTransaction);
                        for (int i = 0; i < paymentModes.Count; i++)
                        {
                            cb.ShowTextAligned(0, paymentModes[i], col1x, yLocation, 0);
                            cb.ShowTextAligned(0, amountList[i].ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, amountList[i].ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col4x, yLocation, 0);

                            yLocation -= yIncrement;
                        }

                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Game Card") + ":", col1x, yLocation, 0);

                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["GameCardamount"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, (0).ToString(utilities.ParafaitEnv.NUMBER_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, (0).ToString(utilities.ParafaitEnv.NUMBER_FORMAT), col4x, yLocation, 0);
                        }
                        else
                        {
                            if (otherShiftDataTable.Rows.Count > 0)
                            {
                                systemGameCardAmount = otherShiftDataTable.Rows[0]["GameCardamount"].ToString();
                            }
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["GameCardamount"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, (Convert.ToDecimal(systemGameCardAmount == "" ? "0" : systemGameCardAmount) + Convert.ToDecimal(shiftDetailsDataTable.Rows[0]["ActualGameCardamount"].ToString())).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["ActualGameCardamount"].ToString(), col4x, yLocation, 0);
                        }
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Cheques") + ":", col1x, yLocation, 0);

                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["ChequeAmount"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, 0.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, 0.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col4x, yLocation, 0);
                        }
                        else
                        {
                            if (otherShiftDataTable.Rows.Count > 0)
                            {
                                systemCheckAmt = otherShiftDataTable.Rows[0]["ChequeAmount"].ToString();
                            }
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["ChequeAmount"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, (Convert.ToDecimal(systemCheckAmt == "" ? "0" : systemCheckAmt) + Convert.ToDecimal(shiftDetailsDataTable.Rows[0]["ActualChequeAmount"].ToString())).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["ActualChequeAmount"].ToString(), col4x, yLocation, 0);
                        }
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Coupons") + ":", col1x, yLocation, 0);

                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["CouponAmount"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, 0.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, 0.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col4x, yLocation, 0);
                        }
                        else
                        {
                            if (otherShiftDataTable.Rows.Count > 0)
                            {
                                systemCouponAmt = otherShiftDataTable.Rows[0]["CouponAmount"].ToString();
                            }
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["CouponAmount"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, (Convert.ToDecimal(systemCouponAmt == "" ? "0" : systemCouponAmt) + Convert.ToDecimal(shiftDetailsDataTable.Rows[0]["ActualChequeAmount"].ToString())).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["ActualCouponAmount"].ToString(), col4x, yLocation, 0);
                        }
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Tickets") + ":", col1x, yLocation, 0);

                        if (shiftDetailsDataTable.Rows[0]["shift_action"].ToString() == "Open")
                        {
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["shift_ticketnumber"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, 0.ToString(utilities.ParafaitEnv.NUMBER_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, 0.ToString(utilities.ParafaitEnv.NUMBER_FORMAT), col4x, yLocation, 0);
                        }
                        else
                        {
                            if (otherShiftDataTable.Rows.Count > 0)
                            {
                                systemTicketNumber = otherShiftDataTable.Rows[0]["shift_ticketnumber"].ToString();
                            }
                            cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["shift_ticketnumber"].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(0, (Convert.ToDecimal((systemTicketNumber == "" ? "0" : systemTicketNumber)) + Convert.ToDecimal(shiftDetailsDataTable.Rows[0]["actual_tickets"].ToString())).ToString(utilities.ParafaitEnv.NUMBER_FORMAT), col3x, yLocation, 0);
                            cb.ShowTextAligned(0, systemTicketNumber == "" ? "0" : systemTicketNumber, col4x, yLocation, 0);
                        }
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Remarks") + ":", col1x, yLocation, 0);

                        cb.ShowTextAligned(0, shiftDetailsDataTable.Rows[0]["shift_remarks"].ToString(), col2x, yLocation, 0);

                        yLocation -= yIncrement;

                        string getTaxQuery = @"select isnull(sum(TaxableAmount), 0), isnull(sum(DiscountOnTaxableAmount), 0), isnull(sum(NonTaxableAmount), 0),
                                          isnull(sum(DiscountOnNonTaxableAmount), 0), isnull(sum(TaxAmount), 0), isnull(sum(DiscountOnTaxAmount), 0) 
                                        from (select 
                                                   case when tax_id is not null then p.price * p.quantity else 0 end TaxableAmount,
                                                   case when tax_id is not null then p.price * p.quantity * (-1.0 * isnull(td.discPerc, 0)/100.0) else 0 end DiscountOnTaxableAmount,
                                                   case when tax_id is null then p.price * p.quantity else 0 end NonTaxableAmount,
                                                   case when tax_id is null then p.price * p.quantity * (-1.0 * isnull(td.discPerc, 0)/100.0) else 0 end DiscountOnNonTaxableAmount,
                                                   p.price * p.quantity * isnull(p.tax_percentage, 0)/100.0 TaxAmount,
                                                   p.price * p.quantity * isnull(p.tax_percentage, 0)/100.0 * (-1.0 * isnull(td.discPerc, 0)/100.0) DiscountOnTaxAmount
                                               from trx_lines p 
                                                    left outer join (select trxId, lineId, sum(discountPercentage) discPerc
                                                                      from trxDiscounts td
                                                                     group by trxId, lineId) td
                                                    on td.trxId = p.trxId
                                                    and td.lineId = p.lineId,
                                                    trx_header h, shift s
                                              where h.trxDate >= s.shift_time
                                               and h.pos_machine = s.pos_machine
                                               and s.shift_time >= @shiftTimeLin
                                               and s.shift_time <=@shiftTimeLout
                                               and h.trxDate <=@shiftTimeLout
                                               and s.pos_machine = @pos
                                               and p.trxId = h.TrxId 
                                               and product_id is not null) v";

                        SqlParameter[] taxParameters = new SqlParameter[3];
                        taxParameters[0] = new SqlParameter("@pos", shiftDetailsDataTable.Rows[0]["pos_machine"].ToString());
                        taxParameters[1] = new SqlParameter("@shiftTimeLin", lginTime);
                        taxParameters[2] = new SqlParameter("@shiftTimeLout", lgouttime);

                        DataTable taxDataTable = dataAccessHandler.executeSelectQuery(getTaxQuery, taxParameters, sqlTransaction);

                        double TaxableAmount = Convert.ToDouble(taxDataTable.Rows[0][0]);
                        double DiscountOnTaxableAmount = Convert.ToDouble(taxDataTable.Rows[0][1]);
                        double NonTaxableAmount = Convert.ToDouble(taxDataTable.Rows[0][2]);
                        double DiscountOnNonTaxableAmount = Convert.ToDouble(taxDataTable.Rows[0][3]);
                        double TaxAmount = Convert.ToDouble(taxDataTable.Rows[0][4]);
                        double DiscountOnTaxAmount = Convert.ToDouble(taxDataTable.Rows[0][5]);

                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Amount"), col5x, yLocation, 0);
                        cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Tax"), col6x, yLocation, 0);
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(100, utilities.MessageUtils.getMessage("Taxable Sale") + ":", col2x, yLocation, 0);
                        cb.ShowTextAligned(0, TaxableAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col5x, yLocation, 0);
                        cb.ShowTextAligned(0, TaxAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col6x, yLocation, 0);
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(100, utilities.MessageUtils.getMessage("Non-Taxable Sale") + ":", col2x, yLocation, 0);
                        cb.ShowTextAligned(0, NonTaxableAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col5x, yLocation, 0);
                        cb.ShowTextAligned(0, 0.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col6x, yLocation, 0);
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(100, utilities.MessageUtils.getMessage("Disc. On Taxable") + ":", col2x, yLocation, 0);
                        cb.ShowTextAligned(0, DiscountOnTaxableAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col5x, yLocation, 0);
                        cb.ShowTextAligned(0, DiscountOnTaxAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col6x, yLocation, 0);
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(100, utilities.MessageUtils.getMessage("Disc. On Non-Taxable") + ":", col2x, yLocation, 0);
                        cb.ShowTextAligned(0, DiscountOnNonTaxableAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col5x, yLocation, 0);
                        cb.ShowTextAligned(0, 0.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col6x, yLocation, 0);
                        yLocation -= yIncrement;

                        cb.ShowTextAligned(100, utilities.MessageUtils.getMessage("Net Sale") + ":", col2x, yLocation, 0);
                        cb.ShowTextAligned(0, (TaxableAmount + DiscountOnTaxableAmount + NonTaxableAmount + DiscountOnNonTaxableAmount).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col5x, yLocation, 0);
                        cb.ShowTextAligned(0, (TaxAmount + DiscountOnTaxAmount).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col6x, yLocation, 0);
                        yLocation -= yIncrement;

                        string taxQuery = @"select discount_name, isnull(sum(DiscountAmount), 0), isnull(sum(DiscountedTaxAmount), 0)
                                        from (select d.discount_name,
                                                   p.price * p.quantity * dv.discountPercentage/100.0 * -1 DiscountAmount,
                                                   p.price * p.quantity * isnull(p.tax_percentage, 0)/100.0 * (-1.0 * dv.discountPercentage/100.0) DiscountedTaxAmount
                                               from trx_lines p, trx_header h, trxDiscounts dv, discounts d, shift s
                                              where dv.discountId = d.discount_id
                                               and dv.trxId = p.trxId
                                               and dv.lineId = p.lineId
                                               and h.trxDate >= s.shift_time
                                               and h.pos_machine = s.pos_machine
                                               and s.shift_time> = @shiftTimeLin
                                               and s.shift_time <=@shiftTimeLout
                                               and h.trxDate <=@shiftTimeLout
                                               and s.pos_machine = @pos
                                               and p.trxId = h.TrxId) v
                                        group by discount_name";

                        SqlParameter[] taxQueryParameters = new SqlParameter[3];
                        taxQueryParameters[0] = new SqlParameter("@pos", shiftDetailsDataTable.Rows[0]["pos_machine"].ToString());
                        taxQueryParameters[1] = new SqlParameter("@shiftTimeLin", lginTime);
                        taxQueryParameters[2] = new SqlParameter("@shiftTimeLout", lgouttime);

                        DataTable dataTable = dataAccessHandler.executeSelectQuery(taxQuery, taxQueryParameters, sqlTransaction);

                        if (dataTable.Rows.Count > 0)
                        {
                            cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Discounts"), col1x, yLocation, 0);
                            yLocation -= yIncrement;
                        }

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            cb.ShowTextAligned(0, dataTable.Rows[i][0].ToString(), col2x, yLocation, 0);
                            cb.ShowTextAligned(100, Convert.ToDouble(dataTable.Rows[i][1]).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col5x, yLocation, 0);
                            cb.ShowTextAligned(100, Convert.ToDouble(dataTable.Rows[i][2]).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col6x, yLocation, 0);
                            yLocation -= yIncrement;
                        }

                        string paymentModeQuery = @"select 'Cash', isnull(sum(CashAmount), 0), count(trxid), 'Y' sort
                                     from trx_header h, shift s 
                                     where h.CashAmount != 0
                                     and h.trxDate >= s.shift_time
                                     and h.pos_machine = s.pos_machine
                                     and s.shift_time > = @shiftTimeLin
                                     and s.shift_time <=@shiftTimeLout
                                     and h.trxDate <=@shiftTimeLout
                                     and s.pos_machine = @pos
                                    union all
                                    select 'Game Card', isnull(sum(h.GameCardAmount), 0), count(trxid), 'Y' sort
                                     from trx_header h, shift s  
                                     where h.GameCardAmount != 0
                                     and h.trxDate >= s.shift_time
                                     and h.pos_machine = s.pos_machine
                                     and s.shift_time> = @shiftTimeLin
                                     and s.shift_time <=@shiftTimeLout
                                     and h.trxDate <=@shiftTimeLout
                                     and s.pos_machine = @pos
                                    union all
                                    select case p.isCreditCard when 'Y' then 'Credit Card' else p.PaymentMode end,
		                                    sum(tp.Amount), count(h.trxid), p.isCreditCard
                                     from trx_header h, trxPayments tp, PaymentModes p, shift s
                                     where tp.paymentModeId = p.PaymentModeId
                                     and tp.trxId = h.trxId
                                     and (p.isCreditCard = 'Y' or (isCreditCard = 'N' and isCash = 'N' and isDebitCard = 'N'))
                                     and h.trxDate >= s.shift_time
                                     and h.pos_machine = s.pos_machine
                                     and s.shift_time> = @shiftTimeLin
                                     and s.shift_time <=@shiftTimeLout
                                     and h.trxDate <=@shiftTimeLout
                                     and s.pos_machine = @pos
                                     group by case p.isCreditCard when 'Y' then 'Credit Card' else p.PaymentMode end, p.isCreditCard
                                    order by sort desc, 1";

                        SqlParameter[] paymentModeParameters = new SqlParameter[3];
                        paymentModeParameters[0] = new SqlParameter("@pos", shiftDetailsDataTable.Rows[0]["pos_machine"].ToString());
                        paymentModeParameters[1] = new SqlParameter("@shiftTimeLin", lginTime);
                        paymentModeParameters[2] = new SqlParameter("@shiftTimeLout", lgouttime);
                        DataTable paymentModeDataTable = dataAccessHandler.executeSelectQuery(paymentModeQuery, paymentModeParameters, sqlTransaction);

                        if (paymentModeDataTable.Rows.Count > 0)
                        {
                            yLocation -= yIncrement;
                            cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Payment Mode"), col1x, yLocation, 0);
                            cb.ShowTextAligned(0, utilities.MessageUtils.getMessage("Amount"), col5x, yLocation, 0);
                            cb.ShowTextAligned(0, "#" + utilities.MessageUtils.getMessage("Receipts"), col6x, yLocation, 0);
                            yLocation -= yIncrement;
                        }

                        for (int i = 0; i < paymentModeDataTable.Rows.Count; i++)
                        {
                            cb.ShowTextAligned(0, paymentModeDataTable.Rows[i][0].ToString(), col1x, yLocation, 0);
                            cb.ShowTextAligned(0, Convert.ToDouble(paymentModeDataTable.Rows[i][1]).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col5x, yLocation, 0);
                            cb.ShowTextAligned(0, Convert.ToDouble(paymentModeDataTable.Rows[i][2]).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), col6x, yLocation, 0);
                            yLocation -= yIncrement;
                        }
                    }
                    cb.EndText();
                    document.Close();
                    log.LogMethodExit(absolutePath);
                    return absolutePath;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while printing Document", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// It will give the PaymentMode
        /// </summary>
        internal void GetPaymentModesForShiftReport(string posMachine, DateTime loginTime, DateTime logOutTime, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(posMachine, loginTime, logOutTime, sqlTransaction);

            string query = "select PaymentMode, isCreditCard, " +
                              "isnull(sum(tp.Amount), 0) netAmount " +
                              " from shift s left outer join trx_header th " +
                              "on th.trxDate >= s.shift_time " +
                              "and th.pos_machine = s.pos_machine " +
                              "left outer join trxPayments tp on tp.trxId = th.trxId " +
                              "left outer join PaymentModes p on p.PaymentModeId = tp.paymentModeId " +
                              "where s.shift_time >= @shiftTimeLIN " +
                              "and s.shift_time <= @shiftTimeLOUT " +
                              "and th.trxDate <=@shiftTimeLOUT " +
                              "and s.pos_machine = @pos " +
                              "and (p.isCreditCard = 'Y' or (p.isCash = 'N' and p.isDebitCard = 'N' and p.isCreditCard = 'N')) " +
                              "group by PaymentMode, isCreditCard order by 2 desc, 1";

            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@pos", posMachine);
            sqlParameters[1] = new SqlParameter("@shiftTimeLIN", loginTime);
            sqlParameters[2] = new SqlParameter("@shiftTimeLOUT", logOutTime);

            DataTable paymentModeDataTable = new DataTable();
            paymentModeDataTable = dataAccessHandler.executeSelectQuery(query, sqlParameters, sqlTransaction);

            for (int i = 0; i < paymentModeDataTable.Rows.Count; i++)
            {
                paymentModes[i] = paymentModeDataTable.Rows[i]["PaymentMode"].ToString();
                bool isCreditCard = (paymentModeDataTable.Rows[i]["isCreditCard"].ToString() == "Y" ? true : false);
                amountList[i] = Convert.ToDouble(paymentModeDataTable.Rows[i]["netAmount"]);

                if (isCreditCard)
                    paymentModes[i] = " " + paymentModes[i];
            }
            log.LogVariableState("paymentModes", paymentModes);
            log.LogVariableState("amountList", amountList);
            log.LogMethodExit();
        }

        private void SendPrintProgressUpdates(string message)
        {
            log.LogMethodEntry(message);
            if (PrintProgressUpdates != null)
            {
                PrintProgressUpdates(message);
            }
            else
            {
                log.Info("PrintProgressUpdates is not defined. Hence no message sent back");
            }
            log.LogMethodExit();
        }

        private void SendCardPrinterErrorValue(bool errorValue)
        {
            log.LogMethodEntry(errorValue);
            if (SetCardPrinterErrorValue != null)
            {
                SetCardPrinterErrorValue(errorValue);
            }
            else
            {
                log.Info("SetCardPrinterErrorValue is not defined. Hence no error value sent back");
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Ticket Class for Ticket printing
    /// </summary>
    public class clsTicket
    {
        internal PaperSize PaperSize;
        internal System.Drawing.Rectangle TicketBorder;
        internal Margins Margin;
        internal int BorderWidth = 2;
        public class PrintObject
        {
            private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);//added
            internal string Text;
            internal Point Location;

            [JsonIgnore]
            internal System.Drawing.Font Font;
            internal int Width;
            internal char Alignment;

            [JsonIgnore]
            internal System.Drawing.Image BarCode;

            [JsonIgnore]
            internal System.Drawing.Image Image;
            internal char Rotate;
            internal string Color;
            internal int BarCodeHeight;
            internal string BarCodeEncodeType;
            [Browsable(false)]
            public string FontString
            {
                get
                {
                    try
                    {
                        return TypeDescriptor.GetConverter(typeof(System.Drawing.Font)).ConvertToInvariantString(Font);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        return string.Empty;
                    }

                }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return;
                    }
                    try
                    {
                        Font = TypeDescriptor.GetConverter(typeof(System.Drawing.Font)).ConvertFromInvariantString(value) as System.Drawing.Font;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                }
            }
            [Browsable(false)]
            public string BarCodeString
            {
                get
                {
                    if (BarCode == null)
                    {
                        return string.Empty;
                    }
                    try
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            BarCode.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            string base64String = Convert.ToBase64String(imageBytes);
                            return base64String;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        return string.Empty;
                    }

                }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return;
                    }
                    try
                    {
                        BarCode = System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String(value)));
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            [Browsable(false)]
            public string ImageString
            {
                get
                {
                    if (Image == null)
                    {
                        return string.Empty;
                    }
                    try
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            Image.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            string base64String = Convert.ToBase64String(imageBytes);
                            return base64String;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        return string.Empty;
                    }

                }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return;
                    }
                    try
                    {
                        Image = System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String(value)));
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            public string TextProperty { get { return Text; } set { Text = value; } }
            public Point LocationProperty { get { return Location; } set { Location = value; } }
            [JsonIgnore]
            public System.Drawing.Font FontProperty { get { return Font; } set { Font = value; } }
            public int WidthProperty { get { return Width; } set { Width = value; } }
            public char AlignmentProperty { get { return Alignment; } set { Alignment = value; } }
            [JsonIgnore]
            public System.Drawing.Image BarCodeProperty { get { return BarCode; } set { BarCode = value; } }
            [JsonIgnore]
            public System.Drawing.Image ImageProperty { get { return Image; } set { Image = value; } }
            public char RotateProperty { get { return Rotate; } set { Rotate = value; } }
            public string ColorProperty { get { return Color; } set { Color = value; } }
            public int BarCodeHeightProperty { get { return BarCodeHeight; } set { BarCodeHeight = value; } }
            public string BarCodeEncodeTypeProperty { get { return BarCodeEncodeType; } set { BarCodeEncodeType = value; } }
        }
        public List<PrintObject> PrintObjectList = new List<PrintObject>();
        public clsTicket Backside = null;
        public string CardNumber;
        public System.Drawing.Image BackgroundImage;

        public int TrxId = -1;
        public int TrxLineId = -1;
        public PaperSize PaperSizeProperty
        {
            get { return PaperSize; }
            set { PaperSize = value; }
        }
        public System.Drawing.Rectangle TicketBorderProperty
        {
            get { return TicketBorder; }
            set { TicketBorder = value; }
        }
        public Margins MarginProperty
        {
            get { return Margin; }
            set { Margin = value; }
        }
        public int BorderWidthProperty
        {
            get { return BorderWidth; }
            set { BorderWidth = value; }
        }
        public decimal NotchWidth { get; set; }
        public decimal NotchDistance { get; set; }
        public bool PrintReverse { get; set; }

    }

    /// <summary>
    /// Receipt Class for Receipt printing
    /// </summary>
    public class ReceiptClass
    {
        public int TotalLines;
        public class line
        {
            private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            public string TemplateSection;
            public string[] Data;
            public string[] Alignment;
            public int colCount;
            [Browsable(false)]
            public string LineFontString
            {
                get
                {
                    try
                    {
                        return TypeDescriptor.GetConverter(typeof(System.Drawing.Font)).ConvertToInvariantString(LineFont);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        return string.Empty;
                    }

                }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return;
                    }
                    try
                    {
                        LineFont = TypeDescriptor.GetConverter(typeof(System.Drawing.Font)).ConvertFromInvariantString(value) as System.Drawing.Font;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                }
            }
            [Browsable(false)]
            public string BarCodeString
            {
                get
                {
                    if (BarCode == null)
                    {
                        return string.Empty;
                    }
                    try
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            BarCode.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            string base64String = Convert.ToBase64String(imageBytes);
                            return base64String;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        return string.Empty;
                    }

                }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return;
                    }
                    try
                    {
                        BarCode = System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String(value)));
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            [JsonIgnore]
            public System.Drawing.Font LineFont;
            [JsonIgnore]
            public System.Drawing.Image BarCode;
            public int LineId;
            public int LineHeight;//to hold Line Height metadata per receipt line
            public bool CancelledLine = false;

            public line()
            {
                Data = new string[5];
                Alignment = new string[5];
                colCount = 0;
            }
        }
        public line[] ReceiptLines;

        public ReceiptClass(int maxLines)
        {
            ReceiptLines = new line[maxLines];
            for (int i = 0; i < maxLines; i++)
                ReceiptLines[i] = new line();
        }
    }

}
