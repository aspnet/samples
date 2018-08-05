using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace UniversalProviders_Identity_Migrations.Models
{
    [Serializable]
    [ComplexType]
    public class ProfileInfo
    {
        public ProfileInfo()
        {
            UserStats = new PersonalStats();
        }

        public DateTime? DateOfBirth { get; set; }

        public PersonalStats UserStats { get; set; }

        public string City { get; set; }
    }

    [Serializable]
    [ComplexType]
    public class PersonalStats
    {
        public int? Weight { get; set; }

        public int? Height { get; set; }
    }

    public class AppProfile : ProfileBase
    {
        public ProfileInfo ProfileInfo
        {
            get { return (ProfileInfo)GetPropertyValue("ProfileInfo"); }
        }

        public static AppProfile GetProfile()
        {
            return (AppProfile)HttpContext.Current.Profile;
        }

        public static AppProfile GetProfile(string userName)
        {
            return (AppProfile)Create(userName);
        }
    }
}