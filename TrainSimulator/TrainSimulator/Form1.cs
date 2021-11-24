using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBaseConnect;

// Socket related network namespace
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SimpleJSON;

namespace TrainSimulator
{
    public partial class form : Form
    {
        public form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        /************************************************
        *  Thread Relate Code
        ***********************************************/
        /*
                // C# Thread Create
                Thread thDAQ;
                bool bDAQThread = false;

                byte[] msg = new byte[200];
                private void InitDAQThread()
                {
                    if (!bDAQThread)
                    {
                        thDAQ = new Thread(new ThreadStart(DAQThread));
                        thDAQ.IsBackground = true;
                        bDAQThread = true;
                        thDAQ.Start();
                    }
                }

                private void AbortDAQThread()
                {
                    if (bDAQThread)
                    {
                        bDAQThread = false;
                        thDAQ.Abort();
                    }
                }
                static string[] DAQ1_address = new string[] { "10.1.33.161", "10.1.41.161", "10.1.49.161", "10.1.57.161", "10.1.65.161", "10.1.73.161", "193.168.1.161" };
                static string[] DAQ2_address = new string[] { "10.1.33.169", "10.1.41.169", "10.1.49.169", "10.1.57.169", "10.1.65.169", "10.1.73.169", "193.168.1.169" };
                static string[] CBM_address = new string[] { "10.1.33.153", "10.1.41.153", "10.1.49.153", "10.1.57.153", "10.1.65.153", "10.1.73.153", "193.168.1.110" };
                IPAddress DAQ_UDP_address = IPAddress.Parse(DAQ1_address[0]); //("193.168.0.161");
                IPAddress CBM_UDP_address = IPAddress.Parse(CBM_address[0]); //("10.1.33.153");
                int setDAQPort = 6001;
                private void DAQThread()
                {
                    // RX Socket
                    //IPEndPoint ipep = new IPEndPoint(IPAddress.Any, setDAQPort);
                    IPEndPoint ipep = new IPEndPoint(DAQ_UDP_address, setDAQPort);
                    Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                    try
                    {
                        server.Bind(ipep);
                        this.Invoke(new Action(delegate ()
                        {
                            //Sck_Crate_Lb.ForeColor = System.Drawing.Color.Blue;
                            //Sck_Crate_Lb.Text = "Success";
                        }));
                    }
                    catch (Exception e)
                    {
                        this.Invoke(new Action(delegate ()
                        {
                            //Sck_Crate_Lb.ForeColor = System.Drawing.Color.Red;
                            //Sck_Crate_Lb.Text = "Fail";
                        }));
                        Console.WriteLine("Winsock error: " + e.ToString());

                        bDAQThread = false;
                        thDAQ.Abort();
                        return;
                    }
                }
        */

        /************************************************
        *  DB Relate Code
        ***********************************************/

        DBConnect dBConnect;
        private void Init()
        {
            dBConnect = new DBConnect();
            LineSelect();
            StationSelect();
            TrainSelect();
            TMSelect();
            //StationInsert();
        }

        private void LineSelect()
        {
            ListView lvTarget = (Controls.Find("lv0", true)[0] as ListView);
            ListViewItem lvi;

            string selectdata = SelectDirectQuery("select * from tb_line");
            var selectjson = JSON.Parse(selectdata);

            JSONNode select = selectjson["data"]["select * from tb_line"];

            IEnumerable<JSONNode> e = select.Children;
            IEnumerator<JSONNode> b = e.GetEnumerator();

            while (b.MoveNext())
            {
                lvi = new ListViewItem();
                var lineCode = b.Current["LINECODE"];
                var active = b.Current["ACTIVE"];
                Console.WriteLine("");

                lvi.Text = lineCode;
                lvi.SubItems.Add(active);
                lvTarget.Items.Add(lvi);
            }
        }

