/************************************************************************************************************
 * Project Name - Device
 * Description  - KeyBoardWedge
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *************************************************************************************************************
 * 2.70.2     08-Aug-2019     Deeksha        Added logger methods.
 * 2.100.0    09-Dec-2020     Mathew Ninan   Added support for encrypted QR code scan. Current
 *                                           method was not considering case of alphabets. This 
 *                                           is handled by looking at keyboardState and doing 
 *                                           ToUnicodeEx of identified string from GETRAWINPUTDATA
 ************************************************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device
{
    public class USBDevice : DeviceClass
    {

        protected static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region const definitions

        // The following constants are defined in Windows.h

        public const int RIDEV_INPUTSINK = 0x00000100;
        public const int RID_INPUT = 0x10000003;

        public const int RIM_TYPEMOUSE = 0;
        public const int RIM_TYPEKEYBOARD = 1;
        public const int RIM_TYPEHID = 2;

        public const int RIDI_DEVICENAME = 0x20000007;

        internal const int WM_KEYDOWN = 0x0100;
        internal const int WM_KEYUP = 0x0101;
        internal const int WM_CHAR = 0x0102;
        internal const int WM_INPUT = 0x00FF;
        public const int VK_RETURN = 0x0D;
        public const int VK_SHIFT = 0x10;
        public const int VK_CAPITAL = 0x14;
        public const uint MAPVK_VK_TO_CHAR = 2;
        //uCode is a virtual-key code and is translated into a scan code. 
        //If it is a virtual-key code that does not distinguish between left- and right-hand keys, 
        //the left-hand scan code is returned. If there is no translation, the function returns 0.
        public const uint MAPVK_VK_TO_VSC = 0;
        public const string ENGLISHLOCALEIDENTIFIER = "00000409";
        // Required for the processing of the GetLastError, to get the detailed error message for DLL failures
        public const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        public const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        public const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

        #endregion const definitions

        #region structs & enums

        /// <summary>
        /// An enum representing the different types of input devices.
        /// </summary>
        public enum DeviceType
        {
            Key,
            Mouse,
            OEM
        }

        /// <summary>
        /// Enumeration of removal options.
        /// </summary>
        public enum PeekMessageRemoveFlag : uint
        {
            /// <summary>
            /// Messages are not removed from the queue after processing by PeekMessage.
            /// </summary>
            PM_NOREMOVE = 0x0,

            /// <summary>
            /// Messages are removed from the queue after processing by PeekMessage.
            /// </summary>
            PM_REMOVE = 0x1
        }
        /// <summary>
        /// Enumeration containing flags for a raw input device.
        /// </summary>
        [Flags]
        internal enum RawInputDeviceFlags
        {
            /// <summary>
            /// No flags, the default.
            /// </summary>
            NONE = 0x0,

            /// <summary>
            /// Microsoft Windows XP Service Pack 1 (SP1): If set, the 
            /// application command keys are handled. RIDEV_APPKEYS can be 
            /// specified only if RIDEV_NOLEGACY is specified for a keyboard 
            /// device.
            /// </summary>
            RIDEV_APPKEYS = 0x400,

            /// <summary>
            /// If set, the mouse button click does not activate the other
            /// window.
            /// </summary>
            RIDEV_CAPTUREMOUSE = 0x200,

            /// <summary>
            /// If set, this specifies the top level collections to exclude when
            /// reading a complete usage page. This flag only affects a TLC 
            /// whose usage page is already specified with RIDEV_PAGEONLY.
            /// </summary>
            RIDEV_EXCLUDE = 0x10,

            /// <summary>
            /// Windows Vista or later: If set, this enables the caller to 
            /// receive input in the background only if the foreground 
            /// application does not process it. In other words, if the 
            /// foreground application is not registered for raw input, then 
            /// the background application that is registered will receive the 
            /// input.
            /// </summary>
            RIDEV_EXINPUTSINK = 0x1000,

            /// <summary>
            /// If set, this enables the caller to receive the input even when 
            /// the caller is not in the foreground. Note that hwndTarget 
            /// must be specified.
            /// </summary>
            RIDEV_INPUTSINK = 0x100,

            /// <summary>
            /// If set, the application-defined keyboard device hotkeys are not 
            /// handled. However, the system hotkeys; for example, ALT+TAB and 
            /// CTRL+ALT+DEL, are still handled. By default, all keyboard 
            /// hotkeys are handled. RIDEV_NOHOTKEYS can be specified even if 
            /// RIDEV_NOLEGACY is not specified and hwndTarget is NULL.
            /// </summary>
            RIDEV_NOHOTKEYS = 0x200,

            /// <summary>
            /// If set, this prevents any devices specified by usUsagePage or 
            /// usUsage from generating legacy messages. This is only for the 
            /// mouse and keyboard. If RIDEV_NOLEGACY is set for a mouse or a
            /// keyboard, the system does not generate any legacy message for 
            /// that device for the application. For example, if the mouse TLC 
            /// is set with RIDEV_NOLEGACY, WM_LBUTTONDOWN and related legacy
            /// mouse messages are not generated. Likewise, if the keyboard 
            /// TLC is set with RIDEV_NOLEGACY, WM_KEYDOWN and related legacy 
            /// keyboard messages are not generated.
            /// </summary>
            RIDEV_NOLEGACY = 0x30,

            /// <summary>
            /// If set, this specifies all devices whose top level collection 
            /// is from the specified usUsagePage. Note that usUsage must be zero. 
            /// To exclude a particular top level collection, use RIDEV_EXCLUDE.
            /// </summary>
            RIDEV_PAGEONLY = 0x20,

            /// <summary>
            /// If set, this removes the top level collection from the inclusion 
            /// list. This tells the operating system to stop reading from a 
            /// device which matches the top level collection.
            /// </summary>
            RIDEV_REMOVE = 0x1
        }

        /// <summary>
        /// Class encapsulating the information about a
        /// keyboard event, including the device it
        /// originated with and what key was pressed
        /// </summary>
        public class DeviceInfo
        {
            public string deviceName;
            public string deviceType;
            public IntPtr deviceHandle;
            public string Name;
            public string source;
            public ushort key;
            public string vKey;
        }

        #region Windows.h structure declarations

        #region DllImports

        [DllImport("User32.dll")]
        public extern static uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll")]
        public extern static uint GetRawInputDeviceInfo(IntPtr hDevice, uint uiCommand, IntPtr pData, ref uint pcbSize);

        [DllImport("User32.dll")]
        public extern static bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevice, uint uiNumDevices, uint cbSize);

        [DllImport("User32.dll")]
        public extern static uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool PeekMessage(out MSG lpmsg, IntPtr hwnd, uint wMsgFilterMin, uint wMsgFilterMax, PeekMessageRemoveFlag wRemoveMsg);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer, uint nSize, IntPtr pArguments);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("user32.dll")]
        internal static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder pwszBuff, int cchBuff, uint wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(Keys nVirtKey);

        /// <summary>
        /// Translates (maps) a virtual-key code 
        /// into a scan code or character value, or translates a scan code into a virtual-key code
        /// </summary>
        /// <param name="uCode">The virtual key code or scan code for a key</param>
        /// <param name="uMapType">The translation to be performed</param>
        /// <returns>
        /// The return value is either a scan code, a virtual-key code, 
        /// or a character value, depending on the value of uCode and uMapType
        /// </returns>
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        /// <summary>
        /// Retrieves the active input locale identifier (formerly called the keyboard layout).
        /// </summary>
        /// <param name="idThread">The identifier of the thread to query, or 0 for the current thread.</param>
        /// <returns>
        /// The return value is the input locale identifier for the thread. 
        /// The low word contains a Language Identifier for the input language 
        /// and the high word contains a device handle to the physical layout of the keyboard.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        /// <summary>
        /// Load keyboardlayout for specific locale
        /// </summary>
        /// <param name="pwszKLID">The name of the input locale identifier to load.</param>
        /// <param name="flags">Specifies how the input locale identifier is to be loaded.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr LoadKeyboardLayoutA(string pwszKLID, uint flags);

        /// <summary>
        /// Translates the specified virtual-key code and keyboard state to the corresponding 
        /// Unicode character or characters. 
        /// </summary>
        /// <param name="wVirtKey">The virtual-key code to be translated</param>
        /// <param name="wScanCode">The hardware scan code of the key to be translated. The high-order bit of this value is set if the key is up</param>
        /// <param name="lpKeyState">keyboard state</param>
        /// <param name="pwszBuff">result unicode character set returned by method</param>
        /// <param name="cchBuff">The size, in characters, of the buffer pointed to by the pwszBuff parameter.</param>
        /// <param name="wFlags"></param>
        /// <param name="dwhkl">The input locale identifier used to translate the specified code.</param>
        /// <returns>Returns -1, 0, 1/above. Dead Key character for -1.
        /// 0 when return value is blank
        /// 1/above - number of characters in result set
        /// </returns>
        [DllImport("user32.dll")]
        public static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
        #endregion DllImports

        // The following structures are defined in Windows.h

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTDEVICELIST
        {
            public IntPtr hDevice;
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTDEVICE
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort usUsagePage;
            [MarshalAs(UnmanagedType.U2)]
            public ushort usUsage;
            [MarshalAs(UnmanagedType.U4)]
            public int dwFlags;
            public IntPtr hwndTarget;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWHID
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwSizHid;
            [MarshalAs(UnmanagedType.U4)]
            public int dwCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct BUTTONSSTR
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonFlags;
            [MarshalAs(UnmanagedType.U2)]
            public ushort usButtonData;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWMOUSE
        {
            [MarshalAs(UnmanagedType.U2)]
            [FieldOffset(0)]
            public ushort usFlags;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(4)]
            public uint ulButtons;
            [FieldOffset(4)]
            public BUTTONSSTR buttonsStr;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(8)]
            public uint ulRawButtons;
            [FieldOffset(12)]
            public int lLastX;
            [FieldOffset(16)]
            public int lLastY;
            [MarshalAs(UnmanagedType.U4)]
            [FieldOffset(20)]
            public uint ulExtraInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWKEYBOARD
        {
            [MarshalAs(UnmanagedType.U2)]
            public ushort MakeCode;
            [MarshalAs(UnmanagedType.U2)]
            public ushort Flags;
            [MarshalAs(UnmanagedType.U2)]
            public ushort Reserved;
            [MarshalAs(UnmanagedType.U2)]
            public ushort VKey;
            [MarshalAs(UnmanagedType.U4)]
            public uint Message;
            [MarshalAs(UnmanagedType.U4)]
            public uint ExtraInformation;
        }

        #endregion Windows.h structure declarations

        /// <summary>
        /// Defines the X and y coordinates of a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public long x;
            public long y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        #endregion structs & enums


        #region Variables
        public DeviceInfo dInfo;
        public String currentReadString;
        public bool isFinal = true;
        public bool isOpen = true;
        public IntPtr inputLocaleIdentifierA = IntPtr.Zero;
        //private CardScannerKeyDownMessageFilter filter;

        #endregion Variables

        #region Events
        /// <summary>
        /// Event fired when a barcode is scanned.
        /// </summary>

        /// <summary>
        /// Hook into the form's WndProc message. We listen for WM_INPUT and do
        /// special processing on the raw data.
        /// </summary>
        #endregion Events

        #region String ProcessLastError()

        private String ProcessLastError()
        {
            log.LogMethodEntry();
            int nLastError = Marshal.GetLastWin32Error();
            IntPtr lpMsgBuf = IntPtr.Zero;
            uint dwChars = FormatMessage(
                FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
                IntPtr.Zero,
                (uint)nLastError,
                0, // Default language
                ref lpMsgBuf,
                0,
                IntPtr.Zero);
            if (dwChars == 0)
            {
                // Handle the error.
                int le = Marshal.GetLastWin32Error();
                log.LogMethodExit();
                return null;
            }
            string sRet = Marshal.PtrToStringAnsi(lpMsgBuf);
            // Free the buffer.

            lpMsgBuf = LocalFree(lpMsgBuf);
            log.LogMethodExit(sRet);
            return sRet;
        }
        #endregion String ProcessLastError()


        #region bool CardReaderIdentifier(string vidStrg, string pidStrg, string optionalStrg)

        public bool CardReaderIdentifier(string vidStrg, string pidStrg, string optionalStrg)
        {
            log.LogMethodEntry(vidStrg, pidStrg, optionalStrg);
            uint deviceCount = 0;
            uint apiOpt;
            int dwSize = (Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            bool devicefound = false;
            if (vidStrg == "")
                vidStrg = "VID_1243";
            if (pidStrg == "")
                pidStrg = "PID_E000";
            if (optionalStrg == "")
                optionalStrg = "MI_00";
            if (GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)dwSize) != 0)
            {
                throw new ApplicationException("An error occurred while retrieving the list of devices. Error in GetRawInputDeviceList. Detailed Error is " + ProcessLastError());
            }
            IntPtr pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
            if (pRawInputDeviceList == null)
                throw new ApplicationException("Memory allocation failed");

            apiOpt = GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint)dwSize);
            log.Debug("Device Count: " + deviceCount);
            if (apiOpt < 0)
                throw new ApplicationException("Could not fetch raw device list. Failure in GetRawInputDeviceList - Second call. Detailed Error is " + ProcessLastError());

            for (uint devCount = 0; devCount < deviceCount; devCount++)
            {
                string deviceName;
                uint pcbSize = 0;

                RAWINPUTDEVICELIST rid = (RAWINPUTDEVICELIST)Marshal.PtrToStructure(
                                               new IntPtr((pRawInputDeviceList.ToInt32() + (dwSize * devCount))),
                                               typeof(RAWINPUTDEVICELIST));

                GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);

                if (pcbSize > 0)
                {
                    IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
                    GetRawInputDeviceInfo(rid.hDevice, RIDI_DEVICENAME, pData, ref pcbSize);
                    deviceName = (string)Marshal.PtrToStringAnsi(pData);
                    log.LogVariableState("Device Name: ", deviceName);
                    log.LogVariableState("Device Type: ", rid.dwType);
                    String upperCasedeviceName = deviceName.ToUpper();
                    if (rid.dwType == RIM_TYPEKEYBOARD)
                    {
                        if (upperCasedeviceName.Contains(vidStrg.ToUpper()) && upperCasedeviceName.Contains(pidStrg.ToUpper()) && upperCasedeviceName.Contains(optionalStrg.ToUpper()))
                        {
                            log.LogVariableState("Device Found: ", deviceName);
                            log.LogVariableState("Device Found flag: ", devicefound);
                            if (devicefound == false)
                            {
                                devicefound = true;
                                dInfo = new DeviceInfo();
                                dInfo.deviceName = deviceName;
                                dInfo.deviceHandle = rid.hDevice;
                                dInfo.deviceType = GetDeviceType(rid.dwType);
                                dInfo.Name = deviceName;
                            }
                            else
                            {
                                throw new ApplicationException("Multiple card readers have been found with the same vendor and product id");
                            }
                        }
                    }
                    Marshal.FreeHGlobal(pData);
                }
            }
            Marshal.FreeHGlobal(pRawInputDeviceList);
            log.LogMethodExit(devicefound);
            return devicefound;
        }
        #endregion CardReaderIdentifier(string vidStrg, string pidStrg, string optionalStrg)

        # region RegisterKBD(IntPtr)
        bool RegisterKBD(IntPtr hwnd)
        {
            log.LogMethodEntry(hwnd);
            RAWINPUTDEVICE[] rid;

            rid = new RAWINPUTDEVICE[1];

            rid[0].usUsagePage = 0x01;      // USB HID Generid Desktop Page
            rid[0].usUsage = 0x06;          // Keyboard Usage ID
            rid[0].dwFlags = (int)RawInputDeviceFlags.RIDEV_INPUTSINK;
            rid[0].hwndTarget = hwnd;

            if (!RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])))
            {
                InvalidOperationException e;

                e = new InvalidOperationException(
                    "The barcode scanner listener could not register for raw input devices.");
                throw e;
            }
            log.LogMethodExit(true);
            return true;
        }
        #endregion RegisterKBD(IntPtr);

        /// <summary>
        /// Hooks into the form's HandleCreated and HandleDestoryed events
        /// to ensure that we start and stop listening at appropriate times.
        /// </summary>
        /// <param name="form">the form to listen to</param>
        private void HookHandleEvents(Form form)
        {
            log.LogMethodEntry(form);
            form.HandleCreated += this.OnHandleCreated;
            form.HandleDestroyed += this.OnHandleDestroyed;
            log.LogMethodExit();
        }

        /// <summary>
        /// When the form's handle is created, let's hook into it so we can see
        /// the WM_INPUT event.
        /// </summary>
        /// <param name="sender">the form whose handle was created</param>
        /// <param name="e">the event arguments</param>
        private void OnHandleCreated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.AssignHandle(((Form)sender).Handle);
            log.LogMethodExit();
        }

        /// <summary>
        /// When the form's handle is destroyed, let's unhook from it so we stop
        /// listening and allow the OS to free up its resources.
        /// </summary>
        /// <param name="sender">the form whose handle was destroyed</param>
        /// <param name="e">the event arguments</param>
        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.ReleaseHandle();
            log.LogMethodExit();
        }

        #region InitializeUSBCardReader(Form appForm, EventHandler cardReadEventHandler, String vidStrg, String pidStrg, String addStringforIdent)
        /// <summary>
        /// This function will initialize the card reader and register to listen
        /// for the card swipe. 
        /// </summary>
        /// <param name="appForm">A Form type, this is the handle to pass the event</param>
        /// <returns>A flag indicating the success</returns>
        /// 
        public bool InitializeUSBReader(Form appForm, String vidStrg, String pidStrg, String addStringforIdent)
        {
            log.LogMethodEntry(appForm, vidStrg, pidStrg, addStringforIdent);
            this.isOpen = false;
            this.isOpen = CardReaderIdentifier(vidStrg, pidStrg, addStringforIdent);
            log.LogVariableState("Listener Open Status: ", this.isOpen);
            if (this.isOpen == false)
            {
                log.LogMethodExit(); 
                return this.isOpen;
            }
            IntPtr handleWnd = appForm.Handle;
            this.isOpen = RegisterKBD(handleWnd);
            if (!this.isOpen)
            {
                log.LogMethodExit();
                return this.isOpen;
            }
            this.HookHandleEvents(appForm);
            if (this.Handle != null)
                this.ReleaseHandle();
            this.AssignHandle(handleWnd);
            this.isFinal = false;

            synContext = System.Threading.SynchronizationContext.Current;

            this.currentReadString = "";
            //            this.filter = new CardScannerKeyDownMessageFilter();
            //            Application.AddMessageFilter(this.filter);
            this.isOpen = true;
            log.LogMethodExit();
            return this.isOpen;
        }
        #endregion InitializeUSBCardReader(Form appForm, EventHandler cardReadEventHandler, String vidStrg, String pidStrg, String addStringforIdent)

        public USBDevice()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public USBDevice(DeviceDefinition deviceDefinition)
            :base(deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            this.isOpen = false;
            this.isOpen = CardReaderIdentifier(deviceDefinition.Vid, deviceDefinition.Pid, deviceDefinition.OptString);
            log.LogVariableState("Listener Open Status: ", this.isOpen);
            if (this.isOpen == false)
            {
                string errorMessage = "Unable to register USB Device " + deviceDefinition.Name;
                log.LogMethodExit(null, "Throwing Exception -" + errorMessage); 
                throw new Exception(errorMessage);
            }
            IntPtr handleWnd = deviceDefinition.Handle;
            this.isOpen = RegisterKBD(handleWnd);
            if (!this.isOpen)
            {
                log.LogMethodExit();
            }
            this.AssignHandle(handleWnd);
            this.isFinal = false;

            synContext = System.Threading.SynchronizationContext.Current;

            this.currentReadString = "";
            this.isOpen = true;
            log.LogMethodExit();
        }

        #region GetDeviceType( int device )

        /// <summary>
        /// Converts a RAWINPUTDEVICELIST dwType value to a string
        /// describing the device type.
        /// </summary>
        /// <param name="device">A dwType value (RIM_TYPEMOUSE, 
        /// RIM_TYPEKEYBOARD or RIM_TYPEHID).</param>
        /// <returns>A string representation of the input value.</returns>
        private string GetDeviceType(int device)
        {
            log.LogMethodEntry(device);
            string deviceType;
            switch (device)
            {
                case RIM_TYPEMOUSE: deviceType = "MOUSE"; break;
                case RIM_TYPEKEYBOARD: deviceType = "KEYBOARD"; break;
                case RIM_TYPEHID: deviceType = "HID"; break;
                default: deviceType = "UNKNOWN"; break;
            }
            log.LogMethodExit(deviceType);
            return deviceType;
        }

        #endregion GetDeviceType( int device )

        public virtual bool ProcessRawInputMessage(IntPtr rawInputHeader)
        {
            log.LogMethodEntry(rawInputHeader);
            log.LogMethodExit(true);
            return true;
        }

        public virtual bool IsLetterKey(char key)
        {
            log.LogMethodEntry(key);
            log.LogMethodExit(true);
            return true;
        }


        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            log.LogMethodEntry(m);
            switch (m.Msg)
            {
                case USBDevice.WM_INPUT:
                    try
                    {
                        //log.Fatal("Start WM_INPUT");
                        //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
                        if (ProcessRawInputMessage(m.LParam))
                        {
                            //log.Fatal("Got a CardNumber");
                            //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
                            USBDevice.MSG message;
                            USBDevice.PeekMessage(
                               out message,
                                IntPtr.Zero,
                                USBDevice.WM_KEYDOWN,
                                USBDevice.WM_KEYDOWN,
                                USBDevice.PeekMessageRemoveFlag.PM_REMOVE);
                            if (isFinal == true)
                            {
                                //log.Fatal("Read to fire read event");
                                //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
                                isFinal = false;
                                String FinalString = currentReadString;
                                currentReadString = "";
                                this.FireDeviceReadCompleteEvent(FinalString);
                            }

                            //this.filter.FilterNext = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        //log.Fatal("Error in WM_INPUT");
                        //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
                        log.Error("Error occurred while executing WndProc()", ex);
                        MessageBox.Show("USB Card Listener: " + ex.Message);
                        isFinal = false;
                        currentReadString = "";
                    }
                    //log.Fatal("End WM_INPUT");
                    //log.Fatal(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffffK"));
                    break;
            }
            base.WndProc(ref m);
            log.LogMethodExit();
        }

        public override void Dispose()
        {
            log.LogMethodEntry();
            this.ReleaseHandle();
            base.Dispose();
            log.LogMethodExit();
        }
    }

    public sealed class KeyboardWedge32 : USBDevice
    {

        public KeyboardWedge32():base()
        {

        }
        public KeyboardWedge32(DeviceDefinition deviceDefinition):base(deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            log.LogMethodExit();
        }
        /* 24 was originally 16 for 32 bit */

        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWINPUT
        {
            [FieldOffset(0)]
            public RAWINPUTHEADER header;
            [FieldOffset(16)]
            public RAWMOUSE mouse;
            [FieldOffset(16)]
            public RAWKEYBOARD keyboard;
            [FieldOffset(16)]
            public RAWHID hid;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTHEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
            [MarshalAs(UnmanagedType.U4)]
            public int dwSize;
            public IntPtr hDevice;
            /* 32 bit */
            [MarshalAs(UnmanagedType.U4)]
            public int wParam;

            /* 64 bit */
            //public IntPtr wParam;
        }

        #region Keypress methods
        
        /// <summary/>
        /// <param name="key">A base key (no modifiers).</param>
        /// <returns>true if and only if the given key represents a letter.</returns>
        public override bool IsLetterKey(char key)
        {
            log.LogMethodEntry(key);
            log.LogMethodExit(key >= 'A' && key <= 'z');
            return (key >= 'A' && key <= 'z');
        }        
        #endregion

        #region ProcessRawInputMessage(IntPtr rawInputHeader)

        /// <summary>
        /// Process the given WM_INPUT message.
        /// </summary>
        /// <param name="rawInputHeader">the rawInputHeader of the message</param>
        /// <returns>whether or not the keystroke was handled</returns>
        public override bool ProcessRawInputMessage(IntPtr rawInputHeader)
        {
            log.LogMethodEntry(rawInputHeader);
            bool handled = false;
            uint size = 0;
            // First we call GetRawInputData() to set the value of size, which
            // we will the nuse to allocate the appropriate amount of memory in
            // the buffer.
            if (GetRawInputData(
                    rawInputHeader,
                    RID_INPUT,
                    IntPtr.Zero,
                    ref size,
                    (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) == 0)
            {
                IntPtr buffer;
                buffer = Marshal.AllocHGlobal((int)size);
                try
                {
                    if (GetRawInputData(
                            rawInputHeader,
                            RID_INPUT,
                            buffer,
                            ref size,
                            (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) == size)
                    {
                        RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));
                        if (raw.header.dwType == RIM_TYPEKEYBOARD)
                        {
                            uint pcbSize = 0;
                            GetRawInputDeviceInfo(raw.header.hDevice, RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);

                            if (pcbSize > 0)
                            {
                                IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
                                GetRawInputDeviceInfo(raw.header.hDevice, RIDI_DEVICENAME, pData, ref pcbSize);
                                string deviceNameOfEvent = (string)Marshal.PtrToStringAnsi(pData);

                                if (dInfo.deviceName.Contains(deviceNameOfEvent))
                                {
                                    handled = true;

                                    if (raw.keyboard.Message == WM_KEYDOWN)
                                    {
                                        ushort key = raw.keyboard.VKey;
                                        log.LogVariableState("Value of key down: ", key);
                                        char keyBoardChar = Convert.ToChar(key);
                                        log.LogVariableState("Current keyBoardChar: ", keyBoardChar);
                                        byte[] state = new byte[255];
                                        if (GetKeyboardState(state) && (key < 48 || key > 57))
                                        {
                                            uint scanCode = MapVirtualKey(key, MAPVK_VK_TO_VSC);
                                            //IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);
                                            if (inputLocaleIdentifierA == null || inputLocaleIdentifierA == IntPtr.Zero)
                                            {
                                               inputLocaleIdentifierA = LoadKeyboardLayoutA(ENGLISHLOCALEIDENTIFIER, 0);
                                            }
                                            log.LogVariableState("Keyboard layout identifier: ", inputLocaleIdentifierA);
                                            StringBuilder result = new StringBuilder();
                                            if (ToUnicodeEx(key, 
                                                             scanCode, 
                                                             state,
                                                             result, 
                                                             5, 
                                                             0,
                                                             inputLocaleIdentifierA) > 0)
                                            {
                                                char? calcKeyBoardChar = result.ToString()[0];
                                                if (calcKeyBoardChar != null)
                                                {
                                                    keyBoardChar = (char)calcKeyBoardChar;
                                                }
                                            }                                            
                                        }
                                        if ((keyBoardChar == 10 || keyBoardChar == 13) && (isFinal == false))
                                        {
                                            isFinal = true;
                                        }
                                        else
                                        {
                                            if (char.IsControl(keyBoardChar) != true)
                                                currentReadString = currentReadString + keyBoardChar;
                                        }
                                        log.LogVariableState("Current String read: ", currentReadString);
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
            log.LogMethodExit(handled);
            return handled;
        }
        #endregion ProcessRawInputMessage(IntPtr rawInputHeader)
    }

    public sealed class KeyboardWedge64 : USBDevice
    {
        public KeyboardWedge64():base()
        {

        }
        public KeyboardWedge64(DeviceDefinition deviceDefinition):base(deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            log.LogMethodExit();
        }
        /* 24 was originally 16 for 32 bit */

        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWINPUT
        {
            [FieldOffset(0)]
            public RAWINPUTHEADER header;
            [FieldOffset(24)]
            public RAWMOUSE mouse;
            [FieldOffset(24)]
            public RAWKEYBOARD keyboard;
            [FieldOffset(24)]
            public RAWHID hid;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTHEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
            [MarshalAs(UnmanagedType.U4)]
            public int dwSize;
            public IntPtr hDevice;
            /* 32 bit */
            //[MarshalAs(UnmanagedType.U4)]
            //public int wParam;

            /* 64 bit */
            public IntPtr wParam;
        }

        #region Keypress methods

        /// <summary/>
        /// <param name="key">A base key (no modifiers).</param>
        /// <returns>true if and only if the given key represents a letter.</returns>
        public override bool IsLetterKey(char key)
        {
            log.LogMethodEntry(key);
            log.LogMethodExit(key >= 'A' && key <= 'z');
            return (key >= 'A' && key <= 'z');
        }
        #endregion Keypress methods

        #region ProcessRawInputMessage(IntPtr rawInputHeader)

        /// <summary>
        /// Process the given WM_INPUT message.
        /// </summary>
        /// <param name="rawInputHeader">the rawInputHeader of the message</param>
        /// <returns>whether or not the keystroke was handled</returns>
        public override bool ProcessRawInputMessage(IntPtr rawInputHeader)
        {
            log.LogMethodEntry(rawInputHeader);
            bool handled = false;
            uint size = 0;
            // First we call GetRawInputData() to set the value of size, which
            // we will the nuse to allocate the appropriate amount of memory in
            // the buffer.
            if (GetRawInputData(
                    rawInputHeader,
                    RID_INPUT,
                    IntPtr.Zero,
                    ref size,
                    (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) == 0)
            {
                IntPtr buffer;
                buffer = Marshal.AllocHGlobal((int)size);
                try
                {
                    if (GetRawInputData(
                            rawInputHeader,
                            RID_INPUT,
                            buffer,
                            ref size,
                            (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) == size)
                    {
                        RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));
                        if (raw.header.dwType == RIM_TYPEKEYBOARD)
                        {
                            uint pcbSize = 0;
                            GetRawInputDeviceInfo(raw.header.hDevice, RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);

                            if (pcbSize > 0)
                            {
                                IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
                                GetRawInputDeviceInfo(raw.header.hDevice, RIDI_DEVICENAME, pData, ref pcbSize);
                                string deviceNameOfEvent = (string)Marshal.PtrToStringAnsi(pData);

                                if (dInfo.deviceName.Contains(deviceNameOfEvent))
                                {
                                    handled = true;

                                    if (raw.keyboard.Message == WM_KEYDOWN)
                                    {
                                        ushort key = raw.keyboard.VKey;
                                        char keyBoardChar = Convert.ToChar(key);

                                        if ((keyBoardChar == 10 || keyBoardChar == 13) && (isFinal == false))
                                        {
                                            isFinal = true;
                                        }
                                        else
                                        {
                                            if (char.IsControl(keyBoardChar) != true)
                                                currentReadString = currentReadString + keyBoardChar;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
            log.LogMethodExit(handled);
            return handled;
        }
        #endregion ProcessRawInputMessage(IntPtr rawInputHeader)
    }

    public sealed class RawKBDWedge32 : USBDevice
    {

        public RawKBDWedge32():base()
        {

        }
        public RawKBDWedge32(DeviceDefinition deviceDefinition):base(deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            log.LogMethodExit();
        }
        /* 24 was originally 16 for 32 bit */

        [StructLayout(LayoutKind.Explicit)]
        internal struct RAWINPUT
        {
            [FieldOffset(0)]
            public RAWINPUTHEADER header;
            [FieldOffset(16)]
            public RAWMOUSE mouse;
            [FieldOffset(16)]
            public RAWKEYBOARD keyboard;
            [FieldOffset(16)]
            public RAWHID hid;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RAWINPUTHEADER
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
            [MarshalAs(UnmanagedType.U4)]
            public int dwSize;
            public IntPtr hDevice;
            /* 32 bit */
            [MarshalAs(UnmanagedType.U4)]
            public int wParam;

            /* 64 bit */
            //public IntPtr wParam;
        }

        #region ProcessRawInputMessage(IntPtr rawInputHeader)

        /// <summary>
        /// Process the given WM_INPUT message.
        /// </summary>
        /// <param name="rawInputHeader">the rawInputHeader of the message</param>
        /// <returns>whether or not the keystroke was handled</returns>
        public override bool ProcessRawInputMessage(IntPtr rawInputHeader)
        {
            log.LogMethodEntry(rawInputHeader);
            bool handled = false;
            uint size = 0;
            // First we call GetRawInputData() to set the value of size, which
            // we will the nuse to allocate the appropriate amount of memory in
            // the buffer.
            if (GetRawInputData(
                    rawInputHeader,
                    RID_INPUT,
                    IntPtr.Zero,
                    ref size,
                    (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) == 0)
            {
                IntPtr buffer;
                buffer = Marshal.AllocHGlobal((int)size);
                try
                {
                    if (GetRawInputData(
                            rawInputHeader,
                            RID_INPUT,
                            buffer,
                            ref size,
                            (uint)Marshal.SizeOf(typeof(RAWINPUTHEADER))) == size)
                    {
                        RAWINPUT raw = (RAWINPUT)Marshal.PtrToStructure(buffer, typeof(RAWINPUT));
                        if (raw.header.dwType == RIM_TYPEKEYBOARD)
                        {
                            uint pcbSize = 0;
                            GetRawInputDeviceInfo(raw.header.hDevice, RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);

                            if (pcbSize > 0)
                            {
                                IntPtr pData = Marshal.AllocHGlobal((int)pcbSize);
                                GetRawInputDeviceInfo(raw.header.hDevice, RIDI_DEVICENAME, pData, ref pcbSize);
                                string deviceNameOfEvent = (string)Marshal.PtrToStringAnsi(pData);

                                if (dInfo.deviceName.Contains(deviceNameOfEvent))
                                {
                                    handled = true;

                                    if (raw.keyboard.Message == WM_KEYDOWN)
                                    {
                                        StringBuilder localBuffer;
                                        byte[] state;

                                        localBuffer = new StringBuilder(256);
                                        state = new byte[256];

                                        if (GetKeyboardState(state))
                                        {
                                            if (ToUnicode(
                                                        raw.keyboard.VKey,
                                                        raw.keyboard.MakeCode,
                                                        state,
                                                        localBuffer,
                                                        256,
                                                        0) > 0)
                                            {
                                                if (isFinal == false
                                                    && currentReadString.Contains("%B")
                                                    && currentReadString.EndsWith("?\r\n")
                                                    && !currentReadString.EndsWith("?\r\n;"))
                                                {
                                                    isFinal = true;
                                                }
                                                else
                                                {
                                                    currentReadString = currentReadString + localBuffer.ToString();
                                                    isFinal = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
            log.LogMethodExit(handled);
            return handled;
        }
        #endregion ProcessRawInputMessage(IntPtr rawInputHeader)
    }
}
