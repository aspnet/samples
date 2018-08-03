using ODataVersioningSample.Tests.V1;
using ODataVersioningSample.Tests.V2;

namespace ODataVersioningSample.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            new V1Client().RunTests();
            new V2Client().RunTests();
        }
    }
}