        private void StationSelect()
        {
            ListView lvTarget = (Controls.Find("lv1", true)[0] as ListView);
            ListViewItem lvi;

            string selectdata = SelectDirectQuery("select * from tb_station");
            var selectjson = JSON.Parse(selectdata);

            JSONNode select = selectjson["data"]["select * from tb_station"];

            IEnumerable<JSONNode> e = select.Children;
            IEnumerator<JSONNode> b = e.GetEnumerator();

            while (b.MoveNext())
            {
                lvi = new ListViewItem();
                var stationCode = b.Current["STATIONCODE"];
                var line = b.Current["LINE"];
                var x = b.Current["X"].AsDouble;
                var y = b.Current["Y"].AsDouble;
                var z = b.Current["Z"].AsDouble;
                var w = b.Current["WIDTH"].AsDouble;
                var h = b.Current["HEIGHT"].AsDouble;
                var l = b.Current["LENGTH"].AsDouble;
                var r = b.Current["ROTATION"].AsDouble;
                var gPerMin = b.Current["GUESTPERMIN"].AsInt;
                var gMax = b.Current["GUESTMAX"].AsInt;
                var yard = b.Current["YARD"];
                var nStation = b.Current["NEXTSTATION"];
                var pStation = b.Current["PREVSTATION"];
                var note = b.Current["NOTE"];
                Console.WriteLine("");

                lvi.Text = stationCode;
                lvi.SubItems.Add(line);
                lvi.SubItems.Add(x.ToString());
                lvi.SubItems.Add(y.ToString());
                lvi.SubItems.Add(z.ToString());
                lvi.SubItems.Add(w.ToString());
                lvi.SubItems.Add(h.ToString());
                lvi.SubItems.Add(l.ToString());
                lvi.SubItems.Add(r.ToString());
                lvi.SubItems.Add(gPerMin.ToString());
                lvi.SubItems.Add(gMax.ToString());
                lvi.SubItems.Add(yard);
                lvi.SubItems.Add(nStation);
                lvi.SubItems.Add(pStation);
                lvi.SubItems.Add(note);
                lvTarget.Items.Add(lvi);
            }
        }

        private void StationInsert()
        {
            string linecd = "123";
            InsetQuery("insert into tb_line values('" + linecd + "')");
        }

        private void TrainSelect()
        {
            ListView lvTarget = (Controls.Find("lv2", true)[0] as ListView);
            ListViewItem lvi;

            string selectdata = SelectDirectQuery("select * from tb_subway");
            var selectjson = JSON.Parse(selectdata);

            JSONNode select = selectjson["data"]["select * from tb_subway"];

            IEnumerable<JSONNode> e = select.Children;
            IEnumerator<JSONNode> b = e.GetEnumerator();

            while (b.MoveNext())
            {
                lvi = new ListViewItem();
                var subwayCode = b.Current["SUBWAYCODE"];
                var startStation = b.Current["STARTSTATION"];
                var line = b.Current["LINE"];
                var type = b.Current["TYPE"];
                var speed = b.Current["SPEED"].AsDouble;
                var maxGuestCount = b.Current["MAXGUESTCOUNT"].AsInt;
                var note = b.Current["NOTE"];
                Console.WriteLine("");

                lvi.Text = subwayCode;
                lvi.SubItems.Add(startStation);
                lvi.SubItems.Add(line);
                lvi.SubItems.Add(type);
                lvi.SubItems.Add(speed.ToString());
                lvi.SubItems.Add(maxGuestCount.ToString());
                lvi.SubItems.Add(note);
                lvTarget.Items.Add(lvi);
            }
        }

        private void TMSelect()
        {
            ListView lvTarget = (Controls.Find("lv3", true)[0] as ListView);
            ListViewItem lvi;

            string selectdata = SelectDirectQuery("select * from tb_subwaymodel");
            var selectjson = JSON.Parse(selectdata);

            JSONNode select = selectjson["data"]["select * from tb_subwaymodel"];

            IEnumerable<JSONNode> e = select.Children;
            IEnumerator<JSONNode> b = e.GetEnumerator();

            while (b.MoveNext())
            {
                lvi = new ListViewItem();
                var smCode = b.Current["SUBWAYMODELCODE"];
                var w = b.Current["WIDTH"].AsDouble;
                var h = b.Current["HEIGHT"].AsDouble;
                var l = b.Current["LENGTH"].AsDouble;
                Console.WriteLine("");

                lvi.Text = smCode;
                lvi.SubItems.Add(w.ToString());
                lvi.SubItems.Add(h.ToString());
                lvi.SubItems.Add(l.ToString());
                lvTarget.Items.Add(lvi);
            }
        }


