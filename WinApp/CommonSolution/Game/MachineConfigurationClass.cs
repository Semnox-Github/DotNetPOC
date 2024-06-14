/********************************************************************************************
 * Project Name - Game
 * Description  - MachineConfigurationClass class used by Game Server under Machine Object
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.2      21-Feb-2023   Abhishek       Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Game
{
    public class MachineConfigurationClass
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Core.Utilities.ExecutionContext ConfigExecutionContext;
        private List<clsConfig> configuration = new List<clsConfig>();

        public MachineConfigurationClass(Core.Utilities.ExecutionContext inExecutionContext)
        {
            log.LogMethodEntry(inExecutionContext);
            ConfigExecutionContext = inExecutionContext;
            log.LogMethodExit(null);
        }

        public MachineConfigurationClass(Core.Utilities.ExecutionContext inExecutionContext, List<clsConfig> machineConfigurationList)
            : this(inExecutionContext)
        {
            log.LogMethodEntry(inExecutionContext, machineConfigurationList);
            configuration = new List<clsConfig>();
            configuration.AddRange(machineConfigurationList);
            log.LogMethodExit(null);
        }

        public class clsConfig
        {
            private string configParameter;
            private string configvalue;
            private bool enableForPromotion;
            public string ConfigParameter
            {
                get { return configParameter; }
                set { configParameter = value; }
            }
            public string Value
            {
                get { return configvalue; }
                set { configvalue = value; }
            }
            public bool EnableForPromotion
            {
                get { return enableForPromotion; }
                set { enableForPromotion = value; }
            }

            public clsConfig(string _configParameter, string _Value, bool _enableForPromotion)
            {
                log.LogMethodEntry(_configParameter, _Value, _enableForPromotion);
                configParameter = _configParameter;
                configvalue = _Value;
                enableForPromotion = _enableForPromotion;
                log.LogMethodExit(null);
            }
        }

        public List<clsConfig> Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        public string getValue(string ConfigParameter)
        {
            log.LogMethodEntry(ConfigParameter);
            clsConfig config = configuration.Find(delegate (clsConfig keyValue) { return keyValue.ConfigParameter == ConfigParameter; });
            if (config != null)
            {
                log.LogMethodExit(config.Value);
                return config.Value;
            }

            else
            {
                log.LogMethodExit("0");
                return "0";
            }
        }

        /// <summary>
        /// Added method to give configuration object based on enableForPromotio flag
        /// </summary>
        /// <param name="enableForPromotion">Bool</param>
        /// <returns>List(ClsConfig)</returns>
        public List<clsConfig> GetConfigList(bool enableForPromotion)
        {
            log.LogMethodEntry(enableForPromotion);
            List<clsConfig> listConfig = configuration.FindAll(x => x.EnableForPromotion);
            log.LogMethodExit(listConfig);
            return listConfig;
        }

        //Added new parameter to set EnableForPromotion property from GameProfileAttributes
        public void addValue(string ConfigParameter, string Value, bool EnableForPromotion)
        {
            log.LogMethodEntry(ConfigParameter, Value, EnableForPromotion);
            clsConfig keyValue = configuration.Find(delegate (clsConfig searchKeyValue) { return searchKeyValue.ConfigParameter == ConfigParameter; });
            if (keyValue == null)
            {
                keyValue = new clsConfig(ConfigParameter, Value, EnableForPromotion);
                configuration.Add(keyValue);
            }
            else
            {
                keyValue.Value = Value;
                keyValue.EnableForPromotion = EnableForPromotion;
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Populate configuration value considering Promotion, PromotionDetail
        /// GameProfile, Game, Machine
        /// </summary>
        /// <param name="MachineId">Machineid</param>
        /// <param name="PromotionDetailId">Promotion Detail Id if applicable else -1</param>
        public void Populate(int MachineId, int PromotionDetailId)
        {
            log.LogMethodEntry(MachineId, PromotionDetailId);
            MachineAttributeListBL machineAttributeListBL = new MachineAttributeListBL(ConfigExecutionContext);
            List<clsConfig> machineAttributeList = machineAttributeListBL.Populate(MachineId, PromotionDetailId);
            log.LogVariableState("machine_id", MachineId);
            log.LogVariableState("promotionDetailId", PromotionDetailId);
            if (machineAttributeList != null && machineAttributeList.Count > 0)
            {
                foreach (clsConfig machineAttribute in machineAttributeList)
                {
                    addValue(machineAttribute.ConfigParameter, machineAttribute.Value, machineAttribute.EnableForPromotion);
                }
            }
            log.LogMethodExit(null);
        }
    }
}

