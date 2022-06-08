using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using RnCore.Helpers;
using RnCore.Logging;

namespace Rn.WebManLib.Utils
{
    public class ApiUsers
    {
        private static readonly ApiUsers instance = new ApiUsers();
        public static ApiUsers Instance
        {
            get { return instance; }
        }

        // Properties used in this class
        public bool Ready { get; private set; }
        public string ApiXmlFilePath { get; private set; }
        private readonly XmlDocument _xml;
        private readonly Dictionary<string, ApiUser> _apiUsers;


        private ApiUsers()
        {
            try
            {
                _xml = new XmlDocument();
                _apiUsers = new Dictionary<string, ApiUser>();
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

        private ApiUser GetApiUser(string username)
        {
            try
            {
                return _apiUsers[username.ToLower().Trim()];
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
                {
                    var apiEntry = new ApiUser(n);
                    if (!_apiUsers.ContainsKey(apiEntry.UserName))
                        _apiUsers.Add(apiEntry.UserName, apiEntry);
                }

                RnLogger.Loggers.LogDebug("Loaded '{0}' API users", 100, _apiUsers.Count);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void ApiXmlFileChanges(object sender, XmlNodeChangedEventArgs e)
        {
            try
            {
                RnLogger.Loggers.LogDebug("Saving changes to API XML file...");
                _xml.Save(ApiXmlFilePath);
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
                ApiXmlFilePath = filePath;

                // Check to see if the API KEY file exists - create it if not
                if (!File.Exists(ApiXmlFilePath))
                {
                    const string fileContents = @"<?xml version=""1.0"" encoding=""utf-8"" ?><ApiKeys></ApiKeys>";
                    if (!RnIO.WriteToFile(ApiXmlFilePath, fileContents))
                    {
                        RnLogger.Loggers.LogError("Could not load the API KEYS file '{0}'", 100, ApiXmlFilePath);
                        ApiXmlFilePath = "";
                        return;
                    }
                }

                // Open the API KEY file
                _xml.Load(ApiXmlFilePath);
                LoadAllUsers();
                _xml.NodeChanged += ApiXmlFileChanges;
                _xml.NodeRemoved += ApiXmlFileChanges;

                RnLogger.Loggers.LogDebug("The APIKEY file '{0}' has been loaded", 100, ApiXmlFilePath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        // todo: create a close file method

        public bool RegisterApiUser(string username, string apikey, bool enabled = true)
        {
            try
            {
                if (CheckApiUserExists(username))
                    return true;

                // Create the user node
                var n = _xml.CreateElement("ApiKey");
                n.SetAttribute("UserName", username);
                n.SetAttribute("API", apikey);
                n.SetAttribute("Enabled", "true");
                n.SetAttribute("LastSeen", "");
                n.SetAttribute("ApiCalls", "0");

                // Register the user
                if (_xml.DocumentElement != null)
                {
                    _xml.DocumentElement.AppendChild(n);
                    
                    var apiEntry = new ApiUser(username, apikey, enabled);
                    _apiUsers.Add(apiEntry.UserName, apiEntry);
                    
                    return true;
                }

                RnLogger.Loggers.LogError("Could not find the ROOT node in API XML file!");
                return false;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool CheckApiUserExists(string username)
        {
            try
            {
                if (_apiUsers.Count == 0)
                    return false;

                return _apiUsers.ContainsKey(username.ToLower().Trim());
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool DisableApiUser(string username)
        {
            try
            {
                if (!CheckApiUserExists(username))
                    return false;

                var userObj = GetApiUser(username);
                userObj.SetEnabledState(false);

                var userXml = GetUserNode(username);
                if (userXml.Attributes != null)
                    userXml.Attributes["Enabled"].Value = "false";

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool EnableApiUser(string username)
        {
            try
            {
                if (!CheckApiUserExists(username))
                    return false;

                var userObj = GetApiUser(username);
                userObj.SetEnabledState(true);

                var userXml = GetUserNode(username);
                if (userXml.Attributes != null)
                    userXml.Attributes["Enabled"].Value = "true";

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool IsApiUserEnabled(string username)
        {
            try
            {
                if (!CheckApiUserExists(username))
                    return false;

                return _apiUsers[username].Enabled;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool ValidateApiUser(WmAuthApi authInfo)
        {
            try
            {
                if (authInfo == null)
                {
                    RnLogger.Loggers.LogWarning("No authentication supplied for method '{0}'", 100,
                                                RnLoggerHelper.GetFrameName(2));
                    return false;
                }

                return ValidateApiUser(authInfo.UserName, authInfo.ApiKey);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool ValidateApiUser(string username, string apikey)
        {
            try
            {
                if (!IsApiUserEnabled(username))
                {
                    RnLogger.Loggers.LogWarning("The API User '{0}' is disabled", 100, username);
                    return false;
                }

                if (_apiUsers[username].ApiKey == apikey)
                {
                    _apiUsers[username].IncrementApiCallCount();
                    var userNode = GetUserNode(username);

                    if (userNode.Attributes != null)
                    {
                        // todo: set missing attributes
                        if (userNode.Attributes["ApiCalls"] != null)
                            userNode.Attributes["ApiCalls"].Value =
                                _apiUsers[username].ApiCalls.ToString(CultureInfo.InvariantCulture);
                        if (userNode.Attributes["LastSeen"] != null)
                            userNode.Attributes["LastSeen"].Value =
                                _apiUsers[username].LastSeen.ToString(CultureInfo.InvariantCulture);
                    }

                    return true;
                }

                RnLogger.Loggers.LogWarning("The API users '{0}' API key is wrong (Key: {1})", 100, username, apikey);
                return false;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public bool DeleteApiUser(string username)
        {
            try
            {
                if (!CheckApiUserExists(username))
                    return true;

                _apiUsers.Remove(username);

                var userNode = GetUserNode(username);
                if (userNode == null)
                    return true;

                if (_xml.DocumentElement != null)
                    _xml.DocumentElement.RemoveChild(userNode);

                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }


    }
    
}