        public string SelectDirectQuery(string queryID)
        {
            string temp = string.Empty;

            if (Connect())
            {
                temp = dBConnect.SelectDirectQuery(queryID);

                Clear();
            }
            return temp;
        }

        public void InsetQuery(string queryID)
        {
            if (Connect())
            {
                dBConnect.InsertQuery(queryID);

                Clear();
            }
        }

        public void DeleteQuery(string queryID)
        {
            if (Connect())
            {
                dBConnect.DeleteQuery(queryID);

                Clear();
            }
        }

        bool isExit = true;
        public bool Connect()
        {
            bool result = true;

            if (isExit == true)
            {
                result = dBConnect.Connect();

                if (result == true)
                {
                    isExit = false;
                }
            }
            return result;
        }

        private bool Clear()
        {
            bool result = true;

            if (isExit == false)
            {
                result = dBConnect.Disconnect();

                if (result == true)
                {
                    isExit = true;
                }
            }
            return result;
        }

        /************************************************
        *  Communication Relate Code
        ***********************************************/// YARD 5 / Station 15

        IPEndPoint ipep;
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        string str_msg;                     // 유니티 전용 UDP 메세지
        int[] column_num = { 2, 15, 7, 4 }; // ListView 별 SubItem 갯수

        // [0] LIN1 -> 라인가동,    [1] LIN2 -> 라인정지
        // [2] STA1 -> 역 생성,     [3] STA2 -> 역 제거,     [4] STA3 -> 역 정보 변경
        // [5] SUB1 -> 열차생성,    [6] SUB2 -> 열차제거,    [7] SUB3 -> 열차 정보 변경(EX.속도)
        static string[] MSG_ID = new string[8] { "LIN1", "LIN2", "STA1", "STA2", "STA3", "SUB1", "SUB2", "SUB3" };
        
        int chkBtnTag;      // DB Table 관련 변수
        /* message length relate */
        //int headerSize = 4;
        //int mIDSize = 4;

