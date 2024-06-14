/********************************************************************************************
 * Project Name - VirtualPoints DTO                                                                         
 * Description  - Dto to hold the game machine level results in point structure
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/

namespace Semnox.Parafait.Game.VirtualArcade
{
    /// <summary>
    /// Points
    /// </summary>
    public class Points
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string type;
        private int value;
        /// <summary>
        /// Points
        /// </summary>
        public Points()
        {
            type = PointTypes.Point.ToString();
            value = 0;
        }

        /// <summary>
        /// Points
        /// </summary>
        /// <param name="pointType"></param>
        /// <param name="pointvalue"></param>
        public Points(string pointType,int pointvalue)
        {
            this.type = pointType;
            this.value = pointvalue;
        }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return type; } set {  type = value; } }
        /// <summary>
        /// Value
        /// </summary>
        public int Value { get { return value; } set { this.value = value; } }
    }
}
