/********************************************************************************************
 * Project Name - Product
 * Description  - Product Group Map Business object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 ********************************************************************************************* 
 *2.170.0     05-Jul-2023      Lakshminarayana     Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Discounts
{
    public class DiscountedGamesBL
    {
        private DiscountedGamesDTO discountedGamesDTO;
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Default constructor of DiscountedGamesBL class
        /// </summary>
        private DiscountedGamesBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DiscountedGamesId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DiscountedGamesBL(ExecutionContext executionContext, int id, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, id, unitOfWork);
            DiscountedGamesDataHandler discountedGamesDataHandler = new DiscountedGamesDataHandler(executionContext, unitOfWork);
            discountedGamesDTO = discountedGamesDataHandler.GetDiscountedGames(id);
            if (discountedGamesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "DiscountedGames", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DiscountedGamesBL object using the DiscountedGamesDTO
        /// </summary>
        public DiscountedGamesBL(ExecutionContext executionContext, DiscountedGamesDTO discountedGamesDTO, UnitOfWork unitOfWork)
            : this(executionContext, unitOfWork)
        {
            log.LogMethodEntry(executionContext, discountedGamesDTO, unitOfWork);
            if (discountedGamesDTO.Id < 0)
            {
                ValidateGameId(discountedGamesDTO.GameId);
                ValidateDiscounted(discountedGamesDTO.Discounted);
            }
            this.discountedGamesDTO = discountedGamesDTO;
            log.LogMethodExit();
        }

        public void Update(DiscountedGamesDTO parameterDiscountedGamesDTO)
        {
            log.LogMethodEntry(parameterDiscountedGamesDTO);
            ChangeGameId(parameterDiscountedGamesDTO.GameId);
            ChangeDiscounted(parameterDiscountedGamesDTO.Discounted);
            ChangeIsActive(parameterDiscountedGamesDTO.IsActive);
            log.LogMethodExit();
        }

        private void ChangeGameId(int GameId)
        {
            log.LogMethodEntry(GameId);
            if (discountedGamesDTO.GameId == GameId)
            {
                log.LogMethodExit(null, "No changes to DiscountedGames GameId");
                return;
            }
            ValidateGameId(GameId);
            discountedGamesDTO.GameId = GameId;
            log.LogMethodExit();
        }

        private void ChangeDiscounted(string discounted)
        {
            log.LogMethodEntry(discounted);
            if (discountedGamesDTO.Discounted == discounted)
            {
                log.LogMethodExit(null, "No changes to DiscountedGames discounted");
                return;
            }
            ValidateDiscounted(discounted);
            discountedGamesDTO.Discounted = discounted;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (discountedGamesDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to DiscountedGames isActive");
                return;
            }
            discountedGamesDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ValidateGameId(int gameId)
        {
            log.LogMethodEntry(gameId);
            if (gameId <= -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Game"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Product is empty.", "DiscountedGames", "GameId", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateDiscounted(string discounted)
        {
            log.LogMethodEntry(discounted);
            if (string.IsNullOrWhiteSpace(discounted) || (discounted != "Y" && discounted != "N"))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discounted"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Invalid value for discounted.", "DiscountedGames", "Discounted", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the DiscountedGames
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            DiscountedGamesDataHandler discountedGamesDataHandler = new DiscountedGamesDataHandler(executionContext, unitOfWork);
            discountedGamesDTO = SiteContainerList.ToSiteDateTime(executionContext, discountedGamesDTO);
            discountedGamesDataHandler.Save(discountedGamesDTO);
            discountedGamesDTO = SiteContainerList.FromSiteDateTime(executionContext, discountedGamesDTO);
            discountedGamesDTO.AcceptChanges();
            log.LogMethodExit();
        }

        #region Properties
        /// <summary>
        /// Gets the DTO
        /// </summary>
        internal DiscountedGamesDTO DiscountedGamesDTO
        {
            get
            {
                return discountedGamesDTO;
            }
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id
        {
            get
            {
                return discountedGamesDTO.Id;
            }
        }

        /// <summary>
        /// Get/Set method of the discountId field
        /// </summary>
        [Browsable(false)]
        public int DiscountId
        {
            get
            {
                return discountedGamesDTO.DiscountId;
            }
        }

        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>
        [DisplayName("Game")]
        public int GameId
        {
            get
            {
                return discountedGamesDTO.GameId;
            }

        }

        /// <summary>
        /// Get/Set method of the Discounted field
        /// </summary>
        [DisplayName("Discounted")]
        public string Discounted
        {
            get
            {
                return discountedGamesDTO.Discounted;
            }

        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [Browsable(false)]
        public bool IsActive
        {
            get
            {
                return discountedGamesDTO.IsActive;
            }

        }
        #endregion
    }

    /// <summary>
    /// Manages the list of DiscountedGames
    /// </summary>
    public class DiscountedGamesListBL
    {
        private readonly Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;

        /// <summary>
        /// Parameterized constructor of DiscountedGamesListBL class
        /// </summary>
        public DiscountedGamesListBL(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates and saves the DiscountedGamesDTOList to the db
        /// </summary>
        public void Save(List<DiscountedGamesDTO> discountedGamesDTOList)
        {
            log.LogMethodEntry(discountedGamesDTOList);
            if (discountedGamesDTOList == null ||
                discountedGamesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            foreach (var discountedGamesDTO in discountedGamesDTOList)
            {
                DiscountedGamesBL discountedGamesBL = new DiscountedGamesBL(executionContext, discountedGamesDTO, unitOfWork);
                discountedGamesBL.Save();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DiscountedGamesDTO List for Discount Id List
        /// </summary>
        /// <param name="discountIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of DiscountedGamesDTO</returns>
        public List<DiscountedGamesDTO> GetDiscountedGamesDTOListOfDiscounts(List<int> discountIdList, bool activeRecords = true, bool onlyDiscountedChildRecord = false)
        {
            log.LogMethodEntry(discountIdList);
            DiscountedGamesDataHandler discountedGamesDataHandler = new DiscountedGamesDataHandler(executionContext, unitOfWork);
            List<DiscountedGamesDTO> discountedGamesDTOList = discountedGamesDataHandler.GetDiscountedGamesDTOListOfDiscounts(discountIdList, activeRecords, onlyDiscountedChildRecord);
            log.LogMethodExit(discountedGamesDTOList);
            return discountedGamesDTOList;
        }
    }
}