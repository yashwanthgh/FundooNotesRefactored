using ModelLayer.RegisterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helper
{
    public class KafkaPublishingDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public KafkaPublishingDetails(RegisterUserModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (model.FirstName == null) throw new ArgumentNullException("First name is null");
            if (model.LastName == null) throw new ArgumentNullException("Last name is null");
            if (model.Email == null) throw new ArgumentNullException("Email is null");
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.Email;
        }
    }
}
