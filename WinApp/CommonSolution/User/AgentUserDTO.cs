/* Project Name - AgentUserDTO Programs 
* Description  - Data object of the AgentUserDTO
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*1.00        10-May-2016   Rakshith           Created 
*2.70.2        15-Jul-2019   Girish Kundar      Modified : Added MasterEntityId field to the constructor 
*                                                         and LogMethodEntry() and LogMaethodExit().
********************************************************************************************/
using System;
using System.ComponentModel;


namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the AgentUserDTO data object class. This class Extend  the properties of AgentDTO class
    /// </summary>
    public class AgentUserDTO : AgentsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string loginId;
        protected string email;
        protected string userName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AgentUserDTO()
            : base()
        {
            log.LogMethodEntry();
            loginId = string.Empty;
            email = string.Empty;
            userName = string.Empty;
            log.LogMethodExit();
        }

        

        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public AgentUserDTO(int agentId, int partnerId, int user_Id, string mobileNo, double commission, string createdBy,
                              DateTime creationDate, string lastUpdatedUser, DateTime lastUpdatedDate, bool active,
                              int site_id, string guid, bool synchStatus, string loginid, string email, string userName,int masterEntityId)
            :base( agentId,partnerId, user_Id, mobileNo, commission, createdBy, creationDate, lastUpdatedUser, lastUpdatedDate, active,
                                site_id, guid, synchStatus,masterEntityId)
        {
            log.LogMethodEntry( agentId,  partnerId,  user_Id,  mobileNo,  commission, createdBy,
                               creationDate,  lastUpdatedUser,  lastUpdatedDate,  active,
                               site_id,  guid,  synchStatus,  loginid,  email,  userName,  masterEntityId);
            this.LoginId = loginid;
            this.email = email;
            this.UserName = userName;
            log.LogMethodExit();
        }

       

        
        /// <summary>
        /// Get/Set method of the Loginid field
        /// </summary>
        [DisplayName("LoginId")]
        public string LoginId { get { return loginId; } set { loginId = value; } }

        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        [DisplayName("Email")]
        public string Email { get { return email; } set { email = value; } }

        /// <summary>
        /// Get/Set method of the UserName field
        /// </summary>
        [DisplayName("UserName")]
        public string UserName { get { return userName; } set { userName = value; } }

    }
}
