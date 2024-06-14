/********************************************************************************************
 * Project Name - Promotions
 * Description  - Bussiness logic of lookups for Promotions module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70.0      29-Aug-2019   Mushahid Faizan      Created
 *2.80.0      02-Dec-2019   Indrajeet Kumar      Modified -Added Lookup ADVANCEDSEARCH & OPERATOR, TYPE 
 *2.90.0      30-Jul-2020   Mushahid Faizan      Modified - WMS issue fixes in CAMPAIGNCUSTOMERADVANCEDSEARCH.
 *2.110.0     11-Jan-2020   Jinto Thomas         Modified- WhatsApp integration Changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
//using Semnox.Parafait.Achievements;
using Semnox.Parafait.Category;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Game;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Product;
using Semnox.Parafait.Promotions;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Achievements;
using System.Data;
using Semnox.Parafait.Languages;

namespace Semnox.CommonAPI.Lookups
{
    public class PromotionsLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        string dependentDropdownName = string.Empty;
        string dependentDropdownSelectedId = string.Empty;
        string isActive = string.Empty;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private List<LookupValuesDTO> lookupValuesDTOList;
        private CommonLookupDTO lookupDataObject;
        private CommonLookupsDTO lookupDTO;

        /// <summary>
        /// Constructor for the method PromotionsLookupBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public PromotionsLookupBL(string entityName, ExecutionContext executioncontext)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }

        public enum PromotionsEntityLookup
        {

            CUSTOMER,
            LAUNCHPROMOTIONS,
            LOYALTYMANAGEMENT,
            CAMPAIGNS,
            MESSAGEMANAGEMENT
        }

        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                string dropdownNames = string.Empty;
                string[] dropdowns = null;
                PromotionsEntityLookup promotionsEntityLookup = (PromotionsEntityLookup)Enum.Parse(typeof(PromotionsEntityLookup), entityName.ToUpper().ToString());
                switch (promotionsEntityLookup)
                {
                    case PromotionsEntityLookup.CUSTOMER:
                        dropdownNames = "TITLE,MEMBERSHIP,SITE,ADDRESSTYPE,CONTACTTYPE,RELATIONSHIPTYPE,CUSTOMERS,CUSTOMERADVANCEDSEARCH,OPERATOR,GENDER,CUSTOMERTYPE";
                        break;
                    case PromotionsEntityLookup.LAUNCHPROMOTIONS:
                        dropdownNames = "MEMBERSHIP,CAMPAIGN,CATEGORY,PRODUCT,VISUALIZATIONTHEME,DAY,GAME,GAMEPROFILE,DISPLAYTHEME,RECURFREQUENCY,RECURTYPE,TIMELOOKUP,PROMOTIONTYPE";
                        break;
                    case PromotionsEntityLookup.LOYALTYMANAGEMENT:
                        dropdownNames = "ATTRIBUTE,GAME,GAMEPROFILE,MEMBERSHIP,ACTIVEMEMBERSHIP,TIME,PRODUCT,RECHARGEPRODUCT,COUNTERPRODUCT,REWARDAMOUNT,COUNTER,CATEGORY,TYPE";
                        break;
                    case PromotionsEntityLookup.CAMPAIGNS:
                        dropdownNames = "MEMBERSHIP,TOTALSPEND,COMMUNICATIONMODE,ADVANCEDSEARCH,OPERATOR,CAMPAIGNCUSTOMERADVANCEDSEARCH";
                        break;
                    case PromotionsEntityLookup.MESSAGEMANAGEMENT:
                        dropdownNames = "TRIGGERTYPE,RECEIPTTEMPLATE,PRODUCT,MESSAGETYPE,EVENT";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                foreach (string dropdownName in dropdowns)
                {
                    lookupDTO = new CommonLookupsDTO
                    {
                        Items = new List<CommonLookupDTO>(),
                        DropdownName = dropdownName
                    };

                    if (dropdownName.ToUpper().ToString() == "MEMBERSHIP")
                    {
                        LoadDefaultValue("<SELECT>");
                        MembershipsList membershipsListBL = new MembershipsList(executionContext);
                        List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, "1"));
                        List<MembershipDTO> membershipList = membershipsListBL.GetAllMembership(searchParam, executionContext.GetSiteId(), true);
                        if (membershipList != null && membershipList.Any())
                        {
                            foreach (MembershipDTO membershipDTO in membershipList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(membershipDTO.MembershipID), membershipDTO.MembershipName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    if (dropdownName.ToUpper().ToString() == "ACTIVEMEMBERSHIP")
                    {
                        LoadDefaultValue("<SELECT>");
                        MembershipsList membershipsListBL = new MembershipsList(executionContext);
                        List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, "1"));
                        List<MembershipDTO> membershipList = membershipsListBL.GetAllMembership(searchParam, executionContext.GetSiteId(), true);
                        if (membershipList != null && membershipList.Any())
                        {
                            foreach (MembershipDTO membershipDTO in membershipList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(membershipDTO.MembershipID), membershipDTO.MembershipName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CUSTOMERTYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject = new CommonLookupDTO("R", "Registered");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("U", "Unregistered");
                        lookupDTO.Items.Add(lookupDataObject);

                    }
                    else if (dropdownName.ToUpper().ToString() == "GENDER")
                    {
                        LoadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject = new CommonLookupDTO("M", "Male");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("F", "Female");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("N", "Not Set");
                        lookupDTO.Items.Add(lookupDataObject);

                    }
                    else if (dropdownName.ToUpper().ToString() == "TITLE")
                    {
                        LoadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject = new CommonLookupDTO("Mr.", "Mr.");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("Mrs.", "Mrs.");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("Ms.", "Ms.");
                        lookupDTO.Items.Add(lookupDataObject);

                    }
                    else if (dropdownName.ToUpper().ToString() == "SITE")
                    {
                        LoadDefaultValue("<SELECT>");
                        SiteList siteList = new SiteList(executionContext);
                        List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParam = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                        searchParam.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParam);
                        if (siteDTOList != null && siteDTOList.Any())
                        {
                            foreach (SiteDTO siteDTO in siteDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(siteDTO.SiteId), siteDTO.SiteName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ADDRESSTYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        AddressTypeListBL addressTypeListBL = new AddressTypeListBL(executionContext);
                        List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<AddressTypeDTO.SearchByParameters, string>(AddressTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AddressTypeDTO> addressTypeDTOList = addressTypeListBL.GetAddressTypeDTOList(searchParam);
                        if (addressTypeDTOList != null && addressTypeDTOList.Any())
                        {
                            foreach (AddressTypeDTO addressTypeDTO in addressTypeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(addressTypeDTO.Id), addressTypeDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CONTACTTYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        ContactTypeListBL contactTypeListBL = new ContactTypeListBL(executionContext);
                        List<KeyValuePair<ContactTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ContactTypeDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<ContactTypeDTO.SearchByParameters, string>(ContactTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ContactTypeDTO> contactTypeDTOList = contactTypeListBL.GetContactTypeDTOList(searchParam);
                        if (contactTypeDTOList != null && contactTypeDTOList.Any())
                        {
                            foreach (ContactTypeDTO contactTypeDTO in contactTypeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(contactTypeDTO.Id), contactTypeDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RELATIONSHIPTYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        CustomerRelationshipTypeListBL customerRelationshipListBL = new CustomerRelationshipTypeListBL(executionContext);
                        List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = customerRelationshipListBL.GetCustomerRelationshipTypeDTOList(searchParam);
                        if (customerRelationshipTypeDTOList != null && customerRelationshipTypeDTOList.Any())
                        {
                            foreach (CustomerRelationshipTypeDTO customerRelationshipTypeDTO in customerRelationshipTypeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(customerRelationshipTypeDTO.Id), customerRelationshipTypeDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CAMPAIGN")
                    {
                        LoadDefaultValue("<SELECT>");
                        CampaignBL.CampaignListBL campaignListBL = new CampaignBL.CampaignListBL(executionContext);
                        List<KeyValuePair<CampaignDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<CampaignDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<CampaignDTO.SearchByParameters, string>(CampaignDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<CampaignDTO> campaignDTOList = campaignListBL.GetCampaignDTOList(searchParam);
                        if (campaignDTOList != null && campaignDTOList.Any())
                        {
                            foreach (CampaignDTO campaignDTO in campaignDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(campaignDTO.CampaignId), campaignDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CATEGORY")
                    {
                        LoadDefaultValue("<All>");
                        CategoryList categoryList = new CategoryList(executionContext);
                        List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<CategoryDTO> categoryDTOList = categoryList.GetAllCategory(searchParameters);

                        if (categoryDTOList != null && categoryDTOList.Any())
                        {
                            categoryDTOList = categoryDTOList.OrderBy(x => x.Name).ToList();
                            foreach (CategoryDTO category in categoryDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(category.CategoryId), category.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRODUCT")
                    {
                        LoadDefaultValue("<All>");
                        ProductsList productsList = new ProductsList(executionContext);
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                        List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParameters);

                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            productsDTOList = productsDTOList.OrderBy(x => x.ProductName).ToList();
                            foreach (ProductsDTO productsDTO in productsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), (productsDTO.ActiveFlag == true ? productsDTO.ProductName.ToString() : productsDTO.ProductName + "(Inactive)"));
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RECHARGEPRODUCT")
                    {
                        LoadDefaultValue("<All>");
                        string productsType = string.Empty;
                        ProductTypeListBL productTypeList = new ProductTypeListBL(executionContext);
                        List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> typesearchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>();
                        typesearchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        typesearchParameters.Add(new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE, "ATTRACTION"));
                        List<ProductTypeDTO> productTypeObj = productTypeList.GetProductTypeDTOList(typesearchParameters);
                        if (productTypeObj != null && productTypeObj.Any())
                        {
                            if(productTypeObj[0].CardSale)
                            {
                                productsType += "'ATTRACTION'";
                            }
                        }
                        productsType += "'NEW','RECHARGE','VARIABLECARD','CARDSALE','COMBO','GAMETIME'";
                        ProductsList productsList = new ProductsList(executionContext);
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST, productsType));
                        List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParameters);

                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            productsDTOList = productsDTOList.OrderBy(x => x.ProductName).ToList();
                            foreach (ProductsDTO productsDTO in productsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName + (productsDTO.ActiveFlag == false ? "(Inactive)" : string.Empty));
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "COUNTERPRODUCT")
                    {
                        LoadDefaultValue("<All>");
                        string productsType = "'MANUAL', 'ATTRACTION', 'CHECK-IN', 'CHECK-OUT', 'COMBO','LOADTICKETS'";
                        ProductsList productsList = new ProductsList(executionContext);
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST, productsType));
                        List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParameters);

                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            productsDTOList = productsDTOList.OrderBy(x => x.ProductName).ToList();
                            foreach (ProductsDTO productsDTO in productsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "VISUALIZATIONTHEME")
                    {
                        LoadDefaultValue("<SELECT>");
                        ThemeListBL themeListBL = new ThemeListBL(executionContext);
                        List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ThemeDTO> themeDTOList = themeListBL.GetThemeDTOList(searchParameters, true, true);

                        if (themeDTOList != null && themeDTOList.Any())
                        {
                            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                            LoadLookupValues("THEME_TYPE");
                            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                            {
                                var visualizationThemeDTOList = (from thems in themeDTOList
                                                                 join lookupsData in lookupValuesDTOList on thems.TypeId equals lookupsData.LookupValueId
                                                                 where lookupsData.LookupName == "THEME_TYPE" && lookupsData.LookupValue == "Visualization"
                                                                 select thems).ToList();
                                if (visualizationThemeDTOList != null && visualizationThemeDTOList.Any())
                                {
                                    foreach (ThemeDTO themeDTO in visualizationThemeDTOList)
                                    {
                                        CommonLookupDTO lookupDataObject;
                                        //lookupDataObject = new CommonLookupDTO(Convert.ToString(themeDTO.Id), themeDTO.Name);
                                        lookupDataObject = new CommonLookupDTO(Convert.ToString(themeDTO.Id), themeDTO.Name + "[" + themeDTO.ThemeNumber + "]");
                                        lookupDTO.Items.Add(lookupDataObject);
                                    }
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "GAME")
                    {
                        LoadDefaultValue("<All>");
                        List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
                        searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                        GameList gamesList = new GameList(executionContext);
                        List<GameDTO> games = gamesList.GetGameList(searchParameters, false);
                        if (games != null && games.Any())
                        {
                            foreach (GameDTO gameDTO in games)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(gameDTO.GameId), gameDTO.GameName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "GAMEPROFILE")
                    {
                        LoadDefaultValue("<All>");
                        GameProfileList gameProfileList = new GameProfileList(executionContext);
                        List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParam = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
                        searchParam.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<GameProfileDTO> gameProfileDTOs = gameProfileList.GetGameProfileDTOList(searchParam, false);
                        if (gameProfileDTOs != null && gameProfileDTOs.Any())
                        {
                            foreach (GameProfileDTO gameProfile in gameProfileDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(gameProfile.GameProfileId), gameProfile.ProfileName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ATTRIBUTE")
                    {
                        LoyaltyAttributeListBL loyaltyAttributeListBL = new LoyaltyAttributeListBL(executionContext);
                        List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>(LoyaltyAttributesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        var loyaltyAttributesDTOList = loyaltyAttributeListBL.GetAllLoyaltyAttributesList(searchParameters);
                        if (loyaltyAttributesDTOList != null && loyaltyAttributesDTOList.Any())
                        {
                            foreach (LoyaltyAttributesDTO loyaltyAttributesDTO in loyaltyAttributesDTOList)
                            {
                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    { "PurchaseApplicable", Convert.ToString(loyaltyAttributesDTO.PurchaseApplicable) },
                                    { "ConsumptionApplicable", Convert.ToString(loyaltyAttributesDTO.ConsumptionApplicable) }
                                };
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(loyaltyAttributesDTO.LoyaltyAttributeId.ToString(), loyaltyAttributesDTO.Attribute, values);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DISPLAYTHEME")
                    {
                        LoadDefaultValue("<SELECT>");

                        ThemeListBL themeListBL = new ThemeListBL(executionContext);
                        List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ThemeDTO> themeDTOList = themeListBL.GetThemeDTOList(searchParameters, true, true);

                        if (themeDTOList != null && themeDTOList.Any())
                        {
                            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                            LoadLookupValues("THEME_TYPE");
                            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                            {
                                var displayThemeDTOList = (from thems in themeDTOList
                                                           join lookupsData in lookupValuesDTOList on thems.TypeId equals lookupsData.LookupValueId
                                                           where lookupsData.LookupName == "THEME_TYPE" && lookupsData.LookupValue == "Display"
                                                           select thems).ToList();
                                if (displayThemeDTOList != null && displayThemeDTOList.Any())
                                {
                                    foreach (ThemeDTO themeDTO in displayThemeDTOList)
                                    {
                                        CommonLookupDTO lookupDataObject;
                                        lookupDataObject = new CommonLookupDTO(Convert.ToString(themeDTO.Id), themeDTO.Name + "[" + themeDTO.ThemeNumber + "]");
                                        lookupDTO.Items.Add(lookupDataObject);
                                    }
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "REWARDAMOUNT")
                    {
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("LOYALTY_APPLICABLE_ELEMENTS");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.Description), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "COUNTER")
                    {
                        LoadDefaultValue("<All>");
                        POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
                        List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<POSTypeDTO> posTypeList = pOSTypeListBL.GetPOSTypeDTOList(searchParam);
                        if (posTypeList != null && posTypeList.Any())
                        {
                            foreach (POSTypeDTO pOSTypeDTO in posTypeList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(pOSTypeDTO.POSTypeId), pOSTypeDTO.POSTypeName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TIME")
                    {
                        LoadDefaultValue("<SELECT>");
                        List<KeyValuePair<decimal, string>> timeList = new List<KeyValuePair<decimal, string>>();
                        TimeSpan ts;
                        for (int i = 0; i <= 95; i++)
                        {
                            ts = new TimeSpan(0, i * 15, 0);
                            timeList.Add(new KeyValuePair<decimal, string>(Convert.ToDecimal(ts.Hours + ts.Minutes * 0.01), string.Format("{0:0}:{1:00} {2}", (ts.Hours % 12) == 0 ? (ts.Hours == 12 ? 12 : 0) : ts.Hours % 12, ts.Minutes, ts.Hours >= 12 ? "PM" : "AM")));
                        }
                        if (timeList.Count != 0)
                        {
                            foreach (var timeValue in timeList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(timeValue.Key), timeValue.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DAY")
                    {
                        LoadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject = new CommonLookupDTO("1", "Sunday");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("2", "Monday");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("3", "Tuesday");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("4", "Wednesday");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("5", "Thursday");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("6", "Friday");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("7", "Saturday");
                        lookupDTO.Items.Add(lookupDataObject);
                    }
                    else if (dropdownName.ToUpper().ToString() == "TOTALSPEND")
                    {
                        LoadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject = new CommonLookupDTO(">=", ">=");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("<=", "<=");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("=", "=");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("!=", "!=");
                        lookupDTO.Items.Add(lookupDataObject);
                    }
                    else if (dropdownName.ToUpper().ToString() == "COMMUNICATIONMODE")
                    {
                        LoadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject = new CommonLookupDTO("Email", "Email");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("SMS", "SMS");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("App", "AppNotification");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("Both", "All");
                        lookupDTO.Items.Add(lookupDataObject);
                    }
                    else if (dropdownName.ToUpper().ToString() == "TRIGGERTYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject = new CommonLookupDTO("P", "Purchase");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("R", "Redemption");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("V", "Card Validity");
                        lookupDTO.Items.Add(lookupDataObject);
                    }
                    else if (dropdownName.ToUpper().ToString() == "MESSAGETYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject = new CommonLookupDTO("E", "Email");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("S", "SMS");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("A", "AppNotification");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("W", "WhatsApp");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("B", "All");
                        lookupDTO.Items.Add(lookupDataObject);
                    }
                    else if (dropdownName.ToUpper().ToString() == "RECEIPTTEMPLATE")
                    {
                        LoadDefaultValue("<SELECT>");
                        ReceiptPrintTemplateHeaderListBL receiptPrintTemplateHeaderListBL = new ReceiptPrintTemplateHeaderListBL(executionContext);
                        List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>(ReceiptPrintTemplateHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOList = receiptPrintTemplateHeaderListBL.GetReceiptPrintTemplateHeaderDTOList(searchParam, false);
                        if (receiptPrintTemplateHeaderDTOList != null && receiptPrintTemplateHeaderDTOList.Any())
                        {
                            receiptPrintTemplateHeaderDTOList = receiptPrintTemplateHeaderDTOList.OrderBy(x => x.TemplateName).ToList();
                            foreach (ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO in receiptPrintTemplateHeaderDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(receiptPrintTemplateHeaderDTO.TemplateId), receiptPrintTemplateHeaderDTO.TemplateName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CUSTOMERS")
                    {
                        LoadDefaultValue("<SELECT>");
                        CustomerListBL customerListBL = new CustomerListBL(executionContext);
                        CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria();
                        searchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.CUSTOMER_SITE_ID, Operator.EQUAL_TO, executionContext.GetSiteId());
                        List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(searchCriteria, true);
                        if (customerDTOList != null && customerDTOList.Any())
                        {
                            foreach (CustomerDTO customerDTO in customerDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(customerDTO.Id), customerDTO.FirstName + ' ' + customerDTO.MiddleName + ' ' + customerDTO.LastName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ADVANCEDSEARCH")
                    {
                        AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria();
                        ColumnProvider columnProvider = accountSearchCriteria.GetColumnProvider();
                        Utilities utilities = new Utilities();
                        List<CommonLookupDTO> filterCommonLookupDTOList = new List<CommonLookupDTO>();
                        foreach (KeyValuePair<Enum, Column> columnItem in columnProvider.GetAllColumns())
                        {
                            CommonLookupDTO lookupDataObject;
                            if (columnItem.Value.Browsable)
                            {
                                Type myClassType = columnItem.Value.GetType();
                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    {"DataType", myClassType.Name.ToString() }
                                };
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(columnItem.Value.Name), Convert.ToString(MessageContainerList.GetMessage(executionContext, columnItem.Value.DisplayName)), values);
                                filterCommonLookupDTOList.Add(lookupDataObject);
                            }
                        }
                        if (filterCommonLookupDTOList != null)
                        {
                            filterCommonLookupDTOList = filterCommonLookupDTOList.OrderBy(m => m.Name).ToList();
                        }
                        lookupDTO.Items = filterCommonLookupDTOList;
                    }

                    else if (dropdownName.ToUpper().ToString() == "CUSTOMERADVANCEDSEARCH")
                    {
                        CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
                        ColumnProvider columnProvider = customerSearchCriteria.GetColumnProvider();
                        Utilities utilities = new Utilities();
                        List<CommonLookupDTO> filterCommonLookupDTOList = new List<CommonLookupDTO>();
                        foreach (KeyValuePair<Enum, Column> columnItem in columnProvider.GetAllColumns())
                        {
                            CommonLookupDTO lookupDataObject;
                            if (columnItem.Value.Browsable)
                            {
                                Type myClassType = columnItem.Value.GetType();
                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    {"DataType", myClassType.Name.ToString() }
                                };
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(columnItem.Value.Name), Convert.ToString(MessageContainerList.GetMessage(executionContext, columnItem.Value.DisplayName)), values);
                                //lookupDTO.Items.Add(lookupDataObject);
                                filterCommonLookupDTOList.Add(lookupDataObject);
                            }
                        }
                        if (filterCommonLookupDTOList != null)
                        {
                            filterCommonLookupDTOList = filterCommonLookupDTOList.OrderBy(m => m.Name).ToList();
                        }
                        lookupDTO.Items = filterCommonLookupDTOList;
                    }

                    else if (dropdownName.ToUpper().ToString() == "CAMPAIGNCUSTOMERADVANCEDSEARCH")
                    {
                        string query = "select c.name, c.name dispName, ty.name datatype, -1 CustomAttributeId " +
                                "from sys.columns c, sys.tables t, sys.types ty  " +
                                "where t.name = 'Customers' "+
                                "and t.object_id = c.object_id " +
                                "and ty.user_type_id = c.user_type_id " +
                                "and c.name not in ('MembershipId','CardTypeId','MasterEntityId', 'Guid', 'SynchStatus', 'CustomDataSetId') " +
                               @"union 
                                 select Name, Name, Type, CustomAttributeId
                                 from customAttributes
                                 where applicability = 'CUSTOMER'
                                 order by 1 ";

                        DataAccessHandler dataAccessHandler = new DataAccessHandler();
                        DataTable dTable = dataAccessHandler.executeSelectQuery(query,null);

                        System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                        System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

                        CommonLookupDTO lookupDataObject = null;
                        if (dTable.Rows.Count > 0)
                        {
                            foreach (DataRow dataRows in dTable.Rows)
                            {
                                string name = dataRows["dispName"].ToString();
                                if (name.Contains("_"))
                                {
                                    dataRows["dispName"] = textInfo.ToTitleCase(name.Replace('_', ' '));
                                }
                                else if (name.Length > 1)
                                {
                                    dataRows["dispName"] = char.ToUpper(name[0]) + name.Substring(1);
                                }

                                DataRow dataRow = dTable.Rows[0];
                                Dictionary<string, string> values = new Dictionary<string, string>
                                    {
                                        {"DataType",dataRows["dataType"].ToString() }
                                    };
                                lookupDataObject = new CommonLookupDTO(name, dataRows["dispName"].ToString(), values);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "OPERATOR")
                    {
                        foreach (Enum operatorEnum in Enum.GetValues(typeof(Operator)))
                        {
                            CommonLookupDTO lookupDataObject;
                            IOperator @operator = OperatorFactory.GetOperator((Operator)operatorEnum);
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(operatorEnum), Convert.ToString(MessageContainerList.GetMessage(executionContext, @operator.DisplayName)));
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RECURFREQUENCY")
                    {
                        List<KeyValuePair<string, string>> recurFrequecyList = new List<KeyValuePair<string, string>>();
                        recurFrequecyList.Add(new KeyValuePair<string, string>("D", "Daily"));
                        recurFrequecyList.Add(new KeyValuePair<string, string>("W", "Weekly"));
                        recurFrequecyList.Add(new KeyValuePair<string, string>("M", "Monthly"));
                        foreach (var recurFrequecy in recurFrequecyList)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(recurFrequecy.Key), recurFrequecy.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RECURTYPE")
                    {
                        List<KeyValuePair<string, string>> recurFrequecyList = new List<KeyValuePair<string, string>>();
                        recurFrequecyList.Add(new KeyValuePair<string, string>("D", "Date"));
                        recurFrequecyList.Add(new KeyValuePair<string, string>("W", "Weekday"));
                        foreach (var recurFrequecy in recurFrequecyList)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(recurFrequecy.Key), recurFrequecy.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TIMELOOKUP")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "<SELECT>");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);
                        string time;
                        int hour;
                        int mins;
                        string ampm;
                        for (int i = 0; i < 48; i++)
                        {
                            hour = i / 2;
                            mins = (i % 2) * 30;

                            if (hour >= 12)
                                ampm = "PM";
                            else
                                ampm = "AM";

                            if (hour == 0)
                                hour = 12;
                            if (hour > 12)
                                hour = hour - 12;

                            time = hour.ToString() + ":" + mins.ToString().PadLeft(2, '0') + " " + ampm;
                            lookupDataObject = new CommonLookupDTO(time, time);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "EVENT")
                    {
                        LoadDefaultValue("<SELECT>");
                        List<KeyValuePair<string, string>> eventList = new List<KeyValuePair<string, string>>();
                        eventList.Add(new KeyValuePair<string, string>(RedemptionDTO.RedemptionStatusEnum.OPEN.ToString(), "New Redemption Placed"));
                        eventList.Add(new KeyValuePair<string, string>(RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString(), "Ready for Delivery"));
                        eventList.Add(new KeyValuePair<string, string>(RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString(), "Delivered"));
                        foreach (var events in eventList)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(events.Key), events.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                        keyValuePairs.Add(new KeyValuePair<string, string>("P", "Card Purchase"));
                        keyValuePairs.Add(new KeyValuePair<string, string>("C", "Item Purchase"));
                        keyValuePairs.Add(new KeyValuePair<string, string>("G", "Game Play"));
                        foreach (var type in keyValuePairs)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(type.Key), type.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PROMOTIONTYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                        keyValuePairs.Add(new KeyValuePair<string, string>("P", "Product"));
                        keyValuePairs.Add(new KeyValuePair<string, string>("G", "Game Play"));
                        foreach (var type in keyValuePairs)
                        {
                            lookupDataObject = new CommonLookupDTO(Convert.ToString(type.Key), type.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }
                    lookups.Add(lookupDTO);
                }
                return lookups;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Loads default value to the lookupDto
        /// </summary>
        /// <param name="defaultValue">default value of Type string</param>
        private void LoadDefaultValue(string defaultValue)
        {
            List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
            selectKey.Add(new KeyValuePair<string, string>("-1", defaultValue));
            foreach (var select in selectKey)
            {
                CommonLookupDTO lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                lookupDTO.Items.Add(lookupDataObject);
            }
        }
        /// <summary>
        /// Loads the lookupValues
        /// </summary>
        /// <param name="lookupName"></param>
        private List<LookupValuesDTO> LoadLookupValues(string lookupName)
        {
            log.LogMethodEntry(lookupName);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
        }
    }
}
