using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RnCore.Helpers;
using RnCore.Logging;

namespace Rn.WebManLib.Utils
{
    public class ApiKeys
    {
        private static readonly ApiKeys instance = new ApiKeys();
        public static ApiKeys Instance
        {
            get { return instance; }
        }


        private bool _ready;
        private string _apiXmlFilePath;
        private readonly XmlDocument _xml;
        private readonly List<ApiKeyEntry> _apiKeys;


        private ApiKeys()
        {
            try
            {
                _xml = new XmlDocument();
                _apiKeys = new List<ApiKeyEntry>();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


        private XmlNode GetUserNode(string username)
        {
            try
            {
                var xpath = String.Format("/ApiKeys/ApiKey[@UserName='{0}']", username);
                return _xml.SelectSingleNode(xpath);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return null;
            }
        }

        private void LoadAllUsers()
        {
            try
            {
                var colUsers = _xml.SelectNodes("/ApiKeys/ApiKey");
                if (colUsers == null || colUsers.Count == 0)
                    return;

                foreach (XmlNode n in colUsers)
                    _apiKeys.Add(new ApiKeyEntry(n));

                RnLogger.Loggers.LogDebug("Loaded '{0}' API users", 100, _apiKeys.Count);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }





        public void LoadApiXmlFile(string filePath)
        {
            // todo: boot call if keys are loaded

            try
            {
                _apiXmlFilePath = filePath;

                // Check to see if the API KEY file exists - create it if not
                if (!File.Exists(_apiXmlFilePath))
                {
                    const string fileContents = @"<?xml version=""1.0"" encoding=""utf-8"" ?><ApiKeys></ApiKeys>";
                    if (!RnIO.WriteToFile(_apiXmlFilePath, fileContents))
                    {
                        RnLogger.Loggers.LogError("Could not load the API KEYS file '{0}'", 100, _apiXmlFilePath);
                        _apiXmlFilePath = "";
                        return;
                    }
                }

                // Open the API KEY file
                _xml.Load(_apiXmlFilePath);
                LoadAllUsers();

                // todo: add timer to save this xml file


                RnLogger.Loggers.LogDebug("The APIKEY file '{0}' has been loaded", 100, _apiXmlFilePath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        // todo: create a close file method

        public bool RegisterApiKey(string username, string apikey)
        {
            try
            {
                if (UserHasApiKey(username))
                    return true;

                // Create the user node
                var n = _xml.CreateElement("ApiKey");
                n.SetAttribute("UserName", username);
                n.SetAttribute("API", apikey);
                n.SetAttribute("Enabled", "true");
                n.SetAttribute("LastSeen", "");

                // todo: push the user into the "_apiKeys" list
                
                // Attempt to save the user node
                if (_xml.DocumentElement != null)
                {
                    _xml.DocumentElement.AppendChild(n);
                }
                else
                {
                    // todo: add to logger
                    Console.WriteLine("No root NODE!!!!");
                }
                _xml.Save(_apiXmlFilePath);

                // todo: add the user to the global user collection

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool UserHasApiKey(string username)
        {
            try
            {
                var xpath = String.Format("/ApiKeys/ApiKey[@UserName='{0}']", username);
                var node = _xml.SelectSingleNode(xpath);
                return node != null;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool DisableUsersApiKey(string username)
        {
            try
            {
                // todo: update the loaded users array

                var userNode = GetUserNode(username);
                if (userNode == null) return false;

                if (userNode.Attributes != null)
                    userNode.Attributes["Enabled"].Value = "false";

                _xml.Save(_apiXmlFilePath);
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }




    }

    class ApiKeyEntry
    {
        public string UserName { get; private set; }
        public string ApiKey { get; private set; }
        public bool Enabled { get; private set; }
        public DateTime LastSeen { get; private set; }


        public ApiKeyEntry()
        {
            // Static constructor
        }

        public ApiKeyEntry(XmlNode n)
        {
            try
            {
                UserName = n.GetAttributeString("UserName");
                ApiKey = n.GetAttributeString("API");
                Enabled = n.GetAttributeBool("Enabled");
                
                // todo: load last seen
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

    }
}
