/********************************************************************************************
 * Project Name - Aloha Integration                                                                          
 * Description  - CEC uses the Aloha for processing the orders from the front counter
 * This class has been developed to integrate with the Aloha. 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        04-Nov-2014   Kiran          Created 
 *1.01        12-Dec-2015   Kiran          Updated to add the logic of handling the "time" products. 
 *                                         As well cleaned up the code and commented it. 
 *1.02        12-Jan-2016   Kiran          Added the function to get the price of the item - This
 *                                         is needed to ensure that the prices in the Parafait system
 *                                         are in sync with what is there in Aloha
 *2.00        04-Oct-2016   Kiran          Compiled the dll so that additional functions are available. 
 *                                         Changed the DISCOVERER_PROCESSED_TENDERID to 115 per
 *                                         Tim Kappell's email on 10/03/2016                            
 *                                  
 *2.10        11-Jul-2017   Mathew         Added logic to copy ITM.DBF file and parse into data table to
 *                                         get token quantity and time value. This change is to avoid referring
 *                                         base ITM.DBF file for every check
 *2.70.2        16-Jul-2019   Deeksha        Added logger methods.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;
using Interop.AlohaFOHLib;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// Class managing the Aloha exception
    /// The exception code is checked and the error message is derived from the code
    /// </summary>
    public class ParafaitAlohaException : Exception
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// ErrCOM_UnauthorizedCOMTerm
        /// </summary>
        public const int ErrCOM_UnauthorizedCOMTerm = 0x0001;
        /// <summary>
        /// ErrCOM_SomeoneAlreadyLoggedIn
        /// </summary>
        public const int ErrCOM_SomeoneAlreadyLoggedIn = 0x0002;
        /// <summary>
        /// ErrCOM_CouldNotFindEmployeeFromId
        /// </summary>
        public const int ErrCOM_CouldNotFindEmployeeFromId = 0x0003;
        /// <summary>
        /// ErrCOM_InvalidEmpPassword
        /// </summary>
        public const int ErrCOM_InvalidEmpPassword = 0x0004;
        /// <summary>
        /// ErrCOM_InvalidMagCard
        /// </summary>
        public const int ErrCOM_InvalidMagCard = 0x0005;
        /// <summary>
        /// ErrCOM_EmpLoggedOnOtherTerm
        /// </summary>
        public const int ErrCOM_EmpLoggedOnOtherTerm = 0x0006;
        /// <summary>
        /// ErrCOM_NoOneLoggedIn
        /// </summary>
        public const int ErrCOM_NoOneLoggedIn = 0x0007;
        /// <summary>
        /// ErrCOM_EmpAlreadyClockedIn
        /// </summary>
        public const int ErrCOM_EmpAlreadyClockedIn = 0x0008;
        /// <summary>
        /// ErrCOM_EmpToManyShiftsToday
        /// </summary>
        public const int ErrCOM_EmpToManyShiftsToday = 0x0009;
        /// <summary>
        /// ErrCOM_EmpInvalidJobcode
        /// </summary>
        public const int ErrCOM_EmpInvalidJobcode = 0x000A;
        /// <summary>
        /// ErrCOM_UnknownEmpClockInError
        /// </summary>
        public const int ErrCOM_UnknownEmpClockInError = 0x000B;
        /// <summary>
        /// ErrCOM_EmpNotClockedIn
        /// </summary>
        public const int ErrCOM_EmpNotClockedIn = 0x000C;
        /// <summary>
        /// ErrCOM_EmpNeedToCheckoutFirst
        /// </summary>
        public const int ErrCOM_EmpNeedToCheckoutFirst = 0x000D;
        /// <summary>
        /// ErrCOM_UnknownEmpClockOutError
        /// </summary>
        public const int ErrCOM_UnknownEmpClockOutError = 0x000E;
        /// <summary>
        /// ErrCOM_NoCheckout
        /// </summary>
        public const int ErrCOM_NoCheckout = 0x000F;
        /// <summary>
        /// ErrCOM_EmpAlreadyCheckedout
        /// </summary>
        public const int ErrCOM_EmpAlreadyCheckedout = 0x00010;
        /// <summary>
        /// ErrCOM_InvalidQueue
        /// </summary>
        public const int ErrCOM_InvalidQueue = 0x00011;
        /// <summary>
        /// ErrCOM_InvalidTable
        /// </summary>
        public const int ErrCOM_InvalidTable = 0x00012;
        /// <summary>
        /// ErrCOM_OpenChecksOnTable
        /// </summary>
        public const int ErrCOM_OpenChecksOnTable = 0x00013;
        /// <summary>
        /// ErrCOM_TableIsClosed
        /// </summary>
        public const int ErrCOM_TableIsClosed = 0x00014;
        /// <summary>
        /// ErrCOM_OnlyOneCheckPerTableAllowed
        /// </summary>
        public const int ErrCOM_OnlyOneCheckPerTableAllowed = 0x00015;
        /// <summary>
        /// ErrCOM_InvalidCheck
        /// </summary>
        public const int ErrCOM_InvalidCheck = 0x00016;
        /// <summary>
        /// ErrCOM_CheckHasUnorderedItems
        /// </summary>
        public const int ErrCOM_CheckHasUnorderedItems = 0x00017;
        /// <summary>
        /// ErrCOM_CheckNotFullyTendered
        /// </summary>
        public const int ErrCOM_CheckNotFullyTendered = 0x00018;
        /// <summary>
        /// ErrCOM_InvalidPayment
        /// </summary>
        public const int ErrCOM_InvalidPayment = 0x00019;
        /// <summary>
        /// ErrCOM_NoLocalPrinter
        /// </summary>
        public const int ErrCOM_NoLocalPrinter = 0x0001A;
        /// <summary>
        /// ErrCOM_InvalidTender
        /// </summary>
        public const int ErrCOM_InvalidTender = 0x0001B;
        /// <summary>
        /// ErrCOM_CheckIsClosed
        /// </summary>
        public const int ErrCOM_CheckIsClosed = 0x0001C;
        /// <summary>
        /// ErrCOM_CheckIsEmpty
        /// </summary>
        public const int ErrCOM_CheckIsEmpty = 0x0001D;
        /// <summary>
        /// ErrCOM_EmpNotAssignedToDrawer
        /// </summary>
        public const int ErrCOM_EmpNotAssignedToDrawer = 0x0001E;
        /// <summary>
        /// ErrCOM_EmpDrawerNotLocal
        /// </summary>
        public const int ErrCOM_EmpDrawerNotLocal = 0x0001F;
        /// <summary>
        /// ErrCOM_TableActiveOnOtherTerm
        /// </summary>
        public const int ErrCOM_TableActiveOnOtherTerm = 0x00020;
        /// <summary>
        /// ErrCOM_InvalidItem
        /// </summary>
        public const int ErrCOM_InvalidItem = 0x00021;
        /// <summary>
        /// ErrCOM_NoEntry
        /// </summary>
        public const int ErrCOM_NoEntry = 0x00022;
        /// <summary>
        /// ErrCOM_InvalidOrderMode
        /// </summary>
        public const int ErrCOM_InvalidOrderMode = 0x00023;
        /// <summary>
        /// ErrCOM_InvalidVoidReason
        /// </summary>
        public const int ErrCOM_InvalidVoidReason = 0x00024;
        /// <summary>
        /// ErrCOM_InvalidEntry
        /// </summary>
        public const int ErrCOM_InvalidEntry = 0x00025;
        /// <summary>
        /// ErrCOM_UnavailableItem
        /// </summary>
        public const int ErrCOM_UnavailableItem = 0x00026;
        /// <summary>
        /// ErrCOM_InvalidModCode
        /// </summary>
        public const int ErrCOM_InvalidModCode = 0x00027;
        /// <summary>
        /// ErrCOM_ModNotAuthForParentItem
        /// </summary>
        public const int ErrCOM_ModNotAuthForParentItem = 0x00028;
        /// <summary>
        /// ErrCOM_ModReqsNotMet
        /// </summary>
        public const int ErrCOM_ModReqsNotMet = 0x00029;
        /// <summary>
        /// ErrCOM_ItemIsNOTOpenItem
        /// </summary>
        public const int ErrCOM_ItemIsNOTOpenItem = 0x0002A;
        /// <summary>
        /// ErrCOM_NeedMoreCCInfo
        /// </summary>
        public const int ErrCOM_NeedMoreCCInfo = 0x0002B;
        /// <summary>
        /// ErrCOM_EmpDrawerNotConfirmed
        /// </summary>
        public const int ErrCOM_EmpDrawerNotConfirmed = 0x0002C;
        /// <summary>
        /// ErrCOM_JobcodeCannotCloseChecks
        /// </summary>
        public const int ErrCOM_JobcodeCannotCloseChecks = 0x0002D;
        /// <summary>
        /// ErrCOM_BadCCTrackInfo
        /// </summary>
        public const int ErrCOM_BadCCTrackInfo = 0x0002E;
        /// <summary>
        /// ErrCOM_IllegalDeclaredTipsAmount
        /// </summary>
        public const int ErrCOM_IllegalDeclaredTipsAmount = 0x0002F;
        /// <summary>
        /// ErrCOM_IllegalDeclaredCashAmount
        /// </summary>
        public const int ErrCOM_IllegalDeclaredCashAmount = 0x00030;
        /// <summary>
        /// ErrCOM_TableNotFound
        /// </summary>
        public const int ErrCOM_TableNotFound = 0x00031;
        /// <summary>
        /// ErrCOM_TableInUse
        /// </summary>
        public const int ErrCOM_TableInUse = 0x00032;
        /// <summary>
        /// ErrCOM_NoLocalCashier
        /// </summary>
        public const int ErrCOM_NoLocalCashier = 0x00033;
        /// <summary>
        /// ErrCOM_EmpCannotTenderCash
        /// </summary>
        public const int ErrCOM_EmpCannotTenderCash = 0x00034;
        /// <summary>
        /// ErrCOM_CheckIsMissingItemsFromReqdCat
        /// </summary>
        public const int ErrCOM_CheckIsMissingItemsFromReqdCat = 0x00035;
        /// <summary>
        /// ErrCOM_CheckHasPendingPayments
        /// </summary>
        public const int ErrCOM_CheckHasPendingPayments = 0x00036;
        /// <summary>
        /// ErrCOM_CheckIsFull
        /// </summary>
        public const int ErrCOM_CheckIsFull = 0x00037;
        /// <summary>
        /// ErrCOM_CouldNotPrintBackOfficeGiftCertificate
        /// </summary>
        public const int ErrCOM_CouldNotPrintBackOfficeGiftCertificate = 0x00038;
        /// <summary>
        /// ErrCOM_ToManyChecksOnTable
        /// </summary>
        public const int ErrCOM_ToManyChecksOnTable = 0x00039;
        /// <summary>
        /// ErrCOM_EntryAlreadySelected
        /// </summary>
        public const int ErrCOM_EntryAlreadySelected = 0x0003A;
        /// <summary>
        /// ErrCOM_InvalidSpecialMessage
        /// </summary>
        public const int ErrCOM_InvalidSpecialMessage = 0x0003B;
        /// <summary>
        /// ErrCOM_InvalidMenu
        /// </summary>
        public const int ErrCOM_InvalidMenu = 0x0003C;
        /// <summary>
        /// ErrCOM_NoPivotSeating
        /// </summary>
        public const int ErrCOM_NoPivotSeating = 0x0003D;
        /// <summary>
        /// ErrCOM_TooManySeats
        /// </summary>
        public const int ErrCOM_TooManySeats = 0x0003E;
        /// <summary>
        /// ErrCOM_NotSupportedInQuickService
        /// </summary>
        public const int ErrCOM_NotSupportedInQuickService = 0x00040;
        /// <summary>
        /// ErrCOM_NotSupportedInTableService
        /// </summary>
        public const int ErrCOM_NotSupportedInTableService = 0x00041;
        /// <summary>
        /// ErrCOM_CannotAdjustFinalizedGiftCard
        /// </summary>
        public const int ErrCOM_CannotAdjustFinalizedGiftCard = 0x00042;
        /// <summary>
        /// ErrCOM_FinalizingGiftCard
        /// </summary>
        public const int ErrCOM_FinalizingGiftCard = 0x00043;
        /// <summary>
        /// ErrCOM_CheckIsFromPreviousShift
        /// </summary>
        public const int ErrCOM_CheckIsFromPreviousShift = 0x00044;
        /// <summary>
        /// ErrCOM_EmployeeIsCheckedOut
        /// </summary>
        public const int ErrCOM_EmployeeIsCheckedOut = 0x00045;
        /// <summary>
        /// ErrCOM_InvalidExceptionModifierGroup
        /// </summary>
        public const int ErrCOM_InvalidExceptionModifierGroup = 0x00046;
        /// <summary>
        /// ErrCOM_InvalidExceptionModifierGroup
        /// </summary>
        public const int ErrCOM_NotInExceptionModifierGroup = 0x00047;
        /// <summary>
        /// ErrCOM_NoEntriesSelected
        /// </summary>
        public const int ErrCOM_NoEntriesSelected = 0x00048;
        /// <summary>
        /// ErrCOM_NoEntriesMoved
        /// </summary>
        public const int ErrCOM_NoEntriesMoved = 0x00049;
        /// <summary>
        /// ErrCOM_InvalidCompType
        /// </summary>
        public const int ErrCOM_InvalidCompType = 0x0004A;
        /// <summary>
        /// ErrCOM_InvalidPromotion
        /// </summary>
        public const int ErrCOM_InvalidPromotion = 0x0004B;
        /// <summary>
        /// ErrCOM_InvalidComp
        /// </summary>
        public const int ErrCOM_InvalidComp = 0x0004C;
        /// <summary>
        /// ErrCOM_InvalidPromo
        /// </summary>
        public const int ErrCOM_InvalidPromo = 0x0004D;
        /// <summary>
        /// ErrCOM_FOHCOM_CurrentlyBusy
        /// </summary>
        public const int ErrCOM_FOHCOM_CurrentlyBusy = 0x0004E;
        /// <summary>
        /// ErrCOM_FOHCOM_ServerException
        /// </summary>
        public const int ErrCOM_FOHCOM_ServerException = 0x0004F;
        /// <summary>
        /// ErrCOM_CheckHasPaymentsPendingSignatureVerification
        /// </summary>
        public const int ErrCOM_CheckHasPaymentsPendingSignatureVerification = 0x00050;
        /// <summary>
        /// ErrCOM_InvalidParameter
        /// </summary>
        public const int ErrCOM_InvalidParameter = 0x00051;
        /// <summary>
        /// ErrCOM_eFrequency_InvalidRequest
        /// </summary>
        public const int ErrCOM_eFrequency_InvalidRequest = 0x00052;
        /// <summary>
        /// ErrCOM_eFrequency_InvalidCardNumber
        /// </summary>
        public const int ErrCOM_eFrequency_InvalidCardNumber = 0x00053;
        /// <summary>
        /// ErrCOM_eFrequency_InvalidPrefix
        /// </summary>
        public const int ErrCOM_eFrequency_InvalidPrefix = 0x00054;
        /// <summary>
        /// ErrCOM_eFrequency_RequireManagerOverride
        /// </summary>
        public const int ErrCOM_eFrequency_RequireManagerOverride = 0x00055;
        /// <summary>
        /// ErrCOM_eFrequency_UnknownError
        /// </summary>
        public const int ErrCOM_eFrequency_UnknownError = 0x00056;
        /// <summary>
        /// ErrCOM_ManagerNotClockedIn
        /// </summary>
        public const int ErrCOM_ManagerNotClockedIn = 0x00057;
        /// <summary>
        /// ErrCOM_IllegalBreakTypeForShift
        /// </summary>
        public const int ErrCOM_IllegalBreakTypeForShift = 0x00058;
        /// <summary>
        /// ErrCOM_IllegalComboOfBreakAndPaidForShift
        /// </summary>
        public const int ErrCOM_IllegalComboOfBreakAndPaidForShift = 0x00059;
        /// <summary>
        /// ErrCOM_EmployeeCannotBreakWithOpenTables
        /// </summary>
        public const int ErrCOM_EmployeeCannotBreakWithOpenTables = 0x0005A;
        /// <summary>
        /// ErrCOM_EmployeeOnBreak
        /// </summary>
        public const int ErrCOM_EmployeeOnBreak = 0x0005B;
        /// <summary>
        /// ErrCOM_InvalidBreakTypeForEmployee
        /// </summary>
        public const int ErrCOM_InvalidBreakTypeForEmployee = 0x0005C;
        /// <summary>
        /// ErrCOM_EmployeeIsNotOnABreak
        /// </summary>
        public const int ErrCOM_EmployeeIsNotOnABreak = 0x0005D;
        /// <summary>
        /// ErrCOM_EnforcedBreakTimesNotMet
        /// </summary>
        public const int ErrCOM_EnforcedBreakTimesNotMet = 0x0005E;
        /// <summary>
        /// ErrCOM_PasswordHasNonNumericChars
        /// </summary>
        public const int ErrCOM_PasswordHasNonNumericChars = 0x0005F;
        /// <summary>
        /// ErrCOM_SystemNotSetupToUsePasswords
        /// </summary>
        public const int ErrCOM_SystemNotSetupToUsePasswords = 0x00060;
        /// <summary>
        /// ErrCOM_PasswordIsLessThanMinLength
        /// </summary>
        public const int ErrCOM_PasswordIsLessThanMinLength = 0x00061;
        /// <summary>
        /// ErrCOM_PasswordIsGreaterThanMaxLength
        /// </summary>
        public const int ErrCOM_PasswordIsGreaterThanMaxLength = 0x00062;
        /// <summary>
        /// ErrCOM_EmpIsMagcardOnly
        /// </summary>
        public const int ErrCOM_EmpIsMagcardOnly = 0x00063;
        /// <summary>
        /// ErrCOM_ManagerMustClockout
        /// </summary>
        public const int ErrCOM_ManagerMustClockout = 0x00064;
        /// <summary>
        /// ErrCOM_MagcardAlreadyInUse
        /// </summary>
        public const int ErrCOM_MagcardAlreadyInUse = 0x00065;
        /// <summary>
        /// ErrCOM_NoClearPasswordRights
        /// </summary>
        public const int ErrCOM_NoClearPasswordRights = 0x00066;
        /// <summary>
        /// ErrCOM_EmployeeIsTerminated
        /// </summary>
        public const int ErrCOM_EmployeeIsTerminated = 0x00067;
        /// <summary>
        /// ErrCOM_EmployeeIsLockedOnAnotherTerminal
        /// </summary>
        public const int ErrCOM_EmployeeIsLockedOnAnotherTerminal = 0x00068;
        /// <summary>
        /// ErrCOM_EmpCurrentlyClockedIn
        /// </summary>
        public const int ErrCOM_EmpCurrentlyClockedIn = 0x00069;
        /// <summary>
        /// ErrCOM_EmpCannotDeleteClockoutEmpHasNoShift
        /// </summary>
        public const int ErrCOM_EmpCannotDeleteClockoutEmpHasNoShift = 0x0006A;
        /// <summary>
        /// ErrCOM_EmpNotScheduledToWork
        /// </summary>
        public const int ErrCOM_EmpNotScheduledToWork = 0x0006B;
        /// <summary>
        /// ErrCOM_EmpCannotWorkAnotherShift
        /// </summary>
        public const int ErrCOM_EmpCannotWorkAnotherShift = 0x0006C;
        /// <summary>
        /// ErrCOM_EmpCannotClockOutYet
        /// </summary>
        public const int ErrCOM_EmpCannotClockOutYet = 0x0006D;
        /// <summary>
        /// ErrCOM_CannotFindDrawerForEmployee
        /// </summary>
        public const int ErrCOM_CannotFindDrawerForEmployee = 0x0006E;
        /// <summary>
        /// ErrCOM_DrawerCurrentlyAssignedToAnotherEmployee
        /// </summary>
        public const int ErrCOM_DrawerCurrentlyAssignedToAnotherEmployee = 0x0006F;
        /// <summary>
        /// ErrCOM_EmpsPasswordWasClearedButPasswordProvided
        /// </summary>
        public const int ErrCOM_EmpsPasswordWasClearedButPasswordProvided = 0x00070;
        /// <summary>
        /// ErrCOM_EmployeeIsCheckedIn
        /// </summary>
        public const int ErrCOM_EmployeeIsCheckedIn = 0x00071;
        /// <summary>
        /// ErrCOM_NeedManagerToCheckoutEmp
        /// </summary>
        public const int ErrCOM_NeedManagerToCheckoutEmp = 0x00072;
        /// <summary>
        /// ErrCOM_NoDeleteClockoutRights
        /// </summary>
        public const int ErrCOM_NoDeleteClockoutRights = 0x00073;
        /// <summary>
        /// ErrCOM_NoDeleteCheckoutRights
        /// </summary>
        public const int ErrCOM_NoDeleteCheckoutRights = 0x00074;
        /// <summary>
        /// ErrCOM_InvalidResetPasswordState
        /// </summary>
        public const int ErrCOM_InvalidResetPasswordState = 0x00075;
        /// <summary>
        /// ErrCOM_SetExpiredPasswordRequired
        /// </summary>
        public const int ErrCOM_SetExpiredPasswordRequired = 0x00076;
        /// <summary>
        /// ErrCOM_SetClearedPasswordRequired
        /// </summary>
        public const int ErrCOM_SetClearedPasswordRequired = 0x00077;
        /// <summary>
        /// ErrCOM_SetUninitPasswordRequired
        /// </summary>
        public const int ErrCOM_SetUninitPasswordRequired = 0x00078;
        /// <summary>
        /// ErrCOM_RequiresMgrOverride
        /// </summary>
        public const int ErrCOM_RequiresMgrOverride = 0x00079;
        /// <summary>
        /// ErrCOM_InsufficientAccessLevel
        /// </summary>
        public const int ErrCOM_InsufficientAccessLevel = 0x00080;
        /// <summary>
        /// ErrCOM_InvalidRequest
        /// </summary>
        public const int ErrCOM_InvalidRequest = 0x00081;
        /// <summary>
        /// ErrCOM_UnableToReopenCheckToEmployee
        /// </summary>
        public const int ErrCOM_UnableToReopenCheckToEmployee = 0x00082;
        /// <summary>
        /// ErrCOM_CheckHasAnOverPayment
        /// </summary>
        public const int ErrCOM_CheckHasAnOverPayment = 0x00083; // RFC 058888
        /// <summary>
        /// ErrCOM_InvalidGetCheckContext
        /// </summary>
        public const int ErrCOM_InvalidGetCheckContext = 0x00084;
        /// <summary>
        /// ErrCOM_OperationFailed
        /// </summary>
        public const int ErrCOM_OperationFailed = 0x00085;
        /// <summary>
        /// ErrCOM_InvalidOrderNumber
        /// </summary>
        public const int ErrCOM_InvalidOrderNumber = 0x00086;
        /// <summary>
        /// ErrCOM_DriverMileageRequired
        /// </summary>
        public const int ErrCOM_DriverMileageRequired = 0x00087;
        /// <summary>
        /// ErrCOM_TerminalNotFound
        /// </summary>
        public const int ErrCOM_TerminalNotFound = 0x00088;
        /// <summary>
        /// ErrCOM_TerminalNotAnRIT
        /// </summary>
        public const int ErrCOM_TerminalNotAnRIT = 0x00089;
        /// <summary>
        /// ErrCOM_InvalidRITHost
        /// </summary>
        public const int ErrCOM_InvalidRITHost = 0x00090;
        /// <summary>
        /// ErrCOM_MaxNumberOfVirtualTerminals
        /// </summary>
        public const int ErrCOM_MaxNumberOfVirtualTerminals = 0x00091;
        /// <summary>
        /// ErrCOM_DefaultQueueNotDefined
        /// </summary>
        public const int ErrCOM_DefaultQueueNotDefined = 0x00092;
        /// <summary>
        /// ErrCOM_SourceOrTargetSeatNotAvailable
        /// </summary>
        public const int ErrCOM_SourceOrTargetSeatNotAvailable = 0x00093;
        /// <summary>
        /// ErrCOM_SplitSeatAtleastOneSeatRequired
        /// </summary>
        public const int ErrCOM_SplitSeatAtleastOneSeatRequired = 0x00094;
        /// <summary>
        /// ErrCOM_InactiveHouseAccount
        /// </summary>
        public const int ErrCOM_InactiveHouseAccount = 0x00095;
        /// <summary>
        /// ErrCOM_HouseAccountNotFound
        /// </summary>
        public const int ErrCOM_HouseAccountNotFound = 0x00096;
        /// <summary>
        /// ErrCOM_MembershipLiquorPoolException
        /// </summary>
        public const int ErrCOM_MembershipLiquorPoolException = 0x00097;
        /// <summary>
        /// ErrCOM_NoMembershipChargePrivileges
        /// </summary>
        public const int ErrCOM_NoMembershipChargePrivileges = 0x00098;
        /// <summary>
        /// ErrCOM_MembershipAccountExpired
        /// </summary>
        public const int ErrCOM_MembershipAccountExpired = 0x00099;
        /// <summary>
        /// ErrCOM_MemberNotFound
        /// </summary>
        public const int ErrCOM_MemberNotFound = 0x00100;
        /// <summary>
        /// ErrCOM_ConfigurationError
        /// </summary>
        public const int ErrCOM_ConfigurationError = 0x00101;
        /// <summary>
        /// ErrCOM_InvalidGiftCard
        /// </summary>
        public const int ErrCOM_InvalidGiftCard = 0x00102;
        /// <summary>
        /// ErrCOM_PMSInactive
        /// </summary>
        public const int ErrCOM_PMSInactive = 0x00103;
        /// <summary>
        /// ErrCOM_NotAPMSPayment
        /// </summary>
        public const int ErrCOM_NotAPMSPayment = 0x00104;
        /// <summary>
        /// ErrCOM_PMSPaymentIdRequired
        /// </summary>
        public const int ErrCOM_PMSPaymentIdRequired = 0x00105;
        /// <summary>
        /// ErrCOM_EmpCannotDeletePayment
        /// </summary>
        public const int ErrCOM_EmpCannotDeletePayment = 0x00106;
        /// <summary>
        /// ErrCOM_EDCInactive
        /// </summary>
        public const int ErrCOM_EDCInactive = 0x00107;
        /// <summary>
        /// ErrCOM_DrawerNotFound
        /// </summary>
        public const int ErrCOM_DrawerNotFound = 0x00108;
        /// <summary>
        /// ErrCOM_ModLevelMaxReached
        /// </summary>
        public const int ErrCOM_ModLevelMaxReached = 0x0010A; // RFC 67163
        /// <summary>
        /// ErrCOM_InvalidScreenHandle
        /// </summary>
        public const int ErrCOM_InvalidScreenHandle = 0x0010B; // RFC 72101
        /// <summary>
        /// ErrCOM_InvalidButtonFunc
        /// </summary>
        public const int ErrCOM_InvalidButtonFunc = 0x0010C; // RFC 72101
        /// <summary>
        /// ErrCOM_RecallOrderNotAllowed
        /// </summary>
        public const int ErrCOM_RecallOrderNotAllowed = 0x00110;
        /// <summary>
        /// ErrCOM_GiftCardRestrictionViolation
        /// </summary>
        public const int ErrCOM_GiftCardRestrictionViolation = 0x00111; //RFC 62456
        /// <summary>
        /// ErrCOM_TenderIDAndCardNumberDontMatch
        /// </summary>
        public const int ErrCOM_TenderIDAndCardNumberDontMatch = 0x00112; //RFC 80700
        /// <summary>
        /// ErrCOM_NoPaymentAtNonTenderingTerminal
        /// </summary>
        public const int ErrCOM_NoPaymentAtNonTenderingTerminal = 0x00113; //RFC 81972
        /// <summary>
        /// ErrCOM_NoCloseCheckAtNonTenderingTerminal
        /// </summary>
        public const int ErrCOM_NoCloseCheckAtNonTenderingTerminal = 0x00114; //RFC 81972
        /// <summary>
        /// ErrCOM_StaticEmployeeClockInOtherTerminal
        /// </summary>
        public const int ErrCOM_StaticEmployeeClockInOtherTerminal = 0x00115; //RFC 81972
        /// <summary>
        /// ErrCOM_NoStaicEmployeeClockOut
        /// </summary>
        public const int ErrCOM_NoStaicEmployeeClockOut = 0x00116; //RFC 81972
        /// <summary>
        /// ErrCOM_EmpHasOpenCheck
        /// </summary>
        public const int ErrCOM_EmpHasOpenCheck = 0x00117;
        /// <summary>
        /// ErrCOM_GiftCardRedeemptionViolation
        /// </summary>
        public const int ErrCOM_GiftCardRedeemptionViolation = 0x00119; // RFC 

        /// <summary>
        /// Constructor creating the exception
        /// Populates the Aloha message based on the exception code
        /// This is done so that readable message can be shown to the customer
        /// </summary>
        public ParafaitAlohaException(Exception ex) : base(AlohaMessageDecode(ex))
        {
            log.LogMethodEntry();
            HResult = Marshal.GetHRForException(ex);
            log.LogMethodExit();
        }
        private static String AlohaMessageDecode(Exception ex)
        {
            log.LogMethodEntry();
            int exceptionCode = Marshal.GetHRForException(ex);


            Dictionary<int, string> AlohaErrorMessages = new Dictionary<int, string>()
                {
                    { ErrCOM_UnauthorizedCOMTerm, "Unauthorized COM Terminal" },
                    { ErrCOM_SomeoneAlreadyLoggedIn, "Some one has already logged into the terminal. Please check." },
                    { ErrCOM_CouldNotFindEmployeeFromId, "Please check employee number, could not find the employee." },
                    { ErrCOM_InvalidEmpPassword, "Password entered is invalid. Please check." },
                    { ErrCOM_InvalidMagCard, "Magcard swiped is invalid. Please check." },
                    { ErrCOM_EmpLoggedOnOtherTerm, "Employee is logged onto another terminal. Please check." },
                    { ErrCOM_NoOneLoggedIn, "No one has logged into the terminal, please login before any operation." },
                    { ErrCOM_EmpAlreadyClockedIn, "Employee has already clocked-in, cannot clock-in again." },
                    { ErrCOM_EmpToManyShiftsToday, "Too many shifts today." },
                    { ErrCOM_EmpInvalidJobcode, "Job code is invalid. Please check." },
                    { ErrCOM_UnknownEmpClockInError, "Unknown employee is being clocked in. Please check." },
                    { ErrCOM_EmpNotClockedIn, "Employee has not clocked in. Please clock-in" },
                    { ErrCOM_EmpNeedToCheckoutFirst, "Employee needs to checkout first" },
                    { ErrCOM_UnknownEmpClockOutError, "Unknown employee is being clocked-out. Please check." },
                    { ErrCOM_NoCheckout, "No checkout" },
                    { ErrCOM_EmpAlreadyCheckedout, "Employee already checked out." },
                    { ErrCOM_InvalidQueue, "Invalid queue" },
                    { ErrCOM_InvalidTable, "Invalid table" },
                    { ErrCOM_OpenChecksOnTable, "There are open checks on the table" },
                    { ErrCOM_TableIsClosed, "Table is closed" },
                    { ErrCOM_OnlyOneCheckPerTableAllowed, "Only one check is allowed per table" },
                    { ErrCOM_InvalidCheck, "Invalid check" },
                    { ErrCOM_CheckHasUnorderedItems, "Check has unordered items. Please order the items." },
                    { ErrCOM_CheckNotFullyTendered, "Check is not fully tendered" },
                    { ErrCOM_InvalidPayment, "Invalid payment" },
                    { ErrCOM_InvalidTender, "Invalid tender" },
                    { ErrCOM_CheckIsClosed, "Check is closed" },
                    { ErrCOM_CheckIsEmpty, "Check is empty" },
                    { ErrCOM_EmpNotAssignedToDrawer, "Employee not assigned to drawer" },
                    { ErrCOM_EmpDrawerNotLocal, "Employee drawer is not local" },
                    { ErrCOM_TableActiveOnOtherTerm, "Table is active on another terminal." },
                    { ErrCOM_InvalidItem, "Invalid item. Please check." },
                    { ErrCOM_NoEntry, "No entry" },
                    { ErrCOM_InvalidOrderMode, "Invalid order mode" },
                    { ErrCOM_InvalidVoidReason, "Invalid void reason" },
                    { ErrCOM_InvalidEntry, "Invalid entry" },
                    { ErrCOM_UnavailableItem, "Item is unavailable" },
                    { ErrCOM_InvalidModCode, "Invalid modification code" },
                    { ErrCOM_ModNotAuthForParentItem, "Modification to the parent item is not allowed.." },
                    { ErrCOM_CheckHasAnOverPayment, "Check has an overpayment" }
                };

            int finalExcepCode = exceptionCode & 0xFFF;
            long topBytes = (exceptionCode >> 12) & 0xFFFFF;
            String value;
            bool hasValue = AlohaErrorMessages.TryGetValue(finalExcepCode, out value);
            if (topBytes != 0xC0068)
                return ex.Message;
            if (hasValue)
                return value;
            else
                return ex.Message;
        }
    }

    /// <summary>
    /// Class handling the integration with Aloha
    /// The integration has been developed using the COM interface provided by NCR/Radiant
    /// 
    /// There are two basic functions
    /// a. Creating a check - So that external POS systems are able to post the check into Aloha. 
    /// This involves number of steps like opening check, add items, making payment etc
    /// b. Reading a check - This is for the Parafait application to get the check details from Aloha
    /// Here the application is looking for items with tokens or time on it so that cards can be issued
    /// with the tokens/time. 
    /// </summary>
    public class ParafaitAlohaIntegrator
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int FILE_ACC = 1;     // access codes
        private const int FILE_CAT = 2;     // categories
        private const int FILE_CIT = 3;     // categories/items
        private const int FILE_CMG = 4;     // clock in messages
        private const int FILE_CMP = 5;     // comp types
        private const int FILE_DWR = 6;     // cash drawers
        private const int FILE_EMP = 7;     // employees
        private const int FILE_EXC = 8;     // exception modifier groups
        private const int FILE_GCI = 9;     // guest check information
        private const int FILE_GIF = 10;    // gift certificates
        private const int FILE_HSE = 11;    // house accounts
        private const int FILE_ITM = 12;    // items
        private const int FILE_JOB = 13;    // job codes
        private const int FILE_LAB = 14;    // labor categories
        private const int FILE_MNU = 15;    // menus
        private const int FILE_MOD = 16;    // modifiers
        private const int FILE_ODR = 17;    // order modes
        private const int FILE_PRD = 18;    // periods
        private const int FILE_PRF = 19;    // performance categories
        private const int FILE_PRG = 20;    // printer groups
        private const int FILE_PRO = 21;    // promotions
        private const int FILE_PRT = 22;    // printers
        private const int FILE_REV = 23;    // revenue centers
        private const int FILE_RSN = 24;    // void reasons
        private const int FILE_SCH = 25;    // labor scheduling
        private const int FILE_SHF = 26;    // shift file
        private const int FILE_SMG = 27;    // server information messages
        private const int FILE_SUB = 28;    // submenus
        private const int FILE_SYS = 29;    // system parameters (defined in ini.h)
        private const int FILE_TAB = 30;    // tables
        private const int FILE_TAX = 31;    // taxes
        private const int FILE_TDR = 32;    // tenders
        private const int FILE_TRM = 33;    // terminals
        private const int FILE_VAL = 34;    // valid tender idents
        private const int FILE_GITEM = 35;  // item grinds
        private const int FILE_GVOID = 36;   // void grinds
        private const int FILE_GTIME = 37;   // time and attendance grinds
        private const int FILE_GSALE = 38;   // sales grinds
        private const int FILE_GTNDR = 39;   // tender grinds
        private const int FILE_GREVN = 40;   // revenue center summary
        private const int FILE_GTURN = 41;   // table turn summary
        private const int FILE_SITEM = 42;   // item summary
        private const int FILE_SVOID = 43;   // void summary
        private const int FILE_STIME = 44;   // time and attendance summary
        private const int FILE_SSALE = 45;   // sales summary
        private const int FILE_STNDR = 46;   // tender summary
        private const int FILE_SREVN = 47;   // revenue center summary
        private const int FILE_STURN = 48;   // table turn summary
        private const int FILE_MSG = 49;   // generic message file
        private const int FILE_HST = 50;   // host INI file
        private const int FILE_PTP = 51;   // printer type file
        private const int FILE_PET = 52;   // petty cash account file
        private const int FILE_SUR = 53;   // surcharge file
        private const int FILE_ZAP = 54;   // employee termination reasons
        private const int FILE_VID = 55;   // video monitors
        private const int FILE_RPT = 56;   // report options (.ini file)
        private const int FILE_ADJ = 57;   // adjusted time and attendance
        private const int FILE_OVR = 58;   // overtime (S&A only!)
        private const int FILE_ECITEM = 59;   // El Chico menu item export file
        private const int FILE_ECTIME = 60;   // El Chico labor export file
        private const int FILE_ECSALE = 61;   // El Chico sales export file
        private const int FILE_GLINE = 62;   // line item grinds
        private const int FILE_GCLOG = 63;   // gift certificate activity grinds
        private const int FILE_DEL = 64;   // delivery fields
        private const int FILE_CST = 65;   // delivery customers
        private const int FILE_DMG = 66;   // delivery messages
        private const int FILE_GADJP = 67;   // Adjust Pay Grinds
        private const int FILE_GTRAN = 68;   // Transfer Table Grinds
        private const int FILE_FF = 69;   // Fast Food Settings (INI)
        private const int FILE_VGP = 70;   // Video Groups
        private const int FILE_BTN = 75;   // Buttons
        private const int FILE_PNL = 76;  // Panels
        private const int FILE_QUE = 77;   // Order Queues
        private const int FILE_PIT = 78;   // Prep Items
        private const int FILE_IPI = 79;   // Items to Prep Items
        private const int FILE_PRP = 80;   // Prep Schedule
        private const int FILE_SCR = 82;   // Screens
        private const int FILE_RTL = 83;   // Routing levels
        private const int FILE_GKVI = 84;   // Key volume indicator grinds
        private const int FILE_VOL = 85;   // Volume Levels
        private const int FILE_PRL = 86;   // Price Levels
        private const int FILE_GBRK = 87;   // Break grind
        private const int FILE_AMNU = 88;   // auto menus
        private const int FILE_AMNUD = 89;   // auto menu detail
        private const int FILE_MEMB = 90;   // club members
        private const int FILE_TRK = 91;   // Track Items
        private const int FILE_CTK = 92;   // Composite Track Items
        private const int FILE_RCP = 93;   // Composite Recipes
        private const int FILE_SQLINI = 94;   // SQL system parameters
        private const int FILE_GPERF = 95;   // perfs grinds
        private const int FILE_SMCRD = 96;   // smart card devices
        private const int FILE_DNK = 97;   // drink dispensers
        private const int FILE_PC = 98;   // price changes
        private const int FILE_PCID = 99;   // price change item details
        private const int FILE_OCC = 100;  // Occasions
        private const int FILE_OCT = 101;  // Occasion Categories
        private const int FILE_PCPLD = 102;  // price changes amount level details
        private const int FILE_GQKCNT = 103;  // quick count grinds
        private const int FILE_SCALE = 104;  // scales (for weiging)
        private const int FILE_TARE = 105;  // scales (for weiging)
        private const int FILE_QTYPRICE = 106;  // scales (for weiging)
        private const int FILE_FIXPRICE = 107;  // scales (for weiging)
        private const int FILE_MULTCURR = 108;	 // Multiple Currencies
        private const int FILE_PRJT = 109;	 // Prep Projections for Track Items
        private const int FILE_PRJC = 110;	 // Prep Projections for Composite Track Items
        private const int FILE_GRINDINI = 111;	 // Grind INI file settings
        private const int FILE_CORPLEVL = 112;	 // Grind INI file settings
        private const int FILE_REGRINDDETAIL = 113;	 // regrind details
        private const int FILE_LABEL = 114;	 // labels
        private const int FILE_STOREEDIT = 115;	 // store edits - Enterprise I & II
        private const int FILE_CORPEVENT = 119;	 // Corporate Event
        private const int FILE_CORPEVENT_SUBSCRIBER = 120; // Corporate Event Subscribers
        private const int FILE_STOREGROUP = 121;	// Store Groups
        private const int FILE_STOREGROUP_MEMBERS = 122; // Store Group Members
        private const int FILE_FLEXTAX = 123; // Flex Taxes
        private const int FILE_POLLEVENT = 124; // poll event
        private const int FILE_POLLEVENTFILE = 125; // poll event files
        private const int FILE_POLLEVENTSUB = 126; // poll event subscribers
        private const int FILE_TABLEDEF = 127; // table definition
        private const int FILE_DISPLAY_BOARD = 128;	// Display board
        private const int FILE_TEAM_SERVICE = 129; // Team Service Tip Sharing File
        private const int FILE_LABREVCTR = 130; // Labor By Revenue Center
        private const int FILE_LABDAYPART = 131; // Labor By Day Part
        private const int FILE_LABLABCAT = 132; // Labor By Labor Category
        private const int FILE_VOIDREV = 133; // voids by revenue center and day part
        private const int FILE_SURVEY = 134;	// surveys
        private const int FILE_LABREVDETAIL = 135; // Labor By Revenue Center and Day Part
        private const int FILE_PCPD = 136; // Promo price change detail
        private const int FILE_VDV = 137; // video devices
        private const int FILE_VKP = 138; // video keypads
        private const int FILE_VQU = 139; // video queues
        private const int FILE_TRDTX = 140; // Tiered taxes RFC 10613
        private const int FILE_GCHKINFO = 141;	// Grind Check Info
        private const int FILE_CNCPT = 142;	// Concepts
        private const int FILE_OTHERWAGES = 143; // Other Wages
        private const int FILE_GND_OTHERWAGES = 144; // Gnd Other Wages
        private const int FILE_GND_OTHERWAGE_EDITS = 145; // Gnd Other Wage Edits (audit trail)

        private const int FILE_JBRV = 199;   // Job Code by Revenue ctr
        private const int FILE_STOR = 200;   // Stores
        private const int FILE_REGN = 201;   // Regions
        private const int FILE_SQLRPT = 202;   // sql reports
        private const int FILE_SQLRPTPM = 203;   // sql report parms
        private const int FILE_CASHCARD = 204;  // cash card
        private const int FILE_NOSALERSN = 205;	 // No sale reasons
        private const int FILE_VER = 206;	 // System versions
        private const int FILE_EMPI9 = 207;	 // Employee I9 information
        private const int FILE_SECLVL = 208;	 // Security level info
        private const int FILE_SECLVLDTL = 209;	 // Security Level detail info
        private const int FILE_ITMGRP = 210;  // Item Group
        private const int FILE_ITMGRPLIST = 211;  // Item Group List
        private const int FILE_SPYCOMMDEVICE = 212;	 // AlohaSpy - Spy COMM Device
        private const int FILE_SPYDEVICE = 213;	 // AlohaSpy - Spy Device
        private const int FILE_SPYTERMINAL = 214;	 // AlohaSpy - Spy Terminal
        private const int FILE_PRINTDATA = 215;	 // Thumb Print

        // Data integration - American Labor Scheduler RFC 12606
        private const int FILE_AVAILABILITY = 216;	 // Employee availability for scheduling purposes
        private const int FILE_WORKPOLICY = 217;
        private const int FILE_SPECIALREQUEST = 218;
        private const int FILE_SCHEDULESHIFT = 219;
        private const int FILE_SCHEDULESHIFTTAG = 220;
        private const int FILE_TASKTYPE = 221;
        private const int FILE_TASK = 222;
        private const int FILE_GNDSALESSUMMARY = 223;
        private const int FILE_GNDSALESPROJECTION = 224;
        private const int FILE_GNDLABORPROJECTION = 225;
        private const int FILE_LABORPERIOD = 226;
        private const int FILE_LABORPERIODTIME = 227;
        private const int FILE_REQUIREMENTRULE = 228;

        private const int FILE_OPENDRAWER = 229;	 // Open Drawer


        /// These are private const int s for FOH COM "datatypes" DO NOT allow the FILE_ types above to "overrun" these values!! FTI
        private const int INTERNAL_EMPLOYEES = 500;
        private const int INTERNAL_EMP_OPEN_TABLES = 501;
        private const int INTERNAL_EMP_CLOSED_TABLES = 502;
        private const int INTERNAL_EMP_SHARED_TABLES = 503;
        private const int INTERNAL_EMP_BARTAB_TABLES = 504;
        private const int INTERNAL_EMP_CARRYOVER_TABLES = 505;
        private const int INTERNAL_EMP_CLOSED_CHECKS = 506;
        private const int INTERNAL_EMP_SHIFT0 = 507;
        private const int INTERNAL_EMP_SHIFT1 = 508;
        private const int INTERNAL_EMP_SHIFT2 = 509;
        private const int INTERNAL_EMP_SHIFT3 = 510;
        private const int INTERNAL_EMP_SHIFT4 = 511;
        private const int INTERNAL_EMP_SHIFT5 = 512;
        private const int INTERNAL_EMP_SHIFT6 = 513;
        private const int INTERNAL_EMP_SHIFT7 = 514;
        private const int INTERNAL_EMP_SHIFT8 = 515;
        private const int INTERNAL_EMP_SHIFT9 = 516;

        private const int INTERNAL_TABLES = 520;	// Users canNOT call for this struct directly!
        private const int INTERNAL_TABLES_OWNER_EMP = 521;
        private const int INTERNAL_TABLES_FROM_EMP = 522;
        private const int INTERNAL_TABLES_TO_EMP = 523;
        private const int INTERNAL_TABLES_ORIG_FROM_EMP = 524;
        private const int INTERNAL_TABLES_DRIVER = 525;
        private const int INTERNAL_TABLES_CHECKS = 526;
        private const int INTERNAL_TABLES_ENTRIES = 527;

        private const int INTERNAL_CHECKS = 540;		// Users canNOT call for this struct directly!
        private const int INTERNAL_CHECKS_TIPPABLE_EMP = 541;
        private const int INTERNAL_CHECKS_ENTRIES = 542;
        private const int INTERNAL_CHECKS_PAYMENTS = 543;
        private const int INTERNAL_CHECKS_PROMOS = 544;
        private const int INTERNAL_CHECKS_COMPS = 545;
        private const int INTERNAL_CHECKS_QUEUE = 546;

        private const int INTERNAL_ENTRIES = 560;		// Users canNOT call for this struct directly!
        private const int INTERNAL_ENTRIES_COMP_DATA = 561;
        private const int INTERNAL_ENTRIES_ITEM_DATA = 562;
        private const int INTERNAL_ENTRIES_PAYMENT_DATA = 563;
        private const int INTERNAL_ENTRIES_PROMO_DATA = 564;

        private const int INTERNAL_COMPS = 580;		// Users canNOT call for this struct directly!

        private const int INTERNAL_PAYMENTS = 600;		// Users canNOT call for this struct directly!

        private const int INTERNAL_PROMOS = 620;		// Users canNOT call for this struct directly!

        private const int INTERNAL_QUEUES = 640;
        private const int INTERNAL_QUEUES_HEAD_ORDER = 641;
        private const int INTERNAL_QUEUES_TAIL_ORDER = 642;

        private const int INTERNAL_SHIFTS = 660;		// Users canNOT call for this struct directly!
        private const int INTERNAL_SHIFTS_BREAKS = 661;

        private const int INTERNAL_BREAKS = 680;		// Users canNOT call for this struct directly!

        private const int INTERNAL_OPENITEMS = 700;		// Users canNOT call for this struct directly!

        private const int INTERNAL_LOCALSTATE = 720;		// LocalState of the actual terminal
        private const int INTERNAL_LOCALSTATE_COM = 721;		// LocalState of the "artifical" COM terminal
        private const int INTERNAL_LOCALSTATE_CUR_CHECK = 722;
        private const int INTERNAL_LOCALSTATE_CUR_EMP = 723;
        private const int INTERNAL_LOCALSTATE_KEY_EMP = 724;
        private const int INTERNAL_LOCALSTATE_CUR_ENTRY = 725;
        private const int INTERNAL_LOCALSTATE_CUR_PAYMENT = 726;
        private const int INTERNAL_LOCALSTATE_CUR_PROMO = 727;
        private const int INTERNAL_LOCALSTATE_CUR_TABLE = 728;
        private const int INTERNAL_LOCALSTATE_TEMP_EMP = 729;
        private const int INTERNAL_LOCALSTATE_CUR_QUEUE = 730;

        private const int INTERNAL_ITEMS = 740;		// Users canNOT call for this struct directly!

        private const int INTERNAL_CATS = 750;		// Internal Category structure
        private const int INTERNAL_CATS_ITEMIDS = 751;

        private const int INTERNAL_ITEMIDS = 760;		// Users canNOT call for this struct directly!

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // Tender ids
        // Need to find better way to manager the tender ids
        private const int VISA_TENDERID = 15;
        private const int MASTERCARD_TENDERID = 16;
        private const int AMEX_TENDERID = 17;
        private const int DISCOVERER_TENDERID = 19;

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // Tender ids in case the payment has already been processed via external gateways
        // Need to find better way to manager the tender ids
        private const int VISA_PROCESSED_TENDERID = 100;
        private const int MASTERCARD_PROCESSED_TENDERID = 105;
        private const int AMEX_PROCESSED_TENDERID = 110;
        // Kiran Karanki - Made the change per Tim Kappell's email on 10/03/2016
        private const int DISCOVERER_PROCESSED_TENDERID = 115;
        //private const int VISA_DEBIT_PROCESSED_TENDERID = 115;
        //private const int MASTERCARD_DEBIT_PROCESSED_TENDERID = 115;
        //private const int DISCOVERER_DEBIT_PROCESSED_TENDERID = 115;

        private const int VISA_DEBIT_PROCESSED_TENDERID = 120;
        private const int MASTERCARD_DEBIT_PROCESSED_TENDERID = 120;
        private const int DISCOVERER_DEBIT_PROCESSED_TENDERID = 120;

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        //
        // GetPaymentStatus return values
        //
        /// <summary>
        /// PAYMENT_WAITING
        /// </summary>
        public const long PAYMENT_WAITING = 0;
        /// <summary>
        /// PAYMENT_SUCCESS
        /// </summary>
        public const long PAYMENT_SUCCESS = 1;
        /// <summary>
        /// PAYMENT_FAILED 
        /// </summary>
        public const long PAYMENT_FAILED = -1;

        // Mod codes
        private const int MODCODE_ADD = 19;

        private IberDepot iberDepot;
        // MPSW: Changed type from IIberFuncs to IberFuncs
        private IberFuncs iberFunc;
        private int termId;
        private int openTableId;
        private int openCheckId;
        private List<int> itemIdList;
        private List<int> paymentIdList;
        private List<string> qualifiedItems;

        private bool debug = true;


        /// <summary>
        /// Constructor with terminal id
        /// Instantiates the IberDepot and IberFuncs, ensuring that Iber is running fine
        /// 
        /// Throws ParafaitAlohaException - In case Aloha throws exception
        /// </summary>
        public ParafaitAlohaIntegrator(int terminalId)
        {
            log.LogMethodEntry(terminalId);
            try
            {
                iberDepot = new IberDepot();
                // MPSW: Changed from IberFuncs to IberFuncsClass
                iberFunc = new IberFuncsClass();
                termId = terminalId;
                openTableId = -1;
                openCheckId = -1;
                itemIdList = new List<int>();
                paymentIdList = new List<int>();
                qualifiedItems = null;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ParafaitAlohaIntegrator()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with qualified items
        /// If the check contains any of the items listed as qualified items, it will be made part of the list
        /// of items to be returned back to the calling system    
        /// 
        /// Throws ParafaitAlohaException - In case Aloha throws exception
        /// </summary>
        public ParafaitAlohaIntegrator(int terminalId, List<string> qualifiedItems)
            : this(terminalId)
        {
            log.LogMethodEntry(terminalId, qualifiedItems);
            try
            {
                this.qualifiedItems = qualifiedItems;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ParafaitAlohaIntegrator()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Resets the Aloha terminal
        /// 
        /// Throws ParafaitAlohaException - In case Aloha throws exception
        /// </summary>
        public void ResetTerminal()
        {
            log.LogMethodEntry();
            try
            {
                iberFunc.ResetTerminal(termId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ResetTerminal()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Performs the login function
        /// 
        /// Throws ParafaitAlohaException - In case Aloha throws exception
        /// </summary>
        /// <param name="employeeNumber">Aloha Employee id</param>
        /// <param name="empPassWord">Password</param>
        public void PerformLogin(int employeeNumber, String empPassWord)
        {
            log.LogMethodEntry(employeeNumber, "empPassWord");
            try
            {
                int loginStatus = iberFunc.LogIn(termId, employeeNumber, empPassWord, "");
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PerformLogin()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Performs the logout function, logs out the currently logged in employee
        /// 
        /// Throws ParafaitAlohaException - In case Aloha throws exception
        /// </summary>
        public void PerformLogout()
        {
            log.LogMethodEntry();
            try
            {
                iberFunc.LogOut(termId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PerformLogout()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Clocks in the user
        /// 
        /// Throws ParafaitAlohaException - In case Aloha throws exception
        /// </summary>
        /// <param name="jobCode">Job code to be used to perform the clock in</param>
        public void PerformClockIn(int jobCode)
        {
            log.LogMethodEntry(jobCode);
            try
            {
                iberFunc.ClockIn(termId, jobCode);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PerformClockIn()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Clocks out the user
        /// 
        /// Throws ParafaitAlohaException - In case Aloha throws exception
        /// </summary>
        public void PerformClockOut()
        {
            log.LogMethodEntry();
            try
            {
                iberFunc.PerformCheckout(termId, 0);
                iberFunc.ClockOut(termId, 0);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PerformClockOut()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Opens an Aloha transaction
        /// This involves two steps - Allocating a table and then opening the check
        /// 
        /// Throws ParafaitAlohaException - In case Aloha throws exception
        /// </summary>
        /// <param name="queueId">Queue Id</param>
        /// <param name="tableNum">Table number</param>
        /// <param name="tableName">Name given to the table for identification during order delivery etc</param>
        /// <param name="numberOfGuests">Number of guests</param>
        public void OpenTransaction(int queueId, int tableNum, String tableName, int numberOfGuests)
        {
            log.LogMethodEntry(queueId, tableNum, tableName, numberOfGuests);
            try
            {
                openTableId = iberFunc.AddTable(termId, queueId, tableNum, tableName, numberOfGuests);
                openCheckId = iberFunc.AddCheck(termId, openTableId);

                itemIdList.Clear();
                paymentIdList.Clear();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing OpenTransaction()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Opens an Aloha transaction
        /// This involves two steps - Allocating a table and then opening the check
        /// Passes default value for queue id, table number, table name and number of guests
        /// </summary>
        public void OpenTransaction()
        {
            log.LogMethodEntry();
            OpenTransaction(1, 0, "", 0);
            log.LogMethodExit();
        }

        /// <summary>
        /// Adds the item to the check         
        /// </summary>
        /// <param name="itemNumber">Aloha item id</param>
        /// <param name="itemPrice">Price of the item</param>
        /// <param name="quantity">Item quantity</param>
        public void AddItem(int itemNumber, double itemPrice, int quantity)
        {
            log.LogMethodEntry(itemNumber, itemPrice, quantity);
            try
            {
                for (int i = 0; i < quantity; i++)
                {
                    int itemAddId = iberFunc.BeginItem(termId, openCheckId, itemNumber, "", itemPrice);
                    iberFunc.EndItem(termId);
                    itemIdList.Add(itemAddId);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing AddItem()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Adds the item to the check with the modifiers selected         
        /// </summary>
        /// <param name="itemNumber">Aloha item id</param>
        /// <param name="itemPrice">Price of the item</param>
        /// <param name="quantity">Item quantity</param>
        /// <param name="modifierItems">List of modifiers selected</param>
        public void AddItemWithModifier(int itemNumber, double itemPrice, int quantity, List<ParafaitAlohaInterfaceItem> modifierItems)
        {
            log.LogMethodEntry(itemNumber, itemPrice, quantity, modifierItems);
            try
            {
                for (int i = 0; i < quantity; i++)
                {
                    int itemAddId = iberFunc.BeginItem(termId, openCheckId, itemNumber, "", itemPrice);
                    if ((modifierItems != null) && (modifierItems.Count > 0))
                    {
                        foreach (ParafaitAlohaInterfaceItem currModifierItem in modifierItems)
                        {
                            iberFunc.ModItem(termId, itemAddId, currModifierItem.ItemId, "", currModifierItem.Price, currModifierItem.ModCode);
                        }
                    }
                    iberFunc.EndItem(termId);
                    itemIdList.Add(itemAddId);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing AddItemWithModifier()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Places the order - Causes the items to be printed on the kitchen printer        
        /// </summary>
        public void PlaceOrder()
        {
            log.LogMethodEntry();
            try
            {
                iberFunc.OrderItems(termId, openTableId, 0);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PlaceOrder()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Adds the item to the check with an option to defer the ordering    
        /// </summary>
        /// <param name="itemNumber">Aloha item id</param>
        /// <param name="itemPrice">Price of the item</param>
        /// <param name="quantity">Item quantity</param>
        /// <param name="deferOrdering">Specifies whether the order has to be placed or defered</param>
        public void OrderItemsWithDefer(int itemNumber, double itemPrice, int quantity, bool deferOrdering)
        {
            log.LogMethodEntry(itemNumber, itemPrice, quantity, deferOrdering);
            try
            {
                for (int i = 0; i < quantity; i++)
                {
                    int itemAddId = iberFunc.BeginItem(termId, openCheckId, itemNumber, "", itemPrice);
                    iberFunc.EndItem(termId);
                    itemIdList.Add(itemAddId);
                }
                if (deferOrdering == false)
                    iberFunc.OrderItems(termId, openTableId, 0);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing OrderItemsWithDefer()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Adds the item to the check and orders the same    
        /// </summary>
        /// <param name="itemNumber">Aloha item id</param>
        /// <param name="itemPrice">Price of the item</param>
        /// <param name="quantity">Item quantity</param>
        public void OrderItem(int itemNumber, double itemPrice, int quantity)
        {
            log.LogMethodEntry(itemNumber, itemPrice, quantity);
            OrderItemsWithDefer(itemNumber, itemPrice, quantity, false);
            log.LogMethodExit();
        }

        #region Methods added to support discounts (comps) and promotions (promo)

        /// <summary>
        /// Selects the specified entry on the check. If value is >= 0
        /// then the selected entry is associated with the integer value.
        /// </summary>
        /// <param name="entryId">Id of entry to be selected</param>
        /// <param name="value">Integer value associated with selected entry</param>
        public void SelectEntry(int entryId, int value = -1)
        {
            log.LogMethodEntry(entryId, value);
            try
            {
                if (value < 0)
                    iberFunc.SelectEntry(termId, openCheckId, entryId);
                else
                    iberFunc.SelectEntryWithValue(termId, openCheckId, entryId, value);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing SelectEntry()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Selects all entries on the check. If value is >= 0
        /// then the selected entries are associated with the integer value.
        /// </summary>
        /// <param name="value">Integer value associated with selected entry</param>
        public void SelectAllEntries(int value = -1)
        {
            log.LogMethodEntry(value);
            try
            {
                if (value < 0)
                    iberFunc.SelectAllEntriesOnCheck(termId, openCheckId);
                else
                    iberFunc.SelectAllEntriesOnCheckWithValue(termId, openCheckId, value);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing SelectAllEntries()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Selects the specified entry and all child entries (if any) on
        /// the check. If value is >= 0 then the selected entries are associated 
        /// with the integer value.
        /// </summary>
        /// <param name="entryId">Id of entry to be selected</param>
        /// <param name="value">Integer value associated with selected entry</param>
        public void SelectEntryAndChildren(int entryId, int value = -1)
        {
            log.LogMethodEntry(entryId, value);
            try
            {
                if (value < 0)
                    iberFunc.SelectEntryAndChildren(termId, openCheckId, entryId);
                else
                    iberFunc.SelectEntryAndChildrenWithValue(termId, openCheckId, entryId, value);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing SelectEntryAndChildren()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// De selects specified entry.
        /// </summary>
        /// <param name="entryId">Id of entry to be selected</param>
        public void DeselectEntry(int entryId)
        {
            log.LogMethodEntry(entryId);
            try
            {
                iberFunc.DeselectEntry(termId, entryId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing DeselectEntry()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// De selects all selected entries on the check.
        /// </summary>
        public void DeselectAllEntries()
        {
            log.LogMethodEntry();
            try
            {
                iberFunc.DeselectAllEntries(termId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing DeselectAllEntries()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// De selects specified entry and all selected children.
        /// </summary>
        /// <param name="entryId">Id of entry to be selected</param>
        public void DeselectEntryAndChildren(int entryId)
        {
            log.LogMethodEntry(entryId);
            try
            {
                iberFunc.DeselectEntryAndChildren(termId, openCheckId, entryId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing DeselectEntryAndChildren()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Authorizes the override manager for the next operation (k.e. ApplyComp or ApplyPromo) if necessary.
        /// </summary>
        /// <param name="mgrNum">The manager manual number.</param>
        /// <param name="mgrPwd">The manager manual password.</param>
        /// <param name="mgrMagCardPwd">The manager manual mag card password.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="ParafaitAlohaIntegration.ParafaitAlohaException"></exception>
        public int AuthorizeOverrideManager(int mgrNum, string mgrPwd, string mgrMagCardPwd)
        {
            log.LogMethodEntry("mgrNum", "mgrPwd", "mgrMagCardPwd");
            try
            {
                int mgrId = iberFunc.AuthorizeOverrideMgr(termId, mgrNum, mgrPwd, mgrMagCardPwd);
                log.LogMethodExit(mgrId);
                return mgrId;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing AuthorizeOverrideManager()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
        }

        /// <summary>
        /// Applies the specified comp to the check     
        /// </summary>
        /// <param name="mgrId">Manager id for promos requiring manager override; zero otherwise</param>
        /// <param name="compTypeId">Complimentary discount id to apply to check</param>
        /// <param name="amount">Amount being paid by customer</param>
        /// <param name="unit">Unit name</param>
        /// <param name="name">Complimentary discount name</param>
        public int ApplyComp(int compTypeId, double amount, string unit, string name, int mgrId = 0)
        {
            log.LogMethodEntry(compTypeId, amount, unit, name, mgrId);
            try
            {
                int paymentId = iberFunc.ApplyComp(termId, mgrId, openCheckId, compTypeId, amount, unit, name);
                paymentIdList.Add(paymentId);
                log.LogMethodExit(paymentId);
                return paymentId;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ApplyComp()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
        }

        /// <summary>
        /// Applies the specified promo to the check     
        /// </summary>
        /// <param name="mgrId">Manager id for promos requiring manager override; zero otherwise</param>
        /// <param name="promoId">Promotion id to apply to check</param>
        /// <param name="amount">Amount being paid by customer</param>
        /// <param name="ident">Optional promotion identifier (i.e. coupon #)</param>
        public int ApplyPromo(int promoId, double amount, string ident, int mgrId = 0)
        {
            log.LogMethodEntry(promoId, amount, ident, mgrId);
            try
            {
                int paymentId = iberFunc.ApplyPromo(termId, mgrId, openCheckId, promoId, amount, ident);
                paymentIdList.Add(paymentId);
                iberFunc.DeselectAllEntries(termId);
                log.LogMethodExit(paymentId);
                return paymentId;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ApplyPromo()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
        }
        #endregion

        /// <summary>
        /// Applies the cash amount to the check     
        /// </summary>
        /// <param name="amount">Amount being paid by customer</param>
        public int ApplyCashPayment(double amount)
        {
            log.LogMethodEntry(amount);
            try
            {
                int paymentId = iberFunc.ApplyCashPayment(termId, openCheckId, amount);
                paymentIdList.Add(paymentId);
                log.LogMethodExit(paymentId);
                return paymentId;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ApplyCashPayment()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
        }

        /// <summary>
        /// Gets the open balance (amount pending to be paid)    
        /// </summary>
        public double GetCheckBalance()
        {
            log.LogMethodEntry();
            try
            {
                double checkBalance = iberFunc.GetCheckBalance(termId, openCheckId);
                log.LogMethodExit(checkBalance);
                return checkBalance;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing GetCheckBalance()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
        }

        /// <summary>
        /// Applies the credit card amount to the check     
        /// 
        /// First the processing is done using the complete credit card track information
        /// If the track information based processing fails, then the credit card number 
        /// and expiry date is passed to the processor. 
        /// </summary>
        /// <param name="amount">Amount being paid by customer</param>
        /// <param name="cardId">Credit card number</param>
        /// <param name="expDate">Expiry date in mmyy format</param>
        /// <param name="cardTrackInfo">Complete track information</param>
        [Obsolete("Not used anymore, unencrypted credit card number cannot be passed for processing. Method is there only for backward compatibility", false)]
        public int ApplyCreditCardPayment(double amount, String cardId, String expDate, String cardTrackInfo)
        {
            log.LogMethodEntry(amount, cardId, expDate, cardTrackInfo);
            int cardType = 15;
            int paymentId = 0;
            bool paymentSuccess = false;

            CreditCard creditCard = new CreditCard(cardId);
            if (creditCard.CardValid == false)
                throw new Exception("Credit card information is not valid. Card number validation failed");
            switch (creditCard.CardType)
            {
                case CreditCard.CreditCardType.VISA:
                    cardType = VISA_TENDERID;
                    break;
                case CreditCard.CreditCardType.MASTERCARD:
                    cardType = MASTERCARD_TENDERID;
                    break;
                case CreditCard.CreditCardType.AMEX:
                    cardType = AMEX_TENDERID;
                    break;
                case CreditCard.CreditCardType.DISCOVERER:
                    cardType = DISCOVERER_TENDERID;
                    break;
                case CreditCard.CreditCardType.DINERSCLUB:
                case CreditCard.CreditCardType.ENROUTE:
                case CreditCard.CreditCardType.CARTEBLANCH:
                case CreditCard.CreditCardType.JCB:
                    throw new Exception("Invalid card types, the tender id is not defined in Aloha. The credit card type passed was " + creditCard.CardType);
                case CreditCard.CreditCardType.UNKNOWN:
                    throw new Exception("Credit card was not recognized, please retry..");
                default:
                    throw new Exception("Credit card processing hit an error. Please retry..");
            }
            try
            {
                try
                {
                    if (cardTrackInfo.CompareTo("") != 0)
                    {
                        double checkAmount = GetCheckBalance();
                        if (checkAmount < amount)
                            amount = checkAmount;
                        paymentId = iberFunc.ApplyPayment(termId, openCheckId, cardType, amount, 0, "", "", cardTrackInfo, "");
                        paymentSuccess = true;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing ApplyPayment()" + ex.Message);
                    // Code might be required to handle specific exception and proceed. Currently all exception are being suppressed and the card number, expiry date is being passed
                }

                if (paymentSuccess == false)
                {
                    if (expDate.CompareTo("") != 0)
                        paymentId = iberFunc.ApplyPayment(termId, openCheckId, cardType, amount, 0, cardId, expDate, "", "");
                    else
                        throw new Exception("Credit card information is not valid, expiry date is not valid. Please check..");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing GetCheckBalance()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            paymentIdList.Add(paymentId);
            log.LogMethodExit(paymentId);
            return paymentId;
        }


        /// <summary>
        /// Applies the credit card amount to the check  marking that the credit card was already processed   
        /// In this case the credit card has already been processed through some external gateway
        /// This method allows the payment to be applied in Aloha but marking it as already processed
        /// so that Aloha does not process the credit card again
        /// </summary>
        /// <param name="amount">Amount being paid by customer</param>
        /// <param name="cardType">Type of credit card</param>
        /// <param name="cardId">Credit Card number</param>
        /// <param name="expDate">Expiry date in mmyy format</param>
        /// <param name="authCode">Authorization code - Referring to the processing already done</param>
        public int ApplyProcessedCreditCardPayment(double amount, String cardType, String cardId, String expDate, String authCode)
        {
            log.LogMethodEntry(amount, cardId, expDate, authCode);
            int cardTenderType = 15;
            int paymentId = 0;

            if (cardType.CompareTo("VISA") == 0)
                cardTenderType = VISA_PROCESSED_TENDERID;
            else if (cardType.CompareTo("AMEX") == 0)
                cardTenderType = AMEX_PROCESSED_TENDERID;
            else if (cardType.CompareTo("MASTERCARD") == 0)
                cardTenderType = MASTERCARD_PROCESSED_TENDERID;
            else if (cardType.CompareTo("DISCOVERER") == 0)
                cardTenderType = DISCOVERER_PROCESSED_TENDERID;
            else if (cardType.CompareTo("VISA_DEBIT") == 0)
                cardTenderType = VISA_DEBIT_PROCESSED_TENDERID;
            else if (cardType.CompareTo("MASTERCARD_DEBIT") == 0)
                cardTenderType = MASTERCARD_DEBIT_PROCESSED_TENDERID;
            else if (cardType.CompareTo("DISCOVERER_DEBIT") == 0)
                cardTenderType = DISCOVERER_DEBIT_PROCESSED_TENDERID;
            else throw new Exception("Credit card processing hit an error. Please retry..");

            paymentId = ApplyProcessedCreditCardPaymentWithTenderId(amount, cardTenderType, cardId, expDate, authCode);
            log.LogMethodExit(paymentId);
            return paymentId;
        }

        /// <summary>
        /// Applies the credit card amount to the check  marking that the credit card was already processed   
        /// In this case the credit card has already been processed through some external gateway
        /// This method allows the payment to be applied in Aloha but marking it as already processed
        /// so that Aloha does not process the credit card again
        /// </summary>
        /// <param name="amount">Amount being paid by customer</param>
        /// <param name="tenderId">Aloha tender id mapping to the credit/debit card</param>
        /// <param name="cardId">Credit Card number</param>
        /// <param name="expDate">Expiry date in mmyy format</param>
        /// <param name="authCode">Authorization code - Referring to the processing already done</param>
        public int ApplyProcessedCreditCardPaymentWithTenderId(double amount, int tenderId, String cardId, String expDate, String authCode)
        {
            log.LogMethodEntry(amount, tenderId, cardId, expDate, authCode);
            int cardTenderType = tenderId;
            int paymentId = 0;
            try
            {
                if (expDate.CompareTo("") != 0)
                    paymentId = iberFunc.ApplyPayment(termId, openCheckId, cardTenderType, amount, 0, cardId, expDate, "", authCode);
                else
                    throw new Exception("Credit card information is not valid, expiry date is not valid. Please check..");
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ApplyProcessedCreditCardPaymentWithTenderId()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }

            paymentIdList.Add(paymentId);
            log.LogMethodExit(paymentId);
            return paymentId;
        }

        /// <summary>
        /// Reverses the aloha transaction
        /// This needs to be done in the cases where the credit card payment is not processed or
        /// the system has hit exception
        /// </summary>
        public void ReverseTransaction()
        {
            log.LogMethodEntry();
            try
            {
                List<ParafaitAlohaItem> entriesList = GetItemsInLastCheck();
                foreach (ParafaitAlohaItem voidItem in entriesList)
                {
                    iberFunc.VoidItem(termId, openCheckId, voidItem.EntryId, 10);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ReverseTransaction()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Closes the check and the table
        /// </summary>
        public void CloseTransaction()
        {
            log.LogMethodEntry();
            try
            {
                //PlaceOrder();
                iberFunc.CloseCheck(termId, openCheckId);
                iberFunc.CloseTable(termId, openTableId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing CloseTransaction()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the list of items in the check. 
        /// This is required by the Parafait system to evaluate whether the check has any token or time items
        /// If it has the token items or time items or is part of the "qualified items" list, it will be passed back
        /// to the calling program
        /// </summary>
        public List<ParafaitAlohaItem> GetItemsInLastCheck()
        {
            log.LogMethodEntry();
            List<ParafaitAlohaItem> parafaitAlohaItemList = new List<ParafaitAlohaItem>();

            String fileName = "ITM.DBF";
            String currentPath = System.Environment.CurrentDirectory;
            Writelog("ParafaitAlohaIntegrator GetItemsInLastCheck...Current Path: " + currentPath);
            String baseFilePath = Path.GetFullPath(Path.Combine(currentPath, @"DATA\")) + fileName;
            Writelog("ParafaitAlohaIntegrator GetItemsInLastCheck...Base file Path: " + baseFilePath);
            String finalPath = Path.GetFullPath(Path.Combine(currentPath, @"TMP\")) + fileName;
            Writelog("ParafaitAlohaIntegrator GetItemsInLastCheck...Final Path: " + finalPath);
            Writelog("ParafaitAlohaIntegrator GetItemsInLastCheck...Checking file status in TMP folder");
            if (!CheckFileStatus(baseFilePath, finalPath))
                PerformFileCopy(baseFilePath, finalPath);
            Writelog("ParafaitAlohaIntegrator GetItemsInLastCheck...Complete check file status in TMP folder");
            DataTable itemDataTable = ParseDBF.ReadDBF(finalPath);

            try
            {
                IberEnum LocalStateEnum = iberDepot.GetEnum(INTERNAL_LOCALSTATE);
                IIberObject LocalState = LocalStateEnum.First();
                bool termIdFound = false;
                while ((LocalState != null) && (termIdFound == false))
                {
                    try
                    {
                        int termIdOfLocalState = Convert.ToInt32(LocalState.GetStringVal("TERMINAL_ID"));
                        if (termIdOfLocalState == termId)
                            termIdFound = true;
                        else
                            LocalState = LocalStateEnum.Next();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while Converting to Int" + ex.Message);
                        LocalState = null;
                    }
                }
                IberEnum CheckEnum = LocalState.GetEnum(INTERNAL_LOCALSTATE_CUR_CHECK);
                IIberObject check = CheckEnum.First();
                String checkId = check.GetStringVal("ID");
                IberEnum EntriesEnum = check.GetEnum(INTERNAL_CHECKS_ENTRIES);
                IIberObject Entries = EntriesEnum.First();

                while (Entries != null)
                {
                    if (Entries.GetLongVal("LEVEL") == 0)
                    {
                        //IberEnum ItemEnum = iberDepot.FindObjectFromId(740, Convert.ToInt32(Entries.GetLongVal("DATA")));
                        //IIberObject Item = ItemEnum.First();
                        //String cat = Item.GetStringVal("CAT_ID");
                        String id = Entries.GetStringVal("ID");
                        String dispName = Entries.GetStringVal("DISP_NAME");
                        String data = Entries.GetStringVal("DATA");
                        String quantity = Entries.GetStringVal("QUANTITY");
                        String units = Entries.GetStringVal("UNITS");
                        String changed = Entries.GetStringVal("CHANGED");
                        String stored = Entries.GetStringVal("STORED");

                        String mode = Entries.GetStringVal("MODE");
                        String type = Entries.GetStringVal("TYPE");
                        String selected = Entries.GetStringVal("SELECTED");
                        String period = Entries.GetStringVal("PERIOD");
                        String modCode = Entries.GetStringVal("MOD_CODE");
                        String revisionId = Entries.GetStringVal("REV_ID");
                        String termId = Entries.GetStringVal("TERM_ID");
                        String origin = Entries.GetStringVal("ORIGIN");
                        String itemOrderTime = Entries.GetStringVal("TIME");
                        String price = Entries.GetStringVal("PRICE");

                        //String tokenCount = "60";
                        try
                        {
                            // For the item in the check, see if it's in the list. If yes, get the BOHNAME and TOKENQTY from the same
                            String tokenCount;
                            String timeCount;

                            //DataRow itemDataRow = itemDataTable.AsEnumerable().SingleOrDefault(r => r.Field<String>("ID") == id);
                            DataRow itemDataRow = itemDataTable.AsEnumerable().FirstOrDefault(r => r.Field<Int32>("ID") == Convert.ToInt32(data));

                            if (itemDataRow != null)
                            {
                                Writelog("ParafaitAlohaIntegrator GetItemsInLastCheck...fetch Item ID " + data);
                                tokenCount = itemDataRow["TOKENQTY"].ToString();
                                timeCount = itemDataRow["BOHNAME"].ToString();
                            }
                            else
                            {
                                Writelog("ParafaitAlohaIntegrator GetItemsInLastCheck...else part Item ID: " + data);
                                IberEnum itemsEnum = iberDepot.GetEnum(INTERNAL_ITEMS);
                                IberObject itemIberObj = itemsEnum.FindFromLongAttr("ID", Convert.ToInt32(data));
                                tokenCount = itemIberObj.GetStringVal("TOKENQTY");
                                timeCount = itemIberObj.GetStringVal("BOHNAME");
                            }
                            try
                            {
                                int tokenVal = 0;
                                try
                                {
                                    tokenVal = Convert.ToInt32(tokenCount);
                                }
                                catch (FormatException ex)
                                {
                                    tokenVal = 0;
                                    log.Error("Error occurred while Converting to Int" + ex.Message);

                                }

                                int timeCountVal = 0;
                                try
                                {
                                    timeCountVal = Convert.ToInt32(timeCount);
                                }
                                catch (FormatException ex)
                                {
                                    log.Error("Error occurred while Converting to Int" + ex.Message);
                                    timeCountVal = 0;
                                }

                                //int modeVal = Convert.ToInt32(mode);
                                if (tokenVal != 0)
                                {
                                    parafaitAlohaItemList.Add(new ParafaitAlohaItem(id, checkId, data, dispName, quantity, units, tokenCount, changed, stored,
                                        mode, type, selected, period, modCode, revisionId, termId, origin, itemOrderTime, price, timeCountVal));
                                    Writelog("parafaitAlohaItemList updated with Token value" + data + tokenCount);
                                }
                                else if (timeCountVal != 0)
                                    parafaitAlohaItemList.Add(new ParafaitAlohaItem(id, checkId, data, dispName, quantity, units, tokenCount, changed, stored,
                                        mode, type, selected, period, modCode, revisionId, termId, origin, itemOrderTime, price, timeCountVal));
                                else
                                {
                                    if (qualifiedItems != null)
                                    {
                                        foreach (string currString in qualifiedItems)
                                        {
                                            if (data.CompareTo(currString) == 0)
                                                parafaitAlohaItemList.Add(new ParafaitAlohaItem(id, checkId, data, dispName, quantity, units, tokenCount, changed, stored,
                                                    mode, type, selected, period, modCode, revisionId, termId, origin, itemOrderTime, price, timeCountVal));
                                        }
                                    }
                                }
                            }
                            catch (FormatException ex)
                            {
                                log.Error("Error occurred while executing GetItemsInLastCheck()" + ex.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while executing GetItemsInLastCheck()" + ex.Message);
                        }
                    }
                    try
                    {
                        Entries = EntriesEnum.Next();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while executing EntriesEnum.Next()" + ex.Message);
                        Entries = null;
                    }

                }
                Writelog("Before returning list " + parafaitAlohaItemList.Count.ToString());
                log.LogMethodExit(parafaitAlohaItemList);
                return parafaitAlohaItemList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing GetItemsInLastCheck()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
        }

        /// <summary>
        /// Gets the details of item from the Aloha INTERNAL_ITEMS
        /// </summary>
        public AlohaItemDTO GetItemDetails(int itemId)
        {
            log.LogMethodEntry(itemId);
            try
            {
                IberEnum itemsEnum = iberDepot.GetEnum(INTERNAL_ITEMS);
                IberObject itemIberObj = itemsEnum.FindFromLongAttr("ID", itemId);
                String itemName = itemIberObj.GetStringVal("LONGNAME");
                String itemPrice = itemIberObj.GetStringVal("PRICE");
                String taxId = itemIberObj.GetStringVal("TAXID");
                AlohaItemDTO currItem = new AlohaItemDTO(itemId, itemName, itemPrice, taxId);
                log.LogMethodExit(currItem);
                return currItem;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing GetItemDetails()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
        }


        /// <summary>
        /// Prints a receipt
        /// The reader, footer and the name value pair of content is given as input
        /// The content has two fields referred as name and value. The name is printed on left side and the value on the right side.
        /// The reader and footer are centered.
        /// </summary>
        /// <param name="printHeader">List of strings to be shown as header</param>
        /// <param name="printFooter">List of strings to be shown as footer</param>
        /// <param name="printText">List of name value pair containing the content to be shown in the receipt</param>
        public string PrintReceipt(List<string> printHeader, List<string> printFooter, List<KeyValuePair<string, string>> printText)
        {
            log.LogMethodEntry(printHeader, printFooter, printText);
            try
            {
                IberPrinter printer = new IberPrinter();
                const string printStreamStart = "<PRINT>";
                const string printStreamEnd = "</PRINT>";
                const string printCMDStart = "<COMMANDS>";
                const string printCMDEnd = "</COMMANDS>";
                const string printCenteredStart = "<PRINTCENTERED>";
                const string printCenteredEnd = "</PRINTCENTERED>";
                const string printLeftRightStart = "<PRINTLEFTRIGHT>";
                const string printLeftRightEnd = "</PRINTLEFTRIGHT>";
                const string leftStart = "<LEFT>";
                const string leftEnd = "</LEFT>";
                const string rightStart = "<RIGHT>";
                const string rightEnd = "</RIGHT>";

                string printAddlCmdsStrg = "<DOCTYPE>2</DOCTYPE><PRINTER>0</PRINTER>";
                string printEndOfReceiptCmdsStrg = "<LINEFEED>2</LINEFEED><CUT>PARTIAL</CUT>";
                string finalPrintStrg = printStreamStart + printAddlCmdsStrg;
                finalPrintStrg += printCMDStart;
                foreach (string printHeaderVal in printHeader)
                {
                    finalPrintStrg += printCenteredStart + printHeaderVal + printCenteredEnd;
                }

                foreach (KeyValuePair<string, string> printTextVal in printText)
                {
                    finalPrintStrg += printLeftRightStart + leftStart + printTextVal.Key + leftEnd + rightStart + printTextVal.Value + rightEnd + printLeftRightEnd;
                }

                foreach (string printFooterVal in printFooter)
                {
                    finalPrintStrg += printCenteredStart + printFooterVal + printCenteredEnd;
                }
                finalPrintStrg += printEndOfReceiptCmdsStrg;
                finalPrintStrg += printCMDEnd;
                finalPrintStrg += printStreamEnd;
                printer.PrintStream(finalPrintStrg);
                log.LogMethodExit(finalPrintStrg);
                return finalPrintStrg;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PrintReceipt()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
        }

        /// <summary>
        /// Checks the payment status. The status can be one of three values
        /// a. Processing
        /// b. Accepted
        /// c. Declined
        /// </summary>
        /// <param name="paymentId">Payment Identifier - Returned by ApplyPayment method</param>
        public int GetPaymentStatus(int paymentId)
        {
            log.LogMethodEntry(paymentId);
            try
            {
                int returnValue = iberFunc.GetPaymentStatus(termId, paymentId);
                log.LogMethodExit(returnValue);
                return returnValue;

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing GetPaymentStatus()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new ParafaitAlohaException(ex);
            }
        }

        /// <summary>
        /// Gets the table id
        /// </summary>
        public int GetTableId()
        {
            log.LogMethodEntry();
            log.LogMethodExit(openTableId);
            return openTableId;
        }

        /// <summary>
        /// Gets the check id
        /// </summary>
        public int GetCheckId()
        {
            log.LogMethodEntry();
            log.LogMethodExit(openCheckId);
            return openCheckId;
        }

        /// <summary>
        /// Checks status of file in TMP folder and will return false
        /// if file is not found or if original file timestamp is changed
        /// </summary>
        /// <param name="baseFilePath">should be DATA folder</param>
        /// <param name="finalPath">should be TMP folder</param>
        /// <returns></returns>
        public bool CheckFileStatus(string baseFilePath, string finalPath)
        {
            log.LogMethodEntry(baseFilePath, finalPath);
            bool fileStatus = false;
            try
            {
                if (File.Exists(finalPath))
                {
                    Writelog("ParafaitAlohaIntegrator CheckFileStatus...File exists in TMP folder");
                    if (File.GetLastWriteTime(finalPath).CompareTo(File.GetLastWriteTime(baseFilePath)) == 0)
                    {
                        Writelog("ParafaitAlohaIntegrator CheckFileStatus...Last modified time matches with DATA folder");
                        fileStatus = true; //if file exists and last modified time is same with base file then return true
                    }
                    else
                    {
                        Writelog("ParafaitAlohaIntegrator CheckFileStatus...Last modified time does not match with DATA folder");
                        fileStatus = false;
                    }
                }
                else
                {
                    Writelog("ParafaitAlohaIntegrator CheckFileStatus...file does not exist in TMP folder");
                    fileStatus = false; //If file does not exists, return false
                }
                log.LogMethodExit(fileStatus);
                return fileStatus;
            }
            catch (Exception ex)
            {
                Writelog("ParafaitAlohaIntegrator CheckFileStatus...exception" + ex.Message);
                log.LogMethodExit(false);
                return false; //any exception, return false so that file can be refreshed
            }
        }

        /// <summary>
        /// Performs file copy from origin to final destination
        /// </summary>
        /// <param name="baseFilePath">should be DATA folder</param>
        /// <param name="finalPath">should be TMP folder</param>
        public void PerformFileCopy(string baseFilePath, string finalPath)
        {
            log.LogMethodEntry(baseFilePath, finalPath);
            try
            {
                if (File.Exists(baseFilePath))
                {
                    var basepath = new FileInfo(baseFilePath);
                    Writelog("ParafaitAlohaIntegrator PerformFileCopy...basefilePatch: " + basepath.ToString());
                    basepath.CopyTo(finalPath, true);
                    var destination = new FileInfo(finalPath);
                    Writelog("ParafaitAlohaIntegrator PerformFileCopy...finalPath: " + destination.ToString());
                    if (destination.IsReadOnly)
                        destination.IsReadOnly = false;
                    destination.LastAccessTime = basepath.LastAccessTime;
                    destination.LastWriteTime = basepath.LastWriteTime;
                    destination.CreationTime = basepath.CreationTime;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing PerformFileCopy()" + ex.Message);
                Writelog("ParafaitAlohaIntegrator PerformFileCopy...Exception block");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Message to be logged</param>
        private void Writelog(string message)
        {
            log.LogMethodEntry(message);
            try
            {
                if (debug)
                {
                    message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss :") + message + Environment.NewLine;
                    string path = new DirectoryInfo(System.Environment.CurrentDirectory).Parent.FullName;
                    System.IO.Directory.CreateDirectory(path + "\\AlohaQS\\TMP\\Logs\\");
                    File.AppendAllText(path + "\\AlohaQS\\TMP\\Logs\\" + DateTime.Today.ToString("yyyy_MM_dd_") + "ParafaitAlohaIntegratorLog.txt", message);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing Writelog()" + ex.Message);
                log.LogMethodExit(null, "Throwing Exception" + ex.Message);
                throw new Exception(ex.Message + ex.InnerException.Message);
            }
            log.LogMethodExit();
        }
    }
}
