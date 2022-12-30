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
                                ewopinfo.EventDate = reader.GetDateTime(1).ToShortDateString();
                                ewopinfo.EventDyno = reader.GetString(2);
                                ewopinfo.EventInfo1 = reader.GetString(3);
                                ewopinfo.EventInfo2 = reader.GetString(4);
                                ewopinfo.EventInfo3 = reader.GetString(5);
                                ewopinfo.CalDate = DateTime.ParseExact(ewopinfo.EventDate, "MM/dd/yyyy", null).ToString("yyyy-MM-dd");
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
              ewopinfo.EventDyno = Request.Form["dyno"];
            ewopinfo.EventInfo1 = Request.Form["info1"];
            ewopinfo.EventInfo2 = Request.Form["info2"];
            ewopinfo.EventInfo3 = Request.Form["info3"];

            if (ewopinfo.EventDate.Length == 0 || ewopinfo.EventDyno.Length == 0 || ewopinfo.EventInfo1.Length == 0)
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
                        "EventDate=ewopinfo.DbDate, EventDyno=@dyno, EventInfo1=@info1, EventInfo2=@info2, EventInfo3=@info3" +
                        "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@date", ewopinfo.EventDate); 
                        command.Parameters.AddWithValue("@dyno", ewopinfo.EventDyno);
                        command.Parameters.AddWithValue("@info1", ewopinfo.EventInfo1);
                        command.Parameters.AddWithValue("@info2", ewopinfo.EventInfo2);
                        command.Parameters.AddWithValue("@info3", ewopinfo.EventInfo3);
                        

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
