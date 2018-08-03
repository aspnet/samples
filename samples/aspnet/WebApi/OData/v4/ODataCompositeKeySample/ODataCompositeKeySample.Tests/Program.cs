using System;
using System.Linq;
using ODataCompositeKeySample.Tests.ODataCompositeKeySample.Models;
using ODataCompositeKeySample.Tests.Default;

namespace ODataCompositeKeySample.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GetPeople();
            GetPerson();
            PostPerson();
            PatchPerson();
            DeletePerson();
            Console.WriteLine("Pres any key to continue...");
            Console.ReadKey();
        }

        private static Container GetPeople()
        {
            Console.WriteLine("Get People");
            Container c = new Container();
            var people = c.People;
            foreach (var p in people)
            {
                Console.WriteLine("\t{0}, {1}: {2}", p.FirstName, p.LastName, p.Age);
            }
            return c;
        }

        private static void GetPerson()
        {
            Console.WriteLine("Get Person");
            Container c = new Container();
            var person = c.People.Where(p => p.FirstName == "Kate" && p.LastName == "Jones").Single();
            Console.WriteLine("\t{0}, {1} was found", person.FirstName, person.LastName);
        }

        private static void PostPerson()
        {
            Console.WriteLine("Post Person");
            Container c = new Container();
            Person person = new Person { FirstName = "Eric", LastName = "Smith", Age = 8 };
            c.AddToPeople(person);
            c.SaveChanges();
            Console.WriteLine("\t{0}, {1} was added", person.FirstName, person.LastName);
            GetPeople();
        }

        private static void PatchPerson()
        {
            Console.WriteLine("Patch Person");
            Container c = new Container();
            var person = c.People.Where(p => p.FirstName == "Eric" && p.LastName == "Smith").Single();
            person.Age = 9;
            c.UpdateObject(person);
            c.SaveChanges();
            Console.WriteLine("\t{0}, {1}'s age is changed to {2}", person.FirstName, person.LastName, person.Age);
            GetPeople();
        }

        private static void DeletePerson()
        {
            Console.WriteLine("Delete Person");
            Container c = new Container();
            var person = c.People.Where(p => p.FirstName == "Eric" && p.LastName == "Smith").Single();
            c.DeleteObject(person);
            c.SaveChanges();
            Console.WriteLine("\t{0}, {1} was deleted", person.FirstName, person.LastName);
            GetPeople();
        }
    }
}
