/********************************************************************************************
 * Project Name - TagsUI
 * Description  - PerformManualEventView 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.130       04-Feb-2022   Girish Kundar          Created - Korean Fisckalization
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.PrintUI
{
    /// <summary>
    /// Interaction logic for VCATPrintOptionView
    /// </summary>
    public partial class VCATPrintOptionView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// VCATPrintOptionView
        /// </summary>
        public VCATPrintOptionView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            if (MainGrid != null)
            {
                MainGrid.MaxWidth = SystemParameters.PrimaryScreenWidth / 2;
                MainGrid.MaxHeight = SystemParameters.PrimaryScreenHeight / 2;
            }
        }
    }
}
