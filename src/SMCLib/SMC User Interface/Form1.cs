using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SMC_Core;

namespace SMCUserInterface
{
    public partial class Form1 : Form
    {

        StreamHandler streamHandler = new StreamHandler();
        SensorHandler sensorHandler = new SensorHandler();
        MQTTServerConnection serverConnection;
        // this is needed to prevent overflows
        private long timeOffset = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        private delegate void EmptyDelegate();

        private MQTTServerConfig serverConfig => new MQTTServerConfig
        {
            serverAddress = tbServer.Text,
            topic = tbTopic.Text,
            username = tbUsername.Text,
            password = tbPassword.Text
        };

        private void UpdatePozyxDataBox(IEnumerable<PozyxData> data)
        {
            foreach (PozyxData item in data)
            {
                int timestamp = (int)(item.systemTimestamp - timeOffset);
                if (sldrTimingMin.Maximum < timestamp)
                {
                    sldrTimingMin.Maximum = timestamp;
                    sldrTimingMax.Maximum = timestamp;
                    sldrTimingMax.Value = timestamp;
                }
                else if (sldrTimingMin.Minimum > timestamp)
                {
                    sldrTimingMin.Minimum = timestamp;
                    sldrTimingMax.Minimum = timestamp;
                }
                if (timestamp > sldrTimingMin.Value &&
                    timestamp < sldrTimingMax.Value)
                {
                    lbData.Items.Add(item);
                }
            }
        }

        private void AddSensorToTagList()
        {
            clbTags.Items.Clear();
            foreach (PozyxSensor sensor in sensorHandler.getSensorsofType<PozyxSensor>())
            {
                clbTags.Items.Add(sensor);
            }
        }

        private int AddSensorToTagListEvent(IStreamContainer container)
        {
            // invoke function on main thread. See also: https://visualstudiomagazine.com/articles/2010/11/18/multith`ading-in-winforms.aspx?m=1
            clbTags.Invoke(new EmptyDelegate(AddSensorToTagList));
            return 0;
        }

        public Form1()
        {
            InitializeComponent();
            streamHandler.AddSensorHandlerCallback(sensorHandler.addSensor);
            streamHandler.AddSensorHandlerCallback(AddSensorToTagListEvent);
        }

        private void btnConnectMQTT_Click(object sender, EventArgs e)
        {
            serverConnection = new MQTTServerConnection(serverConfig, streamHandler);
            serverConnection.connectRemote();
            btnConnectMQTT.Hide();
            btnDisconnectMQTT.Show();
            SensorDataUpdateTimer.Start();
            int unixTimestamp = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timeOffset);
            sldrTimingMin.Minimum = unixTimestamp;
            sldrTimingMax.Minimum = unixTimestamp;
            sldrTimingMin.Maximum = unixTimestamp;
            sldrTimingMax.Maximum = unixTimestamp;
            sldrTimingMin.Value = unixTimestamp;
            sldrTimingMax.Value = unixTimestamp;

        }

        private void btnDisconnectMQTT_Click(object sender, EventArgs e)
        {
            btnConnectMQTT.Show();
            btnDisconnectMQTT.Hide();
            // clbTags.Items.Clear();
            lbData.Items.Clear();
            SensorDataUpdateTimer.Stop();
            serverConnection.Disconnect();
            serverConnection = null;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "JSON files (*.a51)|*.a51|CSV file (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    FileImporter fileImporter = new FileImporter(openFileDialog.FileName, streamHandler);
                }
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            // TODO;
            
        }

        private void clbTags_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // https://stackoverflow.com/a/16936590/8945815
            BeginInvoke((MethodInvoker)(
            () =>
            {
                foreach (Sensor sensor in clbTags.Items)
                {
                    sensor.Active = clbTags.CheckedItems.Contains(sensor);
                }
            }));
            updateSensorData();
        }

        private void updateSensorData()
        {
            // SuspendLayout stops the drawing of UI elements
            // ResumeLayout starts it again
            SuspendLayout();
            lbData.Items.Clear();
            foreach (PozyxSensor sensor in sensorHandler.getSensorsofType<PozyxSensor>())
            {
                if (sensor != null && sensor.Active)
                {
                    sensor.updateData();
                    UpdatePozyxDataBox(sensor.getAllPozyxData());
                }
            }
            ResumeLayout();
        }
        private void updateSensorData_Event(object sender, EventArgs e)
        {
            updateSensorData();
        }
        private void sliderMax_Slide(object sender, EventArgs e)
        {
            if (sldrTimingMax.Value < sldrTimingMin.Value)
                sldrTimingMin.Value = sldrTimingMax.Value;
            updateSensorData();
        }
        private void sliderMin_Slide(object sender, EventArgs e)
        {
            if (sldrTimingMin.Value > sldrTimingMax.Value)
                sldrTimingMax.Value = sldrTimingMin.Value;
            updateSensorData();
        }
        /*
         * if you want to add items to the tags list, you can directly add objects:
         * clbTags.Items.Add(new PozyxSensor(new Stream<PozyxData>()));
         * to change the string output, see the toString override in PozyxSensor
         * ditto for Sensor data
         */
    }
}
