/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Screen Model class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Redemption_Kiosk
{
    /// <summary>
    /// Logic ScreenModel class.
    /// </summary>
    public class ScreenModel
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal int ScreenId;
        internal string ScreenName;
        internal string CodeObjectName;
        internal string ScreenKey;

        /// <summary>
        /// UIPanel class.
        /// </summary>
        internal class UIPanel
        {
            internal int PanelId;
            internal string PanelName;
            internal int PanelIndex;
            internal int PanelWidth = -1;
            internal int ScreenUIPanelId;
            internal List<UIPanelElement> Elements = new List<UIPanelElement>();

            /// <summary>
            /// Get Element by index value
            /// </summary>
            /// <param name="index"></param>
            /// <returns>UIPanelElement object</returns>
            internal UIPanelElement getElementByIndex(int index)
            {
                return Elements.Find(x => x.ElementIndex == index);
            }
        }
        /// <summary>
        /// UIPanelElement class.
        /// </summary>
        internal class UIPanelElement
        {
            internal int UIPanelElementId;
            internal string ElementName;
            internal int ElementIndex;
            internal string Action;
            internal int ActionScreenId = -1;
            internal UIPanelElementAttribute Attribute = new UIPanelElementAttribute();

            internal List<ElementParameter> Parameters = new List<ElementParameter>();
            internal List<ElementParameter> LeftParameters = new List<ElementParameter>();
            internal List<ElementParameter> RightParameters = new List<ElementParameter>();

            internal List<ElementParameter> AllParameters
            {
                get
                {
                    List<ElementParameter> lclList = new List<ElementParameter>();
                    lclList.AddRange(Parameters);
                    lclList.AddRange(LeftParameters);
                    lclList.AddRange(RightParameters);
                    return lclList;
                }
            }

            internal List<ElementParameter> VisibleParameters
            {
                get
                {
                    return Parameters.FindAll(x => x.DisplayIndex > 0);
                }
            }
            internal UIPanelElement()//To add suggestive sale product to the checkout screen model
            { }
            internal UIPanelElement(int uIPanelElementId,
                                    string elementName,
                                    string action,
                                    int actionScreenId,
                                    int elementIndex,
                                    string displayText,
                                    string actionScreenTitle1,
                                    string actionScreenTitle2,
                                    string actionScreenFooter1,
                                    string actionScreenFooter2,
                                    Image displayImage)
            {
                UIPanelElementId = uIPanelElementId;
                ElementName = elementName;
                Action = action;
                ActionScreenId = actionScreenId;
                ElementIndex = elementIndex;
                Attribute.DisplayText = displayText;
                Attribute.ActionScreenTitle1 = actionScreenTitle1;
                Attribute.ActionScreenTitle2 = actionScreenTitle2;
                Attribute.ActionScreenFooter1 = actionScreenFooter1;
                Attribute.ActionScreenFooter2 = actionScreenFooter2;
                Attribute.DisplayImage = displayImage;

                GetParameters();
            }
            /// <summary>
            /// Get Parameters 
            /// </summary>
            internal void GetParameters()
            {
                log.LogMethodEntry();
                using (Utilities utils = new Utilities())
                {
                    DataTable dt = utils.executeDataTable(@"select ParameterId, ParameterName, Parameter, 
                                                                SQLBindName, DisplayIndex, ParentParameterId,
                                                            (select top 1 DisplayText1 
                                                            from AppUIElementParameterAttribute a 
                                                            where a.ParameterId = p.ParameterId 
                                                              and (LanguageId = @LanguageId or LanguageId is null) 
                                                            order by LanguageId desc) DisplayText1,
                                                            (select top 1 DisplayText2
                                                            from AppUIElementParameterAttribute a 
                                                            where a.ParameterId = p.ParameterId 
                                                              and (LanguageId = @LanguageId or LanguageId is null) 
                                                            order by LanguageId desc) DisplayText2,
                                                            (select top 1 DisplayImage
                                                            from AppUIElementParameterAttribute a 
                                                            where a.ParameterId = p.ParameterId 
                                                              and (LanguageId = @LanguageId or LanguageId is null) 
                                                            order by LanguageId desc) DisplayImage,
                                                              ActionScreenId, ScreenGroup
                                                              from AppUIElementParameters p
                                                            where UIPanelElementId = @UIPanelElementId 
                                                            and p.ActiveFlag = 1
                                                            order by case when ParentParameterId is null then p.DisplayIndex else isnull(p.DisplayIndex, 9999) end",
                                                        new SqlParameter("@UIPanelElementId", UIPanelElementId),
                                                        new SqlParameter("@LanguageId", Common.parafaitEnv.LanguageId));
                    log.Debug("dt.Rows.Count :" + dt.Rows.Count);
                    foreach (DataRow dr in dt.Rows)
                    {
                        int displayIndex = 0;
                        if (dr["DisplayIndex"] != DBNull.Value)
                        {
                            displayIndex = Convert.ToInt32(dr["DisplayIndex"]);
                        }

                        int actionScreenId = -1;
                        if (dr["actionScreenId"] != DBNull.Value)
                        {
                            actionScreenId = Convert.ToInt32(dr["actionScreenId"]);
                        }

                        int screenGroup = -1;
                        if (dr["screenGroup"] != DBNull.Value)
                        {
                            screenGroup = Convert.ToInt32(dr["screenGroup"]);
                        }

                        Image DisplayImage = null;//Goody bag changes
                        if (dr["DisplayImage"] != DBNull.Value)
                        {
                            var image = (byte[])(dr["DisplayImage"]);
                            using (var stream = new System.IO.MemoryStream(image))
                            {
                                DisplayImage = Image.FromStream(stream);
                            }
                        }

                        ElementParameter parameter = new ElementParameter(this,
                                                                        displayIndex,
                                                                        dr["DisplayText1"].ToString(),
                                                                        dr["DisplayText2"].ToString(),
                                                                        dr["Parameter"].ToString(),
                                                                        Convert.ToInt32(dr["ParameterId"]),
                                                                        dr["ParameterName"].ToString(),
                                                                        dr["SQLBindName"].ToString(),
                                                                        dr["ParentParameterId"],
                                                                        actionScreenId,
                                                                        screenGroup,
                                                                        DisplayImage);//Goody bag changes

                        Parameters.Add(parameter);
                    }

                    foreach (ElementParameter parameter in Parameters)
                    {
                        if (parameter._ParentParameterId != DBNull.Value)
                        {
                            parameter.ParentParameter = GetParameterById(Convert.ToInt32(parameter._ParentParameterId));
                            parameter.Toplevel = false;
                        }
                    }

                    foreach (ElementParameter parameter in Parameters)
                    {
                        parameter.GetDataSource();
                    }
                }
                log.LogMethodExit();
            }

            internal UIPanelElement Clone()
            {
                log.LogMethodEntry();
                UIPanelElement uiPanelElement = new UIPanelElement(UIPanelElementId,
                                            ElementName,
                                            Action,
                                            ActionScreenId,
                                            ElementIndex,
                                            Attribute.DisplayText,
                                            Attribute.ActionScreenTitle1,
                                            Attribute.ActionScreenTitle2,
                                            Attribute.ActionScreenFooter1,
                                            Attribute.ActionScreenFooter2,
                                            Attribute.DisplayImage);//Goody bag changes

                log.LogMethodExit(uiPanelElement);
                return uiPanelElement;
            }

            internal ElementParameter GetParameterById(int ParameterId)
            {
                log.LogMethodEntry(ParameterId);
                ElementParameter elementParameter = Parameters.Find(x => x.ParameterId == ParameterId);
                log.LogMethodExit(elementParameter);
                return elementParameter;
            }
        }
        /// <summary>
        /// UIPanelElementAttribute class.
        /// </summary>
        internal class UIPanelElementAttribute
        {
            internal string DisplayText;
            internal string ActionScreenTitle1;
            internal string ActionScreenTitle2;
            internal string ActionScreenFooter1;
            internal string ActionScreenFooter2;
            internal Image DisplayImage;
        }

        internal enum ParameterUIType
        {
            Default,
            ChoiceButton
        }

        internal enum ParameterType
        {
            Default,
            Combo,
            CardSale,
        }
        /// <summary>
        /// ElementParameter class.
        /// </summary>
        internal class ElementParameter
        {
            internal string identifier = "";
            internal int ParameterId = -1;
            internal string ParameterName;
            internal string Parameter;
            internal object _ParentParameterId;
            internal ElementParameter ParentParameter;
            internal string SQLBindName;
            internal int DisplayIndex;
            internal string DisplayText1;
            internal string DisplayText2;
            internal Image DisplayImage;//Goody bag changes
            internal int ActionScreenId = -1;
            internal int ScreenGroup = -1;

            internal ParameterUIType UIType = ParameterUIType.Default;

            internal DataTable DataSource = null;
            internal object UserSelectedValue = null;
            internal object OrderedValue = null;
            internal int UserQuantity = 1;
            internal int OrderedQuantity = 0;
            //internal decimal UserPrice;

            //internal string CardNumber;
            //internal int CardCount;

            internal bool QuantityLimit = false;
            internal bool Toplevel = true;

            internal bool DefaultSelected = false;
            internal bool Disabled = false;
            internal bool HalfHalf = true;
            internal object ModifierSetId = null;
            internal int FreeQuantity = 0;
            internal int MaxQuantity = 0;//Subsale max min quantity validation
            internal int MinQuantity = 0;//Subsale max min quantity validation
            internal bool FreeAvailed = false;

            internal bool Exploded = false;

            internal UIPanelElement OwningElement;

            internal System.Windows.Forms.ComboBox UIControl;

            internal ParameterType ParameterType = ParameterType.Default;

            internal void UserSelectedEvent(object userData)
            {
                log.LogMethodEntry(userData);
                UserSelectedValue = userData;
                if (Toplevel)
                {
                    foreach (ElementParameter parameter in OwningElement.Parameters)
                    {
                        if (this == parameter.ParentParameter)
                        {
                            parameter.GetDataSource();
                            if (parameter.UIControl != null && parameter.UIType == ParameterUIType.Default)
                            {
                                parameter.UIControl.DataSource = parameter.DataSource;
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }

            internal ElementParameter()
            {

            }

            internal ElementParameter(UIPanelElement owningElement,
                                        int displayIndex,
                                        string displayText1,
                                        string displayText2,
                                        string parameter,
                                        int parameterId,
                                        string parameterName,
                                        string sQLBindName,
                                        object parentParameterId,
                                        int actionScreenId,
                                        int screenGroup,
                                        Image displayImage)//Goody bag changes
            {
                log.LogMethodEntry(owningElement, displayIndex,  displayText1,  displayText2, parameter, parameterId,  parameterName,  sQLBindName,
                                         parentParameterId, actionScreenId, screenGroup,  displayImage);
                OwningElement = owningElement;

                DisplayIndex = displayIndex;
                DisplayText1 = displayText1;
                DisplayText2 = displayText2;
                Parameter = parameter;
                ParameterId = parameterId;
                DisplayImage = displayImage;//Goody bag changes
                identifier = parameter.ToString();
                ParameterName = parameterName;
                SQLBindName = sQLBindName;
                _ParentParameterId = parentParameterId;
                ActionScreenId = actionScreenId;
                ScreenGroup = screenGroup;
                log.LogMethodExit();
            }

            internal void GetDataSource()
            {
                log.LogMethodEntry();
                /* columns definition:
                 * In case of parent product
                 * 1: id
                 * 2: description
                 * 3: half-half allowed in case of pizza (0 or 1)

                 * In case of Modifier
                 * 1: id
                 * 2: description
                 * 3: default selected (0 or 1)
                 * 4: modifier set id
                 * 5: enabled in UI for edit (0 or 1)
                 * 6: half-half allowed in case of pizza (0 or 1)
                 */

                using (Utilities utils = new Utilities())
                {
                    if (ParentParameter == null || ParentParameter.SQLBindName.Trim() == "")
                    {
                        DataSource = utils.executeDataTable(Parameter);
                    }
                    else
                    {
                        DataSource = utils.executeDataTable(Parameter,
                            new SqlParameter(ParentParameter.SQLBindName, ParentParameter.UserSelectedValue == null ? DBNull.Value : ParentParameter.UserSelectedValue));
                    }
                }

                UIType = ParameterUIType.Default;
                if (DataSource.Rows.Count == 2)
                {
                    if (DataSource.Rows[0][0] == DBNull.Value || DataSource.Rows[1][0] == DBNull.Value)
                    {
                        UIType = ParameterUIType.ChoiceButton;
                    }
                }
                else if (DataSource.Rows.Count == 1 && ParentParameter != null) // modifier
                {
                    if (DataSource.Columns.Count > 2 && DataSource.Rows[0][2].ToString() == "1")
                    {
                        DefaultSelected = true;
                    }

                    if (DataSource.Columns.Count > 4 && DataSource.Rows[0][4].ToString() == "0")
                    {
                        Disabled = true;
                    }
                }

                //hidden parameter should get a default valueÉ
                if (DisplayIndex == 0 && DataSource.Rows.Count > 0)
                {
                    UserSelectedValue = DataSource.Rows[0][0];
                }

                if (ModifierSetId == null) // execute only once
                {
                    if (DataSource.Columns.Count > 3 && DataSource.Rows.Count > 0)
                    {
                        ModifierSetId = DataSource.Rows[0][3];
                        using (Utilities utils = new Utilities())
                        {
                            //Subsale max min quantity validation
                            DataTable dTable = utils.executeDataTable("select isnull(FreeQuantity, 0),Isnull(MinQuantity,0) as MinQuantity, isnull(MaxQuantity,0) as MaxQuantity from ModifierSet where ModifierSetId = @id",
                                                            new SqlParameter("@id", ModifierSetId));
                            if (dTable != null && dTable.Rows.Count > 0)
                            {
                                FreeQuantity = Convert.ToInt32(dTable.Rows[0][0]);
                                MaxQuantity = Convert.ToInt32(dTable.Rows[0][2]);
                                MinQuantity = Convert.ToInt32(dTable.Rows[0][1]);
                            }//Subsale max min quantity validation

                            //object fq = utils.executeScalar("select isnull(FreeQuantity, 0) from ModifierSet where ModifierSetId = @id",
                            //                                new SqlParameter("@id", ModifierSetId));
                            //if (fq != null)
                            //{
                            //    FreeQuantity = Convert.ToInt32(fq);
                            //}
                        }
                    }
                }
                log.LogMethodExit();
            }

            internal ElementParameter Clone()
            {
                log.LogMethodEntry();
                ElementParameter epClone = new ElementParameter(this.OwningElement,
                                                                this.DisplayIndex,
                                                                this.DisplayText1,
                                                                this.DisplayText2,
                                                                this.Parameter,
                                                                this.ParameterId,
                                                                this.ParameterName,
                                                                this.SQLBindName,
                                                                this._ParentParameterId,
                                                                this.ActionScreenId,
                                                                this.ScreenGroup,
                                                                this.DisplayImage)
                {
                    ParentParameter = this.ParentParameter,
                    Toplevel = this.Toplevel,
                    ModifierSetId = this.ModifierSetId,
                    FreeQuantity = this.FreeQuantity,
                    identifier = this.identifier
                };//Goody bag changes
                log.LogMethodExit(epClone);
                return epClone;
            }
        }

        internal List<UIPanel> UIPanels = new List<UIPanel>();

        internal UIPanel GetPanelByIndex(int index)
        {
            log.LogMethodEntry(index);
            UIPanel uiPanel = UIPanels.Find(x => x.PanelIndex == index);
            log.LogMethodExit(uiPanel);
            return uiPanel;
        }

        public ScreenModel(int inScreenId)
        {
            log.LogMethodEntry(inScreenId);
            ScreenId = inScreenId;
            Refresh();
            log.LogMethodExit();
        }

        public void Refresh()
        {
            log.LogMethodEntry();
            UIPanels.Clear();           
            using (Utilities utils = new Utilities())
            {
                DataTable dtScreen = utils.executeDataTable(@"select * from AppScreens where ScreenId = @screenId and AppScreenProfileId = (select AppScreenProfileId from AppScreenProfile where AppScreenProfileName = 'Redemption Kiosk')",
                                                               new SqlParameter("@screenId", ScreenId));

                if (dtScreen.Rows.Count == 0)
                {
                    throw new ApplicationException("Screen Id " + ScreenId.ToString() + " is invalid");
                }

                ScreenName = dtScreen.Rows[0]["ScreenName"].ToString();
                CodeObjectName = dtScreen.Rows[0]["CodeObjectName"].ToString();
                ScreenKey = dtScreen.Rows[0]["ScreenKey"].ToString();

                DataTable dtPanels = utils.executeDataTable(@"select a.UIPanelId, a.UIPanelIndex, b.UIPanelName, a.ScreenUIPanelId, b.PanelWidth
                                                            from AppScreenUIPanels a, AppUIPanels b
                                                            where a.ScreenId = @screenId
                                                            and a.UIPanelId = b.UIPanelId
                                                            and a.ActiveFlag = 1
                                                            and b.AppScreenProfileId = (select AppScreenProfileId from AppScreenProfile where AppScreenProfileName = 'Redemption Kiosk')
                                                            order by a.UIPanelIndex",
                                                               new SqlParameter("@screenId", ScreenId));

                foreach (DataRow dr in dtPanels.Rows)
                {
                    UIPanel panel = new UIPanel();
                    panel.PanelName = dr["UIPanelName"].ToString();
                    log.Debug("UI Issue: Panel_Name = " + panel.PanelName);
                    if (dr["UIPanelIndex"] != DBNull.Value)
                        panel.PanelIndex = Convert.ToInt32(dr["UIPanelIndex"]);

                    if (dr["PanelWidth"] != DBNull.Value)
                    {
                        panel.PanelWidth = Convert.ToInt32(dr["PanelWidth"]);
                    }

                    log.Debug("UI Issue: Panel_Width = " + panel.PanelWidth);
                    panel.PanelId = Convert.ToInt32(dr["UIPanelId"]);
                    panel.ScreenUIPanelId = Convert.ToInt32(dr["ScreenUIPanelId"]);

                    DataTable dtElements = utils.executeDataTable(@"select e.ElementName, e.ElementIndex, e.Action, e.ActionScreenId,
                                                                e.UIPanelElementId,
                                                                (select top 1 isnull(a2.DisplayText, a1.DisplayText) DisplayText
                                                                    from AppUIPanelElementAttribute a1
                                                                    left outer join AppUIPanelElementAttribute a2
                                                                        on a2.UIPanelElementId = a1.UIPanelElementId
                                                                        and a2.ScreenUIPanelId = @ScreenUIPanelId
                                                                        and a2.ActiveFlag = 1
                                                                    where a1.UIPanelElementId = e.UIPanelElementId
                                                                    and (a1.LanguageId = @languageId or a1.LanguageId is null)
                                                                    and a1.ActiveFlag = 1 
                                                                  order by a1.LanguageId desc) DisplayText,
                                                                (select top 1 isnull(a2.ActionScreenTitle1, a1.ActionScreenTitle1)
                                                                    from AppUIPanelElementAttribute a1
                                                                    left outer join AppUIPanelElementAttribute a2
                                                                        on a2.UIPanelElementId = a1.UIPanelElementId
                                                                        and a2.ScreenUIPanelId = @ScreenUIPanelId
                                                                        and a2.ActiveFlag = 1
                                                                    where a1.UIPanelElementId = e.UIPanelElementId
                                                                    and (a1.LanguageId = @languageId or a1.LanguageId is null)
                                                                    and a1.ActiveFlag = 1 
                                                                  order by a1.LanguageId desc) ActionScreenTitle1,
                                                                (select top 1 isnull(a2.ActionScreenTitle2, a1.ActionScreenTitle2)
                                                                    from AppUIPanelElementAttribute a1
                                                                    left outer join AppUIPanelElementAttribute a2
                                                                        on a2.UIPanelElementId = a1.UIPanelElementId
                                                                        and a2.ScreenUIPanelId = @ScreenUIPanelId
                                                                        and a2.ActiveFlag = 1
                                                                    where a1.UIPanelElementId = e.UIPanelElementId
                                                                    and (a1.LanguageId = @languageId or a1.LanguageId is null)
                                                                    and a1.ActiveFlag = 1 
                                                                  order by a1.LanguageId desc) ActionScreenTitle2,
                                                                (select top 1 isnull(a2.ActionScreenFooter1, a1.ActionScreenFooter1)
                                                                    from AppUIPanelElementAttribute a1
                                                                    left outer join AppUIPanelElementAttribute a2
                                                                        on a2.UIPanelElementId = a1.UIPanelElementId
                                                                        and a2.ScreenUIPanelId = @ScreenUIPanelId
                                                                        and a2.ActiveFlag = 1
                                                                    where a1.UIPanelElementId = e.UIPanelElementId
                                                                    and (a1.LanguageId = @languageId or a1.LanguageId is null)
                                                                    and a1.ActiveFlag = 1 
                                                                  order by a1.LanguageId desc) ActionScreenFooter1,
                                                                (select top 1 isnull(a2.ActionScreenFooter2, a1.ActionScreenFooter2) 
                                                                    from AppUIPanelElementAttribute a1
                                                                    left outer join AppUIPanelElementAttribute a2
                                                                        on a2.UIPanelElementId = a1.UIPanelElementId
                                                                        and a2.ScreenUIPanelId = @ScreenUIPanelId
                                                                        and a2.ActiveFlag = 1
                                                                    where a1.UIPanelElementId = e.UIPanelElementId
                                                                    and (a1.LanguageId = @languageId or a1.LanguageId is null)
                                                                    and a1.ActiveFlag = 1 
                                                                  order by a1.LanguageId desc) ActionScreenFooter2,
                                                                (select top 1 isnull(a2.Image, a1.Image) 
                                                                    from AppUIPanelElementAttribute a1
                                                                    left outer join AppUIPanelElementAttribute a2
                                                                        on a2.UIPanelElementId = a1.UIPanelElementId
                                                                        and a2.ScreenUIPanelId = @ScreenUIPanelId
                                                                        and a2.ActiveFlag = 1
                                                                    where a1.UIPanelElementId = e.UIPanelElementId
                                                                    and (a1.LanguageId = @languageId or a1.LanguageId is null)
                                                                    and a1.ActiveFlag = 1
                                                                  order by a1.LanguageId desc) Image
                                                                from AppUIPanelElements e
                                                                where e.UIPanelId = @UIPanelId
                                                                and e.ActiveFlag = 1",
                                                        new SqlParameter("@UIPanelId", panel.PanelId),
                                                        new SqlParameter("@languageId", Common.parafaitEnv.LanguageId),
                                                        new SqlParameter("@ScreenUIPanelId", panel.ScreenUIPanelId));

                    foreach (DataRow dre in dtElements.Rows)
                    {
                        int ActionScreenId = -1;
                        if (dre["ActionScreenId"] != DBNull.Value)
                        {
                            ActionScreenId = Convert.ToInt32(dre["ActionScreenId"]);
                        }

                        int ElementIndex = 0;
                        if (dre["ElementIndex"] != DBNull.Value)
                        {
                            ElementIndex = Convert.ToInt32(dre["ElementIndex"]);
                        }

                        Image DisplayImage = null;
                        if (dre["Image"] != DBNull.Value)
                        {
                            var image = (byte[])(dre["Image"]);
                            using (var stream = new System.IO.MemoryStream(image))
                            {
                                DisplayImage = Image.FromStream(stream);
                            }
                        }

                        UIPanelElement element = new UIPanelElement(
                                                    Convert.ToInt32(dre["UIPanelElementId"]),
                                                    dre["ElementName"].ToString(),
                                                    dre["Action"].ToString(),
                                                    ActionScreenId,
                                                    ElementIndex,
                                                    dre["DisplayText"].ToString(),
                                                    dre["ActionScreenTitle1"].ToString(),
                                                    dre["ActionScreenTitle2"].ToString(),
                                                    dre["ActionScreenFooter1"].ToString(),
                                                    dre["ActionScreenFooter2"].ToString(),
                                                    DisplayImage);

                        panel.Elements.Add(element);
                    }

                    this.UIPanels.Add(panel);
                    log.Debug("UI Issue: After Image Selection- Panel_Width = " + panel.PanelWidth);
                    
                }
            }
            log.LogMethodExit();
        }
    }
}
