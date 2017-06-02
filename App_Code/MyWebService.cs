using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Web;
using System.Threading;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for MyWebService
/// </summary>
[WebService(Namespace = "http://coachstationmanager.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class MyWebService : System.Web.Services.WebService
{
    CoachStationDataClassesDataContext db;
    //Phan gui mail ne
    string smtpAddress = "smtp.gmail.com";
    int portNumber = 587;
    bool enableSSL = true;
    string emailFrom = "pinkbus2017@gmail.com";
    string password = "pinkbus@2017";
    public MyWebService()
    {
        db = new CoachStationDataClassesDataContext();
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    [WebMethod]
    public List<proTIMETABLEGetTOListResult> GetListTimetable()
    {
        return db.proTIMETABLEGetTOList().ToList();
    }
   [WebMethod]
   public int LOGINforEMPLOYEE(string user, string pass,string company)
    {
        return db.proLOGINForEMPLOYEE(user,pass,company).ToList().ElementAt(0).Column1;
    }
   [WebMethod]
   public int InsertCompany(string id,string name,string tel, byte[] image)
    {
        try
        {
            db.proINSERT_COMPANY(id, name, tel, image);
            return 1;
        }
        catch(Exception e)
        {
            return 0;
        }
    }
    //
    [WebMethod]
    public List<proGET_LIST_COMPANYResult> GetListCompany()
    {
        List<proGET_LIST_COMPANYResult> list = db.proGET_LIST_COMPANY().ToList();
        return list;
    }
    //
    [WebMethod]
    public List<proCOACHSTATIONGetTOListResult> GetListCoachStation()
    {
        return db.proCOACHSTATIONGetTOList().ToList();
    }
    //
    [WebMethod]
    public int InsertCoachStation(string id,string name,string address,string tel,byte[] pic,string city)
    {
            return db.proCOACHSTATIONInsert(id, name, address, tel, pic, city).ElementAt(0).Column1;
    }
   
    [WebMethod]
    public int InsertBrandOffice(string coachstationid,string companyid,string officename,string officetel,byte[] office_picture)
    {
        return db.proINSERT_BRAND_OFFICE(coachstationid, companyid, officename, officetel, office_picture).ElementAt(0).Column1;
    }

    [WebMethod]
    public List<proGetListOfficebyCompanyResult> GetListOfficebyCompany(string company)
    {
        return db.proGetListOfficebyCompany(company).ToList();
    }

    [WebMethod]
    public List<proGetListOfficebyCoachStationResult> GetListOfficebyCoachStation(string coachstation)
    {
        return db.proGetListOfficebyCoachStation(coachstation).ToList();
    }

    [WebMethod]
    public List<proCOACHGetTOListResult> GetListCoach()
    {
        return db.proCOACHGetTOList().ToList();
    }

    [WebMethod]
    public int InsertCoach(string bienso,string loaixe, int tongghe, int fee)
    {
        return db.proCOACHInsert(bienso, loaixe, tongghe, fee).ElementAt(0).Column1;
    }
    [WebMethod]
    public int InsertEmployee(string id,string name,string tel,string vitri,byte[] pic,string nhaxe,string plate,string pass)
    {
        return db.proEMPLOYEEInsert(id, name, tel, vitri, pic, nhaxe,plate,pass).ElementAt(0).Column1;
    }
    [WebMethod]
    public int UpdateEmployee(string id,byte[] image, string plate)
    {
        return db.proUPDATE_EMPLOYEE(id, image, plate);
    }
    [WebMethod]
    public List<proITINERARYListbyCOACHROUTEResult> GetListItinerary(string idroute)
    {
        return db.proITINERARYListbyCOACHROUTE(idroute).ToList();
    }
    [WebMethod]
    public int InsertItinerary(string id,string name,string startpoint,string starttime,string endpoint,string endtime)
    {
        return db.proITINERARYInsert(id,name,startpoint,starttime,endpoint,endtime).ToList().ElementAt(0).Column1;
    }
    [WebMethod]
    public int UpdateTimeTable(string companyid)
    {
        try
        {
            db.proUPDATE_TIMETABLE(companyid);
            return 1;
        }
        catch (Exception e)
        {
            return 0;
        }
    }
    [WebMethod]
    public List<proEMPLOYEEListbyCOMPANYResult> GetListEmployeebyCompany(string companyid)
    {
        return db.proEMPLOYEEListbyCOMPANY(companyid).ToList();
    }
    [WebMethod]
    public List<proCOACHROUTEListResult> GetListCoachRoute()
    {
        return db.proCOACHROUTEList().ToList();
    }
    [WebMethod]
    public int InsertCoachRoute(string routeid,string routename,int price,string start_time,int route_time)
    {
        return db.proCOACHROUTEInsert(routeid, routename, price, start_time, route_time).ElementAt(0).Column1;
    }
    public static string ToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    //
    public void SendGmail(string toEmail, string subject, string body)
    {
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(emailFrom);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            // Can set to false, if you are sending pure text.

            using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
            {
                smtp.Credentials = new NetworkCredential(emailFrom, password);
                smtp.EnableSsl = enableSSL;
                smtp.Send(mail);
            }
        }
    }
    //Hien danh sach from to
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_FROM_CITY_LIST()
    {
      
        List<proFROM_CITYResult> list_FROM_CITY = db.proFROM_CITY().ToList();

        dynamic collectionWrapper = new
        {

            FROM_CITY = list_FROM_CITY
        };

        return ToJson(list_FROM_CITY);
    }

    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_TO_CITY_LIST(string from_city)
    {
       
        List<proTO_CITYResult> list = db.proTO_CITY(from_city).ToList();

        dynamic collectionWrapper = new
        {
            TO_CITY = list
        };

        return ToJson(list);
    }
    //Hien thi danh sach tuyen xe
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_ROUTE_DETAIL_LIST(string from, string to, string dp_date)
    {
       
        List<proROUTE_COACH_TO_LISTResult> list = db.proROUTE_COACH_TO_LIST(from, to, dp_date).ToList();

        dynamic collectionWrapper = new
        {
            ROUTE_LIST=list
        };

        return ToJson(list);
    }
    //Chon diem len xe va xuong xe
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_BOARDING_LIST(string couch_route_id, string company_name)
    {

        List<proBOARDING_LISTResult> list =
            db.proBOARDING_LIST(couch_route_id,company_name).ToList();

        dynamic collectionWrapper = new
        {
            BOARDING_LIST = list
        };

        return ToJson(list);
    }
    //Chon diem xuong xe va xuong xe
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_DROPPING_LIST(string couch_route_id, string company_name)
    {

        List<proDROPPING_LISTResult> list =
            db.proDROPPING_LIST(couch_route_id, company_name).ToList();

        dynamic collectionWrapper = new
        {
            BOARDING_LIST = list
        };

        return ToJson(list);
    }
    //Hien danh sach ghe xe
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_SEAT_LIST(string from, string to, string dp_date, string route_id, string company_id)
    {
      
        List<proSEAT_TO_LISTResult> list = db.proSEAT_TO_LIST(from, to, dp_date, route_id, company_id).ToList();

        dynamic collectionWrapper = new
        {
            SEAT_LIST = list
        };

        return ToJson(list);
    }
    //Ham dat ve xe
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_BOOKING_TICKET(String tel, String name, String dpt_date, String boarding,
        String dropping, String seat_id, String coach_route_id, String email,string order_id)
    {

        List<proTICKET_BOOKINGResult> list = db.proTICKET_BOOKING(tel, name, dpt_date, boarding, dropping, seat_id, coach_route_id, email,order_id).ToList();

        dynamic collectionWrapper = new
        {
            TICKET_ID = list
        };

        return ToJson(list);
    }
    //Cap nhat phuong thuc thanh toan
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_UPDATE_PAYMENT(string order_id, string payment_id,int ticket_id)
    {

        List<proUPDATE_PAYMENTResult> list = db.proUPDATE_PAYMENT(order_id, payment_id,ticket_id).ToList();
        string email="";
        string ticket="";
        string order_date="";
        string status_ticket="";
        string payout_date="";
        string recived_date="";
        foreach(proUPDATE_PAYMENTResult orderticket in list)
        {
            email = orderticket.EMAIL;
            ticket = ticket + orderticket.TICKET_ID.ToString();
            order_date = orderticket.ORDER_DATE.ToString();
            status_ticket = orderticket.TICKET_STATUS.ToString();
            payout_date = orderticket.PAYOUT_DATE.ToString();
            recived_date = orderticket.RECEVIED_DATE.ToString();
        }
        string subject = "Confirmation Of Successful Booking In PinkBus";
        string body = "You have successely booking the ticket. Your Ticket ID: " + ticket + ".Payout Date: " + payout_date + ". Order Date: " + order_date+". Have you a good journey";
        string body2= "You have successely booking the ticket. Your Ticket ID: " + ticket +". Order Date: " + order_date +"You have to payout before "+recived_date+ ". Have you a good journey";
        string note = "/nTo check details your ticket. Please Login app and check on My Booking.";
        try
        {
            if (status_ticket == "BOOKED")
            {
                SendGmail(email, subject, body + note);
            }
            else
            {
                SendGmail(email, subject, body2 + note);
            }
        }
        catch(Exception e)
        {
            return "0";
        }
        return ToJson(list);
    }
    //
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_TICKET_ORDER(string order_id,string email)
    {
        List<proTICKET_ORDER_LISTResult> list = db.proTICKET_ORDER_LIST(order_id,email).ToList();
        return ToJson(list);
    }
    //
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_LOGIN_PASSENGER(string email, string password)
    {
        List<proLOGINResult> list = db.proLOGIN(email, password).ToList();
        string str = list.ElementAt(0).EMAIL;
        if(str==null)
        {
            return "0";
        }
        return ToJson(list);
    }
    //
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_SIGNUP_PASSENGER(string email, string tel, string name,string password)
    {
        List<proSIGNUPResult> list = db.proSIGNUP(email, tel, name,password).ToList();
        string str = list.ElementAt(0).EMAIL;
        if (str == null)
        {
            return "0";
        }
        //
        string subject = "PinkBus- The Best App To Get Ticket";
        string body = "Hello " + name +
            ";\n You have successfully registered your PinkBus account;";
        SendGmail(str, subject, body);
       
        //
        return ToJson(list);
    }
    //
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_DELETE_TICKET(int ticket_id,string email)
    {
        int list = db.proDELETE_TICKET(ticket_id,email).ToList().ElementAt(0).RESULT;
        if (list > 0)
        {
            string subject = "Confirmation Of Successful Deleting Your Ticket.";
            string body = "It's a teriable. Your ticket " + ticket_id + " was deleted. Because you dont choose payment methoad.";
            SendGmail(email, subject, body);
            return "1";
        }
        return "0";
    }
    //Gui Mail 
    [WebMethod]
    [WebInvokeAttribute(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    public string android_GET_LIST_COMPANY()
    {
        return ToJson(db.proGET_LIST_COMPANY().ToList());
    }


}
