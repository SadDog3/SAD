﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    public partial class Claim : Form
    {
        public int dogID;
        public int adminID;
        public MySqlConnection conn;
        public Dog dog { get; set; }
        public Claim()
        {
            InitializeComponent();
            conn = new MySqlConnection("Server=localhost;Database=impounddogpound;Uid=root;Pwd=root;");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            dog.Show();
            this.Close();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbname.Text != "" && tbadd.Text != "" && tbIDnum.Text != "" && tbIDtype.Text != "")
            {
                String name = tbname.Text;
                String add = tbadd.Text;
                String idnum = tbIDnum.Text;
                String idtype = tbIDtype.Text;
                String num = tbnumber.Text;
                String date = DateTime.Now.ToString("yyyy-mm-dd");
                int vaccine = 0;
                if (checkbox.Checked)
                {
                    vaccine = 1;
                }
                try
                {
                    conn.Open();
                    MySqlCommand comm = new MySqlCommand("INSERT INTO client(name, contactNumber, validIDtype, validIDnumber, address) VALUES('" + name + "', '" + num + "', '" + idtype + "', '" + idnum + "', '" + add + "')", conn);
                    comm.ExecuteNonQuery();

                    comm = new MySqlCommand("SELECT clientID FROM client WHERE validIDType = '" + idtype + "' AND validIDNumber = '" + idnum + "'", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(comm);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    int clientID = int.Parse(dt.Rows[0]["clientID"].ToString());
                   
                    comm = new MySqlCommand("INSERT INTO dogtransaction(clientID, dogID, date, payment, vaccine, type) VALUES(" + clientID + ", " + dogID + ", '" + date + "', " + "250" + ", " + vaccine + ", '" + "claim" + "')", conn);
                    comm.ExecuteNonQuery();

                    comm = new MySqlCommand("UPDATE dogprofile SET status = 'claimed' WHERE dogID = " + dogID, conn);
                    comm.ExecuteNonQuery();

                    MessageBox.Show("Successfully CLaimed!");
                    dog.Show();
                    this.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Please enter required fields");
            }
        }

        private void Claim_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                MySqlCommand comm = new MySqlCommand("SELECT breed, color, size, gender, location, date FROM (dogoperation INNER JOIN dogprofile ON dogprofile.operationID = dogoperation.operationID) INNER JOIN location on dogoperation.locationID = location.locationID WHERE dogID = " + dogID, conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(comm);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                breed.Text = dt.Rows[0]["breed"].ToString();
                color.Text = dt.Rows[0]["color"].ToString();
                size.Text = dt.Rows[0]["size"].ToString();
                gender.Text = dt.Rows[0]["gender"].ToString();
                location.Text = dt.Rows[0]["location"].ToString();
                date.Text = dt.Rows[0]["date"].ToString();


                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conn.Close();
            }
        }

        private void Claim_FormClosing(object sender, FormClosingEventArgs e)
        {
            dog.refreshSearch();
            dog.tbBreedSearch.Text = "";
            dog.tbColorSearch.Text = "";
            dog.cbGenderSearch.SelectedIndex = 0;
            
        }
    }
}
