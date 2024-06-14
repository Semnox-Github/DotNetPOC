/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - frmHEDetails 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        12-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace ParafaitQueueManagement
{
    public partial class frmHEDetails : Form
    {
        HttpWebResponse response = null;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmHEDetails()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Common.Utilities = new Utilities();
            log.LogMethodExit();
        }

        private void frmHEDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public void LoadHEDetails(string tokenNumber)
        {
            log.LogMethodEntry(tokenNumber);
            //bool retval = false;
            string username = string.Empty;
            //double userID = 0.0;
            string day=string.Empty;
            string month=string.Empty;
            string year=string.Empty;
            string mobileNumber = string.Empty;
            string dob=string.Empty;
           
                HttpWebRequest webreq = (HttpWebRequest)HttpWebRequest.Create(Common.Utilities.getParafaitDefaults("EXTERNAL_GAME_SYSTEM_WEB_SERVICE_URL") + "get?token=" + tokenNumber);
                response = (HttpWebResponse)webreq.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream streamresponse = response.GetResponseStream();
                    StreamReader reader = new StreamReader(streamresponse);
                    string responseData = reader.ReadToEnd();
                    //  MessageBox.Show(responseData);
                    //  MessageBox.Show(response.ContentType);
                    JsonTextReader jsonreader = new JsonTextReader(new StringReader(responseData));
                    while (jsonreader.Read())
                    {
                        if (jsonreader.Value != null)
                        {
                            if (jsonreader.Value.ToString() == "username")
                            {
                                jsonreader.Read();
                                TxtUserName.Text = jsonreader.Value.ToString();
                            }
                            if (jsonreader.Value.ToString() == "id")
                            {
                                jsonreader.Read();
                                txtUserID.Text = jsonreader.Value.ToString();
                            }
                            if (jsonreader.Value.ToString() == "mobileNumber")
                            {
                                jsonreader.Read();
                                txtMobNo.Text = jsonreader.Value.ToString();
                            }
                            if (jsonreader.Value.ToString() == "dateOfBirthYear")
                            {
                                jsonreader.Read();
                                year = jsonreader.Value.ToString();
                            }
                            if (jsonreader.Value.ToString() == "dateOfBirthMonth")
                            {
                                jsonreader.Read();
                                month = jsonreader.Value.ToString();
                            }
                            if (jsonreader.Value.ToString() == "dateOfBirthDay")
                            {
                                jsonreader.Read();
                                day = jsonreader.Value.ToString();
                            }

                        }
                    }
                    dob = day + "-" + month + "-" + year;
                    txtDOB.Text = dob;
                }
            //using (XmlReader xmlrdr = XmlReader.Create(streamresponse))
            //{
            //    xmlrdr.ReadToFollowing("success");
            //    retval = xmlrdr.ReadElementContentAsBoolean();

            //    if (retval)
            //    {
            //        xmlrdr.ReadToFollowing("id");
            //        userID = xmlrdr.ReadElementContentAsDouble();
            //        txtUserID.Text = userID.ToString();

            //        xmlrdr.ReadToFollowing("username");
            //        username = xmlrdr.ReadElementContentAsString();

            //        xmlrdr.ReadToFollowing("mobileNumber");
            //        mobileNumber = xmlrdr.ReadElementContentAsString();
            //    }
            //}
            log.LogMethodExit();

            }
        }
    }

