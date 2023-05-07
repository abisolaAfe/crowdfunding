using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using crowfunding.Models;
using System.Data.Entity.Validation;
namespace crowfunding.Controllers
{
    public class UserController : ApiController    
    {
        CrowFudingEntities14 db = new CrowFudingEntities14();

        //this is for user authenticatin here we are getting username and password and compairing it with vaules in profile db  
        [HttpGet]
        public HttpResponseMessage UserLogin(string username, string password)
        {
            try
            {

                var userFound = db.Profiles.Where(u => u.user_email == username).FirstOrDefault();
                Profile i = new Profile();
                i.user_fname = userFound.user_fname;
                i.user_id = userFound.user_id;
                if (userFound == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = false, message = "user not found" });
                }
                if (SecurePasswordHasher.Verify(password, userFound.user_password))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = true, user = i });
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = false, message = "user not found" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        //this is for sign up it will get get data from sign up page and save in db profile
        [HttpPost]
        public HttpResponseMessage SignUp(Profile signup)
        {
            try
            {

                var hash = SecurePasswordHasher.Hash(signup.user_password);
                signup.user_password = hash;
                var userFound = db.Profiles.Where(u => u.user_email == signup.user_email).ToList();//for checking whether email exits in email or not
                if (userFound.Count==0)
                {
                        db.Profiles.Add(signup);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = true, message = "sign up succesfully" });
  
                }


                return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = false,message="email already exits"});
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //this will get compaingn data front end save it in db compaign
        [HttpPost]
        public HttpResponseMessage createCompaign(Compaign compaign)
        {
            try
            {

                compaign.compaign_date_created = DateTime.Now.ToString();
                db.Compaigns.Add(compaign);// for saving data in db compaign
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = true,message="compaign added" });//for sending response to compaingn
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
        //this is for showing user all compaigns in the app
        [HttpGet]
        public HttpResponseMessage showAllCompaigns(int id)
        {
            try
            {
              var allcompaings=  db.Compaigns.Where(o => o.compaign_creater_id != id);

              var selectee = allcompaings.Join(db.Profiles, u => u.compaign_creater_id, p => p.user_id, (u, p) => new
              {
                
                  u.compaign_description,
                  u.compaign_title,
                  u.compaign_id,
                  p.user_fname,
                  u.compaign_date_created

              }).ToList();


              return Request.CreateResponse(HttpStatusCode.OK,  selectee );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
         //this is for showing user their own compaign that was created by him 
        [HttpGet]
        public HttpResponseMessage showMyCompaigns(int id)
        {
            try
            {
                List<customMyCompaingns> myList = new List<customMyCompaingns>();
                
                var findCompaign = db.Compaigns.Where(p => p.compaign_creater_id == id).ToList();//finding the user in in compaings
                if (findCompaign.Count() == 0) {//checking nulls
                    return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = false, message = "No compaigns" });
                
                }
                foreach (var item in findCompaign) {
                    var data = new customMyCompaingns();
                    data.id = item.compaign_id;
                    data.title = item.compaign_title;
                    int withdrawAmount = 0;
                    var tots = db.Donations.Where(p => p.donation_compaign_id == item.compaign_id);//for finding donations on his compaings
                    var findWithDrawl = db.withdrawl_desciption.Where(o => o.withdrawl_status == 1 && o.withdrawl_compaign_id == item.compaign_id).ToList();
                    if (findWithDrawl.Count() == 0)
                    {
                        withdrawAmount = 0;
                    }
                    else
                    {
                        withdrawAmount = findWithDrawl.Sum(l => l.withdrawl_amount);
                    }
                    if (tots.Count() == 0)
                    {
                        data.amount = 0;
                    }
                    else {
                        data.amount = tots.Sum(o => o.donation_amount)-withdrawAmount;//adding all the donations  and substracting withdrawl amount to calculate current balance
                    }

                    myList.Add(data);
                }
                return Request.CreateResponse(HttpStatusCode.OK, myList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        //this is for the detail of compaign here this funtion is getting compaign id 
        [HttpGet]
        public HttpResponseMessage compaignDetail(int cid)
        {
            customCompaignDetail detail = new customCompaignDetail();
            try
            {
                //for finding the users compaign based on his id
                var allcompaings = db.Compaigns.Where(o => o.compaign_id == cid).FirstOrDefault();//finding the compaign
                detail.title = allcompaings.compaign_title;
                var dc = db.Donations.Where(s => s.donation_compaign_id == cid).ToList();//finding donations on that compaingn so we can calculate current balance etc
                if (dc.Count == 0)
                {
                    detail.totalDonation = 0;
                }
                else { 
                   detail.totalDonation=dc.Sum(l => l.donation_amount);
                }

                detail.countPeopleDonated = dc.Count();
                var findWithDrawl = db.withdrawl_desciption.Where(o => o.withdrawl_status == 1&&o.withdrawl_compaign_id==cid).ToList();
                if (findWithDrawl.Count() == 0)
                {
                    detail.withdrawnAmount = 0;
                }
                else
                {
                    detail.withdrawnAmount = findWithDrawl.Sum(l => l.withdrawl_amount);
                }
                detail.currentBalance = detail.totalDonation - detail.withdrawnAmount;
                return Request.CreateResponse(HttpStatusCode.OK, detail);
               
                
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }


        //this is for adding donation the donaters will add donation and will be added in dbDonation
        [HttpPost]
        public HttpResponseMessage addDonation(Donation donation)
        {
            try
            {
                DateTime now = DateTime.Now;
                donation.donation_date_created = now.ToString();
                db.Donations.Add(donation);//for saving to db Donations
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = true, message = "compaign added" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        //this function is used to request withdraw 
        [HttpPost]
        public HttpResponseMessage requestWithdraw(withdrawl_desciption request)
        {
            try
            {
                var findDonatersId = db.Donations.Where(i => i.donation_compaign_id == request.withdrawl_compaign_id).ToList();//
                if (findDonatersId.Count() == 0) {
                    return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = false, message = "No Donaters to send Request" });
                }
                request.donation_date_created = DateTime.Now.ToString();
                var datas = new withdrawl_desciption();
                datas.withdrawl_compaign_id = request.withdrawl_compaign_id;
             
                datas.withdrawl_amount = request.withdrawl_amount;
                datas.donation_date_created = request.donation_date_created;
                datas.withdrawl_discription = request.withdrawl_discription;
                datas.withdrawl_user_id = request.withdrawl_user_id;
                
                db.withdrawl_desciption.Add(datas);
                db.SaveChanges();
                
               
              
        //here we are adding donaters id in withdraw requests table so that they can see request
                foreach (var item in findDonatersId) {
                    var withDrawlRequest = new withdrawls_request();
                    withDrawlRequest.requested_by = request.withdrawl_user_id;
                    withDrawlRequest.requested_to = item.donation_user_id;
                    withDrawlRequest.status = 3;
                    withDrawlRequest.withdraw_description_id = datas.withdrawl_reference;
                    db.withdrawls_request.Add(withDrawlRequest);
                }
                db.SaveChanges();       
                

                 return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = true });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        //this is for showing donaters withdrawl request that was send by the compaign creater
        [HttpGet]
        public HttpResponseMessage showDonatersRequest(int id)
        {
            try
            {
                var requests = db.withdrawls_request.Where(o => o.requested_to == id && o.status == 3).Select(l => new
                {
                    l.withdraw_description_id
                });

                var description = db.withdrawl_desciption.Join(requests, p => p.withdrawl_reference, o => o.withdraw_description_id, (p, o) => new
                {
                    p.withdrawl_status,
                    p.withdrawl_reference,
                    p.withdrawl_amount,
                    p.withdrawl_compaign_id,
                    p.withdrawl_discription
                }).Where(d => d.withdrawl_status == null).Join(db.Compaigns, k => k.withdrawl_compaign_id, u => u.compaign_id, (k, u) => new { 
                    k.withdrawl_reference,
                    u.compaign_title,
                    k.withdrawl_amount,
                    k.withdrawl_discription
                });
              
                return Request.CreateResponse(HttpStatusCode.OK, description);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        //this methode will accept and reject compaign creater withdrawl request
        [HttpGet]
        public HttpResponseMessage approveDisaproveWithDrawal(Boolean status, int withdrawl_reference,int userId)
        {
            try
            {
                var compaingn = db.withdrawl_desciption.Where(o => o.withdrawl_reference == withdrawl_reference).FirstOrDefault();
                var totalDonation = db.Donations.Where(o => o.donation_compaign_id == compaingn.withdrawl_compaign_id).Count();
               
                var withdrawlSucessNumber = (75 /(float) 100) * totalDonation;//for finding 
               
                var floorvaule = Math.Floor(withdrawlSucessNumber);
                
                var withdrawlRequest = db.withdrawls_request.Where(i => i.withdraw_description_id == withdrawl_reference && i.requested_to == userId).FirstOrDefault();
                var updatedwithdrawlRequest = withdrawlRequest;
                
                 if (status)
                {
                    updatedwithdrawlRequest.status = 1;
                   
                }
                else {
                    updatedwithdrawlRequest.status = 2;
                }
                 db.Entry(withdrawlRequest).CurrentValues.SetValues(updatedwithdrawlRequest);
                 db.SaveChanges();
                 var accepted = db.withdrawls_request.Where(o => o.status == 1&&o.withdraw_description_id==withdrawl_reference).Count();
               
                 if ((Double)accepted >= floorvaule)
                 {
                     var updatedWithDrawlDiscription = compaingn;
                     updatedWithDrawlDiscription.withdrawl_status = 1;
                     db.Entry(compaingn).CurrentValues.SetValues(updatedWithDrawlDiscription);
                 }
                 db.SaveChanges();
                

                return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = true });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

       

        //this methode is used to refund amount 
        [HttpGet]
        public HttpResponseMessage donaterSupportedCompaigns(int userId)
        {
            try
            {
                var data = db.Donations.Where(u => u.donation_user_id == userId).Select(o => new { 
                    o.donation_compaign_id,
                    o.donation_date_created,
                    o.donation_reference,
                    o.donation_amount
                });
                List<customSupported> items = new List<customSupported>();
                var join = data.Join(db.Compaigns, p => p.donation_compaign_id, o => o.compaign_id, (p, o) => new
                {
                    o.compaign_id,
                    o.compaign_title,
                    p.donation_reference,
                    p.donation_amount
                    
                });
                foreach (var it in join) {
                    customSupported datas = new customSupported();
                    datas.compaignId = it.compaign_id;
                    datas.donationId = it.donation_reference;
                    datas.title = it.compaign_title;
                    var checkwithdraw = db.refunds.Where(i => i.users_id == userId && i.Compaign_id == it.compaign_id && i.donation_reference == it.donation_reference).FirstOrDefault();
                    if (checkwithdraw == null)
                    {
                        datas.status = 0;
                    }
                    else {
                        datas.status = 1;
                    }
                    datas.amount = it.donation_amount;
                    items.Add(datas);
                }
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        //this functionis used to refund  
        [HttpGet]
        public HttpResponseMessage refundRequest(int userId,int donationId)
        {
            try
            {
                var data = db.Donations.Where(u => u.donation_user_id == userId && u.donation_reference == donationId).FirstOrDefault();

                Donation item = new Donation();
                item = data;
                var totalDonations=db.Donations.Where(i=>i.donation_compaign_id==data.donation_compaign_id);
                var withdrawls = db.withdrawl_desciption.Where(i => i.withdrawl_compaign_id == data.donation_compaign_id&&i.withdrawl_status==1).ToList();
              
                
                int total=0;
                int withdraw = 0;
                foreach(var it in totalDonations){
                    total=total+it.donation_amount;
                }
                if (withdrawls.Count == 0)
                {
                    withdraw=0;
                }
                else{
                    withdraw = withdrawls.Sum(k => k.withdrawl_amount);
                }

                int walletbalance = total - withdraw; 
                if (total == 0) {
                    return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = false ,message="No Donations"});
                }
                var result = (item.donation_amount / (float)total) * walletbalance;
                if (walletbalance-result>1)
                {
                    var updatedDonation = data;
                    updatedDonation.donation_amount = 0;
                    db.Entry(data).CurrentValues.SetValues(updatedDonation);
                    refund i = new refund();
                    i.Compaign_id = data.donation_compaign_id;
                    i.users_id = userId;
                    i.Amount = data.donation_amount;
                    i.donation_reference = data.donation_reference;
                    db.refunds.Add(i);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = true, message = "Refunded Succesfuly" });
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.OK, new { isSuccessfull = false, message = "sorry you cannot refund" });
                }
                
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

         
    }
}
