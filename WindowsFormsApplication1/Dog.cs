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
    public partial class Dog : Form
    {
        private Color use;
        public int dogID;
        public int adminID;
        public int adoptID;
        public Home back { get; set; }
        public MySqlConnection conn;
        public Dog()
        {
            InitializeComponent();
            use = Color.FromArgb(253, 208, 174);
            conn = new MySqlConnection("Server=localhost;Database=impounddogpound;Uid=root;Pwd=root;");
        }

        private void AddDog_Load(object sender, EventArgs e)
        {

            this.Top = 278;
        }
        public void trig()
        {
            this.Close();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //panel2.Visible = false;
            addDog.Visible = true;
            searchDog.Visible = false;
            adoptDog.Visible = false;
            euthanizeDog.Visible = false;

            button1.BackColor = use;
            button2.BackColor = Color.Transparent;
            button3.BackColor = Color.Transparent;
            selectButton.Visible = true;
            int i = 9;
            selectButton.Location = new Point(13, i);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //panel2.Visible = false;
            searchDog.Visible = true;
            addDog.Visible = false;
            adoptDog.Visible = false;
            euthanizeDog.Visible = false;

            button2.BackColor = use;
            button1.BackColor = Color.Transparent;
            button3.BackColor = Color.Transparent;
            selectButton.Visible = true;
            int i = 98;
            selectButton.Location = new Point(13, i);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void searchDog_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int operationID;
            int adminID;
            String color = "";
            char gender;
            String size = "";
            String other = "";
            String breed = "";
            if (!cbOperation.Text.Equals("Operation Date and Location") && tbColor.Text != "Color" && cbGender.Text != "Gender" && cbSize.Text != "Size") {

                adminID = back.adminID;
                operationID = cbOperation.SelectedIndex + 1;
                color = tbColor.Text;
                breed = tbBreed.Text;

                if (cbGender.SelectedIndex == 0)
                    gender = 'M';
                else
                    gender = 'F';

                if (cbSize.SelectedIndex == 0)
                    size = "xs";
                else if (cbSize.SelectedIndex == 1)
                    size = "s";
                else if (cbSize.SelectedIndex == 2)
                    size = "l";
                else if (cbSize.SelectedIndex == 3)
                    size = "xl";

                other = tbDesc.Text;

                try
                {
                    conn.Open();
                    MySqlCommand comm = new MySqlCommand("INSERT INTO dogprofile(operationID, adminID, color, gender, size, otherDesc, breed, status) VALUES(" + operationID + ", " + adminID + ", '" + color + "', '" + gender + "', '" + size + "', '" + other + "', '" + breed + "', '" + "unclaimed" +"')", conn);
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Profile Added Successfully");

                    cbGender.Text = "Gender";
                    cbOperation.Text = "Operation Date and Location";
                    cbSize.Text = "Size";
                    tbBreed.Text = "Breed";
                    tbColor.Text = "Color";
                    tbDesc.Text = "Other Description";

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

        private void tbBreed_Enter(object sender, EventArgs e)
        {
            tbBreed.Text = "";
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            tbColor.Text = "";
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            refreshSearch();
        }

        private void dgvProfiles_CellClick(object sender, DataGridViewCellEventArgs e)
            
        {
            dogID = int.Parse(dgvProfiles.Rows[e.RowIndex].Cells["dogID"].Value.ToString());
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (dogID != 0) {
                Claim claim = new Claim();
                claim.dogID = this.dogID;
                claim.adminID = this.adminID;
                claim.dog = this;
                claim.Show();
            }
            else
            {
                MessageBox.Show("Please select a dog");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Adopt adopt = new Adopt();
            adopt.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //panel2.Visible = false;
            searchDog.Visible = true;
            addDog.Visible = false;

            button2.BackColor = use;
            button1.BackColor = Color.Transparent;
            button3.BackColor = Color.Transparent;
            selectButton.Visible = true;
            int i = 98;
            selectButton.Location = new Point(13, i);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            
            searchDog.Visible = false;
            addDog.Visible = false;
            adoptDog.Visible = true;
            euthanizeDog.Visible = false;

            button2.BackColor = use;
            button1.BackColor = Color.Transparent;
            button3.BackColor = Color.Transparent;
            selectButton.Visible = true;
            
            selectButton.Location = new Point(13, 194);

            refreshAdoption();
        }

        public void refreshSearch()
        {
            if (tbColorSearch.Text != "" || tbBreedSearch.Text != "" || cbGenderSearch.SelectedIndex != 0)
            {
                Char gender = ' ';

                if (cbGenderSearch.SelectedIndex == 1)
                    gender = 'M';
                else if (cbGenderSearch.SelectedIndex == 2)
                    gender = 'F';


                try
                {
                    conn.Open();
                    MySqlCommand comm = new MySqlCommand("SELECT dogID, breed, date, location, size, otherDesc FROM (dogprofile INNER JOIN dogoperation ON dogprofile.operationID = dogoperation.operationID) INNER JOIN location ON location.locationID = dogoperation.locationID WHERE gender = '" + gender + "' AND breed LIKE '%" + tbBreedSearch.Text + "%' AND color LIKE '%" + tbColorSearch.Text + "%' AND status = 'unclaimed'", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(comm);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    dgvProfiles.DataSource = dt;

                    dgvProfiles.Columns["dogID"].Visible = false;
                    dgvProfiles.Columns["breed"].HeaderText = "Breed";
                    dgvProfiles.Columns["date"].HeaderText = "Date Caught";
                    dgvProfiles.Columns["location"].HeaderText = "Location Caught";
                    dgvProfiles.Columns["otherDesc"].HeaderText = "Other Description";
                    dgvProfiles.Columns["breed"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvProfiles.Columns["date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvProfiles.Columns["location"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvProfiles.Columns["otherDesc"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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

        public void refreshAdoption()
        {
            try
            {
                conn.Open();
                MySqlCommand comm = new MySqlCommand("SELECT dogID, breed, color, gender, size FROM dogprofile INNER JOIN dogoperation ON dogoperation.operationID = dogprofile.operationID WHERE status = 'unclaimed' AND date > DATE_ADD(NOW(), INTERVAL -3 DAY)", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(comm);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                dgvAdoption.DataSource = dt;

                dgvAdoption.Columns["dogID"].Visible = false;
                dgvAdoption.Columns["breed"].HeaderText = "Breed";
                dgvAdoption.Columns["breed"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvAdoption.Columns["color"].HeaderText = "Color";
                dgvAdoption.Columns["color"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvAdoption.Columns["gender"].HeaderText = "Gender";
                dgvAdoption.Columns["gender"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvAdoption.Columns["size"].HeaderText = "Size";
                dgvAdoption.Columns["size"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conn.Close();

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (adoptID != 0) {
                Adopt adopt = new Adopt();
                adopt.dogID = this.adoptID;
                adopt.adminID = this.adminID;
                adopt.dog = this;
                adopt.Show();
            }
            else
            {
                MessageBox.Show("Please select a dog");
            }
        }

        private void dgvAdoption_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            adoptID = int.Parse(dgvAdoption.Rows[e.RowIndex].Cells["dogID"].Value.ToString()); 
        }

        private void adoptDog_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tbDesc_Enter(object sender, EventArgs e)
        {
            tbDesc.Text = "";
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            //panel2.Visible = false;
            searchDog.Visible = false;
            addDog.Visible = false;
            adoptDog.Visible = false;
            euthanizeDog.Visible = true;

            button2.BackColor = Color.Transparent;
            button1.BackColor = Color.Transparent;
            button3.BackColor = Color.Transparent;
            button4.BackColor = use;
            selectButton.Visible = true;
            int i = 280;
            selectButton.Location = new Point(13, i);

            refreshArchive();
            
        }

        public void refreshArchive()
        {
            try
            {
                conn.Open();
                MySqlCommand comm = new MySqlCommand("SELECT dogID, color, gender, size, breed, otherDesc FROM dogprofile INNER JOIN dogoperation ON dogoperation.operationID = dogprofile.operationID WHERE date <= DATE_ADD(NOW(), INTERVAL -3 DAY) AND status = 'unclaimed'", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(comm);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                dgvArchive.DataSource = dt;
                
                dgvArchive.Columns["dogID"].Visible = false;
                dgvArchive.Columns["color"].HeaderText = "Color";
                dgvArchive.Columns["color"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvArchive.Columns["gender"].HeaderText = "Gender";
                dgvArchive.Columns["gender"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvArchive.Columns["size"].HeaderText = "Size";
                dgvArchive.Columns["size"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvArchive.Columns["breed"].HeaderText = "Breed";
                dgvArchive.Columns["breed"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvArchive.Columns["otherDesc"].HeaderText = "Other Description";
                dgvArchive.Columns["otherDesc"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conn.Close();
            }


        }

        private void euthanizeDog_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                
                MySqlCommand comm = new MySqlCommand("UPDATE dogprofile SET status = 'euthanized' WHERE dogID = " + dogID, conn);
                comm.ExecuteNonQuery();
                

                conn.Close();
                refreshArchive();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conn.Close();
            }
        }

        private void dgvArchive_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dogID = int.Parse(dgvArchive.Rows[e.RowIndex].Cells["dogID"].Value.ToString());
        }
    }
}
