using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class clsPeople
    {
        public int PersonID { set; get; }
        public string NationalNo { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public int NationalityCountryID { set; get; }
        public DateTime DateOfBirth { set; get; }
        public string ImagePath { set; get; }

        public static DataTable GetAllPeople()
        {
            return clsPeopleData.GetAllPeople();
        }
    }
}
