using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Models
{
    /// <summary>
    /// Model class for storing send email content such as CC, BCC, subject, etc.
    /// </summary>
    public class UserSideSendMailModel
    {
        /// <summary>
        /// Constructor for providing default List values.
        /// </summary>
        public UserSideSendMailModel()
        {
            this.Tos = new List<string>();
            this.Ccs = new List<string>();
            this.Bccs = new List<string>();
        }

        /// <summary>
        /// The "From" field in the email.
        /// </summary>
        /// <value></value>
        public string From { get; set; }

        /// <summary>
        /// The List of "To" or email address destination.
        /// </summary>
        /// <value></value>
        public List<string> Tos { get; set; }

        /// <summary>
        /// The List of "CC" or carbon copy.
        /// </summary>
        /// <value></value>
        public List<string> Ccs { get; set; }

        /// <summary>
        /// The List of "BCC" or blind carbon copy.
        /// </summary>
        /// <value></value>
        public List<string> Bccs { get; set; }

        /// <summary>
        /// The email subject.
        /// </summary>
        /// <value></value>
        public string Subject { get; set; }

        /// <summary>
        /// The email message / body.
        /// </summary>
        /// <value></value>
        public string Message { get; set; }
    }
}
