/********************************************************************************************
 * Project Name - Discounts
 * Description  - Data structure of DiscountedGamesContainer
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      12-Apr-2021      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Data structure of DiscountedProductsContainerDTO
    /// </summary>
    public class DiscountedGamesContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int discountId;
        private int gameId;
        private string discounted;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountedGamesContainerDTO()
        {
            log.LogMethodEntry();
            id = -1;
            discounted = string.Empty;
            discountId = -1;
            gameId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        public DiscountedGamesContainerDTO(int id, int gameId, int discountId, string discounted)
            :this()
        {
            log.LogMethodEntry(id, gameId, discountId, discounted);
            this.id = id;
            this.discountId = discountId;
            this.gameId = gameId;
            this.discounted = discounted;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Get/Set method of discountId field
        /// </summary>
        public int DiscountId
        {
            get { return discountId; }
            set { discountId = value; }
        }

        /// <summary>
        /// Get/Set method of gameId field
        /// </summary>
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        /// <summary>
        /// Get/Set method of discounted field 
        /// </summary>
        public string Discounted
        {
            get { return discounted; }
            set {  discounted = value;}
        }
    }
}
