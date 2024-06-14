/********************************************************************************************
 * Project Name - Department BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018      Indhu               Created 
 *2.70        19-09-2019       Indrajeet Kumar     Modified-Added Paramertized Contructor to DepartmentList
 *2.90        20-May-2020     Vikas Dwivedi        Modified as per the Standard CheckList
 *2.90        20-Aug-2020      Girish Kundar       Modified : Issue FIx handled SQL exceptions in Save and delete method
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business Logic for DepartmentBL
    /// </summary>
    public class DepartmentBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DepartmentDTO departmentDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parametrized Constructor of DepartmentBL 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private DepartmentBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the department id as the parameter
        /// Would fetch the department object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="departmentId">id of Department Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DepartmentBL(ExecutionContext executionContext, int departmentId, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, departmentId, sqlTransaction);
            DepartmentDataHandler departmentDataHandler = new DepartmentDataHandler(null);
            departmentDTO = departmentDataHandler.GetDepartment(departmentId);
            if (departmentDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "DepartmentDTO", departmentId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DepartmentBL object using the DepartmentDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="departmentDTO">departmentDTO</param>
        public DepartmentBL(ExecutionContext executionContext, DepartmentDTO departmentDTO)
            :this(executionContext)
        {
            log.LogMethodEntry();
            this.departmentDTO = departmentDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the DepartmentDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// Saves the department record
        /// Checks if the DepartmentId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            DepartmentDataHandler departmentDataHandler = new DepartmentDataHandler(sqlTransaction);
            if (departmentDTO.DepartmentId < 0)
            {
                departmentDTO = departmentDataHandler.InsertDepartment(departmentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                departmentDTO.AcceptChanges();
            }
            else
            {
                if (departmentDTO.IsChanged)
                {
                    departmentDTO = departmentDataHandler.UpdateDepartment(departmentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    departmentDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the DepartmentDTO based on DepartmentId
        /// </summary>
        /// <param name="SQLTrx"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                DepartmentDataHandler departmentDataHandler = new DepartmentDataHandler(sqlTransaction);
                departmentDataHandler.Delete(departmentDTO.DepartmentId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Gets the DepartmentDTO
        /// </summary>
        public DepartmentDTO GetDepartmentDTO { get { return departmentDTO; } }
    }

    /// <summary>
    /// Manages the list of department
    /// </summary>
    public class DepartmentList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<DepartmentDTO> departmentDTOList = new List<DepartmentDTO>();

        /// <summary>
        /// Paramterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public DepartmentList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="departmentDTOList">departmentDTOList</param>
        public DepartmentList(ExecutionContext executionContext, List<DepartmentDTO> departmentDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, departmentDTOList);
            this.departmentDTOList = departmentDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the department list
        /// </summary>
        public List<DepartmentDTO> GetDepartmentDTOList(List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            DepartmentDataHandler departmentDataHandler = new DepartmentDataHandler(sqlTransaction);
            List<DepartmentDTO> departmentDTOList = departmentDataHandler.GetDepartmentDTOList(searchParameters);
            log.LogMethodExit(departmentDTOList);
            return departmentDTOList;
        }

        /// <summary>
        /// This Method is used to Svae the departmentDTO.
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (departmentDTOList == null ||
               departmentDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < departmentDTOList.Count; i++)
            {
                var departmentDTO = departmentDTOList[i];
                if (departmentDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    DepartmentBL departmentBL = new DepartmentBL(executionContext, departmentDTO);
                    departmentBL.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving DepartmentDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("DepartmentDTOList", departmentDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete DeleteDepartmentList
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (departmentDTOList != null && departmentDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (DepartmentDTO departmentDTO in departmentDTOList)
                    {
                        if (departmentDTO.IsChanged)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                DepartmentBL departmentBL = new DepartmentBL(executionContext, departmentDTO);
                                departmentBL.Delete(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                log.LogMethodExit(null, "Throwing validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (SqlException ex)
                            {
                                log.Error(ex);
                                if (ex.Number == 2601)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                                }
                                else if (ex.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                                }
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
                }
                log.LogMethodExit();
            }
        }
    }
}

