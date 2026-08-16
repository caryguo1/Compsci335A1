using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A1Template.Models;

namespace A1Template.Data
{
    public interface IA1Repo
    {
        //string GetVersion();
        IEnumerable<Sign> GetAllSigns();
        IEnumerable<Sign> GetSigns(string search_term);
        Comment? GetCommentById(int id);
        Comment AddComment(Comment comment);
        IEnumerable<Comment> GetFirstNComments(int num_of_comments);
        


        //IEnumerable<Customer> GetAllCustomers();
        //Customer GetCustomerByID(int id);
        //Customer AddCustomer(Customer customer);
        //void DeleteCustomer(int id);
        //void SaveChanges();
    }
}

