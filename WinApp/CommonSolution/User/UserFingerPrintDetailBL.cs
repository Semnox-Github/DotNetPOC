/********************************************************************************************
* Project Name - UserFingerPrintDetailBL
* Description  - 
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*2.80        09-Mar-2020   Indrajeet Kumar    Created 
********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Biometric;

namespace Semnox.Parafait.User
{
    public class UserFingerPrintDetailBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<UserFingerPrintDetailDTO> userFingerPrintDetailDTOList = null;
        UserFingerPrintDetailDTO userFingerPrintDetailDTO = null;

        /// <summary>
        /// Parametrized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public UserFingerPrintDetailBL(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        /// <summary>
        /// Below Method create the List of UserFingerPrintDetailDTO based on UsersDTO List.
        /// </summary>
        /// <param name="usersDTOList"></param>
        /// <returns></returns>
        public List<UserFingerPrintDetailDTO> CreateUsersFPDetailsList(List<UsersDTO> usersDTOList)
        {
            log.LogMethodEntry(usersDTOList);
            userFingerPrintDetailDTOList = new List<UserFingerPrintDetailDTO>();
            log.Debug("usersDTOList Count : " + usersDTOList.Count);
            log.Debug("usersDTOList Value : " + usersDTOList);
            if (usersDTOList != null && usersDTOList.Any())
            {
                //usersDTOList = usersDTOList.Where(x => x.UserIdentificationTagsDTO.Where(y => y.FingerNumber > 0 && y.FPTemplate.Length > 0) !=null).ToList();
                //userFingerPrintDetailDTOList = usersDTOList.Where(x => x.UserIdentificationTagsDTO.Where(x => x.FingerNumber > 0 && x.FPTemplate != null) != null).ToList();                                
                foreach (UsersDTO usersDTO in usersDTOList)
                {
                    log.Debug("UserIdentificationTagsDTO in UserFingerPrintDetailBL Value : " + usersDTO.UserIdentificationTagsDTOList);
                    if (usersDTO.UserIdentificationTagsDTOList != null)
                    {
                        foreach (UserIdentificationTagsDTO userIdentificationTagsDTO in usersDTO.UserIdentificationTagsDTOList)
                        {
                            log.Debug("UserId Value : " + userIdentificationTagsDTO.UserId + " UserIdentificationTagsDTO FPTemplate : " + userIdentificationTagsDTO.FPTemplate);
                            log.Debug("Before UserIdentificationTagsDTO Value : " + userIdentificationTagsDTO); 
                            if (userIdentificationTagsDTO.FPTemplate != null && userIdentificationTagsDTO.FPTemplate.Length > 0 && userIdentificationTagsDTO.FingerNumber > 0)
                            {
                                log.Debug("After UserIdentificationTagsDTO Value : " + userIdentificationTagsDTO);
                                userFingerPrintDetailDTO = new UserFingerPrintDetailDTO();
                                userFingerPrintDetailDTO.UserId = userIdentificationTagsDTO.UserId;
                                userFingerPrintDetailDTO.FingerNumber = userIdentificationTagsDTO.FingerNumber;
                                userFingerPrintDetailDTO.FPTemplate = userIdentificationTagsDTO.FPTemplate;                                
                                userFingerPrintDetailDTOList.Add(userFingerPrintDetailDTO);
                            }                            
                        }
                    }                    
                }
            }
            log.LogMethodExit(userFingerPrintDetailDTOList);
            log.Debug("End - userFingerPrintDetailDTOList.Count " + userFingerPrintDetailDTOList.Count + "userFingerPrintDetailDTOList Value" + userFingerPrintDetailDTOList);
            return userFingerPrintDetailDTOList;
        }
    }
}
