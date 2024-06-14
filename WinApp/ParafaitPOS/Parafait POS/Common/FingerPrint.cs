/********************************************************************************************
 * Project Name - Common
 * Description  -  Class for FingerPrint
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80         20-Aug-2019     Girish Kundar        Modified : Added Logger methods and Removed unused namespace's 
 *********************************************************************************************/
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Parafait_POS
{
    /// <summary>
    /// Information about the fingerprint
    /// </summary>
    public class FingerPrint
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int _user;
        /// <summary>
        /// The person the finger print belongs to
        /// </summary>
        public int User
        {
            get { return _user; }
            set { _user = value; }
        }

        private Image _image = Properties.Resources.DefaultImage;
        /// <summary>
        /// The fingerprint itself
        /// </summary>
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        private byte[] _template;
        /// <summary>
        /// Finger print template
        /// </summary>
        public byte[] Template
        {
            get { return _template; }
            set { _template = value; }
        }
	
        /// <summary>
        /// helper method to convert a byte[] into an image
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        internal Image ParseImage(byte[] img)
        {
            log.LogMethodEntry();
            using (MemoryStream ms = new MemoryStream(img))
            {
                log.LogMethodExit();
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// helper method to convert an image to a byte[]
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        internal byte[] ParseImage(Image img)
        {
            log.LogMethodEntry();
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                log.LogMethodExit();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public FingerPrint()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// data reader constructor
        /// </summary>
        /// <param name="rdr"></param>
        internal FingerPrint(IDataReader rdr)
        {
            log.LogMethodEntry();
            this.User = int.Parse(rdr["user_id"].ToString());
            this.Image = ParseImage((byte[])rdr["finger_print"]);
            this.Template = (byte[])rdr["fp_template"];
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="u">the user the print belongs to</param>
        /// <param name="f">the finger the print is on</param>
        /// <param name="h">the hand the finger is on</param>
        /// <param name="i">the image of the print</param>
        /// <param name="template">the meta data template about the print</param>
        public FingerPrint(int UserID, Image i, byte[] template)
        {
            log.LogMethodEntry();
            this.User = UserID;
            this.Image = i;
            this.Template = template;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save the print to the database
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            //remove it if it already is there
            this.Delete();

            //save it
            SqlCommand cmd = POSStatic.Utilities.getCommand();

            cmd.CommandText =
                "update users set finger_print = @finger_print, fp_template = @fp_template where user_id= @userID";
            cmd.Parameters.AddWithValue("@userID", this.User);
            cmd.Parameters.AddWithValue("@finger_print", ParseImage(this.Image));
            cmd.Parameters.AddWithValue("@fp_template", this.Template);
            cmd.ExecuteNonQuery();
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete this print from the database
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            //using the user/finger/hand combo so that we dont end up with orphans
            SqlCommand cmd = POSStatic.Utilities.getCommand();

            cmd.CommandText = "update users set finger_print = null, fp_template = null where user_id= @userID";
            cmd.Parameters.AddWithValue("@userID", this.User);
            cmd.ExecuteNonQuery();
            log.LogMethodExit();
        }	
    }
}
