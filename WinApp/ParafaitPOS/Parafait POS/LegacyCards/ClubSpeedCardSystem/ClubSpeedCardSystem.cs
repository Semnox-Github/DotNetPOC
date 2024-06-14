/*/********************************************************************************************
 * Project Name - ClubSpeedCardSystem
 * Description  - Club Speed Card System
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.100.0     1-Sep-2020    Dakshakh raj            Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.ThirdParty.GoKart.ClubSpeed;
using System.Data.SqlClient;

namespace Parafait_POS
{
    class ClubSpeedCardSystem : ExternalCardSystem
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private LegacyCardDTO legacyCardDTO;

        /// <summary>
        /// ClubSpeedCardSystem 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="legacyCardDTO"></param>
        public ClubSpeedCardSystem(ExecutionContext executionContext, LegacyCardDTO legacyCardDTO)
            : base(executionContext, legacyCardDTO)
        {
            log.LogMethodEntry(executionContext, legacyCardDTO);
            this.executionContext = executionContext;
            this.legacyCardDTO = legacyCardDTO;
            log.LogMethodExit(null);
        }
        /// <summary>
        /// GetCardInformation
        /// </summary>
        /// <returns></returns>
        public override LegacyCardDTO GetCardInformation()
        {
            log.LogMethodEntry();
            try
            {
                GiftCardBalanceBL giftCardBalanceBL = new GiftCardBalanceBL(executionContext, legacyCardDTO.CardNumber);
                ExternalCardReferenceDTO externalCardReferenceDTO;
                externalCardReferenceDTO = giftCardBalanceBL.GetGiftCardBalanceDTO();
                log.LogMethodExit();
                return MapCards(externalCardReferenceDTO);
            }
            catch (ValidationException ex)

            {
                log.Error(ex.Message);
                log.LogMethodExit(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing an Exception - " + ex.Message);
                throw ex;
            }

        }

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            log.LogMethodEntry();
            string interfaceUri = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CLUBSPEED_INTERFACE_URI");
            if (!string.IsNullOrEmpty(interfaceUri))
            {
                ClubSpeedEnvironment.INTERFACE_URI = interfaceUri;
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 2866);
                throw new Exception(message);
            }
            //Need to check for Encrypted Auth key POSStatic.Utilities.getParafaitDefaults("CLUBSPEED_AUTHENTICATION_KEY");
            string encryptedAuthenticationKey = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CLUBSPEED_AUTHENTICATION_KEY");
            if (!string.IsNullOrEmpty(encryptedAuthenticationKey))
            {
                ClubSpeedEnvironment.AUTHENTICATION_KEY = string.Format("key={0}", encryptedAuthenticationKey);
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 2867);
                throw new Exception(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Used to map ExternalCardReferenceDTO to LegacyCardDTO
        /// </summary>
        /// <param name="externalCardReferenceDTO"></param>
        /// <returns></returns>
        private LegacyCardDTO MapCards(ExternalCardReferenceDTO externalCardReferenceDTO)
        {
            log.LogMethodEntry();
            if (externalCardReferenceDTO != null)
            {
                legacyCardDTO.CardNumber = Convert.ToString(externalCardReferenceDTO.CustomerId);
                legacyCardDTO.Credits = externalCardReferenceDTO.Points;
                log.LogMethodExit(legacyCardDTO);
                return legacyCardDTO;
            }
            else
            {
                legacyCardDTO = null;
                log.LogMethodExit(legacyCardDTO);
                return legacyCardDTO;
            }
        }
        /// <summary>
        /// ProcessCardData
        /// </summary>
        /// <param name="sqltransaction"></param>
        public override void ProcessCardData(SqlTransaction sqltransaction)
        {
            log.LogMethodEntry(sqltransaction);
            try
            {
                RacersDTO racersDTO = new RacersDTO(legacyCardDTO.CustomerId,null,null,null,null,DateTime.MinValue,-1,null,-1,null,null,null,null,null,null,null,null,-1,-1,false,0,null,null,null);
                racersDTO.PointAmount = -Convert.ToInt32(legacyCardDTO.Credits);
                racersDTO.Type = -1;
                racersDTO.Notes = "Deducting Points to the customer";
                RacersBL racersBL = new RacersBL(executionContext, racersDTO);
                log.LogMethodExit();
                racersBL.UpdatePointHistory();
            }
            catch (ValidationException ex)
            {
                log.LogMethodExit(ex);
                log.Error(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error(ex.Message);
                throw ex;
            }
        }
    }
}
