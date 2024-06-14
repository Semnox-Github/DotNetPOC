/********************************************************************************************
 * Project Name - Device                                                                        
 * Description  -CashdrawerBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 *2.140.0     21-Mar-2022     Abhishek           Modified : Added HasActivePosCashdrawers() method to fetch active poscashdrawers
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    /// <summary>
    /// 
    /// </summary>
    public class CashdrawerBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CashdrawerDTO cashdrawerDTO;
        private ExecutionContext executionContext;
        public static Semnox.Parafait.Device.COMPort CashDrawerSerialPort;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private CashdrawerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// CashdrawerBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public CashdrawerBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadCashdrawerDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterCashdrawerDTO">parameterCashdrawerDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CashdrawerBL(ExecutionContext executionContext, CashdrawerDTO parameterCashdrawerDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterCashdrawerDTO, sqlTransaction);

            if (parameterCashdrawerDTO.CashdrawerId > -1)
            {
                LoadCashdrawerDTO(parameterCashdrawerDTO.CashdrawerId, sqlTransaction);
                ThrowIfUserDTOIsNull(parameterCashdrawerDTO.CashdrawerId);
                if (parameterCashdrawerDTO.IsActive == false)
                {
                    HasActivePosCashdrawers(parameterCashdrawerDTO.CashdrawerId, sqlTransaction);
                }
                Update(parameterCashdrawerDTO);
            }
            else
            {
                cashdrawerDTO = new CashdrawerDTO(-1, parameterCashdrawerDTO.CashdrawerName, parameterCashdrawerDTO.InterfaceType, parameterCashdrawerDTO.CommunicationString,
                                                               parameterCashdrawerDTO.SerialPort, parameterCashdrawerDTO.SerialPortBaud, false,
                                                               parameterCashdrawerDTO.IsActive);
                Validate(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="cashdrawerId">cashdrawerId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void HasActivePosCashdrawers(int cashdrawerId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(cashdrawerId, sqlTransaction);
            bool result = false;
            CashdrawerDataHandler cashdrawerDataHandler = new CashdrawerDataHandler(sqlTransaction);
            result = cashdrawerDataHandler.HasActivePosCashdrawers(cashdrawerId);
            if (result)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
            }
            log.LogMethodExit();
        }

        private void ThrowIfUserDTOIsNull(int id)
        {
            log.LogMethodEntry();
            if (cashdrawerDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Cashdrawers", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadCashdrawerDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            CashdrawerDataHandler cashdrawerDataHandler = new CashdrawerDataHandler(sqlTransaction);
            cashdrawerDTO = cashdrawerDataHandler.GetCashdrawerDTO(id);
            ThrowIfUserDTOIsNull(id);
            log.LogMethodExit();
        }

        private void Update(CashdrawerDTO parameterCashdrawerDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterCashdrawerDTO);
            cashdrawerDTO.CashdrawerId = parameterCashdrawerDTO.CashdrawerId;
            cashdrawerDTO.CashdrawerName = parameterCashdrawerDTO.CashdrawerName;
            cashdrawerDTO.InterfaceType = parameterCashdrawerDTO.InterfaceType;
            cashdrawerDTO.CommunicationString = parameterCashdrawerDTO.CommunicationString;
            cashdrawerDTO.SerialPort = parameterCashdrawerDTO.SerialPort;
            cashdrawerDTO.SerialPortBaud = parameterCashdrawerDTO.SerialPortBaud;
            cashdrawerDTO.CommunicationString = parameterCashdrawerDTO.CommunicationString;
            cashdrawerDTO.IsActive = parameterCashdrawerDTO.IsActive;
            //cashdrawerDTO.IsSystem = parameterCashdrawerDTO.IsSystem;
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CASHDRAWER_INTERFACE_MODE");
            log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);

            if (string.IsNullOrWhiteSpace(cashdrawerDTO.CashdrawerName))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4076)); 
            }
            string[] allowedInterfaceTypes = Enum.GetNames(typeof(CashdrawerIntefaceTypes));
            if (allowedInterfaceTypes.Contains(cashdrawerDTO.InterfaceType.ToString()) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4077));
            }
            if(cashdrawerDTO.InterfaceType.ToString() == CashdrawerIntefaceTypes.RECEIPTPRINTER.ToString())
            {
                if(cashdrawerDTO.SerialPort >0 || cashdrawerDTO.SerialPortBaud > 0)
                {
                    cashdrawerDTO.SerialPort = 0;
                    cashdrawerDTO.SerialPortBaud = 0;
                }
            }
            log.LogMethodExit();
        }

       
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SaveImpl(sqlTransaction);
            log.LogMethodExit();
        }
        private void SaveImpl(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            CashdrawerDataHandler cashdrawerDataHandler = new CashdrawerDataHandler(sqlTransaction);
            if (cashdrawerDTO.CashdrawerId < 0)
            {
                cashdrawerDTO = cashdrawerDataHandler.Insert(cashdrawerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cashdrawerDTO.AcceptChanges();
            }
            else
            {
                if (cashdrawerDTO.IsChanged)
                {
                    cashdrawerDTO = cashdrawerDataHandler.Update(cashdrawerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    cashdrawerDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        public void OpenCashDrawer()
        {
            log.LogMethodEntry();
            byte[] bCashDrawerPrintString;
            try
            {
                string[] strCASH_DRAWER_PRINT_STRING = cashdrawerDTO.CommunicationString.Split(',');
                bCashDrawerPrintString = new byte[strCASH_DRAWER_PRINT_STRING.Length];
                int i = 0;
                foreach (string str in strCASH_DRAWER_PRINT_STRING)
                    bCashDrawerPrintString[i++] = Convert.ToByte(Convert.ToInt32(str.Trim()));
            }
            catch (Exception ex)
            {
                log.Error("Unable to get the value of CASH_DRAWER_PRINT_STRING", ex);
                bCashDrawerPrintString = new byte[] { 27, 112, 0, 100, 250 };
            }
            if (cashdrawerDTO.InterfaceType == "SERIALPORT")
            {
                try
                {
                    CashDrawerSerialPort = new Semnox.Parafait.Device.COMPort(cashdrawerDTO.SerialPort, cashdrawerDTO.SerialPortBaud);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to get the value of CashDrawerSerialPort", ex);
                }
                if (CashDrawerSerialPort != null && CashDrawerSerialPort.comPort.IsOpen)
                {
                    CashDrawerSerialPort.comPort.Write(bCashDrawerPrintString, 0, bCashDrawerPrintString.Length);
                }
            }
            else
            {
                IntPtr pBytes = Marshal.AllocHGlobal(Marshal.SizeOf(bCashDrawerPrintString[0]) * bCashDrawerPrintString.Length);
                try
                {
                    Marshal.Copy(bCashDrawerPrintString, 0, pBytes, bCashDrawerPrintString.Length);
                    RawPrinterHelper.SendBytesToPrinter(new PrinterSettings().PrinterName, pBytes, bCashDrawerPrintString.Length);
                }
                finally
                {
                    // Free the unmanaged memory.
                    Marshal.FreeHGlobal(pBytes);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// get CashdrawerDTO Object
        /// </summary>
        public CashdrawerDTO CashdrawerDTO
        {
            get
            {
                CashdrawerDTO result = new CashdrawerDTO(cashdrawerDTO);
                return result;
            }
        }

        public bool IsSystemCashdrawer()
        {
            log.LogMethodEntry();
            if(cashdrawerDTO.IsSystem)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
    }

    /// <summary>
    /// OrderDetailListBL list class for order details
    /// </summary>
    public class CashdrawerListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CashdrawerDTO> cashdrawerDTOList;

        /// <summary>
        /// default constructor
        /// </summary>
        public CashdrawerListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CashdrawerListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cashdrawerDTOList"></param>
        public CashdrawerListBL(ExecutionContext executionContext, List<CashdrawerDTO> cashdrawerDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, cashdrawerDTOList);
            this.cashdrawerDTOList = cashdrawerDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetCashdrawers
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<CashdrawerDTO> GetCashdrawers(List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CashdrawerDataHandler cashdrawerDataHandler = new CashdrawerDataHandler(sqlTransaction);
            List<CashdrawerDTO> cashdrawerDTOList = cashdrawerDataHandler.GetCashdrawers(searchParameters);
            log.LogMethodExit(cashdrawerDTOList);
            return cashdrawerDTOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CashdrawerDTO> Save(SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<CashdrawerDTO> savedCashdrawerDTOList = new List<CashdrawerDTO>();
            try
            {
                if (cashdrawerDTOList != null && cashdrawerDTOList.Any())
                {
                    foreach (CashdrawerDTO cashdrawerDTO in cashdrawerDTOList)
                    {
                        CashdrawerBL cashdrawerBL = new CashdrawerBL(executionContext, cashdrawerDTO);
                        cashdrawerBL.Save(sqlTransaction);
                        savedCashdrawerDTOList.Add(cashdrawerBL.CashdrawerDTO);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                if (sqlEx.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else
                {
                    throw;
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit("Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit("Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit(savedCashdrawerDTOList);
            return savedCashdrawerDTOList;
        }

        internal DateTime? GetCashdrawerLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CashdrawerDataHandler cashdrawerDataHandler = new CashdrawerDataHandler(sqlTransaction);
            DateTime? result = cashdrawerDataHandler.GetCashdrawerLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
       
    }
}