        // Connect() 후 Send() 가능
        private void Insert_Click(object sender, EventArgs e)
        {
            int i, j;
            int mSize = 0;
            bool overlapItem = false;   // 중복 아이템 확인 변수
            string cmd_query = string.Empty;           // 쿼리문 등록 변수

            TextBox txtTarget;
            Button btn = sender as Button;          // Insert button
            chkBtnTag = Convert.ToInt32(btn.Tag);   // 탭구별 : 0 = 라인 / 1 = 역 / 2 = 열차 / 3 = 열차 모델
            ListView lvTarget = (Controls.Find("lv" + chkBtnTag.ToString(), true)[0] as ListView);
            ListViewItem lvi = new ListViewItem();

            str_msg = string.Empty;

            // Data
            for (i = 1; i <= column_num[chkBtnTag]; i++)
            {
                txtTarget = (Controls.Find("tb" + chkBtnTag.ToString() + "_" + i.ToString(), true)[0] as TextBox);
                //str_msg = string.Concat(str_msg, txtTarget.Text);
                str_msg += txtTarget.Text + ",";
                if (i == 1)
                {
                    lvi.Text = txtTarget.Text;

                    for (j = 0; j < lvTarget.Items.Count; j++)
                    {
                        if (lvTarget.Items[j].SubItems[0].Text.Equals(txtTarget.Text.Trim()))
                        {
                            overlapItem = true;
                        }
                    }
                }
                else lvi.SubItems.Add(txtTarget.Text);
            }

            str_msg = str_msg.Substring(0, str_msg.Length - 1);
            //str_msg += "\n"; // null(1byte)

            // overlapItem : true = Update, false = Insert
            if (overlapItem == true)
            {
                switch (chkBtnTag)
                {
                    case 0:
                        if (chk_active.Checked)
                        {
                            str_msg = MSG_ID[0] + str_msg;
                            cmd_query = "update tb_line set LINECODE ='" + tb0_1.Text + "',ACTIVE='" + tb0_2.Text + "' where LINECODE ='" + tb0_1.Text + "'";
                        }
                        else
                        {
                            str_msg = MSG_ID[1] + str_msg;
                            cmd_query = "update tb_line set LINECODE ='" + tb0_1.Text + "',ACTIVE='" + tb0_2.Text + "' where LINECODE ='" + tb0_1.Text + "'";
                        }
                        break;
                    case 1:
                        str_msg = MSG_ID[4] + str_msg;
                        cmd_query = "update tb_station set STATIONCODE ='" + tb1_1.Text + "',LINE='" + tb1_2.Text + "',X='" + tb1_3.Text + "',Y='" + tb1_4.Text + "',Z='" + tb1_5.Text +
                            "',WIDTH='" + tb1_6.Text + "',HEIGHT='" + tb1_7.Text + "',LENGTH='" + tb1_8.Text + "',ROTATION='" + tb1_9.Text + "',GUESTPERMIN='" + tb1_10.Text + 
                            "',GUESTMAX='" + tb1_11.Text + "',YARD='" + tb1_12.Text + "',NEXTSTATION='" + tb1_13.Text + "',PREVSTATION='" + tb1_14.Text + "',NOTE='" + tb1_15.Text +
                            "' where STATIONCODE ='" + tb1_1.Text + "'";
                        break;
                    case 2:
                        str_msg = MSG_ID[7] + str_msg;
                        cmd_query = "update tb_subway set SUBWAYCODE ='" + tb2_1.Text + "',STARTSTATION='" + tb2_2.Text + "',LINE='" + tb2_3.Text + "',TYPE='" + tb2_4.Text + "',SPEED='" + tb2_5.Text +
                            "',MAXGUESTCOUNT='" + tb2_6.Text + "',NOTE='" + tb2_7.Text + "' where SUBWAYCODE ='" + tb2_1.Text + "'";
                        break;
                }

                mSize = str_msg.Length + 4;

                Update_ListView(lvi);
                // string --> byte[]
                //byte[] _data = Encoding.Default.GetBytes("Server SendTo Data");
                byte[] _data = Encoding.UTF8.GetBytes(str_msg);

                InsetQuery(cmd_query);
                // SendTo()
                //client.SendTo(_data, _data.Length, SocketFlags.None, ipep);
                client.SendTo(_data, ipep); // UDP Communication
            }
            else
            {
                switch (chkBtnTag)
                {
                    case 0:
                        if (chk_active.Checked)
                        {
                            str_msg = MSG_ID[0] + str_msg;
                            cmd_query = "insert into tb_line (LINECODE, ACTIVE) " + "value ('" + tb0_1.Text + "', '" + tb0_2.Text + "');";
                        }
                        else
                        {
                            str_msg = MSG_ID[1] + str_msg;
                            cmd_query = "insert into tb_line (LINECODE, ACTIVE) " + "value ('" + tb0_1.Text + "', '" + tb0_2.Text + "');";
                        }
                        break;
                    case 1:
                        str_msg = MSG_ID[2] + str_msg;
                        cmd_query = "insert into tb_station (STATIONCODE, LINE, X, Y, Z, WIDTH, HEIGHT, LENGTH, ROTATION, GUESTPERMIN, GUESTMAX, YARD, NEXTSTATION, PREVSTATION, NOTE) " +
                                    "value ('" + tb1_1.Text + "', '" + tb1_2.Text + "', '" + tb1_3.Text + "', '" + tb1_4.Text + "', '" + tb1_5.Text + "', '"
                                               + tb1_6.Text + "', '" + tb1_7.Text + "', '" + tb1_8.Text + "', '" + tb1_9.Text + "', '" + tb1_10.Text + "', '"
                                               + tb1_11.Text + "', '" + tb1_12.Text + "', '" + tb1_13.Text + "', '" + tb1_14.Text + "', '" + tb1_15.Text + "');";
                        break;
                    case 2:
                        str_msg = MSG_ID[5] + str_msg;
                        cmd_query = "insert into tb_subway (SUBWAYCODE, STARTSTATION, LINE, TYPE, SPEED, MAXGUESTCOUNT, NOTE) " +
                                    "value ('" + tb2_1.Text + "', '" + tb2_2.Text + "', '" + tb2_3.Text + "', '" + tb2_4.Text + "', '" + tb2_5.Text + "', '"
                                               + tb2_6.Text + "', '" + tb2_7.Text + "');";
                        break;
                }

                mSize = str_msg.Length + 4;

                //lvTarget.Items.Insert(0, lvi);
                lvTarget.Items.Add(lvi);
                lvTarget.EnsureVisible(lvTarget.Items.Count - 1);
                byte[] _data = Encoding.UTF8.GetBytes(str_msg);

                InsetQuery(cmd_query);
                client.SendTo(_data, ipep); // UDP Communication
            }

            //IPEndPoint msg_sender = new IPEndPoint(IPAddress.Any, 0);
            // EndPoint remote = (EndPoint)(msg_sender);

            //_data = new byte[1024];

            // ReceiveFrom() 수신
            //client.ReceiveFrom(_data, ref remote);
            //Console.WriteLine("{0} : \r\nClient Receive Data : {1}", remote.ToString(),
            //Encoding.Default.GetString(_data));

            // Close()
            //client.Close();
        }

