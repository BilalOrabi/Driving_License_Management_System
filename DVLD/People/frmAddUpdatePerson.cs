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
using DVLD.Properties;

namespace DVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(int person_id);
        public event DataBackEventHandler DataBack;
        public enum enMode {AddNew = 1 ,Update = 2 }
        private enMode _Mode = enMode.AddNew;
        private int _PersonID = -1; 
        private clsPeople _Person;
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int person_id)
        {
            InitializeComponent();
            this._PersonID = person_id;
            _Mode = enMode.Update;
        }
        private void _FillCountryInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow drCountry in dtCountries.Rows)
            {
                cbCountry.Items.Add(drCountry["CountryName"]);
            }
        }
        private void _ResetFields()
        {
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
        }
        private void _ResetDefaultValues()
        {
            //this will initialize the reset the default values

            _FillCountryInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                this.Text = "Add New Person";
                _Person = new clsPeople();
            }
            else
            {
                lblTitle.Text = "Update Person";
                this.Text = "Update Person";
            }

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            //hide/show the remove link in case there is no image for the person
            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);

            //we set the max date to 18 years from today
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);

            //should not allow adding age more than 100 years
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            //this will set default country to Jordan
            cbCountry.SelectedIndex = cbCountry.FindString("Jordan");

            _ResetFields();
        }

        private void _FillFieldsWithPersonInfo()
        {
            lblPersonID.Text = _Person.PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.Gendor == (byte)clsPeople.enGendor.Male)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            // To show the name of the country
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
