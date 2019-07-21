using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LinkManager
{
    class ConfigManager
    {
        XmlDocument xmlDoc;

        public ConfigManager()
        {
            loadConfig();
        }

        public void loadConfig()
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");
        }

        public bool KeyExists(string strKey)
        {

            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                    return true;
            }
            return false;
        }


        public void SetKey(string strKey, string newValue)
        {
            if (!KeyExists(strKey))
            {
                throw new ArgumentNullException("Key", "<" + strKey + "> does not exist in the configuration. Update failed.");
            }
            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == strKey)
                    childNode.Attributes["value"].Value = newValue;
            }

            xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config");
            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }


        public string GetKey(string key)
        {
            if (!KeyExists(key))
            {
                throw new ArgumentNullException("Key", "<" + key + "> does not exist in the configuration. Update failed.");
            }
            XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.Attributes["key"].Value == key)
                    return childNode.Attributes["value"].Value;
            }
            return "";
        }
    }
}
