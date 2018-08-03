using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UniversalProviders_Identity_Migrations;
using UniversalProviders_Identity_Migrations.Models;

namespace MigrateProfileData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dbContext = new ApplicationDbContext();

            foreach (var profile in dbContext.Profiles)
            {
                var stringId = profile.UserId.ToString();

                var user = dbContext.Users.Where(x => x.Id == stringId).FirstOrDefault();

                Console.WriteLine("Adding Profile for user:" + user.UserName);

                var serializer = new XmlSerializer(typeof(ProfileInfo));

                var stringReader = new StringReader(profile.PropertyValueStrings);

                var profileData = serializer.Deserialize(stringReader) as ProfileInfo;

                if (profileData == null)
                {
                    Console.WriteLine("Profile data deserialization error for user:" + user.UserName);
                }
                else
                {
                    user.Profile = profileData;
                }
            }
            dbContext.SaveChanges();
        }
    }
}
