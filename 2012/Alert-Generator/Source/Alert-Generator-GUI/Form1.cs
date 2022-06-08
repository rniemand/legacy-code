using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Alert_Generator
{
    public partial class Form1 : Form
    {
        public Collection<EventLog> _EventLogs = new Collection<EventLog>();
        public Collection<EventLogEntryType> _Severities = new Collection<EventLogEntryType>();

        public Form1()
        {
            InitializeComponent();
        }

        #region Button Clicks
        private void Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MakeAlert_Click(object sender, EventArgs e)
        {
            // Get Values
            timer1.Stop();
            string eventText = Get_EventDescription();
            string eventSource = Get_EventSource();
            int eventID = Get_EventID();
            EventLog log = Get_EventLog();
            EventLogEntryType severity = Get_EventSeverity();

            // Set defaults
            if (string.IsNullOrEmpty(eventText)) eventText = "No TEXT DEFINED!";
            if (string.IsNullOrEmpty(eventSource)) eventSource = "Richard Event Source";
            if (eventID == 0) eventID = 10;

            // Check for the Event Source in the selected event log
            try
            {
                if (!EventLog.SourceExists(eventSource))
                    EventLog.CreateEventSource(eventSource, log.Log);
            }
            catch (Exception ee)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(String.Format("There was an error creating the Event Source '{0}'.", eventSource));
                sb.Append(Environment.NewLine + Environment.NewLine);
                sb.Append(ee.GetType().ToString() + Environment.NewLine);
                sb.Append(ee.Message);
                MessageBox.Show(sb.ToString(), "Error Creating Event Source");
                return;
            }

            // Write the event
            try
            {
                EventLog.WriteEntry(eventSource, eventText, severity, eventID);
            }
            catch (Exception ee)
            {
                var sb = new StringBuilder();
                sb.Append("There was an error creating the alert...");
                sb.Append(Environment.NewLine + Environment.NewLine);
                sb.Append(ee.GetType().ToString() + Environment.NewLine);
                sb.Append(ee.Message);
                MessageBox.Show(sb.ToString(), "Error Creating Alert");
                return;
            }

            // Show the usr that the alert was created
            SetMessage("Alert Created!", 1);
        } 
        #endregion

        public void SetMessage(string message, int showTime)
        {
            if (showTime > 0)
            {
                timer1.Interval = (showTime * 1000);
                timer1.Start();
            }

            messageArea.Text = message;
        }

        #region Getter Functions
        private string Get_EventSource()
        {
            return evtSource.Text;
        }

        private int Get_EventID()
        {
            string eventID = evtID.Text;
            int eventID_Done = 0;
            int.TryParse(eventID, out eventID_Done);
            return eventID_Done;
        }

        private string Get_EventDescription()
        {
            return eventText.Text;
        }

        private EventLog Get_EventLog()
        {
            return _EventLogs[eventLog.SelectedIndex];
        }

        private EventLogEntryType Get_EventSeverity()
        {
            return _Severities[evtSeverity.SelectedIndex];
        }
        #endregion

        #region Form Load Functions
        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshEventLogs();
            RefreshEventSeverities();
            SetMessage("Ready...", 0);
        }

        private void RefreshEventLogs()
        {
            if (eventLog.DataSource != null)
                eventLog.DataSource = null;

            _EventLogs.Clear();
            var logs = EventLog.GetEventLogs();

            foreach (var log in logs)
                _EventLogs.Add(log);

            eventLog.DataSource = _EventLogs;
            eventLog.DisplayMember = "Log";
        }

        private void RefreshEventSeverities()
        {
            if (evtSeverity.DataSource != null)
                evtSeverity.DataSource = null;

            _Severities.Clear();
            _Severities.Add(EventLogEntryType.Error);
            _Severities.Add(EventLogEntryType.Warning);
            _Severities.Add(EventLogEntryType.Information);
            _Severities.Add(EventLogEntryType.FailureAudit);
            _Severities.Add(EventLogEntryType.SuccessAudit);

            evtSeverity.DataSource = _Severities;
        } 
        #endregion

        private void TimerTick(object sender, EventArgs e)
        {
            timer1.Stop();
            messageArea.Text = "Ready...";
        }
    }
}
