//Import various C# things.
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

//Import Procon things.
using PRoCon.Core;
using PRoCon.Core.Plugin;
using PRoCon.Core.Players;

namespace PRoConEvents
{
    public class HelloWorld : PRoConPluginAPI, IPRoConPluginInterface
    {

        //--------------------------------------
        //Class level variables.
        //--------------------------------------

        private bool pluginEnabled = false;
        private String someString = "string";

        private int debugLevel = 1;

        //--------------------------------------
        //Plugin constructor. Can be left blank.
        //--------------------------------------

        public HelloWorld()
        {

        }

        //--------------------------------------
        //Description settings for your plugin.
        //--------------------------------------

        public string GetPluginName()
        {
            return "Hello World";
        }

        public string GetPluginVersion()
        {
            return "0.0.0";
        }

        public string GetPluginAuthor()
        {
            return "Analytalica";
        }

        public string GetPluginWebsite()
        {
            return "purebattlefield.org";
        }

        public string GetPluginDescription()
        {
            return @"Sends 'Hello World' to in-game chat when enabled.";
        }

        //--------------------------------------
        //Helper Functions
        //--------------------------------------

        public void toChat(String message)
        {
            if (!message.Contains("\n"))
            {
                this.ExecuteCommand("procon.protected.send", "admin.say", message, "all");
            }
            else
            {
                string[] multiMsg = message.Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (string send in multiMsg)
                {
                    toChat(send);
                }
            }
        }

        public void toChat(String message, String playerName)
        {
            if (!message.Contains("\n"))
            {
                this.ExecuteCommand("procon.protected.send", "admin.say", message, "player", playerName);
            }
            else
            {
                string[] multiMsg = message.Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (string send in multiMsg)
                {
                    toChat(send, playerName);
                }
            }
        }

        public void toConsole(int msgLevel, String message)
        {
            if (debugLevel >= msgLevel)
            {
                this.ExecuteCommand("procon.protected.pluginconsole.write", message);
            }
        }

        //--------------------------------------
        //These methods run when Procon does what's on the label.
        //--------------------------------------

        //Runs when the plugin is compiled.

        public void OnPluginLoaded(string strHostName, string strPort, string strPRoConVersion)
        {
            // Depending on your plugin you will need different types of events. See other plugins for examples.
            this.RegisterEvents(this.GetType().Name, "OnPluginLoaded");
        }

        //Runs when the plugin is enabled (checked in Procon)
        //Note that this also runs right after a server restart if it was enabled before.

        public void OnPluginEnable()
        {
            //Use a variable like this one to turn on and off your plugin.
            this.pluginEnabled = true;
            //Say something in the console.
            this.toConsole(1, "Template Plugin Enabled!");
            this.ExecuteCommand("procon.protected.send", "admin.say", "Hello World!", "all");
        }

        //Runs when the pluginh is disabled (unchecked in Procon)

        public void OnPluginDisable()
        {
            //Use a variable like this one to turn on and off your plugin.
            this.pluginEnabled = false;
            //Say something in the console.
            this.toConsole(1, "Template Plugin Disabled!");
        }

        //List plugin variables.
        public List<CPluginVariable> GetDisplayPluginVariables()
        {
            List<CPluginVariable> lstReturn = new List<CPluginVariable>();
            lstReturn.Add(new CPluginVariable("Settings|Some String", typeof(string), someString));
            lstReturn.Add(new CPluginVariable("Settings|Debug Level", typeof(string), debugLevel.ToString()));
            return lstReturn;
        }

        public List<CPluginVariable> GetPluginVariables()
        {
            return GetDisplayPluginVariables();
        }

        //Set variables.
        public void SetPluginVariable(String strVariable, String strValue)
        {
            if (Regex.Match(strVariable, @"Some String").Success)
            {
                someString = strValue;
            }
            else if (Regex.Match(strVariable, @"Debug Level").Success)
            {
                try
                {
                    debugLevel = Int32.Parse(strValue);
                }
                catch (Exception z)
                {
                    toConsole(1, "Invalid debug level! Choose 0, 1, or 2 only.");
                    debugLevel = 1;
                }
            }
        }
    }
}