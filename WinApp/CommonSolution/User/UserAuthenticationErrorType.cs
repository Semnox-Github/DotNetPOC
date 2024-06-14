namespace Semnox.Parafait.User
{
    /// <summary>
    /// Represent the different types of authentication errors
    /// </summary>
    public enum UserAuthenticationErrorType
    {
        /// <summary>
        /// Login Id is Invalid
        /// </summary>
        INVALID_LOGIN = 807,
        /// <summary>
        /// Invalid password
        /// </summary>
        INVALID_PASSWORD = 808,
        /// <summary>
        /// User disabled
        /// </summary>
        USER_DISABLED = 809,
        /// <summary>
        /// User inactive
        /// </summary>
        USER_INACTIVE = 810,
        /// <summary>
        /// User locked out
        /// </summary>
        USER_LOCKED_OUT = 811,
        /// <summary>
        /// Change password is required
        /// </summary>
        CHANGE_PASSWORD = 812,
        /// <summary>
        /// Role for the user not defined
        /// </summary>
        ROLE_NOT_DEFINED = 813,
        /// <summary>
        /// Invalid user tag
        /// </summary>
        INVALID_USER_TAG = 819,
    }
}
