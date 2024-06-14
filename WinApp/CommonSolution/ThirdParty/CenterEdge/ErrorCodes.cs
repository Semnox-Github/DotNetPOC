/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - ErrorCode class - This would Error Code
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public enum ErrorCode
    {
        cardNotFound,
        cardExists,
        pinNotFound,
        badRequest,
        invalidLogin,
        unauthorized
    }
}
