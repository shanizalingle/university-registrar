using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Registrar.Controllers
{
  public class DepartmentsController : Controller
  {
    private readonly RegistrarContext _db;

    public DepartmentsController(RegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Department> model = _db.Departments.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Title");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Department department, int StudentId, int CourseId)
    {
      _db.Departments.Add(department);
      _db.SaveChanges();
      if (CourseId != 0)
      {
        _db.DepartmentCourse.Add(new DepartmentCourse() { CourseId = CourseId, DepartmentId = department.DepartmentId });
        _db.SaveChanges();
      }
      if (StudentId != 0)
      {
        _db.DepartmentStudent.Add(new DepartmentStudent() {  StudentId = StudentId, DepartmentId = department.DepartmentId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisDepartment = _db.Departments
        .Include(department => department.JoinDeptStu)
        .ThenInclude(join => join.Student)
        .Include(department => department.JoinDeptCou)
        .ThenInclude(join => join.Course)
        .FirstOrDefault(department => department.DepartmentId == id);
      return View(thisDepartment);
    }

    public ActionResult Edit(int id)
    {
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Title");
      Department thisDepartment = _db.Departments.FirstOrDefault(department => department.DepartmentId == id);
      return View(thisDepartment);
    }

    [HttpPost]
    public ActionResult Edit(Department department)
    {
      _db.Entry(department).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Department thisDepartment = _db.Departments.FirstOrDefault(department => department.DepartmentId == id);
      return View(thisDepartment);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Department thisDepartment = _db.Departments.FirstOrDefault(department => department.DepartmentId == id);
      _db.Departments.Remove(thisDepartment);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}