/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Translate helper file
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.ViewContainer;
using System.Windows.Controls;
using System.Windows.Media;

namespace Semnox.Parafait.CommonUI
{
    public static class TranslateHelper
    {
        #region Members
        public static ExecutionContext executioncontext;
        public static bool isDesignMode;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Methods
        public static void Translate(ExecutionContext executioncontext, Visual myVisual)
        {
            log.LogMethodEntry(executioncontext, myVisual);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(myVisual); i++)
            {
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(myVisual, i);
                string type = childVisual.GetType().ToString().ToLower();
                if ((type.EndsWith("customtextblock")))
                {
                    CustomTextBlock textBlock = childVisual as CustomTextBlock;
                    if (textBlock.Text != null && textBlock.GetBindingExpression(TextBlock.TextProperty) == null)
                    {
                        textBlock.Text = MessageViewContainerList.GetMessage(executioncontext, textBlock.Text.ToString(), null);
                    }
                }
                else if (type.EndsWith("label"))
                {
                    Label label = childVisual as Label;
                    if (label.Content != null && label.GetBindingExpression(Label.ContentProperty) == null)
                    {
                        label.Content = MessageViewContainerList.GetMessage(executioncontext, label.Content.ToString(), null);
                    }
                }
                else if (type.EndsWith("customactionbutton"))
                {
                    CustomActionButton actionButton = childVisual as CustomActionButton;
                    if (actionButton.Content != null
                    && actionButton.GetBindingExpression(ContentControl.ContentProperty) == null)
                    {
                        string translatedText = MessageViewContainerList.GetMessage(executioncontext, actionButton.Content.ToString(), null);
                        if (translatedText != null)
                        {
                            actionButton.Content = translatedText.ToUpper();
                        }
                    }
                }
                else if (type.EndsWith("customtextbox"))
                {
                    CustomTextBox customTextBox = childVisual as CustomTextBox;
                    if (customTextBox.Heading != null && customTextBox.GetBindingExpression(CustomTextBox.HeadingDependencyProperty) == null)
                    {
                        customTextBox.Heading = MessageViewContainerList.GetMessage(executioncontext, customTextBox.Heading.ToString(), null);
                    }
                    if (customTextBox.DefaultValue != null && customTextBox.GetBindingExpression(CustomTextBox.DefaultValueDependencyProperty) == null)
                    {
                        customTextBox.DefaultValue = MessageViewContainerList.GetMessage(executioncontext, customTextBox.DefaultValue.ToString(), null);
                    }
                }
                else if (type.EndsWith("customtextboxdatepicker"))
                {
                    CustomTextBoxDatePicker customTextBoxDatePicker = childVisual as CustomTextBoxDatePicker;
                    if (customTextBoxDatePicker.Heading != null && customTextBoxDatePicker.GetBindingExpression(CustomTextBoxDatePicker.HeadingDependencyProperty) == null)
                    {
                        customTextBoxDatePicker.Heading = MessageViewContainerList.GetMessage(executioncontext, customTextBoxDatePicker.Heading.ToString(), null);
                    }
                }
                else if (type.EndsWith("customcombobox"))
                {
                    CustomComboBox customComboBox = childVisual as CustomComboBox;
                    if (customComboBox.Heading != null
                    && customComboBox.GetBindingExpression(CustomComboBox.HeadingDependencyProperty) == null)
                    {
                        customComboBox.Heading = MessageViewContainerList.GetMessage(executioncontext, customComboBox.Heading.ToString(), null);
                    }
                }
                else if (type.EndsWith("customcheckbox"))
                {
                    CustomCheckBox customCheckBox = childVisual as CustomCheckBox;
                    if (customCheckBox.Heading != null
                    && customCheckBox.GetBindingExpression(CustomCheckBox.HeadingDependencyProperty) == null)
                    {
                        customCheckBox.Heading = MessageViewContainerList.GetMessage(executioncontext, customCheckBox.Heading.ToString(), null);
                    }
                }
                else if (type.EndsWith("customsearchtextbox"))
                {
                    CustomSearchTextBox customSearchTextBox = childVisual as CustomSearchTextBox;
                    if (customSearchTextBox.Heading != null
                    && customSearchTextBox.GetBindingExpression(CustomSearchTextBox.HeadingDependencyProperty) == null)
                    {
                        customSearchTextBox.Heading = MessageViewContainerList.GetMessage(executioncontext, customSearchTextBox.Heading.ToString(), null);
                    }
                }
                else
                {
                    Translate(executioncontext, childVisual);
                }

            }
            log.LogMethodExit();
        }

        internal static string TranslateMessage(string message)
        {
            log.LogMethodEntry(message);
            if (!isDesignMode)
            {
                return MessageViewContainerList.GetMessage(executioncontext, message, null);
            }
            else
            {
                return message;
            }
        }

        internal static string TranslateMessage(int messageNo)
        {
            log.LogMethodEntry(messageNo);
            return MessageViewContainerList.GetMessage(executioncontext, messageNo, null);
        }
        #endregion

    }
}
