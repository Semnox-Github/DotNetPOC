/********************************************************************************************
 * Project Name - Deployment Site Map UI
 * Description  - User interface for deployment plan site map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        01-Mar-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        27-Jul-2016   Raghuveera          Modified 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// user interface of publish to ui
    /// </summary>
    public partial class DeploymentSiteMapUI : Form
    {
        Utilities utilities;
        AutoPatchDepPlanDTO deploymentPlanDTO;
        ExecutionContext deploymentSiteUserContext = ExecutionContext.GetExecutionContext();
        //List<DeploymentSiteMapDTO> deploymentSiteMapDTOList;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="_Utilities"></param>
        /// <param name="_DeploymentPlanDTO">AutoPatchDepPlanDTO type parameter</param>
        public DeploymentSiteMapUI(Utilities _Utilities, AutoPatchDepPlanDTO _DeploymentPlanDTO)
        {
            log.Debug("Starts-DeploymentSiteMapUI(Utilities, _DeploymentPlanDTO) parameterized constructor.");
            InitializeComponent();
            utilities = _Utilities;
            deploymentPlanDTO = _DeploymentPlanDTO;
            
            deploymentSiteUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            deploymentSiteUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            log.Debug("Ends-DeploymentSiteMapUI(Utilities, _DeploymentPlanDTO) parameterized constructor.");
        }
        /// <summary>
        /// Loads the site details to the tree
        /// </summary>
        private void PopulateTree(object OrgId, object Orgname)
        {
            log.Debug("Starts-PopulateTree(" + OrgId + "," + Orgname + ") method.");
            TreeNode node = new TreeNode(Orgname.ToString());
            node.Tag = OrgId;
            tvOrganization.Nodes.Add(node);            
            GetNodes(node);
            if (tvOrganization.Nodes.Count > 0)
            {
                tvOrganization.Nodes[0].ExpandAll();
                tvOrganization.Nodes[0].Text = tvOrganization.Nodes[0].Text; // reassign to set proper width for text
            }
            utilities.setLanguage(tvOrganization);
            log.Debug("Ends-PopulateTree(" + OrgId + "," + Orgname + ") method.");
        }


        TreeNode[] GetChildren(int parentOrgId)
        {
            log.Debug("Starts-GetChildren(" + parentOrgId + ") method.");
            OrganizationList organizationList = new OrganizationList();
            List<OrganizationDTO> organizationDTOList = new List<OrganizationDTO>();
            List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>> organizationSearchParams = new List<KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>>();
            organizationSearchParams.Add(new KeyValuePair<OrganizationDTO.SearchByOrganizationParameters, string>(OrganizationDTO.SearchByOrganizationParameters.PARENT_ORG_ID, parentOrgId.ToString()));
            organizationDTOList = organizationList.GetAllOrganizations(organizationSearchParams);
            
            if (organizationDTOList == null || organizationDTOList.Count == 0)
            {
                log.Debug("GetChildren(" + parentOrgId + ") method entered. if(organizationDTOList == null || organizationDTOList.Count == 0)");
                SiteList siteList = new SiteList(deploymentSiteUserContext);
                List<SiteDTO> siteDTOList = new List<SiteDTO>();
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> siteSearchParams = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                siteSearchParams.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.ORG_ID, parentOrgId.ToString()));
                siteDTOList = siteList.GetAllSites(siteSearchParams);
                if (siteDTOList == null)
                {
                    log.Debug("Ends-GetChildren(parentOrgId) method in if(organizationDTOList == null || organizationDTOList.Count == 0)&&if (siteDTOList == null) part.");
                    return null;
                }
                else
                {
                    log.Debug("Total " + siteDTOList.Count + " of sites are fetched belongs to OrgId:" + parentOrgId);
                    TreeNode[] tnCollection = new TreeNode[siteDTOList.Count];

                    for (int i = 0; i < siteDTOList.Count; i++)
                    {
                        log.Debug("Creating Tree node for site :" + siteDTOList[i].SiteName);
                        tnCollection[i] = new TreeNode(((siteDTOList[i].SiteCode != -1) ? siteDTOList[i].SiteCode.ToString() + " - " : "") + siteDTOList[i].SiteName);//Modification on 27-Jul-2016 added the site id with site name
                        tnCollection[i].Tag = siteDTOList[i].SiteId;
                        tnCollection[i].ForeColor = Color.Blue;
                    }
                    log.Debug("Ends-GetChildren(parentOrgId) method in if(organizationDTOList == null || organizationDTOList.Count == 0)&&if (siteDTOList != null) part.");
                    return (tnCollection);
                }
            }
            else
            {
                log.Debug("Total " + organizationDTOList.Count + " of organizations are fetched belongs to parentOrgId:" + parentOrgId);
                log.Debug("GetChildren(" + parentOrgId + ") method entered. Else part of if(organizationDTOList == null || organizationDTOList.Count == 0)");
                TreeNode[] tnCollection = new TreeNode[organizationDTOList.Count];

                for (int i = 0; i < organizationDTOList.Count; i++)
                {
                    log.Debug("Creating Tree node for Organization :" + organizationDTOList[i].OrgName);
                    tnCollection[i] = new TreeNode(organizationDTOList[i].OrgName);
                    tnCollection[i].Tag = organizationDTOList[i].OrgId;
                    tnCollection[i].ForeColor = Color.Gray;
                }
                log.Debug("Ends-GetChildren(parentOrgId) method in else part of if(organizationDTOList == null || organizationDTOList.Count == 0).");
                return (tnCollection);
            }
        }
        TreeNode GetNodes(TreeNode rootNode)
        {
            log.Debug("Starts-GetNodes(" + rootNode .Tag+ ") method.");
            TreeNode[] tn = GetChildren(Convert.ToInt32(rootNode.Tag));
            if (tn == null)
            {
                log.Debug("Ends-GetNodes(" + rootNode.Tag + ") method if(tn == null).");
                return null;
            }
            else
            {
                foreach (TreeNode tnode in tn)
                {
                    TreeNode node = GetNodes(tnode);
                    if (node == null)
                        rootNode.Nodes.Add(tnode);
                    else
                        rootNode.Nodes.Add(node);
                }
                log.Debug("Ends-GetNodes(" + rootNode.Tag + ") method returns rootNode.");
                return (rootNode);
            }
        }
        private void CallRecursive(TreeNode treeNode)
        {
            log.Debug("Starts-CallRecursive(treeNode) method.");
            if (treeNode.Checked && treeNode.ForeColor == Color.Blue)
            {
                PublishDeploymentPlan(deploymentPlanDTO.PatchDeploymentPlanId, (int)treeNode.Tag);
            }

            foreach (TreeNode tn in treeNode.Nodes)
            {
                CallRecursive(tn);
            }
            log.Debug("Ends-CallRecursive(treeNode) method.");
        }
        private void PublishDeploymentPlan(int patchDeploymentPlanId, int siteId)
        {
            log.Debug("Starts-PublishDeploymentPlan(patchDeploymentPlanId,siteId) method.");
            AutoPatchApplTypeList autoPatchApplTypeList = new AutoPatchApplTypeList();
            List<AutoPatchApplTypeDTO> autoPatchApplTypeDTOList = new List<AutoPatchApplTypeDTO>();
            List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>> autoPatchApplTypeSearchParams = new List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>>();

            AutoPatchDepPlanList autoPatchDepPlanList = new AutoPatchDepPlanList();
            List<AutoPatchDepPlanDTO> autoPatchDepPlanDTOList = new List<AutoPatchDepPlanDTO>();
            List<KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>> depPlanSearchParams = new List<KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>>();

            AutoPatchDepPlanApplicationList autoPatchDepPlanApplicationList = new AutoPatchDepPlanApplicationList();
            List<AutoPatchDepPlanApplicationDTO> autoPatchDepPlanSourceApplicationDTOList = new List<AutoPatchDepPlanApplicationDTO>();
            List<AutoPatchDepPlanApplicationDTO> autoPatchDepPlanMasterApplicationDTOList = new List<AutoPatchDepPlanApplicationDTO>();
            List<AutoPatchDepPlanApplicationDTO> autoPatchDepPlanApplicationDTOList = new List<AutoPatchDepPlanApplicationDTO>();
            List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>> depPlanApplicationSearchParams = new List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>>();
            AutoPatchDepPlanDTO autoPatchDepPlanDTO = new AutoPatchDepPlanDTO();
            //autoPatchDepPlanDTO = deploymentPlanDTO;
            //assigning the deployment plan values to local deployment plan values.
            autoPatchDepPlanDTO.DeploymentPlanName = deploymentPlanDTO.DeploymentPlanName;
            autoPatchDepPlanDTO.DeploymentPlannedDate = deploymentPlanDTO.DeploymentPlannedDate;
            autoPatchDepPlanDTO.DeploymentStatus = deploymentPlanDTO.DeploymentStatus;
            autoPatchDepPlanDTO.IsActive =deploymentPlanDTO.IsActive;
            autoPatchDepPlanDTO.PatchFileName = deploymentPlanDTO.PatchFileName;            
           // autoPatchDepPlanDTO.Siteid = siteId;
            deploymentSiteUserContext.SetSiteId(siteId);
            autoPatchDepPlanDTO.MasterEntityId = patchDeploymentPlanId;

            depPlanSearchParams.Add(new KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.SITE_ID, siteId.ToString()));
            depPlanSearchParams.Add(new KeyValuePair<AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters, string>(AutoPatchDepPlanDTO.SearchByAutoPatchDepPlanParameters.MASTER_ENTITY_ID, patchDeploymentPlanId.ToString()));
            autoPatchDepPlanDTOList = autoPatchDepPlanList.GetAllAutoPatchDepPlans(depPlanSearchParams);

            if (autoPatchDepPlanDTOList == null)
            {
                autoPatchDepPlanDTO.PatchDeploymentPlanId = -1;
            }
            else
            {
                autoPatchDepPlanDTO.PatchDeploymentPlanId = autoPatchDepPlanDTOList[0].PatchDeploymentPlanId;
            }
            AutoPatchDeploymentPlan autoPatchDeploymentPlan = new AutoPatchDeploymentPlan(autoPatchDepPlanDTO);
            autoPatchDeploymentPlan.Save();
            depPlanApplicationSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_ID, patchDeploymentPlanId.ToString()));
            depPlanApplicationSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.SITE_ID, deploymentPlanDTO.Siteid.ToString()));
            autoPatchDepPlanMasterApplicationDTOList = autoPatchDepPlanApplicationList.GetAllAutoPatchDepPlanApplications(depPlanApplicationSearchParams);
            if (autoPatchDepPlanMasterApplicationDTOList != null)
            {
                foreach (AutoPatchDepPlanApplicationDTO autoPatchDepPlanApplicationDTO in autoPatchDepPlanMasterApplicationDTOList)
                {
                    //Fetching the application if it is already saved
                    depPlanApplicationSearchParams = new List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>>();
                    depPlanApplicationSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_ID, autoPatchDepPlanDTO.PatchDeploymentPlanId.ToString()));
                    depPlanApplicationSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.MASTER_ENTITY_ID, autoPatchDepPlanApplicationDTO.PatchDeploymentPlanApplicationId.ToString()));
                    depPlanApplicationSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.SITE_ID, autoPatchDepPlanDTO.Siteid.ToString()));
                    autoPatchDepPlanApplicationDTOList = autoPatchDepPlanApplicationList.GetAllAutoPatchDepPlanApplications(depPlanApplicationSearchParams);
                    if (autoPatchDepPlanApplicationDTOList != null)
                    {
                        if (autoPatchDepPlanApplicationDTOList.Count == 1)
                        {
                            //Checking for master appliction
                            depPlanApplicationSearchParams = new List<KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>>();                            
                            depPlanApplicationSearchParams.Add(new KeyValuePair<AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters, string>(AutoPatchDepPlanApplicationDTO.SearchByAutoPatchDepPlanApplicationParameters.PATCH_DEPLOYMENT_PLAN_APPLICATION_ID, autoPatchDepPlanApplicationDTOList[0].MasterEntityId.ToString()));                            
                            autoPatchDepPlanSourceApplicationDTOList = autoPatchDepPlanApplicationList.GetAllAutoPatchDepPlanApplications(depPlanApplicationSearchParams);
                            if (autoPatchDepPlanSourceApplicationDTOList.Count == 1)
                            {
                                autoPatchDepPlanApplicationDTOList[0].DeploymentVersion = autoPatchDepPlanSourceApplicationDTOList[0].DeploymentVersion;
                                autoPatchApplTypeSearchParams = new List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>>();
                                autoPatchApplTypeSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.PATCH_APPLICATION_TYPE_ID, autoPatchDepPlanApplicationDTO.PatchApplicationTypeId.ToString()));
                                autoPatchApplTypeSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID, deploymentPlanDTO.Siteid.ToString()));
                                autoPatchApplTypeDTOList = autoPatchApplTypeList.GetAllAutoPatchApplTypes(autoPatchApplTypeSearchParams);
                                if (autoPatchApplTypeDTOList != null && autoPatchApplTypeDTOList.Count == 1)
                                {
                                    autoPatchApplTypeSearchParams = new List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>>();
                                    autoPatchApplTypeSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.APPLICATION_TYPE, autoPatchApplTypeDTOList[0].ApplicationType));
                                    autoPatchApplTypeSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID, siteId.ToString()));
                                    autoPatchApplTypeDTOList = autoPatchApplTypeList.GetAllAutoPatchApplTypes(autoPatchApplTypeSearchParams);
                                    if (autoPatchApplTypeDTOList != null && autoPatchApplTypeDTOList.Count == 1)
                                    {
                                        autoPatchDepPlanApplicationDTOList[0].PatchApplicationTypeId = autoPatchApplTypeDTOList[0].PatchApplicationTypeId;                                        
                                    }                                    
                                }                               
                                
                                autoPatchDepPlanApplicationDTOList[0].MinimumVersionRequired = autoPatchDepPlanSourceApplicationDTOList[0].MinimumVersionRequired;
                                autoPatchDepPlanApplicationDTOList[0].UpgradeType = autoPatchDepPlanSourceApplicationDTOList[0].UpgradeType;
                                autoPatchDepPlanApplicationDTOList[0].IsActive = autoPatchDepPlanSourceApplicationDTOList[0].IsActive;
                            }
                            AutoPatchDepPlanApplication autoPatchDeploymentPlanApplication = new AutoPatchDepPlanApplication(autoPatchDepPlanApplicationDTOList[0]);
                            autoPatchDeploymentPlanApplication.Save();
                        }
                    }
                    else
                    {//If not saved
                        autoPatchApplTypeSearchParams = new List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>>();
                        autoPatchApplTypeSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.PATCH_APPLICATION_TYPE_ID, autoPatchDepPlanApplicationDTO.PatchApplicationTypeId.ToString()));
                        autoPatchApplTypeSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID, deploymentPlanDTO.Siteid.ToString()));
                        autoPatchApplTypeDTOList = autoPatchApplTypeList.GetAllAutoPatchApplTypes(autoPatchApplTypeSearchParams);
                        if (autoPatchApplTypeDTOList != null && autoPatchApplTypeDTOList.Count == 1)
                        {
                            autoPatchApplTypeSearchParams = new List<KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>>();
                            autoPatchApplTypeSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.APPLICATION_TYPE, autoPatchApplTypeDTOList[0].ApplicationType));
                            autoPatchApplTypeSearchParams.Add(new KeyValuePair<AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters, string>(AutoPatchApplTypeDTO.SearchByAutoPatchApplTypeParameters.SITE_ID, siteId.ToString()));
                            autoPatchApplTypeDTOList = autoPatchApplTypeList.GetAllAutoPatchApplTypes(autoPatchApplTypeSearchParams);
                            if (autoPatchApplTypeDTOList != null && autoPatchApplTypeDTOList.Count == 1)
                            {
                                autoPatchDepPlanApplicationDTO.PatchApplicationTypeId = autoPatchApplTypeDTOList[0].PatchApplicationTypeId;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        autoPatchDepPlanApplicationDTO.MasterEntityId = autoPatchDepPlanApplicationDTO.PatchDeploymentPlanApplicationId;
                        autoPatchDepPlanApplicationDTO.PatchDeploymentPlanApplicationId = -1;
                        autoPatchDepPlanApplicationDTO.PatchDeploymentPlanId = autoPatchDepPlanDTO.PatchDeploymentPlanId;
                        autoPatchDepPlanApplicationDTO.Siteid = siteId;
                        AutoPatchDepPlanApplication autoPatchDeploymentPlanApplication = new AutoPatchDepPlanApplication(autoPatchDepPlanApplicationDTO);
                        autoPatchDeploymentPlanApplication.Save();
                    }
                }

            }
            log.Debug("Ends-PublishDeploymentPlan(patchDeploymentPlanId,siteId) method.");
        }

        private void DeploymentSiteMapUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-DeploymentSiteMapUI_Load() event.");
            tvOrganization.Nodes.Clear();
            if (deploymentPlanDTO == null)
            {
                tvOrganization.CheckBoxes = false;
            }
            OrganizationList organizationList = new OrganizationList();
            List<OrganizationDTO> organizationDTOList = new List<OrganizationDTO>();
            organizationDTOList = organizationList.GetRootOrganizationList();
            if (organizationDTOList != null)
            {
                foreach (OrganizationDTO organizationDTO in organizationDTOList)
                {
                    log.Debug("DeploymentSiteMapUI_Load() event.");
                    PopulateTree(organizationDTO.OrgId, organizationDTO.OrgName);
                }
            }

            lblPublishObject.Text = "";
            lblPublishObject.Text = "Publishing" + Environment.NewLine + Environment.NewLine + "Plan Id : " + deploymentPlanDTO.PatchDeploymentPlanId + Environment.NewLine + Environment.NewLine + "Deployment Plan : " + deploymentPlanDTO.DeploymentPlanName ;

            if (deploymentPlanDTO == null)
            {
                btnPublish.Visible = btnClose.Visible = lblPublishObject.Visible = false;
                this.Width = panelTree.Width;
                panelTree.Dock = DockStyle.Fill;
            }
            log.Debug("Ends-DeploymentSiteMapUI_Load() event.");
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnPublish_Click() event.");
            if (deploymentPlanDTO != null && deploymentPlanDTO.PatchDeploymentPlanId > -1)
            {
                foreach (TreeNode tn in tvOrganization.Nodes)
                {
                    CallRecursive(tn);
                }
                deploymentSiteUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                MessageBox.Show(deploymentPlanDTO.DeploymentPlanName +" "+ utilities.MessageUtils.getMessage(691));
            }
            log.Debug("Ends-btnPublish_Click() event.");
        }

        private void tvOrganization_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode tn in e.Node.Nodes)
            {
                tn.Checked = e.Node.Checked;
            }
        }
    }
}
