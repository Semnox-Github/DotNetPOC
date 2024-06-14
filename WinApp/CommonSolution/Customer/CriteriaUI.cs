/********************************************************************************************
 * Project Name - Customer
 * Description  - Class for  of CriteriaUI      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Ui control to display the criteria
    /// </summary>
    public class CriteriaUI : FlowLayoutPanel
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Criteria criteria;
        private Label label;
        private List<CriteriaUI> criteriaUIList;
        private CheckBox checkBox;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaUI(Criteria criteria)
        {
            log.LogMethodEntry(criteria);
            this.criteria = criteria;
            FlowDirection = FlowDirection.LeftToRight;
            this.AutoSize = true;
            BorderStyle = BorderStyle.FixedSingle;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            InitializeComponents();
            log.LogMethodExit();
        }

        private void InitializeComponents()
        {
            log.LogMethodEntry();
            checkBox = new CheckBox();
            checkBox.Width = 15;
            Controls.Add(checkBox);
            label = new Label();
            Controls.Add(label);
            label.AutoSize = true;
            label.Anchor = AnchorStyles.Bottom | AnchorStyles.Top;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            if (criteria is ComplexCriteria)
            {
                log.Debug("Begin - criteria is ComplexCriteria ");
                ComplexCriteria complexCriteria = criteria as ComplexCriteria;
                label.Text = complexCriteria.GetLogialOperator();
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.BorderStyle = BorderStyle.FixedSingle;
                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel.AutoSize = true;
                flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Controls.Add(flowLayoutPanel);
                criteriaUIList = new List<CriteriaUI>();
                foreach (var criteriaItem in complexCriteria.GetCriteriaList())
                {
                    CriteriaUI creteriaUI = new CriteriaUI(criteriaItem is SearchCriteria? (criteriaItem as SearchCriteria).GetCriteria() : criteriaItem);
                    criteriaUIList.Add(creteriaUI);
                    flowLayoutPanel.Controls.Add(creteriaUI);
                }
                log.Debug("End - criteria is ComplexCriteria ");
            }
            else if (criteria is NotCriteria)
            {
                log.Debug("Begin - criteria is NotCriteria ");
                label.Text = "NOT";
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel.AutoSize = true;
                flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Controls.Add(flowLayoutPanel);
                criteriaUIList = new List<CriteriaUI>();
                Criteria childCriteria = (criteria as NotCriteria).GetCriteria();
                CriteriaUI creteriaUI = new CriteriaUI(childCriteria is SearchCriteria ? (childCriteria as SearchCriteria).GetCriteria() : childCriteria);
                criteriaUIList.Add(creteriaUI);
                flowLayoutPanel.Controls.Add(creteriaUI);
                log.Debug("End - criteria is NotCriteria ");
            }
            else if (criteria is SearchCriteria)
            {
                log.Debug("Begin - criteria is SearchCriteria ");
                label.Text = "";
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel.AutoSize = true;
                flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                Controls.Add(flowLayoutPanel);
                criteriaUIList = new List<CriteriaUI>();
                CriteriaUI creteriaUI = new CriteriaUI((criteria as SearchCriteria).GetCriteria());
                criteriaUIList.Add(creteriaUI);
                flowLayoutPanel.Controls.Add(creteriaUI);
                log.Debug("End - criteria is SearchCriteria ");
            }
            else
            {
                label.Text = criteria.GetDisplayClause();
            }
            log.LogMethodExit();
        }
        
        /// <summary>
        /// returns the selected criteria
        /// </summary>
        /// <returns></returns>
        public Criteria GetSelectedCriteria()
        {
            log.LogMethodEntry();
            Criteria selectedCriteria = null;
            if(checkBox.Checked)
            {
                selectedCriteria = criteria;
            }
            else
            {
                if(criteriaUIList != null)
                {
                    foreach (var criteriaUI in criteriaUIList)
                    {
                        selectedCriteria = criteriaUI.GetSelectedCriteria();
                        if(selectedCriteria != null)
                        {
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit(selectedCriteria);
            return selectedCriteria;
        }
    }
}
