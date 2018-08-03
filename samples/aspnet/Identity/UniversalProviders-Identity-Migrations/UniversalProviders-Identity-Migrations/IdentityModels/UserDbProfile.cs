using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversalProviders_Identity_Migrations.IdentityModels
{
    public class UserDbProfile
    {
        public UserDbProfile()
        {
        }

        [Key]
        public Guid UserId { get; set; }
        public string PropertyNames { get; set; }
        public string PropertyValueStrings { get; set; }
        public byte[] PropertyValueBinary { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}