/********************************************************************************************
 * Project Name - Site
 * Description  - Bussiness logic of Company
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        08-Mar-2019   Jagan Mohana          Created 
 *            29-Mar-2019   Mushahid Faizan       Added LogMethodEntry & LogMethodExit,Removed unused namespaces.
                                                  added CompanyList parameterized constructor having execution context
                                                  Modified GetAllCompanies(). & added SQLTransaction in SaveUpdateCompanyList().
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Site
{
    public class Company
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CompanyDTO companyDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public Company(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="companyDTO"></param>
        public Company(ExecutionContext executionContext, CompanyDTO companyDTO)
        {
            log.LogMethodEntry(executionContext, companyDTO);
            this.executionContext = executionContext;
            this.companyDTO = companyDTO;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Saves the Company
        /// Checks if the company id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CompanyDataHandler companyDataHandler = new CompanyDataHandler(sqlTransaction);
            if (companyDTO.CompanyId < 0)
            {
                int companyId = companyDataHandler.InsertCompany(companyDTO, executionContext.GetUserId());
                companyDTO.CompanyId = companyId;
            }
            else
            {
                if (companyDTO.IsChanged)
                {
                    companyDataHandler.UpdateCompany(companyDTO, executionContext.GetUserId());
                    companyDTO.AcceptChanges();
                }
            }
            if (companyDTO.OrganizationStructureDTOList != null && companyDTO.OrganizationStructureDTOList.Count != 0)
            {
                foreach (OrganizationStructureDTO organizationStructureDTO in companyDTO.OrganizationStructureDTOList)
                {
                    if (organizationStructureDTO.IsChanged)
                    {
                        organizationStructureDTO.CompanyId = companyDTO.CompanyId;
                        OrganizationStructure organizationStructure = new OrganizationStructure(executionContext, organizationStructureDTO);
                        organizationStructure.Save(sqlTransaction);
                    }
                }
            }
            if (companyDTO.OrganizationDTOList != null && companyDTO.OrganizationDTOList.Count != 0)
            {
                foreach (OrganizationDTO organizationDTO in companyDTO.OrganizationDTOList)
                {
                    if (organizationDTO.IsChanged)
                    {
                        organizationDTO.CompanyId = companyDTO.CompanyId;
                        Organization organization = new Organization(executionContext, organizationDTO);
                        organization.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get companyDTO Object
        /// </summary>
        public CompanyDTO GetCompanyDTO
        {
            get { return companyDTO; }
        }

    }
    /// <summary>
    /// Manages the list of Company
    /// </summary>
    public class CompanyList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CompanyDTO> companyDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CompanyList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.companyDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="companyDTOList"></param>
        /// <param name="executionContext"></param>
        public CompanyList(ExecutionContext executionContext, List<CompanyDTO> companyDTOList)
        {
            log.LogMethodEntry(executionContext, companyDTOList);
            this.executionContext = executionContext;
            this.companyDTOList = companyDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Company list with organizations and Organization Structure
        /// </summary>
        public List<CompanyDTO> GetAllCompanies(List<KeyValuePair<CompanyDTO.SearchByParameters, string>> searchParameters, bool isHqSite = false)
        {
            log.LogMethodEntry(searchParameters);
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            log.LogMethodEntry(searchParameters);
            List<CompanyDTO> companyDTOsList = companyDataHandler.GetCompany(searchParameters);
            if (companyDTOsList != null && companyDTOsList.Count != 0 && isHqSite)
            {
                OrganizationList organizationList = new OrganizationList();
                OrganizationStructureList organizationStructureList = new OrganizationStructureList();

                List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>> searchOrganizationParameters = new List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>>();                
                List<OrganizationDTO> organizationDTOsList = organizationList.GetAllOrganizations(searchOrganizationParameters, isHqSite);
                List<KeyValuePair<OrganizationStructureDTO.SearchByParameters, string>> searchOrganizationStructureParameters = new List<KeyValuePair<OrganizationStructureDTO.SearchByParameters, string>>();                
                List<OrganizationStructureDTO> organizationStructureDTOsList = organizationStructureList.GetAllOrganizationStructure(searchOrganizationStructureParameters);

                foreach (CompanyDTO companyDTO in companyDTOsList)
                {
                    if(organizationDTOsList != null && organizationDTOsList.Any())
                    {
                        companyDTO.OrganizationDTOList = organizationDTOsList.FindAll(m=>m.CompanyId == companyDTO.CompanyId).ToList();
                    }
                    if (organizationStructureDTOsList != null && organizationStructureDTOsList.Any())
                    {
                        companyDTO.OrganizationStructureDTOList = organizationStructureDTOsList.FindAll(m => m.CompanyId == companyDTO.CompanyId).ToList();
                    }
                }
            }
            log.LogMethodExit(companyDTOsList);
            return companyDTOsList;
        }
        /// <summary>
        /// This method should be used to Save and Update the Company details for Web Management Studio.
        /// </summary>
        public void SaveUpdateCompanyList()
        {
            log.LogMethodEntry();

            if (companyDTOList != null && companyDTOList.Any())
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (CompanyDTO companyDTO in companyDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Company company = new Company(executionContext, companyDTO);
                            company.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                log.LogMethodExit();
            }
        }
    }
}