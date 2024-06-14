/********************************************************************************************
 * Project Name - DPLFileRI 
 * Description  - DPL  
 * 
 **************
 **Version Log
 **************
 *Version      Date             Modified By        Remarks          
 *********************************************************************************************
 *2.60.0       12-Apr-2019      Archana            Modified : Include/Exclude for redeemable products
 *2.60         12-Apr-2019      Girish Kundar      Modified : Replaced Purchase Tax 3 tier with tax 3 tier         
 *2.110.0      28-Jan-2021      Mushahid Faizan    DPL issue fixes for products. 
 *2.120.0      07-Jun-2021      Mushahid Faizan    Modified :Issue fixes for - Ticket 27689 - DPL is giving an 'Index out of range' error if UOM is not set
 *                                                 and handle null check to prevent empty dto list error.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class DPLFileRI : DPLFile
    {
        //private StreamReader dplNewFile; 
        private int manualProductTypeId = -1;

        public DPLFileRI(Utilities utilities, StreamReader dplNewFile) : base(utilities)
        {
            log.LogMethodEntry(utilities, dplNewFile);
            int fileHeader = 0;
            this.dplHeader = new DPLHeader();
            dplLineList = new List<DPLLine>();
            while (!dplNewFile.EndOfStream)
            {
                var fileRow = dplNewFile.ReadLine();
                var rowData = Regex.Split(fileRow, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                if (fileHeader == 0)
                {
                    LoadHeader(rowData);
                    fileHeader = 1;
                    GetDefaultsForProductCreation();
                }
                else
                {
                    LoadLine(rowData);
                }
            }
            log.LogMethodExit();
        }

        internal override void LoadHeader(string[] headerRow)
        {
            log.LogMethodEntry(headerRow);
            if (headerRow != null)
            {
                if (headerRow.Length <= 8)
                {
                    dplHeader.hasError = false;
                    try
                    {
                        dplHeader.formatCode = headerRow[0];
                        dplHeader.dplRemarks = "RI format DPL Inventory Receive";
                        if (headerRow[0].Length == 0)
                            throw new Exception(utilities.MessageUtils.getMessage(1262));
                    }
                    catch (Exception ex)
                    {
                        dplHeader.hasError = true;
                        dplHeader.errorMessage += utilities.MessageUtils.getMessage(1263) + ex.Message;
                    }
                    try
                    {
                        dplHeader.poSiteCode = Convert.ToInt32(headerRow[1]);
                        dplHeader.poSiteId = -1;
                        if (headerRow[1].Length == 0)
                            throw new Exception(utilities.MessageUtils.getMessage(1264));
                    }
                    catch (Exception ex)
                    {
                        dplHeader.hasError = true;
                        dplHeader.errorMessage += utilities.MessageUtils.getMessage(1265) + ex.Message;
                    }
                    try
                    {
                        dplHeader.vendorCode = headerRow[2];
                        dplHeader.vendorId = -1;
                        if (headerRow[2].Length == 0)
                            throw new Exception(utilities.MessageUtils.getMessage(1266));

                    }
                    catch (Exception ex)
                    {
                        dplHeader.hasError = true;
                        dplHeader.errorMessage += utilities.MessageUtils.getMessage(1267) + ex.Message;
                    }
                    try
                    {
                        dplHeader.vendorInvoiceNumber = headerRow[3];
                        if (headerRow[3].Length == 0)
                            throw new Exception(utilities.MessageUtils.getMessage(1268));
                    }
                    catch (Exception ex)
                    {
                        dplHeader.hasError = true;
                        dplHeader.errorMessage += utilities.MessageUtils.getMessage(1269) + ex.Message;
                    }
                    try
                    {
                        if (headerRow[4].Length == 0)
                            throw new Exception(utilities.MessageUtils.getMessage(1270));
                        else
                        {
                            DateTime receiveDate = DateTime.ParseExact(headerRow[4], this.utilities.ParafaitEnv.DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
                            dplHeader.receivedDate = receiveDate;
                        }
                    }
                    catch (Exception ex)
                    {
                        dplHeader.hasError = true;
                        dplHeader.errorMessage += utilities.MessageUtils.getMessage(1271) + ex.Message + " ( " + this.utilities.ParafaitEnv.DATE_FORMAT + " ) ";
                    }
                    if (headerRow.Length >= 6)
                    {
                        try
                        {
                            if (headerRow[5] != null && headerRow[5].Length > 0)
                                dplHeader.poNumber = headerRow[5];
                            dplHeader.poId = -1;
                        }
                        catch (Exception ex)
                        {
                            dplHeader.hasError = true;
                            dplHeader.errorMessage += utilities.MessageUtils.getMessage(1272) + ex.Message;
                        }
                    }
                    dplHeader.dplMarkupPercent = Double.NaN;
                    if (headerRow.Length >= 7)
                    {
                        try
                        {
                            if (headerRow[6] != null && headerRow[6].Length > 0)
                                dplHeader.dplMarkupPercent = Convert.ToDouble(headerRow[6]);
                        }
                        catch (Exception ex)
                        {
                            dplHeader.hasError = true;
                            dplHeader.errorMessage += utilities.MessageUtils.getMessage(1371) + ex.Message;
                        }
                    }
                }
                else
                {
                    dplHeader.hasError = true;
                    dplHeader.errorMessage += utilities.MessageUtils.getMessage(1273);
                }

                if (!dplHeader.hasError)
                {
                    ValidateHeaderData();
                }

            }
            log.LogMethodExit();
        }

        internal override void LoadLine(string[] lineRow)
        {
            log.LogMethodEntry(lineRow);
            if (lineRow != null)
            {
                DPLLine dplLineRec = new DPLLine();
                dplLineRec.hasError = false;
                if (lineRow.Length >= 4 && lineRow.Length <= 8)
                {
                    try
                    {
                        dplLineRec.ProductCode = lineRow[0];
                        dplLineRec.productId = -1;
                        //dplLineRec.taxId = -1;
                        if (lineRow[0].Length == 0)
                            throw new Exception(utilities.MessageUtils.getMessage(1274));
                    }
                    catch (Exception ex)
                    {
                        dplLineRec.hasError = true;
                        dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1275) + ex.Message;
                    }
                    try
                    {
                        dplLineRec.prodDescription = lineRow[1].Replace("\"", "");
                        if (lineRow[1].Length == 0)
                            throw new Exception(utilities.MessageUtils.getMessage(1276));
                    }
                    catch (Exception ex)
                    {
                        dplLineRec.hasError = true;
                        dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1277) + ex.Message;
                    }
                    try
                    {
                        dplLineRec.prodQuantity = Convert.ToInt32(lineRow[2]);
                        if (lineRow[2].Length == 0 || dplLineRec.prodQuantity == 0)
                            throw new Exception(utilities.MessageUtils.getMessage(1278));
                        dplLineRec.remainQuantity = dplLineRec.prodQuantity;
                    }
                    catch (Exception ex)
                    {
                        dplLineRec.hasError = true;
                        dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1279) + ex.Message;
                    }
                    try
                    {
                        dplLineRec.prodUOM = lineRow[3];
                        //dplLineRec.uomId = -1;
                        if (lineRow[3].Length == 0)
                        {
                            throw new Exception(utilities.MessageUtils.getMessage(1280));
                        }
                        if (string.IsNullOrEmpty(dplLineRec.prodUOM) == false)
                        {
                            dplLineRec.prodUOM = dplLineRec.prodUOM.Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        dplLineRec.hasError = true;
                        dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1281) + ex.Message;
                    }
                    try
                    {
                        if (lineRow[3].ToUpper() == "PACK" && lineRow[4].Length == 0)
                        { throw new Exception(utilities.MessageUtils.getMessage(1282)); }

                    }
                    catch (Exception ex)
                    {
                        dplLineRec.hasError = true;
                        dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1283) + ex.Message;
                    }

                    try
                    {
                        if (lineRow[4].Length > 0)
                        {
                            dplLineRec.prodPackSize = Convert.ToInt32(lineRow[4]);
                        }
                    }
                    catch (Exception ex)
                    {
                        dplLineRec.hasError = true;
                        dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1284) + ex.Message;
                    }
                    try
                    {
                        dplLineRec.prodPrice = Convert.ToDouble(lineRow[5]);
                        if (lineRow[5].Length == 0)
                            throw new Exception(utilities.MessageUtils.getMessage(1285));
                    }
                    catch (Exception ex)
                    {
                        dplLineRec.hasError = true;
                        dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1286) + ex.Message;
                    }
                    try
                    {
                        if (lineRow.Length > 6 && lineRow[6].Length > 0)
                            dplLineRec.prodPriceInTicket = Convert.ToDouble(lineRow[6]);
                        //if (lineRow[6].Length == 0)
                        //    throw new Exception("Product Price In Ticket is Mandatoty");
                    }
                    catch (Exception ex)
                    {
                        dplLineRec.hasError = true;
                        dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1287) + ex.Message;
                    }


                    try
                    {
                        if (lineRow.Length == 8)
                        {
                            if (lineRow[7].Length > 0 && lineRow[7] != null)
                            {
                                //tempLine.prodExpiryDate = Convert.ToDateTime(lineRow[7]);
                                DateTime receiveDate = DateTime.ParseExact(lineRow[7], this.utilities.ParafaitEnv.DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
                                dplLineRec.prodExpiryDate = receiveDate;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dplLineRec.hasError = true;
                        dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1288) + ex.Message;
                    }
                }
                else
                {
                    dplLineRec.hasError = true;
                    dplLineRec.errorMessage += utilities.MessageUtils.getMessage(1289);
                }
                if (!dplLineRec.hasError)
                {
                    dplLineRec = ValidateLineData(dplLineRec);
                }
                if (lineRow.Length > 2)
                {
                    dplLineList.Add(dplLineRec);
                }
            }
            log.LogMethodExit();
        }

        internal override void ValidateHeaderData()
        {
            log.LogMethodEntry();
            dplHeader.hasError = false;
            try
            {
                dplHeader.poSiteId = GetSiteID(dplHeader.poSiteCode);
            }
            catch (Exception ex)
            {
                dplHeader.hasError = true;
                dplHeader.errorMessage += utilities.MessageUtils.getMessage(1290, dplHeader.poSiteCode.ToString()) + ex.Message;
            }
            try
            {
                VendorList vendorList = new VendorList(utilities.ExecutionContext);
                List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters;
                searchParameters = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDORCODE, dplHeader.vendorCode));
                searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.IS_ACTIVE, "Y"));
                if (utilities.ExecutionContext.IsCorporate)
                {
                    searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, dplHeader.poSiteId.ToString()));
                }
                List<VendorDTO> vendorDTO = vendorList.GetAllVendors(searchParameters);
                if (vendorDTO == null || vendorDTO.Any() == false)
                {
                    throw new Exception(utilities.MessageUtils.getMessage(1291, dplHeader.vendorCode));
                }
                else
                {
                    if (vendorDTO.Count > 1)
                    {
                        throw new Exception(utilities.MessageUtils.getMessage(1292, dplHeader.vendorCode));
                    }
                    else
                    {
                        dplHeader.vendorId = Convert.ToInt32(vendorDTO[0].VendorId);
                    }
                }
            }
            catch (Exception ex)
            {
                dplHeader.hasError = true;
                dplHeader.errorMessage += utilities.MessageUtils.getMessage(1293) + ex.Message;
            }
            try
            {
                InventoryReceiptList inventoryReceiptList = new InventoryReceiptList(utilities.ExecutionContext);
                List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters;
                searchParameters = new List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDOR_BILL_NUMBER, dplHeader.vendorInvoiceNumber));
                if (utilities.ExecutionContext.IsCorporate)
                {
                    searchParameters.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.SITE_ID, dplHeader.poSiteId.ToString()));
                }
                // searchParameters.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVE_DATE, dplHeader.receivedDate.ToString()));
                List<InventoryReceiptDTO> inventoryReceiptListDTO = inventoryReceiptList.GetAllInventoryReceipts(searchParameters);
                if (inventoryReceiptListDTO != null && inventoryReceiptListDTO.Any())
                {
                    if (inventoryReceiptListDTO.Count > 0)
                    {
                        List<PurchaseOrderDTO> purchaseOrderDTOList;
                        PurchaseOrderList purchaseOrderList = new PurchaseOrderList(utilities.ExecutionContext);
                        List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> purchaseOrderDTOSearchParams;

                        foreach (InventoryReceiptDTO inventoryReceiptDTP in inventoryReceiptListDTO)
                        {
                            purchaseOrderDTOSearchParams = null;
                            purchaseOrderDTOSearchParams = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>();
                            purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDERID, inventoryReceiptDTP.PurchaseOrderId.ToString()));
                            purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.VENDORID, dplHeader.vendorId.ToString()));
                            if (utilities.ExecutionContext.IsCorporate)
                            {
                                purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.SITE_ID, dplHeader.poSiteId.ToString()));
                            }
                            purchaseOrderDTOList = purchaseOrderList.GetAllPurchaseOrder(purchaseOrderDTOSearchParams, true);
                            if (purchaseOrderDTOList != null)
                            {
                                if (purchaseOrderDTOList.Count > 0)
                                {
                                    throw new Exception(utilities.MessageUtils.getMessage(1294, dplHeader.vendorInvoiceNumber));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dplHeader.hasError = true;
                dplHeader.errorMessage += ex.Message;
            }
            try
            {
                if (dplHeader.poNumber != null)
                {
                    dplHeader.poId = -1;
                    PurchaseOrderList purchaseOrderList = new PurchaseOrderList(utilities.ExecutionContext);
                    List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> purchaseOrderDTOSearchParams;
                    purchaseOrderDTOSearchParams = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>();
                    purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERNUMBER, dplHeader.poNumber));
                    purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.VENDORID, dplHeader.vendorId.ToString()));
                    purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERSTATUS, "Open,InProgress"));
                    purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_STATUS, "F"));
                    if (utilities.ExecutionContext.IsCorporate)
                    {
                        purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.SITE_ID, dplHeader.poSiteId.ToString()));
                    }
                    List<PurchaseOrderDTO> purchaseOrderDTOList = purchaseOrderList.GetAllPurchaseOrder(purchaseOrderDTOSearchParams, true);
                    if (purchaseOrderDTOList == null || purchaseOrderDTOList.Any() == false)
                    {
                        throw new Exception(utilities.MessageUtils.getMessage(1295, dplHeader.poNumber));
                    }
                    else if (purchaseOrderDTOList.Count > 1)
                        throw new Exception(utilities.MessageUtils.getMessage(1296, dplHeader.poNumber));
                    else
                        dplHeader.poId = purchaseOrderDTOList[0].PurchaseOrderId;
                }
                else
                    dplHeader.poId = -1;
            }
            catch (Exception ex)
            {
                dplHeader.hasError = true;
                dplHeader.errorMessage += utilities.MessageUtils.getMessage(1297, dplHeader.poNumber) + ex.Message;
            }
            try
            {
                if (dplHeader.dplMarkupPercent > 0)
                {
                    double dplMarkupPercentLowerLimit = 0;
                    double dplMarkupPercentUpperLimit = 0;
                    LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
                    searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DPL_FILE_MARKUP_PERCENT"));
                    if (utilities.ExecutionContext.IsCorporate)
                    {
                        searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, dplHeader.poSiteId.ToString()));
                    }
                    //searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "DPL_MARKUPPERCENT_LOWERLIMIT"));
                    List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
                    if (lookupValuesListDTO != null && lookupValuesListDTO.Any())
                    {
                        foreach (LookupValuesDTO lookupValuesDTO in lookupValuesListDTO)
                        {
                            try
                            {
                                if (lookupValuesDTO.LookupValue == "DPL_MARKUPPERCENT_LOWERLIMIT")
                                    dplMarkupPercentLowerLimit = Convert.ToDouble(lookupValuesDTO.Description);
                                if (lookupValuesDTO.LookupValue == "DPL_MARKUPPERCENT_UPPERLIMIT")
                                    dplMarkupPercentUpperLimit = Convert.ToDouble(lookupValuesDTO.Description);
                            }
                            catch
                            {
                                throw new Exception(utilities.MessageUtils.getMessage(1372)); //Check the markup percent setup
                            }
                        }
                    }
                    if (dplMarkupPercentLowerLimit >= dplMarkupPercentUpperLimit)
                    {
                        throw new Exception(utilities.MessageUtils.getMessage(1368)); //range is not set
                    }
                    if (!(dplHeader.dplMarkupPercent >= dplMarkupPercentLowerLimit && dplHeader.dplMarkupPercent <= dplMarkupPercentUpperLimit))
                    {
                        throw new Exception(utilities.MessageUtils.getMessage(1369, dplHeader.dplMarkupPercent.ToString())); //Markup is not within range
                    }
                }
            }
            catch (Exception ex)
            {
                dplHeader.hasError = true;
                dplHeader.errorMessage += utilities.MessageUtils.getMessage(1370) + ex.Message;
                //error validating DPL markup %
            }
            log.LogMethodExit();
        }

        internal override DPLLine ValidateLineData(DPLLine lineObj)
        {
            log.LogMethodEntry(lineObj);
            lineObj.hasError = false;
            //try
            //{
            //    lineObj.productId = GetProductFromUPC(lineObj.prodUPCCode);
            //}
            //catch (Exception ex)
            //{
            //    lineObj.hasError = true;
            //    lineObj.errorMessage += utilities.MessageUtils.getMessage(1298, lineObj.prodUPCCode) + ex.Message;
            //}
            try
            {
                if (lineObj.ProductCode.Length > 0)
                {
                    ProductList productList = new ProductList();
                    List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> prodSearchParm;
                    prodSearchParm = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                    prodSearchParm.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH, lineObj.ProductCode));
                    prodSearchParm.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "Y"));
                    if (utilities.ExecutionContext.IsCorporate)
                    {
                        prodSearchParm.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, dplHeader.poSiteId.ToString()));
                    }
                    List<ProductDTO> productDTOList = productList.GetAllProducts(prodSearchParm);
                    ProductDTO productDTO = new ProductDTO();
                    // productDTO = productBL.GetProduct(lineObj.productId);
                    if (productDTOList != null && productDTOList.Any())
                    {
                        if (productDTOList.Count > 1)
                        {
                            foreach (ProductDTO productDTORecord in productDTOList)
                            {
                                if (productDTORecord.Code == lineObj.ProductCode)
                                {
                                    productDTO = productDTORecord;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            productDTO = productDTOList[0];
                        }

                        lineObj.productId = productDTO.ProductId;
                        lineObj.ProductCode = productDTO.Code; 
                        lineObj.productDTO = productDTO; 
                        if (string.IsNullOrEmpty(lineObj.prodUOM))
                        {
                            lineObj.hasError = true;
                            lineObj.errorMessage += utilities.MessageUtils.getMessage(1280);
                        }
                        if (lineObj.prodUOM.ToUpper() != productDTO.UOMValue.ToUpper())
                        {
                            lineObj.hasError = true;
                            lineObj.errorMessage += utilities.MessageUtils.getMessage(1300, lineObj.prodUOM, productDTO.UOMValue);
                        }
                        if (productDTO.LotControlled && productDTO.ExpiryType == "E" && (lineObj.prodExpiryDate == DateTime.MinValue))
                        {
                            lineObj.hasError = true;
                            lineObj.errorMessage += utilities.MessageUtils.getMessage(1299, lineObj.ProductCode);
                        }
                        if (productDTO.LotControlled && productDTO.ExpiryType == "D" && (lineObj.prodExpiryDate == DateTime.MinValue))
                        {
                            try
                            {
                                lineObj.prodExpiryDate = dplHeader.receivedDate.AddDays(productDTO.ExpiryDays);
                            }
                            catch (Exception ex)
                            {
                                lineObj.hasError = true;
                                //lineObj.errorMessage += utilities.MessageUtils.getMessage(1299, lineObj.ProductCode);
                                lineObj.errorMessage += "Error validating Expiry setup for lot controlled product " + lineObj.ProductCode + " " + ex.Message;
                            }
                        }

                    }
                    else
                    {
                        string allowDPLToCreateProduct;
                        allowDPLToCreateProduct = utilities.ParafaitEnv.getParafaitDefaults("ALLOW_PRODUCT_CREATION_IN_DPL");
                        if (string.IsNullOrEmpty(allowDPLToCreateProduct) || allowDPLToCreateProduct == "N")
                        {
                            throw new Exception(utilities.MessageUtils.getMessage(1301, lineObj.ProductCode));
                        }
                        else
                        {
                            if (dplLineList.Exists(lineEntry => lineEntry.ProductCode == lineObj.ProductCode))
                            {
                                throw new Exception(utilities.MessageUtils.getMessage(1666, lineObj.ProductCode));
                            }
                            else
                            {
                                lineObj.productDTO = CreateNewProductEntry(lineObj);
                                // lineObj.otherMessage = "New product entry is created for " + lineObj.ProductCode;
                            }
                        }
                    }
                }
                else
                    throw new Exception(utilities.MessageUtils.getMessage(1298, lineObj.ProductCode));
            }
            catch (Exception ex)
            {
                lineObj.hasError = true;
                lineObj.errorMessage += utilities.MessageUtils.getMessage(1302) + ex.Message;
            }
            try
            {
                if (lineObj.productDTO != null && lineObj.productDTO.PurchaseTaxId >= 0)
                {
                    TaxList productTaxList = new TaxList(utilities.ExecutionContext);
                    List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchByPTaxParameters;
                    searchByPTaxParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                    searchByPTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_ID, lineObj.productDTO.PurchaseTaxId.ToString()));
                    searchByPTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
                    if (utilities.ExecutionContext.IsCorporate)
                    {
                        searchByPTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, dplHeader.poSiteId.ToString()));
                    }
                    List<TaxDTO> productTaxListDTO = productTaxList.GetAllTaxes(searchByPTaxParameters);

                    if (productTaxListDTO != null && productTaxListDTO.Any())
                    {
                        if (productTaxListDTO.Count > 1)
                            throw new Exception(utilities.MessageUtils.getMessage(1303, lineObj.productDTO.PurchaseTaxId.ToString()));
                        else
                        {
                            lineObj.taxPercentage = productTaxListDTO[0].TaxPercentage;
                        }
                    }
                    else
                        throw new Exception(utilities.MessageUtils.getMessage(1304, lineObj.prodDescription));
                }
                //else
                //    throw new Exception(utilities.MessageUtils.getMessage(1304, lineObj.prodDescription));
            }
            catch (Exception ex)
            {
                lineObj.hasError = true;
                lineObj.errorMessage += utilities.MessageUtils.getMessage(1305) + ex.Message;
            }
            log.LogMethodExit(lineObj);
            return lineObj;
        }

        internal void SetManualProductTypeId()
        {
            log.LogMethodEntry();
            ProductTypeListBL productTypeListBL = new ProductTypeListBL(utilities.ExecutionContext);
            List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE, "MANUAL")
            };
            if (utilities.ExecutionContext.IsCorporate)
            {
                searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.SiteId.ToString()));
            }
            List<ProductTypeDTO> listProductTypeDTOs = productTypeListBL.GetProductTypeDTOList(searchParameters);
            if (listProductTypeDTOs != null && listProductTypeDTOs.Any())
            {
                if (listProductTypeDTOs.Count == 1)
                {
                    manualProductTypeId = listProductTypeDTOs[0].ProductTypeId;
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "Unable to fetch Product type info for manual products"));
                }
            }
            log.LogMethodExit();
        }

        internal override void DoProcessDPLFile()
        {
            log.LogMethodEntry();

            try
            {
                SetManualProductTypeId();
                this.dplFileLinesListDTO = new List<DPLFileLinesDTO>();
                List<DPLLine> localDPLLineList = new List<DPLLine>();
                Products productsBL;

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    try
                    {
                        foreach (DPLLine dplLine in dplLineList)
                        {
                            DPLLine localDPLLine = dplLine;
                            double prodQtyValue = 0;
                            double prodRemainQtyValue = 0;
                            if (dplLine.prodPackSize > 1)
                            {
                                //for UOM Pack, Dozen etc. Convert the quantity to each.
                                prodQtyValue = dplLine.prodQuantity * dplLine.prodPackSize;
                                prodRemainQtyValue = dplLine.prodQuantity * dplLine.prodPackSize;
                            }
                            else
                            {
                                prodQtyValue = dplLine.prodQuantity;
                                prodRemainQtyValue = dplLine.prodQuantity;
                            }

                            if (dplLine.productDTO.ProductId == -1)
                            {
                                ProductsDTO productsDTO = new ProductsDTO();
                                productsDTO.ProductName = dplLine.productDTO.Description;
                                productsDTO.Description = dplLine.productDTO.Description;
                                productsDTO.Price = Convert.ToDecimal(dplLine.productDTO.SalePrice);
                                productsDTO.ProductTypeId = manualProductTypeId;
                                productsDTO.MapedDisplayGroup = defaultDisplayGroupName;
                                productsDTO.InventoryProductCode = dplLine.productDTO.Code;
                                productsDTO.CategoryId = dplLine.productDTO.CategoryId;
                                productsDTO.TaxInclusivePrice = dplLine.productDTO.TaxInclusiveCost;
                                if (dplLine.productDTO.InventoryUOMId == -1)
                                {
                                    dplLine.productDTO.InventoryUOMId = dplLine.productDTO.UomId;
                                }
                                ProductBarcodeDTO productBarcodeDTO = new ProductBarcodeDTO(-1, dplLine.productDTO.Code, -1, true,
                                                                                           utilities.getServerTime(), utilities.ExecutionContext.GetUserId(), utilities.ExecutionContext.GetUserId(), utilities.getServerTime(), utilities.ExecutionContext.GetSiteId(), "", false, -1);
                                productBarcodeDTO.BarCode = dplLine.productDTO.Code;
                                dplLine.productDTO.ProductBarcodeDTOList = new List<ProductBarcodeDTO>();
                                dplLine.productDTO.ProductBarcodeDTOList.Add(productBarcodeDTO);
                                productsDTO.InventoryItemDTO = dplLine.productDTO;

                                productsBL = new Products(utilities.ExecutionContext, productsDTO);
                                productsBL.Save(parafaitDBTrx.SQLTrx);
                                dplLine.productDTO.ProductId = productsDTO.InventoryItemDTO.ProductId;
                                localDPLLine.otherMessage = "New product entry is created for " + dplLine.ProductCode;
                                localDPLLine.productId = productsDTO.InventoryItemDTO.ProductId;

                                //ProductBarcodeDTO productBarcodeDTO = new ProductBarcodeDTO(-1, productsDTO.InventoryItemDTO.Code, productsDTO.InventoryItemDTO.ProductId, "Y",
                                //                                                            utilities.getServerTime(), utilities.ExecutionContext.GetUserId(), utilities.ExecutionContext.GetUserId(), utilities.getServerTime(), utilities.ExecutionContext.GetSiteId(), "", false, -1);
                                //ProductBarcodeBL barcodeBL = new ProductBarcodeBL(productBarcodeDTO);
                                //barcodeBL.Save(parafaitDBTrx.SQLTrx);
                            }
                            DPLFileLinesDTO dplFileLinesDTO = new DPLFileLinesDTO(dplHeader.vendorInvoiceNumber, dplHeader.receivedDate, dplHeader.vendorId, dplHeader.poId, dplHeader.dplMarkupPercent, dplLine.productDTO.ProductId, dplLine.ProductCode, dplLine.prodDescription, prodQtyValue,
                                                                                  prodRemainQtyValue, dplLine.prodPackSize, dplLine.prodPrice, dplLine.prodPriceInTicket, dplLine.prodExpiryDate,
                                                                                  dplLine.taxPercentage, dplHeader.dplRemarks, dplLine.productDTO);

                            dplFileLinesListDTO.Add(dplFileLinesDTO);
                            localDPLLineList.Add(localDPLLine);
                        }
                        dplLineList = localDPLLineList;
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException ex)
                    {
                        log.Error(ex);
                        log.Info(ex.GetAllValidationErrorMessages());
                        dpsFileStatus = false;
                        parafaitDBTrx.RollBack();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        dpsFileStatus = false;
                        parafaitDBTrx.RollBack();
                        throw;
                    }
                }

                PurchaseOrderList purchaseOrderList = new PurchaseOrderList(utilities.ExecutionContext);
                List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> purchaseOrderDTOSearchParams;
                purchaseOrderDTOSearchParams = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>();
                if (dplHeader.poId != -1)
                {
                    try
                    {
                        purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDERID, dplHeader.poId.ToString()));
                        purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERSTATUS, "Open,InProgress"));
                        purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.VENDORID, dplHeader.vendorId.ToString()));
                        purchaseOrderDTOSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_STATUS, "F"));
                        List<PurchaseOrderDTO> purchaseOrderListDTO = purchaseOrderList.GetAllPurchaseOrder(purchaseOrderDTOSearchParams, true);
                        //purchaseOrderList.ValidateOrderProvided(dplHeader.poId, dplFileLinesListDTO, purchaseOrderListDTO);
                        if (purchaseOrderListDTO != null && purchaseOrderListDTO.Any())
                        {
                            PurchaseOrder purchaseOrderBL = new PurchaseOrder(purchaseOrderListDTO[0], utilities.ExecutionContext);
                            foreach (DPLFileLinesDTO dplFileLinesDTO in dplFileLinesListDTO)
                            {
                                double productQtyInDPL = dplFileLinesListDTO.Where(dplLines => dplLines.ProductId == dplFileLinesDTO.ProductId).Sum(dplLines => dplLines.ProductQuantity);
                                if (!purchaseOrderBL.CanAcceptProductQty(dplFileLinesDTO.ProductId, productQtyInDPL))
                                {
                                    throw new Exception(utilities.MessageUtils.getMessage(1306, dplFileLinesDTO.ProductId, productQtyInDPL, dplHeader.poNumber));
                                }
                            }
                            this.purchaseOrderListBL.Add(purchaseOrderBL);
                        }
                        else
                            throw new Exception(utilities.MessageUtils.getMessage(1295, dplHeader.poNumber));
                    }
                    catch (ValidationException ex)
                    {
                        dplHeader.hasError = true;
                        dplHeader.errorMessage += ex.Message;
                        log.Error(ex.GetAllValidationErrorMessages()); //log fatal
                        utilities.EventLog.logEvent("DPL", 'E', "Error while calling purchaseOrderListBL.ValidateOrderProvided: " + ex.Message, "DPLErrorLog", "DPL", 0);
                        throw new Exception(utilities.MessageUtils.getMessage(1297, dplHeader.poNumber));
                    }
                    catch (Exception ex)
                    {
                        dplHeader.hasError = true;
                        dplHeader.errorMessage += ex.Message;
                        log.Error(ex.Message); //log fatal
                        utilities.EventLog.logEvent("DPL", 'E', "Error while calling purchaseOrderListBL.ValidateOrderProvided: " + ex.Message, "DPLErrorLog", "DPL", 0);
                        throw new Exception(utilities.MessageUtils.getMessage(1297, dplHeader.poNumber));
                    }
                }
                 
                try
                {
                    this.ReceiveDPL();
                    dpsFileStatus = true;
                }
                catch (Exception ex)
                {
                    dpsFileStatus = false;
                    log.Error("Error-DoProcessDPLFile. Error Message1 : " + ex.Message.ToString(), ex); //log fatal
                    utilities.EventLog.logEvent("DPL", 'E', "Error-DoProcessDPLFile. Error Message1 : " + ex.Message.ToString(), "DPLErrorLog", "DPL", 0);
                }
            }
            catch (Exception ex)
            {
                dpsFileStatus = false;
                log.Error("Error-DoProcessDPLFile. Error Message2 : " + ex.Message.ToString(), ex);//log fatal
                utilities.EventLog.logEvent("DPL", 'E', "Error-DoProcessDPLFile. Error Message2 : " + ex.Message.ToString(), "DPLErrorLog", "DPL", 0);
            }
            log.LogMethodExit();
        }


    }
}
