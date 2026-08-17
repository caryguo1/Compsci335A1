//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using A1Template.Models;

namespace A1Template.Data
{
    public class A1Repo : IA1Repo
    {
        private readonly A1DbContext _dbContext;

        public A1Repo(A1DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Sign> GetAllSigns()
        {
            IEnumerable<Sign> signs = _dbContext.Signs.ToList<Sign>(); 
            return signs;
        }

        public IEnumerable<Sign> GetSigns(string search_term)
        {
            IEnumerable<Sign> selected_signs = _dbContext.Signs.Where(s => s.Description.ToLower().Contains(search_term)).ToList<Sign>(); 
            return selected_signs;
        }
        
        public Comment? GetCommentById(int id)
        {
            Comment? selected_comment = _dbContext.Comments.FirstOrDefault(comment => comment.Id == id);
            return selected_comment;
        }

        public Comment AddComment(Comment input_comment)
        {
            EntityEntry<Comment> added_comment = _dbContext.Comments.Add(input_comment);
            Comment added_comment_entity = added_comment.Entity;
            _dbContext.SaveChanges();
            return added_comment_entity;
        }
        
        public IEnumerable<Comment> GetFirstNComments(int num_of_comments)
        {
            IEnumerable<Comment> selected_comments = _dbContext.Comments.OrderByDescending(comment => comment.Id).Take(num_of_comments).ToList<Comment>();
            return selected_comments; 
        }
        


        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
