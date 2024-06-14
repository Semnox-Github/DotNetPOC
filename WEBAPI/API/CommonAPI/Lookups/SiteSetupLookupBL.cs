/********************************************************************************************
 * Project Name - Site setup
 * Description  - Business logic of lookups for Site setup module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70.0      24-Jun-2019   Mushahid Faizan      Created
 *2.70.3      01-Apr-2020   Girish Kundar        Modified: PRINTER_TYPE return description as look up value
 *2.110.0     31-Jan-2021   Dakshakh Raj         Modified: Peru Invoice changes 
 *2.130.4     01-Mar-2022   Abhishek             Modified: Added @VirtualQueueURL, @QRCodeVirtualQueueURL, @VirtualQueueText 
                                                 help variable under Header/Footer section for print.
 *2.130.7     30-Mar-2022   Abhishek             Modified: Added WBPRINTERMODEL to Load RFID_WRISTBAND_MODELS 
 *2.130.10    13-Sep-2022   Abhishek             Modified: Added @AdvancePaidText, @AdvancePaidAmount
                                                 help variable under Header/Footer section for print.
 *2.160.0     14-Dec-2022   Abhishek             Modified: Added OPTIONNAMEUSER dropdown in PRAFAITOPTIONS to get 
                                                 parafait defaults based on User Level.                                                 
 *2.152.00    08-Feb-2024   Adarsh S Shetty      Vietnam Fiscalization Changes
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Site;
using Newtonsoft.Json;
using Semnox.Parafait.PriceList;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Product;
using Semnox.Parafait.Printer;
using Semnox.Parafait.User;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using System.Linq;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.TableAttributeSetup;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.DeliveryIntegration;

namespace Semnox.CommonAPI.Lookups
{
    public class SiteSetupLookupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string entityName;
        private ExecutionContext executionContext;
        string dependentDropdownName = string.Empty;
        string keyValuePair = string.Empty;
        private List<LookupValuesDTO> lookupValuesDTOList;
        string isActive = string.Empty;
        DataAccessHandler dataAccessHandler = new DataAccessHandler();
        private CommonLookupsDTO lookupDTO;

        /// <summary>
        /// Constructor for the method SiteSetupLookupBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        public SiteSetupLookupBL(string entityName, ExecutionContext executioncontext, string dependentDropdownName, string keyValuePair, string isActive)
        {
            log.LogMethodEntry(entityName, executioncontext);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            this.dependentDropdownName = dependentDropdownName;
            this.keyValuePair = keyValuePair;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor for the method SiteSetupLookupBL()
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="executioncontext"></param>
        /// <param name="keyValuePair"></param>
        public SiteSetupLookupBL(string entityName, ExecutionContext executioncontext, string keyValuePair)
        {
            log.LogMethodEntry(entityName, executioncontext, keyValuePair);
            this.entityName = entityName;
            this.executionContext = executioncontext;
            this.keyValuePair = keyValuePair;
            this.lookupDTO = new CommonLookupsDTO();
            log.LogMethodExit();
        }
        /// <summary>
        /// Enumuration for AccountsEntityNameLookup
        /// </summary>
        public enum SiteSetupEntityNameLookup
        {
            SITE,
            USERS,
            USERROLES,
            POSMANAGEMENT,
            PERIPHERALS,
            PARAFAITOPTIONS,
            POSMANAGEMENTPRODUCTS,
            RECEIPTTEMPLATE,
            LOOKUPS,
            TRANSACTIONPROFILES,
            MESSAGES,
            CUSTOMATTRIBUTES,
            KIOSKSETUP,
            TABLELAYOUT,
            ORDERTYPE,
            INVOICESEQUENCESETUP,
            DATAACCESSRULE,
            ATTENDANCEROLES,
            PRODUCTKEY,
            TICKETTEMPLATE,
            PARAFAITCONFIGURATION,
            HQOPTIONS,
            RECEIPTTEMPLATEHELP,
            EMAILTEMPLATEHELP,
            TICKETSTATION,
            WBPRINTERMODEL,
            DELIVERYINTEGRATION,
            TABLEATTRIBUTESETUP
        }
        /// <summary>
        /// Gets the All look ups for all dropdowns based on the page in the Site Setup module.
        /// </summary>       
        public List<CommonLookupsDTO> GetLookUpMasterDataList(int roleId)
        {
            try
            {
                log.LogMethodEntry();

                List<CommonLookupsDTO> completeTablesDataList = new List<CommonLookupsDTO>();
                string[] dropdowns = null;
                string dropdownNames = string.Empty;
                SiteSetupEntityNameLookup siteSetupEntityNameLookupValue = (SiteSetupEntityNameLookup)Enum.Parse(typeof(SiteSetupEntityNameLookup), entityName);
                switch (siteSetupEntityNameLookupValue)
                {
                    case SiteSetupEntityNameLookup.SITE:
                        dropdownNames = "STRUCTURENAME,ORGNAME,DELIVERY_TYPE,DELIVERY_CHANNELS";
                        break;
                    case SiteSetupEntityNameLookup.USERS:
                        dropdownNames = "ROLE,STATUS,COUNTER,DEPARTMENT,MANAGER,ATTENDANCE_ROLES";
                        break;
                    case SiteSetupEntityNameLookup.USERROLES:
                        dropdownNames = "ASSIGNEDMANAGERROLE,SECURITY,ACCESS_LEVEL,DATA_ACCESS_RULE,PRICELIST,DISPLAYGROUP,ATTENDANCE_ROLES";
                        break;
                    case SiteSetupEntityNameLookup.DATAACCESSRULE:
                        dropdownNames = "DATA_ACCESS_ENTITY,DATA_ACCESS_LIMIT,DATA_EXCLUSION_ENTITY,DATA_ACCESS_LEVEL,MEMBERSHIP,INVENTORYADJUSTMENTSUI,PHYSICALCOUNTUI1,PHYSICALCOUNTUI2,INVENTORYLOTUI1";
                        break;
                    case SiteSetupEntityNameLookup.ATTENDANCEROLES:
                        dropdownNames = "ATTENDANCE_ROLES";
                        break;
                    case SiteSetupEntityNameLookup.POSMANAGEMENT:
                        dropdownNames = "COUNTER,INVENTORYLOCATION,PRINTER,PRINTTEMPLATE,PRINTER_TYPE,ORDERTYPEGROUP,FISCALIZATION_TYPE,OVERRIDE_OPTION_ITEM";
                        break;
                    case SiteSetupEntityNameLookup.PERIPHERALS:
                        dropdownNames = "DEVICETYPE,DEVICESUBTYPE";
                        break;
                    case SiteSetupEntityNameLookup.PARAFAITOPTIONS:
                        dropdownNames = "OPTIONNAME";
                        break;
                    case SiteSetupEntityNameLookup.RECEIPTTEMPLATE:
                        dropdownNames = "SECTION,ALIGNMENT,DISPLAYGROUP,PRINTER_TYPE,WBPRINTERMODEL,PAPERSIZE";
                        break;
                    case SiteSetupEntityNameLookup.LOOKUPS:
                        dropdownNames = "PAYMENT_GATEWAY,POS,ORDERTYPEGROUP,INVOICESEQUENCES,PAYMENTMODECHANNEL,MESSAGINGCHANNELTYPE,ATTRIBUTENAME,DISPLAYGROUP";
                        break;
                    case SiteSetupEntityNameLookup.TRANSACTIONPROFILES:
                        dropdownNames = "TAX,TAXSTRUCTURE";
                        break;
                    case SiteSetupEntityNameLookup.MESSAGES:
                        dropdownNames = "MESSAGES";
                        break;
                    case SiteSetupEntityNameLookup.CUSTOMATTRIBUTES:
                        dropdownNames = "TYPE,APPLICABILITY";
                        break;
                    case SiteSetupEntityNameLookup.KIOSKSETUP:
                        dropdownNames = "NOTECOINFLAG";
                        break;
                    case SiteSetupEntityNameLookup.TABLELAYOUT:
                        dropdownNames = "FACILITY,TABLETYPE";
                        break;
                    case SiteSetupEntityNameLookup.ORDERTYPE:
                        dropdownNames = "ORDERTYPEGROUP";
                        break;
                    case SiteSetupEntityNameLookup.INVOICESEQUENCESETUP:
                        dropdownNames = "INVOICETYPE";
                        break;
                    case SiteSetupEntityNameLookup.TICKETTEMPLATE:
                        dropdownNames = "BACKSIDETEMPLATE,MAINSECTION,PRODUCT,TOTALS,CARDINFO,DISCOUNT,COUPONDETAILSEXTRA,DATE,NUMBER,ENCODEOPTIONS";
                        break;
                    case SiteSetupEntityNameLookup.PARAFAITCONFIGURATION:
                        dropdownNames = "DATATYPE";
                        break;
                    case SiteSetupEntityNameLookup.HQOPTIONS:
                        dropdownNames = "HQOPTIONS";
                        break;
                    case SiteSetupEntityNameLookup.RECEIPTTEMPLATEHELP:
                        dropdownNames = "RECEIPTTEMPLATEHELP";
                        break;
                    case SiteSetupEntityNameLookup.EMAILTEMPLATEHELP:
                        dropdownNames = "EMAILTEMPLATEHELP";
                        break;
                    case SiteSetupEntityNameLookup.TICKETSTATION:
                        dropdownNames = "CHECKBITALGORITHIM,TICKETSTATIONTYPE";
                        break;
                    case SiteSetupEntityNameLookup.DELIVERYINTEGRATION:
                        dropdownNames = "INTEGRATION_NAME,FOOD_TYPE,GOODS_TYPE,PAYMENT_MODE,AGGREGATOR_DISCOUNT,PACKING_CHARGE_PRODUCT,DEFAULT_RIDER";
                        break;
                    case SiteSetupEntityNameLookup.TABLEATTRIBUTESETUP:
                        dropdownNames = "TABLE_NAME,LOOKUPS,DATASOURCETYPE,DATA_TYPE";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                foreach (string dropdownName in dropdowns)
                {
                    lookupDTO = new CommonLookupsDTO
                    {
                        Items = new List<CommonLookupDTO>(),
                        DropdownName = dropdownName
                    };

                    if (dropdownName.ToUpper().ToString() == "DISPLAYGROUP")
                    {
                        ProductDisplayGroupList productDisplayList = new ProductDisplayGroupList(executionContext);
                        List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> SearchParameters = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                        SearchParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        SearchParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE, "1"));
                        List<ProductDisplayGroupFormatDTO> displayFormatDtoList = productDisplayList.GetAllProductDisplayGroup(SearchParameters);

                        if (displayFormatDtoList != null && displayFormatDtoList.Any())
                        {
                            foreach (ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO in displayFormatDtoList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productDisplayGroupFormatDTO.Id), productDisplayGroupFormatDTO.DisplayGroup);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ORGNAME")
                    {
                        LoadDefaultValue("<SELECT>");
                        OrganizationList organizationBL = new OrganizationList();
                        List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>> searchParam = new List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>>();
                        List<OrganizationDTO> organizationList = organizationBL.GetAllOrganizations(searchParam);
                        if (organizationList != null && organizationList.Any())
                        {
                            foreach (var org in organizationList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(org.OrgId), org.OrgName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TABLE_NAME")
                    {
                        List<AttributeEnabledTablesContainerDTO> attributeEnabledTablesContainerDTOList = AttributeEnabledTablesViewContainerList.GetAttributeEnabledTablesContainerDTOList(executionContext);
                        if (attributeEnabledTablesContainerDTOList != null && attributeEnabledTablesContainerDTOList.Any())
                        {
                            foreach (var attributeEnabledTablesContainerDTO in attributeEnabledTablesContainerDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(attributeEnabledTablesContainerDTO.AttributeEnabledTableId), attributeEnabledTablesContainerDTO.TableName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "LOOKUPS")
                    {
                        List<LookupsContainerDTO> lookupsContainerDTOList = LookupsViewContainerList.GetLookupsContainerDTOList(executionContext);
                        if (lookupsContainerDTOList != null && lookupsContainerDTOList.Any())
                        {
                            foreach (var lookup in lookupsContainerDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookup.LookupId), lookup.LookupName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "STRUCTURENAME")
                    {
                        LoadDefaultValue("<SELECT>");
                        OrganizationStructureList organizationStructureList = new OrganizationStructureList();
                        List<KeyValuePair<OrganizationStructureDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<OrganizationStructureDTO.SearchByParameters, string>>();
                        List<OrganizationStructureDTO> organizationStructureDTOList = organizationStructureList.GetAllOrganizationStructure(searchParam);
                        if (organizationStructureDTOList != null && organizationStructureDTOList.Any())
                        {
                            foreach (var structureName in organizationStructureDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(structureName.StructureId), structureName.StructureName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATATYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        DefaultDataTypeListBL defaultDataTypeListBL = new DefaultDataTypeListBL(executionContext);
                        List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>>();
                        List<DefaultDataTypeDTO> defaultDataTypeDTOList = defaultDataTypeListBL.GetDefaultDataTypeValues(searchParam);
                        if (defaultDataTypeDTOList != null && defaultDataTypeDTOList.Any())
                        {
                            foreach (var datatype in defaultDataTypeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(datatype.DatatypeId), datatype.Datatype);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATASOURCETYPE")
                    {
                        int i = 0;
                        foreach (string name in Enum.GetNames(typeof(TableAttributeSetupDTO.DataSourceTypeEnum)))
                        {
                            CommonLookupDTO lookupDataObject = new CommonLookupDTO(i.ToString(), name);
                            lookupDTO.Items.Add(lookupDataObject);
                            i++;
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATA_TYPE")
                    {
                        int i = 0;
                        foreach (string name in Enum.GetNames(typeof(TableAttributeSetupDTO.DataTypeEnum)))
                        {
                            CommonLookupDTO lookupDataObject = new CommonLookupDTO(i.ToString(), name);
                            lookupDTO.Items.Add(lookupDataObject);
                            i++;
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PAYMENTMODECHANNEL")
                    {
                        LoadLookupValues("PAYMENT_CHANNELS");                        
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (var paymentChannel in lookupValuesDTOList)
                            {
                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    { "Description", Convert.ToString(paymentChannel.Description) }
                                };
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(paymentChannel.LookupValueId), paymentChannel.LookupValue, values);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MEMBERSHIP")
                    {
                        LoadDefaultValue("<SELECT>");
                        MembershipsList membershipsListBL = new MembershipsList(executionContext);
                        List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<MembershipDTO> membershipList = membershipsListBL.GetAllMembership(searchParam, executionContext.GetSiteId());
                        if (membershipList != null && membershipList.Any())
                        {
                            foreach (var membership in membershipList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(membership.Guid), membership.MembershipName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "INVOICETYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("INVOICE_TYPE");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ASSIGNEDMANAGERROLE" || dropdownName.ToUpper().ToString() == "ATTENDANCE_ROLES")
                    {
                        LoadDefaultValue("<SELECT>");
                        UserRolesList userRoleList = new UserRolesList();
                        List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> SearchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                        SearchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<UserRolesDTO> userRoleDtoList = userRoleList.GetAllUserRoles(SearchParameters);
                        if (userRoleDtoList != null && userRoleDtoList.Any())
                        {
                            userRoleDtoList = userRoleDtoList.OrderBy(x => x.Role).ToList();
                            foreach (UserRolesDTO userRolesDTO in userRoleDtoList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(userRolesDTO.RoleId), userRolesDTO.Role);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MANAGER")
                    {
                        LoadDefaultValue("<SELECT>");
                        UsersList usersList = new UsersList(executionContext);
                        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> SearchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        SearchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<UsersDTO> usersDtoList = usersList.GetAllUsers(SearchParameters);
                        if (usersDtoList != null && usersDtoList.Any())
                        {
                            foreach (UsersDTO usersDTO in usersDtoList)
                            {
                                CommonLookupDTO lookupDataObject;
                                if (usersDTO.IsActive == true)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.UserName);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(usersDTO.UserId), usersDTO.UserName + "( Inactive )");
                                }
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ROLE")
                    {
                        LoadDefaultValue("<SELECT>");
                        Users users = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
                        int userId = -1;
                        List<int> roles = new List<int>();
                        if (users.UserDTO != null)
                        {
                            userId = users.UserDTO.UserId;
                        }
                        if (userId > 0)
                        {
                            UserRoles userRoles = new UserRoles(executionContext, -1);
                            string rolesString = userRoles.GetDataAccessRuleLookup(userId).Replace("(", "").Replace(")", ""); ;
                            roles = new List<int>(rolesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)));
                        }

                        UserRolesList userRoleList = new UserRolesList();
                        List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> SearchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                        SearchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<UserRolesDTO> userRoleDtoList = userRoleList.GetAllUserRoles(SearchParameters);
                        if (userRoleDtoList != null && userRoleDtoList.Count != 0)
                        {
                            int siteId = -1;
                            if (executionContext.GetIsCorporate())
                            {
                                siteId = executionContext.GetSiteId();
                            }
                            userRoleDtoList = (from mm in userRoleDtoList
                                               where (mm.SiteId == siteId || siteId == -1) && (mm.Role != "Semnox Admin" || "semnox" == executionContext.GetUserId())
                                    && (mm.RoleId ==roleId || roles.Contains(mm.RoleId))
                                               select mm).ToList();
                            userRoleDtoList = userRoleDtoList.OrderBy(x => x.Role).ToList();
                            if (userRoleDtoList != null && userRoleDtoList.Any())
                            {
                                userRoleDtoList.ForEach(userRolesDTO =>
                                {
                                    string roleName = userRolesDTO.IsActive ? userRolesDTO.Role : userRolesDTO.Role + MessageContainerList.GetMessage(executionContext, "(In Active)");
                                    lookupDTO.Items.Add(new CommonLookupDTO(Convert.ToString(userRolesDTO.RoleId), roleName));
                                });
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DEPARTMENT")
                    {
                        LoadDefaultValue("<SELECT>");
                        DepartmentList departmentList = new DepartmentList(executionContext);
                        List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<DepartmentDTO.SearchByParameters, string>>();
                        searchParameter.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.ISACTIVE, "Y"));
                        searchParameter.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<DepartmentDTO> departmentDTOList = departmentList.GetDepartmentDTOList(searchParameter);
                        if (departmentDTOList != null && departmentDTOList.Any())
                        {
                            foreach (var department in departmentDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(department.DepartmentId), department.DepartmentName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FACILITY")
                    {
                        FacilityList facilityList = new FacilityList(executionContext);
                        List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                        List<FacilityDTO> facilityDTOList = facilityList.GetFacilityDTOList(searchParameters);
                        if (facilityDTOList != null && facilityDTOList.Any())
                        {
                            facilityDTOList = facilityDTOList.OrderBy(x => x.FacilityName).ToList();
                            foreach (var facility in facilityDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(facility.FacilityId), facility.FacilityName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "COUNTER")
                    {
                        LoadDefaultValue("<SELECT>");
                        POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
                        List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<POSTypeDTO> posTypeList = pOSTypeListBL.GetPOSTypeDTOList(searchParam);
                        if (posTypeList != null && posTypeList.Any())
                        {
                            foreach (var posType in posTypeList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(posType.POSTypeId), posType.POSTypeName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MESSAGES")
                    {
                        LoadDefaultValue("<SELECT>");
                        Languages language = new Languages(executionContext);
                        List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LanguagesDTO> languagesDTOList = language.GetAllLanguagesList(searchParameters);
                        if (languagesDTOList != null && languagesDTOList.Any())
                        {
                            languagesDTOList = languagesDTOList.OrderBy(x => x.LanguageName).ToList();
                            foreach (var languages in languagesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(languages.LanguageId), languages.LanguageName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TAX")
                    {
                        LoadDefaultValue("<SELECT>");
                        TaxList taxList = new TaxList(executionContext);
                        List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<TaxDTO> taxDTOList = taxList.GetAllTaxes(searchParameters);
                        if (taxDTOList != null && taxDTOList.Any())
                        {
                            foreach (var tax in taxDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(tax.TaxId), tax.TaxName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "POS")
                    {
                        LoadDefaultValue("<SELECT>");
                        POSMachineList pOSMachineList = new POSMachineList(executionContext);
                        List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParam = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                        searchParam.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<POSMachineDTO> pOSMachineDTOs = pOSMachineList.GetAllPOSMachines(searchParam);
                        if (pOSMachineDTOs != null && pOSMachineDTOs.Any())
                        {
                            foreach (var posMachines in pOSMachineDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(posMachines.POSMachineId), posMachines.POSName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "INVOICESEQUENCES")
                    {
                        LoadDefaultValue("<SELECT>");

                        InvoiceSequenceSetupListBL invoiceSequenceSetupListBL = new InvoiceSequenceSetupListBL(executionContext);
                        List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.ISACTIVE, "1"));

                        searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList = invoiceSequenceSetupListBL.GetAllInvoiceSequenceSetupList(searchParameters);
                        if (invoiceSequenceSetupDTOList != null && invoiceSequenceSetupDTOList.Any())
                        {
                            LoadLookupValues("INVOICE_TYPE");
                            foreach (InvoiceSequenceSetupDTO invoiceSequenceSetupDTO in invoiceSequenceSetupDTOList)
                            {
                                Dictionary<string, string> dependentValues = new Dictionary<string, string>();
                                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                                {
                                    string invoiceTypeName = lookupValuesDTOList.Find(m => m.LookupValueId == invoiceSequenceSetupDTO.InvoiceTypeId).LookupValue;
                                    dependentValues = new Dictionary<string, string>
                                        {
                                            { "CurrentValue", Convert.ToString(invoiceSequenceSetupDTO.CurrentValue) },
                                            { "SeriesStartNumber", Convert.ToString(invoiceSequenceSetupDTO.SeriesStartNumber) },
                                            { "SeriesEndNumber", Convert.ToString(invoiceSequenceSetupDTO.SeriesEndNumber) },
                                            { "ExpiryDate", Convert.ToString(invoiceSequenceSetupDTO.ExpiryDate) },
                                            { "InvoiceType", invoiceTypeName }
                                        };
                                }
                                else
                                {
                                    dependentValues = new Dictionary<string, string>
                                        {
                                            { "CurrentValue", Convert.ToString(invoiceSequenceSetupDTO.CurrentValue) },
                                            { "SeriesStartNumber", Convert.ToString(invoiceSequenceSetupDTO.SeriesStartNumber) },
                                            { "SeriesEndNumber", Convert.ToString(invoiceSequenceSetupDTO.SeriesEndNumber) },
                                            { "ExpiryDate", Convert.ToString(invoiceSequenceSetupDTO.ExpiryDate) },
                                            { "InvoiceType", string.Empty }
                                        };
                                }
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(invoiceSequenceSetupDTO.InvoiceSequenceSetupId), invoiceSequenceSetupDTO.Prefix + "(" + invoiceSequenceSetupDTO.SeriesStartNumber + "-" + invoiceSequenceSetupDTO.SeriesEndNumber + ")", dependentValues);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TAXSTRUCTURE")
                    {
                        LoadDefaultValue("<SELECT>");
                        TaxStructureList taxStructureList = new TaxStructureList(executionContext);
                        List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>> searchParameters = new List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<TaxStructureDTO> taxStructureDTOList = taxStructureList.GetTaxStructureList(searchParameters);
                        if (taxStructureDTOList != null && taxStructureDTOList.Any())
                        {
                            foreach (var taxStructure in taxStructureDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(taxStructure.TaxStructureId), taxStructure.StructureName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ALIGNMENT")
                    {
                        LoadDefaultValue("<SELECT>");

                        Dictionary<string, string> dictionary = new Dictionary<string, string>();
                        string value = string.Empty;

                        foreach (int enumValue in
                        Enum.GetValues(typeof(ReceiptPrintTemplateDTO.Alignment)))
                        {
                            value = Enum.GetName(typeof(ReceiptPrintTemplateDTO.Alignment), enumValue);
                            dictionary.Add(value.Substring(0, 1), value);
                        }
                        if (dictionary != null)
                        {
                            foreach (var alignment in dictionary)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(alignment.Key), alignment.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "INVENTORYLOCATION")
                    {
                        LoadDefaultValue("<SELECT>");
                        List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParam = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                        searchParam.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        LocationList locationListBL = new LocationList(executionContext);
                        List<LocationDTO> locationDTOList = locationListBL.GetAllLocations(searchParam);
                        if (locationDTOList != null && locationDTOList.Any())
                        {
                            foreach (var inventoryLocation in locationDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(inventoryLocation.LocationId), inventoryLocation.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRINTER")
                    {
                        LoadDefaultValue("<SELECT>");
                        PrinterListBL printerListBL = new PrinterListBL(executionContext);
                        List<KeyValuePair<PrinterDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<PrinterDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<PrinterDTO.SearchByParameters, string>(PrinterDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<PrinterDTO> printerDTOs = printerListBL.GetPrinterDTOList(searchParam);
                        if (printerDTOs != null && printerDTOs.Any())
                        {
                            foreach (var printer in printerDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(printer.PrinterId), printer.PrinterName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRINTTEMPLATE")
                    {
                        LoadDefaultValue("<SELECT>");
                        ReceiptPrintTemplateHeaderListBL receiptPrintTemplateHeaderListBL = new ReceiptPrintTemplateHeaderListBL(executionContext);
                        List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>(ReceiptPrintTemplateHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOs = receiptPrintTemplateHeaderListBL.GetReceiptPrintTemplateHeaderDTOList(searchParam, false);
                        if (receiptPrintTemplateHeaderDTOs != null && receiptPrintTemplateHeaderDTOs.Any())
                        {
                            foreach (var printerTemplate in receiptPrintTemplateHeaderDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(printerTemplate.TemplateId), printerTemplate.TemplateName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ORDERTYPEGROUP")
                    {
                        LoadDefaultValue("<SELECT>");
                        OrderTypeGroupListBL orderTypeGroupList = new OrderTypeGroupListBL(executionContext);
                        List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                        List<OrderTypeGroupDTO> orderTypeDTOs = orderTypeGroupList.GetOrderTypeGroupDTOList(searchParameters);
                        if (orderTypeDTOs != null && orderTypeDTOs.Any())
                        {
                            foreach (var orderTypeGroup in orderTypeDTOs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(orderTypeGroup.Id), orderTypeGroup.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "SECTION")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> dictionary = new Dictionary<string, string>();
                        string value = string.Empty;
                        foreach (int enumValue in
                        Enum.GetValues(typeof(ReceiptPrintTemplateDTO.Selection)))
                        {
                            value = Enum.GetName(typeof(ReceiptPrintTemplateDTO.Selection), enumValue);
                            dictionary.Add(value, value);
                        }
                        if (dictionary != null && dictionary.Any())
                        {
                            foreach (var section in dictionary)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(section.Key), section.Value.ToString());
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "STATUS")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "ACTIVE", "Active" },
                            { "INACTIVE", "Inactive" },
                            { "LOCKED", "Locked Out" },
                            { "DISABLED", "Disabled" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var status in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(status.Key, status.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATA_ACCESS_RULE")
                    {
                        LoadDefaultValue("<SELECT>");
                        DataAccessRuleList dataAccessRuleList = new DataAccessRuleList(executionContext);
                        List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> SearchParameters = new List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>>();
                        SearchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<DataAccessRuleDTO> dataAccessRuleDTOList = dataAccessRuleList.GetAllDataAccessRule(SearchParameters);
                        if (dataAccessRuleDTOList != null && dataAccessRuleDTOList.Any())
                        {
                            foreach (var dataAccessRule in dataAccessRuleDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(dataAccessRule.DataAccessRuleId), dataAccessRule.Name);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "SECURITY")
                    {
                        LoadDefaultValue("<SELECT>");
                        SecurityPolicyList securityPolicyList = new SecurityPolicyList(executionContext);
                        List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.SITEID, executionContext.GetSiteId().ToString()));
                        List<SecurityPolicyDTO> securityPolicyDTOList = securityPolicyList.GetAllSecurityPolicy(searchParam);
                        if (securityPolicyDTOList != null && securityPolicyDTOList.Any())
                        {
                            foreach (var security in securityPolicyDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(security.PolicyId), security.PolicyName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "BACKSIDETEMPLATE")
                    {
                        LoadDefaultValue("<SELECT>");
                        ReceiptPrintTemplateHeaderListBL receiptPrintTemplateHeaderListBL = new ReceiptPrintTemplateHeaderListBL(executionContext);
                        List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOList = receiptPrintTemplateHeaderListBL.PopulateTemplate();
                        if (receiptPrintTemplateHeaderDTOList != null && receiptPrintTemplateHeaderDTOList.Any())
                        {
                            foreach (ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO in receiptPrintTemplateHeaderDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(receiptPrintTemplateHeaderDTO.TemplateId), receiptPrintTemplateHeaderDTO.TemplateName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ACCESS_LEVEL")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "S", "Site" },
                            { "R", "Role" },
                            { "U", "User" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var level in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(level.Key, level.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "DATA_ACCESS_ENTITY")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("DATA_ACCESS_ENTITY");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATA_ACCESS_LEVEL")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("DATA_ACCESS_LEVEL");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATA_ACCESS_LIMIT")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("DATA_ACCESS_LIMIT");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ACCESS_LEVEL")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("DATA_ACCESS_LEVEL");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "INVENTORYADJUSTMENTSUI" || dropdownName.ToUpper().ToString() == "PHYSICALCOUNTUI1"
                        || dropdownName.ToUpper().ToString() == "PHYSICALCOUNTUI2" || dropdownName.ToUpper().ToString() == "INVENTORYLOTUI1" || dropdownName.ToUpper().ToString() == "INVENTORYLOTUI1")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, dropdownName.ToString()));
                        List<LookupValuesDTO> lookupValuesDataAccessLevelDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

                        if (lookupValuesDataAccessLevelDTOList != null && lookupValuesDataAccessLevelDTOList.Any())
                        {
                            foreach (var accessLevel in lookupValuesDataAccessLevelDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(accessLevel.LookupValueId), accessLevel.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATA_EXCLUSION_ENTITY")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("DATA_EXCLUSION_ENTITY");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRINTER_TYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("PRINTER_TYPE");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FISCALIZATION_TYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> fiscalizationTypeLookups = new Dictionary<string, string>
                            {
                                { Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.CHILE), Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.CHILE) },
                                { Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.ECUADOR), Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.ECUADOR) },
                                { Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.PERU), Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.PERU)},
                                { Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.VIETNAM), Convert.ToString(Semnox.Parafait.Fiscalization.ParafaitFiscalizationNames.VIETNAM)}
                            };
                        if (fiscalizationTypeLookups.Count != 0)
                        {
                            foreach (var fiscalizationType in fiscalizationTypeLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(fiscalizationType.Key, fiscalizationType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "OVERRIDE_OPTION_ITEM")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> overrideOptionItemLookups = new Dictionary<string, string>
                            {
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.NONE), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.NONE) },
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.RECEIPT), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.RECEIPT) },
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.SEQUENCE), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.SEQUENCE)},
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.REVERSALSEQUENCE), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.REVERSALSEQUENCE)},
                                { Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.SHOWERROR), Convert.ToString(Semnox.Parafait.POS.POSPrinterOverrideOptionItemCode.SHOWERROR)}
                            };
                        if (overrideOptionItemLookups.Count != 0)
                        {
                            foreach (var overrideOptionItem in overrideOptionItemLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(overrideOptionItem.Key, overrideOptionItem.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DEVICETYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> deviceTypeLookups = new Dictionary<string, string>
                        {
                            { "CardReader", "CardReader" },
                            { "BarcodeReader", "BarcodeReader" },
                            { "Waiver", "Waiver" }
                        };
                        if (deviceTypeLookups.Count != 0)
                        {
                            foreach (var deviceType in deviceTypeLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(deviceType.Key, deviceType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DEVICESUBTYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> deviceSubTypeLookups = new Dictionary<string, string>
                        {
                            { "KeyboardWedge", "KeyboardWedge" },
                            { "ACR1252U", "ACR1252U" },
                            { "ACR122U", "ACR122U" },
                            { "ACR1222L", "ACR1222L" },
                            { "Wacom", "Wacom" }
                        };
                        if (deviceSubTypeLookups.Count != 0)
                        {
                            foreach (var deviceSubType in deviceSubTypeLookups)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(deviceSubType.Key, deviceSubType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "OPTIONNAME")
                    {
                        LoadDefaultValue("<SELECT>");
                        ParafaitDefaultsListBL optionValueList = new ParafaitDefaultsListBL(executionContext);
                        List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, executionContext.GetSiteId().ToString()));
                        List<ParafaitDefaultsDTO> parafaitDefaultObj = optionValueList.GetParafaitDefaultsDTOList(searchParameters);
                        if (parafaitDefaultObj != null && parafaitDefaultObj.Any())
                        {
                            foreach (var option in parafaitDefaultObj)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(option.DefaultValueId), option.DefaultValueName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }

                    else if (dropdownName.ToUpper().ToString() == "PAYMENT_GATEWAY")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("PAYMENT_GATEWAY");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            lookupValuesDTOList = lookupValuesDTOList.OrderBy(x => x.LookupValue).ToList();
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if(dropdownName.ToUpper().ToString() == "ATTRIBUTENAME")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<int, Dictionary<string, string>> lookupsDict = new Dictionary<int, Dictionary<string, string>>();
                        List<string> stringList = new List<string>();
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("ALLOWED_COLUMNS_FOR_ATTRIBUTE_SETUP");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            lookupValuesDTOList = lookupValuesDTOList.OrderBy(x => x.LookupValue).ToList();
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                if (lookupsDict.ContainsKey(lookupValuesDTO.LookupValueId) == false)
                                {
                                    lookupsDict.Add(lookupValuesDTO.LookupValueId, new Dictionary<string, string>());
                                    stringList = lookupValuesDTO.Description.Split('|').ToList();
                                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                                    for (int i = 0; i < stringList.Count; i++)
                                    {
                                        keyValuePairs.Add(i.ToString(), stringList[i]);
                                    }
                                    lookupsDict[lookupValuesDTO.LookupValueId]=keyValuePairs;
                                }
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(lookupValuesDTO.LookupValueId.ToString(), lookupValuesDTO.LookupValue, lookupsDict[lookupValuesDTO.LookupValueId]);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DELIVERY_TYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("ONLINE_FOOD_DELIVERY_TYPE");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            lookupValuesDTOList = lookupValuesDTOList.OrderBy(x => x.LookupValue).ToList();
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DATE")
                    {
                        LoadDefaultValue("None");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("DATE_FORMAT");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "NUMBER")
                    {
                        LoadDefaultValue("None");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("NUMBER_FORMAT");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "ENCODEOPTIONS")
                    {
                        // LoadDefaultValue("None");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("BARCODE_ENCODE_TYPE");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "TEXT", "TEXT"},
                            { "NUMBER", "NUMBER"},
                            { "DATE", "DATE" },
                            { "LIST", "LIST" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var type in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(type.Key, type.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "APPLICABILITY")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "CUSTOMER", "CUSTOMER"},
                            { "PRODUCT", "PRODUCT"},
                            { "MACHINE", "MACHINE" },
                            { "GAMES", "GAMES" },
                            { "GAME_PROFILE", "GAME_PROFILE"},
                            { "CARDGAMES", "CARDGAMES" },
                            { "INVPRODUCT", "INVPRODUCT" },
                            { "LOCATION", "LOCATION" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var applicability in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(applicability.Key, applicability.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "NOTECOINFLAG")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "C", "Coin" },
                            { "N", "Note" },
                            { "T", "Token" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var flag in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(flag.Key, flag.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DEFAULT_RIDER")
                    {
                        LoadDefaultValue("<SELECT>");
                        CommonLookupDTO lookupDataObject = null;
                        List<string> stringList = new List<string>();
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("RIDER_ASSIGNMENT_ROLES");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            LookupValuesDTO lookupValuesDTO = lookupValuesDTOList.Where(x => x.LookupValue == "RIDER_ASSIGNMENT_ROLE_NAME").FirstOrDefault();
                            stringList = lookupValuesDTO.Description.Split('|').ToList();
                            List<UserRoleContainerDTO> userRoleContainerDTOs = UserRoleContainerList.GetUserRoleContainerDTOList(executionContext.GetSiteId());
                            List<UserRoleContainerDTO> defaultUserRoles = userRoleContainerDTOs.Where(x => stringList.Any(y => y == x.Role)).ToList();
                            if (defaultUserRoles != null && defaultUserRoles.Any())
                            {
                                List<int> roleIds = defaultUserRoles.Select(x => x.RoleId).ToList();
                                List<UserContainerDTO> userContainerDTOList = UserContainerList.GetUserContainerDTOCollection(executionContext.GetSiteId()).UserContainerDTOList;
                                userContainerDTOList = userContainerDTOList.Where(x => roleIds.Any(y => y == x.RoleId)).ToList();
                                if (userContainerDTOList != null && userContainerDTOList.Any())
                                {
                                    foreach (UserContainerDTO userContainerDTO in userContainerDTOList)
                                    {
                                        lookupDataObject = new CommonLookupDTO(userContainerDTO.UserId.ToString(), userContainerDTO.UserName);
                                        lookupDTO.Items.Add(lookupDataObject);
                                    }
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRICELIST")
                    {
                        PriceListList priceListList = new PriceListList(executionContext);
                        List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> SearchParameters = new List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>>();
                        SearchParameters.Add(new KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>(PriceListDTO.SearchByPriceListParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<PriceListDTO> UserRoleDisplpriceListDto = priceListList.GetAllPriceList(SearchParameters);

                        if (UserRoleDisplpriceListDto != null && UserRoleDisplpriceListDto.Any())
                        {
                            foreach (var priceList in UserRoleDisplpriceListDto)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(priceList.PriceListId), priceList.PriceListName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                            LoadDefaultValue("None");
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "MAINSECTION")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "@SiteName", "@SiteName"},
                            { "@SiteLogo","@SiteLogo"},
                            { "@Date","@Date"},
                            { "@SystemDate","@SystemDate" },
                            { "@TrxId","@TrxId" },
                            { "@TrxNo","@TrxNo" },
                            { "@TrxOTP","@TrxOTP" },
                            { "@Cashier","@Cashier" },
                            { "@Token","@Token" },
                            { "@POS","@POS" },
                            { "@TaxNo", "@TaxNo" },
                            { "@CustomerName","@CustomerName" },
                            { "@Phone","@Phone" },
                            { "@CustomerPhoto", "@CustomerPhoto" },
                            { "@CardBalance","@CardBalance" },
                            { "@CreditBalance","@CreditBalance" },
                            { "@BonusBalance","@BonusBalance" },
                            { "@PrimaryCardNumber" ,"@PrimaryCardNumber" },
                            { "@Remarks" ,"@Remarks" },
                            { "@SiteAddress" ,"@SiteAddress" },
                            { "@ScreenNumber" ,"@ScreenNumber" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var section in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(section.Key, section.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PRODUCT")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "@Product", "@Product"},
                            { "@Price","@Price"},
                            { "@Quantity","@Quantity"},
                            { "@Amount","@Amount" },
                            { "@TaxName","@TaxName" },
                            { "@Tax","@Tax" },
                            { "@TaxName1","@TaxName1" },
                            { "@TaxName2","@TaxName2" },
                            { "@TaxName3","@TaxName3" },
                            { "@TaxPercentage1","@TaxPercentage1" },
                            { "@TaxPercentage2", "@TaxPercentage2" },
                            { "@TaxPercentage3","@TaxPercentage3" },
                            { "@TaxAmount1","@TaxAmount1" },
                            { "@TaxAmount2", "@TaxAmount2" },
                            { "@TaxAmount3","@TaxAmount3" },
                            { "@Time","@Time" },
                            { "@FromTime","@FromTime" },
                            { "@ToTime" ,"@ToTime" },
                            { "@Seat" ,"@Seat" },
                            { "@LineRemarks" ,"@LineRemarks" },
                            { "@TicketBarCodeNo" ,"@TicketBarCodeNo" },
                            { "@TicketBarCode" ,"@TicketBarCode" },
                            { "@Tickets" ,"@Tickets" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var product in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(product.Key, product.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TOTALS")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "@Total", "@Total"},
                            { "@TaxTotal","@TaxTotal"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var totals in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(totals.Key, totals.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "CARDINFO")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "@CardNumber", "@CardNumber"},
                            { "@BarCodeCardNumber", "@BarCodeCardNumber"},
                            { "@QRCodeCardNumber", "@QRCodeCardNumber"},
                            { "@CardTickets","@CardTickets"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var cardInfo in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(cardInfo.Key, cardInfo.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DISCOUNT")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "@CouponNumber", "@CouponNumber"},
                            { "@DiscountName", "@DiscountName"},
                            { "@DiscountPercentage", "@DiscountPercentage"},
                            { "@DiscountAmount","@DiscountAmount"},
                            { "@CouponEffectiveDate","@CouponEffectiveDate"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var discount in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(discount.Key, discount.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "COUPONDETAILSEXTRA")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "@CouponExpiryDate", "@CouponExpiryDate"},
                            { "@BarCodeCouponNumber", "@BarCodeCouponNumber"},
                            { "@QRCodeCouponNumber", "@QRCodeCouponNumber"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var discount in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(discount.Key, discount.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TABLETYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "Standard", "Standard"},
                            { "Pool / Snooker", "Pool / Snooker"},
                            { "Karaoke", "Karaoke" }
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var tableType in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(tableType.Key, tableType.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "HQOPTIONS")
                    {
                        SiteList siteList = new SiteList(executionContext);
                        List<SiteDTO> siteDTOList = siteList.GetAllSites(null);
                        ProductTypeListBL productTypeListBL = new ProductTypeListBL(executionContext);
                        List<ProductTypeDTO> productTypeDTOList = productTypeListBL.GetProductTypeDTOList(null);

                        if (siteDTOList != null && siteDTOList.Any())
                        {
                            foreach (var siteDto in siteDTOList)
                            {
                                ProductTypeDTO productTypeDTO = null;
                                if (productTypeDTOList != null && productTypeDTOList.Any())
                                {
                                    productTypeDTO = productTypeDTOList.Find(m => m.SiteId == siteDto.SiteId);
                                }
                                SiteDTO siteDTO = siteDTOList.Find(m => m.SiteId == siteDto.SiteId && string.IsNullOrEmpty(siteDto.SiteGuid));
                                CommonLookupDTO lookupDataObject;
                                if (productTypeDTO != null || siteDTO != null)
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(siteDto.SiteId), "N");
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                                else
                                {
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(siteDto.SiteId), "Y");
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "RECEIPTTEMPLATEHELP")
                    {
                        CommonLookupDTO lookupDataObject;
                        lookupDataObject = new CommonLookupDTO("HEADERS", "HEADER, FOOTER: @SiteName, @SiteLogo, @SiteAddress, @CreditNote, @Date, @SystemDate, @TrxId, @TrxNo, @Cashier, @Token, @POS, @Printer, @TIN, " +
                               "@CustomerName, @Address, @City, @State, @Pin, @Phone, " +
                               "@CardBalance, @CreditBalance, @BonusBalance, @BarCodeTrxId, @CardNumber, @TableNumber, @Waiter, " +
                               "@CashAmount, @GameCardAmount, @CreditCardAmount, @NameOnCreditCard, @CreditCardName, @CreditCardReceipt, @OriginalTrxNo, @InvoicePrefix, @OtherPaymentMode, " +
                               "@OtherModeAmount, @OtherCurrencyCode, @OtherCurrencyRate, @AmountInOtherCurrency, @RoundOffAmount, @CreditCardNumber, @TenderedAmount, @ChangeAmount, @Tickets, @LoyaltyPoints, " +
                               "@TotalTipAmount,@ExpiringCPCredits, @ExpiringCPBonus, @ExpiringCPLoyalty, @ExpiringCPTickets, " +
                               "@CPCrediHEADERtsExpiryDate, @CPBonusExpiryDate, @CPLoyaltyExpiryDate, @CPTicketsExpiryDate, " +
                               "@TrxProfile, @Remarks, @ResolutionNumber, @ResolutionDate, @ResolutionInitialRange, @ResolutionFinalRange," +
                               "@Prefix, @SystemResolutionAuthorization, @InvoiceNumber, @OriginalTrxNetAmount, @Note, @ScreenNumber, @OriginalSIText, @OriginalORText, @SCPwdDetails, @OriginalARText, " +
                               "@CreditPlusCredits, @CreditPlusBonus, @TotalCreditPlusLoyaltyPoints, @CreditPlusTime, @CreditPlusTickets, " +
                               "@CreditPlusCardBalance, @TimeBalance, @RedeemableCreditPlusLoyaltyPoints, @SuggestiveTipText, " +
                               "@SuggestiveTipValues,@FiscalizationReference, @POSFriendlyName, @VariableRefundText, @LockerName,@PaymentLinkQRCode, @PaymentLinkRemarks, " +
                               "@QRCodePeruInvoice, @TaxCode, @UniqueId, @VirtualQueueURL, @QRCodeVirtualQueueURL, @VirtualQueueText, @AdvancePaidText, @AdvancePaidAmount,@OrderDispensingExtSystemRef," +
                               "@BarCodeOrderDispensingExtSystemRef,@QRCodeOrderDispensingExtSystemRef,@DeliveryChannelCustomerRef,@BarCodeDeliveryChannelCustomerRef, " +
                               "@QRCodeDeliveryChannelCustomerRef, @BalanceDueAmount, @OriginalTrxIdLabel, @OriginalTrxIdValue, @OriginalTrxNoLabel, @OriginalTrxNoValue, " +
                               "@OriginalTrxDateLabel, @OriginalTrxDateValue, @TrxReversalRemarksLabel, @TrxReversalRemarksValue" +
                               "@VietnamTaxInvoiceTrxId, @VietnamTaxInvoiceReservationCode, @GSTInvoiceQRCode");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("PRODUCT", "PRODUCT: @Product, @Price, @Quantity, @PreTaxAmount, @TaxName, @Tax, @Amount, @LineRemarks, @HSNCode, @Tickets, @GraceTickets, @AmountExclTax,@DiscountedAmount,@ApprovedBy");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("PRODUCTSUMMARY", "PRODUCTSUMMARY: @Product, @Price, @Quantity, @Amount, @PreTaxAmount, @TaxName, @Tax, @LineRemarks, @HSNCode, @AmountExclTax,@DiscountedAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("TRANSACTIONTOTAL", "TRANSACTIONTOTAL: @RentalAmount, @RentalDeposit, @Total, @PreTaxTotal, @GiftTotal, @TicketsTotal, @DiscountedPreTaxAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("DISCOUNT", "DISCOUNT: @DiscountName, @DiscountPercentage, @DiscountAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("DISCOUNTTOTAL", "DISCOUNTTOTAL: @DiscountTotal, @DiscountAmountExclTax, @DiscountedTotal, @DiscountRemarks");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("TAXABLECHARGES / NONTAXABLECHARGES", "TAXABLECHARGES / NONTAXABLECHARGES: @ChargeName, @ChargeAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("TAXLINE", "TAXLINE: @TaxName, @TaxPercentage, @TaxAmount, @TaxableLineAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("TAXTOTAL", "TAXTOTAL: @TaxableTotal, @NonTaxableTotal, @Tax, @TaxExempt, @ZeroRatedTaxable");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("GRANDTOTAL", "GRANDTOTAL: @GrandTotal, @RoundedOffGrandTotal");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("ITEMSLIP", "ITEMSLIP: @TrxId, @TrxNo, @TrxOTP, @Token, @Product, @Quantity, @Price, @Tax, @Amount, @LineRemarks");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("CARDINFO", "CARDINFO: @CardNumber, @CustomerName, @FaceValue, @Credits, @Bonus, @Time, @TotalCardValue, @Tax, @Amount, @BarCodeCardNumber, @LineRemarks, @QRCodeCardNumber, @CustomerUniqueId, @CustomerTaxCode, @CardBalanceTickets, @RedemptionCurrencyName, @RedemptionCurrencyValue, @RedemptionCurrencyQuantity, @RedeemedTickets");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("REDEMPTION_SOURCE", "REDEMPTION_SOURCE: @RedemptionReceiptNo, @RedemptionCardNo, @RedemptionCurrencyNo, @RedemptionManualTickets, @RedemptionGraceTickets, @TurnInTickets, @TicketQuantity, @TicketValue, @TotalTickets");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("REDEMPTION_SOURCE_TOTAL", "REDEMPTION_SOURCE_TOTAL: @TotalTickets");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("REDEMPTION_BALANCE", "REDEMPTION_BALANCE: @RedemptionReceiptNo, @RedemptionCardNo, @TicketValue");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("REDEEMED_GIFTS", "REDEEMED_GIFTS: @Product, @Price, @Quantity, @PreTaxAmount, @TaxName, @Tax, @Amount, @LineRemarks, @HSNCode, @Tickets, @GraceTickets, @AmountExclTax");
                        lookupDTO.Items.Add(lookupDataObject);
                    }
                    else if (dropdownName.ToUpper().ToString() == "EMAILTEMPLATEHELP")
                    {
                        CommonLookupDTO lookupDataObject;
                        lookupDataObject = new CommonLookupDTO("HEADER", "HEADER, FOOTER: @siteName, @sitelogo, @fromDate, @customerName, @address, @phoneNumber, @emailAddress, @bookingName, @reservationCode, @bookingRemarks, @VirtualQueueURL, @QRCodeVirtualQueueURL, @VirtualQueueText, " +
                             "@OrderDispensingExtSystemRef,@BarCodeOrderDispensingExtSystemRef,@QRCodeOrderDispensingExtSystemRef,@DeliveryChannelCustomerRef,@BarCodeDeliveryChannelCustomerRef,@QRCodeDeliveryChannelCustomerRef");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("RESERVATION_DATE", "RESERVATION DATE: @fromDate");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("FROM_TIME", "FROM TIME: @fromTime");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("TO_TIME", "TO TIME: @toTime");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("GUEST_COUNT", "GUEST COUNT : @guestCount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("BOOKING_PRODUCT", "BOOKING PRODUCT: @bookingProduct");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("BOOKING_PRODUCT_CONTENTS", "BOOKING PRODUCT CONTENTS:  @ProductName");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("ADDITIONAL_PRODUCTS", "ADDITIONAL PRODUCTS: @additionalItems");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("ESTIMATEAMOUNT", "ESTIMATEAMOUNT: @estimateAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("TRANSACTIONTOTAL", "TRANSACTIONTOTAL: @transactionAmount , @transactionNetAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("DISCOUNT", "DISCOUNT: @discountAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("ADVANCE_PAID", "ADVANCE PAID: @advancePaid, @advancePaymentDate");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("BALANCE_DUE", "BALANCE DUE: @balanceDue");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("TAXAMOUNT", "TAXAMOUNT: @taxAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("BOOKING_REMARKS", "BOOKING REMARKS: @bookingRemarks");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("BOOKING_STATUS", "BOOKING STATUS: @bookingStatus");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("BOOKING_SITE_ADDRESS", "BOOKING SITE ADDRESS: @siteAddress");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("PASSWORD_RESET_TOKEN_LINK", "PASSWORD RESET TOKEN LINK: @passwordResetTokenLink");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("WAIVERS", "WAIVERS: @signedBy,@signedDate,@waiverCode,@channel,@signedWaiverDetails,@BarCodeForWaiverCode");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("SIGN_WAIVERS_EMAIL_URL", "SIGN WAIVERS EMAIL URL: @SignWaiverLink,@SignWaiverLinkQRCode");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("PAYMENT_PAGE_URL", "PAYMENT PAGE URL: @PaymentLink,@PaymentLinkQRCode,@PaymentLinkRemarks");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("MAINTENANCE", "MAINTENANCE : @ServiceRequests,@Status,@SiteName");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("SUBSCRIPTION_CUSTOMER_CARD", "SUBSCRIPTION_CUSTOMER_CARD: @CreditCardNumber,  @CardExpiry,@NumberOfActiveSubscriptions,@EarliestBillOnDate");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("SUBSCRIPTION", "SUBSCRIPTION : @SubscriptionName, @SubscriptionDescription,  @SubscriptionPrice,@SubscriptionCycle,@UnitOfSubscriptionCycle,@SubscriptionCycleValidity,@BillInAdvance,@SelectedPaymentCollectionMode "+
                                                                   "@AutoRenew,  @AutoRenewalMarkup,  @NoOfRenewalReminders, @ReminderFrequency,@FirstReminderBeforeXDays,          "+
                                                                   " @LastRenewalReminderSentOn,"+
                                                                   " @RenewalReminderCount,"+
                                                                   " @PaymentRetryLimitReminderSentOn,"+
                                                                   " @PaymentRetryLimitReminderCount, "+
                                                                   " @AllowPause, "+
                                                                   " @FirstBillFromDate, "+
                                                                   " @FirstBillToDate,   "+
                                                                   " @FirstBillOnDate,"+
                                                                   " @LastBillFromDate,"+
                                                                   " @LastBillToDate,"+
                                                                   " @LastBillOnDate, "+
                                                                   " @ContactInfo1,"+
                                                                   " @ContactInfo2,"+
                                                                   " @ContactType, "+
                                                                   " @CreditCardNumber,  "+
                                                                   " @CardExpiry, "+
                                                                   " @TransactionDate,   "+
                                                                   " @OriginalSystemReference,"+
                                                                   " @TransactionOTP,"+
                                                                   " @TransactionNumber");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("PRIMARY_CARD", "PRIMARY CARD: @PrimaryCardNumber, @CardBalance, @CreditBalance, @BonusBalance, @CardBalanceTickets");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("PAYMENT_OTP_VALIDATION", "PAYMENT OTP VALIDATION: @PrimaryCardNumber, @CardBalance, @CreditBalance, @BonusBalance, @CardBalanceTickets");
                        lookupDTO.Items.Add(lookupDataObject);
                        lookupDataObject = new CommonLookupDTO("CHARGES", "CHARGES: @StartChargeLine, @EndChargeLine, @ChargeName, @ChargeAmount");
                        lookupDTO.Items.Add(lookupDataObject);
                    }
                    else if (dropdownName.ToUpper().ToString() == "CHECKBITALGORITHIM")
                    {
                        Dictionary<string, string> checkBitAlgorithm = new Dictionary<string, string>
                        {
                            { "NONE", "None"},
                            { "MODULO_TEN_WEIGHT_THREE","Modulo Ten Weight Three"},
                        };
                        if (checkBitAlgorithm.Count != 0)
                        {
                            foreach (var algorithim in checkBitAlgorithm)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(algorithim.Key, algorithim.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "TICKETSTATIONTYPE")
                    {
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
                        {
                            { "0", "POS"},
                            { "1","STATION"}
                        };
                        if (keyValuePairs.Count != 0)
                        {
                            foreach (var keyValue in keyValuePairs)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(keyValue.Key, keyValue.Value);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "WBPRINTERMODEL")
                    {
                        LoadDefaultValue("");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("RFID_WRISTBAND_MODELS");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PAPERSIZE")
                    {
                        LoadDefaultValue("");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("PRINTER_PAPER_SIZE");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.LookupValue);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "DELIVERY_CHANNELS")
                    {
                        //List<DeliveryChannelContainerDTO> deliveryChannelContainerDTOList = DeliveryChannelcon

                        DeliveryChannelListBL deliveryChannelListBL = new DeliveryChannelListBL(executionContext);
                        List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>>();
                        searchParams.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>(DeliveryChannelDTO.SearchByParameters.IS_ACTIVE, "1"));
                        searchParams.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>(DeliveryChannelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<DeliveryChannelDTO> deliveryChannelDTOList = deliveryChannelListBL.GetDeliveryChannels(searchParams);
                        if (deliveryChannelDTOList!= null && deliveryChannelDTOList.Any())
                        {
                            foreach (DeliveryChannelDTO deliveryChannelDTO in deliveryChannelDTOList)
                            {
                                CommonLookupDTO lookupDataObject = new CommonLookupDTO(deliveryChannelDTO.DeliveryChannelId.ToString(), deliveryChannelDTO.ChannelName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    //delivery_channels
                    else if (dropdownName.ToUpper().ToString() == "INTEGRATION_NAME")
                    {
                        LoadDefaultValue("<SELECT>");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        LoadLookupValues("ONLINE_ORDER_DELIVERY_INTEGRATIONS");
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(lookupValuesDTO.LookupValueId), lookupValuesDTO.Description);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "FOOD_TYPE" || dropdownName.ToUpper().ToString() == "GOODS_TYPE")
                    {
                        LoadDefaultValue("<SELECT>");
                        SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(executionContext);
                        List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefSearch = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                        segmentDefSearch.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "1"));
                        segmentDefSearch.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        segmentDefSearch.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, "POS PRODUCTS"));
                        List<SegmentDefinitionDTO> segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitionsDTOList(segmentDefSearch);
                        if (segmentDefinitionDTOList != null && segmentDefinitionDTOList.Any())
                        {
                            foreach (SegmentDefinitionDTO segmentDefinitionDTO in segmentDefinitionDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(segmentDefinitionDTO.SegmentDefinitionId), segmentDefinitionDTO.SegmentName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PAYMENT_MODE")
                    {
                        LoadDefaultValue("<SELECT>");
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> paymentModeSearch = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        paymentModeSearch.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISACTIVE, "1"));
                        paymentModeSearch.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        PaymentModeList paymentModeList = new PaymentModeList(executionContext);
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeList.GetAllPaymentModeList(paymentModeSearch);
                        if (paymentModeDTOList != null && paymentModeDTOList.Any())
                        {
                            foreach (PaymentModeDTO paymentModeDTO in paymentModeDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(paymentModeDTO.PaymentModeId), paymentModeDTO.PaymentMode);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "AGGREGATOR_DISCOUNT")
                    {
                        LoadDefaultValue("<SELECT>");
                        List<DiscountsDTO> discountsDTOList = null;
                        using(UnitOfWork unitOfWork = new UnitOfWork())
                        {
                            SearchParameterList<DiscountsDTO.SearchByParameters> discountsSearch = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                            discountsSearch.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                            discountsSearch.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, unitOfWork);
                            discountsDTOList = discountsListBL.GetDiscountsDTOList(discountsSearch);
                        }
                        
                        if (discountsDTOList != null && discountsDTOList.Any())
                        {
                            discountsDTOList = discountsDTOList.Where(d => string.IsNullOrWhiteSpace(d.VariableDiscounts) == false && d.VariableDiscounts == "Y").ToList();
                            if (discountsDTOList != null && discountsDTOList.Any())
                            {
                                foreach (DiscountsDTO discountsDTO in discountsDTOList)
                                {
                                    CommonLookupDTO lookupDataObject;
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(discountsDTO.DiscountId), discountsDTO.DiscountName);
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                    }
                    else if (dropdownName.ToUpper().ToString() == "PACKING_CHARGE_PRODUCT")
                    {
                        LoadDefaultValue("<SELECT>");
                        List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> productsSearch = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                        productsSearch.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "1"));
                        productsSearch.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                        productsSearch.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, "GENERICSALE"));
                        ProductsList productsListBL = new ProductsList(executionContext);
                        List<ProductsDTO> productsDTOList = productsListBL.GetProductsDTOList(productsSearch);
                        if (productsDTOList != null && productsDTOList.Any())
                        {
                            foreach (ProductsDTO productsDTO in productsDTOList)
                            {
                                CommonLookupDTO lookupDataObject;
                                lookupDataObject = new CommonLookupDTO(Convert.ToString(productsDTO.ProductId), productsDTO.ProductName);
                                lookupDTO.Items.Add(lookupDataObject);
                            }
                        }
                    }
                    completeTablesDataList.Add(lookupDTO);
                }

                log.LogMethodExit(completeTablesDataList);
                return completeTablesDataList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Loads the lookupValues
        /// </summary>
        /// <param name="lookupName"></param>
        private List<LookupValuesDTO> LoadLookupValues(string lookupName)
        {
            log.LogMethodEntry(lookupName);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
        }
        /// <summary>
        /// Loads default value to the lookupDto
        /// </summary>
        /// <param name="defaultValue">default value of Type string</param>
        private void LoadDefaultValue(string defaultValue)
        {
            List<KeyValuePair<string, string>> selectKey = new List<KeyValuePair<string, string>>();
            selectKey.Add(new KeyValuePair<string, string>("-1", defaultValue));
            foreach (var select in selectKey)
            {
                CommonLookupDTO lookupDataObject = new CommonLookupDTO(Convert.ToString(select.Key), select.Value);
                lookupDTO.Items.Add(lookupDataObject);
            }
        }
        public List<CommonLookupsDTO> GetLookupFilteredList()
        {
            try
            {
                log.LogMethodEntry();
                List<CommonLookupsDTO> completeTablesDataList = new List<CommonLookupsDTO>();
                string dropdownNames = string.Empty;
                string[] dropdowns = null;
                SiteSetupEntityNameLookup siteSetupEntityNameLookupValue = (SiteSetupEntityNameLookup)Enum.Parse(typeof(SiteSetupEntityNameLookup), entityName);
                switch (siteSetupEntityNameLookupValue)
                {
                    case SiteSetupEntityNameLookup.RECEIPTTEMPLATE:
                        dropdownNames = "AVAILABLEPRODUCT";
                        break;
                }
                dropdowns = dropdownNames.Split(',');
                foreach (string dropdownName in dropdowns)
                {
                    lookupDTO = new CommonLookupsDTO();
                    lookupDTO.Items = new List<CommonLookupDTO>();
                    lookupDTO.DropdownName = dropdownName;

                    if (dropdownName.ToUpper().ToString() == "AVAILABLEPRODUCT")
                    {
                        Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(keyValuePair);
                        string displayGroupFormatId = string.Empty;
                        foreach (var key in keyValuePairs)
                        {
                            if (key.Key == "DisplayGroupFormatId")
                            {
                                displayGroupFormatId = key.Value;
                            }
                        }

                        List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParameters = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID, displayGroupFormatId.ToString()));
                        ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext);
                        PrinterProductsListBL printerProductsListBL = new PrinterProductsListBL(executionContext);
                        List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList = productsDisplayGroupList.GetAllProductsDisplayGroup(searchParameters);
                        if(productsDisplayGroupDTOList == null)
                        {
                            productsDisplayGroupDTOList = new List<ProductsDisplayGroupDTO>();
                        }
                        foreach (ProductsDisplayGroupDTO productsDisplayGroup in productsDisplayGroupDTOList)
                        {
                            var productId = productsDisplayGroup.ProductId;
                            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchProducts = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                            searchProducts.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, productId.ToString()));

                            ProductsList productsList = new ProductsList(executionContext);
                            var Productvalue = productsList.GetProductsDTOList(searchProducts);

                            if (Productvalue != null)
                            {
                                foreach (var product in Productvalue)
                                {
                                    CommonLookupDTO lookupDataObject;
                                    lookupDataObject = new CommonLookupDTO(Convert.ToString(product.ProductId), product.ProductName);
                                    lookupDTO.Items.Add(lookupDataObject);
                                }
                            }
                        }
                    }
                    completeTablesDataList.Add(lookupDTO);
                }
                log.LogMethodExit(completeTablesDataList);
                return completeTablesDataList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}