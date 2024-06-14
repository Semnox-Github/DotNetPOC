/********************************************************************************************
 * Project Name - Kiosk Core
 * Description  - Audio.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019  Deeksha      Added logger methods.
 ********************************************************************************************/
using System;
using WMPLib;

namespace Semnox.Parafait.KioskCore
{
    public static class Audio
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string BuyNewCard = "BuyNewCard";
        public const string CheckBalance_Activity = "CheckBalance-Activity";
        public const string ChooseOption = "ChooseOption";
        public const string CollectCard = "CollectCard";
        public const string CollectCardAndReceipt = "CollectCardAndReceipt";
        public const string EnterDetailsAndSave = "EnterDetailsAndSave";
        public const string TapCardOnReader = "TapCardOnReader";
        public const string InsertCardIntoReader = "InsertCardIntoReader";
        public const string InsertExactAmount = "InsertExactAmount";
        public const string Register = "Register";
        public const string SelectLanguage = "SelectLanguage";
        public const string SelectNewCardProduct = "SelectNewCardProduct";
        public const string SelectTopUpProduct = "SelectTopUpProduct";
        public const string ThankYouEnjoyGame = "ThankYouEnjoyGame";
        public const string TopUpCard = "TopUpCard";
        public const string WaitForCardDispense = "WaitForCardDispense";
        public const string WaitForCardTopUp = "WaitForCardTopUp";
        public const string RegisterCardPrompt = "RegisterCardPrompt";
        public const string GamePlayDetails = "GamePlayDetails";
        public const string RegisterTermsAndConditions = "RegisterTermsAndConditions";
        public const string RedeemTokenTapCard = "RedeemTokenTapCard";
        public const string RedeemInsertToken = "RedeemInsertToken";
        public const string CollectReceipt = "CollectReceipt";
        public const string TransferFromTapCard = "TransferFromTapCard";
        public const string TransferToCardTap = "TransferToCardTap";
        public const string PointsToBeTransferred = "PointsToBeTransferred";
        
        const string extension = ".mp3";
        public static WindowsMediaPlayer soundPlayer = new WindowsMediaPlayer();
        public static void PlayAudio(params string[] FileNames)
        {
            log.LogMethodEntry(FileNames);
            if (KioskStatic.PlayKioskAudio == false)
            {
                log.LogMethodExit();
                return;
            }
            try
            {
                soundPlayer.controls.stop();

                string dir = System.Windows.Forms.Application.StartupPath + @"\Media\Audio\";
                string dirLang = dir;
                if (KioskStatic.Utilities.ParafaitEnv.LanguageId > 0)
                    dirLang += KioskStatic.Utilities.ParafaitEnv.LanguageCode + @"\";

                if (soundPlayer.currentPlaylist == null)
                    soundPlayer.currentPlaylist = soundPlayer.playlistCollection.newPlaylist(Guid.NewGuid().ToString());
                else
                {
                    soundPlayer.currentPlaylist.clear();
                }

                WMPLib.IWMPMedia media;
                foreach (string file in FileNames)
                {
                    string sfile = dirLang + file + extension;
                    if (!System.IO.File.Exists(sfile))
                        sfile = dir + file + extension;

                    media = soundPlayer.newMedia(sfile);
                    soundPlayer.currentPlaylist.appendItem(media);
                }

                soundPlayer.controls.play();
            }
            catch (Exception ex)
            {
                log.Error("Error while excuting PlayAudio()" + ex.Message);
                KioskStatic.logToFile("Audio:" + ex.Message + string.Join(", ", FileNames));
            }
            log.LogMethodExit();
        }

        public static void Stop()
        {
            log.LogMethodEntry();
            try
            {
                soundPlayer.controls.stop();
            }
            catch (Exception ex)
            {
                log.Error("Error while executing stop()", ex);
            }
            log.LogMethodExit();
        }
    }
}
