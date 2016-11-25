using System.Collections.Generic;
using System.Web.Mvc;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class CourseViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<SelectListItem> Department { get; set; }
    }
}