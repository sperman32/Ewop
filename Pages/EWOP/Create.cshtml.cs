using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Tracing;
using System.Linq.Expressions;
using System.Data.SqlClient;

namespace Ewop2.Pages.EWOP
{
    public class CreateModel : PageModel
    {
        public EwopInfo ewopinfo = new EwopInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost() 
        {
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

            //save the new client into the dateabase
            try
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EWOP2;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Schedule " +
                        "(EventDyno, EventInfo1, EventInfo2, EventInfo3, EventYear, EventMonth, EventDay) VALUES " +
                        "(@dyno, @info1, @info2, @info3, @eventYear, @eventMonth, @eventDay);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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
                errorMessage = ex.Message;
                return;
            }



            ewopinfo.CalDate = ""; ewopinfo.EventDyno = ""; ewopinfo.EventInfo1 = ""; ewopinfo.EventInfo2 = ""; ewopinfo.EventInfo3 = ""; ewopinfo.EventYear = 1990; ewopinfo.EventMonth = 1; ewopinfo.EventDay = 1;
            successMessage = "New Event Added Successfully";

            Response.Redirect("/EWOP/Events");
        }



    }
}
