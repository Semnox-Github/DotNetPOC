/*/********************************************************************************************
 * Project Name - LegacyCardCreditPlusBL
 * Description  - BL for Legacy Card Credit Plus
 *
 **************
 ** Version Log
 **************
 * Version     Date Modified     By          Remarks
 *********************************************************************************************
 *2.100.0     03-Sep-2020        Dakshakh    Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Parafait_POS
{
    /// <summary>
    /// Business logic for LegacyCardCreditPlus class.
    /// </summary>
    public class LegacyCardCreditPlusBL
    {
        private LegacyCardCreditPlusDTO legacyCardCreditPlusDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of LegacyCardCreditPlusBL class
        /// </summary>
        private LegacyCardCreditPlusBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the LegacyCardCreditPlus id as the parameter
        /// Would fetch the LegacyCardCreditPlus object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public LegacyCardCreditPlusBL(ExecutionContext executionContext, int id,
            bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            LegacyCardCreditPlusDataHandler LegacyCardCreditPlusDataHandler = new LegacyCardCreditPlusDataHandler(sqlTransaction);
            legacyCardCreditPlusDTO = LegacyCardCreditPlusDataHandler.GetLegacyCardCreditPlusDTO(id);
            if (legacyCardCreditPlusDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "LegacyCardCreditPlus", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (LegacyCardCreditPlusDTO != null && loadChildRecords)
            {
                LegacyCardCreditPlusBuilderBL LegacyCardCreditPlusBuilderBL = new LegacyCardCreditPlusBuilderBL(executionContext);
                LegacyCardCreditPlusBuilderBL.Build(LegacyCardCreditPlusDTO, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LegacyCardCreditPlusBL object using the LegacyCardCreditPlusDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="LegacyCardCreditPlusDTO">LegacyCardCreditPlusDTO object</param>
        public LegacyCardCreditPlusBL(ExecutionContext executionContext, LegacyCardCreditPlusDTO LegacyCardCreditPlusDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, LegacyCardCreditPlusDTO);
            this.legacyCardCreditPlusDTO = LegacyCardCreditPlusDTO;
            log.LogMethodExit();
        }

        //// <summary>
        /// Saves the LegacyCardCreditPlus
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (legacyCardCreditPlusDTO.IsChangedRecursive || legacyCardCreditPlusDTO.LegacyCardCreditPlusId == -1)
            {
                LegacyCardCreditPlusDataHandler LegacyCardCreditPlusDataHandler = new LegacyCardCreditPlusDataHandler(sqlTransaction);
                if (legacyCardCreditPlusDTO.IsActive)
                {
                    if (legacyCardCreditPlusDTO.IsChanged)
                    {
                        if (legacyCardCreditPlusDTO.LegacyCardCreditPlusId < 0)
                        {
                            legacyCardCreditPlusDTO = LegacyCardCreditPlusDataHandler.InsertLegacyCardCreditPlus(legacyCardCreditPlusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                            legacyCardCreditPlusDTO.AcceptChanges();
                        }
                        else
                        {
                            if (LegacyCardCreditPlusDTO.IsChanged)
                            {
                                legacyCardCreditPlusDTO = LegacyCardCreditPlusDataHandler.Update(LegacyCardCreditPlusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                legacyCardCreditPlusDTO.AcceptChanges();
                            }
                        }
                    }

                    if (LegacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList != null)
                    {
                        foreach (var legacyCardCreditPlusConsumptionDTO in LegacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList)
                        {
                            if (legacyCardCreditPlusConsumptionDTO.IsChanged || legacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusConsumptionId == -1)
                            {
                                if (legacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusId != LegacyCardCreditPlusDTO.LegacyCardCreditPlusId)
                                {
                                    legacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusId = LegacyCardCreditPlusDTO.LegacyCardCreditPlusId;
                                }
                                LegacyCardCreditPlusConsumptionBL LegacyCardCreditPlusConsumptionBL = new LegacyCardCreditPlusConsumptionBL(executionContext, legacyCardCreditPlusConsumptionDTO);
                                LegacyCardCreditPlusConsumptionBL.Save(sqlTransaction);
                            }
                        }
                    }

                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LegacyCardCreditPlusDTO LegacyCardCreditPlusDTO
        {
            get
            {
                return legacyCardCreditPlusDTO;
            }
        }

    }


        /// <summary>
        /// Manages the list of LegacyCardCreditPlus
        /// </summary>
        public class LegacyCardCreditPlusListBL
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private readonly ExecutionContext executionContext;

            /// <summary>
            /// Parameterized constructor
            /// </summary>
            /// <param name="executionContext">execution context</param>
            public LegacyCardCreditPlusListBL(ExecutionContext executionContext)
            {
                log.LogMethodEntry(executionContext);
                this.executionContext = executionContext;
                log.LogMethodExit();
            }
            /// <summary>
            /// Returns the LegacyCardCreditPlus list
            /// </summary>
            public List<LegacyCardCreditPlusDTO> GetLegacyCardCreditPlusDTOList(List<KeyValuePair<LegacyCardCreditPlusDTO.SearchByParameters, string>> searchParameters,
                bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(searchParameters);
                LegacyCardCreditPlusDataHandler LegacyCardCreditPlusDataHandler = new LegacyCardCreditPlusDataHandler(sqlTransaction);
                List<LegacyCardCreditPlusDTO> LegacyCardCreditPlusDTOList = LegacyCardCreditPlusDataHandler.GetLegacyCardCreditPlusDTOList(searchParameters, sqlTransaction);
                if (loadChildRecords)
                {
                    if (LegacyCardCreditPlusDTOList != null && LegacyCardCreditPlusDTOList.Count > 0)
                    {
                        LegacyCardCreditPlusBuilderBL LegacyCardCreditPlusBuilder = new LegacyCardCreditPlusBuilderBL(executionContext);
                        LegacyCardCreditPlusBuilder.Build(LegacyCardCreditPlusDTOList, activeChildRecords, sqlTransaction);
                    }
                }
                log.LogMethodExit(LegacyCardCreditPlusDTOList);
                return LegacyCardCreditPlusDTOList;
            }
        }

    /// <summary>
    /// Builds the complex LegacyCardCreditPlus entity structure
    /// </summary>
    public class LegacyCardCreditPlusBuilderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public LegacyCardCreditPlusBuilderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex LegacyCardCreditPlus DTO structure
        /// </summary>
        /// <param name="LegacyCardCreditPlusDTO">LegacyCardCreditPlus dto</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(LegacyCardCreditPlusDTO LegacyCardCreditPlusDTO, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(LegacyCardCreditPlusDTO, activeChildRecords);
            if (LegacyCardCreditPlusDTO != null && LegacyCardCreditPlusDTO.LegacyCardCreditPlusId != -1)
            {
                LegacyCreditPlusConsumptionListBL LegacyCardCreditPlusConsumptionListBL = new LegacyCreditPlusConsumptionListBL(executionContext);
                List<KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>> LegacyCardCreditPlusConsumptionSearchParams = new List<KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>>();
                LegacyCardCreditPlusConsumptionSearchParams.Add(new KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>(LegacyCardCreditPlusConsumptionDTO.SearchByParameters.LEGACY_CARD_CREDIT_PLUS_ID, LegacyCardCreditPlusDTO.LegacyCardCreditPlusId.ToString()));
                if (LegacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList == null)
                {
                    LegacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList = new List<LegacyCardCreditPlusConsumptionDTO>();
                }
                LegacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList = LegacyCardCreditPlusConsumptionListBL.GetLegacyCardCreditPlusConsumptionDTOList(LegacyCardCreditPlusConsumptionSearchParams, sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex LegacyCardCreditPlusDTO structure
        /// </summary>
        /// <param name="LegacyCardCreditPlusDTOList">LegacyCardCreditPlus dto list</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(List<LegacyCardCreditPlusDTO> LegacyCardCreditPlusDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(LegacyCardCreditPlusDTOList, activeChildRecords, sqlTransaction);
            if (LegacyCardCreditPlusDTOList != null && LegacyCardCreditPlusDTOList.Count > 0)
            {
                Dictionary<int, LegacyCardCreditPlusDTO> legacyCardCreditPlusDictionary = new Dictionary<int, LegacyCardCreditPlusDTO>();
                HashSet<int> legacyCardIdSet = new HashSet<int>();
                string cardIdList;
                for (int i = 0; i < LegacyCardCreditPlusDTOList.Count; i++)
                {
                    if (LegacyCardCreditPlusDTOList[i].LegacyCardCreditPlusId != -1 &&
                        LegacyCardCreditPlusDTOList[i].LegacyCard_id != -1)
                    {
                        legacyCardIdSet.Add(LegacyCardCreditPlusDTOList[i].LegacyCard_id);
                        legacyCardCreditPlusDictionary.Add(LegacyCardCreditPlusDTOList[i].LegacyCardCreditPlusId, LegacyCardCreditPlusDTOList[i]);
                    }
                }
                cardIdList = string.Join<int>(",", legacyCardIdSet);
                LegacyCreditPlusConsumptionListBL legacyCardCreditPlusConsumptionListBL = new LegacyCreditPlusConsumptionListBL(executionContext);
                List<KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>> LegacyCardCreditPlusConsumptionSearchParams = new List<KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>>();
                LegacyCardCreditPlusConsumptionSearchParams.Add(new KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>(LegacyCardCreditPlusConsumptionDTO.SearchByParameters.CARD_ID_LIST, cardIdList));
                List<LegacyCardCreditPlusConsumptionDTO> LegacyCardCreditPlusConsumptionDTOList = legacyCardCreditPlusConsumptionListBL.GetLegacyCardCreditPlusConsumptionDTOList(LegacyCardCreditPlusConsumptionSearchParams, sqlTransaction);
                if (LegacyCardCreditPlusConsumptionDTOList != null && LegacyCardCreditPlusConsumptionDTOList.Count > 0)
                {
                    foreach (var legacyCardCreditPlusConsumptionDTO in LegacyCardCreditPlusConsumptionDTOList)
                    {
                        if (legacyCardCreditPlusDictionary.ContainsKey(legacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusId))
                        {
                            if (legacyCardCreditPlusDictionary[legacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusId].LegacyCardCreditPlusConsumptionDTOList == null)
                            {
                                legacyCardCreditPlusDictionary[legacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusId].LegacyCardCreditPlusConsumptionDTOList = new List<LegacyCardCreditPlusConsumptionDTO>();
                            }
                            legacyCardCreditPlusDictionary[legacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusId].LegacyCardCreditPlusConsumptionDTOList.Add(legacyCardCreditPlusConsumptionDTO);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
