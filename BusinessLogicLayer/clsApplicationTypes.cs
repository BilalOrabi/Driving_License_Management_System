using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class clsApplicationTypes
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ApplicationID { get; set; }
        public string ApplicationTitle { get; set; }
        public float Fees { get; set; }

        public clsApplicationTypes()
        {
            this.ApplicationID = -1;
            this.ApplicationTitle = string.Empty;
            this.Fees = 0;
            Mode = enMode.AddNew;
        }

        private clsApplicationTypes(int ID,string Title,float Fees)
        {
            this.ApplicationID = ID;
            this.ApplicationTitle = Title;
            this.Fees = Fees;
            Mode = enMode.Update;
        }

        public static DataTable GetAllApplicationTypes() => clsApplicationsTypeData.GetAllApplicationsTypes();

        private bool _AddNewApplicationType()
        {
            // Call Data Access Layer
            this.ApplicationID = clsApplicationsTypeData.AddNewApplicationType(this.ApplicationTitle, this.Fees);

            return (ApplicationID != -1);
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationsTypeData.UpdateApplications(this.ApplicationID, this.ApplicationTitle, this.Fees);
        }

        public static clsApplicationTypes Find(int ID)
        {
            string Title = ""; 
            float Fees = 0;

            if (clsApplicationsTypeData.GetApplicationTypeByID(ID, ref Title, ref Fees))

                return new clsApplicationTypes(ID, Title, Fees);

            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicationType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;


                case enMode.Update:
                    return _UpdateApplicationType();

            }

            return false;
        }
    }
}
