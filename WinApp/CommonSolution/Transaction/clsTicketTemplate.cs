/********************************************************************************************
* Project Name - Semnox.Parafait.Transaction - clsTicketTemplate
* Description  - clsTicketTemplate 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Online Transaction in Kiosk changes
*2.70.3      11-Feb-2020      Deeksha            Invariant culture-Font Issue Fix
 *2.120       27-Apr-2021      Girish Kundar     Duplicate Wristband printing issue Fix
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Printer;

namespace Semnox.Parafait.Transaction
{
    public class clsTicketTemplate
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int _TemplateId = -1;
        public string _Name;
        public class clsHeader
        {
            public int _TicketTemplateId = -1;
            public int BorderWidth = 2;
            public decimal _Width = 3, _Height = 2.5M;
            public System.Drawing.Printing.Margins Margins = new System.Drawing.Printing.Margins(10, 10, 10, 10);
            public Image BackgroundImage;
            public int BacksideTemplateId = -1;
            public clsTicketTemplate BacksideTemplate = null;
            public decimal NotchDistance = 1;
            public decimal NotchWidth = 025M;
            public bool PrintReverse = false;

        }
        public clsHeader Header = new clsHeader();

        public class clsKeywords
        {
            public class clsSection
            {
                public string Name;
                public string[] Keywords;

                public clsSection(string name, string[] keywords)
                {
                    log.LogMethodEntry(name, keywords);
                    Name = name;
                    Keywords = keywords;
                    log.LogMethodExit(null);
                }
            }

            public List<clsSection> Sections = new List<clsSection>();

            public clsKeywords()
            {
                log.LogMethodEntry();

                string[] MainSection = (@"@SiteName, @SiteLogo, @Date, @SystemDate, @TrxId, @TrxNo, @TrxOTP, @Cashier, @Token, @POS, @TaxNo,
                                       @CustomerName, @Phone, @CustomerPhoto, 
                                       @CardBalance, @CreditBalance, @BonusBalance, @PrimaryCardNumber, 
                                       @Remarks, @SiteAddress,@ScreenNumber").Split(',');
                Sections.Add(new clsSection("MAIN", MainSection));

                string[] product = (@"@Product, @Price, @Quantity, @Amount, @TaxName, @Tax, @TaxName1, @TaxName2, @TaxName3, 
                                    @TaxPercentage1, @TaxPercentage2, @TaxPercentage3, @TaxAmount1, @TaxAmount2, @TaxAmount3,
                                    @Time, @FromTime, @ToTime, @Seat, @LineRemarks, @TicketBarCodeNo, @TicketBarCode, @Tickets").Split(',');
                Sections.Add(new clsSection("PRODUCT", product));

                string[] totals = { "@Total", "@TaxTotal" };
                Sections.Add(new clsSection("TOTALS", totals));

                string[] cardInfo = { "@CardNumber", "@BarCodeCardNumber", "@QRCodeCardNumber", "@CardTickets" };
                Sections.Add(new clsSection("CARDINFO", cardInfo));

                string[] couponDetails = ("@CouponNumber, @DiscountName, @DiscountPercentage, @DiscountAmount, @CouponEffectiveDate").Split(',');
                Sections.Add(new clsSection("DISCOUNT", couponDetails));

                string[] couponDetailsExtra = ("@CouponExpiryDate, @BarCodeCouponNumber, @QRCodeCouponNumber").Split(',');
                Sections.Add(new clsSection("", couponDetailsExtra));

                log.LogMethodExit(null);
            }
        }
        public clsKeywords Keywords = new clsKeywords();

        public enum ELEMENT_TYPE
        {
            KEYWORD = 1,
            FREE_FORMAT = 2,
            UNKNOWN = 0,
        }

        public class clsTicketElement
        {
            public ELEMENT_TYPE ElementType;
            public string Name;
            public string Value;
            public int formatId;
            public Font Font;
            public Point Location;
            public int Width;
            public char Alignment;
            public char Rotate;
            public string uniqueId;
            public string Color;
            public int BarCodeHeight;
            public clsTicketElement()
            {
                log.LogMethodEntry();
                formatId = -1;

                log.LogMethodExit(null);
            }
        }

        public List<clsTicketElement> TicketElements = new List<clsTicketElement>();
        Utilities _utilities;
        public clsTicketTemplate(Utilities inUtils)
        {
            log.LogMethodEntry(inUtils);
            _utilities = inUtils;
            log.LogMethodExit(null);
        }

        public clsTicketTemplate(int TemplateId, Utilities inUtils)
            : this(inUtils)
        {
            log.LogMethodEntry(TemplateId, inUtils);

            _TemplateId = TemplateId;
            populateTemplate();

            log.LogMethodExit(null);
        }

        void populateTemplate()
        {
            log.LogMethodEntry();
            try
            {

                // 3 tier logic 
                TicketTemplateHeaderListBL ticketTemplateHeaderListBL = new TicketTemplateHeaderListBL(_utilities.ExecutionContext);
                List<KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>> searchParameters = new List<KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>>();
                searchParameters.Add(new KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>(TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.TEMPLATE_ID, _TemplateId.ToString()));
                searchParameters.Add(new KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>(TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.IS_ACTIVE, "1")); // Only one record
                List<TicketTemplateHeaderDTO> ticketTemplateHeaderDTOList = ticketTemplateHeaderListBL.GetTicketTemplateHeaderDTOList(searchParameters, true, true);
                if (ticketTemplateHeaderDTOList != null && ticketTemplateHeaderDTOList.Any())
                {
                    foreach (TicketTemplateHeaderDTO ticketTemplateHeaderDTO in ticketTemplateHeaderDTOList)
                    {
                        ReceiptPrintTemplateHeaderBL receiptPrintTemplateHeaderBL = new ReceiptPrintTemplateHeaderBL(_utilities.ExecutionContext, _TemplateId, false);
                        if (receiptPrintTemplateHeaderBL.ReceiptPrintTemplateHeaderDTO != null)
                        {
                            _Name = receiptPrintTemplateHeaderBL.ReceiptPrintTemplateHeaderDTO.TemplateName;
                        }
                        Header._TicketTemplateId = ticketTemplateHeaderDTO.TicketTemplateId;
                        Header._Height = ticketTemplateHeaderDTO.Height;
                        Header._Width = ticketTemplateHeaderDTO.Width;
                        Header.BorderWidth = Convert.ToInt32(ticketTemplateHeaderDTO.BorderWidth);
                        Header.Margins.Left = Convert.ToInt32(ticketTemplateHeaderDTO.LeftMargin);
                        Header.Margins.Right = Convert.ToInt32(ticketTemplateHeaderDTO.RightMargin);
                        Header.Margins.Top = Convert.ToInt32(ticketTemplateHeaderDTO.TopMargin);
                        Header.Margins.Bottom = Convert.ToInt32(ticketTemplateHeaderDTO.BottomMargin);

                        Header.NotchDistance = Convert.ToDecimal(ticketTemplateHeaderDTO.NotchDistance);
                        Header.NotchWidth = Convert.ToDecimal(ticketTemplateHeaderDTO.NotchWidth);
                        Header.PrintReverse = Convert.ToBoolean(ticketTemplateHeaderDTO.PrintReverse);

                        if (ticketTemplateHeaderDTO.BacksideTemplateId > -1)
                        {
                            Header.BacksideTemplateId = Convert.ToInt32(ticketTemplateHeaderDTO.BacksideTemplateId);
                            Header.BacksideTemplate = new clsTicketTemplate(Header.BacksideTemplateId, _utilities);
                        }
                        if (ticketTemplateHeaderDTO.BackgroundImage != null)
                        {
                            Header.BackgroundImage = _utilities.ConvertToImage(ticketTemplateHeaderDTO.BackgroundImage);
                        }
                        if (ticketTemplateHeaderDTO.TicketTemplateElementDTOList != null && ticketTemplateHeaderDTO.TicketTemplateElementDTOList.Any())
                        {
                            foreach (TicketTemplateElementDTO ticketTemplateElementDTO in ticketTemplateHeaderDTO.TicketTemplateElementDTOList)
                            {
                                clsTicketElement element = new clsTicketElement();
                                if (Convert.ToInt32(ticketTemplateElementDTO.Type) == 1)
                                    element.ElementType = ELEMENT_TYPE.KEYWORD;
                                else
                                    element.ElementType = ELEMENT_TYPE.FREE_FORMAT;

                                element.Font = CustomFontConverter.ConvertStringToFont(_utilities.ExecutionContext, ticketTemplateElementDTO.Font.ToString());
                                string[] coords = ticketTemplateElementDTO.Location.ToString().Split(',');
                                element.Location = new Point(int.Parse(coords[0]), int.Parse(coords[1]));
                                element.Name = ticketTemplateElementDTO.Name.ToString();
                                element.uniqueId = ticketTemplateElementDTO.UniqueId.ToString();
                                element.Value = ticketTemplateElementDTO.Value.ToString();
                                element.Width = Convert.ToInt32(ticketTemplateElementDTO.Width);
                                element.Alignment = ticketTemplateElementDTO.Alignment.ToString()[0];
                                element.Rotate = ticketTemplateElementDTO.Rotate;  // N
                                element.formatId = Convert.ToInt32(ticketTemplateElementDTO.FormatId);
                                element.Color = ticketTemplateElementDTO.Color.ToString();
                                element.BarCodeHeight = Convert.ToInt32(ticketTemplateElementDTO.BarCodeHeight);
                                TicketElements.Add(element);
                            }
                        }
                    }
                }
                else
                {
                    log.LogMethodExit(null, "Throwing ApplicationException - Invalid ticket template");
                    throw new ApplicationException("Invalid ticket template");
                }
                //DataTable dtH = _utilities.executeDataTable(@"select rh.TemplateName, th.* 
                //                                            from TicketTemplateHeader th, ReceiptPrintTemplateHeader rh 
                //                                            where rh.TemplateId = th.TemplateId 
                //                                            and rh.TemplateId = @TemplateId", new SqlParameter("@TemplateId", _TemplateId));

                //log.LogVariableState("@TemplateId", _TemplateId);

                //if (dtH.Rows.Count == 0)
                //{
                //    log.LogMethodExit(null, "Throwing ApplicationException - Invalid ticket template");
                //    throw new ApplicationException("Invalid ticket template");
                //}

                //_Name = dtH.Rows[0]["TemplateName"].ToString();
                //Header._TicketTemplateId = Convert.ToInt32(dtH.Rows[0]["TicketTemplateId"]);

                //Header._Height = Convert.ToDecimal(dtH.Rows[0]["Height"]);
                //Header._Width = Convert.ToDecimal(dtH.Rows[0]["Width"]);
                //Header.BorderWidth = Convert.ToInt32(dtH.Rows[0]["BorderWidth"]);

                //Header.Margins.Left = Convert.ToInt32(dtH.Rows[0]["LeftMargin"]);
                //Header.Margins.Right = Convert.ToInt32(dtH.Rows[0]["RightMargin"]);
                //Header.Margins.Top = Convert.ToInt32(dtH.Rows[0]["TopMargin"]);
                //Header.Margins.Bottom = Convert.ToInt32(dtH.Rows[0]["BottomMargin"]);

                //Header.NotchDistance= Convert.ToDecimal(dtH.Rows[0]["NotchDistance"]);


                //if (dtH.Rows[0]["BacksideTemplateId"] != DBNull.Value)
                //{
                //    Header.BacksideTemplateId = Convert.ToInt32(dtH.Rows[0]["BacksideTemplateId"]);
                //    Header.BacksideTemplate = new clsTicketTemplate(Header.BacksideTemplateId, _utilities);
                //}

                //if (dtH.Rows[0]["BackgroundImage"] != DBNull.Value)
                //{
                //    Header.BackgroundImage = _utilities.ConvertToImage(dtH.Rows[0]["BackgroundImage"]);
                //}

                //DataTable dtE = _utilities.executeDataTable("select * from TicketTemplateElements where TicketTemplateId = @id", new SqlParameter("@id", Header._TicketTemplateId));
                //foreach (DataRow dr in dtE.Rows)
                //{
                //    clsTicketElement element = new clsTicketElement();
                //    if (Convert.ToInt32(dr["Type"]) == 1)
                //        element.ElementType = ELEMENT_TYPE.KEYWORD;
                //    else
                //        element.ElementType = ELEMENT_TYPE.FREE_FORMAT;


                //    element.Font = CustomFontConverter.ConvertStringToFont(_utilities.ExecutionContext, dr["Font"].ToString());
                //    string[] coords = dr["Location"].ToString().Split(',');
                //    element.Location = new Point(int.Parse(coords[0]), int.Parse(coords[1]));
                //    element.Name = dr["Name"].ToString();
                //    element.uniqueId = dr["UniqueId"].ToString();
                //    element.Value = dr["Value"].ToString();
                //    element.Width = Convert.ToInt32(dr["Width"]);
                //    element.Alignment = dr["Alignment"].ToString()[0];
                //    if (dr["Rotate"] == DBNull.Value)
                //        element.Rotate = 'N';
                //    else
                //        element.Rotate = dr["Rotate"].ToString()[0];
                //    if (dr["FormatId"] != DBNull.Value)
                //    {
                //        element.formatId = Convert.ToInt32(dr["FormatId"]);
                //    }
                //    else
                //    {
                //        element.formatId = -1;
                //    }

                //    element.Color = dr["Color"].ToString();
                //    if (dr["BarCodeHeight"] != DBNull.Value)
                //        element.BarCodeHeight = Convert.ToInt32(dr["BarCodeHeight"]);
                //    //else
                //    //    element.BarCodeHeight = null;

                //    TicketElements.Add(element);
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(null);
        }

        public Size getTicketSize()
        {
            log.LogMethodEntry();

            decimal DPIx = 100;
            Size returnValue = new Size((int)(Header._Width * DPIx - (Header.Margins.Left + Header.Margins.Right) * DPIx / 100), (int)(Header._Height * DPIx - (Header.Margins.Top + Header.Margins.Bottom) * DPIx / 100));

            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public void RemoveElement(clsTicketElement removeItem)
        {
            log.LogMethodEntry(removeItem);

            TicketElements.Remove(removeItem);
            _utilities.executeNonQuery("delete from TicketTemplateElements where TicketTemplateId = @ticketTemplateId and UniqueId = @uniqueId",
                new SqlParameter("@ticketTemplateId", Header._TicketTemplateId),
                new SqlParameter("@uniqueId", removeItem.uniqueId));

            log.LogVariableState("@ticketTemplateId", Header._TicketTemplateId);
            log.LogVariableState("@uniqueId", removeItem.uniqueId);

            log.LogMethodExit(null);
        }

        public void Save()
        {
            log.LogMethodEntry();

            if (_TemplateId == -1)
            {
                SqlTransaction SQLTrx = _utilities.createConnection().BeginTransaction();
                SqlConnection cnn = SQLTrx.Connection;
                try
                {
                    object o = _utilities.executeScalar(@"insert into ReceiptPrintTemplateHeader (TemplateName)
                                                     values (@TemplateName);
                                                     select @@identity",
                                                        SQLTrx,
                                                        new SqlParameter("@TemplateName", _Name));

                    log.LogVariableState("@TemplateName", _Name);

                    _TemplateId = Convert.ToInt32(o);

                    SqlParameter bgImageParam = new SqlParameter("@BackgroundImage", SqlDbType.VarBinary, -1);

                    log.LogVariableState("@BackgroundImage", SqlDbType.VarBinary);

                    bgImageParam.Value = (Header.BackgroundImage == null ? DBNull.Value : (object)_utilities.ConvertToByteArray(Header.BackgroundImage));

                    o = _utilities.executeScalar(@"insert into TicketTemplateHeader (TemplateId, Width, Height, LeftMargin, RightMargin, TopMargin, BottomMargin, BorderWidth, LastUpdateDate, LastUpdatedBy, BackgroundImage, BacksideTemplateId)
                                                     values (@TemplateId, @Width, @Height, @Left, @Right, @Top, @Bottom, @BorderWidth, getdate(), @LastUpdatedBy, @BackgroundImage, @BacksideTemplateId);
                                                     select @@identity",
                                                        SQLTrx,
                                                        new SqlParameter("@TemplateId", _TemplateId),
                                                        new SqlParameter("@Width", Header._Width),
                                                        new SqlParameter("@Height", Header._Height),
                                                        new SqlParameter("@Left", Header.Margins.Left),
                                                        new SqlParameter("@Right", Header.Margins.Right),
                                                        new SqlParameter("@Top", Header.Margins.Top),
                                                        new SqlParameter("@Bottom", Header.Margins.Bottom),
                                                        new SqlParameter("@BorderWidth", Header.BorderWidth),
                                                        new SqlParameter("@LastUpdatedBy", _utilities.ParafaitEnv.LoginID),
                                                        bgImageParam,
                                                        new SqlParameter("@BacksideTemplateId", (Header.BacksideTemplateId == -1 ? DBNull.Value : (object)Header.BacksideTemplateId)));

                    log.LogVariableState("@TemplateId", _TemplateId);
                    log.LogVariableState("@Width", Header._Width);
                    log.LogVariableState("@Height", Header._Height);
                    log.LogVariableState("@Left", Header.Margins.Left);
                    log.LogVariableState("@Right", Header.Margins.Right);
                    log.LogVariableState("@Top", Header.Margins.Top);
                    log.LogVariableState("@Bottom", Header.Margins.Bottom);
                    log.LogVariableState("@BorderWidth", Header.BorderWidth);
                    log.LogVariableState("@LastUpdatedBy", _utilities.ParafaitEnv.LoginID);
                    log.LogVariableState("@BacksideTemplateId", (Header.BacksideTemplateId == -1 ? DBNull.Value : (object)Header.BacksideTemplateId));

                    Header._TicketTemplateId = Convert.ToInt32(o);

                    SQLTrx.Commit();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving the ticket template", ex);

                    SQLTrx.Rollback();

                    log.LogMethodExit(null, "Throwing Exception" + ex);
                    throw ex;
                }
                finally
                {
                    cnn.Close();
                }
            }
            else
            {
                SqlParameter bgImageParam = new SqlParameter("@BackgroundImage", SqlDbType.VarBinary, -1);
                bgImageParam.Value = (Header.BackgroundImage == null ? DBNull.Value : (object)_utilities.ConvertToByteArray(Header.BackgroundImage));

                _utilities.executeNonQuery(@"update ReceiptPrintTemplateHeader set TemplateName = @templateName where TemplateId = @templateId;
                                             update TicketTemplateHeader set Width = @Width, Height = @Height, LeftMargin = @Left, RightMargin = @Right, TopMargin = @Top, BottomMargin = @Bottom,
                                                BorderWidth = @BorderWidth, LastUpdateDate = getdate(), LastUpdatedBy = @LastUpdatedBy,
                                                BackgroundImage = @BackgroundImage, BacksideTemplateId = @BacksideTemplateId
                                            where TicketTemplateId = @TicketTemplateId",
                                           new SqlParameter("@Width", Header._Width),
                                           new SqlParameter("@Height", Header._Height),
                                           new SqlParameter("@Left", Header.Margins.Left),
                                           new SqlParameter("@Right", Header.Margins.Right),
                                           new SqlParameter("@Top", Header.Margins.Top),
                                           new SqlParameter("@Bottom", Header.Margins.Bottom),
                                           new SqlParameter("@BorderWidth", Header.BorderWidth),
                                           new SqlParameter("@TicketTemplateId", Header._TicketTemplateId),
                                           new SqlParameter("@templateId", _TemplateId),
                                           new SqlParameter("@templateName", _Name),
                                           new SqlParameter("@LastUpdatedBy", _utilities.ParafaitEnv.LoginID),
                                           bgImageParam,
                                           new SqlParameter("@BacksideTemplateId", (Header.BacksideTemplateId == -1 ? DBNull.Value : (object)Header.BacksideTemplateId)));

                log.LogVariableState("@BackgroundImage", SqlDbType.VarBinary);
                log.LogVariableState("@Width", Header._Width);
                log.LogVariableState("@Height", Header._Height);
                log.LogVariableState("@Left", Header.Margins.Left);
                log.LogVariableState("@Right", Header.Margins.Right);
                log.LogVariableState("@Top", Header.Margins.Top);
                log.LogVariableState("@Bottom", Header.Margins.Bottom);
                log.LogVariableState("@BorderWidth", Header.BorderWidth);
                log.LogVariableState("@TicketTemplateId", Header._TicketTemplateId);
                log.LogVariableState("@templateId", _TemplateId);
                log.LogVariableState("@templateName", _Name);
                log.LogVariableState("@LastUpdatedBy", _utilities.ParafaitEnv.LoginID);
                log.LogVariableState("@BacksideTemplateId", (Header.BacksideTemplateId == -1 ? DBNull.Value : (object)Header.BacksideTemplateId));
            }

            foreach (clsTicketElement item in TicketElements)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
                string fontString = converter.ConvertToString(item.Font);

                if (_utilities.executeNonQuery(@"update TicketTemplateElements
                                                    set Value = @Value,
                                                    Type = @Type,
                                                    Font = @Font,
                                                    Location = @Location,
                                                    Width = @Width,
                                                    Alignment = @Alignment,
                                                    FormatId = @FormatId,
                                                    Rotate = @Rotate,
                                                    Color = @Color,
                                                    BarcodeHeight = @barCodeHeight
                                                    where uniqueId = @uniqueId
                                                    and TicketTemplateId = @TicketTemplateId",
                                                new SqlParameter("@Value", item.Value),
                                                new SqlParameter("@Width", item.Width),
                                                new SqlParameter("@Alignment", item.Alignment),
                                                new SqlParameter("@Rotate", item.Rotate),
                                                new SqlParameter("@Color", (string.IsNullOrEmpty(item.Color) ? DBNull.Value : (object)item.Color)),
                                                new SqlParameter("@Type", item.ElementType),
                                                new SqlParameter("@Font", fontString),
                                                new SqlParameter("@barCodeHeight", ((item.BarCodeHeight != 0) ? (object)item.BarCodeHeight : DBNull.Value)),
                                                (item.formatId == -1) ? new SqlParameter("@FormatId", DBNull.Value) : new SqlParameter("@FormatId", item.formatId),
                                                new SqlParameter("@Location", item.Location.X.ToString() + "," + item.Location.Y.ToString()),
                                                new SqlParameter("@TicketTemplateId", Header._TicketTemplateId),
                                                new SqlParameter("@uniqueId", item.uniqueId)) == 0)
                {
                    _utilities.executeNonQuery(@"insert into TicketTemplateElements 
                                                       (uniqueId, Name, Value, Width, Alignment, Type, Font, Location, TicketTemplateId, Rotate, FormatId, Color, BarcodeHeight)
                                                    values (@uniqueId, @Name, @Value, @Width, @Alignment, @Type, @Font, @Location, @TicketTemplateId, @Rotate, @FormatId, @Color, @barCodeHeight)",
                                                new SqlParameter("@Name", item.Name),
                                                new SqlParameter("@Value", item.Value),
                                                new SqlParameter("@Width", item.Width),
                                                new SqlParameter("@Alignment", item.Alignment),
                                                new SqlParameter("@Rotate", item.Rotate),
                                                new SqlParameter("@Color", (string.IsNullOrEmpty(item.Color) ? DBNull.Value : (object)item.Color)),
                                                 new SqlParameter("@barCodeHeight", ((item.BarCodeHeight != 0) ? (object)item.BarCodeHeight : DBNull.Value)),
                                                new SqlParameter("@Type", item.ElementType),
                                                new SqlParameter("@Font", fontString),
                                                (item.formatId == -1) ? new SqlParameter("@FormatId", DBNull.Value) : new SqlParameter("@FormatId", item.formatId),
                                                new SqlParameter("@Location", item.Location.X.ToString() + "," + item.Location.Y.ToString()),
                                                new SqlParameter("@TicketTemplateId", Header._TicketTemplateId),
                                                new SqlParameter("@uniqueId", item.uniqueId));

                    log.LogVariableState("@Name", item.Name);
                    log.LogVariableState("@Value", item.Value);
                    log.LogVariableState("@Width", item.Width);
                    log.LogVariableState("@barCodeHeight", item.BarCodeHeight);
                    log.LogVariableState("@Alignment", item.Alignment);
                    log.LogVariableState("@Rotate", item.Rotate);
                    log.LogVariableState("@Color", (string.IsNullOrEmpty(item.Color) ? DBNull.Value : (object)item.Color));
                    log.LogVariableState("@Type", item.ElementType);
                    log.LogVariableState("@Font", fontString);
                    log.LogVariableState("@FormatId", DBNull.Value);
                    log.LogVariableState("@FormatId", item.formatId);
                    log.LogVariableState("@Location", item.Location.X.ToString() + "," + item.Location.Y.ToString());
                    log.LogVariableState("@TicketTemplateId", Header._TicketTemplateId);
                    log.LogVariableState("@uniqueId", item.uniqueId);
                }
            }

            log.LogMethodExit(null);
        }
    }
}
