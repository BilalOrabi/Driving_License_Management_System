using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private void _FillPersonObjectWithFieldsData()
        {
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();

            _Person.Gendor = (rbMale.Checked) ? (byte)clsPeople.enGendor.Male : (byte)clsPeople.enGendor.Female;

            _Person.DateOfBirth = dtpDateOfBirth.Value;

            _Person.NationalityCountryID = clsCountry.Find(cbCountry.Text).ID;

            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;
            else
                _Person.ImagePath = "";
        }
        private void _LoadData()
        {
            _Person = clsPeople.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.Close();
                return;
            }

            _FillFieldsWithPersonInfo();

            //load person image in case it was set.
            if (_Person.ImagePath != "")
                pbPersonImage.ImageLocation = _Person.ImagePath;

            //hide/show the remove link in case there is no image for the person
            llRemoveImage.Visible = (_Person.ImagePath != "");
        }

        private bool _HandlePersonImage()
        {
            // this procedure will handle the person image,
            // it will take care of deleting the old image from the folder
            // in case the image changed, and it will rename the new image with guid and 
            // place it in the images folder.

            // _Person.ImagePath contains the old Image, we check if it changed then we copy the new image
            if (_Person.ImagePath != pbPersonImage.ImageLocation)
            {

                if (_Person.ImagePath != "")
                {
                    // first we delete the old image from the folder in case there is any.
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {
                        // We could not delete the file.
                        // log it later   
                    }
                }

                if (pbPersonImage.ImageLocation != null)
                {
                    // then we copy the new image to the image folder after we rename it
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();

                    //if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    //{
                    //    pbPersonImage.ImageLocation = SourceImageFile;

                    //    return true;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Error Copying Image File", "Error",
                    //        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return false;
                    //}
                }
            }

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
