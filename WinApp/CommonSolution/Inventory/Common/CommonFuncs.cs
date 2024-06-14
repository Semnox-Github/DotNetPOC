/********************************************************************************************
 * Project Name - Inventory
 * Description  - Commom Function
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
  *2.70.2        13-Aug-2019   Deeksha       Added logger methods.
  *2.100.0       13-Sep-2020   Deeksha       Added a method to Set the related UOM's for each row in the data grid view combo box
  *2.130.10      18-Aug-2022   Vignesh Bhat  Reconcile EzeeInventory register reader method to support all devices supported by Management studio
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public class CommonFuncs
    {
        public static Utilities Utilities;
        public static ParafaitEnv ParafaitEnv;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, UOMContainer> uomContainerDictionary = new ConcurrentDictionary<int, UOMContainer>();
        public static DeviceClass readerDevice;
        public static void setSiteIdFilter(BindingSource bs)
        {
            log.LogMethodEntry();
            try
            {
                //if (Utilities.ParafaitEnv.IsCorporate) // corporate db
                if (ParafaitEnv.IsCorporate) // corporate db
                {
                    if (bs.Filter == null || bs.Filter == "")
                        bs.Filter = "site_id = " + ParafaitEnv.SiteId.ToString();
                    else
                        bs.Filter = "(" + bs.Filter + ") and site_id = " + ParafaitEnv.SiteId.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing setSiteIdFilter()" + ex.Message);
                MessageBox.Show(ex.Message, "Binding Source: " + bs.DataMember);
            }
            log.LogMethodExit();
        }

        public static object getSiteid()
        {
            log.LogMethodEntry();
            if (ParafaitEnv.IsCorporate)
            {
                //return CommonFuncs.Utilities.ParafaitEnv.SiteId;
                log.LogMethodExit();
                return ParafaitEnv.SiteId;
            }
            else
            {
                log.LogMethodExit();
                return DBNull.Value;
            }
            //return -1;
        }

        public static void displayInfo(string info) // display in info labl of toolstrip
        {
            log.LogMethodEntry(info);
            try
            {
                Form f = Application.OpenForms["ParentMDI"];
                if (f != null)
                {
                    Control[] ctrls = f.Controls.Find("fillBy3ToolStrip", true);
                    if (ctrls.Length > 0)
                    {
                        ToolStrip st = ctrls[0] as ToolStrip;
                        ToolStripItem lbl = st.Items["toolStripLabelMessage"];
                        lbl.Text = info;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing displayInfo()" + ex.Message);
            }
            log.LogMethodExit();
        }

        public string getNextSeqNo(string sequenceName, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(sequenceName, SQLTrx);
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = @"declare @value varchar(20)
                                exec GetNextSeqValue N'" + sequenceName + "', @value out, -1 "
                                + " select @value";
            try
            {
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    log.LogMethodExit();
                    return (o.ToString());
                }
                else
                {
                    log.LogMethodExit("-1");
                    return "-1";
                }
            }
            catch(Exception ex)
            {
                log.Error("Error while executing getNextSeqNo()" + ex.Message);
                log.LogMethodExit("-1");
                return "-1";
            }
        }

        public static void LoadToSite(ComboBox cmbTosite, bool isIncludingCurrentSite, bool loadOnlyMasterSite = false)
        {
            log.LogMethodEntry(cmbTosite, isIncludingCurrentSite, loadOnlyMasterSite);
            if (Utilities.getParafaitDefaults("ENABLE_INTER_STORE_ADJUSTMENT").Equals("Y"))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
                string message = "";
                SiteList siteList = new SiteList(ExecutionContext.GetExecutionContext());
                List<SiteDTO> siteDTOList;
                List<SiteDTO> siteDTOFilterList;
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchBySiteParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                searchBySiteParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, Utilities.ParafaitEnv.SiteId.ToString()));
                siteDTOList = siteList.GetAllSites(searchBySiteParameters);
                if (siteDTOList != null)
                {
                    try
                    {
                        if (Utilities.ParafaitEnv.IsMasterSite && Utilities.ParafaitEnv.IsCorporate)
                        {
                            siteDTOList = siteList.GetAllSites(((isIncludingCurrentSite) ? -1 : Utilities.ParafaitEnv.SiteId), siteDTOList[0].OrgId, siteDTOList[0].CompanyId);
                            if (siteDTOList == null)
                            {
                                siteDTOList = new List<SiteDTO>();
                            }
                            if (loadOnlyMasterSite)
                            {
                                siteDTOFilterList = siteDTOList.Where(x => x.IsMasterSite).ToList<SiteDTO>();
                            }
                            else
                            {
                                siteDTOFilterList = siteDTOList;
                            }
                            siteDTOFilterList.Insert(0, new SiteDTO());
                            siteDTOFilterList[0].SiteId = -1;
                            siteDTOFilterList[0].SiteName = "<SELECT>";
                            if (siteDTOFilterList != null && siteDTOFilterList.Count > 0)
                            {
                                cmbTosite.DataSource = siteDTOFilterList;
                                cmbTosite.ValueMember = "SiteId";
                                cmbTosite.DisplayMember = "SiteName";
                                cmbTosite.Tag = "T";
                            }
                        }
                        else
                        {
                            ParafaitGateway.SiteDTO[] hqSiteDTOArray;
                            ParafaitGateway.Service parafaitGateway = new ParafaitGateway.Service();
                            parafaitGateway.Url = Utilities.getParafaitDefaults("WEBSERVICE_UPLOAD_URL");
                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };//certificate validation procedure for the SSL/TLS secure channel   
                            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers

                            Utilities.ParafaitEnv.getCompanyLoginKey();
                            hqSiteDTOArray = parafaitGateway.GetAllSite(Utilities.ParafaitEnv.CompanyLoginKey, (isIncludingCurrentSite) ? -1 : Utilities.ParafaitEnv.SiteId, siteDTOList[0].OrgId, siteDTOList[0].CompanyId, ref message);
                            if (hqSiteDTOArray == null)
                            {
                                hqSiteDTOArray = new ParafaitGateway.SiteDTO[1];
                            }
                            if (hqSiteDTOArray.Length == 0)
                            {
                                log.Error("LoadToSite() constructor.Web service not returned the data.");
                                MessageBox.Show("There is a problem while loading other store details. Due to this you may not be able to proceed with inter store transfer operation. " + message);
                            }
                            else
                            {
                                List<ParafaitGateway.SiteDTO> hqSiteDTOList = hqSiteDTOArray.ToList<ParafaitGateway.SiteDTO>();                                
                                List<ParafaitGateway.SiteDTO> sitesList;
                                if (loadOnlyMasterSite)
                                {
                                    sitesList = hqSiteDTOList.Where(x => x.IsMasterSite).ToList<ParafaitGateway.SiteDTO>();
                                }
                                else
                                {
                                    sitesList = hqSiteDTOList;
                                }
                                sitesList.Insert(0, new ParafaitGateway.SiteDTO());
                                sitesList[0].SiteId = -1;
                                sitesList[0].SiteName = "<SELECT>";


                                cmbTosite.DataSource = sitesList;
                                cmbTosite.ValueMember = "SiteId";
                                cmbTosite.DisplayMember = "SiteName";
                                cmbTosite.Tag = "T";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("LoadToSite() constructor.Inter store details load error:" + ex.ToString());
                        MessageBox.Show("There is a problem while loading other store details. There might be a problem to do inter store transfer.");
                    }
                }
                else
                {
                    log.Error("LoadToSite() constructor.Unable to load current site details");
                    MessageBox.Show("There is a problem while loading other store details. Due to this you may not be able to proceed with inter store transfer operation.");
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to get the UOM Container
        /// </summary>
        /// <returns></returns>
        public static UOMContainer GetUOMContainer()
        {
            log.LogMethodEntry();
            if (uomContainerDictionary.ContainsKey(Utilities.ExecutionContext.GetSiteId()) == false)
            {
                uomContainerDictionary[Utilities.ExecutionContext.GetSiteId()] = new UOMContainer(Utilities.ExecutionContext);
            }
            UOMContainer result = uomContainerDictionary[Utilities.ExecutionContext.GetSiteId()];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Method to Set the related UOM's for each row in the data grid view combo box
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="index"></param>
        /// <param name="uomId"></param>
        public static void GetUOMComboboxForSelectedRows(DataGridView dgv, int index, int uomId)
        {
            log.LogMethodEntry(index, uomId);
            try
            {
                UOMList uomListBL = new UOMList(Utilities.ExecutionContext);
                DataGridViewComboBoxCell cbcell = dgv.Rows[index].Cells["cmbUOM"] as DataGridViewComboBoxCell;
                if (cbcell.DataSource != null)
                {
                    cbcell = new DataGridViewComboBoxCell();
                }
                UOMContainer uomcontainer = GetUOMContainer();
                if (uomcontainer.relatedUOMDTOList != null)
                {
                    List<UOMDTO> uomDTOList = uomcontainer.relatedUOMDTOList.FirstOrDefault(x => x.Key == uomId).Value;
                    //List<UOMDTO> uomDTOList = uomcontainer.GetRelatedUOMList(uomId);
                    if (uomDTOList != null && uomDTOList.Any())
                    {
                        cbcell.DataSource = uomDTOList;
                        cbcell.ValueMember = "UOMId";
                        cbcell.DisplayMember = "UOM";
                        dgv.Rows[index].Cells["cmbUOM"].Value = uomId;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
