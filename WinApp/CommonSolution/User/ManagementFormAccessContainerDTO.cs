/********************************************************************************************
 * Project Name - Users
 * Description  - Data structure of the UserRoleViewContainer class holds the information about the management form access
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 2.120.0      10-May-2021      Fiona                     Performance fix for use role container
 ********************************************************************************************/
namespace Semnox.Parafait.User
{
    /// <summary>
    /// Data structure of the UserRoleViewContainer class holds the information about the management form access
    /// </summary>
    public class ManagementFormAccessContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int managementFormAccessId;
        private int roleId;
        private string mainMenu;
        private string formName;
        //private bool accessAllowed;
        //private int functionId;
        private string functionGroup;
        //private string functionGUID;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ManagementFormAccessContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        public ManagementFormAccessContainerDTO(int managementFormAccessId, int roleId, string mainMenu, string formName,
            string functionGroup)
        {
            log.LogMethodEntry(managementFormAccessId, roleId, mainMenu, formName, functionGroup);
            this.managementFormAccessId = managementFormAccessId;
            this.roleId = roleId;
            this.mainMenu = mainMenu;
            this.formName = formName;
            this.functionGroup = functionGroup;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of managementFormAccessId field
        /// </summary>
        public int ManagementFormAccessId
        {
            get
            {
                return managementFormAccessId;
            }
            set
            {
                managementFormAccessId = value;
            }
        }

        /// <summary>
        /// Get/Set method of roleId field 
        /// </summary>
        public int RoleId
        {
            get
            {
                return roleId;
            }
            set
            {
                roleId = value;
            }
        }

        /// <summary>
        /// Get/Set method of mainMenu field 
        /// </summary>
        public string MainMenu
        {
            get
            {
                return mainMenu;
            }
            set
            {
                mainMenu = value;
            }
        }

        /// <summary>
        /// Get/Set method of formName field 
        /// </summary>
        public string FormName
        {
            get
            {
                return formName;
            }
            set
            {
                formName = value;
            }
        }


        /// <summary>
        /// Get/Set method of functionGroup field 
        /// </summary>
        public string FunctionGroup
        {
            get
            {
                return functionGroup;
            }
            set
            {
                functionGroup = value;
            }
        }

    }
}