        private void Update_ListView(ListViewItem lvi)
        {
            ListView lvTarget = (Controls.Find("lv" + chkBtnTag.ToString(), true)[0] as ListView);
            for (int i = 0; i < lvTarget.Items.Count; i++)
            {
                if (lvTarget.Items[i].Text == lvi.Text)
                {
                    for (int j = 1; j <= column_num[chkBtnTag]; j++)
                    {
                        lvTarget.Items[i].SubItems[j - 1].Text = lvi.SubItems[j - 1].Text;
                    }
                }
            }
        }

        private void tb_port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        bool chkIPSet = false;
        static string[] def_address = new string[] { "127.0.0.1" };
        IPAddress UDP_address = IPAddress.Parse(def_address[0]);
        int setPort = 5001; // default
        private void btn_connect_Click(object sender, EventArgs e)
        {
            int i;
            Button btnTarget;
            if (!chkIPSet)
            {
                chkIPSet = !chkIPSet;
                //TextBox txtTarget1 = (Controls.Find("tb_ip", true)[0] as TextBox);
                //TextBox txtTarget2 = (Controls.Find("tb_port", true)[0] as TextBox);

                if (!(String.IsNullOrEmpty(tb_ip.Text)))
                {
                    UDP_address = IPAddress.Parse(tb_ip.Text);
                }
                if (!(String.IsNullOrEmpty(tb_port.Text)))
                {
                    setPort = Convert.ToInt32(tb_port.Text);
                }
                ipep = new IPEndPoint(UDP_address, setPort);

                try
                {
                    this.Invoke(new Action(delegate ()
                    {
                        lb_conn.ForeColor = System.Drawing.Color.Blue;
                        lb_conn.Text = "Success";
                    }));

                    for (i = 0; i < 4; i++)
                    {
                        btnTarget = (Controls.Find("btn_insert" + i.ToString(), true)[0] as Button);
                        btnTarget.Enabled = true;
                        btnTarget = (Controls.Find("btn_delete" + i.ToString(), true)[0] as Button);
                        btnTarget.Enabled = true;
                    }
                }
                catch (Exception)
                {
                    this.Invoke(new Action(delegate ()
                    {
                        lb_conn.ForeColor = System.Drawing.Color.Red;
                        lb_conn.Text = "Fail";
                    }));

                    //bDAQThread = false;
                    //thDAQ.Abort();
                    //return;
                }
            }
        }

