/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - CustomCheckBox 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Custom checkbox created for pos
    /// </summary>
    public class CustomCheckBox: CheckBox
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomCheckBox()
        {
            log.LogMethodEntry();
            AutoSize = false;
            ImageList imageList = new ImageList();
            imageList.ImageSize = new System.Drawing.Size(20, 20);
            imageList.Images.Add(Semnox.Core.GenericUtilities.Resources.Checked);
            imageList.Images.Add(Semnox.Core.GenericUtilities.Resources.Unchecked);
            ImageIndex = 1;
            Appearance = System.Windows.Forms.Appearance.Button;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            FlatAppearance.BorderSize = 0;
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ImageList = imageList;
            TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            UseVisualStyleBackColor = true;
            CheckedChanged += CustomCheckBox_CheckedChanged;
            log.LogMethodExit();
        }

        private void CustomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if(Checked)
            {
                ImageIndex = 0; 
            }
            else
            {
                ImageIndex = 1;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// set the background color
        /// </summary>
        /// <param name="color"></param>
        public void SetBackGroundColor(Color color)
        {
            log.LogMethodEntry(color);
            FlatAppearance.MouseDownBackColor = color;
            BackColor = color;
            FlatAppearance.MouseOverBackColor = color;
            FlatAppearance.CheckedBackColor = color;
            log.LogMethodExit();
        }
    }
}
