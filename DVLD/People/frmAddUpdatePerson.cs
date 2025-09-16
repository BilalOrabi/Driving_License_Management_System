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
using DVLD.Global_Classes;
using DVLD.Properties;

namespace DVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int person_id);

        // Declare an event using the delegate
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
        private void _SaveData()
        {
            _FillPersonObjectWithFieldsData();

            if (_Person.Save())
            {
                lblTitle.Text = "Update Person";
                this.Text = "Update Person";
                lblPersonID.Text = _Person.PersonID.ToString();

                // change form mode to Update
                _Mode = enMode.Update;

                MessageBox.Show("Data Saved Successfully", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send back data to the caller form

                DataBack?.Invoke(this,_Person.PersonID);
            }
            else 
            {
                MessageBox.Show("Data Saved Failed", "Failed",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;

                       return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if(_Mode == enMode.Update)
                _LoadData();
        }

       
        private void rbMale_Click(object sender, EventArgs e)
        {
            //change the defualt image to Male incase there is no image set.
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Male_512;
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            //change the defualt image to Female incase there is no image set.
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This filed is required!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }

        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            // no need for validtate incase it's empty
            if (txtEmail.Text.Trim() == "")
                return;

            if (!clsValidation.ValidateEmail(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required");
                return;
            }
            else 
            {
                errorProvider1.SetError (txtNationalNo, null);
            }

            //Make sure the national number is not used by another person
            if (txtNationalNo.Text != _Person.NationalNo && clsPeople.isPersonExist(txtNationalNo.Text.Trim()))
            {
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            llRemoveImage.Visible = false;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            // ``this.ValidateChildren()`` it triggers the (Validating event) for each child control that supports validation.

            if (!this.ValidateChildren())
            {
                //Here we don't continue because the form is not valid
                MessageBox.Show("Some fileds are not valid!,put the mouse over the red icon to see the error");
                return;
            }

            if (!_HandlePersonImage())
                return;

            _SaveData();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
