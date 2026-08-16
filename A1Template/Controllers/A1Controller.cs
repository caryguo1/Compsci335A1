using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using A1Template.Models;
using A1Template.Data;
using A1Template.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace A1Template.Controllers
{
    [Route("webapi")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly IA1Repo _repository;

        public CustomersController(IA1Repo repository)
        {
            _repository = repository;
        }

        [HttpGet("GetVersion")]
        public ActionResult<string> GetVersion()
        {
            string upi = "bguo686";
            string return_messgae = $"1.0.0 (Ngāruawāhia) by {upi}";
            return Ok(return_messgae);  
        }

        [HttpGet("Logo")]
        public ActionResult GetLogo()
        {
            string image_name = "Logo";
            string image_type = ".png";
            string respHeader = "image/png";
            string image_folder_name = "Logos";
            string path = Directory.GetCurrentDirectory();
            string imgDir = Path.Combine(path, image_folder_name);
            string fileName = Path.Combine(imgDir, image_name + image_type);
            
            if (System.IO.File.Exists(fileName))
            {
                return PhysicalFile(fileName, respHeader);
            }
            else 
            {
                return NotFound("File not found");
            }
            
            
        }
        [HttpGet("AllSigns")]
        public ActionResult GetAllSigns()
        {
            IEnumerable<Sign> signs = _repository.GetAllSigns();
            return Ok(signs);
        }

        [HttpGet("Signs/{search_term}")]
        public ActionResult GetSigns(string search_term)
        {
            IEnumerable<Sign> selected_signs = _repository.GetSigns(search_term);
            return Ok(selected_signs);
        }

        [HttpGet("SignImage/{sign_name}")]
        public ActionResult GetSignImage(string sign_name)
        {

            // !!!!! maybe implement GetFileName() here (strips the path from a user entered name)

            //string image_name = "Logo";
            //string image_type = ".png";
            //string respHeader = "image/png";
            string images_folder_name = "SignsImages";
            string path = Directory.GetCurrentDirectory();
            string imgDir = Path.Combine(path, images_folder_name);
            
            string fileName = Path.Combine(imgDir, sign_name);
            
            string searchPattern = $"{sign_name}.*";

            string? foundFile = Directory.GetFiles(imgDir, searchPattern).FirstOrDefault();

            if (foundFile == null)
            {
                string image_name = "default";
                string image_type = ".png";
                foundFile = Path.Combine(imgDir, image_name + image_type);
            }

                var provider = new FileExtensionContentTypeProvider();

                string? contentType;
                if (!provider.TryGetContentType(foundFile, out contentType))
                {
                    // Fallback type if the extension is completely unique or unknown
                    contentType = "application/octet-stream"; 
                }

            // 6. Stream the file directly to the client
            return PhysicalFile(foundFile, contentType);
            
            
        }

        [HttpGet("GetComment/{id}")]
        public ActionResult GetComment(int id)
        {
            Comment? comment = _repository.GetCommentById(id);
            if (comment == null)
            {
                return BadRequest($"Comment {id} does not exist.");
            } 
            else
            {
                return Ok(comment);
            }
        }

        [HttpPost("WriteComment")]
        public ActionResult WriteComment(CommentInputDto input_comment)
        {
            Comment new_comment = new Comment {UserComment = input_comment.UserComment, Name = input_comment.Name};
            Comment addedComment = _repository.AddComment(new_comment);
            return CreatedAtAction(nameof(WriteComment), new {id = addedComment.Id}, addedComment);
        }

        [HttpGet("Comments/{number_of_comments_input?}")]
        public ActionResult Comments(int? number_of_comments_input = 5)
        {
            int number_of_comments = number_of_comments_input ?? 5;

            IEnumerable<Comment> selected_comments =
                _repository.GetFirstNComments(number_of_comments);

            return Ok(selected_comments);
        }



        /*
        // GET /webapi/GetCustomers
        [HttpGet("GetCustomers")]
        public ActionResult<IEnumerable<CustomerOutDto>> GetCustomers()
        {
            IEnumerable<Customer> customers = _repository.GetAllCustomers();
            IEnumerable<CustomerOutDto> c = customers.Select(e => new CustomerOutDto { Id = e.Id, FirstName = e.FirstName, LastName = e.LastName });
            return Ok(c);
        }

        // GET /webapi/GetCustomer/{id}
        [HttpGet("GetCustomer/{id}")]
        public ActionResult<CustomerOutDto> GetCustomer(int id)
        {
            Customer customer = _repository.GetCustomerByID(id);
            if (customer == null)
                return NotFound();
            else {
                CustomerOutDto c = new CustomerOutDto { Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName };
                return Ok(c);
            }
                
        }

        [HttpPost("AddCustomer")]
        public ActionResult<CustomerOutDto> AddCustomer(CustomerInputDto customer)
        {
            Customer c = new Customer { FirstName = customer.FirstName, LastName = customer.LastName, Email = customer.Email };
            Customer addedCustomer = _repository.AddCustomer(c);
            CustomerOutDto co = new CustomerOutDto { Id = addedCustomer.Id, FirstName = addedCustomer.FirstName, LastName = addedCustomer.LastName };
            return CreatedAtAction(nameof(GetCustomer), new { id = co.Id }, co);
        }
        */

        //// PUT /webapi/UpdateCustomer/{id}
        //[HttpPut("UpdateCustomer/{id}")]
        //public ActionResult UpdateCustomer(int id, CustomerInputDto customer)
        //{
        //    Customer c = _repository.GetCustomerByID(id);
        //    if (c == null)
        //        return NotFound();
        //    else
        //    {
        //        c.FirstName = customer.FirstName;
        //        c.LastName = customer.LastName;
        //        c.Email = customer.Email;
        //        _repository.SaveChanges();
        //        return NoContent();
        //    }
        //}

        //// DELETE /webapi/DeleteCustomer/{id}
        //[HttpDelete("DeleteCustomer/{id}")]
        //public ActionResult DeleteCustomer(int id)
        //{
        //    Customer c = _repository.GetCustomerByID(id);
        //    if (c == null)
        //        return NotFound();
        //    else
        //    {
        //        _repository.DeleteCustomer(id);
        //        return NoContent();
        //    }
        //}
    }
}
