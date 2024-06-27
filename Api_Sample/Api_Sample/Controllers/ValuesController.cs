using Api_Sample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Api_Sample.Controllers
{
    [RoutePrefix("Values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route("details")]
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            List<Customer> customerList = new List<Customer>();
            using(CustomerContext customerContext = new CustomerContext())
            {
                var temp = customerContext.Customer_DBtable.Select(c => c.ID).ToList();
                foreach (var cid in temp) {
                 Customer customer= new Customer();
                    customer = customerContext.Customer_DBtable.Single(c => c.ID == cid);
                    customerList.Add(customer);
                        }
                return (customerList);
            }
           
        }

        [Route("api/getimage")]
        [HttpGet]
        public IHttpActionResult GetImage(int id)
        {
            try
                {
                using (CustomerContext customerContext = new CustomerContext()) // Replace with your DbContext instantiation
                {
                    var image = customerContext.Customer_DBtable.Single(c=>c.ID==id); // Replace with appropriate logic to fetch the desired image

                    if (image != null)
                    {
                        string base64Image = Convert.ToBase64String(image.ImageData);
                        return Ok(new { imageData = base64Image });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
    }
}

        // GET api/values/5
        //public string Get()
        //{
        //    return ;
        //}


        // POST api/values
        [Route("create")]
        [HttpPost]
        public IHttpActionResult Post(Customer customer_detail)
        {
            try
            {
                using (CustomerContext customer_details = new CustomerContext())
                {
                    Customer customer = new Customer();
                    customer.First_Name = customer_detail.First_Name;
                    customer.Last_Name = customer_detail.Last_Name;
                    customer.Email_ID = customer_detail.Email_ID;
                    customer.Password = customer_detail.Password;
                    customer_details.Customer_DBtable.Add(customer);
                    customer_details.SaveChanges();
                    return Ok(customer.ID);
                }
            }
            catch (Exception e)
            {

                return Ok(-1);
            }
        }

        [Route("uploadimage")]
        [HttpPost]
        public IHttpActionResult Upload_Image()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files["image"];
                string id = HttpContext.Current.Request.Form["id"];
                var Id = Convert.ToInt32(id);               //converting string id to int id

                CustomerContext customer = new CustomerContext();

                var result = customer.Customer_DBtable.FirstOrDefault(c => c.ID == Id);
                if (result.ID > 0)
                {
                    if (postedFile != null)
                    {
                        using (var binaryReader = new BinaryReader(postedFile.InputStream))
                        {
                            byte[] imageData = binaryReader.ReadBytes(postedFile.ContentLength);

                            using (CustomerContext customerContext = new CustomerContext())
                            {
                                result.ImageData = imageData;
                      
                                var temp = result;

                                customerContext.Customer_DBtable.AddOrUpdate(result);
                                customerContext.SaveChanges();
                            }

                            return Ok(new { message = "Image uploaded and saved successfully." });
                        }
                    }
                    else
                    {
                        return BadRequest("No image data found in the request.");
                    }
                }
                else
                {
                    return BadRequest("No User found.");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        //login 
        [Route("api/logincustomer")]
        [HttpPost]
        public HttpResponseMessage LoginCustomer(string Emailid, string password)
        {
            if (Emailid == " " && password == " ")
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter EmailId and Password");
            }
            else
            {
                CustomerContext customerContext = new CustomerContext();
                Customer custom_details = new Customer();
                var login = customerContext.Customer_DBtable.Where(c => c.Email_ID == Emailid && c.Password == password).Count();
                if (login > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User not found");
                }
            }
        }
        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