        private void chk_enable_Click(object sender, EventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            if (chkBox.Checked)
            {
                switch (Convert.ToInt32(chkBox.Tag))
                {
                    case 0:
                        tb0_2.Text = "Y"; //Line_Active
                        break;
                    case 1:
                        tb1_12.Text = "Y";//Station_Yard
                        break;
                }
            }
            else
            {
                switch (Convert.ToInt32(chkBox.Tag))
                {
                    case 0:
                        tb0_2.Text = "N";   // Line_Active
                        break;
                    case 1:
                        tb1_12.Text = "N";  // Station_Yard
                        break;
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string selectKey;
            string cmd_query;
            Button btn = sender as Button;
            int chkBtnTag = Convert.ToInt32(btn.Tag);
            ListView lvTarget = (Controls.Find("lv" + chkBtnTag.ToString(), true)[0] as ListView);

            str_msg = string.Empty;

            switch (chkBtnTag)
            {
                case 0:
                    if (lvTarget.SelectedIndices.Count > 0)
                    {
                        selectKey = lvTarget.Items[lvTarget.FocusedItem.Index].SubItems[0].Text;
                        cmd_query = "delete from tb_line where LINECODE ='" + tb0_1.Text + "'";
                        DeleteQuery(cmd_query);
                        lvTarget.Items.RemoveAt(lvTarget.SelectedIndices[0]);
                    }
                    break;
                case 1:
                    if (lvTarget.SelectedIndices.Count > 0)
                    {
                        selectKey = lvTarget.Items[lvTarget.FocusedItem.Index].SubItems[0].Text;
                        cmd_query = "delete from tb_station where STATIONCODE ='" + tb1_1.Text + "'";
                        DeleteQuery(cmd_query);
                        lvTarget.Items.RemoveAt(lvTarget.SelectedIndices[0]);
                        str_msg = MSG_ID[3] + tb1_1.Text;
                    }
                    break;
                case 2:
                    if (lvTarget.SelectedIndices.Count > 0)
                    {
                        selectKey = lvTarget.Items[lvTarget.FocusedItem.Index].SubItems[0].Text;
                        cmd_query = "delete from tb_subway where SUBWAYCODE = '" + tb2_1.Text + "'";
                        DeleteQuery(cmd_query);
                        lvTarget.Items.RemoveAt(lvTarget.SelectedIndices[0]);
                        str_msg = MSG_ID[6] + tb2_1.Text;
                    }
                    break;
             }
                    byte[] _data = Encoding.UTF8.GetBytes(str_msg);
                    // SendTo()
                    //client.SendTo(_data, _data.Length, SocketFlags.None, ipep);
                    client.SendTo(_data, ipep);
            
        }

        private void lv_SelectedIndex(object sender, MouseEventArgs e)
        {
            TextBox txtTarget;
            ListView lvTarget = sender as ListView;

            int chkLvTag = Convert.ToInt32(lvTarget.Tag); // 탭구별, 0: 라인 / 1 : 역 / 2 : 열차  

            for (int i = 1; i <= lvTarget.Items[lvTarget.FocusedItem.Index].SubItems.Count; i++)
            {
                txtTarget = (Controls.Find("tb" + chkLvTag.ToString() + "_" + i.ToString(), true)[0] as TextBox);
                txtTarget.Text = lvTarget.Items[lvTarget.FocusedItem.Index].SubItems[i - 1].Text;
            }
            switch (chkLvTag)
            {
                case 0:
                    if (tb0_2.Text == "N") chk_active.Checked = false;
                    else if (tb0_2.Text == "Y") chk_active.Checked = true;
                    break;
                case 1:
                    if (tb1_12.Text == "N") chk_yard.Checked = false;
                    else if (tb1_12.Text == "Y") chk_yard.Checked = true;
                    break;
            }
        }
    }
}

