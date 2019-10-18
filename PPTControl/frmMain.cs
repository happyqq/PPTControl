using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MqttLib;
//using WebSocket4Net;
using ZXing.QrCode;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;
using Models;
using CommonHelper;
using InputSML;

namespace PPTControl
{
    public partial class frmMain : Form
    {
        //WebSocket wsPPTControl = new WebSocket("ws://42.121.56.70:9800/");
        const string strHello = "Hi,HappyQQ!";

        const string strAPPTITLE = "PPT控制器 "; //控制器控制器控制器控制器器

        const string strMQTT_TCP_Server = "tcp://desktop.happyqq.cn:1883";
        const string strMQTT_TCP_Server_Topic_Format = "pptcontrol/{0}/{1}";
        const string strMQTT_WEB_Server_Format = "http://webapi.happyqq.cn/pptcontrol/control/{0}/{1}";
        const string str_APP_My_Setting_Format = "http://webapi.happyqq.cn/setting/get/{0}";


        string strMachineID = string.Empty;
        string strClientId = Guid.NewGuid().ToString();
        string strMQTT_TCP_Server_Topic = string.Empty;
        string strMQTT_WEB_Server = string.Empty;
        string strDst = string.Empty;
        string strSRC = string.Empty;
        string sProceName = "POWERPNT"; //"WPP"
        string strADURL = "http://weibo.com/happyqq";


        //string clientID = string.Empty;

        EncodingOptions options = null;
        BarcodeWriter writer = null;
        MQTTMessage_Send message_send = null;
        MQTTMessage_Recv message_recv = null;


        public frmMain()
        {
            InitializeComponent();

            //strMachineID = SoftReg.getMNum(); //为获取CPU ID失败，改用取随机数的MD5值

            strMachineID = Encrypt.GetMD5(strClientId);
            //-------------------------------测试环境写死-----------------------------------------
            //strMachineID = "aaabbbccc";
            //strClientId = "111222333";
            strDst = strMachineID + "_" + strClientId;
            //-------------------------------------------------------------------------------------

            strMQTT_TCP_Server_Topic = string.Format(strMQTT_TCP_Server_Topic_Format, strMachineID, strClientId);
            strMQTT_WEB_Server = string.Format(strMQTT_WEB_Server_Format, strMachineID, strClientId);

            message_send = new MQTTMessage_Send();
            message_recv = new MQTTMessage_Recv();

            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = picQRCodeURL.Width,
                Height = picQRCodeURL.Height
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;

            Bitmap bitmap = writer.Write(strMQTT_WEB_Server);
            picQRCodeURL.Image = bitmap;

            string strID = string.Empty;
            string strHTTPReq = string.Empty;
            string strCustAppName = string.Empty;
            string strCustLogoSRC = string.Empty;
            string strCustLogoURL = string.Empty;

            string strEXEName = Application.ExecutablePath.Substring(Application.ExecutablePath.ToString().LastIndexOf("\\") + 1).ToLower();



            

            AppCustSetting message_setting = new AppCustSetting();

            if (strEXEName.IndexOf("__") != 0)
            {
                strID = "10000";
                strHTTPReq = HttpHelper.GetHTTPInfo(string.Format(str_APP_My_Setting_Format, strID), "utf-8", 2000);

                this.Text = strAPPTITLE + Application.ProductVersion.ToString();

                if (strHTTPReq.Trim() != "")
                {
                    message_setting = JsonHelper.DeserializeJsonToObject<AppCustSetting>(strHTTPReq);
                    lnkAD.Text = message_setting.GlobalADText.ToString();
                    strADURL = message_setting.GlobalADURL.ToString();
                    lnkAD.Visible = (message_setting.HiddenAD.ToString().ToLower() != "true");
                }
            }
            else
            {
                //通过Internet获取自定义的名称
                int iBegin = strEXEName.IndexOf("__");
                int iEnd = strEXEName.LastIndexOf(".");
                strID = strEXEName.Substring(iBegin + 2, iEnd - iBegin - 2);

                strHTTPReq = HttpHelper.GetHTTPInfo(string.Format(str_APP_My_Setting_Format, strID), "utf-8", 2000);

                if (strHTTPReq.Trim() != "")
                {



                    message_setting = JsonHelper.DeserializeJsonToObject<AppCustSetting>(strHTTPReq);

                    strCustAppName = message_setting.CustAppName.ToString();
                    strCustLogoSRC = message_setting.CustLogoSRC.ToString();
                    strCustLogoURL = message_setting.CustLogoURL.ToString();
                    lnkAD.Text = message_setting.GlobalADText.ToString();
                    strADURL = message_setting.GlobalADURL.ToString();
                    lnkAD.Visible = (message_setting.HiddenAD.ToString().ToLower() != "true");

                    this.Text = strAPPTITLE + " - " + strCustAppName;
                }
                else
                {
                    this.Text = strAPPTITLE + Application.ProductVersion.ToString();
                }

               
            }

            try
            {
                MQTTProgram(strMQTT_TCP_Server, strMQTT_TCP_Server_Topic);
            }
            catch (Exception ex)
            {

                MessageBox.Show("好像遇到点麻烦事了，亲，是不是你这个电脑还不能够联上互联网呀？");

            }





            // wsPPTControl.Opened += new EventHandler(wsPPTControl_Opened);
            // wsPPTControl.Open();


            //websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            //websocket.Closed += new EventHandler(websocket_Closed);
            //websocket.MessageReceived += new EventHandler(websocket_MessageReceived);

        }

