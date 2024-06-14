/********************************************************************************************
 * Project Name - Product Display Filter DTO
 * Description  - Data object of product Display Filter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        24-May-2016   Jeevan          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ClientApp
{
    /// <summary>
    /// This is the user Registration parametrs object 
    /// </summary>
    public class ClientAppUserParams
    {
        int clientId;
        int userId;
        string token;
        string deviceId;
        string loginId;
        string password;
        string firstName;
        string lastName;
        string email;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientAppUserParams()
        {
             this.clientId = -1;
             this.userId = -1;
             this.token = "";
             this.deviceId = "";
             this.loginId = "";
             this.password = "";
             this.firstName = "";
             this.lastName = "";
             this.email = "";
            
        }

        /// <summary>
        /// Get/Set method of the ClientId field
        /// </summary>
        [DisplayName("ClientId")]
        [DefaultValue(-1)]
        public int ClientId { get { return clientId; } set { clientId = value; } }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("UserId")]
        [DefaultValue(-1)]
        public int UserId { get { return userId; } set { userId = value; } }

        /// <summary>
        /// Get/Set method of the Token field
        /// </summary>
        [DisplayName("Token")]
        [DefaultValue("")]
        public string Token { get { return token; } set { token = value; } }

        /// <summary>
        /// Get/Set method of the DeviceId field
        /// </summary>
        [DisplayName("DeviceId")]
        [DefaultValue("")]
        public string DeviceId { get { return deviceId; } set { deviceId = value; } }

        /// <summary>
        /// Get/Set method of the LoginId field
        /// </summary>
        [DisplayName("LoginId")]
        [DefaultValue("")]
        public string LoginId { get { return loginId; } set { loginId = value; } }

        /// <summary>
        /// Get/Set method of the Password field
        /// </summary>
        [DisplayName("Password")]
        [DefaultValue("")]
        public string Password { get { return password; } set { password = value; } }

        /// <summary>
        /// Get/Set method of the FirstName field
        /// </summary>
        [DisplayName("FirstName")]
        [DefaultValue("")]
        public string FirstName { get { return firstName; } set { firstName = value; } }

        /// <summary>
        /// Get/Set method of the LastName field
        /// </summary>
        [DisplayName("LastName")]
        [DefaultValue("")]
        public string LastName { get { return lastName; } set { lastName = value; } }

        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        [DisplayName("Email")]
        [DefaultValue("")]
        public string Email { get { return email; } set { email = value; } }

       
    }

}
