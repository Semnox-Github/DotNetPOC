﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Semnox.Parafait.Report.Reports.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=MLR-LT037\\SQLEXPRESS;Initial Catalog=Parafaitpinballz;Persist Securit" +
            "y Info=True;User ID=parafait;Password=\"pezz4aLNZ5rhAMN04bUczw==\";Connection Time" +
            "out=600;")]
        public string ParafaitConnectionString {
            get {
                return ((string)(this["ParafaitConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=MLR-LT037\\SQLEXPRESS;Initial Catalog=ParafaitCentral;Persist Security" +
            " Info=True;User ID=parafait;Password=\"pezz4aLNZ5rhAMN04bUczw==\";Connection Timeo" +
            "ut=600;")]
        public string ParafaitCentralConnectionString {
            get {
                return ((string)(this["ParafaitCentralConnectionString"]));
            }
        }
    }
}