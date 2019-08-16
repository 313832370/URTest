using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
//Add reference before you 
using URSDK.RobController.Communication;

namespace DEMO_URSDK
{
    public partial class Form1 : Form
    {
        //Definition
        private DashBoard aDashBoard;        //Dashboard instance, port: 29999
        private RTClient aRTClient;          //RTClient instance, port: 30003
        private RTDE aRTDE;                  //RTDE instance, port: 30004
        public Form1()
        {
            InitializeComponent();
            // create instance of robot communication class
            aDashBoard = new DashBoard(textBox1.Text);
            aRTClient = new RTClient(textBox1.Text);
        aRTDE = new RTDE(System.Windows.Forms.Application.StartupPath+"\\RTDEConfig.xml", textBox1.Text);
        
         //aRTDE = new RTDE(@"D:\UR Work\020 VS Projects\DEMO_URSDK\DEMO_URSDK\bin\Debug\RTDEConfig.xml", textBox1.Text);
            timer1.Interval = Convert.ToInt32(textBox10.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string retv=aDashBoard.loadProgram(textBox2.Text);
            textBox3.Text = retv;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)   //IP address change event 
        {
            aDashBoard = new DashBoard(textBox1.Text);
            if (aRTClient.Status != RTClientStatus.Stopped)
                aRTClient.stopRTClient();
            aRTClient = new RTClient(textBox1.Text);
            if (aRTDE.isSynchronizing)
                aRTDE.stopSync();
            aRTDE = new RTDE(System.Windows.Forms.Application.StartupPath + "\\RTDEConfig.xml",textBox1.Text);        
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string retv=aDashBoard.play();
            textBox3.Text = retv;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string retv = aDashBoard.stop();
            textBox3.Text = retv;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string retv = aDashBoard.popup(textBox4.Text);
            textBox3.Text = retv;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string retv = aDashBoard.closepopup();
            textBox3.Text = retv;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            aRTClient.startRTClient();
            while (aRTClient.Status != RTClientStatus.Syncing)
                Thread.Sleep(10);

            button2.Enabled = false;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            aRTClient.stopRTClient();
            while (aRTClient.Status != RTClientStatus.Stopped)
                Thread.Sleep(10);
            button2.Enabled = true;
            button3.Enabled = false;
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textBox10.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (aRTClient.Status == RTClientStatus.Syncing)
            {
                RTClientObj aObj = aRTClient.getRTClientObj();
                textBox5.Text = aObj.actual_TCP_pose.ToString();
            }

            if (aRTDE.isSynchronizing)
            {
                RTDEOutputObj OutObj = aRTDE.getOutputObj();
                textBox7.Text = "Actual tcp pose: "+OutObj.actual_TCP_pose.ToString()+"\n";
                textBox7.Text =textBox7.Text+" Elbow position: "+ OutObj.elbow_position.ToString();
            }
           
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (aRTClient.Status == RTClientStatus.Syncing)
                aRTClient.sendScript(textBox6.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            aRTDE.startSync();
            if (!aRTDE.isSynchronizing)
                Thread.Sleep(10);
            button4.Enabled = false;
            button5.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            aRTDE.stopSync();
            if (!aRTDE.isSynchronizing)
                Thread.Sleep(10);
            button4.Enabled = true;
            button5.Enabled = false;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (aRTDE.isSynchronizing)
            {
                try
                {
                    byte tempV = Convert.ToByte(textBox11.Text);
                    if (tempV >= 0 && tempV <= 255)
                    {
                        RTDEInputObj InObj = new RTDEInputObj();
                        InObj.standard_digital_output_mask = 255;
                        InObj.standard_digital_output = tempV;
                        aRTDE.setInputObj(InObj);
                    }
                }
                catch
                { }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (aRTDE.isSynchronizing)
            {
                try
                {
                    double tempV = Convert.ToDouble(textBox8.Text);
                    if (tempV > 0 && tempV <= 1)
                    {
                        RTDEInputObj InObj = new RTDEInputObj();
                        InObj.speed_slider_mask = 1;
                        InObj.speed_slider_fraction = tempV;
                        aRTDE.setInputObj(InObj);
                    }
                }
                catch { }
            }
        }
    }
}