        private void lnkAbout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAbout myfrm = new frmAbout();
            myfrm.ShowDialog();
        }

        private void lnkSetting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSetting myfrm = new frmSetting();
            myfrm.ShowDialog();
        }


        //void MyMain(string[] args)
        //{
        //    if (args.Length != 2)
        //    {

        //        Console.WriteLine("Usage: " + Environment.GetCommandLineArgs()[0] + " ConnectionString ClientId");
        //        return;
        //    }

        //    Console.WriteLine("Starting MqttDotNet sample program.");
        //    Console.WriteLine("Press any key to stop\n");

        //    MQTTProgram(args[0], args[1]);
        //    Start();

        //    Console.ReadKey();
        //    Stop();
        //}

        IMqtt _client;

        void MQTTProgram(string connectionString, string clientId)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(clientId))
            {
                return;
            }
            // Instantiate client using MqttClientFactory
            _client = MqttClientFactory.CreateClient(connectionString, clientId);

            // Setup some useful client delegate callbacks
            _client.Connected += new ConnectionDelegate(client_Connected);
            _client.ConnectionLost += new ConnectionDelegate(_client_ConnectionLost);
            _client.PublishArrived += new PublishArrivedDelegate(client_PublishArrived);



            Start();
            //Stop();
        }

        void Start()
        {
            // Connect to broker in 'CleanStart' mode
            Console.WriteLine("Client connecting\n");
            _client.Connect(true);
        }

        void Stop(string msg = "bye")
        {
            if (_client.IsConnected)
            {
                SendMsg(msg);
                btnClose.Visible = false;
                _client.Disconnect();
                Console.WriteLine("Client disconnected\n");
            }
        }

        void client_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Client connected\n");
            RegisterOurSubscriptions();
            //PublishSomething();
        }

        void _client_ConnectionLost(object sender, EventArgs e)
        {
            SendMsg("lost");
            Console.WriteLine("Client connection lost\n");
        }

        void RegisterOurSubscriptions()
        {
            Console.WriteLine("Subscribing to mqttdotnet/subtest/#\n");
            _client.Subscribe(strMQTT_TCP_Server_Topic, QoS.BestEfforts);
        }

        void PublishSomething()
        {
            Console.WriteLine("Publishing on mqttdotnet/pubtest\n");
            _client.Publish(strMQTT_TCP_Server_Topic, "Hello MQTT World abc 123dddddd", QoS.OnceAndOnceOnly, false);
        }
        bool CheckPPTISRunning()
        {
            string sProceName1 = "POWERPNT"; //"WPP"
            string sProceName2 = "WPP"; //"WPP"
            if (ProcessHelper.ProcessIsRunning(sProceName1))
            {
                sProceName = sProceName1;
                return true;
            }

            if (ProcessHelper.ProcessIsRunning(sProceName2))
            {
                sProceName = sProceName2;
                return true;
            }
            return false;
        }
        bool client_PublishArrived(object sender, PublishArrivedArgs e)
        {
            /*
            Console.WriteLine("Received Message");
            Console.WriteLine("Topic: " + e.Topic);
            Console.WriteLine("Payload: " + e.Payload);
            Console.WriteLine();*/



            string sAction = string.Empty;
            string sDst = string.Empty;

            //{ "src":"BN83I6IIBX3QP1DS4Y8URO41P2VWOTY9","dst":"aaabbbccc_111222333","msg":"hello"}

            //{ "src":"K91NS8PPF1B7KLNI2VWGO0D49HOM524T","dst":"testid000001_testst000001","msg":"hello"}
            //{"src":"K91NS8PPF1B7KLNI2VWGO0D49HOM524T","dst":"testid000001_testst000001","msg":"start"}
            //{"src":"K91NS8PPF1B7KLNI2VWGO0D49HOM524T","dst":"testid000001_testst000001","msg":"prev"}
            //{"src":"K91NS8PPF1B7KLNI2VWGO0D49HOM524T","dst":"testid000001_testst000001","msg":"next"}

            message_recv = JsonHelper.DeserializeJsonToObject<MQTTMessage_Recv>(e.Payload.ToString());

            sAction = message_recv.msg.ToString().ToLower();
            sDst = message_recv.dst.ToString();
            strSRC = message_recv.src.ToString();

            bool isPPTRunning = CheckPPTISRunning(); //确保 sProceName 全局变量被赋值 

            if (strDst == sDst)
            {
                if (sAction == "hello")
                {
                    if (!isPPTRunning)
                    {

                        SendState("0");
                    }
                    else
                    {
                        //btnClose.Visible = true;



                        if (ProcessHelper.ProcessIsFront(sProceName))
                        {
                            SendState("3");
                        }
                        else
                        {
                            SendState("1");
                        }

                    }

                }
                else if (sAction == "start")
                {
                    ProcessHelper.BringWindowToFrontNow(sProceName);
                    InputSMLTR.SimulateKeyPress(VirtualKeyCode.F5);
                }
                else if (sAction == "prev")
                {
                    ProcessHelper.BringWindowToFrontNow(sProceName);
                    InputSMLTR.SimulateKeyPress(VirtualKeyCode.UP);
                }
                else if (sAction == "next")
                {
                    ProcessHelper.BringWindowToFrontNow(sProceName);
                    InputSMLTR.SimulateKeyPress(VirtualKeyCode.DOWN);
                }
            }


            return true;
        }
        public delegate void Buttondelegate(Button btn, bool isVisible);
        public void button(Button btn, bool isVisible)
        {
            if (btn.InvokeRequired)
            {
                Buttondelegate dt = new Buttondelegate(button);
                btn.Invoke(dt, new object[] { btn });
            }
            else
            {
                btn.Visible = isVisible;
            }
        }
        //MQTTProgram("tcp://42.121.56.70:1883", "testid");

        //private void wsPPTControl_Opened(object sender, EventArgs e)
        //{
        //    wsPPTControl.Send(strHello + "First time,I Love You!\n");
        //}
        private void SendMsg(string sMsg, string state = "")
        {
            message_send.src = strDst;
            message_send.dst = strSRC;

            // message_send.src =strSRC ;
            // message_send.dst = strDst;
            message_send.msg = sMsg;
            message_send.state = state;

            string json_message_send = JsonHelper.SerializeObject(message_send);
            _client.Publish(strMQTT_TCP_Server_Topic, json_message_send, QoS.BestEfforts, false);
        }

        private void SendState(string state)
        {

            SendMsg("state", state);
        }
        private void btnTest_Click(object sender, EventArgs e)
        {


            // wsPPTControl.Send(strHello + "I am Test!\n");
            // K91NS8PPF1B7KLNI2VWGO0D49HOM524T
            // "pptcontrol/testid000001/testst000001"

            //{"src":"K91NS8PPF1B7KLNI2VWGO0D49HOM524T","dst":"K91NS8PPF1B7KLNI2VWGO0D49HOM524T","msg":"state","state":"1"}

            SendState("1");

            //_client.Publish(strMQTT_TCP_Server_Topic, "Hello MQTT World abc TestID abc", QoS.OnceAndOnceOnly, false);




        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("Will Close it??",)
            if (MessageBox.Show("主人，你是否确定要退出PPT控制器？", "确定退出？", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            //Stop("quit");
            SendMsg("quit");
            


        }

        private void lnkHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ppt.happyqq.cn");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void lnkAD_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(strADURL);
        }
    }
}
