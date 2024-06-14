/* Project Name -ProductsWithComboDetailsDatahandler
* Description  - ProductsWithComboDetailsDatahandler
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.70        14-Mar-2019    Guru S A               Booking phase 2 enhancement changes 
*2.70.2      15-Sep-2019    Nitin Pai              BIR Enhancement 
*2.80.0      09-Jun-2020   Jinto Thomas            Enable Active flag for Comboproduct data
*2.120.0     15-Apr-2021   Nitin Pai               price Override feature in tablet
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
   internal class ProductsWithComboDetailsDatahandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private SqlTransaction sqlTransaction;
        private string StrDisplaygroupFilter ="";
        private int userRoleId = -1;
        private string connstring;
        /// <summary>
        /// Default constructor of  ProductsWithComboDetailsDatahandler class
        /// </summary>
        public ProductsWithComboDetailsDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            connstring = dataAccessHandler.ConnectionString;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetProductFilterQuery method
        /// </summary>
        /// <param name="loginId"> loginId</param>
        public void GetProductFilterQuery(string loginId)
        {
            log.LogMethodEntry(loginId);
            bool checkPosmachine = true;

            if (loginId != null && loginId != "")
            {
                UsersDTO userDTO = new Users(executionContext,loginId).UserDTO;
                if (userDTO.UserId != -1)
                {
                    userRoleId = userDTO.RoleId;
                    // Modified Query
                    StrDisplaygroupFilter = @" AND NOT EXISTS ( select 1
                                                                  from UserRoleDisplayGroupExclusions U  
                                                                  where U.Role_id=@roleId
                                                                    and Pdf.Id = U.ProductDisplayGroupId
                                                              ) ";
                    checkPosmachine = false;
                }
            }

            if (checkPosmachine)
            {
                StrDisplaygroupFilter = @"  AND NOT EXISTS (SELECT POSPE.ProductDisplayGroupFormatId 
                                                            FROM POSProductExclusions POSPE 
					                                        WHERE POSPE.POSMachineId=POSM.POSMachineId
					                                            AND POSPE.ProductDisplayGroupFormatId = Pdf.Id)";

            }
            log.LogMethodExit();
        }


        /// <summary>
        /// GetProductListTable method
        /// </summary>
        /// <param name="productsFilterParams">ProductsFilterParams</param>
        /// <returns>returns data table</returns>
        public List<ProductsWithComboDetails> GetProductList(ProductsFilterParams productsFilterParams)
        {
            log.LogMethodEntry(productsFilterParams);
            Products productsBL = new Products();

            if (productsFilterParams.RequiresCardProduct == true)
            {
                if (productsFilterParams.NewCard == true)
                {
                    productsFilterParams.ProductTypeExclude = "RECHARGE";
                }
                else
                {
                    productsFilterParams.ProductTypeExclude = "NEW";
                }
            }

            // To Display Products based on User Role 
            GetProductFilterQuery(productsFilterParams.LoginId);

            TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
            int offSetDuration = timeZoneUtil.GetOffSetDuration(productsFilterParams.SiteId, productsFilterParams.DateOfPurchase);
            offSetDuration = offSetDuration * (-1);

            List<ProductsWithComboDetails> foodAndBeverageProductList = new List<ProductsWithComboDetails>();

            //fetching upsell product based on the productsFilterParams.fetchUpsellProduct value passed
            if (productsFilterParams.FetchUpsellProduct == true)
            {
                int upsellProductId = productsBL.GetUpsellProductId(productsFilterParams.ProductId);
                if (upsellProductId > 0)
                    productsFilterParams.ProductId = productsBL.GetUpsellProductId(productsFilterParams.ProductId);
                else
                    return foodAndBeverageProductList;
            }

            // Added : 25-Jan-2017 Condition in product Query to filter products based on Expiry date - AND (ExpiryDate is null or ExpiryDate > @today ) 

            log.Debug("Starts-GetProductList(productsFilterParams) method.");
            string selectProductQuery = @"SELECT PRD1.product_id ProductId,
                                                PRD1.product_name ProductName,
			                                    (select (select case cnt when 0 then '' else stuff((select ' | '+ cast(BarCode as nvarchar(250))
                                                    FROM ProductBarcode PB
                                                    WHERE PB.productid = PRD2.ProductId and isactive = 'Y'
                                                        group by PB.BarCode
                                                        FOR XML PATH('')), 1, 3, '') end 
                                                    from (select count(*) cnt
                                                    from ProductBarcode PB1
                                                         where PB1.ProductId = PRD2.ProductId and isactive = 'Y'
                                                         group by PB1.productid)v ) from product PRD2 where PRD2.manualProductId=PRD1.product_id) as Barcodes,
                                                isnull((select top 1 translation 
                                                        from ObjectTranslations
                                                        where ElementGuid = PRD1.Guid
                                                            and Element = 'PRODUCT_NAME'
                                                            and LanguageId = @lang), '') TranslatedProductName,
	                                            PRD1.description ProductDescription,
                                                isnull((select top 1 translation 
                                                        from ObjectTranslations
                                                        where ElementGuid = PRD1.Guid
                                                            and Element = 'DESCRIPTION'
                                                            and LanguageId = @lang), '') TranslatedProductDescription,
                                                isnull(PRD1.WebDescription, '') WebDescription ,
                                                isnull((select top 1 translation 
                                                        from ObjectTranslations
                                                        where ElementGuid = PRD1.Guid
                                                            and Element = 'WEBDESCRIPTION'
                                                            and LanguageId = @lang), '') TranslatedWebDescription,
	                                            PTYPE.Product_type ProductType,
                                                isnull(PTYPE.CardSale, 'Y') CardSale,
                                                isnull(pdf.Id, -1) as DisplayGroupId, 
                                                pdf.DisplayGroup,
	                                            isnull(PRD1.price,0) Price,
	                                            isnull(tax.tax_percentage, 0) TaxPercentage,
	                                            isnull(prd1.CategoryId, -1) CategoryId,
                                                Category.name CategoryName,
	                                            prd1.ButtonColor ButtonColor,
	                                            prd1.TextColor TextColor,
	                                            prd1.DisplayInPOS DisplayInPOS,
                                                case when prd1.AutoGenerateCardNumber = 'Y' then 'Y' else 'N' end AutoGenerateCardNumber,
                                                isnull(prd1.MinimumQuantity,0) ProductMinQuantity,
                                                isnull(prd1.CardCount,0) ProductMaxQuantity,
                                                isnull(prd1.QuantityPrompt,'N') QuantityPrompt,
                                                isnull(prd1.face_value,0) FaceValue,
	                                            CP.ChildProductId ComboProductId,
	                                            isnull(CP.Quantity,0) ComboProductQuantity,
	                                            C.Name ComboCategoryName,
	                                            CP.CategoryId ComboCategoryId,
                                                ModSet.ModifierSetId ModifierSetId, 
	                                            ModSet.SetName ModifierSetName,
                                                ModSet.ParentModifierSetId ParentModifierSetId,
	                                            PrdMod.AutoShowInPos ModifierAutoShowInPOS,
	                                            ISNULL(ModSetDet.ModifierProductId, -1) ModifierChildProductIds,
                                                ISNULL(ModSetDet.price, -1) ModifierChildProductPrice,
                                                ISNULL(ModSet.MinQuantity,0) MinQuantity,
                                                ISNULL(ModSet.MaxQuantity,999) MaxQuantity,
                                                ISNULL(ModSet.FreeQuantity,0) FreeQuantity,
                                                PRD1.ImageFileName ProductImage,
                                                case when PRD1.InvokeCustomerRegistration = 1 then 'true' else 'false' end InvokeCustomerRegistration,
                                                case when PRD1.TrxHeaderRemarksMandatory = 1 then 'Y' else 'N' end TrxHeaderRemarksMandatory,
                                                ISNULL(PRD1.TrxRemarksMandatory, 'N') TrxRemarksMandatory,
                                                case when PRD1.TaxInclusivePrice = 'Y' then 'true' else 'false' end IsTaxInclusive,
                                                ISNULL(PRD1.WaiverSetId, -1) WaiverSetId,
                                                ISNULL(PRD1.AttractionMasterScheduleId, -1) AttractionMasterScheduleId,
                                                ISNULL(PRD1.IsGroupMeal,'N') IsGroupMeal,
                                                ISNULL(PRD1.AllowPriceOverride,'N') AllowPriceOverride 
                                            FROM POSMachines POSM,
	                                            PRODUCTS PRD1 
                                            INNER JOIN ProductsDisplayGroup pd on pd.ProductId = PRD1.product_id 
                                            LEFT OUTER JOIN product invp on invp.ManualProductId = PRD1.product_id
                                            LEFT OUTER JOIN ProductDisplayGroupFormat pdf on pdf.Id = pd.DisplayGroupId 
	                                        LEFT OUTER JOIN tax on tax.tax_id = PRD1.tax_id
	                                        LEFT OUTER JOIN Category on PRD1.categoryId = Category.CategoryId
	                                        LEFT OUTER JOIN ComboProduct CP on CP.Product_Id = PRD1.product_id and ISNULL(CP.IsActive, 1) = 1
                                            LEFT OUTER JOIN Category C on C.CategoryId = CP.CategoryId 
	                                        LEFT OUTER JOIN ProductModifiers PrdMod on PrdMod.ProductId = PRD1.product_id
	                                        LEFT OUTER JOIN ModifierSet ModSet on ModSet.ModifierSetId = PrdMod.ModifierSetId
					                        LEFT OUTER JOIN ModifierSetDetails ModSetDet on ModSetDet.ModifierSetId = ModSet.ModifierSetId,
	                                            PRODUCT_TYPE PTYPE
                                            WHERE (PRD1.product_id = case when @productId = -1 then PRD1.product_id else @productId end)
                                            AND (POSM.IPAddress = @tabletIPAddress OR POSM.Computer_Name = @tabletIPAddress)
                                            AND ISNULL(invp.IsSellable,'Y') = 'Y'
                                            AND (POSM.site_id is null or POSM.site_id = @siteId or @siteId = -1) 
                                            AND (PRD1.site_id is null or PRD1.site_id = @siteId or @siteId = -1) 
                                            AND (PRD1.ExpiryDate is null or cast(DATEADD(SECOND, @offSetDuration, PRD1.ExpiryDate) as date) > @today ) 
                                            AND (PRD1.StartDate is null or cast(DATEADD(SECOND, @offSetDuration, PRD1.StartDate)  as date)  <= @today )
                                            AND PRD1.product_type_id = PTYPE.product_type_id
                                            AND ltrim(PTYPE.product_type) != @productTypeExclude
                                            AND Pdf.Id is not null
                                            --AND ltrim(PRD1.display_group) is not null
                                            --AND PTYPE.cardSale !='Y' --Modification to exclude cardsale product types
                                            AND PRD1.active_flag='Y'
                                            --AND ltrim(PRD1.display_group) != '' --version 1.02
                                            " + StrDisplaygroupFilter + @"
                                            AND (PRD1.POSTypeId = POSM.POSTypeId or PRD1.POSTypeId is null) --version 1.02
                                            AND (not exists (select 1
                                                                from ProductCalendar pc
                                                            where pc.product_id = PRD1.product_id)
                                                or exists (select 1 
                                                                from (select top 1 date, day,
                                                                            case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort,
                                                                            isnull(FromTime, @nowHour) FromTime, isnull(ToTime, @nowHour) ToTime, ShowHide                     
                                                                        from ProductCalendar pc
                                                                    where pc.product_id = PRD1.product_id
                                                                        and (cast(DATEADD(SECOND, @offSetDuration, date)  as date) = @today 
                                                                        or Day = @DayNumber 
                                                                        or Day = @weekDay 
                                                                        or Day = -1) 
                                                                    order by 1 desc, 2 desc, 3) inView
                                                                    where (ShowHide = 'Y'
                                                                        and (@nowHour >= FromTime and @nowHour <= case ToTime when 0 then 24 else ToTime end))
                                                                        or (ShowHide = 'N'
                                                                        and (@nowHour < FromTime or @nowHour > case ToTime when 0 then 24 else ToTime end))))
                                        ORDER BY isnull(prd1.sort_order, 1000), ProductId, ComboCategoryId, PrdMOd.id, ModifierChildProductIds, isnull(cp.sortorder, 1000)";

            DateTime serverTime = productsFilterParams.DateOfPurchase;

            SqlParameter[] queryParams = new SqlParameter[12];
            queryParams[0] = new SqlParameter("@tabletIPAddress", productsFilterParams.MachineName);
            queryParams[1] = new SqlParameter("@today", serverTime.Date);
            queryParams[2] = new SqlParameter("@nowHour", serverTime.Hour + serverTime.Minute / 100.0 + serverTime.Second / 10000.0);
            queryParams[3] = new SqlParameter("@DayNumber", serverTime.Day + 1000); // day of month stored as 1000 + day of month
            int dayofweek = -1;
            switch (serverTime.DayOfWeek)
            {
                case DayOfWeek.Sunday: dayofweek = 0; break;
                case DayOfWeek.Monday: dayofweek = 1; break;
                case DayOfWeek.Tuesday: dayofweek = 2; break;
                case DayOfWeek.Wednesday: dayofweek = 3; break;
                case DayOfWeek.Thursday: dayofweek = 4; break;
                case DayOfWeek.Friday: dayofweek = 5; break;
                case DayOfWeek.Saturday: dayofweek = 6; break;
                default: break;
            }
            queryParams[4] = new SqlParameter("@weekDay", dayofweek);
            queryParams[5] = new SqlParameter("@siteId", productsFilterParams.SiteId);
            queryParams[6] = new SqlParameter("@lang", productsFilterParams.LanguageId);
            queryParams[7] = new SqlParameter("@productTypeExclude", productsFilterParams.ProductTypeExclude);
            queryParams[8] = new SqlParameter("@strDisplaygroupFilter", StrDisplaygroupFilter);
            queryParams[9] = new SqlParameter("@roleId", userRoleId.ToString());
            queryParams[10] = new SqlParameter("@offSetDuration", offSetDuration);
            queryParams[11] = new SqlParameter("@productId", productsFilterParams.ProductId);


            DataTable foodAndBeverageTable = dataAccessHandler.executeSelectQuery(selectProductQuery, queryParams);

            int currProductId = -1;
            ProductsWithComboDetails currProduct = null;
            int prevModifierSetId = -1;
            ProductsModifierSetStruct parentModifierSet;
            for (int i = 0; i < foodAndBeverageTable.Rows.Count; i++)
            {
                if (currProductId != Convert.ToInt32(foodAndBeverageTable.Rows[i]["ProductId"]))
                {
                    currProductId = Convert.ToInt32(foodAndBeverageTable.Rows[i]["ProductId"]);
                    bool productFoundFlag = false;
                    for (int j = 0; (j < foodAndBeverageProductList.Count) && (productFoundFlag = false); j++)
                    {
                        if (currProductId == foodAndBeverageProductList[j].ProductId)
                        {
                            currProduct = foodAndBeverageProductList[j];
                            productFoundFlag = true;
                        }
                    }
                    if (productFoundFlag == false)
                    {
                        currProduct = new ProductsWithComboDetails(Convert.ToInt32(foodAndBeverageTable.Rows[i]["ProductId"]),
                                                                foodAndBeverageTable.Rows[i]["ProductName"].ToString(), foodAndBeverageTable.Rows[i]["ProductDescription"].ToString(),
                                                                foodAndBeverageTable.Rows[i]["ProductType"].ToString(), foodAndBeverageTable.Rows[i]["DisplayGroup"].ToString(),
                                                                Convert.ToDouble(foodAndBeverageTable.Rows[i]["Price"]), Convert.ToDouble(foodAndBeverageTable.Rows[i]["TaxPercentage"]),
                                                                Convert.ToInt32(foodAndBeverageTable.Rows[i]["CategoryId"]), foodAndBeverageTable.Rows[i]["CategoryName"].ToString(),
                                                                ColorCodes.GetColorCode(foodAndBeverageTable.Rows[i]["ButtonColor"].ToString()), ColorCodes.GetColorCode(foodAndBeverageTable.Rows[i]["TextColor"].ToString()),
                                                                foodAndBeverageTable.Rows[i]["DisplayInPOS"].ToString(),
                                                                foodAndBeverageTable.Rows[i]["CardSale"].ToString(), foodAndBeverageTable.Rows[i]["ProductImage"].ToString(),
                                                                foodAndBeverageTable.Rows[i]["TranslatedProductName"].ToString(), foodAndBeverageTable.Rows[i]["TranslatedProductDescription"].ToString(), //added translation specific fields
                                                                Convert.ToInt32(foodAndBeverageTable.Rows[i]["ProductMinQuantity"]), //added Product Min Quantity specific fields
                                                                Convert.ToInt32(foodAndBeverageTable.Rows[i]["ProductMaxQuantity"]),
                                                                foodAndBeverageTable.Rows[i]["QuantityPrompt"].ToString(),
                                                                Convert.ToDouble(foodAndBeverageTable.Rows[i]["FaceValue"]), //added Product QuantityPrompt fields
                                                                Convert.ToInt32(foodAndBeverageTable.Rows[i]["DisplayGroupId"]),
                                                                foodAndBeverageTable.Rows[i]["AutoGenerateCardNumber"].ToString(),
                                                                Convert.ToBoolean(foodAndBeverageTable.Rows[i]["InvokeCustomerRegistration"]),//added displaygroupid,autoGenaratecardNumber and InvokeCustomerRegistration fields
                                                                foodAndBeverageTable.Rows[i]["TrxHeaderRemarksMandatory"].ToString(),
                                                                foodAndBeverageTable.Rows[i]["TrxRemarksMandatory"].ToString(),
                                                                foodAndBeverageTable.Rows[i]["webDescription"].ToString(),
                                                                foodAndBeverageTable.Rows[i]["TranslatedWebDescription"].ToString(),
                                                                foodAndBeverageTable.Rows[i]["Barcodes"].ToString(),//added TrxHeaderRemarksMandatory and TrxRemarksMandatory fields
                                                                Convert.ToBoolean(foodAndBeverageTable.Rows[i]["IsTaxInclusive"]),
                                                                Convert.ToInt32(foodAndBeverageTable.Rows[i]["WaiverSetId"]),
                                                                Convert.ToInt32(foodAndBeverageTable.Rows[i]["AttractionMasterScheduleId"]),
                                                                foodAndBeverageTable.Rows[i]["IsGroupMeal"].ToString().Equals("Y")? true:false,
                                                                foodAndBeverageTable.Rows[i]["AllowPriceOverride"].ToString().Equals("Y") ? true : false, productsFilterParams.SiteId
                                                                );


                        foodAndBeverageProductList.Add(currProduct);
                    }
                }
                if ((foodAndBeverageTable.Rows[i]["ComboProductId"] != DBNull.Value) && (currProduct != null))
                    currProduct.AddComboProduct(Convert.ToInt32(foodAndBeverageTable.Rows[i]["ComboProductId"]), foodAndBeverageTable.Rows[i]["ComboCategoryName"].ToString(), Convert.ToInt32(foodAndBeverageTable.Rows[i]["ComboProductQuantity"]), ProductsWithComboDetails.COMBOPRODUCT); //Added product name as parameter to AddComboProduct
                if ((foodAndBeverageTable.Rows[i]["ComboCategoryId"] != DBNull.Value) && (currProduct != null))
                    currProduct.AddComboProduct(Convert.ToInt32(foodAndBeverageTable.Rows[i]["ComboCategoryId"]), foodAndBeverageTable.Rows[i]["ComboCategoryName"].ToString(), Convert.ToInt32(foodAndBeverageTable.Rows[i]["ComboProductQuantity"]), ProductsWithComboDetails.COMBOCATEGORY); //Added product name as parameter to AddComboProduct. Category Name is passed

                if (foodAndBeverageTable.Rows[i]["ModifierSetId"] != DBNull.Value)
                {
                    parentModifierSet = null;
                    if ((foodAndBeverageTable.Rows[i]["ParentModifierSetId"] != DBNull.Value) && (prevModifierSetId != Convert.ToInt32(foodAndBeverageTable.Rows[i]["ModifierSetId"])))
                    {
                        parentModifierSet = productsBL.GetModifierSet(Convert.ToInt32(foodAndBeverageTable.Rows[i]["ParentModifierSetId"]));
                    }

                    currProduct.AddModifierProduct(Convert.ToInt32(foodAndBeverageTable.Rows[i]["ModifierSetId"]),
                                                    foodAndBeverageTable.Rows[i]["ModifierSetName"].ToString(),
                                                    foodAndBeverageTable.Rows[i]["ModifierAutoShowInPOS"].ToString(),
                                                    Convert.ToInt32(foodAndBeverageTable.Rows[i]["ModifierChildProductIds"]),
                                                    foodAndBeverageTable.Rows[i]["ModifierChildProductPrice"].ToString(),
                                                    Convert.ToInt32(foodAndBeverageTable.Rows[i]["MinQuantity"]),
                                                    Convert.ToInt32(foodAndBeverageTable.Rows[i]["MaxQuantity"]),
                                                    Convert.ToInt32(foodAndBeverageTable.Rows[i]["FreeQuantity"]),
                                                    parentModifierSet);
                    prevModifierSetId = Convert.ToInt32(foodAndBeverageTable.Rows[i]["ModifierSetId"]);
                }

            }

            log.Debug("Ends-GetProductList(productsFilterParams) method.");

            if (String.IsNullOrEmpty(productsFilterParams.DeviceType))
            {
                List<ProductsWithComboDetails> foodAndBeverageProductList1 = GetProductListByDisplayGroup(foodAndBeverageProductList, productsFilterParams);
                return foodAndBeverageProductList1;
            }
            else
                return foodAndBeverageProductList;
        }

        /// <summary>
        /// This method is used to Filter the products based on the Display Groups from given the List of Products
        /// </summary>
        /// <param name="inProductsWithComboDetailsList">List of ProductsWithComboDetails</param>
        /// <param name="productsFilterParams">ProductsFilterParams</param>
        /// <returns>List<ProductsWithComboDetails/></returns>
        private List<ProductsWithComboDetails> GetProductListByDisplayGroup(List<ProductsWithComboDetails> inProductsWithComboDetailsList,
                                                                                ProductsFilterParams productsFilterParams)
        {

            log.LogMethodEntry(inProductsWithComboDetailsList, productsFilterParams);

            string COMBOPRODUCT = "ComboProduct";
            string COMBOCATEGORY = "ComboCategory";

            List<ProductsWithComboDetails> lstProductsMain = new List<ProductsWithComboDetails>();
            List<ProductsWithComboDetails> lstProducts = new List<ProductsWithComboDetails>();
            bool IsExcluded = false;
            try
            {
                List<int> childProducts = new List<int>();

                foreach (ProductsWithComboDetails currproductDetails in inProductsWithComboDetailsList)
                {
                    IsExcluded = false;
                    if (currproductDetails.ProductType == "COMBO")
                    {
                        if (currproductDetails.ComboProducts != null)
                        {
                            List<PurchasedProductsStruct> comboProducts = new List<PurchasedProductsStruct>();
                            foreach (PurchasedProductsStruct currComboProduct in currproductDetails.ComboProducts)
                            {
                                currComboProduct.WaiverSetId = -1;
                                currComboProduct.AttractionMasterScheduleId = -1;
                                currComboProduct.CategoryId = -1;

                                if (currComboProduct.ProductType == COMBOPRODUCT)
                                {
                                    IEnumerable<ProductsWithComboDetails> comboProduct = inProductsWithComboDetailsList.Where(c => c.ProductId.Equals(currComboProduct.ProductId) && !c.ProductType.Equals("ComboProduct"));
                                    if (comboProduct.Count() == 1)
                                    {
                                        currComboProduct.ProductName = comboProduct.ToList()[0].ProductName;
                                        currComboProduct.Price = comboProduct.ToList()[0].Price;
                                        currComboProduct.WaiverSetId = comboProduct.ToList()[0].WaiverSetId;
                                        currComboProduct.AttractionMasterScheduleId = comboProduct.ToList()[0].AttractionMasterScheduleId;
                                        currComboProduct.AutoGenerateCardNumber = comboProduct.ToList()[0].AutoGenerateCardNumber;
                                        currComboProduct.MaxQuantity = comboProduct.ToList()[0].MaxQuantity;
                                        currComboProduct.MinQuantity = comboProduct.ToList()[0].MinQuantity;
                                        currComboProduct.ComboProductType = comboProduct.ToList()[0].ProductType;
                                        currComboProduct.SiteId = productsFilterParams.SiteId;
                                        childProducts.Add(currComboProduct.ProductId);
                                    }
                                }
                                else if (currComboProduct.ProductType == COMBOCATEGORY)
                                {
                                    IEnumerable<ProductsWithComboDetails> categoryProducts = inProductsWithComboDetailsList.Where(c => c.CategoryId.Equals(currComboProduct.ProductId));
                                    foreach (ProductsWithComboDetails currCategoryProduct in categoryProducts)
                                    {
                                        PurchasedProductsStruct purchasedProductsStruct = new PurchasedProductsStruct();
                                        purchasedProductsStruct.ProductId = currCategoryProduct.ProductId;
                                        purchasedProductsStruct.ProductName = currCategoryProduct.ProductName;
                                        purchasedProductsStruct.ProductDescription = currCategoryProduct.ProductDescription;
                                        purchasedProductsStruct.ProductType = COMBOCATEGORY;
                                        purchasedProductsStruct.Price = currCategoryProduct.Price;
                                        purchasedProductsStruct.ProductQuantity = currComboProduct.ProductQuantity;
                                        purchasedProductsStruct.TaxAmount = 0;

                                        purchasedProductsStruct.WaiverSetId = currCategoryProduct.WaiverSetId;
                                        purchasedProductsStruct.AttractionMasterScheduleId = currCategoryProduct.AttractionMasterScheduleId;
                                        purchasedProductsStruct.AutoGenerateCardNumber = currCategoryProduct.AutoGenerateCardNumber;
                                        purchasedProductsStruct.MaxQuantity = currCategoryProduct.MaxQuantity;
                                        purchasedProductsStruct.MinQuantity = currCategoryProduct.MinQuantity;
                                        purchasedProductsStruct.ComboProductType = currCategoryProduct.ProductType;
                                        purchasedProductsStruct.CategoryId = currCategoryProduct.CategoryId;
                                        purchasedProductsStruct.SiteId = productsFilterParams.SiteId;
                                        comboProducts.Add(purchasedProductsStruct);
                                        childProducts.Add(currCategoryProduct.ProductId);
                                    }

                                    
                                }
                            }
                            if(comboProducts.Any())
                                currproductDetails.ComboProducts = comboProducts.ToList();
                        }
                    }


                    // Exclude Combo without child Items
                    if (childProducts.Contains(currproductDetails.ProductId) == true ||
                       (currproductDetails.ProductType == "COMBO" && (currproductDetails.ComboProducts == null || currproductDetails.ComboProducts.Count() == 0)) ||
                       (productsFilterParams.ProductDisplayGroups.Count > 0 && productsFilterParams.ProductDisplayGroups.Contains(currproductDetails.DisplayGroup, StringComparer.CurrentCultureIgnoreCase) == false))
                    {
                        IsExcluded = true;
                    }

                    if (IsExcluded == false)
                    {
                        lstProducts.Add(currproductDetails);
                    }

                }

                // exclude products which are not part of combo from main list 
                foreach (ProductsWithComboDetails currproductDetails in lstProducts)
                {
                    if (childProducts.Contains(currproductDetails.ProductId) == false)
                    {
                        lstProductsMain.Add(currproductDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }

            log.LogMethodExit(lstProductsMain);
            return lstProductsMain;

        }


        /// <summary>
        /// GetPromotionalPriceUpdate method
        /// </summary>
        /// <param name="inProdList">List ProductsWithComboDetails</param>
        /// <param name="productsFilterParams">ProductsFilterParams</param>
        /// <returns>returns List ProductsWithComboDetails with the promotional price udpated</returns>
        public List<ProductsWithComboDetails> GetPromotionalPriceUpdate(List<ProductsWithComboDetails> inProdList, ProductsFilterParams productsFilterParams)
        {
            log.LogMethodEntry(null);
            using (Utilities parafaitUtility = new Utilities(connstring))
            {
                parafaitUtility.ParafaitEnv.Initialize();

                parafaitUtility.ParafaitEnv.IsCorporate = true;

                Semnox.Parafait.Transaction.Transaction trx = new Semnox.Parafait.Transaction.Transaction(parafaitUtility);


                log.Debug("Starts-GetPromotionalPriceUpdate(inProdList)  method getProductPromotionPrice .");

                // Agent fix for user price list price
                if (!String.IsNullOrEmpty(productsFilterParams.LoginId) && productsFilterParams.LoginId != "External POS")
                {
                    UsersDTO userDTO = new Users(parafaitUtility.ExecutionContext, productsFilterParams.LoginId).UserDTO;
                    if (userDTO.UserId != -1)
                    {
                        userRoleId = userDTO.RoleId;
                        parafaitUtility.ParafaitEnv.RoleId = userRoleId;
                    }
                }
                // Agent fix for user price list price
                bool isExcluded = false;
                foreach (ProductsWithComboDetails currprod in inProdList)
                {

                    DataRow prodRow = trx.getProductDetails(currprod.ProductId);
                    //Card PrimaryCard = null;
                    CustomerDTO customerDTO = null;
                    double price = Convert.ToDouble(prodRow["price"]) - currprod.FaceValue;
                    int promotionId = Promotions.getProductPromotionPrice(customerDTO, currprod.ProductId, prodRow["CategoryId"], prodRow["TaxInclusivePrice"].ToString(), currprod.TaxPercentage, ref price, parafaitUtility, productsFilterParams.DateOfPurchase);
                    //If tax inclusive, add tax percentage to the price
                    if (prodRow["TaxInclusivePrice"].ToString().Equals("Y") && promotionId > -1)
                    {
                        price = price * (1.0 + currprod.TaxPercentage / 100.0);
                    }

                    currprod.Price = price + currprod.FaceValue;

                    if (currprod.ComboProducts != null)
                    {
                        foreach (PurchasedProductsStruct childprod in currprod.ComboProducts)
                        {
                            isExcluded = false;
                            if ((!String.IsNullOrEmpty(productsFilterParams.DeviceType)) && (childprod.ProductType == "ComboCategory"))
                            {
                                isExcluded = true;
                            }

                            if (isExcluded == false)
                            {
                                prodRow = trx.getProductDetails(childprod.ProductId);
                                price = Convert.ToDouble(prodRow["price"]) - childprod.FaceValue;
                                promotionId = Promotions.getProductPromotionPrice(customerDTO, childprod.ProductId, prodRow["CategoryId"], prodRow["TaxInclusivePrice"].ToString(), childprod.TaxPercentage, ref price, parafaitUtility, productsFilterParams.DateOfPurchase);
                                //If tax inclusive, add tax percentage to the price
                                if (prodRow["TaxInclusivePrice"].ToString().Equals("Y") && promotionId > -1)
                                {
                                    price = price * (1.0 + currprod.TaxPercentage / 100.0);
                                }
                                childprod.Price = price + currprod.FaceValue;
                            }
                        }
                    }
                }

            }

            log.LogMethodExit(inProdList);

            return inProdList;
        }
    }
}
