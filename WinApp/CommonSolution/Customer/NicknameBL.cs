/********************************************************************************************
 * Project Name - Customer
 * Description  - Business logic of Nickname
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.150.3     26-07-2023       Abhishek       Created : Nickname Generation
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Linq;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for Nickname class.
    /// </summary>
    public class NicknameBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of NicknameBL class
        /// </summary>
        public NicknameBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets NickName
        /// </summary>
        public string GetNickName()
        {
            log.LogMethodEntry();
            Random random = new Random();
            string firstName = NickNameDTO.nameNickNameDictionary.ElementAt(random.Next(0, NickNameDTO.nameNickNameDictionary.Count)).Value;
            string lastName = NickNameDTO.colorNickNameDictionary.ElementAt(random.Next(0, NickNameDTO.nameNickNameDictionary.Count)).Value;
            const int maxValue = 999;
            string number = random.Next(100, maxValue + 1).ToString();
            string nickName = firstName + lastName + number;
            log.LogMethodExit(nickName);
            return nickName;
        }
    }
}
