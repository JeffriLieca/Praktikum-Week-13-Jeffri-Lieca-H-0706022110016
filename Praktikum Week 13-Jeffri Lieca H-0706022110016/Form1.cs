using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Praktikum_Week_13_Jeffri_Lieca_H_0706022110016
{
    public partial class Form1 : Form
    {
        public static string sqlConnection = "server=localhost;uid=root;pwd=;database=premier_league";
        public MySqlConnection sqlConnect = new MySqlConnection(sqlConnection);
        public MySqlCommand sqlCommand;
        public MySqlDataAdapter sqlAdapter;

        

        
        public Form1()
        {
            InitializeComponent();
        }

        public static int counter = 0;
        public static int lastData;
        public string Nomor;
        public string ID;
        public static bool Avai;
        public string Team;
        public string Captain;
        public bool Kaptenkah;
        public string TimLama;
        private void Form1_Load(object sender, EventArgs e)
        {
            cbCap.Visible = false;
            labelCap.Visible = false;

            sqlConnect.Open();
            Perbarui();
        }


        public void Perbarui()
        {
            string sqlQuery = "select p.player_id as ID, p.player_name as Name, p.birthdate as Birth, p.nationality_id as NatioID , n.nation as Natio,p.team_id as TeamID, t.team_name as Team, p.team_number as Number  from player p,team t, nationality n where n.nationality_id=p.nationality_id and p.team_id=t.team_id;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            DataTable dtLoad = new DataTable();
            sqlAdapter.Fill(dtLoad);
            tbID.Text = dtLoad.Rows[counter][0].ToString();
            ID = tbID.Text;
            tbName.Text = dtLoad.Rows[counter][1].ToString();
            dtpBirth.Text = dtLoad.Rows[counter][2].ToString();
            lastData = dtLoad.Rows.Count-1;


            sqlQuery = "select n.nationality_id as NatioID, n.nation as Natio from nationality n;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            DataTable dtNatio = new DataTable();
            sqlAdapter.Fill(dtNatio);

            cBNatio.DataSource = dtNatio;
            cBNatio.DisplayMember = "Natio";
            cBNatio.ValueMember = "NatioID";
            cBNatio.Text = dtLoad.Rows[counter][4].ToString();

            sqlQuery = "select t.team_id as TeamID, t.team_name as Team from team t;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            DataTable dtTeam = new DataTable();
            sqlAdapter.Fill(dtTeam);

            cBTeam.DataSource = dtTeam;
            cBTeam.DisplayMember = "Team";
            cBTeam.ValueMember = "TeamID";
            cBTeam.Text = dtLoad.Rows[counter][6].ToString();
            Team = dtLoad.Rows[counter][5].ToString();
            TimLama = dtLoad.Rows[counter][5].ToString();

            numNumber.Value = Convert.ToInt32(dtLoad.Rows[counter][7]);
            Nomor = numNumber.Value.ToString();


            
                sqlQuery = "select t.team_id as IDTeam, t.team_name as NameTeam,p.player_name as Name, p.player_id as ID from team t, player p where p.player_id=t.captain_id and t.team_id='" + Team + "';";
                sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
                sqlAdapter = new MySqlDataAdapter(sqlCommand);
                DataTable dtCaptain = new DataTable();
                sqlAdapter.Fill(dtCaptain);
                cbCap.DataSource = dtCaptain;
                cbCap.DisplayMember = "Name";
                cbCap.ValueMember = "ID";
                
            if (ID == dtCaptain.Rows[0][3].ToString())
            {
                labelCap.Text = "Captain";
                Kaptenkah = true;
            }
            else
            {
                labelCap.Text = "Bukan Captain";
                Kaptenkah = false;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (counter == lastData)
            {
                MessageBox.Show("Sudah Data Terakhir");
            }
            else
            {
                counter++;
                Perbarui();
            }
        }
        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (counter == 0)
            {
                MessageBox.Show("Sudah Data Pertama");
            }
            else
            {
                counter--;
                Perbarui();
            }
        }

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            counter = 0;
            Perbarui();
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            counter = lastData;
            Perbarui();
        }

        private void numNumber_ValueChanged(object sender, EventArgs e)
        {
            string sqlQuery = "select p.player_name as Name,p.team_number as Number from team t,player p where p.team_id=t.team_id and t.team_id='"+cBTeam.SelectedValue.ToString()+"' having p.team_number='"+numNumber.Value+"';";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            DataTable dtNumber = new DataTable();
            sqlAdapter.Fill(dtNumber);
            if(dtNumber.Rows.Count == 0)
            {
                labelAvailable.Text = "Available";
                Avai = true;
            }
            else
            {
                labelAvailable.Text = "Not Available";
                Avai = false;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (Avai == true)
            {
                if (Kaptenkah == true)
                {
                    if (cBTeam.SelectedValue == TimLama)
                    {

                    }
                    else
                    {
                        string sqlUpdate = "update team set captain_id = (select player_id from player where team_id = '"+TimLama+"' order by birthdate limit 1)where team_id = '"+TimLama+"'; ";
                        sqlCommand = new MySqlCommand(sqlUpdate, sqlConnect);
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                else
                {

                }
                try
                {
                    string sqlUpdate = "update player set player_id='" + tbID.Text + "',team_number='" + numNumber.Value.ToString() + "', player_name='" + tbName.Text + "', nationality_id='" + cBNatio.SelectedValue.ToString() + "', birthdate='" + dtpBirth.Value.ToString("yyyyMMdd") + "',team_id='" + cBTeam.SelectedValue.ToString() + "' where player_id='" + ID + "'";
                    sqlCommand = new MySqlCommand(sqlUpdate, sqlConnect);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Data sudah terupdate");
                }
                catch (Exception)
                {
                    MessageBox.Show("Player ID sudah ada");
                }
                

                
            }
            else
            {
                MessageBox.Show("Team Number tidak tersedia");
            }


        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            sqlConnect.Close();
            Application.Exit();
        }

        private void cBTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlQuery = "select p.player_name as Name,p.team_number as Number from team t,player p where p.team_id=t.team_id and t.team_id='" + cBTeam.SelectedValue.ToString() + "' having p.team_number='" + numNumber.Value + "';";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            DataTable dtNumber = new DataTable();
            sqlAdapter.Fill(dtNumber);
            if (dtNumber.Rows.Count == 0)
            {
                labelAvailable.Text = "Available";
                Avai = true;
            }
            else
            {
                labelAvailable.Text = "Not Available";
                Avai = false;
            }
        }
    }
}
