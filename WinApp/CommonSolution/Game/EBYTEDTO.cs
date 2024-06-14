/********************************************************************************************
 * Project Name - EBYTE Hub DTO                                                                         
 * Description  - Dto of the EBYTE Hub class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.70        16-Oct-2019   Rakesh Kumar       Created
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public class EBYTEDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                
        private int uARTParity;
        private int dataRate;
        private int transmissionMode;
        private int iODriveMode;
        private int wakeUpTime;
        private int fecSwitch;
        private int outputPower;

        public EBYTEDTO()
        {
            log.LogMethodEntry();
            uARTParity = 0;
            dataRate = 0;
            transmissionMode = 0;
            iODriveMode = 0;
            wakeUpTime = 0;
            fecSwitch = 0;
            outputPower = 0;
            log.LogMethodExit();
        }
        public EBYTEDTO(int uARTParity, int dataRate, int transmissionMode, int iODriveMode, int wakeUpTime, int fecSwitch, int outputPower)
        {
            log.LogMethodEntry( uARTParity, dataRate, transmissionMode, iODriveMode, wakeUpTime, fecSwitch, outputPower);
            this.uARTParity = uARTParity;
            this.dataRate = dataRate;
            this.transmissionMode = transmissionMode;
            this.iODriveMode = iODriveMode;
            this.wakeUpTime = wakeUpTime;
            this.fecSwitch = fecSwitch;
            this.outputPower = outputPower;
            log.LogMethodExit();
        }        
        /// <summary>
        /// Get/Set method of the UARTParity
        /// </summary>
        public int UARTParity { get { return uARTParity; } set { uARTParity = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DataRate
        /// </summary>
        public int DataRate { get { return dataRate; } set { dataRate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TransmissionMode
        /// </summary>
        public int TransmissionMode { get { return transmissionMode; } set { transmissionMode = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ODriveMode
        /// </summary>
        public int IODriveMode { get { return iODriveMode; } set { iODriveMode = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the WakeupTime
        /// </summary>
        public int WakeupTime { get { return wakeUpTime; } set { wakeUpTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FecSwitch
        /// </summary>
        public int FecSwitch { get { return fecSwitch; } set { fecSwitch = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the OutputPower
        /// </summary>
        public int OutputPower { get { return outputPower; } set { outputPower = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || uARTParity < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }
        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
