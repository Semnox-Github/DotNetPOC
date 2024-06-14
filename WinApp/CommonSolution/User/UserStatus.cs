/********************************************************************************************
 * Project Name - User
 * Description  - Represents different user status
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Represents different user status
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// Active User
        /// </summary>
        ACTIVE,
        /// <summary>
        /// Inactive User
        /// </summary>
        INACTIVE,
        /// <summary>
        /// User locked out
        /// </summary>
        LOCKED,
        /// <summary>
        /// User disabled 
        /// </summary>
        DISABLED
    }
}
