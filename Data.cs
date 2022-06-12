using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using Search.DsSearchTableAdapters;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace Search
{
    public enum Query
    {
        @Ipu,
        IpuLoop,
        IpuPort,
        IpuCCU,
        IpuOC,
        Ser,
        SerObc,
        SerObcRack,
        SerObcRackPos,
        Place,
        PlaceObj,
        CabLay
    }
    public class Data
    {
        
        private OleDbConnection con;
        private OleDbDataAdapter da;
        private AlarmsTableAdapter alarmsTableAdapter;
        private feu_codeTableAdapter feu_CodeTableAdapter;
        private OpisanieTableAdapter opisanieTableAdapter;
        private ClassTableAdapter classTableAdapter;
        private DsSearch dsSearch;

        private DataSet ds;
        private DataTable tableMain;

        private BindingSource bind_1;
        private BindingSource bind_2;
        private BindingSource bind_3;
        private BindingSource bind_4;
        private BindingSource bind_search;


        private bool remote;
        private string localDbPath;
        private string remoteDbPath;

        private bool error;
        private string errMesg;

        private Query query;

        private bool disableEvents;

        public bool Error { get => this.error; set => this.error = value; }
        public string ErrMesg { get => this.errMesg; set => this.errMesg = value; }

        public bool Remote { get => this.remote; set => this.remote = value; }
        public DataTable TableMain { get => this.tableMain; set => this.tableMain = value; }
        public BindingSource Bind_1 { get => this.bind_1; set => this.bind_1 = value; }
        public BindingSource Bind_2 { get => this.bind_2; set => this.bind_2 = value; }
        public BindingSource Bind_3 { get => this.bind_3; set => this.bind_3 = value; }
        public BindingSource Bind_4 { get => this.bind_4; set => this.bind_4 = value; }
        public BindingSource Bind_search { get => this.bind_search; set => this.bind_search = value; }
        public NotifyHelper NotifyHelper { get => this.notifyHelper; set => this.notifyHelper = value; }
        public DsSearch DsSearch { get => this.dsSearch; set => this.dsSearch = value; }
        public string RemoteDbPath { get => this.remoteDbPath; set => this.remoteDbPath = value; }

        NotifyHelper notifyHelper;

        public Data()
        {
            localDbPath = UserDataFolder + "db/db.mdb";
#if DEBUG
            localDbPath = AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.DbLocalPath;
#endif
            remoteDbPath = Properties.Settings.Default.DbRemotePath;
            remote = Properties.Settings.Default.RemoteDb;
            bind_1 = new BindingSource();
            bind_2 = new BindingSource();
            bind_3 = new BindingSource();
            bind_4 = new BindingSource();
            bind_search = new BindingSource();
            bind_1.CurrentItemChanged += Bind_1_CurrentItemChanged;
            bind_2.CurrentItemChanged += Bind_2_CurrentItemChanged;
            bind_3.CurrentItemChanged += Bind_3_CurrentItemChanged;
            bind_4.CurrentItemChanged += Bind_4_CurrentItemChanged;
            bind_search.CurrentItemChanged += Bind_search_CurrentItemChanged;
            alarmsTableAdapter = new AlarmsTableAdapter();
            feu_CodeTableAdapter = new feu_codeTableAdapter();
            opisanieTableAdapter = new OpisanieTableAdapter();
            classTableAdapter = new ClassTableAdapter();
            dsSearch = new DsSearch();
            notifyHelper = new NotifyHelper();
        }

        private void Bind_search_CurrentItemChanged(object sender, EventArgs e)
        {
            DataRowView currentSearch = (DataRowView)((BindingSource)sender).Current;
            if (currentSearch == null)
            {
                return;
            }
            notifyHelper.Cab = !string.IsNullOrWhiteSpace(currentSearch["CAB"].ToString());
            notifyHelper.Loop = string.IsNullOrWhiteSpace(currentSearch["IPUloop"].ToString());
            notifyHelper.IpuLoop = !string.IsNullOrWhiteSpace(currentSearch["IPUloop"].ToString());
            notifyHelper.Next = ((BindingSource)sender).Position < ((BindingSource)sender).Count - 1;
            notifyHelper.Prev = ((BindingSource)sender).Position != 0;
            notifyHelper.Pomps = currentSearch["PROM"].ToString() == "POMPS"
                && string.IsNullOrWhiteSpace(currentSearch["IPUloop"].ToString());
            notifyHelper.Sildz = currentSearch["PROM"].ToString().Contains("SILDZ")
                && string.IsNullOrWhiteSpace(currentSearch["IPUloop"].ToString());
            notifyHelper.Viomps = currentSearch["PROM"].ToString() == "VIOMPS";
        }

        private void Bind_4_CurrentItemChanged(object sender, EventArgs e)
        {
            if (disableEvents)
            {
                return;
            }
            DataRowView current4 = (DataRowView)((BindingSource)sender).Current;
            switch (query)
            {
                case Query.SerObcRackPos:
                    bind_search.Filter = "SER ='" + current4["SER"]
                            + "' AND " + "OBC ='" + current4["OBC"] + "' AND H ='" + current4["H"]
                            + "' AND POS = '" + current4["POS"] + "'";
                    break;
                default:
                    break;
            }
        }

        private void Bind_3_CurrentItemChanged(object sender, EventArgs e)
        {
            if (disableEvents)
            {
                return;
            }
            DataRowView current3 = (DataRowView)((BindingSource)sender).Current;
            switch (query)
            {
                case Query.SerObcRack:
                    bind_search.Filter = "SER ='" + current3["SER"]
                            + "' AND " + "OBC ='" + current3["OBC"] + "' AND H ='" + current3["H"] + "'";
                    break;
                case Query.SerObcRackPos:
                    bind_4.Filter = "SER ='" + current3["SER"]
                            + "' AND " + "OBC ='" + current3["OBC"] + "' AND H ='" + current3["H"] + "'";
                    break;
                default:
                    break;
            }
        }

        private void Bind_2_CurrentItemChanged(object sender, EventArgs e)
        {
            if (disableEvents)
            {
                return;
            }
            DataRowView current2 = (DataRowView)((BindingSource)sender).Current;
            switch (query)
            {
                case Query.Ipu:
                    //bind_search.Filter = "IPU ='" + current1["IPU"] + "'";
                    break;
                case Query.IpuLoop:
                    bind_search.Filter = "IPU ='" + current2["IPU"] + "' AND " + "LOOP ='" + current2["LOOP"] + "'";
                    break;
                case Query.IpuPort:
                    bind_search.Filter = "IPU ='" + current2["IPU"] + "' AND " + "PORT ='" + current2["PORT"] + "'";
                    break;
                case Query.IpuCCU:
                    bind_search.Filter = "IPU ='" + current2["IPU"] + "' AND " + "CCU ='" + current2["CCU"] + "'";
                    break;
                case Query.IpuOC:
                    bind_search.Filter = "IPU ='" + current2["IPU"] + "' AND " + "A1A2 ='" + current2["A1A2"] + "'";
                    break;
                case Query.SerObc:
                    bind_search.Filter = "SER ='" + current2["SER"] + "' AND " + "OBC ='" + current2["OBC"] + "'";
                    break;
                case Query.SerObcRack:
                case Query.SerObcRackPos:
                    bind_3.Filter = "SER ='" + current2["SER"] + "' AND " + "OBC ='" + current2["OBC"] + "'";
                    break;
                case Query.PlaceObj:
                    bind_search.Filter = "MESTO = '" + current2["PLACE"] + "' AND"
                                               + "(P1 = '" + current2["OBJ"] + "'"
                                             + "OR P2 = '" + current2["OBJ"] + "'"
                                             + "OR S1 = '" + current2["OBJ"] + "'"
                                             + "OR S2 = '" + current2["OBJ"] + "'"
                                             + "OR S3 = '" + current2["OBJ"] + "'"
                                             + "OR S4 = '" + current2["OBJ"] + "'"
                                             + "OR R1 = '" + current2["OBJ"] + "'"
                                             + "OR R2 = '" + current2["OBJ"] + "'"
                                             + "OR R3 = '" + current2["OBJ"] + "'"
                                             + "OR R4 = '" + current2["OBJ"] + "'"
                                             + "OR R5 = '" + current2["OBJ"] + "'"
                                             + "OR R6 = '" + current2["OBJ"] + "'"
                                             + "OR R7 = '" + current2["OBJ"] + "'"
                                             + "OR R8 = '" + current2["OBJ"] + "'"
                                             + "OR R9 = '" + current2["OBJ"] + "'"
                                             + "OR R10 = '" + current2["OBJ"] + "'"
                                             + "OR R11 = '" + current2["OBJ"] + "'"
                                             + "OR R12 = '" + current2["OBJ"] + "')";
                    break;
                case Query.CabLay:
                    bind_search.Filter = "MESTO = '" + current2["PLACE"] + "' AND"
                                               + "(P1 = '" + current2["OBJ"] + "'"
                                             + "OR P2 = '" + current2["OBJ"] + "'"
                                             + "OR S1 = '" + current2["OBJ"] + "'"
                                             + "OR S2 = '" + current2["OBJ"] + "'"
                                             + "OR S3 = '" + current2["OBJ"] + "'"
                                             + "OR S4 = '" + current2["OBJ"] + "')";
                    break;
                default:
                    break;
            }
        }

        private void Bind_1_CurrentItemChanged(object sender, EventArgs e)
        {
            if (disableEvents)
            {
                return;
            }
            DataRowView current1 = (DataRowView)((BindingSource)sender).Current;
            DataRowView current2;
            DataRowView current3;
            DataRowView current4;
            switch (query)
            {
                case Query.Ipu:
                    bind_search.Filter = "IPU ='" + current1["IPU"] + "'";
                    break;
                case Query.IpuLoop:
                    bind_2.Filter = "IPU ='" + current1["IPU"] + "'";
                    current2 = (DataRowView)(bind_2).Current;
                    if (current2 != null)
                    {
                        bind_search.Filter = "IPU ='" + current1["IPU"] + "' AND " + "LOOP ='" + current2["LOOP"] + "'";
                    }
                    break;
                case Query.IpuPort:
                    bind_2.Filter = "IPU ='" + current1["IPU"] + "'";
                    current2 = (DataRowView)(bind_2).Current;
                    if (current2 != null)
                    {
                        bind_search.Filter = "IPU ='" + current1["IPU"] + "' AND " + "PORT ='" + current2["PORT"] + "'";
                    }
                    break;
                case Query.IpuCCU:
                    bind_2.Filter = "IPU ='" + current1["IPU"] + "'";
                    current2 = (DataRowView)(bind_2).Current;
                    if (current2 != null)
                    {
                        bind_search.Filter = "IPU ='" + current1["IPU"] + "' AND " + "CCU ='" + current2["CCU"] + "'";
                    }
                    break;
                case Query.IpuOC:
                    bind_2.Filter = "IPU ='" + current1["IPU"] + "'";
                    current2 = (DataRowView)(bind_2).Current;
                    if (current2 != null)
                    {
                        bind_search.Filter = "IPU ='" + current1["IPU"] + "' AND " + "A1A2 ='" + current2["A1A2"] + "'";
                    }
                    break;
                case Query.Ser:
                    bind_search.Filter = "SER ='" + current1["SER"] + "'";
                    break;
                case Query.SerObc:
                    bind_2.Filter = "SER ='" + current1["SER"] + "'";
                    current2 = (DataRowView)(bind_2).Current;
                    if (current2 != null)
                    {
                        bind_search.Filter = "SER ='" + current1["SER"] + "' AND " + "OBC ='" + current2["OBC"] + "'";
                    }
                    break;
                case Query.SerObcRack:
                    bind_2.Filter = "SER ='" + current1["SER"] + "'";
                    current2 = (DataRowView)(bind_2).Current;
                    current3 = (DataRowView)(bind_3).Current;
                    if (current2 != null)
                    {
                        bind_3.Filter = "SER ='" + current1["SER"] + "' AND " + "OBC ='" + current2["OBC"] + "'";
                    }
                    if (current3 != null)
                    {
                        bind_search.Filter = "SER ='" + current1["SER"]
                            + "' AND " + "OBC ='" + current2["OBC"] + "' AND H ='" + current3["H"] + "'";
                    }
                    break;
                case Query.SerObcRackPos:
                    bind_2.Filter = "SER ='" + current1["SER"] + "'";
                    current2 = (DataRowView)(bind_2).Current;
                    current3 = (DataRowView)(bind_3).Current;
                    current4 = (DataRowView)(bind_4).Current;
                    if (current2 != null)
                    {
                        bind_3.Filter = "SER ='" + current1["SER"] + "' AND " + "OBC ='" + current2["OBC"] + "'";
                    }
                    if (current3 != null)
                    {
                        bind_4.Filter = "SER ='" + current1["SER"]
                            + "' AND " + "OBC ='" + current2["OBC"] + "' AND H ='" + current3["H"] + "'";
                    }
                    if (current4 != null)
                    {
                        bind_search.Filter = "SER ='" + current1["SER"]
                            + "' AND " + "OBC ='" + current2["OBC"] + "' AND H ='" + current3["H"]
                            + "' AND POS = '" + current4["POS"] + "'";
                    }
                    break;
                case Query.Place:
                    bind_search.Filter = "MESTO ='" + current1["PLACE"] + "'";
                    break;
                case Query.PlaceObj:
                    bind_2.Filter = "PLACE ='" + current1["PLACE"] + "'";
                    current2 = (DataRowView)(bind_2).Current;
                    if (current2 != null)
                    {
                        bind_search.Filter = "MESTO = '" + current1["PLACE"] + "' AND"
                                               + "(P1 = '" + current2["OBJ"] + "'"
                                             + "OR P2 = '" + current2["OBJ"] + "'"
                                             + "OR S1 = '" + current2["OBJ"] + "'"
                                             + "OR S2 = '" + current2["OBJ"] + "'"
                                             + "OR S3 = '" + current2["OBJ"] + "'"
                                             + "OR S4 = '" + current2["OBJ"] + "'"
                                             + "OR R1 = '" + current2["OBJ"] + "'"
                                             + "OR R2 = '" + current2["OBJ"] + "'"
                                             + "OR R3 = '" + current2["OBJ"] + "'"
                                             + "OR R4 = '" + current2["OBJ"] + "'"
                                             + "OR R5 = '" + current2["OBJ"] + "'"
                                             + "OR R6 = '" + current2["OBJ"] + "'"
                                             + "OR R7 = '" + current2["OBJ"] + "'"
                                             + "OR R8 = '" + current2["OBJ"] + "'"
                                             + "OR R9 = '" + current2["OBJ"] + "'"
                                             + "OR R10 = '" + current2["OBJ"] + "'"
                                             + "OR R11 = '" + current2["OBJ"] + "'"
                                             + "OR R12 = '" + current2["OBJ"] + "')"; 
                    }
                    break;
                case Query.CabLay:
                    bind_2.Filter = "PLACE ='" + current1["PLACE"] + "'";
                    current2 = (DataRowView)(bind_2).Current;
                    if (current2 != null)
                    {
                        bind_search.Filter = "MESTO = '" + current1["PLACE"] + "' AND"
                                               + "(P1 = '" + current2["OBJ"] + "'"
                                             + "OR P2 = '" + current2["OBJ"] + "'"
                                             + "OR S1 = '" + current2["OBJ"] + "'"
                                             + "OR S2 = '" + current2["OBJ"] + "'"
                                             + "OR S3 = '" + current2["OBJ"] + "'"
                                             + "OR S4 = '" + current2["OBJ"] + "')";
                    }
                    break;
                default:
                    break;
            }
        }

        private string GetConnString()
        {
            string connString = "";
            string provider = "Microsoft.ACE.OLEDB.12.0;";
            //string provider = "Microsoft.Jet.OLEDB.4.0;";
            if (remote)
            {
                connString =
                    @"Provider=" + provider +
                    @"Data Source=" + remoteDbPath + ";";
            }
            else
            {
                connString =
                    @"Provider=" + provider +
                    @"Data Source=" + localDbPath + ";";
            }
            return connString;
        }

        public bool GetData()
        {
            error = false;
            string connString = GetConnString();

            con = new OleDbConnection(connString);
            try
            {
                con.Open();
                da = new OleDbDataAdapter("SELECT * FROM MAIN", con);
                ds = new DataSet();
                da.Fill(ds, "MAIN");
                tableMain = ds.Tables["MAIN"];
                bind_search.DataSource = tableMain;

                //string orderSql = "SELECT DISTINCT IPU FROM MAIN AS [IPU]";
                string orderIpuSql = "SELECT IPU.IPU,  ipu_sort.Sort" +
                                  " FROM MAIN AS [IPU] INNER JOIN ipu_sort ON IPU.IPU = ipu_sort.IPU" +
                                  " GROUP BY IPU.IPU,  ipu_sort.Sort" +
                                  " ORDER BY ipu_sort.Sort";
                //string orderSerSql = "SELECT DISTINCT SER FROM MAIN AS [SER] WHERE SER <> ''"
                string orderSerSql = "SELECT SER.SER,  ser_sort.Sort" +
                                  " FROM MAIN AS [SER] INNER JOIN ser_sort ON SER.SER = ser_sort.SER" +
                                  " WHERE SER.SER <> ''" +
                                  " GROUP BY SER.SER,  ser_sort.Sort" +
                                  " ORDER BY ser_sort.Sort";
                //string orderPlaceSql = "SELECT DISTINCT MESTO AS [PLACE] FROM MAIN AS [PLACE] WHERE MESTO <> ''"
                string orderPlaceSql = "SELECT PLACE.MESTO AS [PLACE],  place_sort.Sort" +
                                  " FROM MAIN AS [PLACE] INNER JOIN place_sort ON PLACE.MESTO = place_sort.PLACE" +
                                  " WHERE PLACE.MESTO <> ''" +
                                  " GROUP BY PLACE.MESTO,  place_sort.Sort" +
                                  " ORDER BY place_sort.Sort";
                da = new OleDbDataAdapter(orderIpuSql, con);
                //
                da.Fill(ds, "IPU");

                da.SelectCommand.CommandText = "SELECT DISTINCT IPU, LOOP FROM MAIN AS [IPULOOP]";
                da.Fill(ds, "IPULOOP");

                da.SelectCommand.CommandText = "SELECT DISTINCT IPU, PORT FROM MAIN AS [IPUPORT]";
                da.Fill(ds, "IPUPORT");

                da.SelectCommand.CommandText = "SELECT DISTINCT IPU, CCU FROM MAIN AS [IPUCCU] WHERE CCU <> ''";
                da.Fill(ds, "IPUCCU");

                da.SelectCommand.CommandText = "SELECT DISTINCT IPU, A1A2 FROM MAIN AS [IPUOC] WHERE A1A2 <> ''";
                da.Fill(ds, "IPUOC");

                da = new OleDbDataAdapter(orderSerSql, con);
                da.Fill(ds, "SER");

                da = new OleDbDataAdapter("SELECT DISTINCT SER, OBC FROM MAIN AS [SEROBC] WHERE OBC <> '' AND SER <> ''", con);
                da.Fill(ds, "SEROBC");

                da = new OleDbDataAdapter("SELECT DISTINCT SER, OBC, H FROM MAIN AS [SEROBCH] WHERE OBC <> '' AND SER <> '' AND H <> ''", con);
                da.Fill(ds, "SEROBCH");

                da = new OleDbDataAdapter("SELECT DISTINCT SER, OBC, H, POS FROM MAIN AS [SEROBCHPOS] WHERE OBC <> '' AND SER <> '' AND H <> '' AND POS <> ''", con);
                da.Fill(ds, "SEROBCHPOS");

                da = new OleDbDataAdapter(orderPlaceSql, con);
                da.Fill(ds, "PLACE");

                da = new OleDbDataAdapter(sqlObjects, con);
                da.Fill(ds, "PLACEOBJ");

                da = new OleDbDataAdapter("SELECT DISTINCT MESTO AS [PLACE] FROM MAIN AS [CABPLACE] WHERE MESTO <> '' AND CAB <> ''", con);
                da.Fill(ds, "CABPLACE");

                da = new OleDbDataAdapter(sqlObjectsCab, con);
                da.Fill(ds, "CABPLACEOBJ");
                
                alarmsTableAdapter.Connection = con;
                alarmsTableAdapter.Fill(dsSearch.Alarms);

                feu_CodeTableAdapter.Connection = con;
                feu_CodeTableAdapter.Fill(dsSearch.feu_code);

                opisanieTableAdapter.Connection = con;
                opisanieTableAdapter.Fill(dsSearch.Opisanie);

                classTableAdapter.Connection = con;
                classTableAdapter.Fill(dsSearch.Class);

            }
            catch (Exception e)
            {
                error = true;
                errMesg = e.Message;
            }
            finally
            {
                con.Close();
            }
            return !error;
        }

        public bool ConstructSources(Query query)
        {
            disableEvents = true;
            error = false;
            this.query = query;
            bind_1.Filter = "";
            bind_1.DataSource = null;
            bind_2.Filter = "";
            bind_2.DataSource = null;
            bind_3.Filter = "";
            bind_3.DataSource = null;
            bind_4.Filter = "";
            bind_4.DataSource = null;
            bind_search.Filter = "";
            try
            {
                switch (query)
                {
                    case Query.Ipu:
                        bind_1.DataSource = ds.Tables["IPU"];
                        break;
                    case Query.IpuLoop:
                        bind_1.DataSource = ds.Tables["IPU"];
                        bind_2.DataSource = ds.Tables["IPULOOP"];
                        break;
                    case Query.IpuPort:
                        bind_1.DataSource = ds.Tables["IPU"];
                        bind_2.DataSource = ds.Tables["IPUPORT"];
                        break;
                    case Query.IpuCCU:
                        bind_1.DataSource = ds.Tables["IPU"];
                        bind_2.DataSource = ds.Tables["IPUCCU"];
                        break;
                    case Query.IpuOC:
                        bind_1.DataSource = ds.Tables["IPU"];
                        bind_2.DataSource = ds.Tables["IPUOC"];
                        break;
                    case Query.Ser:
                        bind_1.DataSource = ds.Tables["SER"];
                        break;
                    case Query.SerObc:
                        bind_1.DataSource = ds.Tables["SER"];
                        bind_2.DataSource = ds.Tables["SEROBC"];
                        break;
                    case Query.SerObcRack:
                        bind_1.DataSource = ds.Tables["SER"];
                        bind_2.DataSource = ds.Tables["SEROBC"];
                        bind_3.DataSource = ds.Tables["SEROBCH"];
                        break;
                    case Query.SerObcRackPos:
                        bind_1.DataSource = ds.Tables["SER"];
                        bind_2.DataSource = ds.Tables["SEROBC"];
                        bind_3.DataSource = ds.Tables["SEROBCH"];
                        bind_4.DataSource = ds.Tables["SEROBCHPOS"];
                        break;
                    case Query.Place:
                        bind_1.DataSource = ds.Tables["PLACE"];
                        break;
                    case Query.PlaceObj:
                        bind_1.DataSource = ds.Tables["PLACE"];
                        bind_2.DataSource = ds.Tables["PLACEOBJ"];
                        break;
                    case Query.CabLay:
                        bind_1.DataSource = ds.Tables["CABPLACE"];
                        bind_2.DataSource = ds.Tables["CABPLACEOBJ"];
                        break;
                    default:
                        break;
                }
                disableEvents = false;
                Bind_1_CurrentItemChanged(bind_1, new EventArgs());
            }
            catch (Exception e)
            {
                error = true;
                errMesg = e.Message;
            }
            return !error;
        }

        public bool UpdateAlarms()
        {
            try
            {
                alarmsTableAdapter.Update(dsSearch.Alarms);
                alarmsTableAdapter.Fill(dsSearch.Alarms);
            }
            catch (Exception e)
            {
                this.errMesg = e.Message;
                return false;
            }
            return true;
        }

        public string sqlObjects =
          @"SELECT MESTO AS [PLACE], P1 AS [OBJ] FROM MAIN
            WHERE P1<>''
            UNION
            SELECT MESTO AS [PLACE], P2 FROM MAIN
            WHERE P2<>''
            UNION
            SELECT MESTO AS [PLACE], S1 FROM MAIN
            WHERE S1<>''
            UNION
            SELECT MESTO AS [PLACE], S2 FROM MAIN
            WHERE S2<>''
            UNION
            SELECT MESTO AS [PLACE], S3 FROM MAIN
            WHERE S3<>''
            UNION
            SELECT MESTO AS [PLACE], S4 FROM MAIN
            WHERE S4<>''
            UNION
            SELECT MESTO AS [PLACE], R1 FROM MAIN
            WHERE R1<>''
            UNION
            SELECT MESTO AS [PLACE], R2 FROM MAIN
            WHERE R2<>''
            UNION
            SELECT MESTO AS [PLACE], R3 FROM MAIN
            WHERE R3<>''
            UNION
            SELECT MESTO AS [PLACE], R4 FROM MAIN
            WHERE R4<>''
            UNION
            SELECT MESTO AS [PLACE], R5 FROM MAIN
            WHERE R5<>''
            UNION
            SELECT MESTO AS [PLACE], R6 FROM MAIN
            WHERE R6<>''
            UNION
            SELECT MESTO AS [PLACE], R7 FROM MAIN
            WHERE R7<>''
            UNION
            SELECT MESTO AS [PLACE], R8 FROM MAIN
            WHERE R8<>''
            UNION
            SELECT MESTO AS [PLACE], R9 FROM MAIN
            WHERE R9<>''
            UNION
            SELECT MESTO AS [PLACE], R10 FROM MAIN
            WHERE R10<>''
            UNION
            SELECT MESTO, R11 FROM MAIN
            WHERE R11<>''
            UNION
            SELECT MESTO AS [PLACE], R12 FROM MAIN
            WHERE R12<>'';";

        public string sqlObjectsCab =
          @"SELECT MESTO AS [PLACE], P1 AS [OBJ] FROM MAIN
            WHERE P1<>''
            UNION
            SELECT MESTO AS [PLACE], P2 FROM MAIN
            WHERE P2<>''
            UNION
            SELECT MESTO AS [PLACE], S1 FROM MAIN
            WHERE S1<>''
            UNION
            SELECT MESTO AS [PLACE], S2 FROM MAIN
            WHERE S2<>''
            UNION
            SELECT MESTO AS [PLACE], S3 FROM MAIN
            WHERE S3<>''
            UNION
            SELECT MESTO AS [PLACE], S4 FROM MAIN
            WHERE S4<>''
            ;";

        /// <summary>
        /// Get the Application Guid
        /// </summary>
        public static Guid AppGuid
        {
            get
            {
                Assembly asm = Assembly.GetEntryAssembly();
                object[] attr = (asm.GetCustomAttributes(typeof(GuidAttribute), true));
                return new Guid((attr[0] as GuidAttribute).Value);
            }
        }
        /// <summary>
        /// Get the current assembly Guid.
        /// <remarks>
        /// Note that the Assembly Guid is not necessarily the same as the
        /// Application Guid - if this code is in a DLL, the Assembly Guid
        /// will be the Guid for the DLL, not the active EXE file.
        /// </remarks>
        /// </summary>
        public static Guid AssemblyGuid
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                object[] attr = (asm.GetCustomAttributes(typeof(GuidAttribute), true));
                return new Guid((attr[0] as GuidAttribute).Value);
            }
        }
        /// <summary>
        /// Get the current user data folder
        /// </summary>
        public static string UserDataFolder
        {
            get
            {
                Guid appGuid = AppGuid;
                string folderBase = Environment.GetFolderPath
                                    (Environment.SpecialFolder.LocalApplicationData);
                string dir = string.Format(@"{0}\{1}\", folderBase, appGuid.ToString("B"));
                Directory.CreateDirectory(dir);
                return dir;
            }
        }
        /// <summary>
        /// Get the current user roaming data folder
        /// </summary>
        public static string UserRoamingDataFolder
        {
            get
            {
                Guid appGuid = AppGuid;
                string folderBase = Environment.GetFolderPath
                                    (Environment.SpecialFolder.ApplicationData);
                string dir = string.Format(@"{0}\{1}\",
                             folderBase, appGuid.ToString("B"));
                Directory.CreateDirectory(dir);
                return dir;
            }
        }
        /// <summary>
        /// Get all users data folder
        /// </summary>
        public static string AllUsersDataFolder
        {
            get
            {
                Guid appGuid = AppGuid;
                string folderBase = Environment.GetFolderPath
                                    (Environment.SpecialFolder.CommonApplicationData);
                string dir = string.Format(@"{0}\{1}\",
                             folderBase, appGuid.ToString("B"));
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

    }
}


