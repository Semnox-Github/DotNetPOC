/********************************************************************************************
 * Project Name - UserAuthStructDTO
 * Description  - UserAuthStructDTO object of user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        25-May-2016   Rakshith          Created 
 *2.70.2        12-Aug-2019   Deeksha           Added logger methods.
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.POS
{
    /// <summary>
    ///  This is the UserAuthStructDTO object class. This acts as data holder for the user business object
    /// </summary>    
    public class UserAuthStructDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string loginId;
        string email;
        string passwordPassed;
        string newPassword;
        string macAddressPassed;
        bool isGameMachine;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserAuthStructDTO()
        {
            log.LogMethodEntry();
            this.loginId = "";
            this.email = "";
            this.passwordPassed = "";
            this.newPassword = "";
            this.macAddressPassed = "";
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        public UserAuthStructDTO(string loginId, string passwordPassed,string newPassword, string macAddressPassed, bool isGameMachine)
        {
            log.LogMethodEntry(loginId, "passwordPassed", "newPassword", "macAddressPassed", isGameMachine);
            this.loginId = loginId;
            this.passwordPassed = passwordPassed;
            this.newPassword = newPassword;
            this.macAddressPassed = macAddressPassed;
            this.isGameMachine = isGameMachine;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter loginId,email , macAddressPassed
        /// </summary>
        public UserAuthStructDTO(string loginId, string email, string macAddressPassed)
        {
            log.LogMethodEntry(loginId, email, "macAddressPassed");
            this.loginId = loginId;
            this.email = email;
            this.macAddressPassed = macAddressPassed;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the LoginId field
        /// </summary>
        [DisplayName("LoginId")]
        public string LoginId { get { return loginId; } set { loginId = value;  } }

        /// <summary>
        /// Get/Set method of the email field
        /// </summary>
        [DisplayName("Email")]
        public string Email { get { return email; } set { email = value; } }

        /// <summary>
        /// Get/Set method of the PasswordPassed field
        /// </summary>
        [DisplayName("PasswordPassed")]
        public string PasswordPassed { get { return passwordPassed; } set { passwordPassed = value;  } }
       
        /// <summary>
        /// Get/Set method of the PasswordPassed field
        /// </summary>
        [DisplayName("NewPassword")]
        public string NewPassword { get { return newPassword; } set { newPassword = value;} }

        /// <summary>
        /// Get/Set method of the MacAddressPassed field
        /// </summary>
        [DisplayName("MacAddressPassed")]
        public string MacAddressPassed { get { return macAddressPassed; } set { macAddressPassed = value; } }

        /// <summary>
        /// Get/Set method of the IsGameMachine field
        /// </summary>
        [DisplayName("IsGameMachine")]
        [DefaultValue(false)]
        public bool IsGameMachine { get { return isGameMachine; } set { isGameMachine = value; } }

    }
}
