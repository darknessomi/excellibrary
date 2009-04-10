using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace QiHe.CodeLib
{
    public class AppSettings
    {
        protected class ControlBinding
        {
            public Control Control;
            public string PropertyName;
            public string DataMemberName;

            public ControlBinding(Control control, string propertyname, string datamembername)
            {
                Control = control;
                PropertyName = propertyname;
                DataMemberName = datamembername;
            }
        }

        protected List<ControlBinding> ControlBindings = new List<ControlBinding>();
        public void Set(Control control, string PropertyName, string DataMemberName)
        {
            Binding binding = new Binding(PropertyName, this, DataMemberName);
            binding.FormattingEnabled = true;
            binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            control.DataBindings.Add(binding);
            ControlBindings.Add(new ControlBinding(control, PropertyName, DataMemberName));
        }

        public void Save()
        {
            WinApp.SaveConfig(this);
        }

        public void Save(string settingsFile)
        {
            XmlFile.Save(settingsFile, this);
        }

        public void UpdateControls()
        {
            foreach (ControlBinding binding in ControlBindings)
            {
                object propertyValue = GetPropertyValue(this, binding.DataMemberName);
                SetPropertyValue(binding.Control, binding.PropertyName, propertyValue);
            }
        }

        public void UpdateControl(Control control)
        {
            foreach (ControlBinding binding in ControlBindings)
            {
                if (binding.Control == control)
                {
                    object propertyValue = GetPropertyValue(this, binding.DataMemberName);
                    SetPropertyValue(binding.Control, binding.PropertyName, propertyValue);
                }
            }
        }

        public static string InferSettingsFile(string partnerFile)
        {
            string directory = FileHelper.GetDirectory(partnerFile);
            string fileName = Path.GetFileName(WinApp.ConfigFile);
            return Path.Combine(directory, fileName);
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            System.Reflection.PropertyInfo propertyInfo
                 = obj.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj, null);
            }
            return null;
        }

        public static void SetPropertyValue(object obj, string propertyName, object propertyValue)
        {
            System.Reflection.PropertyInfo propertyInfo
                 = obj.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(obj, propertyValue, null);
            }
        }
    }
}
