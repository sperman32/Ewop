using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Ewop2.Pages.EWOP
{
    public class EventsModel : PageModel
    {
        public List<EwopInfo> EventList0 = new List<EwopInfo>();
        public List<EwopInfo> EventList1 = new List<EwopInfo>();
        public List<EwopInfo> EventList2 = new List<EwopInfo>();
        public List<EwopInfo> EventList3 = new List<EwopInfo>();
        public List<EwopInfo> EventList4 = new List<EwopInfo>();
        public List<EwopInfo> EventList5 = new List<EwopInfo>();
        public List<EwopInfo> EventList6 = new List<EwopInfo>();
        public List<EwopInfo> EventList7 = new List<EwopInfo>();
        public int NowYear;
        public int NowMonth;
        MoYr[] Calendars = new MoYr[6];
        public DateTime DayOne;
        public string? DateString;
        public string? CalString;
        public int DayOffset;

        public CalMonth[] Cal0 = new CalMonth[42];

        public void OnGet()
        {
            
            //reading current date and setting month/year for 6 calendars
            Calendars[0].Yr = DateTime.Now.Year;
            Calendars[0].Mo = DateTime.Now.Month;
            Calendars[0].Ds = DateTime.DaysInMonth(Calendars[0].Yr, Calendars[0].Mo);

            DateString = Convert.ToString(Calendars[0].Mo).PadLeft(2, '0') + "/01/" + Convert.ToString(Calendars[0].Yr) ;
            DayOne = DateTime.Parse(DateString);
            Calendars[0].Dy1 = Convert.ToInt32(DayOne.DayOfWeek);

            for (int CalLoop =1; CalLoop<6; CalLoop++)
            {
                Calendars[CalLoop].Mo = Calendars[CalLoop - 1].Mo + 1;
                Calendars[CalLoop].Yr = Calendars[CalLoop - 1].Yr;
                if (Calendars[CalLoop].Mo >12)
                {
                    Calendars[CalLoop].Mo = 1;
                    Calendars[CalLoop].Yr++;
                }//if (Calendars[CalLoop].Mo >12)
                Calendars[CalLoop].Ds = DateTime.DaysInMonth(Calendars[CalLoop].Yr, Calendars[CalLoop].Mo);

                DateString = Convert.ToString(Calendars[CalLoop].Mo).PadLeft(2, '0') + "/01/" + Convert.ToString(Calendars[CalLoop].Yr);
                DayOne = DateTime.Parse(DateString);
                Calendars[CalLoop].Dy1 = Convert.ToInt32(DayOne.DayOfWeek);

                

            }//for (int CalLoop =1; CalLoop<6,CalLoop++)


            try
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EWOP2;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Schedule";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                EwopInfo ewopinfo = new EwopInfo();
                                ewopinfo.EventId = "" + reader.GetInt32(0);
                                ewopinfo.EventDyno = reader.GetString(1);
                                ewopinfo.EventInfo1 = reader.GetString(2);
                                ewopinfo.EventInfo2 = reader.GetString(3);
                                ewopinfo.EventInfo3 = reader.GetString(4);
                                ewopinfo.EventYear = reader.GetInt32(5);
                                ewopinfo.EventMonth = reader.GetInt32(6);
                                ewopinfo.EventDay  = reader.GetInt32(7);
                                ewopinfo.CalDate = (Convert.ToString(ewopinfo.EventYear) + "-" + Convert.ToString(ewopinfo.EventMonth).PadLeft(2,'0') + "-" + Convert.ToString(ewopinfo.EventDay).PadLeft(2, '0'));
                                
                                
                                //Is there a better way to do this?  Switch case loop?

                                if (ewopinfo.EventYear < Calendars[0].Yr) //Previous year, move to EL7
                                {
                                    EventList7.Add(ewopinfo);
                                }

                                if (ewopinfo.EventYear == Calendars[0].Yr)
                                {
                                    if (ewopinfo.EventMonth < Calendars[0].Mo) //in the past, move to EL7
                                    {
                                        EventList7.Add(ewopinfo);
                                    }

                                    if (ewopinfo.EventMonth == Calendars[0].Mo) //This month move to EL0
                                    {
                                        EventList0.Add(ewopinfo);
                                    }

                                }
                                if (ewopinfo.EventYear == Calendars[1].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[1].Mo) //This next month, move to EL1
                                    {
                                        EventList1.Add(ewopinfo);
                                    }

                                }
                                if (ewopinfo.EventYear == Calendars[2].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[2].Mo) //EL2
                                    {
                                        EventList2.Add(ewopinfo);
                                    }

                                }
                                if (ewopinfo.EventYear == Calendars[3].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[3].Mo) //EL3
                                    {
                                        EventList3.Add(ewopinfo);
                                    }

                                }

                                if (ewopinfo.EventYear == Calendars[4].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[4].Mo) //El4
                                    {
                                        EventList4.Add(ewopinfo);
                                    }

                                }
                                if (ewopinfo.EventYear == Calendars[5].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[5].Mo) //El5
                                    {
                                        EventList5.Add(ewopinfo);
                                    }

                                    if (ewopinfo.EventMonth > Calendars[5].Mo) // future El3
                                    {
                                        EventList6.Add(ewopinfo);
                                    }

                                }

                                if (ewopinfo.EventYear > Calendars[5].Yr) //Future. el3
                                {
                                    EventList6.Add(ewopinfo);
                                }

                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

            //clear existing calendar values
            for (int DayLoop = 0; DayLoop < 42; DayLoop++)
            {
                Cal0[DayLoop].CalDay = "";
                Cal0[DayLoop].CalDyno = "";
                Cal0[DayLoop].CalInfo1 = "";
                Cal0[DayLoop].CalInfo2 = "";
                Cal0[DayLoop].CalInfo3 = "";

            }


            //Set day values
            for (int DayLoop = Calendars[0].Dy1; DayLoop < (Calendars[0].Dy1 + Calendars[0].Ds); DayLoop++)
            {
                CalString = Convert.ToString(DayLoop - Calendars[0].Dy1+1);
                Cal0[DayLoop].CalDay = CalString;
            }

            //Add Events

            foreach (var Evnt in EventList0)
            {
                                
                DayOffset=Evnt.EventDay + Calendars[0].Dy1 - 1;
                Cal0[DayOffset].CalDyno = Evnt.EventDyno;
                Cal0[DayOffset].CalInfo1 = Evnt.EventInfo1;
                Cal0[DayOffset].CalInfo2 = Evnt.EventInfo2;
                Cal0[DayOffset].CalInfo3 = Evnt.EventInfo3;

            }







            //write eventlist to database to share with other pages

            
            //First, empty table of existing values
            try
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EWOP2;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "TRUNCATE TABLE Cal0;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception ex)
            {
                
            }




            //write new values into table
            try
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EWOP2;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    for (int DayLoop = 0; DayLoop < 42; DayLoop++)
                    {
                        {                        
                        String sql = "INSERT INTO Cal0 " +
                            "(EvDay, EvDyno, EvInfo1, EvInfo2, EvInfo3) VALUES " +
                            "(@CalDay, @CalDyno, @CalInfo1, @CalInfo2, @CalInfo3);";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                            
                                    command.Parameters.AddWithValue("@CalDay", Cal0[DayLoop].CalDay);
                                    command.Parameters.AddWithValue("@CalDyno", Cal0[DayLoop].CalDyno);
                                    command.Parameters.AddWithValue("@CalInfo1", Cal0[DayLoop].CalInfo1);
                                    command.Parameters.AddWithValue("@CalInfo2", Cal0[DayLoop].CalInfo2);
                                    command.Parameters.AddWithValue("@CalInfo3", Cal0[DayLoop].CalInfo3);

                                    command.ExecuteNonQuery();
                            }
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                
            }









        } //public void OnGet()
    }
    public class EwopInfo
    {
        public string? EventId;
        public string? EventDyno;
        public string? EventInfo1;
        public string? EventInfo2;
        public string? EventInfo3;
        public int EventYear;
        public int EventMonth;
        public int EventDay;
        public string? CalDate;
 
    }
    public struct MoYr
    {
        public int Mo;  //Month
        public int Yr;  //Year
        public int Dy1; //day of week for 1st
        public int Ds;  //days in month
    }

    public struct CalMonth
    {
        public string? CalId;
        public string? CalDay;
        public string? CalDyno;
        public string? CalInfo1;
        public string? CalInfo2;
        public string? CalInfo3;
    }
}

