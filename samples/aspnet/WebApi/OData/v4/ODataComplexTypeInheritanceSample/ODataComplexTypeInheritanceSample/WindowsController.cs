using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ODataComplexTypeInheritanceSample
{
    public class WindowsController : ODataController
    {
        private IList<Window> _windows = new List<Window>();

        public WindowsController()
        {
            Polygon triagle = new Polygon() { HasBorder = true, Vertexes = new List<Point>() };
            triagle.Vertexes.Add(new Point() { X = 1, Y = 2 });
            triagle.Vertexes.Add(new Point() { X = 2, Y = 3 });
            triagle.Vertexes.Add(new Point() { X = 4, Y = 8 });

            Polygon rectangle = new Polygon() { HasBorder = true, Vertexes = new List<Point>() };
            rectangle.Vertexes.Add(new Point() { X = 0, Y = 0 });
            rectangle.Vertexes.Add(new Point() { X = 2, Y = 0 });
            rectangle.Vertexes.Add(new Point() { X = 2, Y = 2 });
            rectangle.Vertexes.Add(new Point() { X = 0, Y = 2 });
            
            Circle circle = new Circle() { HasBorder = true, Center = new Point(), Radius = 2 };

            Window dashboardWindow = new Window
            {
                Id = 1,
                Name = "CircleWindow",
                CurrentShape = circle,
                OptionalShapes = new List<Shape>(),
            };
            dashboardWindow.OptionalShapes.Add(rectangle);
            _windows.Add(dashboardWindow);

            Window popupWindow = new Window
            {
                Id = 2,
                Name = "Popup",
                CurrentShape = rectangle,
                OptionalShapes = new List<Shape>(),
                Parent = dashboardWindow,
            };

            popupWindow.OptionalShapes.Add(triagle);
            popupWindow.OptionalShapes.Add(circle);
            _windows.Add(popupWindow);

            Window anotherPopupWindow = new Window
            {
                Id = 3,
                Name = "AnotherPopup",
                CurrentShape = rectangle,
                OptionalShapes = new List<Shape>(),
                Parent = popupWindow,
            };

            anotherPopupWindow.OptionalShapes.Add(triagle);
            anotherPopupWindow.OptionalShapes.Add(circle);
            _windows.Add(anotherPopupWindow);
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_windows);
        }

        [EnableQuery]
        public SingleResult<Window> GetWindow([FromODataUri] int key)
        {
            return SingleResult.Create<Window>(_windows.Where(w => w.Id == key).AsQueryable());
        }

        public IHttpActionResult Post(Window window)
        {
            _windows.Add(window);
            window.Id = _windows.Count + 1;
            return Created(window);
        }

        [ODataRoute("Windows({key})/CurrentShape/ODataComplexTypeInheritanceSample.Circle/Radius")]
        public IHttpActionResult GetRadius(int key)
        {
            Window window = _windows.FirstOrDefault(e => e.Id == key);
            if (window == null)
            {
                return NotFound();
            }

            return Ok(((Circle)window.CurrentShape).Radius);
        }

        [ODataRoute("Windows({key})/ODataComplexTypeInheritanceSample.GetTheLastOptionalShape()")]
        public IHttpActionResult GetTheLastOptionalShape(int key)
        {
            Window window = _windows.FirstOrDefault(e => e.Id == key);
            if (window == null)
            {
                return NotFound();
            }
            int count = window.OptionalShapes.Count;
            Shape last = window.OptionalShapes.ElementAt(count - 1);
            return Ok(last);
        }


        public IHttpActionResult AddOptionalShape(int key, ODataActionParameters parameters)
        {
            Shape newShape = parameters["shape"] as Shape;
            Window window = _windows.FirstOrDefault(e => e.Id == key);
            if (window == null)
            {
                return NotFound();
            }
            window.OptionalShapes.Add(newShape);
            int count = window.OptionalShapes.Count;
            return Ok(count);
        }
    }
}
