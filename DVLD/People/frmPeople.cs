using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogicLayer;
using DVLD.People;

namespace DVLD
{
    public partial class frmPeople : Form
    {
        private static DataTable _AllPeople = clsPeople.GetAllPeople();

        //only select the columns that you want to show in the grid
        private DataTable _dtPeople = _AllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
        public frmPeople()
        {
            InitializeComponent();
        }
        private void _RefreshPeopleList()
        {
            _AllPeople = clsPeople.GetAllPeople();
            _dtPeople = _AllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
            dgvALLPeople.DataSource = _dtPeople;
            lblRecordNumber.Text = dgvALLPeople.RowCount.ToString();
        }

        private void frmPeople_Load(object sender, EventArgs e)
        {

            dgvALLPeople.DataSource = _dtPeople;

            if (dgvALLPeople.Rows.Count > 0)
            {

                dgvALLPeople.Columns[0].HeaderText = "Person ID";
                dgvALLPeople.Columns[0].Width = 110;

                dgvALLPeople.Columns[1].HeaderText = "National No.";
                dgvALLPeople.Columns[1].Width = 120;


                dgvALLPeople.Columns[2].HeaderText = "First Name";
                dgvALLPeople.Columns[2].Width = 120;

                dgvALLPeople.Columns[3].HeaderText = "Second Name";
                dgvALLPeople.Columns[3].Width = 140;


                dgvALLPeople.Columns[4].HeaderText = "Third Name";
                dgvALLPeople.Columns[4].Width = 120;

                dgvALLPeople.Columns[5].HeaderText = "Last Name";
                dgvALLPeople.Columns[5].Width = 120;

                dgvALLPeople.Columns[6].HeaderText = "Gendor";
                dgvALLPeople.Columns[6].Width = 120;

                dgvALLPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvALLPeople.Columns[7].Width = 140;

                dgvALLPeople.Columns[8].HeaderText = "Nationality";
                dgvALLPeople.Columns[8].Width = 120;


                dgvALLPeople.Columns[9].HeaderText = "Phone";
                dgvALLPeople.Columns[9].Width = 120;


                dgvALLPeople.Columns[10].HeaderText = "Email";
                dgvALLPeople.Columns[10].Width = 170;
            }
            cbFilterBy.SelectedIndex = 0;
            lblRecordNumber.Text = dgvALLPeople.RowCount.ToString();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            _RefreshPeopleList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "GendorCaption";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || cbFilterBy.Text == "None")
            {
                _AllPeople.DefaultView.RowFilter = "";
                lblRecordNumber.Text = dgvALLPeople.RowCount.ToString();
                return;
            }

            if (FilterColumn == "PersonID")
            {
                if (int.TryParse(txtFilterValue.Text.Trim(), out int id))
                    _AllPeople.DefaultView.RowFilter = $"[{FilterColumn}] = {id}";
                else
                    _AllPeople.DefaultView.RowFilter = "";

            }
            else
                _AllPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordNumber.Text = dgvALLPeople.RowCount.ToString();

        }

        

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvALLPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            _RefreshPeopleList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson((int)dgvALLPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshPeopleList();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvALLPeople.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {

                //Perform Delele and refresh
                if (clsPeople.DeletePerson((int)dgvALLPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeopleList();
                }

                else
                    MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

       
    }
}
