using System;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// MachineParams Class
    /// </summary>
    public class MachineParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int gameId; //data fields made as private on 12-Aug-2019 by Deeksha
        private String machineName;
        private int activeFlag;
        private String macAddress;
        private bool calculateActiveTheme;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineParams()
        {
            log.LogMethodEntry();
            this.gameId = -1;
            this.activeFlag = -1;
            this.machineName = "";
            this.macAddress = "";
            this.calculateActiveTheme = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MachineParams(int gameId, String machineName, int activeFlag, String macAddress)
        {
            log.LogMethodEntry(gameId, machineName, activeFlag, macAddress);
            this.gameId = gameId;
            this.machineName = machineName;
            this.activeFlag = activeFlag;
            this.macAddress = macAddress;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MachineName field
        /// </summary>
        public string MachineName { get { return machineName; } set { machineName = value; } }

        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; } }

        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public int ActiveFlag { get { return activeFlag; } set { activeFlag = value; } }

        /// <summary>
        /// Get/Set method of the MacAddress field
        /// </summary>
        public string MacAddress { get { return macAddress; } set { macAddress = value; } }

        /// <summary>
        /// Get/Set method of the calculateActiveTheme field
        /// </summary>
        public bool CalculateActiveTheme { get { return calculateActiveTheme; } set { calculateActiveTheme = value; } }
    } 
    

}
