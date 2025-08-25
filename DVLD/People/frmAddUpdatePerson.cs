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

namespace DVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(int person_id);
        public event DataBackEventHandler DataBack;
        public enum enMode {AddNew = 1 ,Update = 2 }
        private enMode _Mode;
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


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
