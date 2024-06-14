/********************************************************************************************
 * Project Name - Product Data Handler
 * Description  - Data handler of the product data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        24-May-2016   Jeevan         Created  
 *1.00        14-Jul-2016   Raghuveera     Modified  
 *2.40        05-Dec-2018   Raghuveera     Created new method GetProductByTypeList 
 *2.50        10-Jan-2019   Jagan Mohana   Created new method GetProductTypeList 
 *2.60        08-Feb-2019  Akshay G       Created new methods InsertProducts(), UpdateProducts() and GetSQLParameters(productsDTO, userId, siteId)
 *                                        Made changes for few attributes to nullability in method GetProductDTO(DataRow productDataRow)
 *            18-Feb-2019   Akshay G       Added new methods GetAllProductDetails() and GetProductDetailsDTO()
 *            01-Apr-2019   Akshay G       modified ActiveFlag DataType( from string to bool) and also handled in this dataHandler
 *2.60        15-Mar-2019   Nitin Pai      Added new search for PRODUCT_TYPE_NAME_LIST 
 *2.60        10-Apr-2019   Archana        Include/Exclude for redeemable products changes 
 *            16-Apr-2019   Indrajeet K    Added HsnSacCode to DBSearchParameters & Added HsnSacCode BuildProductSearchParametersList() and
 *                                         Added HSNSACCODE searchParameter to GetAllProductList()
              08-May-2019   Nitin Pai      Added new method to search product by filter params
 *2.70        16-Apr-2019   Guru S A       Booking Phase2 changes
              29-May-2019   Jagan Mohana   Code merge from Development to WebManagementStudio
 *            25-Jun-2019   Mathew Ninan   GetCheckInSlabPrice method added 
              28-Jun-2019   Jagan Mohana   Created UpdateDuplicateProductDetails()
*             29-Jun-2019   Indrajeet K    Created DeleteProducts() method for Hard deletion. 
*             01-Jul-2019   Indrajeet K    Created GetProductsListFromDisplayGroup() & GetProductsDTOForViator() method for Viator             
*             17-Jul-2019   Akshay G       modified selectProductsQuery in GetAllProductList() method
*             26-jul-2019   Indrajeet K    Modified the Query under GetProductsListFromDisplayGroup() Method and removed GetProductsDTOForViator() method
 *2.70        30-Jul-2019   Jeevan         Modified Included ComboProductId to BookingPackageProduct class as part of booking phase2 changes             
 *            06-Aug-2019   Nitin Pai      Added search by category id
 *            05-Sept-2019  Jagan Mohan    Added the new method GetProductsDTOCount()
 *2.70.2      12-Oct-2019   Akshay G       ClubSpeed enhancement Changes - Added ExternalSystemReference and modified DBSearchParameters 
 *2.70.2      10-Dec-2019   Jinto Thomas   Removed siteid from update query
 *2.70.2      31-Dec-2019   Akshay G       Added DBSearchParameters
 *2.70.3      30-Jan-2020   Archana        Modified getSplitProductList() method to skip the upsell products
 *2.80        20-Mar-2020   Indrajeet K    Added - ExternalSystemReference as searchParameters in GetProductsListFromDisplayGroup() method.
 *2.80.0      26-May-2020   Dakshakh       Added - LoadToSingleCard Attribute as a part of CardCount enhancement
 *2.80.0      09-Jun-2020   Jinto Thomas   Enable Active flag for Comboproduct data
 *2.90.0      14-Aug-2020   Girish kundar  Modified: Product price when -1 it should save as -1
 *2.100.0     15-Oct-2020   Deeksha        Modified: Fix for the Guid Value during product save
 *2.100.0     26-Oct-2020   Girish Kundar  Added - LinkChildCard field and ProductName search for center edge  enhancement
 *2.110.0     15-Dec-2020   Deeksha        Modified : Web Inventory/POS UI re-design
 *2.120.0     01-Mar-2020   Girish Kundar  Modified : Radian module changes - Added NotificationTagId, IssueNotificationdevice fields
 *2.120.0     01-Apr-2021   Dakshakh raj   modified : Enabling variable hours for Passtech Lockers and enabling function to extend the time -Added EnableVariableLockerHours field
 *2.140.0     14-Sep-2021   Prajwal S      modified : Added SearchDescription and IsRecommended fields for F&B requirements.
 *2.140.0     06-Dec-2021   Fiona          modified : Issue fix in  GetRewardProductDTOList() and modified GetProductsListFromDisplayGroup method.
 *2.140.0     02-Feb-2022   Fiona          Added masterEntityId search param
 *2.130.04    17-Feb-2022   Nitin Pai      Added Product offset value to calculate against Product Calendar for website and app
 *2.130.05    17-Mar-2022   Nitin Pai      Pass @today as datetime. @today is already in offset, no need to add again. 
 *2.140.2     04-May-2022   Ashish Sreejith  Modifiled : Added advancePercentage field in GetBookingProducts() select query.
 *2.150.0     06-May-2022   Girish Kundar  Modified : Added new columns  MaximumQuantity & PauseType,CustomerProfilingGroupId to Products 
 *2.160.0     02-May-2022   Guru S A       Auto Service Charges and Gratuity changes
 *2.130.12    22-Dec-2022   Ashish Sreejith  Modified: Added webDescription field in GetbookingProducts() select query.
 *********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;
using Microsoft.SqlServer.Server;
using System.Globalization;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Product Data Handler - Handles insert, update and select of product data objects
    /// </summary>
    public class ProductsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<ProductsDTO.SearchByProductParameters, string> DBSearchParameters = new Dictionary<ProductsDTO.SearchByProductParameters, string>
            {
                {ProductsDTO.SearchByProductParameters.PRODUCT_ID, "p.product_id"},
                {ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_ID,  "p.product_type_id"},
                {ProductsDTO.SearchByProductParameters.ISACTIVE, "p.active_flag"},
                {ProductsDTO.SearchByProductParameters.SITEID, "p.site_id"},
                {ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST, "p.product_id"},
                {ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME,  "pt.product_type"},
                {ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST,  "pt.product_type"},
                {ProductsDTO.SearchByProductParameters.PRICE,  "p.price"},
                {ProductsDTO.SearchByProductParameters.DISPLAY_IN_POS,  "p.DisplayInPOS"},
                //{ProductsDTO.SearchByProductParameters.FACILITY_MAP_ID,  "p.FacilityMapId"},
                {ProductsDTO.SearchByProductParameters.CHECKIN_FACILITY_ID,  "p.CheckinFacilityId"},
                {ProductsDTO.SearchByProductParameters.HSNSACCODE, "p.HsnSacCode" },
                {ProductsDTO.SearchByProductParameters.HAS_MODIFIER, "Modifier" },
                {ProductsDTO.SearchByProductParameters.PRODUCT_DISPLAY_GROUP_FORMAT_ID, "pdg.DisplayGroupId" },
                {ProductsDTO.SearchByProductParameters.PRODUCT_DISPLAY_GROUP_FORMAT_ID_LIST, "pdg.DisplayGroupId" },
                {ProductsDTO.SearchByProductParameters.CATEGORY_ID, "p.categoryId" },
                {ProductsDTO.SearchByProductParameters.PRODUCT_NAME, "p.product_name" },
                {ProductsDTO.SearchByProductParameters.PRODUCT_EXACT_NAME, "p.product_name" },
                {ProductsDTO.SearchByProductParameters.POS_TYPE_ID, "p.POSTypeId" },
                {ProductsDTO.SearchByProductParameters.DISPLAY_GROUP_NAME, "pdgf.DisplayGroup" },
                {ProductsDTO.SearchByProductParameters.IS_SELLABLE, "p.Sellable" },
                {ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE, "p.ExternalSystemReference" },
                {ProductsDTO.SearchByProductParameters.LAST_UPDATED_FROM_DATE, "p.last_updated_date" },
                {ProductsDTO.SearchByProductParameters.LAST_UPDATED_TO_DATE, "p.last_updated_date" },
                {ProductsDTO.SearchByProductParameters.CUSTOM_DATA_SET_IS_SET, "p.CustomDataSetId" },
                {ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE_IS_SET, "p.ExternalSystemReference" },
                {ProductsDTO.SearchByProductParameters.PRODUCTS_ENTITY_LAST_UPDATED_FROM_DATE, "p.last_updated_date" },
                {ProductsDTO.SearchByProductParameters.TRX_ONLY_PRODUCT_PURCHASE_DATE, "p.StartDate" },
                {ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE_LIST, "p.ExternalSystemReference"},
                {ProductsDTO.SearchByProductParameters.PRODUCT_GUID, "p.Guid"},
                {ProductsDTO.SearchByProductParameters.IS_SUBSCRIPTION_PRODUCT, "p.product_id"},
                {ProductsDTO.SearchByProductParameters.IS_A_SUBSCRIPTION_PRODUCT_OR_HAS_ACTIVE_SUBSCRIPTION_CHILD, "p.product_id"},
                {ProductsDTO.SearchByProductParameters.SHORT_NAME, "p.SearchDescription"},
                {ProductsDTO.SearchByProductParameters.ISRECOMMENDED, "p.IsRecommended"},
                {ProductsDTO.SearchByProductParameters.MINIMUM_QUANTITY, "p.MinimumQuantity"},
                {ProductsDTO.SearchByProductParameters.TAX_ID, "p.tax_id"},
                {ProductsDTO.SearchByProductParameters.WAIVER_SET_ID, "p.WaiverSetId"},
                {ProductsDTO.SearchByProductParameters.MASTER_ENTITY_ID, "p.masterEntityId"},
                {ProductsDTO.SearchByProductParameters.SERVICE_CHARGE_IS_APPLICABLE, "p.ServiceChargeIsApplicable"},
                {ProductsDTO.SearchByProductParameters.GRATUITY_IS_APPLICABLE, "p.GratuityIsApplicable"},
                {ProductsDTO.SearchByProductParameters.CUSTOM_DATA_SET_ID, "p.CustomDataSetId"},
                {ProductsDTO.SearchByProductParameters.CUSTOM_DATA_SET_ID_LIST, "p.CustomDataSetId"},
            };


        int userRoleId = -1;
        string StrDisplaygroupFilter = "";
        string connstring;
        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS ProductsType;
                                            MERGE INTO Products tbl
                                            USING @ProductsList AS src
                                            ON src.product_id = tbl.product_id
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            product_name = src.product_name,
                                            description = src.description,
                                            active_flag = src.active_flag,
                                            product_type_id = src.product_type_id,
                                            price = src.price,
                                            credits = src.credits,
                                            courtesy = src.courtesy,
                                            bonus = src.bonus,
                                            time = src.time,
                                            sort_order = src.sort_order,
                                            tax_id = src.tax_id,
                                            tickets = src.tickets,
                                            face_value = src.face_value,
                                            display_group = src.display_group,
                                            ticket_allowed = src.ticket_allowed,
                                            vip_card = src.vip_card,
                                            last_updated_date = GETDATE(),
                                            last_updated_user = src.last_updated_user,
                                            InternetKey = src.InternetKey,
                                            TaxInclusivePrice = src.TaxInclusivePrice,
                                            InventoryProductCode = src.InventoryProductCode,
                                            ExpiryDate = src.ExpiryDate,
                                            AvailableUnits = src.AvailableUnits,
                                            AutoCheckOut = src.AutoCheckOut,
                                            CheckInFacilityId = src.CheckInFacilityId,
                                            MaxCheckOutAmount = src.MaxCheckOutAmount,
                                            POSTypeId = src.POSTypeId,
                                            CustomDataSetId = src.CustomDataSetId,
                                            CardTypeId = src.CardTypeId,
                                            TrxRemarksMandatory = src.TrxRemarksMandatory,
                                            CategoryId = src.CategoryId,
                                            OverridePrintTemplateId = src.OverridePrintTemplateId,
                                            StartDate = src.StartDate,
                                            ButtonColor = src.ButtonColor,
                                            AutoGenerateCardNumber = src.AutoGenerateCardNumber,
                                            QuantityPrompt = src.QuantityPrompt,
                                            OnlyForVIP = src.OnlyForVIP,
                                            AllowPriceOverride = src.AllowPriceOverride,
                                            RegisteredCustomerOnly = src.RegisteredCustomerOnly,
                                            ManagerApprovalRequired = src.ManagerApprovalRequired,
                                            MinimumUserPrice = src.MinimumUserPrice,
                                            TextColor = src.TextColor,
                                            Font = src.Font,
                                            VerifiedCustomerOnly = src.VerifiedCustomerOnly,
                                            Modifier = src.Modifier,
                                            MinimumQuantity = src.MinimumQuantity,
                                            DisplayInPOS = src.DisplayInPOS,
                                            TrxHeaderRemarksMandatory = src.TrxHeaderRemarksMandatory,
                                            CardCount = src.CardCount,
                                            ImageFileName = src.ImageFileName,
                                            --AttractionMasterScheduleId = src.AttractionMasterScheduleId,
                                            AdvancePercentage = src.AdvancePercentage,
                                            AdvanceAmount = src.AdvanceAmount,
                                            EmailTemplateId = src.EmailTemplateId,
                                            MaximumTime = src.MaximumTime,
                                            MinimumTime = src.MinimumTime,
                                            CardValidFor = src.CardValidFor,
                                            AdditionalTaxInclusive = src.AdditionalTaxInclusive,
                                            AdditionalPrice = src.AdditionalPrice,
                                            AdditionalTaxId = src.AdditionalTaxId,
                                            MasterEntityId = src.MasterEntityId,
                                            WaiverRequired = src.WaiverRequired,
                                            SegmentCategoryId = src.SegmentCategoryId,
                                            CardExpiryDate = src.CardExpiryDate,
                                            InvokeCustomerRegistration = src.InvokeCustomerRegistration,
                                            ProductDisplayGroupFormatId = src.ProductDisplayGroupFormatId,
                                            MaxQtyPerDay = src.MaxQtyPerDay,
                                            HsnSacCode = src.HsnSacCode,
                                            WebDescription = src.WebDescription,
                                            OrderTypeId = src.OrderTypeId,
                                            ZoneId = src.ZoneId,
                                            LockerExpiryInHours = src.LockerExpiryInHours,
                                            LockerExpiryDate = src.LockerExpiryDate,
                                            WaiverSetId = src.WaiverSetId,
                                            isGroupMeal = src.isGroupMeal,
                                            MembershipID = src.MembershipID,
                                            ExternalSystemReference = src.ExternalSystemReference,
                                            LoadToSingleCard = src.LoadToSingleCard,
                                            EnableVariableLockerHours = src.EnableVariableLockerHours,
                                            LinkChildCard = src.LinkChildCard,
                                            LicenseType = src.LicenseType,
                                            IssueNotificationDevice = src.IssueNotificationDevice,
                                            NotificationTagProfileId = src.NotificationTagProfileId,
                                            ServiceCharge = src.ServiceCharge,
                                            PackingCharge = src.PackingCharge,
                                            SearchDescription = src.SearchDescription,
                                            IsRecommended = src.IsRecommended,
                                            ServiceChargeIsApplicable = src.ServiceChargeIsApplicable,
                                            ServiceChargePercentage = src.ServiceChargePercentage,
                                            GratuityIsApplicable = src.GratuityIsApplicable,
                                            GratuityPercentage = src.GratuityPercentage,
                                            MaximumQuantity = src.MaximumQuantity,
                                            PauseType = src.PauseType,
                                            CustomerProfilingGroupId = src.CustomerProfilingGroupId                                           
                                           --, FacilityMapId = src.FacilityMapId
                                            WHEN NOT MATCHED THEN INSERT (
                                            product_name,
                                            description,
                                            active_flag,
                                            product_type_id,
                                            price,
                                            credits,
                                            courtesy,
                                            bonus,
                                            time,
                                            sort_order,
                                            tax_id,
                                            tickets,
                                            face_value,
                                            display_group,
                                            ticket_allowed,
                                            vip_card,
                                            last_updated_date,
                                            last_updated_user,
                                            InternetKey,
                                            TaxInclusivePrice,
                                            InventoryProductCode,
                                            ExpiryDate,
                                            AvailableUnits,
                                            AutoCheckOut,
                                            CheckInFacilityId,
                                            MaxCheckOutAmount,
                                            POSTypeId,
                                            CustomDataSetId,
                                            CardTypeId,
                                            Guid,
                                            site_id,
                                            TrxRemarksMandatory,
                                            CategoryId,
                                            OverridePrintTemplateId,
                                            StartDate,
                                            ButtonColor,
                                            AutoGenerateCardNumber,
                                            QuantityPrompt,
                                            OnlyForVIP,
                                            AllowPriceOverride,
                                            RegisteredCustomerOnly,
                                            ManagerApprovalRequired,
                                            MinimumUserPrice,
                                            TextColor,
                                            Font,
                                            VerifiedCustomerOnly,
                                            Modifier,
                                            MinimumQuantity,
                                            DisplayInPOS,
                                            TrxHeaderRemarksMandatory,
                                            CardCount,
                                            ImageFileName,
                                            --AttractionMasterScheduleId,
                                            AdvancePercentage,
                                            AdvanceAmount,
                                            EmailTemplateId,
                                            MaximumTime,
                                            MinimumTime,
                                            CardValidFor,
                                            AdditionalTaxInclusive,
                                            AdditionalPrice,
                                            AdditionalTaxId,
                                            MasterEntityId,
                                            WaiverRequired,
                                            SegmentCategoryId,
                                            CardExpiryDate,
                                            InvokeCustomerRegistration,
                                            ProductDisplayGroupFormatId,
                                            MaxQtyPerDay,
                                            HsnSacCode,
                                            WebDescription,
                                            OrderTypeId,
                                            ZoneId,
                                            LockerExpiryInHours,
                                            LockerExpiryDate,
                                            WaiverSetId,
                                            isGroupMeal,
                                            MembershipID,
                                            CreatedBy,
                                            CreationDate,
                                            ExternalSystemReference,
                                            LoadToSingleCard,
                                            EnableVariableLockerHours,
                                            LinkChildCard,
                                            LicenseType,
                                            IssueNotificationDevice,
                                            NotificationTagProfileId,
                                            ServiceCharge,
                                            PackingCharge,
                                            SearchDescription,
                                            IsRecommended,
                                            ServiceChargeIsApplicable,
                                            ServiceChargePercentage,
                                            GratuityIsApplicable,
                                            GratuityPercentage,
                                            MaximumQuantity,
                                            PauseType,
                                            CustomerProfilingGroupId
                                             --, FacilityMapId
                                            )VALUES (
                                            src.product_name,
                                            src.description,
                                            src.active_flag,
                                            src.product_type_id,
                                            src.price,
                                            src.credits,
                                            src.courtesy,
                                            src.bonus,
                                            src.time,
                                            src.sort_order,
                                            src.tax_id,
                                            src.tickets,
                                            src.face_value,
                                            src.display_group,
                                            src.ticket_allowed,
                                            src.vip_card,
                                            GETDATE(),
                                            src.last_updated_user,
                                            src.InternetKey,
                                            src.TaxInclusivePrice,
                                            src.InventoryProductCode,
                                            src.ExpiryDate,
                                            src.AvailableUnits,
                                            src.AutoCheckOut,
                                            src.CheckInFacilityId, 
                                            src.MaxCheckOutAmount,
                                            src.POSTypeId,
                                            src.CustomDataSetId,
                                            src.CardTypeId,
                                            src.Guid,
                                            src.site_id,
                                            src.TrxRemarksMandatory,
                                            src.CategoryId,
                                            src.OverridePrintTemplateId,
                                            src.StartDate,
                                            src.ButtonColor,
                                            src.AutoGenerateCardNumber,
                                            src.QuantityPrompt,
                                            src.OnlyForVIP,
                                            src.AllowPriceOverride,
                                            src.RegisteredCustomerOnly,
                                            src.ManagerApprovalRequired,
                                            src.MinimumUserPrice,
                                            src.TextColor,
                                            src.Font,
                                            src.VerifiedCustomerOnly,
                                            src.Modifier,
                                            src.MinimumQuantity,
                                            src.DisplayInPOS,
                                            src.TrxHeaderRemarksMandatory,
                                            src.CardCount,
                                            src.ImageFileName,
                                          --  src.AttractionMasterScheduleId,
                                            src.AdvancePercentage,
                                            src.AdvanceAmount,
                                            src.EmailTemplateId,
                                            src.MaximumTime,
                                            src.MinimumTime,
                                            src.CardValidFor,
                                            src.AdditionalTaxInclusive,
                                            src.AdditionalPrice,
                                            src.AdditionalTaxId,
                                            src.MasterEntityId,
                                            src.WaiverRequired,
                                            src.SegmentCategoryId,
                                            src.CardExpiryDate,
                                            src.InvokeCustomerRegistration,
                                            src.ProductDisplayGroupFormatId,
                                            src.MaxQtyPerDay,
                                            src.HsnSacCode,
                                            src.WebDescription,
                                            src.OrderTypeId,
                                            src.ZoneId,
                                            src.LockerExpiryInHours,
                                            src.LockerExpiryDate,
                                            src.WaiverSetId,
                                            src.isGroupMeal,
                                            src.MembershipID,
                                            src.CreatedBy,
                                            GETDATE(),
                                            src.ExternalSystemReference,
                                            src.LoadToSingleCard,
                                            src.EnableVariableLockerHours,
                                            src.LinkChildCard, src.LicenseType,
                                            src.IssueNotificationDevice, src.NotificationTagProfileId,
                                            src.ServiceCharge , src.PackingCharge, src.SearchDescription, src.IsRecommended,                                            
                                            src.ServiceChargeIsApplicable, src.ServiceChargePercentage,
                                            src.GratuityIsApplicable, src.GratuityPercentage,
                                            src.MaximumQuantity,src.PauseType,src.CustomerProfilingGroupId
                                             --, src.FacilityMapId
                                            )
                                            OUTPUT
                                            inserted.product_id,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.last_updated_date,
                                            inserted.last_updated_user,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            product_id,
                                            CreatedBy, 
                                            CreationDate, 
                                            last_updated_date, 
                                            last_updated_user, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion
        SqlTransaction sqlTransaction;
        DataAccessHandler dataAccessHandler;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        string selectProductQuery = @"select *, null as maxQtyPerDay, null as hsnSacCode, null as  webDescription, null as orderTypeId, null as isGroupMeal, null as membershipId, null as cardSale,
                                               null as zoneCode, null as lockerMode, null as taxName, null as usedInDiscounts,
                                         ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') as Sellable ,
                                               p.ExternalSystemReference
                                          from products p, product_type pt  where pt.product_type_id = p.product_type_id ";

        /// <summary>
        /// Default constructor of ProductDataHandler class
        /// </summary>
        public ProductsDataHandler()
        {
            log.Debug("Starts-ProductDisplayListDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            connstring = dataAccessHandler.ConnectionString;  //added on 11/28/2016 - reservation
            log.Debug("Ends-ProductDisplayListDataHandler() default constructor.");
        }

        /// <summary>
        /// Default constructor of ProductDataHandler class
        /// </summary>
        public ProductsDataHandler(SqlTransaction sqlTransaction) : this()
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the Products record to the database
        /// </summary>
        /// <param name="productsDTO">ProductsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(ProductsDTO productsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productsDTO, userId, siteId);
            Save(new List<ProductsDTO>() { productsDTO }, userId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the Products record to the database
        /// </summary>
        /// <param name="productsDTOList">List of ProductsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<ProductsDTO> productsDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productsDTOList, userId, siteId);
            Dictionary<string, ProductsDTO> customDataSetDTOGuidMap = GetProductsDTOGuidMap(productsDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(productsDTOList, userId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                            sqlTransaction,
                                                            MERGE_QUERY,
                                                            "ProductsType",
                                                            "@ProductsList");
            UpdateProductsDTOList(customDataSetDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<ProductsDTO> productsDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productsDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[100];
            columnStructures[0] = new SqlMetaData("product_id", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("product_name", SqlDbType.NVarChar, 100);
            columnStructures[2] = new SqlMetaData("description", SqlDbType.NVarChar, 2000);
            columnStructures[3] = new SqlMetaData("active_flag", SqlDbType.Char, 1);
            columnStructures[4] = new SqlMetaData("product_type_id", SqlDbType.Int);
            columnStructures[5] = new SqlMetaData("price", SqlDbType.Decimal, 18, 4);
            columnStructures[6] = new SqlMetaData("credits", SqlDbType.Decimal, 18, 4);
            columnStructures[7] = new SqlMetaData("courtesy", SqlDbType.Decimal, 18, 4);
            columnStructures[8] = new SqlMetaData("bonus", SqlDbType.Decimal, 18, 4);
            columnStructures[9] = new SqlMetaData("time", SqlDbType.Decimal, 18, 4);
            columnStructures[10] = new SqlMetaData("sort_order", SqlDbType.Decimal, 18, 0);
            columnStructures[11] = new SqlMetaData("tax_id", SqlDbType.Int);
            columnStructures[12] = new SqlMetaData("tickets", SqlDbType.Decimal, 8, 0);
            columnStructures[13] = new SqlMetaData("face_value", SqlDbType.Decimal, 18, 2);
            columnStructures[14] = new SqlMetaData("display_group", SqlDbType.NVarChar, 50);
            columnStructures[15] = new SqlMetaData("ticket_allowed", SqlDbType.Char, 1);
            columnStructures[16] = new SqlMetaData("vip_card", SqlDbType.Char, 1);
            columnStructures[17] = new SqlMetaData("last_updated_date", SqlDbType.DateTime);
            columnStructures[18] = new SqlMetaData("last_updated_user", SqlDbType.NVarChar, 50);
            columnStructures[19] = new SqlMetaData("InternetKey", SqlDbType.Int);
            columnStructures[20] = new SqlMetaData("TaxInclusivePrice", SqlDbType.Char, 1);
            columnStructures[21] = new SqlMetaData("InventoryProductCode", SqlDbType.NVarChar, 50);
            columnStructures[22] = new SqlMetaData("ExpiryDate", SqlDbType.DateTime);
            columnStructures[23] = new SqlMetaData("AvailableUnits", SqlDbType.Int);
            columnStructures[24] = new SqlMetaData("AutoCheckOut", SqlDbType.Char, 1);
            columnStructures[25] = new SqlMetaData("CheckInFacilityId", SqlDbType.Int);
            columnStructures[26] = new SqlMetaData("MaxCheckOutAmount", SqlDbType.Decimal, 10, 2);
            columnStructures[27] = new SqlMetaData("POSTypeId", SqlDbType.Int);
            columnStructures[28] = new SqlMetaData("CustomDataSetId", SqlDbType.Int);
            columnStructures[29] = new SqlMetaData("CardTypeId", SqlDbType.Int);
            columnStructures[30] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[31] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[32] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[33] = new SqlMetaData("TrxRemarksMandatory", SqlDbType.Char, 1);
            columnStructures[34] = new SqlMetaData("CategoryId", SqlDbType.Int);
            columnStructures[35] = new SqlMetaData("OverridePrintTemplateId", SqlDbType.Int);
            columnStructures[36] = new SqlMetaData("StartDate", SqlDbType.DateTime);
            columnStructures[37] = new SqlMetaData("ButtonColor", SqlDbType.NVarChar, 50);
            columnStructures[38] = new SqlMetaData("AutoGenerateCardNumber", SqlDbType.Char, 1);
            columnStructures[39] = new SqlMetaData("QuantityPrompt", SqlDbType.Char, 1);
            columnStructures[40] = new SqlMetaData("OnlyForVIP", SqlDbType.Char, 1);
            columnStructures[41] = new SqlMetaData("AllowPriceOverride", SqlDbType.Char, 1);
            columnStructures[42] = new SqlMetaData("RegisteredCustomerOnly", SqlDbType.Char, 1);
            columnStructures[43] = new SqlMetaData("ManagerApprovalRequired", SqlDbType.Char, 1);
            columnStructures[44] = new SqlMetaData("MinimumUserPrice", SqlDbType.Decimal, 18, 4);
            columnStructures[45] = new SqlMetaData("TextColor", SqlDbType.NVarChar, 50);
            columnStructures[46] = new SqlMetaData("Font", SqlDbType.NVarChar, 100);
            columnStructures[47] = new SqlMetaData("VerifiedCustomerOnly", SqlDbType.Char, 1);
            columnStructures[48] = new SqlMetaData("Modifier", SqlDbType.Char, 1);
            columnStructures[49] = new SqlMetaData("MinimumQuantity", SqlDbType.Int);
            columnStructures[50] = new SqlMetaData("DisplayInPOS", SqlDbType.Char, 1);
            columnStructures[51] = new SqlMetaData("TrxHeaderRemarksMandatory", SqlDbType.Bit);
            columnStructures[52] = new SqlMetaData("CardCount", SqlDbType.SmallInt);
            columnStructures[53] = new SqlMetaData("ImageFileName", SqlDbType.NVarChar, 50);
            columnStructures[54] = new SqlMetaData("AttractionMasterScheduleId", SqlDbType.Int);
            columnStructures[55] = new SqlMetaData("AdvancePercentage", SqlDbType.Decimal, 6, 2);
            columnStructures[56] = new SqlMetaData("AdvanceAmount", SqlDbType.Decimal, 18, 4);
            columnStructures[57] = new SqlMetaData("EmailTemplateId", SqlDbType.Int);
            columnStructures[58] = new SqlMetaData("MaximumTime", SqlDbType.Int);
            columnStructures[59] = new SqlMetaData("MinimumTime", SqlDbType.Int);
            columnStructures[60] = new SqlMetaData("CardValidFor", SqlDbType.Int);
            columnStructures[61] = new SqlMetaData("AdditionalTaxInclusive", SqlDbType.Char, 1);
            columnStructures[62] = new SqlMetaData("AdditionalPrice", SqlDbType.Decimal, 18, 4);
            columnStructures[63] = new SqlMetaData("AdditionalTaxId", SqlDbType.Int);
            columnStructures[64] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[65] = new SqlMetaData("WaiverRequired", SqlDbType.Char, 1);
            columnStructures[66] = new SqlMetaData("SegmentCategoryId", SqlDbType.Int);
            columnStructures[67] = new SqlMetaData("CardExpiryDate", SqlDbType.DateTime);
            columnStructures[68] = new SqlMetaData("InvokeCustomerRegistration", SqlDbType.Bit);
            columnStructures[69] = new SqlMetaData("ProductDisplayGroupFormatId", SqlDbType.Int);
            columnStructures[70] = new SqlMetaData("MaxQtyPerDay", SqlDbType.Int);
            columnStructures[71] = new SqlMetaData("HsnSacCode", SqlDbType.NVarChar, 200);
            columnStructures[72] = new SqlMetaData("WebDescription", SqlDbType.NVarChar, -1);
            columnStructures[73] = new SqlMetaData("OrderTypeId", SqlDbType.Int);
            columnStructures[74] = new SqlMetaData("ZoneId", SqlDbType.Int);
            columnStructures[75] = new SqlMetaData("LockerExpiryInHours", SqlDbType.Decimal, 18, 4);
            columnStructures[76] = new SqlMetaData("LockerExpiryDate", SqlDbType.DateTime);
            columnStructures[77] = new SqlMetaData("WaiverSetId", SqlDbType.Int);
            columnStructures[78] = new SqlMetaData("isGroupMeal", SqlDbType.Char, 1);
            columnStructures[79] = new SqlMetaData("MembershipID", SqlDbType.Int);
            columnStructures[80] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[81] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[82] = new SqlMetaData("ExternalSystemReference", SqlDbType.NVarChar, 50);
            columnStructures[83] = new SqlMetaData("LoadToSingleCard", SqlDbType.Bit);
            columnStructures[84] = new SqlMetaData("LinkChildCard", SqlDbType.Bit);
            columnStructures[85] = new SqlMetaData("LicenseType", SqlDbType.NVarChar, 30);
            columnStructures[86] = new SqlMetaData("IssueNotificationDevice", SqlDbType.Bit);
            columnStructures[87] = new SqlMetaData("NotificationTagProfileId", SqlDbType.Int);
            columnStructures[88] = new SqlMetaData("EnableVariableLockerHours", SqlDbType.Bit);
            columnStructures[89] = new SqlMetaData("ServiceCharge", SqlDbType.Decimal, 18, 3);
            columnStructures[90] = new SqlMetaData("PackingCharge", SqlDbType.Decimal, 18, 3);
            columnStructures[91] = new SqlMetaData("SearchDescription", SqlDbType.NVarChar, 1000);
            columnStructures[92] = new SqlMetaData("IsRecommended", SqlDbType.Bit);
            columnStructures[93] = new SqlMetaData("ServiceChargeIsApplicable", SqlDbType.Bit);
            columnStructures[94] = new SqlMetaData("ServiceChargePercentage", SqlDbType.Decimal, 18, 3);
            columnStructures[95] = new SqlMetaData("GratuityIsApplicable", SqlDbType.Bit);
            columnStructures[96] = new SqlMetaData("GratuityPercentage", SqlDbType.Decimal, 18, 3);
            columnStructures[97] = new SqlMetaData("MaximumQuantity", SqlDbType.Int);
            columnStructures[98] = new SqlMetaData("PauseType", SqlDbType.NVarChar, 50);
            columnStructures[99] = new SqlMetaData("CustomerProfilingGroupId", SqlDbType.Int);

            //This logic only for WMS when the price is -1 for combo product it should save as -1 
            // For other products it should save as null
            // This will be removed during products redesign
            int productTypeId = -1;
            ProductTypeListBL productTypeListBL = new ProductTypeListBL(machineUserContext);
            List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE, "COMBO"));
            List<ProductTypeDTO> productTypeObj = productTypeListBL.GetProductTypeDTOList(searchParameters);
            if (productTypeObj != null)
            {
                productTypeId = productTypeObj[0].ProductTypeId;
            }
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(productsDTOList[i].ProductId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(productsDTOList[i].ProductName));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(productsDTOList[i].Description));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(productsDTOList[i].ActiveFlag ? "Y" : "N"));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(productsDTOList[i].ProductTypeId, true));
                if (productTypeId != -1 && productTypeId == productsDTOList[i].ProductTypeId)
                {
                    dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(productsDTOList[i].Price));
                }
                else
                {
                    dataRecord.SetValue(5, dataAccessHandler.GetParameterValue((productsDTOList[i].Price < 0) ? (decimal?)null : productsDTOList[i].Price));
                }
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue((productsDTOList[i].Credits < 0) ? (decimal?)null : productsDTOList[i].Credits));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue((productsDTOList[i].Courtesy < 0) ? (decimal?)null : productsDTOList[i].Courtesy));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue((productsDTOList[i].Bonus < 0) ? (decimal?)null : productsDTOList[i].Bonus));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue((productsDTOList[i].Time < 0) ? (decimal?)null : productsDTOList[i].Time));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue((productsDTOList[i].SortOrder < 0) ? (decimal?)null : productsDTOList[i].SortOrder));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(productsDTOList[i].Tax_id, true));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue((productsDTOList[i].Tickets < 0) ? (decimal?)null : productsDTOList[i].Tickets));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue((productsDTOList[i].FaceValue < 0) ? (decimal?)null : productsDTOList[i].FaceValue));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(productsDTOList[i].DisplayGroup));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(productsDTOList[i].TicketAllowed));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(productsDTOList[i].VipCard));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(productsDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(18, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(19, dataAccessHandler.GetParameterValue(productsDTOList[i].InternetKey, true));
                dataRecord.SetValue(20, dataAccessHandler.GetParameterValue(productsDTOList[i].TaxInclusivePrice));
                dataRecord.SetValue(21, dataAccessHandler.GetParameterValue(productsDTOList[i].InventoryProductCode));
                dataRecord.SetValue(22, dataAccessHandler.GetParameterValue(productsDTOList[i].ExpiryDate));
                dataRecord.SetValue(23, dataAccessHandler.GetParameterValue(productsDTOList[i].AvailableUnits, true));
                dataRecord.SetValue(24, dataAccessHandler.GetParameterValue(productsDTOList[i].AutoCheckOut));
                dataRecord.SetValue(25, dataAccessHandler.GetParameterValue(productsDTOList[i].CheckInFacilityId, true));
                dataRecord.SetValue(26, dataAccessHandler.GetParameterValue((productsDTOList[i].MaxCheckOutAmount < 0) ? (decimal?)null : productsDTOList[i].MaxCheckOutAmount));
                dataRecord.SetValue(27, dataAccessHandler.GetParameterValue(productsDTOList[i].POSTypeId, true));
                dataRecord.SetValue(28, dataAccessHandler.GetParameterValue(productsDTOList[i].CustomDataSetId, true));
                dataRecord.SetValue(29, dataAccessHandler.GetParameterValue(productsDTOList[i].CardTypeId, true));

                dataRecord.SetValue(30, dataAccessHandler.GetParameterValue(Guid.Parse(productsDTOList[i].Guid)));
                dataRecord.SetValue(31, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(32, dataAccessHandler.GetParameterValue(productsDTOList[i].SynchStatus));
                dataRecord.SetValue(33, dataAccessHandler.GetParameterValue(productsDTOList[i].TrxRemarksMandatory));
                dataRecord.SetValue(34, dataAccessHandler.GetParameterValue(productsDTOList[i].CategoryId, true));
                dataRecord.SetValue(35, dataAccessHandler.GetParameterValue(productsDTOList[i].OverridePrintTemplateId, true));
                dataRecord.SetValue(36, dataAccessHandler.GetParameterValue(productsDTOList[i].StartDate));
                dataRecord.SetValue(37, dataAccessHandler.GetParameterValue(productsDTOList[i].ButtonColor));
                dataRecord.SetValue(38, dataAccessHandler.GetParameterValue(productsDTOList[i].AutoGenerateCardNumber));
                dataRecord.SetValue(39, dataAccessHandler.GetParameterValue(productsDTOList[i].QuantityPrompt));
                dataRecord.SetValue(40, dataAccessHandler.GetParameterValue(productsDTOList[i].OnlyForVIP));
                dataRecord.SetValue(41, dataAccessHandler.GetParameterValue(productsDTOList[i].AllowPriceOverride));
                dataRecord.SetValue(42, dataAccessHandler.GetParameterValue(productsDTOList[i].RegisteredCustomerOnly));
                dataRecord.SetValue(43, dataAccessHandler.GetParameterValue(productsDTOList[i].ManagerApprovalRequired));
                dataRecord.SetValue(44, dataAccessHandler.GetParameterValue((productsDTOList[i].MinimumUserPrice < 0) ? (decimal?)null : productsDTOList[i].MinimumUserPrice));
                dataRecord.SetValue(45, dataAccessHandler.GetParameterValue(productsDTOList[i].TextColor));
                dataRecord.SetValue(46, dataAccessHandler.GetParameterValue(productsDTOList[i].Font));
                dataRecord.SetValue(47, dataAccessHandler.GetParameterValue(productsDTOList[i].VerifiedCustomerOnly));
                dataRecord.SetValue(48, dataAccessHandler.GetParameterValue(productsDTOList[i].Modifier));
                dataRecord.SetValue(49, dataAccessHandler.GetParameterValue(productsDTOList[i].MinimumQuantity, true));
                dataRecord.SetValue(50, dataAccessHandler.GetParameterValue(productsDTOList[i].DisplayInPOS));
                dataRecord.SetValue(51, dataAccessHandler.GetParameterValue(productsDTOList[i].TrxHeaderRemarksMandatory));
                dataRecord.SetValue(52, dataAccessHandler.GetParameterValue((productsDTOList[i].CardCount < 0) ? (Int16?)null : (Int16)productsDTOList[i].CardCount));
                dataRecord.SetValue(53, dataAccessHandler.GetParameterValue(productsDTOList[i].ImageFileName));
                //dataRecord.SetValue(54, dataAccessHandler.GetParameterValue(productsDTOList[i].AttractionMasterScheduleId, true));
                dataRecord.SetValue(54, DBNull.Value);
                dataRecord.SetValue(55, dataAccessHandler.GetParameterValue((productsDTOList[i].AdvancePercentage < 0) ? (decimal?)null : productsDTOList[i].AdvancePercentage));
                dataRecord.SetValue(56, dataAccessHandler.GetParameterValue((productsDTOList[i].AdvanceAmount < 0) ? (decimal?)null : productsDTOList[i].AdvanceAmount));
                dataRecord.SetValue(57, dataAccessHandler.GetParameterValue(productsDTOList[i].EmailTemplateId, true));
                dataRecord.SetValue(58, dataAccessHandler.GetParameterValue((productsDTOList[i].MaximumTime < 0) ? 0 : productsDTOList[i].MaximumTime));
                dataRecord.SetValue(59, dataAccessHandler.GetParameterValue((productsDTOList[i].MinimumTime < 0) ? 0 : productsDTOList[i].MinimumTime));
                dataRecord.SetValue(60, dataAccessHandler.GetParameterValue(productsDTOList[i].CardValidFor, true));
                dataRecord.SetValue(61, dataAccessHandler.GetParameterValue(productsDTOList[i].AdditionalTaxInclusive));
                dataRecord.SetValue(62, dataAccessHandler.GetParameterValue((productsDTOList[i].AdditionalPrice < 0) ? (decimal?)null : productsDTOList[i].AdditionalPrice));
                dataRecord.SetValue(63, dataAccessHandler.GetParameterValue(productsDTOList[i].AdditionalTaxId, true));
                dataRecord.SetValue(64, dataAccessHandler.GetParameterValue(productsDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(65, dataAccessHandler.GetParameterValue(productsDTOList[i].WaiverRequired));
                dataRecord.SetValue(66, dataAccessHandler.GetParameterValue(productsDTOList[i].SegmentCategoryId, true));
                dataRecord.SetValue(67, dataAccessHandler.GetParameterValue(productsDTOList[i].CardExpiryDate));
                dataRecord.SetValue(68, dataAccessHandler.GetParameterValue(productsDTOList[i].InvokeCustomerRegistration));
                dataRecord.SetValue(69, dataAccessHandler.GetParameterValue(productsDTOList[i].ProductDisplayGroupFormatId, true));
                dataRecord.SetValue(70, dataAccessHandler.GetParameterValue((productsDTOList[i].MaxQtyPerDay.HasValue == false || productsDTOList[i].MaxQtyPerDay.Value < 0) ? (int?)null : productsDTOList[i].MaxQtyPerDay.Value));
                dataRecord.SetValue(71, dataAccessHandler.GetParameterValue(productsDTOList[i].HsnSacCode));
                dataRecord.SetValue(72, dataAccessHandler.GetParameterValue(productsDTOList[i].WebDescription));
                dataRecord.SetValue(73, dataAccessHandler.GetParameterValue(productsDTOList[i].OrderTypeId, true));
                dataRecord.SetValue(74, dataAccessHandler.GetParameterValue(productsDTOList[i].ZoneId, true));
                dataRecord.SetValue(75, dataAccessHandler.GetParameterValue((productsDTOList[i].LockerExpiryInHours < 0) ? (decimal?)null : (decimal)productsDTOList[i].LockerExpiryInHours));
                dataRecord.SetValue(76, dataAccessHandler.GetParameterValue(productsDTOList[i].LockerExpiryDate));
                dataRecord.SetValue(77, dataAccessHandler.GetParameterValue(productsDTOList[i].WaiverSetId, true));
                dataRecord.SetValue(78, dataAccessHandler.GetParameterValue(productsDTOList[i].IsGroupMeal));
                dataRecord.SetValue(79, dataAccessHandler.GetParameterValue(productsDTOList[i].MembershipId, true));
                dataRecord.SetValue(80, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(81, dataAccessHandler.GetParameterValue(productsDTOList[i].CreationDate));
                dataRecord.SetValue(82, dataAccessHandler.GetParameterValue(productsDTOList[i].ExternalSystemReference));
                dataRecord.SetValue(83, dataAccessHandler.GetParameterValue((productsDTOList[i].LoadToSingleCard)));
                dataRecord.SetValue(84, dataAccessHandler.GetParameterValue((productsDTOList[i].LinkChildCard)));
                dataRecord.SetValue(85, dataAccessHandler.GetParameterValue((productsDTOList[i].LicenseType)));
                dataRecord.SetValue(86, dataAccessHandler.GetParameterValue(productsDTOList[i].IssueNotificationDevice));
                dataRecord.SetValue(87, dataAccessHandler.GetParameterValue(productsDTOList[i].NotificationTagProfileId, true));
                dataRecord.SetValue(88, dataAccessHandler.GetParameterValue(productsDTOList[i].EnableVariableLockerHours));
                //dataRecord.SetValue(82, dataAccessHandler.GetParameterValue(productsDTOList[i].ProductType));
                //dataRecord.SetValue(82, dataAccessHandler.GetParameterValue(productsDTOList[i].FacilityMapId, true));
                dataRecord.SetValue(89, dataAccessHandler.GetParameterValue((productsDTOList[i].ServiceCharge.HasValue == false || productsDTOList[i].ServiceCharge.Value < 0) ? (decimal?)null : productsDTOList[i].ServiceCharge.Value));
                dataRecord.SetValue(90, dataAccessHandler.GetParameterValue((productsDTOList[i].PackingCharge.HasValue == false || productsDTOList[i].PackingCharge.Value < 0) ? (decimal?)null : productsDTOList[i].PackingCharge.Value));
                dataRecord.SetValue(91, dataAccessHandler.GetParameterValue(productsDTOList[i].SearchDescription));
                dataRecord.SetValue(92, dataAccessHandler.GetParameterValue((productsDTOList[i].IsRecommended)));
                dataRecord.SetValue(93, dataAccessHandler.GetParameterValue((productsDTOList[i].ServiceChargeIsApplicable)));
                dataRecord.SetValue(94, dataAccessHandler.GetParameterValue((productsDTOList[i].ServiceChargePercentage.HasValue == false || productsDTOList[i].ServiceChargePercentage.Value < 0) ? (decimal?)null : productsDTOList[i].ServiceChargePercentage.Value));
                dataRecord.SetValue(95, dataAccessHandler.GetParameterValue((productsDTOList[i].GratuityIsApplicable)));
                dataRecord.SetValue(96, dataAccessHandler.GetParameterValue((productsDTOList[i].GratuityPercentage.HasValue == false || productsDTOList[i].GratuityPercentage.Value < 0) ? (decimal?)null : productsDTOList[i].GratuityPercentage.Value));
                dataRecord.SetValue(97, dataAccessHandler.GetParameterValue((productsDTOList[i].MaximumQuantity.HasValue == false || productsDTOList[i].MaximumQuantity.Value < 0) ? (int?)null : productsDTOList[i].MaximumQuantity.Value));
                dataRecord.SetValue(98, dataAccessHandler.GetParameterValue((productsDTOList[i].PauseType == ProductsContainerDTO.PauseUnPauseType.UNPAUSE ? "U" : (productsDTOList[i].PauseType == ProductsContainerDTO.PauseUnPauseType.PAUSE ? "P" : string.Empty))));
                dataRecord.SetValue(99, dataAccessHandler.GetParameterValue(productsDTOList[i].CustomerProfilingGroupId, true));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, ProductsDTO> GetProductsDTOGuidMap(List<ProductsDTO> productsDTOList)
        {
            Dictionary<string, ProductsDTO> result = new Dictionary<string, ProductsDTO>();
            for (int i = 0; i < productsDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(productsDTOList[i].Guid))
                {
                    productsDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(productsDTOList[i].Guid, productsDTOList[i]);
            }
            return result;
        }

        private void UpdateProductsDTOList(Dictionary<string, ProductsDTO> productsDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                ProductsDTO productsDTO = productsDTOGuidMap[Convert.ToString(row["Guid"])];
                productsDTO.ProductId = row["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["product_id"]);
                productsDTO.LastUpdatedUser = row["last_updated_user"] == DBNull.Value ? null : Convert.ToString(row["last_updated_user"]);
                productsDTO.LastUpdatedDate = row["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["last_updated_date"]);
                productsDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                productsDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                productsDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                productsDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Gets the Products data of passed Products Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ProductsDTO</returns>
        public ProductsDTO GetProductsDTO(int id)
        {
            log.LogMethodEntry(id);
            ProductsDTO result = null;
            string query = @"select *, null as maxQtyPerDay, null as hsnSacCode, null as  webDescription, null as orderTypeId, null as isGroupMeal, null as membershipId, null as cardSale,
                            null as zoneCode, null as lockerMode, null as taxName, null as usedInDiscounts,
                            ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') as Sellable ,
                            p.ExternalSystemReference
                            from products p, product_type pt  
                            where pt.product_type_id = p.product_type_id and p.product_id = @Id ";
            DataTable table = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@Id", id, true) }, sqlTransaction);
            if (table != null && table.Rows.Count > 0)
            {
                var list = table.Rows.Cast<DataRow>().Select(x => GetProductDTO(x));
                if (list != null)
                {
                    result = list.FirstOrDefault();
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        ///Starts: Modification on 14-Jul-2016 for updating the segmentation to products.
        /// <summary>
        /// This method is used to update the categorization id to the products table
        /// </summary>
        /// <param name="productsId">Product id from the Products table.</param>
        /// <param name="segmentGetegoryId">Segment category Id of the Product</param>
        /// <returns>the count of rows updated.</returns>
        public int UpdateProductsSegmentCategoryId(int productsId, int segmentGetegoryId)
        {
            log.LogMethodEntry(productsId, segmentGetegoryId);
            string updateProductQuery = @"update Products set SegmentCategoryId=@segmentCategoryId where product_id=@productsId";
            List<SqlParameter> updateProductParameters = new List<SqlParameter>();
            updateProductParameters.Add(new SqlParameter("@productsId", productsId));
            updateProductParameters.Add(new SqlParameter("@segmentCategoryId", segmentGetegoryId));
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateProductQuery, updateProductParameters.ToArray());
            log.Debug("Ends-UpdateProductsSegmentCategoryId(" + productsId + "," + segmentGetegoryId + ") method.");
            return rowsUpdated;

        }//Ends: Modification on 14-Jul-2016 for updating the segmentation to products.

        ///// <summary>
        ///// Get the product by type
        ///// </summary>
        ///// <param name="productType"> product type should be passed</param>
        ///// <param name="siteId"> site id of the product</param>
        ///// <returns></returns>
        //public ProductsDTO GetProductByType(string productType, int siteId)
        //{
        //    log.LogMethodEntry(productType,siteId);
        //    try
        //    {
        //        string productQuery = @"select top 1 * , null as maxQtyPerDay, null as hsnSacCode, null as  webDescription, null as orderTypeId, null as isGroupMeal, null as membershipId, null as cardSale,
        //                                       null as zoneCode, null as lockerMode, null as taxName, null as usedInDiscounts
        //                                  from products p,product_type pt 
        //                                  where p.product_type_id=pt.product_type_id and product_type =@productType and (p.site_id = @siteId or @siteId = -1)";

        //        SqlParameter[] productparameters = new SqlParameter[2];
        //        productparameters[0] = new SqlParameter("@productType", productType);
        //        productparameters[1] = new SqlParameter("@siteId", siteId);

        //        DataTable dtproductDTO = dataAccessHandler.executeSelectQuery(productQuery, productparameters);
        //        ProductsDTO productDTO = new ProductsDTO();
        //        if (dtproductDTO.Rows.Count > 0)
        //        {
        //            DataRow productDTORow = dtproductDTO.Rows[0];
        //            productDTO = GetProductDTO(productDTORow);

        //        }
        //        log.LogMethodExit(productDTO);
        //        return productDTO;
        //    }
        //    catch (Exception expn)
        //    {
        //        log.Error(expn.ToString());
        //        throw new System.Exception("At  GetProductDTO" + expn.Message.ToString());
        //    }

        //}


        ///// <summary>
        ///// Get the product by type
        ///// </summary>
        ///// <param name="productType"> product type should be passed</param>
        ///// <param name="siteId"> site id of the product</param>
        ///// <returns>Product Type List</returns>
        ///// Get the Lookups values for products in games entity by passing the product type
        ///// Created by Jagan Mohana - 05-Dec-2018
        //public List<ProductsDTO> GetProductByTypeList(string productType, int siteId)
        //{
        //    log.LogMethodEntry(productType,siteId);
        //    try
        //    {
        //        string productQuery = @"select * , null as maxQtyPerDay, null as hsnSacCode, null as  webDescription, null as orderTypeId, null as isGroupMeal, null as membershipId, null as cardSale,
        //                                       null as zoneCode, null as lockerMode, null as taxName, null as usedInDiscounts
        //                                  from products p,product_type pt 
        //                                  where p.product_type_id=pt.product_type_id and product_type =@productType and (p.site_id = @siteId or @siteId = -1)";

        //        SqlParameter[] productparameters = new SqlParameter[2];
        //        productparameters[0] = new SqlParameter("@productType", productType);
        //        productparameters[1] = new SqlParameter("@siteId", siteId);

        //        DataTable dtproductDTO = dataAccessHandler.executeSelectQuery(productQuery, productparameters);
        //        List<ProductsDTO> productsTypeDTO = new List<ProductsDTO>();
        //        if (dtproductDTO.Rows.Count > 0)
        //        {

        //            foreach (DataRow productDataRow in dtproductDTO.Rows)
        //            {
        //                ProductsDTO productDTORow = GetProductDTO(productDataRow);
        //                productsTypeDTO.Add(productDTORow);
        //            }

        //        }
        //        log.LogMethodExit(productsTypeDTO);
        //        return productsTypeDTO;
        //    }
        //    catch (Exception expn)
        //    {
        //        log.Error(expn.ToString());
        //        throw new System.Exception("At  GetProductDTO" + expn.Message.ToString());
        //    }
        //}

        /// <summary>
        /// Get the product types
        /// </summary>        
        /// <param name="siteId"> site id of the product</param>
        /// <returns>Product Type List</returns>
        /// Get the Lookups values for product types
        /// Created by Jagan Mohana -11-Jan-2019
        public List<CommonLookupDTO> GetProductTypeList(int siteId)
        {
            log.LogMethodEntry(siteId);
            try
            {
                string productQuery = @"select product_type_id,product_type from product_type where (site_id = @siteId or @siteId = -1)";

                SqlParameter[] productparameters = new SqlParameter[1];
                productparameters[0] = new SqlParameter("@siteId", siteId);

                DataTable dtproductDTO = dataAccessHandler.executeSelectQuery(productQuery, productparameters);
                List<CommonLookupDTO> productTypeDTORow = new List<CommonLookupDTO>();
                if (dtproductDTO.Rows.Count > 0)
                {

                    foreach (DataRow productDataRow in dtproductDTO.Rows)
                    {
                        CommonLookupDTO productDTORow = GetProductsTypeDTO(productDataRow);
                        productTypeDTORow.Add(productDTORow);
                    }

                }
                log.LogMethodExit(productTypeDTORow);
                return productTypeDTORow;
            }
            catch (Exception expn)
            {
                log.Error(expn.ToString());
                throw new System.Exception("At  GetProductsTypeDTO" + expn.Message.ToString());
            }
        }

        private CommonLookupDTO GetProductsTypeDTO(DataRow productDataRow)
        {
            log.Debug("starts- GetProductDTO(DataRow productDataRow) Method.");
            try
            {
                CommonLookupDTO productDTO = new CommonLookupDTO(
                    productDataRow["product_type_id"].ToString(),
                    productDataRow["product_type"].ToString()
                );
                log.Debug("Ends- GetProductDTO(DataRow productDataRow) Method.");
                return productDTO;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }

        private ProductsStruct GetProductsStructDTO(DataRow productStructDataRow)
        {
            log.LogMethodEntry(productStructDataRow);
            ProductsStruct productsObject = new ProductsStruct(Convert.ToInt32(productStructDataRow["product_id"]),
                                                    productStructDataRow["product_name"].ToString(),
                                                    productStructDataRow["description"].ToString(),
                                                    productStructDataRow["product_type"].ToString(),
                                                    productStructDataRow["display_group"].ToString()
                                                    );
            log.LogMethodExit(productsObject);
            return productsObject;
        }



        /// <summary>
        /// To get Staff card products list
        /// </summary>
        /// <param name="productsFilterParams">filter parameters</param>
        /// <returns>returns list of staff of card products</returns>
        public List<ProductsStruct> StaffCardProductList(ProductsFilterParams productsFilterParams)
        {
            log.LogMethodEntry(productsFilterParams);
            List<ProductsStruct> staffCardProducts = new List<ProductsStruct>();
            try
            {
                string selectStaffCardProductsQuery = @"SELECT p.product_id, p.product_name as product_name, 
		                                                        ISNULL(case p.description when '' then null else p.description end, p.product_name) as description, 
		                                                        pt.product_type, 
		                                                        ISNULL(case pdf.displayGroup when '' then null else pdf.displayGroup end, 'Others') display_group,  
		                                                        invP.ImageFileName, p.ButtonColor, p.TextColor, p.Font,
		                                                        pdf.ButtonColor DispGroupButtonColor, pdf.TextColor DispGroupTextColor, pdf.Font DispGroupFont
                                                        FROM products p LEFT OUTER JOIN product invP on p.product_id = invP.ManualProductId
				                                                        LEFT OUTER JOIN ProductsDisplayGroup pdg on pdg.ProductId = p.product_id
				                                                        LEFT OUTER JOIN ProductDisplayGroupFormat pdf on pdf.Id = pdg.DisplayGroupId, 
				                                                        product_type pt 
                                                        WHERE p.product_type_id = pt.product_type_id 
				                                            AND p.active_flag = 'Y' 
				                                            AND p.DisplayInPOS = 'Y'
				                                            AND (p.POSTypeId = @Counter or @Counter = -1 or p.POSTypeId is null)
				                                            AND (p.expiryDate >= getdate() or p.expiryDate is null)
				                                            AND (p.StartDate <= getdate() or p.StartDate is null)
				                                            AND pdf.Id = @displayGrpId 
                                                        ORDER BY ISNULL(pdf.sortOrder,(select top 1 SortOrder from ProductDisplayGroupFormat where DisplayGroup = 'Others')), display_group, sort_order,
                                                        CASE product_type 
				                                            WHEN 'CARDSALE' then 0
				                                            WHEN 'NEW' then 1
				                                            WHEN 'RECHARGE' then 2 
				                                            WHEN 'VARIABLECARD' then 3 
				                                            WHEN 'GAMETIME' then 4
				                                            WHEN 'CHECK-IN' then 5
				                                            WHEN 'CHECK-OUT' then 6 
				                                            ELSE 7 end";
                SqlParameter[] queryParams = new SqlParameter[2];
                queryParams[0] = new SqlParameter("@Counter", productsFilterParams.POSTypeId);
                queryParams[1] = new SqlParameter("@displayGrpId", productsFilterParams.DisplayGroupId);
                DataTable usersIdTagData = dataAccessHandler.executeSelectQuery(selectStaffCardProductsQuery, queryParams);
                if (usersIdTagData.Rows.Count > 0)
                {
                    foreach (DataRow productDataRow in usersIdTagData.Rows)
                    {
                        ProductsStruct usersDataObject = GetProductsStructDTO(productDataRow);
                        staffCardProducts.Add(usersDataObject);
                    }
                    log.Debug("Ends-GetStaffCards() Method by returning productList.");
                    return staffCardProducts;
                }
                else
                {
                    log.Debug("Ends-GetStaffCards() Method by returning null.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends with error GetStaffCards(), " + ex.Message); 
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to ProductsDTO object
        /// </summary>
        /// <returns>return the ProductsDTO object</returns>
        private ProductsDTO GetProductDTO(DataRow productDataRow)
        {
            log.LogMethodEntry(productDataRow);
            try
            {
                ProductsDTO productDTO = new ProductsDTO(
                string.IsNullOrEmpty(productDataRow["product_id"].ToString()) ? -1 : Convert.ToInt32(productDataRow["product_id"]),
                productDataRow["product_name"].ToString(),
                productDataRow["description"].ToString(),
                productDataRow["active_flag"] == DBNull.Value ? true : productDataRow["active_flag"].ToString() == "Y",
                string.IsNullOrEmpty(productDataRow["product_type_id"].ToString()) ? -1 : Convert.ToInt32(productDataRow["product_type_id"]),
                string.IsNullOrEmpty(productDataRow["price"].ToString()) ? 0 : Convert.ToDecimal(productDataRow["price"]),
                string.IsNullOrEmpty(productDataRow["credits"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["credits"]),
                string.IsNullOrEmpty(productDataRow["courtesy"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["courtesy"]),
                string.IsNullOrEmpty(productDataRow["bonus"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["bonus"]),
                string.IsNullOrEmpty(productDataRow["time"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["time"]),
                string.IsNullOrEmpty(productDataRow["sort_order"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["sort_order"]),
                string.IsNullOrEmpty(productDataRow["tax_id"].ToString()) ? -1 : Convert.ToInt32(productDataRow["tax_id"]),
                string.IsNullOrEmpty(productDataRow["tickets"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["tickets"]),
                string.IsNullOrEmpty(productDataRow["face_value"].ToString()) ? 0 : Convert.ToDecimal(productDataRow["face_value"]),
                productDataRow["display_group"].ToString(),
                productDataRow["ticket_allowed"].ToString(),
                productDataRow["vip_card"].ToString(),
                string.IsNullOrEmpty(productDataRow["last_updated_date"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(productDataRow["last_updated_date"]),
                productDataRow["last_updated_user"].ToString(),
                string.IsNullOrEmpty(productDataRow["InternetKey"].ToString()) ? -1 : Convert.ToInt32(productDataRow["InternetKey"]),
                productDataRow["TaxInclusivePrice"].ToString(),
                productDataRow["InventoryProductCode"].ToString(),
                string.IsNullOrEmpty(productDataRow["ExpiryDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(productDataRow["ExpiryDate"]),
                string.IsNullOrEmpty(productDataRow["AvailableUnits"].ToString()) ? -1 : Convert.ToInt32(productDataRow["AvailableUnits"]),
                productDataRow["AutoCheckOut"].ToString(),
                string.IsNullOrEmpty(productDataRow["CheckInFacilityId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["CheckInFacilityId"]),
                string.IsNullOrEmpty(productDataRow["MaxCheckOutAmount"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["MaxCheckOutAmount"]),
                string.IsNullOrEmpty(productDataRow["POSTypeId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["POSTypeId"]),
                string.IsNullOrEmpty(productDataRow["CustomDataSetId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["CustomDataSetId"]),
                string.IsNullOrEmpty(productDataRow["CardTypeId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["CardTypeId"]),
                productDataRow["Guid"].ToString(),
                string.IsNullOrEmpty(productDataRow["site_id"].ToString()) ? -1 : Convert.ToInt32(productDataRow["site_id"]),
                string.IsNullOrEmpty(productDataRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(productDataRow["SynchStatus"]),
                productDataRow["TrxRemarksMandatory"].ToString(),
                string.IsNullOrEmpty(productDataRow["CategoryId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["CategoryId"]),
                string.IsNullOrEmpty(productDataRow["OverridePrintTemplateId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["OverridePrintTemplateId"]),
                string.IsNullOrEmpty(productDataRow["StartDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(productDataRow["StartDate"]),
                productDataRow["ButtonColor"].ToString(),
                productDataRow["AutoGenerateCardNumber"].ToString(),
                productDataRow["QuantityPrompt"].ToString(),
                productDataRow["OnlyForVIP"].ToString(),
                productDataRow["AllowPriceOverride"].ToString(),
                productDataRow["RegisteredCustomerOnly"].ToString(),
                productDataRow["ManagerApprovalRequired"].ToString(),
                string.IsNullOrEmpty(productDataRow["MinimumUserPrice"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["MinimumUserPrice"]),
                productDataRow["TextColor"].ToString(),
                productDataRow["Font"].ToString(),
                productDataRow["VerifiedCustomerOnly"].ToString(),
                productDataRow["Modifier"].ToString(),
                string.IsNullOrEmpty(productDataRow["MinimumQuantity"].ToString()) ? 0 : Convert.ToInt32(productDataRow["MinimumQuantity"]),
                productDataRow["DisplayInPOS"].ToString(),
                string.IsNullOrEmpty(productDataRow["TrxHeaderRemarksMandatory"].ToString()) ? false : Convert.ToBoolean(productDataRow["TrxHeaderRemarksMandatory"]),
               string.IsNullOrEmpty(productDataRow["CardCount"].ToString()) ? -1 : Convert.ToInt16(productDataRow["CardCount"]),
                productDataRow["ImageFileName"].ToString(),
                //string.IsNullOrEmpty(productDataRow["AttractionMasterScheduleId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["AttractionMasterScheduleId"]),
                string.IsNullOrEmpty(productDataRow["AdvancePercentage"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["AdvancePercentage"]),
                string.IsNullOrEmpty(productDataRow["AdvanceAmount"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["AdvanceAmount"]),
                string.IsNullOrEmpty(productDataRow["EmailTemplateId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["EmailTemplateId"]),
                string.IsNullOrEmpty(productDataRow["MaximumTime"].ToString()) ? -1 : Convert.ToInt32(productDataRow["MaximumTime"]),
                string.IsNullOrEmpty(productDataRow["MinimumTime"].ToString()) ? -1 : Convert.ToInt32(productDataRow["MinimumTime"]),
                string.IsNullOrEmpty(productDataRow["CardValidFor"].ToString()) ? -1 : Convert.ToInt32(productDataRow["CardValidFor"]),
                productDataRow["AdditionalTaxInclusive"].ToString(),
                string.IsNullOrEmpty(productDataRow["AdditionalPrice"].ToString()) ? -1 : Convert.ToDecimal(productDataRow["AdditionalPrice"]),
                string.IsNullOrEmpty(productDataRow["AdditionalTaxId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["AdditionalTaxId"]),
                string.IsNullOrEmpty(productDataRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["MasterEntityId"]),
                productDataRow["WaiverRequired"].ToString(),
                string.IsNullOrEmpty(productDataRow["SegmentCategoryId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["SegmentCategoryId"]),
                string.IsNullOrEmpty(productDataRow["CardExpiryDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(productDataRow["CardExpiryDate"]),
                string.IsNullOrEmpty(productDataRow["InvokeCustomerRegistration"].ToString()) ? false : Convert.ToBoolean(productDataRow["InvokeCustomerRegistration"]),
                string.IsNullOrEmpty(productDataRow["ProductDisplayGroupFormatId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["ProductDisplayGroupFormatId"]),
                productDataRow["ZoneId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["ZoneId"]),
                productDataRow["LockerExpiryInHours"] == DBNull.Value ? 0 : Convert.ToDouble(productDataRow["LockerExpiryInHours"]),
                productDataRow["LockerExpiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productDataRow["LockerExpiryDate"]),
                productDataRow["WaiverSetId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["WaiverSetId"]),
                productDataRow["product_type"].ToString(),
                string.IsNullOrEmpty(productDataRow["maxQtyPerDay"].ToString()) ? (int?)null : Convert.ToInt32(productDataRow["maxQtyPerDay"]),
                productDataRow["hsnSacCode"].ToString(),
                productDataRow["webDescription"].ToString(),
                string.IsNullOrEmpty(productDataRow["orderTypeId"].ToString()) ? (int?)null : Convert.ToInt32(productDataRow["orderTypeId"]),
                productDataRow["isGroupMeal"].ToString(),
                string.IsNullOrEmpty(productDataRow["membershipId"].ToString()) ? (int?)null : Convert.ToInt32(productDataRow["membershipId"]),
                productDataRow["cardSale"].ToString(),
                productDataRow["zoneCode"].ToString(),
                productDataRow["lockerMode"].ToString(),
                productDataRow["taxName"].ToString(),
                string.IsNullOrEmpty(productDataRow["usedInDiscounts"].ToString()) ? (bool?)null : Convert.ToBoolean(productDataRow["usedInDiscounts"]),
                productDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(productDataRow["CreatedBy"]),
                productDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productDataRow["CreationDate"]),
                //string.IsNullOrEmpty(productDataRow["FacilityMapId"].ToString()) ? -1 : Convert.ToInt32(productDataRow["FacilityMapId"])
                productDataRow["Sellable"].ToString() == "Y" ? true : false,
                string.IsNullOrEmpty(productDataRow["externalsystemreference"].ToString()) ? "" : productDataRow["externalsystemreference"].ToString(),
                productDataRow.Table.Columns.Contains("TranslatedProductName") ? productDataRow["TranslatedProductName"].ToString() : "",
                productDataRow.Table.Columns.Contains("TranslatedProductDescription") ? productDataRow["TranslatedProductDescription"].ToString() : "",
                productDataRow["LoadToSingleCard"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["LoadToSingleCard"]),
                productDataRow["LinkChildCard"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["LinkChildCard"]),
                productDataRow["LicenseType"].ToString(),
                productDataRow["IssueNotificationDevice"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(productDataRow["IssueNotificationDevice"]),
                productDataRow["EnableVariableLockerHours"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["EnableVariableLockerHours"]),
                productDataRow["NotificationTagProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["NotificationTagProfileId"]),
                productDataRow["ServiceCharge"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productDataRow["ServiceCharge"].ToString()),
                productDataRow["PackingCharge"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productDataRow["PackingCharge"].ToString()),
                productDataRow["SearchDescription"] == DBNull.Value ? string.Empty : Convert.ToString(productDataRow["SearchDescription"].ToString()),
                productDataRow["IsRecommended"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["IsRecommended"]),
                  productDataRow["ServiceChargeIsApplicable"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["ServiceChargeIsApplicable"]),
                productDataRow["ServiceChargePercentage"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productDataRow["ServiceChargePercentage"].ToString()),
                productDataRow["GratuityIsApplicable"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["GratuityIsApplicable"]),
                productDataRow["GratuityPercentage"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productDataRow["GratuityPercentage"].ToString()),
                productDataRow["MaximumQuantity"] == DBNull.Value ? (int?)null : Convert.ToInt32(productDataRow["MaximumQuantity"].ToString()),
                productDataRow["PauseType"] == DBNull.Value ? (ProductsContainerDTO.PauseUnPauseType.NONE) : 
                                    (productDataRow["PauseType"].ToString() == "P" ? ProductsContainerDTO.PauseUnPauseType.PAUSE : ProductsContainerDTO.PauseUnPauseType.UNPAUSE),
                productDataRow["CustomerProfilingGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["CustomerProfilingGroupId"].ToString()));
                log.LogMethodExit(productDTO);
                return productDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// GetProductDTO(int productId) method
        /// </summary>
        /// <param name="productId">productId</param>
        /// <returns>returns ProductsDTO object</returns>
        public ProductsDTO GetProductDTO(int productId, int roleId = -1, int? membershipId = null)
        {
            log.LogMethodEntry(productId);
            try
            {
                string productQuery = @"select p.product_id,IsNull(p.OrderTypeId, pt.OrderTypeId) as OrderTypeId, p.product_name, p.description, p.CardExpiryDate, isnull(isnull(plp.price, p.price), 0) Price, p.tickets, InventoryProductCode, 
                                p.credits, p.courtesy, p.bonus, p.time, p.tax_id, p.ticket_allowed, p.vip_card, p.product_type_id,
                                pt.description product_type_desc, pt.product_type, p.face_value, p.TaxInclusivePrice, p.active_flag,
                                availableUnits, AutoCheckOut, 
                                --CASE WHEN pt.product_type IN ('BOOKINGS','RENTAL','ATTRACTION') THEN 
                                --        (select top 1 fac.FacilityId 
                                --           from CheckInFacility fac, ProductsAllowedInFacility paif,facilityMapDetails vfd
                                --          where vfd.facilityMapId = paif.facilityMapId
                                --            AND fac.FacilityId = vfd.FacilityId
                                --            and paif.IsActive = 1
                                --            and vfd.IsActive = 1 
                                --            and paif.ProductsId = p.Product_id ) 
                                --    ELSE
                                p.checkinfacilityId as checkinfacilityId,
                                t.tax_name as taxName,
                                null CreditPlusConsumptionId, MembershipID, TrxRemarksMandatory, CategoryId, 
                                AutoGenerateCardNumber, QuantityPrompt, OnlyForVIP, isnull(t.tax_percentage, 0) tax_percentage, 
                                AllowPriceOverride, ManagerApprovalRequired, RegisteredCustomerOnly, MinimumUserPrice, VerifiedCustomerOnly,
                                isnull(MinimumQuantity, 0) MinimumQuantity, isnull(TrxHeaderRemarksMandatory, 0) TrxHeaderRemarksMandatory, pt.cardSale,
                                p.CategoryId, isnull(p.isGroupMeal, 'N') isGroupMeal, p.WaiverRequired, isnull(p.InvokeCustomerRegistration,0) InvokeCustomerRegistration, lz.ZoneCode, lz.LockerMode,
                                Isnull(p.ZoneId,-1) ZoneId, p.LockerExpiryInHours, isnull(LockerExpiryDate,case when LockerExpiryInHours is null or LockerExpiryInHours = 0 then NULL else DATEADD(minute,LockerExpiryInHours*60, getdate())End) as LockerExpiryDate, p.WaiverSetId, 
                                (select 1 
                                    where exists (select 1 from DiscountPurchaseCriteria dpc, discounts d where d.discount_id = dpc.discountId and ISNULL(dpc.IsActive, 'Y')='Y' and d.active_flag = 'Y' and (productId = p.product_id or CategoryId = p.CategoryId))
                                    or exists (select 1 from DiscountedProducts dp, discounts d where d.discount_id = dp.discountId and ISNULL(dp.IsActive,'Y') = 'Y' and d.active_flag = 'Y' and Discounted = 'Y' and (productId = p.product_id or CategoryId = p.CategoryId))) UsedInDiscounts, 
                                isnull(p.MaxQtyPerDay, 0) MaxQtyPerDay,
                                isnull(p.HsnSacCode,'') HsnSacCode,
                                isnull(p.CardValidFor, 0) CardValidFor,
                                isnull(p.CardCount, 0) CardCount,
                                p.sort_order,
                                p.display_group,
                                p.last_updated_date,
                                p.last_updated_user,
                                p.internetKey,
                                p.ExpiryDate,
                                p.MaxCheckoutAmount,
                                p.PosTypeId,
                                p.customDataSetId,
                                p.cardTypeId,
                                p.Guid,
                                p.site_id,
                                p.synchStatus,
								p.OverridePrintTemplateId,
                                p.startDate,
                                p.ButtonColor,
                                p.textCOlor,
                                p.font, p.Modifier, p.displayInPOS, p.ImageFileName, p.attractionMasterScheduleId, p.advancePercentage, p.AdvanceAmount, 
                                p.emailTemplateId, p.MaximumTime, p.MinimumTime, p.AdditionalTaxInclusive, p.AdditionalPrice, p.AdditionalTaxId, p.MasterEntityId,
                                p.segmentCategoryId, p.productDisplayGroupFormatId, p.webDescription,
                                p.CreatedBy, p.CreationDate,
                                --p.FacilityMapId
                                ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') as Sellable ,
                                p.ExternalSystemReference,
                                 p.LoadToSingleCard ,
                                p.LinkChildCard,
                                p.LicenseType,
                                p.IssueNotificationDevice,
                                p.NotificationTagProfileId,
                                p.EnableVariableLockerHours,
                                p.NotificationTagProfileId,
                                p.ServiceCharge,
                                p.PackingCharge,
                                p.SearchDescription,
								p.IsRecommended,
                                p.ServiceChargeIsApplicable,
								p.ServiceChargePercentage,
								p.GratuityIsApplicable,
								p.GratuityPercentage,
								p.MaximumQuantity,
								p.PauseType,
								p.CustomerProfilingGroupId
                              from products p 
                               left outer join (select top 1 ProductId, Price
                                                    from PriceListProducts
                                                   where ProductId = @ProductId
                                                   and PriceListId = (select top 1 *
                                                                       from (select PriceListId
                                                                               from Membership
                                                                              where MembershipId = @MembershipId
                                                                               --from CardType
                                                                              --where CardTypeId = @CardTypeId
                                                                              union all
                                                                            select PriceListId
                                                                                from UserRolePriceList
                                                                                 where Role_id = @RoleId
                                                                              union all
                                                                             select PriceListId
                                                                               from PriceList
                                                                              where PriceListName = 'Default'
                                                                            ) v
                                                                      )
                                                    and isnull(EffectiveDate, getdate()-10000) <= getdate()
                                                    order by EffectiveDate desc) plp
                                    on plp.ProductId = p.product_id
                                Left Outer join LockerZones lz on lz.ZoneId = p.ZoneId  
                                left outer join tax t on t.tax_id = p.tax_id,
                                product_type pt where p.product_id = @ProductId and p.product_type_id = pt.product_type_id ";

                SqlParameter[] productparameters = new SqlParameter[3];
                productparameters[0] = new SqlParameter("@ProductId", productId);
                productparameters[1] = membershipId == null ? new SqlParameter("@MembershipId", -1) : new SqlParameter("@MembershipId", membershipId);
                productparameters[2] = new SqlParameter("@RoleId", roleId);

                DataTable dtproductDTO = dataAccessHandler.executeSelectQuery(productQuery, productparameters);
                ProductsDTO productDTO = new ProductsDTO();
                if (dtproductDTO.Rows.Count > 0)
                {
                    DataRow productDTORow = dtproductDTO.Rows[0];
                    productDTO = GetProductDTO(productDTORow);

                }
                log.Debug("Ends-GetProductDTO(int productId)  Method.");
                return productDTO;
            }
            catch (Exception expn)
            {
                log.Error(expn.Message);
                throw new System.Exception("At  GetProductDTO" + expn.Message.ToString());
            }
        }

        /// <summary>
        /// GetProductDTOByDisplayGroupId(int displayGroupId) method
        /// </summary>
        /// <param name="displayGroupId">productId</param>
        /// <returns>returns List<ProductsDTO> object</returns>
        public List<ProductsDTO> GetProductDTOByDisplayGroupId(int displayGroupId)
        {
            log.LogMethodEntry(displayGroupId);

            try
            {
                string productQuery = @"select * , null as maxQtyPerDay, null as hsnSacCode, null as  webDescription, null as orderTypeId, null as isGroupMeal, null as membershipId, null as cardSale,
                                               null as zoneCode, null as lockerMode, null as taxName, null as usedInDiscounts,
                                               ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') as Sellable ,
                                                p.ExternalSystemReference
                                          from Products p, product_type pt, ProductsDisplayGroup pdg
                                          where p.product_type_id = pt.product_type_id and pdg.ProductId=p.product_id and pdg.DisplayGroupId = @displayGroupId";

                SqlParameter[] productparameters = new SqlParameter[1];
                productparameters[0] = new SqlParameter("@displayGroupId", displayGroupId);

                DataTable productsData = dataAccessHandler.executeSelectQuery(productQuery, productparameters, null);
                List<ProductsDTO> productsDTOList = new List<ProductsDTO>();
                if (productsData.Rows.Count > 0)
                {

                    foreach (DataRow dataRow in productsData.Rows)
                    {
                        ProductsDTO productsDTOObject = GetProductDTO(dataRow);
                        productsDTOList.Add(productsDTOObject);
                    }
                }
                log.LogMethodExit(productsDTOList);
                return productsDTOList;
            }
            catch (Exception expn)
            {
                throw new System.Exception("At  GetProductDTOByDisplayGroupId" + expn.Message.ToString());
            }
        }


        /// <summary>
        /// return the record from the database based on  agentId
        /// </summary>
        /// <returns>return the ProductsDTO object</returns>
        /// or empty ProductsDTO
        public List<ProductsDTO> GetProductDTOList(ProductsFilterParams productsFilterParams)
        {
            log.LogMethodEntry(productsFilterParams);

            try
            {
                log.Debug("Ends-GetProductDTOList Method.");
                return GetAllProductList(BuildProductSearchParametersList(productsFilterParams));
            }
            catch (Exception expn)
            {
                log.Error(expn.Message);
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// return the product list that can be set as rewards
        /// </summary>
        /// <returns>return the ProductsDTO object</returns>
        /// or empty ProductsDTO
        public List<ProductsDTO> GetRewardProductDTOList(ProductsFilterParams productsFilterParams)
        {
            log.LogMethodEntry(productsFilterParams);

            try
            {
                // log.LogMethodExit();
                // return GetAllProductList(BuildProductSearchParametersList(productsFilterParams));
                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = BuildProductSearchParametersList(productsFilterParams);
                int count = 0;
                string selectProductQuery = @"select *, null as maxQtyPerDay, null as hsnSacCode, null as  webDescription, null as orderTypeId, null as isGroupMeal, null as membershipId, null as cardSale,
                                                       null as zoneCode, null as lockerMode, null as taxName, null as usedInDiscounts,
                                                       ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') as Sellable ,
                                                       p.ExternalSystemReference
                                                 from products p, product_type pt
                                                 where p.product_type_id = pt.product_type_id 
                                                   and pt.product_type_id in (select ptin.product_type_id 
                                                                            from product_type ptin
                                                                           where ptin.cardSale ='Y' 
                                                                             and ptin.active_flag='Y'
                                                                             and ptin.product_type not in ('NEW','REFUND','CARDDEPOSIT','CARDSALE','LOCKER','LOCKER_RETURN')) 
                                                   and not exists (SELECT 1 
                                                                     from productGames pg
                                                                     where pg.product_id = p.product_id
                                                                       and pg.game_id is not null
                                                                       and dbo.GetGameProfileValue(pg.game_id, 'QUEUE_SETUP_REQUIRED') ='1' ) ";

                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    StringBuilder query = new StringBuilder(" ");
                    foreach (KeyValuePair<ProductsDTO.SearchByProductParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            string joinOperartor = " and ";

                            if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_ID) ||
                                searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_ID) ||
                                 // searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.FACILITY_MAP_ID) ||
                                 searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.CHECKIN_FACILITY_ID))
                            {
                                query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value + " )");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST))
                            {
                                query.Append(joinOperartor  + DBSearchParameters[searchParameter.Key] + " IN  " + "(" + searchParameter.Value + " )");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.SITEID))
                            {
                                query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value + " OR -1=" + searchParameter.Value + ")");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME))
                            {
                                query.Append(joinOperartor + "( " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' )");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST) ||
                                searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE_LIST))
                            {
                                query.Append(joinOperartor + "( " + DBSearchParameters[searchParameter.Key] + ") IN (" + searchParameter.Value + " )");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.DISPLAY_IN_POS))
                            {
                                query.Append(joinOperartor + "( " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "') ");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.ISACTIVE))
                            {
                                query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N") + "'");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.LAST_UPDATED_FROM_DATE))
                            {
                                query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.LAST_UPDATED_TO_DATE))
                            {
                                query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCTS_ENTITY_LAST_UPDATED_FROM_DATE))
                            {
                                query.Append(joinOperartor + @"( EXISTS( SELECT 1 FROM ComboProduct comboP  
																	WHERE comboP.Product_Id = p.product_id
																	and comboP.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL 
																SELECT 1 FROM FacilityMap vf, ProductsAllowedInFacility paif
																	WHERE paif.ProductsId=p.product_id
																	and paif.FacilityMapId=vf.FacilityMapId
																	and vf.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL 
																SELECT 1 FROM FacilityMapDetails vfd, ProductsAllowedInFacility paif
																	WHERE paif.ProductsId=p.product_id
																	and paif.FacilityMapId=vfd.FacilityMapId
																	and vfd.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL
																SELECT 1 FROM CheckInFacility cif, FacilityMapDetails fmd, ProductsAllowedInFacility paif
																	WHERE paif.ProductsId = p.product_id
																	AND fmd.FacilityMapId = paif.FacilityMapId
																	AND cif.FacilityId = fmd.FacilityId
																	AND cif.last_updated_date > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL
																SELECT 1 FROM CustomData cd
																	WHERE cd.CustomDataSetId=p.CustomDataSetId
																	AND cd.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
                                                            ) 
															OR ( " + DBSearchParameters[searchParameter.Key] + " > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "' ))");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.CUSTOM_DATA_SET_IS_SET) ||
                                     searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE_IS_SET))
                            {
                                query.Append(joinOperartor + "ISNULL( CASE WHEN " + DBSearchParameters[searchParameter.Key] + " IS NOT NULL THEN '1' ELSE '0' END, '0')= '" + searchParameter.Value + "'");
                            }
                            else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.IS_SUBSCRIPTION_PRODUCT))
                            {
                                query.Append(joinOperartor + " CASE WHEN (SELECT top 1 1 FROM productSubscription ps where ps.productsId = " + DBSearchParameters[searchParameter.Key] + " ) = 1 THEN 1 ELSE 0 END = " + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0") + " ");
                            }
                            else if (searchParameter.Key == ProductsDTO.SearchByProductParameters.IS_A_SUBSCRIPTION_PRODUCT_OR_HAS_ACTIVE_SUBSCRIPTION_CHILD)
                            {
                                query.Append(joinOperartor + @" ISNULL(
                                                            (SELECT TOP 1 1 from (
                                                            SELECT 1 as aCol --self
                                                               FROM ProductSubscription ps 
                                                              WHERE ps.productsId  = " + DBSearchParameters[searchParameter.Key] + @"  
                                                              UNION ALL
                                                             SELECT 1 as aCol --for child products
                                                               FROM ProductSubscription ps, 
                                                                    ComboProduct combop
                                                              WHERE combop.Product_Id = " + DBSearchParameters[searchParameter.Key] + @" 
                                                                AND ps.ProductsId = combop.ChildProductId 
                                                                AND ISNULL(combop.IsActive,1) = 1
                                                               UNION ALL
                                                              SELECT 1 as aCol-- for catagory products
                                                                FROM ProductSubscription ps,  
                                                                     ComboProduct combop, products catgp
                                                               WHERE combop.Product_Id = " + DBSearchParameters[searchParameter.Key] + @" 
                                                                 AND catgp.CategoryId = comboP.CategoryId
                                                                 AND ps.ProductsId = catgp.product_id
                                                                 AND ISNULL(combop.IsActive,1) = 1
                                                                ) as aaa),0) = " + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0"));
                            }
                            else
                            {
                                query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",'') like N'%" + searchParameter.Value + "%'");
                            }
                            //else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.ALLOWED_IN_FACILITY_ID))
                            //{
                            //    query.Append(joinOperartor + "( exists (select 1 from ProductsAllowedInFacility paif where paif.FacilityId = " + searchParameter.Value + " and p.product_id = paif.ProductsId)) ");
                            //}                            count++;
                        }
                        else
                        {
                            log.Error("Throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                            throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        }
                    }

                    if (searchParameters.Count > 0)
                        selectProductQuery = selectProductQuery + query;
                    selectProductQuery = selectProductQuery + " Order by product_name";
                }

                DataTable productsData = dataAccessHandler.executeSelectQuery(selectProductQuery, null);
                List<ProductsDTO> productsDTOList = new List<ProductsDTO>();
                if (productsData.Rows.Count > 0)
                {

                    foreach (DataRow dataRow in productsData.Rows)
                    {
                        ProductsDTO productsDTOObject = GetProductDTO(dataRow);
                        productsDTOList.Add(productsDTOObject);
                    }
                }

                log.LogMethodExit(productsDTOList);
                return productsDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// BuildProductSearchParametersList
        /// </summary>
        /// <param name="productsFilterParams"></param>
        /// <returns></returns>
        public List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> BuildProductSearchParametersList(ProductsFilterParams productsFilterParams)
        {
            log.Debug("Starts-BuildProductSearchParametersList Method");
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> productSearchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            if (productsFilterParams != null)
            {
                if (productsFilterParams.ProductTypeId == -1 && !(string.IsNullOrEmpty(productsFilterParams.ProductType)))
                {
                    productsFilterParams.ProductTypeId = GetProductTypeId(productsFilterParams.ProductType);
                }
                if (productsFilterParams.IsActive)
                {
                    productSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, productsFilterParams.IsActive ? "Y" : "N"));
                }
                if (productsFilterParams.ProductId > 0)
                    productSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, productsFilterParams.ProductId.ToString()));
                if (productsFilterParams.ProductTypeId > 0)
                    productSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_ID, productsFilterParams.ProductTypeId.ToString()));
                if (productsFilterParams.SiteId > 0)
                    productSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, productsFilterParams.SiteId.ToString()));
                if (!string.IsNullOrEmpty(productsFilterParams.HsnSacCode))
                    productSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.HSNSACCODE, productsFilterParams.HsnSacCode.ToString()));
                if (!string.IsNullOrEmpty(productsFilterParams.ExternalSystemReference))
                    productSearchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE, productsFilterParams.ExternalSystemReference.ToString()));
            }
            log.Debug("Starts-BuildProductSearchParametersList Method");
            return productSearchParams;
        }



        private int GetProductTypeId(string productType)
        {
            log.LogMethodEntry(productType);
            int productTypeId = -1;
            string productQuery = @" select * from product_type where product_type=@product_type";

            SqlParameter[] productparameters = new SqlParameter[1];
            productparameters[0] = new SqlParameter("@product_type", productType);

            DataTable dtproductDTO = dataAccessHandler.executeSelectQuery(productQuery, productparameters);

            if (dtproductDTO.Rows.Count > 0)
            {
                int.TryParse(dtproductDTO.Rows[0]["product_type_id"].ToString(), out productTypeId);

            }
            log.LogMethodExit(productTypeId);
            return productTypeId;
        }

        private string BuildProductQuery(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            int count = 0;
            StringBuilder query = new StringBuilder(" ");
            foreach (KeyValuePair<ProductsDTO.SearchByProductParameters, string> searchParameter in searchParameters)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    string joinOperartor = " and ";

                    if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_ID) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_ID) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.POS_TYPE_ID) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.CHECKIN_FACILITY_ID) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRICE) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.CATEGORY_ID) ||
                           searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.TAX_ID) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.WAIVER_SET_ID) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.MINIMUM_QUANTITY)||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.CUSTOM_DATA_SET_ID))
                    {
                        query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value + " )");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.SITEID) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.MASTER_ENTITY_ID))
                    {
                        query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value + " OR -1=" + searchParameter.Value + ")");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.CUSTOM_DATA_SET_ID_LIST))
                    {
                        query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " IN  (" + searchParameter.Value + ") )");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME)
                        || searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_GUID)
                        || searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.SHORT_NAME))
                    {
                        query.Append(joinOperartor + "( " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' )");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_NAME))
                    {
                        query.Append(joinOperartor + "( " + DBSearchParameters[searchParameter.Key] + " like " + "'%" + searchParameter.Value + "%') ");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_EXACT_NAME)) // Used for CenterEdge to fetch the Variable card product based on the name
                    {
                        query.Append(joinOperartor + "( " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' )");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.ISACTIVE))
                    {
                        query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N") + "'");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.ISRECOMMENDED) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.SERVICE_CHARGE_IS_APPLICABLE) ||
                        searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.GRATUITY_IS_APPLICABLE))
                    {
                        query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N") + "'");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST) ||
                            searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE_LIST))
                    {
                        query.Append(joinOperartor + "( " + DBSearchParameters[searchParameter.Key] + ") IN (" + searchParameter.Value + " )");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.DISPLAY_IN_POS))
                    {
                        query.Append(joinOperartor + "( " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "') ");
                    }
                    //else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.ALLOWED_IN_FACILITY_ID))
                    //{
                    //    query.Append(joinOperartor + "( exists (select 1 from ProductsAllowedInFacility paif where paif.FacilityId = " + searchParameter.Value + " and p.product_id = paif.ProductsId)) ");
                    //}
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.HAS_MODIFIER))
                    {
                        query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " = 'Y' or exists (select 1 from ModifierSetDetails pm where pm.ModifierProductId = p.product_Id))");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.HSNSACCODE))
                    {
                        query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' ");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.DISPLAY_GROUP_NAME))
                    {
                        query.Append(joinOperartor + "p.product_id in (select pdg.ProductId from ProductsDisplayGroup pdg, ProductDisplayGroupFormat pdgf  where pdg.DisplayGroupId = pdgf.id  and pdgf.DisplayGroup like '%" + searchParameter.Value + "%')");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_DISPLAY_GROUP_FORMAT_ID))
                    {
                        query.Append(joinOperartor + "p.product_id in (select pdg.ProductId from ProductsDisplayGroup pdg, ProductDisplayGroupFormat pdgf  where pdg.DisplayGroupId = pdgf.id  and pdg.DisplayGroupId = " + searchParameter.Value + ")");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_DISPLAY_GROUP_FORMAT_ID_LIST))
                    {
                        query.Append(joinOperartor + "p.product_id in (select pdg.ProductId from ProductsDisplayGroup pdg, ProductDisplayGroupFormat pdgf  where pdg.DisplayGroupId = pdgf.id  and pdg.DisplayGroupId IN  (" + searchParameter.Value + "))");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.IS_SELLABLE))
                    {
                        query.Append(joinOperartor + "(ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') = '" + searchParameter.Value + "')");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.LAST_UPDATED_FROM_DATE))
                    {
                        query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.LAST_UPDATED_TO_DATE))
                    {
                        query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCTS_ENTITY_LAST_UPDATED_FROM_DATE))
                    {
                        query.Append(joinOperartor + @"( EXISTS( SELECT 1 FROM ComboProduct comboP  
																	WHERE comboP.Product_Id = p.product_id
																	and comboP.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL 
																SELECT 1 FROM FacilityMap vf, ProductsAllowedInFacility paif
																	WHERE paif.ProductsId=p.product_id
																	and paif.FacilityMapId=vf.FacilityMapId
																	and vf.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL 
																SELECT 1 FROM FacilityMapDetails vfd, ProductsAllowedInFacility paif
																	WHERE paif.ProductsId=p.product_id
																	and paif.FacilityMapId=vfd.FacilityMapId
																	and vfd.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL
																SELECT 1 FROM CheckInFacility cif, FacilityMapDetails fmd, ProductsAllowedInFacility paif
																	WHERE paif.ProductsId = p.product_id
																	AND fmd.FacilityMapId = paif.FacilityMapId
																	AND cif.FacilityId = fmd.FacilityId
																	AND cif.last_updated_date > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL
																SELECT 1 FROM CustomData cd
																	WHERE cd.CustomDataSetId=p.CustomDataSetId
																	AND cd.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
                                                            ) 
															OR ( " + DBSearchParameters[searchParameter.Key] + " > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "' ))");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.CUSTOM_DATA_SET_IS_SET) ||
                             searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE_IS_SET))
                    {
                        query.Append(joinOperartor + "ISNULL( CASE WHEN " + DBSearchParameters[searchParameter.Key] + " IS NOT NULL THEN '1' ELSE '0' END, '0')= '" + searchParameter.Value + "'");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.IS_SUBSCRIPTION_PRODUCT))
                    {
                        query.Append(joinOperartor + " CASE WHEN (SELECT top 1 1 FROM productSubscription ps where ps.productsId = " + DBSearchParameters[searchParameter.Key] + " ) = 1 THEN 1 ELSE 0 END = " + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0") + " ");
                    }
                    else if (searchParameter.Key == ProductsDTO.SearchByProductParameters.IS_A_SUBSCRIPTION_PRODUCT_OR_HAS_ACTIVE_SUBSCRIPTION_CHILD)
                    {
                        query.Append(joinOperartor + @" ISNULL(
                                                            (SELECT TOP 1 1 from (
                                                            SELECT 1 as aCol --self
                                                               FROM ProductSubscription ps 
                                                              WHERE ps.productsId  = " + DBSearchParameters[searchParameter.Key] + @"  
                                                              UNION ALL
                                                             SELECT 1 as aCol --for child products
                                                               FROM ProductSubscription ps, 
                                                                    ComboProduct combop
                                                              WHERE combop.Product_Id = " + DBSearchParameters[searchParameter.Key] + @" 
                                                                AND ps.ProductsId = combop.ChildProductId 
                                                                AND ISNULL(combop.IsActive,1) = 1
                                                               UNION ALL
                                                              SELECT 1 as aCol-- for catagory products
                                                                FROM ProductSubscription ps,  
                                                                     ComboProduct combop, products catgp
                                                               WHERE combop.Product_Id = " + DBSearchParameters[searchParameter.Key] + @" 
                                                                 AND catgp.CategoryId = comboP.CategoryId
                                                                 AND ps.ProductsId = catgp.product_id
                                                                 AND ISNULL(combop.IsActive,1) = 1
                                                                ) as aaa),0) =  " + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0"));
                    }
                    else
                    {
                        query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",'') like N'%" + searchParameter.Value + "%'");
                    }
                    count++;
                }
                else
                {
                    log.Debug("Ends-GetAchievementClassesList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                    throw new Exception("The query parameter does not exist " + searchParameter.Key);
                }
            }

            return query.ToString();
        }

        /// <summary>
        /// GetAllProductList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetAllProductList(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                String query = BuildProductQuery(searchParameters);

                if (!string.IsNullOrEmpty(query))
                    selectProductQuery = selectProductQuery + query;

                selectProductQuery = selectProductQuery + " Order by product_id";
            }
            List<ProductsDTO> list = null;
            DataTable table = dataAccessHandler.executeSelectQuery(selectProductQuery, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetAllProductList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetProductsList(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters, int currentPage, int pageSize, int languageId = -1)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectProductQuery = @"select *, null as maxQtyPerDay, null as hsnSacCode, null as  webDescription, null as orderTypeId, null as isGroupMeal, null as membershipId, null as cardSale,
                                               null as zoneCode, null as lockerMode, null as taxName, null as usedInDiscounts,
                                         ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') as Sellable ,
                                                p.ExternalSystemReference, isnull((select translation 
                                                              from ObjectTranslations
                                                             where ElementGuid = p.Guid
                                                               and Element = 'PRODUCT_NAME'
                                                                and LanguageId = @lang), p.product_name) as TranslatedProductName, 
                                                    isnull((select translation 
                                                            from ObjectTranslations
                                                            where ElementGuid = p.Guid
                                                            and Element = 'DESCRIPTION'
                                                            and LanguageId = @lang), p.description) as TranslatedProductDescription
                                          from products p, product_type pt  where pt.product_type_id = p.product_type_id ";

            SqlParameter[] queryParameters = new SqlParameter[1];
            queryParameters[0] = new SqlParameter("@lang", languageId);

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                String query = BuildProductQuery(searchParameters);

                if (searchParameters.Count > 0)
                {
                    selectProductQuery = selectProductQuery + query;
                }
                if (currentPage >= 0 && pageSize > 0)
                {
                    selectProductQuery += " ORDER BY p.product_id OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                    selectProductQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
                }
            }

            List<ProductsDTO> list = null;
            DataTable table = dataAccessHandler.executeSelectQuery(selectProductQuery, queryParameters, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetAllProductList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetTransactionProductsList(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectProductQuery = @"SELECT p.product_id, isnull(plp.Price, p.price) Price,  p.* , '' product_type, '' as cardSale, '' as zoneCode, '' as lockerMode, '' as taxName, 'false' as usedInDiscounts, '' as Sellable
                                            FROM POSMachines POSM,
                                            PRODUCTS p
                                            INNER JOIN ProductsDisplayGroup pd on pd.ProductId = p.product_id  
                                            LEFT OUTER JOIN ProductDisplayGroupFormat pdf on pdf.Id = pd.DisplayGroupId
                                            left outer join (select top 1 ProductId, Price
                                                            from PriceListProducts
                                                            where ProductId in ( @ProductIdList)
                                                            and PriceListId = (select top 1 *
                                                                               from (select PriceListId
                                                                                       from Membership
                                                                                       where MembershipId = @MembershipId
                                                                                   AND(Membership.site_id is null or Membership.site_id = @siteId or  @siteId = -1)
                                                                                       union all
                                                                                   select PriceListId
                                                                                       from UserRolePriceList
                                                                                           where Role_id = @RoleId
                                                                                       AND(UserRolePriceList.site_id is null or UserRolePriceList.site_id = @siteId or  @siteId = -1)
                                                                                       union all
                                                                                       select PriceListId
                                                                                       from PriceList
                                                                                       where PriceListName = 'Default'
                                                                                       AND(PriceList.site_id is null or PriceList.site_id = @siteId or  @siteId = -1)
                                                                                   ) v
                                                                               )
                                                            and isnull(EffectiveDate, getdate()-10000) <= getdate()
                                                            order by EffectiveDate desc) plp on plp.ProductId = p.product_id,
                                            product_type pt 
                                            WHERE p.product_type_id = pt.product_type_id and
                                            (POSM.IPAddress = @posMachineName 
                                                 OR POSM.Computer_Name = @posMachineName
                                                 OR POSM.PosName = @posMachineName)
                                                AND(p.site_id is null or p.site_id = @siteId or  @siteId = -1)
                                                AND p.active_flag = 'Y'
                                             AND (isnull(p.StartDate, @today) <= @today and isnull(p.ExpiryDate, @today) >= @today)
                                                AND(p.POSTypeId = POSM.POSTypeId or p.POSTypeId is null)
                                                AND NOT EXISTS(SELECT POSPE.ProductDisplayGroupFormatId
                                                                FROM POSProductExclusions POSPE
                                                                WHERE POSPE.POSMachineId = POSM.POSMachineId
                                                                AND POSPE.ProductDisplayGroupFormatId = Pdf.Id)
                                                AND NOT EXISTS(SELECT USRPE.RoleDisplayGroupId
                                                                FROM UserRoleDisplayGroupExclusions USRPE
                                                                WHERE USRPE.Role_id = @RoleId
                                                                AND USRPE.ProductDisplayGroupId = Pdf.Id)
                                                 AND(not exists(select 1 from ProductCalendar pc where pc.product_id = p.product_id)
                                                    or exists(select 1 from(select top 1 date, day,
                                                                        case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort,
                                                                        isnull(FromTime, @nowHour) FromTime, isnull(ToTime, @nowHour) ToTime, ShowHide
                                                                        from ProductCalendar pc
                                                                        where pc.product_id = p.product_id
                                                                            and (Date = @today
                                                                            or Day = @DayNumber
                                                                            or Day = @weekDay
                                                                            or Day = -1)
                                                                        order by 1 desc, 2 desc, 3) inView
                                                                        where(ShowHide = 'Y'
                                                                            and(@nowHour >= FromTime and @nowHour <= case ToTime when 0 then 24 else ToTime end))
                                                                            or(ShowHide = 'N'
                                                                            and(@nowHour < FromTime or @nowHour > case ToTime when 0 then 24 else ToTime end))))
                                                AND(POSM.site_id is null or POSM.site_id = @siteId or  @siteId = -1)
                                                AND isnull(p.DisplayInPOS, 'N') = 'Y'
                                                AND p.Product_Id in ( @ProductIdList)  
                                                AND isnull(p.DisplayInPOS, 'Y') = 'Y'
                                                ORDER BY ISNULL(pdf.sortOrder,(select top 1 SortOrder from ProductDisplayGroupFormat where DisplayGroup = 'Others')), display_group, sort_order";

            List<SqlParameter> queryParameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                foreach (KeyValuePair<ProductsDTO.SearchByProductParameters, string> searchParameter in searchParameters)
                {
                    if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST))
                    {
                        selectProductQuery = selectProductQuery.Replace("@ProductIdList", dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value));
                        queryParameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.TRX_ONLY_PRODUCT_PURCHASE_DATE))
                    {
                        DateTime purchaseDate = Convert.ToDateTime(searchParameter.Value);
                        queryParameters.Add(new SqlParameter("@today", purchaseDate.Date));
                        queryParameters.Add(new SqlParameter("@nowHour", purchaseDate.Hour + purchaseDate.Minute / 100.0));
                        int dayofweek = -1;
                        switch (purchaseDate.DayOfWeek)
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
                        queryParameters.Add(new SqlParameter("@weekDay", dayofweek));
                        queryParameters.Add(new SqlParameter("@DayNumber", purchaseDate.Day + 1000)); // day of month stored as 1000 + day of month
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.SITEID))
                    {
                        queryParameters.Add(new SqlParameter("@siteId", dataAccessHandler.GetParameterValue(searchParameter.Value)));
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.TRX_ONLY_POS_MACHINE))
                    {
                        queryParameters.Add(new SqlParameter("@posMachineName", dataAccessHandler.GetParameterValue(searchParameter.Value)));
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.TRX_ONLY_USER_ROLE))
                    {
                        queryParameters.Add(new SqlParameter("@RoleId", dataAccessHandler.GetParameterValue(searchParameter.Value)));
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.TRX_ONLY_MEMBERSHIP))
                    {
                        queryParameters.Add(new SqlParameter("@MembershipId", dataAccessHandler.GetParameterValue(searchParameter.Value)));
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.OFFSET))
                    {
                        queryParameters.Add(new SqlParameter("@offSetDuration", dataAccessHandler.GetParameterValue(searchParameter.Value)));
                    }
                    else
                    {
                        queryParameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), dataAccessHandler.GetParameterValue(searchParameter.Value)));
                    }
                }
            }

            List<ProductsDTO> list = null;
            DataTable table = dataAccessHandler.executeSelectQuery(selectProductQuery, queryParameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Retruns the no of products details count
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public int GetProductsDTOCount(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectProductQuery = @"select *, null as maxQtyPerDay, null as hsnSacCode, null as  webDescription, null as orderTypeId, null as isGroupMeal, null as membershipId, null as cardSale,
                                               null as zoneCode, null as lockerMode, null as taxName, null as usedInDiscounts,
                                         ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') as Sellable ,
                                               p.ExternalSystemReference
                                          from products p, product_type pt  where pt.product_type_id = p.product_type_id ";

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                String query = BuildProductQuery(searchParameters);

                if (!string.IsNullOrWhiteSpace(query))
                {
                    selectProductQuery = selectProductQuery + query;
                }
                selectProductQuery = selectProductQuery + " Order by product_id";
            }
            int productsCount = 0;
            DataTable table = dataAccessHandler.executeSelectQuery(selectProductQuery, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productsCount = table.Rows.Count;
            }
            log.LogMethodExit(productsCount);
            return productsCount;
        }

        /// <summary>
        /// method to get ModifierSet deatils by passing ModifierSetId
        /// </summary>
        /// <returns>ProductsModifierSetStruct object</returns>
        public ProductsModifierSetStruct GetModifierSet(int modifierSetId)
        {
            log.Debug("Starts - GetParentModifierSet(int parentModifierSetId).");
            ProductsModifierSetStruct productModifierSet = null;
            try
            {
                string parentProductsModifierQuery = @"select ModSet.ModifierSetId ModifierSetId,
                                                                ModSet.SetName SetName,
                                                                ISNULL(PrdMod.AutoShowInPos, 'N') ModifierAutoShowInPOS,
	                                                            ModSetDet.ModifierProductId ModifierProductId,
                                                                ISNULL(ModSetDet.price, -1) ModifierProductPrice,
                                                                ISNULL(ModSet.MinQuantity,0) MinQuantity,
                                                                ISNULL(ModSet.MaxQuantity,0) MaxQuantity,
                                                                ISNULL(ModSet.FreeQuantity,0) FreeQuantity, 
	                                                            prod.product_name productName
                                                                from ModifierSetDetails ModSetDet 
                                                                LEFT OUTER JOIN ModifierSet ModSet on ModSetDet.ModifierSetId = ModSet.ModifierSetId
	                                                            LEFT OUTER JOIN products Prod on prod.product_id = ModSetDet.ModifierProductId 
                                                                LEFT OUTER JOIN ProductModifiers PrdMod on PrdMod.ProductId = ModSetDet.ModifierProductId
	                                                            where ModSet.ModifierSetId = @modifierSetId ";

                SqlParameter[] parentProductsModifierParameters = new SqlParameter[1];
                parentProductsModifierParameters[0] = new SqlParameter("@modifierSetId", modifierSetId);

                DataTable dtModifiersData = dataAccessHandler.executeSelectQuery(parentProductsModifierQuery, parentProductsModifierParameters);
                foreach (DataRow modifiersRow in dtModifiersData.Rows)
                {
                    if (productModifierSet == null)
                    {
                        productModifierSet = new ProductsModifierSetStruct(Convert.ToInt32(modifiersRow["ModifierSetId"].ToString()),
                                                                                  modifiersRow["SetName"].ToString(),
                                                                                  modifiersRow["ModifierAutoShowInPOS"].ToString(),
                                                                                  modifiersRow["MinQuantity"] == null ? 0 : Convert.ToInt32(modifiersRow["MinQuantity"]),
                                                                                  modifiersRow["MaxQuantity"] == null ? 0 : Convert.ToInt32(modifiersRow["MaxQuantity"]),
                                                                                  modifiersRow["FreeQuantity"] == null ? 0 : Convert.ToInt32(modifiersRow["FreeQuantity"]),
                                                                                  null);
                    }
                    productModifierSet.AddModifier(Convert.ToInt32(modifiersRow["ModifierProductId"].ToString()), modifiersRow["productName"].ToString(), Convert.ToDouble(modifiersRow["ModifierProductPrice"]));
                }
                log.Debug("Ends - GetParentModifierSet(int parentModifierSetId) by returning parentProductsModifierSet.");
                return productModifierSet;
            }
            catch (Exception expn)
            {
                log.Debug("Ends - GetParentModifierSet(int parentModifierSetId) by throwing exception.");
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Check for queue products and return true/false
        /// </summary>
        /// <returns>True if queue products doesnt exists</returns>
        public bool CheckQueueProductsExistence(int cardId, int productId, string productIdList)
        {
            log.LogMethodEntry(cardId, productId, productIdList);

            string checkQueueProductsQuery = @"select top 1 1 
                                                  from (select 1 a) v
                                                where (exists (select 1 
                                                                 from cardGames cg
                                                                where cg.card_id = @card_id
                                                                  and cg.BalanceGames > 0
                                                                  and (cg.ExpiryDate is null or cg.ExpiryDate > getdate())
                                                                  and dbo.GetGameProfileValue(cg.game_id, 'QUEUE_SETUP_REQUIRED') = '1')
                                                    or exists (select 1 
                                                               from productGames pg
                                                              where pg.product_id in @productIdList
                                                              and dbo.GetGameProfileValue(pg.game_id, 'QUEUE_SETUP_REQUIRED') = '1'))
                                                  and exists (select 1 
                                                               from productGames pg
                                                              where pg.product_id = @ProductId
                                                                and dbo.GetGameProfileValue(pg.game_id, 'QUEUE_SETUP_REQUIRED') = '1')";

            SqlParameter[] checkQueueProductsParams = new SqlParameter[3];
            checkQueueProductsParams[0] = new SqlParameter("@cardId", cardId);
            checkQueueProductsParams[1] = new SqlParameter("@productIdList", productIdList);
            checkQueueProductsParams[2] = new SqlParameter("@productId", productId);

            DataTable dtcheckQueueProducts = dataAccessHandler.executeSelectQuery(checkQueueProductsQuery, checkQueueProductsParams, sqlTransaction);
            if (dtcheckQueueProducts.Rows.Count > 0)
            {
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }

        internal List<ProductsDTO> GetSystemProductsDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            selectProductQuery = selectProductQuery + " AND product_type IN ('CREDITCARDSURCHARGE', 'CARDDEPOSIT', 'DEPOSIT', 'LOCKERDEPOSIT', 'EXCESSVOUCHERVALUE','LOADTICKETS','LOYALTY', 'SERVICECHARGE', 'GENERICSALE', 'GRATUITY') and (p.site_id = @site_id or @site_id = -1) ";

            selectProductQuery = selectProductQuery + " Order by product_id";
            List<ProductsDTO> list = null;
            DataTable table = dataAccessHandler.executeSelectQuery(selectProductQuery, new[]{new SqlParameter("@site_id", siteId) }, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Get Checkin slab pricing
        /// </summary>
        /// <param name="productID">Product id</param>
        /// <param name="overdue">Time overdue in minutes</param>
        /// <returns>updated slab price if available</returns>
        public decimal? GetCheckInSlabPrice(int productID, int overdue)
        {
            log.LogMethodEntry(productID, overdue);

            string slabPriceQuery = @"select top 1 price 
                                         from CheckInPrices 
                                        where productId = @product_id 
                                          and TimeSlab >= @overdue order by TimeSlab";
            SqlParameter[] slabPricingParams = new SqlParameter[2];
            slabPricingParams[0] = new SqlParameter("@product_id", productID);
            slabPricingParams[1] = new SqlParameter("@overdue", overdue);
            object oSlabPrice = dataAccessHandler.executeScalar(slabPriceQuery, slabPricingParams, sqlTransaction);
            if (oSlabPrice != null)
            {
                decimal oprice = Convert.ToDecimal(oSlabPrice);
                return oprice;
            }
            else
                return null;
        }

        /// <summary>
        /// Special pricing
        /// </summary>
        /// <param name="productID">Product id</param>
        /// <param name="price">Current price</param>
        /// <param name="specialPricingId">Special pricing id</param>
        /// <returns>updated price if special price available</returns>
        public double GetSpecialPrice(int productID, double price, int specialPricingId)
        {
            log.LogMethodEntry(productID, price, specialPricingId);

            if (specialPricingId != -1)
            {
                string specialPricingQuery = @"select isnull(psp.price, isnull(sp.percentage/100 * @price, 0)) 
                                                  from products pr left outer join productSpecialPricing psp 
                                                  on pr.product_id = psp.productId 
                                                  and psp.pricingId = @pricingId, specialPricing sp 
                                                  where pr.product_id = @product_id 
                                                  and sp.pricingId = @pricingId";
                SqlParameter[] specialPricingParams = new SqlParameter[3];
                specialPricingParams[0] = new SqlParameter("@pricingId", specialPricingId);
                specialPricingParams[1] = new SqlParameter("@product_id", productID);
                specialPricingParams[2] = new SqlParameter("@price", price);
                object oSpecialPrice = dataAccessHandler.executeScalar(specialPricingQuery, specialPricingParams, sqlTransaction);
                if (oSpecialPrice != null)
                {
                    double oprice = Convert.ToDouble(oSpecialPrice);
                    return oprice;
                }
                else
                {
                    log.LogMethodExit(price);
                    return price;
                }
            }
            else
            {
                log.LogMethodExit(price);
                return price;
            }
        }

        /// <summary>
        /// method to get the upsell product id based on passed parent product id.
        /// </summary>
        /// <param name="productId">parent product id</param>
        /// <returns>upsell product id</returns>
        public int GetUpsellProductId(int productId)
        {
            log.Debug("Ends- GetUpsellProductId(int productId).");
            int upsellProductId = -1;
            string fetchUpsellProductQuery = @"select Top 1 uso.OfferProductId from UpsellOffers uso 
                                                                    inner join Products p on uso.ProductId = p.product_id 
                                                                    and p.product_id = @productId                                                               
                                                                    where uso.activeflag = 1  
                                                                    order by uso.OfferId";

            SqlParameter[] upsellProductQueryParams = new SqlParameter[1];
            upsellProductQueryParams[0] = new SqlParameter("@productId", productId);

            DataTable upsellProductsDataTable = dataAccessHandler.executeSelectQuery(fetchUpsellProductQuery, upsellProductQueryParams);
            if (upsellProductsDataTable.Rows.Count > 0)
                upsellProductId = Convert.ToInt32(upsellProductsDataTable.Rows[0]["OfferProductId"].ToString());
            log.Debug("Ends- GetUpsellProductId(int productId).");
            return upsellProductId;
        }

        public double UpdateParentModifierPrice(PurchasedProducts productsDTO, ModifierSetDTO modifiersDTO)
        {
            log.LogMethodEntry(productsDTO, modifiersDTO);
            double price = 0;

            string UpdatePriceQuery = @"Declare @price nvarchar(100)		
                            IF NOT EXISTS (SELECT 1 from ParentModifiers 
                                                WHERE ModifierId =  @modifierSetId                                                  
                                       AND ParentModifierId = (SELECT top 1 Id 
										            FROM ModifierSetDetails 
										            WHERE ModifierProductId= @parentProductId 
										            AND ModifierSetId = @parentModifierSetId))                                           
                            BEGIN   
                                SET @price = (SELECT Price 
                                              FROM products 
                                              WHERE product_id = @parentProductId )
                            END
                        ELSE
                            BEGIN
                                SELECT @price = (SELECT price 
                                                FROM ParentModifiers 
                                                WHERE ModifierId = @modifierSetId
                                                AND ParentModifierId = (SELECT top 1 Id 
                                                                        FROM ModifierSetDetails 
                                                                        WHERE ModifierProductId= @parentProductId 
                                                                        AND ModifierSetId =@parentModifierSetId))
                                IF @price IS NULL
                                BEGIN
                                     SET @price = (SELECT Price 
                                                  FROM products 
                                                  WHERE product_id = @parentProductId)
                                END                   
                            END
                        SELECT @price";

            object parentProductId = productsDTO.ParentModifierProduct.ProductId;
            object productId = productsDTO.ProductId;
            object modifierSetId = modifiersDTO.ModifierSetId;
            object parentModifierSetId = modifiersDTO.ParentModifierSetId;

            try
            {
                List<SqlParameter> updateProductPriceParameters = new List<SqlParameter>();
                updateProductPriceParameters.Add(new SqlParameter("@productId", productId));
                updateProductPriceParameters.Add(new SqlParameter("@parentProductId", parentProductId));
                updateProductPriceParameters.Add(new SqlParameter("@modifierSetId", modifierSetId));
                updateProductPriceParameters.Add(new SqlParameter("@parentModifierSetId", parentModifierSetId));
                DataTable parentProductPrice = dataAccessHandler.executeSelectQuery(UpdatePriceQuery, updateProductPriceParameters.ToArray());

                log.LogVariableState("@parentProductId", parentProductId);
                log.LogVariableState("@productId", productId);
                log.LogVariableState("@modifierSetId", modifierSetId);
                log.LogVariableState("@parentModifierSetId", parentModifierSetId);

                if (parentProductPrice != null && parentProductPrice.Rows.Count > 0)
                {
                    double.TryParse(parentProductPrice.Rows[0][0].ToString(), out price);

                }

                //if (!DBNull.Value.Equals(parentProductPrice))
                //    price = Convert.ToDecimal(parentProductPrice);
            }
            catch (Exception e1)
            {
                log.Error("Unable to execute Update Price Query", e1);
                throw new Exception(e1.ToString());
            }
            return price;
        }

        public List<ProductsDTO> getSplitProductList(double price, string productType)
        {
            List<ProductsDTO> productsDTOList = new List<ProductsDTO>();
            try
            {
                string query = @"select p.*, null as maxQtyPerDay, null as hsnSacCode, 
                                    null as  webDescription, null as orderTypeId, null as isGroupMeal, 
                                    null as membershipId, null as cardSale, null as zoneCode, 
                                    null as lockerMode, null as taxName, null as usedInDiscounts, pt.product_type as product_type ,
                                    ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') as Sellable 
                                    from products p, product_type pt , CustomDataView v  
                                    where pt.product_type_id = p.product_type_id  
                                    and pt.product_type = @productType
                                    and price <= @price 
                                    and price > 0
                                    and ( ISNULL(p.credits,0) > 0 
                                          OR EXISTS (SELECT 1 -- only A and G type allowed
                                                       FROM ProductCreditPlus pcp
                                                      WHERE pcp.Product_id = p.product_id
                                                         AND ISNULL(pcp.IsActive ,1) = 1
                                                         AND pcp.CreditPlusType in ('A','G')
                                                         AND NOT EXISTS (SELECT 1 
                                                                           FROM ProductCreditPlus pcpin
				                                                          WHERE pcpin.Product_id = pcp.Product_id
                                                                            AND ISNULL(pcpin.IsActive ,1) = 1
                                                                            AND pcpin.CreditPlusType not in ('A','G'))))
                                    and ISNULL(p.active_flag,'Y') = 'Y'
                                    and p.CustomDataSetId = v.CustomDataSetId
                                    and v.Name = 'External System Identifier'
                                    and not exists (Select 1 from UpsellOffers uso 
                                                        where uso.OfferProductId = p.product_id
                                                        and uso.ActiveFlag = 1
                                                        and (isnull(uso.EffectiveDate, getdate()) <= getdate()))
                                    and v.CustomDataNumber is not null							 
								    order by p.price desc";
                SqlParameter[] queryParams = new SqlParameter[2];
                queryParams[0] = new SqlParameter("@price", price);
                queryParams[1] = new SqlParameter("@productType", productType);
                DataTable productsData = dataAccessHandler.executeSelectQuery(query, queryParams);

                if (productsData.Rows.Count > 0)
                {

                    foreach (DataRow dataRow in productsData.Rows)
                    {
                        ProductsDTO productsDTOObject = GetProductDTO(dataRow);
                        productsDTOList.Add(productsDTOObject);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }


            log.LogMethodExit(productsDTOList);
            return productsDTOList;
        }

        /// <summary>
        /// GetProductListTable method
        /// </summary>
        /// <param name="productsFilterParams">ProductsFilterParams</param>
        /// <returns>returns data table</returns>
        public List<ProductsDTO> GetProductListByFilterParameters(ProductsFilterParams productsFilterParams)
        {
            log.LogMethodEntry(productsFilterParams);

            TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
            int offSetDuration = timeZoneUtil.GetOffSetDuration(productsFilterParams.SiteId, productsFilterParams.DateOfPurchase);
            offSetDuration = offSetDuration * (-1);

            string selectProductQuery = @"SELECT isnull((select translation
                                                              from ObjectTranslations
                                                             where ElementGuid = PRD1.Guid
                                                               and Element = 'PRODUCT_NAME'
                                                                and LanguageId = @lang), PRD1.product_name) as product_name, 
                                                    isnull((select translation
                                                            from ObjectTranslations
                                                            where ElementGuid = PRD1.Guid
                                                            and Element = 'DESCRIPTION'
                                                            and LanguageId = @lang), PRD1.description) as description, *,
                                               null as maxQtyPerDay, null as hsnSacCode, null as orderTypeId, null as isGroupMeal, null as membershipId, null as cardSale,
                                               null as zoneCode, null as lockerMode, null as usedInDiscounts, tax.tax_name taxName,
ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = PRD1.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(PRD1.active_flag,'Y')),'Y') as Sellable ,
                                                 PRD1.ExternalSystemReference
                                            FROM PRODUCTS PRD1
                                            INNER JOIN ProductsDisplayGroup pd on pd.ProductId = PRD1.product_id 
                                            LEFT OUTER JOIN ProductDisplayGroupFormat pdf on pdf.Id = pd.DisplayGroupId 
	                                        LEFT OUTER JOIN tax on tax.tax_id = PRD1.tax_id,
	                                            PRODUCT_TYPE PTYPE
                                            WHERE (PRD1.product_id = case when @productId = -1 then PRD1.product_id else @productId end)
                                            AND (PRD1.site_id is null or PRD1.site_id = @siteId or @siteId = -1) 
                                            AND (PRD1.ExpiryDate is null or cast(DATEADD(SECOND, @offSetDuration, PRD1.ExpiryDate) as date) > @today ) 
                                            AND (PRD1.StartDate is null or cast(DATEADD(SECOND, @offSetDuration, PRD1.StartDate)  as date)  <= @today )
                                            AND PRD1.product_type_id = PTYPE.product_type_id
                                            AND PTYPE.product_type not in (" + dataAccessHandler.GetInClauseParameterName(ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST, productsFilterParams.ProductTypeExclude) + @")
                                            AND PTYPE.CardSale = @cardsale
                                            AND Pdf.Id is not null
                                            AND PRD1.active_flag='Y'
                                            AND isnull(PRD1.DisplayInPOS, 'Y') = 'Y'
                                            AND (pdf.id = case when @displayGroupId = -1 then pdf.id else @displayGroupId end)
                                        ORDER BY isnull(prd1.sort_order, 1000), PRD1.product_id";

            DateTime serverTime = productsFilterParams.DateOfPurchase;

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@today", serverTime.Date));
            parameters.Add(new SqlParameter("@siteId", productsFilterParams.SiteId));
            parameters.Add(new SqlParameter("@offSetDuration", offSetDuration));
            parameters.Add(new SqlParameter("@displayGroupId", productsFilterParams.DisplayGroupId));
            parameters.Add(new SqlParameter("@productId", productsFilterParams.ProductId));
            parameters.Add(new SqlParameter("@cardsale", productsFilterParams.RequiresCardProduct ? "Y" : "N"));
            parameters.Add(new SqlParameter("@lang", productsFilterParams.LanguageId));
            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST, productsFilterParams.ProductTypeExclude));
            DataTable productsTable = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray());

            List<ProductsDTO> products = new List<ProductsDTO>();
            for (int i = 0; i < productsTable.Rows.Count; i++)
            {
                ProductsDTO productDTORow = GetProductDTO(productsTable.Rows[i]);
                products.Add(productDTORow);
            }

            log.Debug("Ends-GetProductList(productsFilterParams) method.");

            return products;
        }

        //Get the Booking Products
        //Begin Modification- Dec-30-2015- Modifed to add  description column//
        public DataTable GetBookingProducts(int siteId, string posMachine = "", string loginId = "")
        {
            log.LogMethodEntry(siteId, posMachine, loginId);

            string BookingFilterQuery = "";
            string QueryJoiner = " where ";
            if (string.IsNullOrEmpty(posMachine))
            {
                BookingFilterQuery = "inner join PRODUCT_TYPE PT on p.product_type_id = PT.product_type_id AND P.active_flag='Y' ";

            }
            else
            {
                BookingFilterQuery = @"inner join PRODUCT_TYPE PT on p.product_type_id = PT.product_type_id AND P.active_flag='Y' 
                                            left outer join (SELECT * from (SELECT *, DENSE_RANK() over (partition by ProductId order by CreatedDate ASC) as D 
                                                             from ProductsDisplayGroup) T
                                                             WHERE T.D = 1) pd on pd.ProductId = p.product_id
                                            left outer join ProductDisplayGroupFormat pdgf on pdgf.Id = pd.DisplayGroupId
                                            where (pdgf.site_id = @siteid  or @siteid = -1) 
                                             and not exists (select 1 
                                                               from UserRoleDisplayGroupExclusions urdge , 
                                                                    users u
                                                              where urdge.ProductDisplayGroupId = pdgf.Id
                                                                and urdge.role_id = u.role_id
                                                                and u.loginId = @loginId )
                                            and pdgf.Id not in  (SELECT distinct ProductDisplayGroupFormatId 
							                                            from POSProductExclusions 
							                                                where POSMachineId in 
								                                            (select POSMachineId from POSMachines
								                                                where Computer_Name=@posMachine )) ";
                QueryJoiner = " and ";
            }


            DataTable dt = dataAccessHandler.executeSelectQuery(@"Select  product_id,product_name,p.ImageFileName,p.description, p.price , 
                                                                isnull(p.AdvanceAmount,0) AdvanceAmount,
                                                                isnull(p.MinimumQuantity,0) MinimumQuantity,
                                                                isnull(p.MinimumTime,0) MinimumTime,
                                                                isnull(p.MaximumTime,0) MaximumTime,
                                                                isnull(p.AvailableUnits,0) AvailableUnits,
                                                                isnull(p.AdvancePercentage,0) AdvancePercentage,
                                                                isnull(p.WebDescription,'') WebDescription,
                                                                isnull(p.sort_order,0) sort_order 
                                                         from products p " + BookingFilterQuery +
                                                        QueryJoiner +
                                                      @"p.product_type_id = pt.product_type_id
                                                        and pt.product_type ='BOOKINGS' 
                                                        and p.active_flag ='Y'
                                                        AND GETDATE() BETWEEN ISNULL(p.StartDate,getdate()) and ISNULL(p.ExpiryDate,getdate()+1)
                                                        AND (p.site_id = @siteid or @siteid = -1) 
                                                        order by isnull(p.sort_order,0) ",
                                                   new SqlParameter[]{ new SqlParameter("@siteid", siteId),
                                                    new SqlParameter("@posMachine", posMachine),
                                                     new SqlParameter("@loginId", loginId) }, sqlTransaction);


            log.LogVariableState("@siteid", siteId);
            log.LogVariableState("@posMachine", posMachine);
            log.LogVariableState("@loginId", loginId);

            log.LogMethodExit(dt);
            return dt;
        }

        /// <summary>
        /// GetBookingProductContents method
        /// </summary>
        /// <param name="bookingProductId">bookingProductId</param>
        /// <returns>returns List of BookingProductContent</returns>
        public List<BookingProductContent> GetBookingProductContents(int bookingProductId, bool getActiveRecords)
        {
            log.LogMethodEntry(bookingProductId, getActiveRecords);
            DataTable dtbookingProductDet = new DataTable();
            int getActiveOnly = -1;
            if (getActiveRecords)
            {
                getActiveOnly = 1;
            }
            List<BookingProductContent> bookingProductContentList = new List<BookingProductContent>();

            dtbookingProductDet = dataAccessHandler.executeSelectQuery(@"SELECT cp.Product_Id ParentId,
                                                                              ChildProductId ChildId,
                                                                              p.price,
                                                                              PriceInclusive,
                                                                              p.product_name,
                                                                              p.ImageFileName productImage,
                                                                              ISNULL(Quantity, 0) Quantity,
                                                                              cp.CategoryId,
                                                                              pt.product_type ProductType  ,
                                                                              cp.id as ComboProductId,
                                                                              ISNULL(p.WebDescription,'') WebDescription,
                                                                              ISNULL(p.face_value,0) as Facevalue
                                                                              FROM ComboProduct cp INNER JOIN Products p ON p.product_id = cp.ChildProductId,
                                                                                  product_type pt
                                                                            WHERE isnull(AdditionalProduct,'N') != 'Y'
                                                                            AND pt.product_type_id = p.product_type_id
                                                                            AND cp.Product_Id = @bookingProductId 
                                                                            and (isnull(cp.IsActive,1) = @getActiveOnly OR @getActiveOnly = -1 )
                                                                            ORDER BY isnull(cp.SortOrder, 1000) ",
                                                                            new SqlParameter[] { new SqlParameter("@bookingProductId", bookingProductId), new SqlParameter("@getActiveOnly", getActiveOnly) }, sqlTransaction);


            foreach (DataRow productRow in dtbookingProductDet.Rows)
            {
                bookingProductContentList.Add(new BookingProductContent(
                                                        Convert.ToInt32(productRow["ChildId"]),
                                                        productRow["product_name"].ToString(),
                                                        (productRow["productImage"] == DBNull.Value ? "" : productRow["productImage"].ToString()),
                                                        "",
                                                        (productRow["ChildId"] == DBNull.Value ? 0 : Convert.ToInt32(productRow["ChildId"])),
                                                        (productRow["CategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(productRow["CategoryId"])),
                                                        (productRow["price"] == DBNull.Value ? 0 : Convert.ToDouble(productRow["price"])),
                                                        (productRow["producttype"] == DBNull.Value ? "" : productRow["producttype"].ToString()),
                                                        (productRow["quantity"] == DBNull.Value ? 0 : Convert.ToInt32(productRow["quantity"])),
                                                        (productRow["priceInclusive"] == DBNull.Value ? "" : productRow["priceInclusive"].ToString()),
                                                        (productRow["ComboProductId"] == DBNull.Value ? -1 : Convert.ToInt32(productRow["ComboProductId"])),
                                                         (productRow["Facevalue"] == DBNull.Value ? 0 : Convert.ToDouble(productRow["Facevalue"])),
                                                         (productRow["WebDescription"] == DBNull.Value ? "" : productRow["WebDescription"]).ToString()
                                                     ));
            }
            log.LogMethodExit(bookingProductContentList);
            return bookingProductContentList;
        }

        internal List<int> GetInactiveProductsToBePublished(int masterSiteId)
        {
            log.LogMethodEntry(masterSiteId);
            List<int> result = new List<int>();
            string query = @"SELECT product_id
                             FROM products p
                             WHERE site_id = @site_id 
                             AND active_flag = 'N'
                             AND EXISTS (SELECT 1 
			                             FROM 	products pp
			                             WHERE pp.MasterEntityId = p.product_id
			                             AND active_flag = 'Y')";
            SqlParameter parameter = new SqlParameter("@site_id", masterSiteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if(row["product_id"] != DBNull.Value)
                    {
                        result.Add(Convert.ToInt32(row["product_id"]));
                    }
                }
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetAdditionalProducts method
        /// </summary>
        /// <param name="bookingProductId">bookingProductId</param>
        /// <returns>returns List of AdditionalProduct</returns>
        public List<AdditionalProduct> GetAdditionalProducts(int bookingProductId, bool getActiveRecords)
        {
            log.LogMethodEntry(bookingProductId, getActiveRecords);
            DataTable dtAddtnlProduct = new DataTable();
            int getActiveOnly = -1;
            if (getActiveRecords)
            {
                getActiveOnly = 1;
            }
            List<AdditionalProduct> addtnlProductList = new List<AdditionalProduct>();
            //using (Utilities parafaitUtility = new Utilities(connstring))
            {
                dtAddtnlProduct = dataAccessHandler.executeSelectQuery(@"SELECT p.product_name,p.ImageFileName productImage,
                                                                         cp.Product_Id ParentId,
                                                                         cp.ChildProductId ChildId,
                                                                         isnull((select DisplayGroup from ProductDisplayGroupFormat where Id = cp.DisplayGroupId),'') display_group,
                                                                         cp.CategoryId,
                                                                         p.price, pt.product_type,
                                                                         isnull(p.description,'')  description ,
                                                                         isnull(p.WebDescription,'')  WebDescription ,
                                                                         isnull(p.MinimumQuantity,0) MinimumQuantity, 
                                                                         isnull(p.CardCount,0) MaximumQuantity,
                                                                         cp.id as ComboProductId,
                                                                         ISNULL(p.face_value,0) as Facevalue
                                                                       FROM ComboProduct cp
                                                                            INNER JOIN Products p
                                                                            ON p.product_id = cp.ChildProductId,
                                                                            product_type pt
                                                                       WHERE cp.AdditionalProduct = 'Y'
                                                                            AND cp.Product_Id = @bookingProductId
                                                                            AND p.active_flag = 'Y'
                                                                            and (isnull(cp.IsActive,1) = @getActiveOnly OR @getActiveOnly = -1 )
                                                                            AND pt.product_type_id = p.product_type_id 
                                                                       ORDER BY isnull(cp.SortOrder, 1000)",
                                                                       new SqlParameter[] { new SqlParameter("@bookingProductId", bookingProductId), new SqlParameter("@getActiveOnly", getActiveOnly) }, sqlTransaction);
            }


            foreach (DataRow productRow in dtAddtnlProduct.Rows)
            {
                addtnlProductList.Add(new AdditionalProduct(
                                                            Convert.ToInt32(productRow["ParentId"]),
                                                            productRow["product_name"].ToString(),
                                                            productRow["description"].ToString(),
                                                            (productRow["productImage"] == DBNull.Value ? "" : productRow["productImage"].ToString()),
                                                            (productRow["display_group"] == DBNull.Value ? "" : productRow["display_group"].ToString()),
                                                            (productRow["ChildId"] == DBNull.Value ? 0 : Convert.ToInt32(productRow["ChildId"])),
                                                            (productRow["CategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(productRow["CategoryId"])),
                                                            (productRow["price"] == DBNull.Value ? 0 : Convert.ToDouble(productRow["price"])),
                                                            (productRow["product_type"] == DBNull.Value ? "" : productRow["product_type"].ToString()),
                                                            (productRow["MinimumQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(productRow["MinimumQuantity"])),
                                                            (productRow["MaximumQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(productRow["MaximumQuantity"])),
                                                             (productRow["ComboProductId"] == DBNull.Value ? -1 : Convert.ToInt32(productRow["ComboProductId"])),
                                                             (productRow["Facevalue"] == DBNull.Value ? 0 : Convert.ToDouble(productRow["Facevalue"])),
                                                             (productRow["WebDescription"] == DBNull.Value ? "" : productRow["WebDescription"].ToString())
                                                        ));
            }
            log.LogMethodExit(addtnlProductList);
            return addtnlProductList;
        }

        /// <summary>
        /// GetPackageContents method
        /// </summary>
        /// <param name="productId">productId</param>
        /// <returns>returns List of PackageContents</returns>
        public List<BookingProductContent> GetPackageContents(int productId, bool getActiveRecords)
        {
            log.LogMethodEntry(productId, getActiveRecords);
            List<BookingProductContent> productContentList = new List<BookingProductContent>();
            int getActiveOnly = -1;
            if (getActiveRecords)
            {
                getActiveOnly = 1;
            }
            DataTable dtProducts = dataAccessHandler.executeSelectQuery(@"SELECT product_name,pt.product_type,
                                                                                    Name Category,
                                                                                    Quantity,
                                                                                    c.CategoryId, cp.id as ComboProductId,
                                                                                    ISNULL(p.face_value,0) as Facevalue
                                                                                FROM comboProduct cp
                                                                                     LEFT OUTER JOIN products p ON p.Product_Id = cp.ChildProductId
                                                                                     LEFT OUTER JOIN category c  ON c.categoryId = cp.categoryId, product_type pt
                                                                                WHERE cp.Product_Id = @parentProductId
                                                                                  and (isnull(cp.IsActive,1) = @getActiveOnly OR @getActiveOnly = -1 )                 
													                            and pt.product_type_id = p.product_type_id
                                                                                ORDER BY isnull(cp.SortOrder, 1000)"
                                                                      , new SqlParameter[] { new SqlParameter("@parentProductId", productId), new SqlParameter("@getActiveOnly", getActiveOnly) });

            foreach (DataRow productRow in dtProducts.Rows)
            {
                BookingProductContent bp = new BookingProductContent();
                bp.ProductName = productRow["product_name"].ToString();
                bp.ProductType = productRow["product_type"].ToString();
                bp.ComboProductId = Convert.ToInt32(productRow["ComboProductId"]);
                bp.FaceValue = Convert.ToDouble(productRow["Facevalue"]);
                productContentList.Add(bp);
            }
            log.LogMethodExit(productContentList);
            return productContentList;
        }
        /// <summary>
        /// Updates the duplicates of previous products of previous product Id
        /// </summary>
        /// <returns>updatedRows</returns>
        public int UpdateDuplicateProductDetails(int duplicateProductId, int newProductId)
        {
            log.LogMethodEntry();
            int updatedRows = 0;
            string query = @"update products set 
                                            --CustomDataSetId = p.CustomDataSetId, 
                                            ButtonColor = p.ButtonColor,
                                            TextColor = p.TextColor,
                                            Font = p.Font,
                                            CheckInFacilityId = p.CheckInFacilityId,
                                            MaxCheckOutAmount = p.MaxCheckOutAmount                      
                                        from (select * from products where product_Id = @duplicateProductId) p
                                            where products.product_id = @product_Id";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@duplicateProductId", duplicateProductId));
            parameters.Add(new SqlParameter("@product_Id", newProductId));
            try
            {
                updatedRows = dataAccessHandler.executeUpdateQuery(query, parameters.ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(updatedRows);
            return updatedRows;
        }

        /// <summary>
        /// Deletes the Product based on the productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int DeleteProducts(int productId)
        {
            log.LogMethodEntry(productId);
            try
            {
                string deleteQuery = @"delete from Products where product_id = @productId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@productId", productId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Below Method is Used for Viator.
        /// Get the List of Product based on productDisplayGroupFormatId, siteId
        /// </summary>
        /// This method is implemented for viator and this will fetch the appropriate products list based on Product display Groups
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductsDTO> GetProductsListFromDisplayGroup(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectGetProductQuery = @"select p.*,null as product_type_id,null as product_type,null as description,null as active_flag,null as last_updated_date,
                                                null as last_updated_user,null as InternetKey,null as Guid,null as site_id,null as SynchStatus,null as CardSale,
                                                null as ReportGroup,null as MasterEntityId,null as OrderTypeId,null as CreatedBy,null as CreationDate,null as maxQtyPerDay,
                                                null as hsnSacCode,null as webDescription,null as orderTypeId,null as isGroupMeal,null as membershipId,null as cardSale,
                                                null as zoneCode,null as lockerMode,null as taxName,null as usedInDiscounts,null as displayGroup, 
                                                ISNULL((Select TOP 1 ISNULL(IsSellable,'N') from product where product.ManualProductId = p.product_id AND ISNULL(product.IsActive,'Y') = ISNULL(p.active_flag,'Y')),'Y') as Sellable ,
                                                     p.ExternalSystemReference
                                                from Products as p ";
            StringBuilder joinQuery = new StringBuilder("inner join ProductsDisplayGroup as pdg on pdg.ProductId = p.product_id left outer join product_type pt on p.product_type_id = pt.product_type_id ");
            StringBuilder query = new StringBuilder(" where ");

            foreach (KeyValuePair<ProductsDTO.SearchByProductParameters, string> searchParameter in searchParameters)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    string joinOperator = (count == 0 ? "" : " and ");

                    if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_DISPLAY_GROUP_FORMAT_ID))
                    {
                        query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.SITEID))
                    {
                        query.Append(joinOperator + " (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.ISACTIVE))
                    {
                        query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "'Y'" : "'N'"));
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST))
                    {
                        query.Append(joinOperator + "( " + DBSearchParameters[searchParameter.Key] + ") IN (" + searchParameter.Value + " )");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE_LIST))
                    {
                        query.Append(joinOperator + "( " + DBSearchParameters[searchParameter.Key] + ") IN ('" + searchParameter.Value + "')");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.LAST_UPDATED_FROM_DATE))
                    {
                        query.Append(joinOperator + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= '" + searchParameter.Value + "'");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.LAST_UPDATED_TO_DATE))
                    {
                        query.Append(joinOperator + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < '" + searchParameter.Value + "'");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCTS_ENTITY_LAST_UPDATED_FROM_DATE))
                    {
                        query.Append(joinOperator + @"( EXISTS( SELECT 1 FROM ComboProduct comboP  
																	WHERE comboP.Product_Id = p.product_id
																	and comboP.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL 
																SELECT 1 FROM FacilityMap vf, ProductsAllowedInFacility paif
																	WHERE paif.ProductsId=p.product_id
																	and paif.FacilityMapId=vf.FacilityMapId
																	and vf.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL 
																SELECT 1 FROM FacilityMapDetails vfd, ProductsAllowedInFacility paif
																	WHERE paif.ProductsId=p.product_id
																	and paif.FacilityMapId=vfd.FacilityMapId
																	and vfd.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL
																SELECT 1 FROM CheckInFacility cif, FacilityMapDetails fmd, ProductsAllowedInFacility paif
																	WHERE paif.ProductsId = p.product_id
																	AND fmd.FacilityMapId = paif.FacilityMapId
																	AND cif.FacilityId = fmd.FacilityId
																	AND cif.last_updated_date > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
															UNION ALL
																SELECT 1 FROM CustomData cd
																	WHERE cd.CustomDataSetId=p.CustomDataSetId
																	AND cd.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
                                                            ) 
															OR ( " + DBSearchParameters[searchParameter.Key] + " > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "' ))");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.CUSTOM_DATA_SET_IS_SET) ||
                             searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.EXTERNAL_SYSTEM_REFERENCE_IS_SET))
                    {
                        query.Append(joinOperator + "ISNULL( CASE WHEN " + DBSearchParameters[searchParameter.Key] + " IS NOT NULL THEN '1' ELSE '0' END, '0')= '" + searchParameter.Value + "'");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.IS_SUBSCRIPTION_PRODUCT))
                    {
                        query.Append(joinOperator + " CASE WHEN (SELECT top 1 1 FROM productSubscription ps where ps.productsId = " + DBSearchParameters[searchParameter.Key] + " ) = 1 THEN 1 ELSE 0 END = " + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0") + " ");
                    }
                    else if (searchParameter.Key.Equals(ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST))
                    {
                        query.Append(joinOperator + @"(" + DBSearchParameters[searchParameter.Key] + " IN  " + "("+ searchParameter.Value  +"))");
                    }
                    else if (searchParameter.Key == ProductsDTO.SearchByProductParameters.IS_A_SUBSCRIPTION_PRODUCT_OR_HAS_ACTIVE_SUBSCRIPTION_CHILD)
                    {
                        query.Append(joinOperator + @" ISNULL(
                                                            (SELECT TOP 1 1 from (
                                                            SELECT 1 as aCol --self
                                                               FROM ProductSubscription ps 
                                                              WHERE ps.productsId  = " + DBSearchParameters[searchParameter.Key] + @"  
                                                              UNION ALL
                                                             SELECT 1 as aCol --for child products
                                                               FROM ProductSubscription ps, 
                                                                    ComboProduct combop
                                                              WHERE combop.Product_Id = " + DBSearchParameters[searchParameter.Key] + @" 
                                                                AND ps.ProductsId = combop.ChildProductId 
                                                                AND ISNULL(combop.IsActive,1) = 1
                                                               UNION ALL
                                                              SELECT 1 as aCol-- for catagory products
                                                                FROM ProductSubscription ps,  
                                                                     ComboProduct combop, products catgp
                                                               WHERE combop.Product_Id = " + DBSearchParameters[searchParameter.Key] + @" 
                                                                 AND catgp.CategoryId = comboP.CategoryId
                                                                 AND ps.ProductsId = catgp.product_id
                                                                 AND ISNULL(combop.IsActive,1) = 1
                                                                ) as aaa),0) =  " + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0"));
                    }
                    else
                    {
                        query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + " N'%" + searchParameter.Value + "%'");
                    }
                    count++;
                }
                else
                {
                    log.LogMethodExit(null, "throwing exception");
                    log.LogVariableState("searchParameter.Key", searchParameter.Key);
                    throw new Exception("The query parameter does not exist " + searchParameter.Key);
                }
            }
            if (searchParameters.Count > 0)
                selectGetProductQuery = selectGetProductQuery + joinQuery + query;
            DataTable ProductData = dataAccessHandler.executeSelectQuery(selectGetProductQuery, null);
            List<ProductsDTO> productList = new List<ProductsDTO>();
            if (ProductData.Rows.Count > 0)
            {
                foreach (DataRow productDataRow in ProductData.Rows)
                {
                    ProductsDTO productsDataObject = GetProductDTO(productDataRow);
                    productList.Add(productsDataObject);
                }
            }
            log.LogMethodExit(productList);
            return productList;
        }


        /// <summary>
        /// Returns the no of RedemptionCurrencies matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetProductsCount(List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int productDTOCount = 0;
            string selectQuery = BuildProductQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                productDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(productDTOCount);
            return productDTOCount;
        }


        internal DateTime? GetProductsLastUpdateTime(int siteId)
        {
            log.LogMethodEntry();
            string query = @"select max(a.LastUpdatedDate) LastUpdatedDate
                            from (
                          SELECT (select max(LastUpdatedDate) LastUpdatedDate
                            from (VALUES (p.last_updated_date), (comboProduct.LastUpdatedDate),
                             (ProductCreditPlus.LastUpdatedDate),
                             (ProductModifiers.LastUpdatedDate),
                             (ProductGames.LastUpdatedDate),
                             (Product.LastUpdatedDate),
                             (ProductsDisplayGroup.LastUpdatedDate)
                             ,(ProductsCustom.LastUpdatedDate)
                             ,(ProductCustom.LastUpdatedDate)
                       )as value(LastUpdatedDate)) LastUpdatedDate
                    FROM products p
       left outer join (select product_id, LastupdateDate LastUpdatedDate from ComboProduct WHERE (site_id = @siteId or @siteId = -1)) comboProduct
                on p.product_id = comboProduct.Product_Id
       left outer join (select product_id, LastupdateDate LastUpdatedDate from ProductCreditPlus WHERE (site_id = @siteId or @siteId = -1)) ProductCreditPlus
                on p.product_id = ProductCreditPlus.Product_Id
       left outer join (select productid, LastupdateDate LastUpdatedDate from ProductModifiers WHERE (site_id = @siteId or @siteId = -1)) ProductModifiers
                on p.product_id = ProductModifiers.ProductId
       left outer join (select product_id, LastupdateDate LastUpdatedDate from ProductGames WHERE (site_id = @siteId or @siteId = -1)) ProductGames
                on p.product_id = ProductGames.Product_Id
       left outer join (select ManualProductId, LastModDttm LastUpdatedDate from Product WHERE (site_id = @siteId or @siteId = -1)) Product
                on p.product_id = Product.ManualProductId
       left outer join (select productid, LastUpdatedDate LastUpdatedDate from ProductsDisplayGroup WHERE (site_id = @siteId or @siteId = -1)) ProductsDisplayGroup
                on p.product_id = ProductsDisplayGroup.ProductId
          LEFT Outer Join (select product_id, ISNULL(CustomData.LastUpdateDate, cast('1753-1-1' as datetime)) LastUpdatedDate
                             from  CustomData, Products
                              where Products.CustomDataSetId = CustomData.CustomDataSetId
                               and (Products.site_id = @siteId or @siteId = -1)
                                       ) ProductsCustom
                     on p.product_id = ProductsCustom.product_id
          LEFT Outer Join (select Manualproductid, ISNULL(CustomData.LastUpdateDate, cast('1753-1-1' as datetime)) LastUpdatedDate
                              from  CustomData, Product
                             where Product.CustomDataSetId = CustomData.CustomDataSetId
                            and (Product.site_id = @siteId or @siteId = -1)) ProductCustom
                     on p.product_id = ProductCustom.Manualproductid
where (site_id = @siteId or @siteId = -1)
   ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
