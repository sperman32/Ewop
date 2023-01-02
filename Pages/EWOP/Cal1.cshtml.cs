using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Ewop2.Pages.EWOP
{
    public class Cal1Model : PageModel
    {
        public List<CalMonth> CalMonth0 = new List<CalMonth>();

        public void OnGet()
        {

            try  //Read event info for calendar from database
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EWOP2;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Cal0";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                CalMonth calmonth = new CalMonth();
                                calmonth.CalId = "" + reader.GetInt32(0);
                                calmonth.CalDay = reader.GetString(1);
                                calmonth.CalDyno = reader.GetString(2);
                                calmonth.CalInfo1 = reader.GetString(3);
                                calmonth.CalInfo2 = reader.GetString(4);
                                calmonth.CalInfo3 = reader.GetString(5);
                               
                                CalMonth0.Add(calmonth);
                                       
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
