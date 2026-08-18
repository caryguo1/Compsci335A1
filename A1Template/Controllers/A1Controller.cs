//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using A1Template.Models;
using A1Template.Data;
using A1Template.Dtos;
//using Microsoft.AspNetCore.Http;
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

        [HttpGet("Signs/{term}")]
        public ActionResult GetSigns(string term)
        {
            IEnumerable<Sign> selected_signs = _repository.GetSigns(term.ToLower());
            return Ok(selected_signs);
        }

        [HttpGet("SignImage/{id}")]
        public ActionResult GetSignImage(string id)
        {
            string sign_name = id;
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
            string IPAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            Comment new_comment = new Comment {UserComment = input_comment.UserComment, Name = input_comment.Name, IP = IPAddress};
            Comment addedComment = _repository.AddComment(new_comment);
            return CreatedAtAction(nameof(GetComment), new {id = addedComment.Id}, addedComment);
        }

        [HttpGet("Comments/{num?}")]
        public ActionResult Comments(int? num = 5)
        {
            int number_of_comments = num ?? 5;

            IEnumerable<Comment> selected_comments =
                _repository.GetFirstNComments(number_of_comments);

            return Ok(selected_comments);
        }
    }
}
