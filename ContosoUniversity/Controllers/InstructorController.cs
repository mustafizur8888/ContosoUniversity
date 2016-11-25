using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        // GET: Instructor

        private readonly SchoolContext _db = new SchoolContext();
        public ActionResult Index(int? id, int? courseId)
        {
            var viewModel = new InstructorIndexData
            {
                Instructors = _db.Instructors
                    .Include(i => i.OfficeAssignment)
                    .Include(i => i.Courses.Select(c => c.Department))
                    .OrderBy(i => i.LastName)
            };

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Single(x => x.ID == id).Courses;
            }
            if (courseId != null)
            {
                ViewBag.CourseID = courseId.Value;
                viewModel.Enrollments = viewModel.Courses.Single(x => x.CourseID == courseId.Value).Enrollments;
            }

            return View(viewModel);
        }
    }

    public class InstructorIndexData : IEnumerable
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}