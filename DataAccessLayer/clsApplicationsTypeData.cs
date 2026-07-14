using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsApplicationsTypeData
    {
        public static DataTable GetAllApplicationsTypes()
        {
            string query = "select * from ApplicationTypes";
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString)) 
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            // logging error logic should be here
                        }
                    }
                }

            }
            return dt;

        }

        public static bool UpdateApplications(int ApplicationTypeID, string ApplicationTypesTitle, float Fees)
        {
            string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@Title", ApplicationTypesTitle);
                    command.Parameters.AddWithValue("@Fees", Fees);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // logging error logic should be here
                    }
                    finally { connection.Close(); }

                }
            }
            return (rowsAffected > 0);
        }
    
        public static bool GetApplicationTypeByID(int ApplicationTypeID ,ref string Title , ref float Fees)
        {
            string query = @"select * from ApplicationTypes where ApplicationTypeID = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Title = reader["ApplicationTypeTitle"].ToString();
                                Fees = Convert.ToInt32(reader["ApplicationFees"]);
                                return true;
                            }
                        }
                         
                    }
                    catch (Exception ex)
                    {
                        // logging error logic should be here
                    }

                }
            }
            // if we reach here it means error has occurde
            return false;
        }

        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ApplicationTypeID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)
                            
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
            command.Parameters.AddWithValue("@ApplicationFees", Fees);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationTypeID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return ApplicationTypeID;

        }
    }
}
