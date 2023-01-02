using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Ewop2.Pages.EWOP
{
    public class EditModel : PageModel
    {
        public EwopInfo ewopinfo = new EwopInfo();
        public String errorMessage = "";
        public String successMessage = "";


        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EWOP2;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Schedule WHERE Eventid=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                ewopinfo.EventId = "" + reader.GetInt32(0);
                                ewopinfo.EventDyno = reader.GetString(1);
                                ewopinfo.EventInfo1 = reader.GetString(2);
                                ewopinfo.EventInfo2 = reader.GetString(3);
                                ewopinfo.EventInfo3 = reader.GetString(4);
                                ewopinfo.EventYear = reader.GetInt32(5);
                                ewopinfo.EventMonth = reader.GetInt32(6);
                                ewopinfo.EventDay = reader.GetInt32(7);
                                ewopinfo.CalDate = (Convert.ToString(ewopinfo.EventYear) + "-" + Convert.ToString(ewopinfo.EventMonth).PadLeft(2, '0') + "-" + Convert.ToString(ewopinfo.EventDay).PadLeft(2, '0'));
                                ewopinfo.EventYear = Convert.ToInt32(ewopinfo.CalDate.Substring(0, 4));
                                ewopinfo.EventMonth = Convert.ToInt32(ewopinfo.CalDate.Substring(5, 2));
                                ewopinfo.EventDay = Convert.ToInt32(ewopinfo.CalDate.Substring(8, 2));

                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost() 
        {

            ewopinfo.EventId = Request.Form["id"];
            ewopinfo.CalDate = Request.Form["date"];
            ewopinfo.EventDyno = Request.Form["dyno"];
            ewopinfo.EventInfo1 = Request.Form["info1"];
            ewopinfo.EventInfo2 = Request.Form["info2"];
            ewopinfo.EventInfo3 = Request.Form["info3"];

            ewopinfo.EventYear = Convert.ToInt32(ewopinfo.CalDate.Substring(0, 4));
            ewopinfo.EventMonth = Convert.ToInt32(ewopinfo.CalDate.Substring(5, 2));
            ewopinfo.EventDay = Convert.ToInt32(ewopinfo.CalDate.Substring(8, 2));

            if (ewopinfo.CalDate.Length == 0 || ewopinfo.EventDyno.Length == 0 || ewopinfo.EventInfo1.Length == 0)
            {
                errorMessage = "Date, SN and Info 1 required";
                return;
            }
            try
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EWOP2;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Schedule " +
                        "SET EventDyno=@dyno, EventInfo1=@info1, EventInfo2=@info2, EventInfo3=@info3, EventYear=@eventYear, EventMonth=@eventMonth, EventDay=@eventDay " +
                        "WHERE EventId=@id";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", ewopinfo.EventId); 
                        command.Parameters.AddWithValue("@dyno", ewopinfo.EventDyno);
                        command.Parameters.AddWithValue("@info1", ewopinfo.EventInfo1);
                        command.Parameters.AddWithValue("@info2", ewopinfo.EventInfo2);
                        command.Parameters.AddWithValue("@info3", ewopinfo.EventInfo3);
                        command.Parameters.AddWithValue("@eventYear", ewopinfo.EventYear);
                        command.Parameters.AddWithValue("@eventMonth", ewopinfo.EventMonth);
                        command.Parameters.AddWithValue("@eventDay", ewopinfo.EventDay);
                        

                        command.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception ex)

            {
                errorMessage= ex.Message; 
                return;

            }

            Response.Redirect("/EWOP/Events");
        }

    }
}
