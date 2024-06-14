/********************************************************************************************
* Project Name - AchievementsLookupBL
* Description  - Created a lookup values in Achievements Module.
**************
**Version Log
**************
*Version     Date          Modified By          Remarks          
*********************************************************************************************
*2.70        28-Aug-2019   Indrajeet Kumar      Created.  
*2.80        04-Jun-2020   Vikas Dwivedi        Resolved Cobra issues.
*2.140.1     08-Apr-2022   Abhishek             WMS Fix : To store bonus entitlement as value.
********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Linq;
using System.Text;
using Semnox.Parafait.Game;
using System.Threading.Tasks;
using Semnox.Parafait.Product;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Achievements
{
    public class AchievementsLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private readonly ExecutionContext executionContext;
        //private Dictionary<string, string> keyValuePairs;
        //private string keyValuePair;
        string dependentDropdownName = string.Empty;
        string dependentDropdownSelectedId = string.Empty;
        string isActive = string.Empty;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private CommonLookupDTO lookupDataObject;
        public List<LookupValuesDTO> lookupValuesDTOList;
        /// <summary>
        /// Constructor for the method MaintenanceLookUpBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public AchievementsLookupBL(string entityName, ExecutionContext executioncontext)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }

        public enum AchievementsEntityLookup
        {
            ACHIEVEMENTCLASS
        }

        public List<CommonLookupsDTO> GetLookUpMasterDataList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                string dropdownNames = "";
                string[] dropdowns = null;
                AchievementsEntityLookup achievementsEntityLookup = (AchievementsEntityLookup)Enum.Parse(typeof(AchievementsEntityLookup), entityName.ToUpper().ToString());
                switch (achievementsEntityLookup)
                {
                    case AchievementsEntityLookup.ACHIEVEMENTCLASS:
                        dropdownNames = "SELECTPROJECT,GAMENAME,PRODUCT,QUALIFYINGLEVEL,BONUSENTITLEMENT";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                foreach (string dropdownName in dropdowns)
                {
                    CommonLookupsDTO lookupDTO = new CommonLookupsDTO();
                    lookupDTO.Items = new List<CommonLookupDTO>();
                    lookupDTO.DropdownName = dropdownName;

                    if (dropdownName.ToUpper().ToString() == "SELECTPROJECT")
                    {
                        AchievementProjectsList achievementProjectsList = new AchievementProjectsList(executionContext);
                        List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<AchievementProjectDTO.SearchByParameters, string>(AchievementProjectDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<AchievementProjectDTO.SearchByParameters, string>(AchievementProjectDTO.SearchByParameters.IS_ACTIVE, "1"));
                        List<AchievementProjectDTO> achievementProjectDTOList = achievementProjectsList.GetAchievementProjectsList(searchParameters, false, true, null);

                        if (achievementProjectDTOList != null && achievementProjectDTOList.Any())
                        {
                            foreach (AchievementProjectDTO achievementProjectDTO in achievementProjectDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(achievementProjectDTO.AchievementProjectId), achievementProjectDTO.ProjectName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "GAMENAME")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);

                        List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
                        searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        GameList gamesList = new GameList(executionContext);
                        List<GameDTO> gameDTOList = gamesList.GetGameList(searchParameters, false);
                        if (gameDTOList != null && gameDTOList.Any())
                        {
                            foreach (GameDTO gameDTO in gameDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(gameDTO.GameId), gameDTO.GameName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRODUCT")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);

                        Products products = new Products(executionContext);
                        ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                        productsFilterParams.SiteId = executionContext.GetSiteId();
                        productsFilterParams.ProductType = "ACHIEVEMENTS";
                        List<ProductsDTO> productList = products.GetProductDTOList(productsFilterParams);
                        if (productList != null && productList.Any())
                        {
                            foreach (ProductsDTO productsDTO in productList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "QUALIFYINGLEVEL")
                    {
                        lookupDataObject = new CommonLookupDTO("-1", "--SELECT--");//to load Default Value (i.e., <SELECT>)
                        lookupDTO.Items.Add(lookupDataObject);

                        List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<AchievementClassLevelDTO.SearchByParameters, string>(AchievementClassLevelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        AchievementClassLevelsList achievementClassLevelsList = new AchievementClassLevelsList(executionContext);
                        List<AchievementClassLevelDTO> achievementClassLevelDTOList = achievementClassLevelsList.GetAchievementClassLevelList(searchParameters, false, true, null);

                        if (achievementClassLevelDTOList != null && achievementClassLevelDTOList.Any())
                        {
                            foreach (AchievementClassLevelDTO achievementClassLevelDTO in achievementClassLevelDTOList)
                            {
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(achievementClassLevelDTO.AchievementClassLevelId), achievementClassLevelDTO.LevelName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }

                    }
                    else if (dropdownName.ToUpper().ToString() == "BONUSENTITLEMENT")
                    {

                        lookupDataObject = new CommonLookupDTO("", "<SELECT>");
                        lookupDTO.Items.Add(lookupDataObject);

                        List<KeyValuePair<CreditPlusType, string>> bonusEntitlementList = new List<KeyValuePair<CreditPlusType, string>>();
                        bonusEntitlementList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.CARD_BALANCE, "Card Balance"));
                        bonusEntitlementList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.COUNTER_ITEM, "Counter Items Only"));
                        bonusEntitlementList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.GAME_PLAY_BONUS, "Game Play Bonus"));
                        bonusEntitlementList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.GAME_PLAY_CREDIT, "Game Play Credits"));
                        bonusEntitlementList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.LOYALTY_POINT, "Loyalty Points"));
                        bonusEntitlementList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.TICKET, "Tickets"));
                        bonusEntitlementList.Add(new KeyValuePair<CreditPlusType, string>(CreditPlusType.TIME, "Time"));
                        foreach (KeyValuePair<CreditPlusType, string> bonusEntitlement in bonusEntitlementList)
                        {
                            CommonLookupDTO lookupDataObject;
                            string key = CreditPlusTypeConverter.ToString(bonusEntitlement.Key);
                            lookupDataObject = new CommonLookupDTO(key, bonusEntitlement.Value);
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
    }
}
