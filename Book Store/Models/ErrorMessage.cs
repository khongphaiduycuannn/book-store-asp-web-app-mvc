using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Book_Store.Models
{
    public class ErrorMessage
    {
        public ErrorMessage()
        {

        }

        public ErrorMessage(String Message)
        {
            this.Message = Message;
        }

        public String Message { get; set; }
    }
}