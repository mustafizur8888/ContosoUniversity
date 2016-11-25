using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course

        private readonly SchoolContext _db = new SchoolContext();

        public ActionResult Index()
        {
            var courses = _db.Courses.Include(x => x.Department).ToList();
            return View(courses);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var courseViewModel = new CourseViewModel { Course = new Course() };
            ICollection<Department> departments = _db.Departments.OrderBy(x => x.Name).ToList();
            courseViewModel.Department = new SelectList(departments, "DepartmentID", "Name");
            ViewBag.Readonly = false;
            return View(courseViewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course c = _db.Courses.Single(x => x.CourseID == id);
            if (c==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            CourseViewModel courseViewModel = new CourseViewModel
            {
                Course =  c,
                Department =  new SelectList(_db.Departments.OrderBy(x=>x.Name), "DepartmentID", "Name",
                c.DepartmentID)
            };

            ViewBag.Readonly = true;
            return View("Create",courseViewModel);
        }

        [HttpPost]
        public ActionResult Create(Course course)
        {
            Course dCourse = new Course
            {
                CourseID = course.CourseID,
                DepartmentID = course.DepartmentID,
                Title = course.Title,
                Credits = course.Credits
            };
            _db.Courses.Add(dCourse);
            int reult = _db.SaveChanges();
            if (reult > 0)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}