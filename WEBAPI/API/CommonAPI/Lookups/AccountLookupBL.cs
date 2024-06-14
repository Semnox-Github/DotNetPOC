///********************************************************************************************
// * Project Name - CardLookupBL
// * Description  - Created to fetch lookup values in Cards Module.
// *  
// **************
// **Version Log
// **************
// *Version     Date          Modified By    Remarks          
// *********************************************************************************************
// *2.60        19-Feb-2019   Mushahid Faizan    Created.
// ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Game;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.PriceList;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Semnox.CommonAPI.Lookups
{
    public class AccountLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        private Dictionary<string, string> keyValuePairs;
        string dependentDropdownName = string.Empty;
        string dependentDropdownSelectedId = string.Empty;
        string isActive = string.Empty;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private CommonLookupsDTO lookupDTO = new CommonLookupsDTO();
        /// <summary>
        /// Constructor for the method AccountLookupBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public AccountLookupBL(string entityName, ExecutionContext executioncontext, string dependentDropdownName, string dependentDropdownSelectedId, string isActive)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            this.dependentDropdownName = dependentDropdownName;
            this.dependentDropdownSelectedId = dependentDropdownSelectedId;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor for the method AccountLookupBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public AccountLookupBL(string entityName, ExecutionContext executioncontext, Dictionary<string, string> keyValuePairs)
        {
            log.LogMethodEntry(entityName, executioncontext, keyValuePairs);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            this.keyValuePairs = keyValuePairs;
            log.LogMethodExit();
        }
        public enum AccountsEntityNameLookup
        {
            CARDSCREDITPLUS,
            VIEWCARDS,
            CARDGAMES,
            CARDDISCOUNTS,
            MEMBERSHIP,
            MEMBERSHIPRULES,
            MEMBERSHIPEXCLUSIONRULES,
            CUSTOMERDETAILS
        }
        /// <summary>
        /// Gets the All lookups for all dropdowns based on the page in the Cards module.
        /// </summary>
        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> completeTablesDataList = new List<CommonLookupsDTO>();
                string[] dropdowns = null;
                string dropdownNames = string.Empty;

                AccountsEntityNameLookup accountsEntityNameLookupValue = (AccountsEntityNameLookup)Enum.Parse(typeof(AccountsEntityNameLookup), entityName);
                switch (accountsEntityNameLookupValue)
                {
                    case AccountsEntityNameLookup.CARDSCREDITPLUS:
                        dropdownNames = "CREDITPLUSTYPE,COUNTER,CATEGORY,PRODUCT,ORDERTYPE,GAMEPROFILE,GAME";
                        break;
                    case AccountsEntityNameLookup.VIEWCARDS:
                        dropdownNames = "CARDTYPE,MEMBERSHIP,ADVANCEDSEARCH,OPERATOR";
                        break;
                    case AccountsEntityNameLookup.CARDGAMES:
                        dropdownNames = "GAMEPROFILE,GAME,FREQUENCY,CARD_GAMES_ENTITLEMENT_TYPES";
                        break;
                    case AccountsEntityNameLookup.CARDDISCOUNTS:
                        dropdownNames = "DISCOUNTS";
                        break;
                    case AccountsEntityNameLookup.MEMBERSHIP:
                        dropdownNames = "MEMBERSHIPRULE,PRICELIST,REWARDPRODUCT,REWARDATTRIBUTE,MEMBERSHIP_REWARD_FUNCTION,REWARDFREQUENCY";
                        break;
                    case AccountsEntityNameLookup.MEMBERSHIPRULES:
                        dropdownNames = "REWARDFREQUENCY";
                        break;
                    case AccountsEntityNameLookup.MEMBERSHIPEXCLUSIONRULES:
                        dropdownNames = "GAME,GAMEPROFILE,PRODUCT";
                        break;
                    case AccountsEntityNameLookup.CUSTOMERDETAILS:
                        dropdownNames = "TITLE,CONTACTTYPE,ADDRESSTYPE";
                        break;
                }

                dropdowns = dropdownNames.Split(',');
                AccountDataHandler accountDataHandler = new AccountDataHandler(null);
                foreach (string dropdownName in dropdowns)
                {
                    lookupDTO = new CommonLookupsDTO
                    {
                        Items = new List<CommonLookupDTO>(),
                        DropdownName = dropdownName
                    };

                    if (dropdownName.ToUpper().ToString() == "CREDITPLUSTYPE")
                    {
                        List<KeyValuePair<CreditPlusType, string>> creditPlusTypeList = new List<KeyValuePair<CreditPlusType, string>>();
                        creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.CARD_BALANCE, "Card Balance"));
                        creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.COUNTER_ITEM, "Counter Items Only"));
                        creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.GAME_PLAY_BONUS, "Game Play Bonus"));
                        creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.GAME_PLAY_CREDIT, "Game Play Credits"));
                        creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.LOYALTY_POINT, "Loyalty Points"));
                        creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.TICKET, "Tickets"));
                        creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.TIME, "Time"));
                        creditPlusTypeList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.VIRTUAL_POINT, "Virtual Points"));
                        foreach (KeyValuePair<CreditPlusType, string> creditsType in creditPlusTypeList)
                        {
                            CommonLookupDTO lookupDataObject;
                            int key = (int)creditsType.Key;
                            lookupDataObject = new CommonLookupDTO(key.ToString(), creditsType.Value);
                            lookupDTO.Items.Add(lookupDataObject);
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "COUNTER")
                    {
                        loadDefaultValue("<SELECT>");
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
                    else if (dropdownName.ToUpper().ToString() == "CATEGORY")
                    {
                        loadDefaultValue("-All-");
                        CategoryList categoryList = new CategoryList(executionContext);
                        List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        CategoryDataHandler categoryDataHandler = new CategoryDataHandler();
                        List<CategoryDTO> categoryDTOList = categoryDataHandler.GetCategoryList(searchParameters);

                        if (categoryDTOList != null && categoryDTOList.Any())
                        {
                            foreach (CategoryDTO categoryDTO in categoryDTOList)
                            {

                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(categoryDTO.CategoryId), categoryDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }

                    }
                    else if (dropdownName.ToUpper().ToString() == "PRODUCT")
                    {
                        loadDefaultValue("-All-");
                        Semnox.Parafait.Product.Products products = new Semnox.Parafait.Product.Products();
                        ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                        productsFilterParams.SiteId = executionContext.GetSiteId();
                        List<ProductsDTO> productsDTOs = products.GetProductDTOList(productsFilterParams);
                        if (productsDTOs != null && productsDTOs.Any())
                        {
                            foreach (ProductsDTO product in productsDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(product.ProductId), product.ProductName.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ORDERTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        OrderTypeListBL orderTypeListBL = new OrderTypeListBL(executionContext);
                        List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<OrderTypeDTO> orderTypeDTOList = orderTypeListBL.GetOrderTypeDTOList(searchParameters);
                        if (orderTypeDTOList != null && orderTypeDTOList.Any())
                        {
                            foreach (OrderTypeDTO orderTypeDTO in orderTypeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(orderTypeDTO.Id), orderTypeDTO.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "GAMEPROFILE")
                    {
                        loadDefaultValue("-All-");

                        GameProfileList gameProfileList = new GameProfileList(executionContext);
                        List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParam = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
                        searchParam.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<GameProfileDTO> gameProfileDTOs = gameProfileList.GetGameProfileDTOList(searchParam, false);
                        if (gameProfileDTOs != null && gameProfileDTOs.Any())
                        {
                            foreach (GameProfileDTO gameProfileDTO in gameProfileDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(gameProfileDTO.GameProfileId), gameProfileDTO.ProfileName);
                                lookupDTO.Items.Add(lookupDataObject);                                
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "GAME")
                    {
                        loadDefaultValue("-All-");
                        List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
                        searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        GameList gamesList = new GameList(executionContext);
                        List<GameDTO> gameDTOList = gamesList.GetGameList(searchParameters, false);
                        if (gameDTOList != null && gameDTOList.Any())
                        {
                            foreach (GameDTO gameDTO in gameDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(gameDTO.GameId), gameDTO.GameName);
                                lookupDTO.Items.Add(lookupDataObject);                                
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CARDTYPE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "N", "None" },
                            { "Y", "Staff Card" },
                            { "D","Enable / Disable Gameplay"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> cardType in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(cardType.Key, cardType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FREQUENCY")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "N", "None" },
                            { "D", "Daily" },
                            { "W", "Weekly" },
                            { "M", "Monthly" },
                            { "Y", "Yearly" },
                            { "B", "Birthday" },
                            { "A", "Anniversary" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> frequency in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(frequency.Key, frequency.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CARD_GAMES_ENTITLEMENT_TYPES" || dropdownName.ToUpper().ToString() == "MEMBERSHIP_REWARD_FUNCTION")
                    {
                        if (dropdownName.ToUpper().ToString() == "MEMBERSHIP_REWARD_FUNCTION")
                        {
                            loadDefaultValue("<SELECT>");
                        }
                        if (dropdownName.ToUpper().ToString() == "CARD_GAMES_ENTITLEMENT_TYPES")
                        {
                            loadDefaultValue("Default");
                        }

                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, dropdownName));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValue), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DISCOUNTS")
                    {
                        List<DiscountsDTO> discountsList = null;
                        using(UnitOfWork unitOfWork = new UnitOfWork())
                        {
                            SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                            searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                            DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, unitOfWork);
                            discountsList = discountsListBL.GetDiscountsDTOList(searchParameters);
                            if (discountsList != null && discountsList.Any())
                            {
                                foreach (DiscountsDTO discountDTO in discountsList)
                                {
                                    CommonLookupDTO lookupDataObject;
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(discountDTO.DiscountId), discountDTO.DiscountName);
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                        
                    }
                    else if (dropdownName.ToUpper().ToString() == "MEMBERSHIP")
                    {
                        loadDefaultValue("<SELECT>");
                        MembershipsList membershipsListBL = new MembershipsList(executionContext);
                        List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<MembershipDTO> membershipList = membershipsListBL.GetAllMembership(searchParam, executionContext.GetSiteId());
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
                    else if (dropdownName.ToUpper().ToString() == "MEMBERSHIPRULE")
                    {
                        loadDefaultValue("<SELECT>");
                        MembershipRulesList membershipsListBL = new MembershipRulesList(executionContext);
                        List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParam.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.ISACTIVE, "1"));
                        List<MembershipRuleDTO> membershipRuleList = membershipsListBL.GetAllMembershipRule(searchParam);
                        if (membershipRuleList != null && membershipRuleList.Any())
                        {
                            foreach (MembershipRuleDTO membershipRuleDTO in membershipRuleList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(membershipRuleDTO.MembershipRuleID), membershipRuleDTO.RuleName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRICELIST")
                    {
                        loadDefaultValue("<SELECT>");
                        PriceListList priceListList = new PriceListList(executionContext);
                        List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> searchParam = new List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>>();
                        searchParam.Add(new KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>(PriceListDTO.SearchByPriceListParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<PriceListDTO> priceListObj = priceListList.GetAllPriceList(searchParam);
                        if (priceListObj != null && priceListObj.Any())
                        {
                            foreach (PriceListDTO priceListDTO in priceListObj)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(priceListDTO.PriceListId), priceListDTO.PriceListName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "REWARDPRODUCT")
                    {
                        loadDefaultValue("<SELECT>");
                        Semnox.Parafait.Product.Products products = new Semnox.Parafait.Product.Products();
                        ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                        productsFilterParams.SiteId = executionContext.GetSiteId();
                        List<ProductsDTO> productsDTOs = products.GetRewardProductDTOList(productsFilterParams);
                        if (productsDTOs != null && productsDTOs.Any())
                        {
                            foreach (ProductsDTO productsDTO in productsDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "REWARDATTRIBUTE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "-1", "<SELECT>" },
                            { "L", "Loyalty Points" },
                            { "T", "Tickets" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var attribute in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(attribute.Key, attribute.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "REWARDFREQUENCY")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "-1", "<SELECT>" },
                            { "D", "Days" },
                            { "M", "Months" },
                            { "Y", "Years" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> frequency in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(frequency.Key, frequency.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TITLE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "-1", "SELECT" },
                            { "Mr.", "Mr" },
                            { "Mrs.", "Mrs" },
                            { "Ms.", "Ms" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (KeyValuePair<string, string> title in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(title.Key, title.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CONTACTTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        ContactTypeListBL contactListBL = new ContactTypeListBL(executionContext);
                        List<KeyValuePair<ContactTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ContactTypeDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<ContactTypeDTO.SearchByParameters, string>(ContactTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ContactTypeDTO> contactList = contactListBL.GetContactTypeDTOList(searchParam);
                        if (contactList != null && contactList.Any())
                        {
                            foreach (ContactTypeDTO contactTypeDTO in contactList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(contactTypeDTO.Id), contactTypeDTO.ContactType.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ADDRESSTYPE")
                    {
                        loadDefaultValue("<SELECT>");
                        AddressTypeListBL addressListBL = new AddressTypeListBL(executionContext);
                        List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<AddressTypeDTO.SearchByParameters, string>(AddressTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AddressTypeDTO> addressList = addressListBL.GetAddressTypeDTOList(searchParam);
                        if (addressList != null && addressList.Any())
                        {
                            foreach (AddressTypeDTO addressTypeDTO in addressList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(addressTypeDTO.Id), addressTypeDTO.AddressType.ToString());
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
                    completeTablesDataList.Add(lookupDTO);
                }
                log.LogMethodExit(completeTablesDataList);
                return completeTablesDataList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Loads default value to the lookupDto
        /// </summary>
        /// <param name="defaultValue">default value of Type string</param>
        /// Added on 16-Apr-2019
        private void loadDefaultValue(string defaultValue)
        {
            List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
            selectKey.Add(new KeyValuePair<string, string>("-1", defaultValue));
            foreach (KeyValuePair<string, string> select in selectKey)
            {
                CommonLookupDTO lookupDataObject;
                lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                lookupDTO.Items.Add(lookupDataObject);
            }
        }
    }
}
