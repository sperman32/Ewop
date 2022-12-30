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
        public int NowYear;
        public int NowMonth;
        MoYr[] Calendars = new MoYr[6];
        public int Cal0Mo { get; set; }



        public void OnGet()
        {
            
            //reading current date and setting month/year for 6 calendars
            Calendars[0].Yr = DateTime.Now.Year;
            Calendars[0].Mo = DateTime.Now.Month;
            Cal0Mo = DateTime.Now.Month;


            for (int CalLoop =1; CalLoop<6; CalLoop++)
            {
                Calendars[CalLoop].Mo = Calendars[CalLoop - 1].Mo + 1;
                Calendars[CalLoop].Yr = Calendars[CalLoop - 1].Yr;
                if (Calendars[CalLoop].Mo >12)
                {
                    Calendars[CalLoop].Mo = 1;
                    Calendars[CalLoop].Yr++;
                }//if (Calendars[CalLoop].Mo >12)

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
                                ewopinfo.CalDate = (Convert.ToString(ewopinfo.EventYear) + "-" + Convert.ToString(ewopinfo.EventMonth) + "-" + Convert.ToString(ewopinfo.EventDay));

                                //Is there a better way to do this?  Switch case loop?

                                if (ewopinfo.EventYear < Calendars[0].Yr) //Previous year, move to EL4
                                {
                                    EventList4.Add(ewopinfo);
                                }

                                if (ewopinfo.EventYear == Calendars[0].Yr)
                                {
                                    if (ewopinfo.EventMonth < Calendars[0].Mo) //in the past, move to EL4
                                    {
                                        EventList4.Add(ewopinfo);
                                    }

                                    if (ewopinfo.EventMonth == Calendars[0].Mo) //This month or next, move to EL0
                                    {
                                        EventList0.Add(ewopinfo);
                                    }

                                }
                                if (ewopinfo.EventYear == Calendars[1].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[1].Mo) //This month or next, move to EL0
                                    {
                                        EventList0.Add(ewopinfo);
                                    }

                                }
                                if (ewopinfo.EventYear == Calendars[2].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[2].Mo) //EL1
                                    {
                                        EventList1.Add(ewopinfo);
                                    }

                                }
                                if (ewopinfo.EventYear == Calendars[3].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[3].Mo) //EL1
                                    {
                                        EventList1.Add(ewopinfo);
                                    }

                                }

                                if (ewopinfo.EventYear == Calendars[4].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[4].Mo) //El2
                                    {
                                        EventList2.Add(ewopinfo);
                                    }

                                }
                                if (ewopinfo.EventYear == Calendars[5].Yr)
                                {
                                    if (ewopinfo.EventMonth == Calendars[5].Mo) //El2
                                    {
                                        EventList2.Add(ewopinfo);
                                    }

                                    if (ewopinfo.EventMonth > Calendars[5].Mo) // future El3
                                    {
                                        EventList3.Add(ewopinfo);
                                    }

                                }

                                if (ewopinfo.EventYear > Calendars[5].Yr) //Future. el3
                                {
                                    EventList3.Add(ewopinfo);
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

        }
    }
    public class EwopInfo
    {
        public string? EventId;
        public string? EventDate;
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
        public int Mo;
        public int Yr;

    }
}

