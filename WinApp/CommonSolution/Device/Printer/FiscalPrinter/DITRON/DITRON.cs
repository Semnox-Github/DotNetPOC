using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using DitronDriverInterface;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    /// <summary>
    /// 
    /// </summary>
    public class Ditron : Semnox.Parafait.Device.Printer.FiscalPrint.FiscalPrinter
    {
        Utilities Utilities;
        string loginID = "";
        bool status;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public Ditron(Utilities _Utilities) : base(_Utilities)
        {
            log.LogMethodEntry(_Utilities);
            Utilities = _Utilities;
            this.loginID = Utilities.ParafaitEnv.LoginID;       
            Utilities.ParafaitEnv.Initialize();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Port Open Method
        /// Method to open the port for the connected printer
        /// </summary>
        public override bool OpenPort()
        {
            log.LogMethodEntry();
            try
            {
                string ComPort = Utilities.getParafaitDefaults("FISCAL_PRINTER_PORT_NUMBER");
                bool status = DriverInterface.connect(ComPort);            
                if (status == false)
                {
                    log.Error(" Printer Initialization failed");
                    log.LogMethodExit(false);
                    return false;
                }               
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);              
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }


        /// <summary>
        /// Printing the Receipt
        /// </summary>     
        public override bool PrintReceipt(int TrxId, ref string Message, SqlTransaction SQLTrx = null, decimal tenderedCash = 0, bool isFiscal = true, bool trxReprint = false)
        {
            log.LogMethodEntry(TrxId, SQLTrx, trxReprint, Message);
            try
            {
                //created on 2018-01-09 for Ditron fisical printer
                string description = "";
                string ProductName = "";
                int productID = 0;
                decimal price = 0;
                decimal discount = 0;
                decimal TotalNetAmount = 0;
                DateTime TrxDate = new DateTime();
                decimal TotalPercentage = 0;
                decimal TotalDiscountValue = 0;
                int quantity = 0;             
                int PosId = Utilities.ParafaitEnv.POSMachineId;

                DataTable dTable = new DataTable();
                SqlCommand cmdgetTrxDetails = Utilities.getCommand(SQLTrx);
                cmdgetTrxDetails.CommandText = "select  tl.product_id,convert(Numeric(12,2),tl.amount) as price, p.product_name,isnull(sum(td.DiscountAmount),0) as discount,sum(tl.quantity) as quantity from trx_lines tl left join TrxDiscounts td on tl.TrxId=td.TrxId and tl.LineId=td.LineId inner join products p on tl.product_id=p.product_id where tl.trxid= " + TrxId + " and not exists (select 'x' from product_type pt where p.product_type_id = pt.product_type_id and pt.CardSale = 'Y') group by tl.product_id,tl.quantity,p.product_name,tl.amount";
                cmdgetTrxDetails.Parameters.AddWithValue("@trxid", TrxId);
                SqlDataAdapter da = new SqlDataAdapter(cmdgetTrxDetails);
                DataTable dt = new DataTable();
                da.Fill(dt);
              

                if (dt != null && dt.Rows.Count > 0)
                {

                    //To get the TotalNetAmount,  Total Discount Value, and Total discount Percentage 
                    dTable = Utilities.executeDataTable("select isnull(t.TrxNetAmount,0) as TrxNetAmount,t.TrxDate,isnull(Round((t.TrxDiscountPercentage),3),0) as TotalPercentageDiscount, isnull(Sum(td.DiscountAmount),0) As TotalDiscountValue from trx_header t  left join TrxDiscounts td on t.TrxId=td.TrxId  where t.TrxId= " + TrxId + "group by t.TrxNetAmount, t.TrxDiscountPercentage,t.TrxDate");
                    if (dTable != null && dTable.Rows.Count > 0)
                    {

                        TotalNetAmount = decimal.Parse(dTable.Rows[0]["TrxNetAmount"].ToString());
                        TotalPercentage = decimal.Parse(dTable.Rows[0]["TotalPercentageDiscount"].ToString());
                        TotalDiscountValue = decimal.Parse(dTable.Rows[0]["TotalDiscountValue"].ToString());
                        TrxDate = DateTime.Parse(dTable.Rows[0]["TrxDate"].ToString());
                       
                    }


                    csOrderPrint order = new csOrderPrint();
                    order.PosID = PosId;
                    order.Operator = this.loginID;
                    order.TableDescription = "";
                    order.TotalDiscountPercDescription = "";
                    order.Data = TrxDate;
                    order.OrderID = TrxId;
                    order.OrderDetails = new List<csOrderPrintDetails>();
                    order.Client = new csClients();
                    csClients client = new csClients();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        csOrderPrintDetails orderDetail = new csOrderPrintDetails();
                        orderDetail.DiscountCollection = new List<csOrderPrintDiscount>();
                        csOrderPrintDiscount orderDiscount1 = new csOrderPrintDiscount();

                        productID = Convert.ToInt32(dt.Rows[i]["product_id"]);
                        price = Convert.ToDecimal(dt.Rows[i]["price"]);
                        quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                        ProductName = description = dt.Rows[i]["product_name"].ToString();
                        discount = Convert.ToDecimal(dt.Rows[i]["discount"]);                     

                        //DrderDetails class for product details
                        //Populating the Orderdetails
                        orderDetail.ProductDescription = ProductName;
                        orderDetail.Department = 1;
                        orderDetail.ProductID = productID;
                        orderDetail.Quantity = quantity;
                        orderDetail.Price = price;
                        orderDetail.Total = orderDetail.Quantity * orderDetail.Price;
                        orderDetail.Comment = "";

                        order.OrderDetails.Add(orderDetail);

                    }


                    //Populating the Client Details
                    client.Name = "";
                    client.Balance = "";
                    client.Spent = "";
                    client.Point1 = "";
                    client.Point2 = "";
                    client.Point1 = "";
                    client.Active = "";
                    client.ClientCategory = "";
                    client.ClientCode = "";
                    client.Discount = "";
                    order.Client = client;

                    order.TotalReceipt = TotalNetAmount;

                    List<string> footer = new List<string>();


                    //Available Credits
                    DataTable dtCredits = new DataTable();
                    dtCredits = Utilities.executeDataTable("select Convert(numeric(18,2), (Credits + CreditPLusCardBalance + CreditPlusCredits)) as AvailableCredits from CardView where card_id = (select top 1 cardid from trxPayments where trxid = " + TrxId + ")");
                    if (dtCredits.Rows.Count > 0 && dtCredits.Rows[0]["AvailableCredits"] != null)
                    {
                        string availableCredits = "";
                        availableCredits = dtCredits.Rows[0]["AvailableCredits"].ToString();
                        footer.Add(Utilities.MessageUtils.getMessage("Available Credits") + ": " + availableCredits);
                        footer.Add("");//For the Ditron printer to print footer contents, count should be 2

                    }
                                    
                    //Fiscal Print
                    status = DriverInterface.LeshimKuponiTatimor(order, footer);
           

                    if (status == false)
                    {
                        Message = "Print Receipt failed";                      
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                else
                {
                    Message = "Print Not Available for Card Products";                  
                    log.LogMethodExit(false);
                    return false;
                }
                    
            }

            catch (Exception ex)
            {
                Message = ex.Message;               
                log.LogMethodExit(false);
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Close Method 
        /// </summary>
        public override void ClosePort()
        {
            log.LogMethodEntry();
            status = DriverInterface.disconnect();
            log.LogMethodExit(null);
        }
    }
}
