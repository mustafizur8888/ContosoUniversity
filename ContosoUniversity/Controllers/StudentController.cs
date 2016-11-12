using System;
using System.Linq;
using System.Net;
using PagedList;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolContext _db = new SchoolContext();
        // GET: Student
        public ActionResult Index(string searchString, string currentFilter,
            string sortOrder, int? page)
        {
            IQueryable<Student> students = _db.Students;

            ViewBag.CurrentSort = sortOrder;

            ViewBag.sortOrder = string.IsNullOrWhiteSpace(sortOrder) ? "name_desc" : "";
            ViewBag.sortDateOrder = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                students = students.Where(x => x.FirstMidName.Contains(searchString) || x.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;

            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);



            return View(students.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [HttpGet]
        public ActionResult Add()
        {
            Student student = new Student { EnrollmentDate = DateTime.Today };
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "LastName, FirstMidName, EnrollmentDate")]Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Students.Add(student);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(student);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student studentToEdit = _db.Students.Find(id);
            return View(studentToEdit);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = _db.Students.Find(id);
            if (TryUpdateModel(studentToUpdate, "", new[] { "LastName", "FirstMidName", "EnrollmentDate" }))
            {
                try
                {
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(studentToUpdate);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student studentToDelete = _db.Students.Find(id);
            if (studentToDelete == null)
            {
                return HttpNotFound();
            }
            return View(studentToDelete);
        }
        [HttpPost]
        public ActionResult Delete([Bind(Include = "Id")]Student student)
        {

            Student studentToDelete = _db.Students.Find(student.ID);
            if (studentToDelete == null)
            {
                return HttpNotFound();
            }
            _db.Students.Remove(studentToDelete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}