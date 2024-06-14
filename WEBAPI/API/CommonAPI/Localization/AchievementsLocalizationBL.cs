/********************************************************************************************
 * Project Name - Achievements Localization
 * Description  - Localization for all Literals and messages 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.70        28-Aug-2019   Indrajeet Kumar           Created
 *2.80        17-Feb-2020   Vikas Dwivedi             Resolved Achievement Issues
 *2.80        04-Mar-2020   Vikas Dwivedi             Added Missing Localization
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Newtonsoft.Json;

namespace Semnox.Parafait.Achievements
{
    public class AchievementsLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();

        /// <summary>
        ///   Default Constructor for Games Locallization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public AchievementsLocalizationBL(ExecutionContext executionContext, string entityName)
        {
            log.LogMethodEntry(executionContext, entityName);
            this.executionContext = executionContext;
            this.entityName = entityName;
            log.LogMethodExit();
        }
        private void GetLiteralsAndMessages(string entity, List<string> literalsOrMessageList)
        {
            string localizedValue = "";
            foreach (string literalsOrMessages in literalsOrMessageList)
            {
                if (entity == "CommonMessages")
                {
                    localizedValue = MessageContainerList.GetMessage(executionContext, Convert.ToInt32(literalsOrMessages));
                }
                else
                {
                    localizedValue = MessageContainerList.GetMessage(executionContext, literalsOrMessages);
                }
                if (!listHeadersList.ContainsKey(literalsOrMessages))
                {
                    listHeadersList.Add(literalsOrMessages, localizedValue);
                }
            }
        }
        public string GetLocalizedLabelsAndHeaders()
        {
            log.LogMethodEntry();
            List<string> entities = new List<string>();
            entities.Add(entityName);
            entities.Add("COMMONMESSAGES");
            entities.Add("COMMONLITERALS");
            //Dictionary<string, string> listHeadersList = new Dictionary<string, string>();
            List<string> literalsOrMessage = new List<string>();
            foreach (string entity in entities)
            {
                if (entity == "COMMONMESSAGES")
                {
                    literalsOrMessage = GetMessages(entity);
                }
                else
                {
                    literalsOrMessage = GetLiterals(entity);
                }
                GetLiteralsAndMessages(entity, literalsOrMessage);
            }

            string literalsMessagesList = string.Empty;
            if (listHeadersList.Count != 0)
            {
                literalsMessagesList = JsonConvert.SerializeObject(listHeadersList, Formatting.Indented);
            }
            log.LogMethodExit(literalsMessagesList);
            return literalsMessagesList;
        }

        /// <summary>
        /// Getting label message number and headers
        /// </summary>
        /// <returns>json</returns>

        private List<string> GetMessages(string entityName)
        {
            log.LogMethodEntry(entityName);
            List<string> messages = new List<string>();
            switch (entityName.ToUpper().ToString())
            {
                case "COMMONMESSAGES":
                    messages.Add("1180");//I promise to pay this total subject to and in accordance with the agreement governing the use of this Card.                    
                    messages.Add("958");//Deleting the existing record is not allowed. Do you want make record inactive?
                    messages.Add("1183");//From date should should be less or equal to To date.
                    messages.Add("371");
                    messages.Add("1184");
                    messages.Add("1185");
                    messages.Add("1186");
                    messages.Add("963");
                    messages.Add("1179");
                    messages.Add("959");
                    messages.Add("957");
                    messages.Add("1181");
                    break;

            }
            return messages;
        }

        public enum AchievementEntityName
        {
            COMMONLITERALS,
            PROJECTS,
            ACHIEVEMENTCLASS
        }

        private List<string> GetLiterals(string entityName)
        {
            log.LogMethodEntry(entityName);
            try
            {
                List<string> literals = new List<string>();
                AchievementEntityName achievementEntityName = (AchievementEntityName)Enum.Parse(typeof(AchievementEntityName), entityName);
                switch (achievementEntityName)
                {
                    case AchievementEntityName.PROJECTS:
                        literals.Add("Achievement Projects");
                        literals.Add("Project Id");
                        literals.Add("Project Name");
                        literals.Add("Active");
                        literals.Add("Achievement Class");
                        break;

                    case AchievementEntityName.ACHIEVEMENTCLASS:
                        literals.Add("Achievement Class");
                        literals.Add("Select Project");
                        literals.Add("Achievement Class Id");
                        literals.Add("Class Name");
                        literals.Add("Game Name");
                        literals.Add("Active");
                        literals.Add("External System Reference Id");
                        literals.Add("Product");
                        literals.Add("Achievement Class Level");
                        literals.Add("Achievement Class Level Id");
                        literals.Add("Level Name");
                        literals.Add("Qualifying Score");
                        literals.Add("Qualifying Level");
                        literals.Add("Registration Required");
                        literals.Add("Bonus Amount");
                        literals.Add("Bonus Entitlement");
                        literals.Add("Medal");
                        literals.Add("Ratio");
                        literals.Add("Conversion Entitlement");
                        literals.Add("From Date");
                        literals.Add("To Date");
                        literals.Add("Id");
                        literals.Add("Achievement Score Conversion");
                        break;

                    case AchievementEntityName.COMMONLITERALS:
                        literals.Add("Save");
                        literals.Add("Refresh");
                        literals.Add("Delete");
                        literals.Add("Close");
                        break;
                }
                log.LogMethodExit(literals);
                return literals;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
