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
        public frmPeople()
        {
            InitializeComponent();
        }
        private void _RefreshPeopleList()
        {
            _AllPeople = clsPeople.GetAllPeople();
            dgvALLPeople.DataSource = _AllPeople;
            lblRecordNumber.Text = dgvALLPeople.RowCount.ToString();
        }

        private void frmPeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();
            cbFilterBy.SelectedIndex = 0;
            lblRecordNumber.Text = dgvALLPeople.RowCount.ToString();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
