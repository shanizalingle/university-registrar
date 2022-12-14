using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Registrar.Controllers
{
  public class CoursesController : Controller
  {
    private readonly RegistrarContext _db;

    public CoursesController(RegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Courses.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Focus");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Course course, int StudentId, int DepartmentId, string Title, string Number)
    {
      _db.Courses.Add(course);
      _db.SaveChanges();
      if (StudentId != 0)
      {
        _db.StudentCourse.Add(new StudentCourse() { StudentId = StudentId, CourseId = course.CourseId });
        _db.SaveChanges();
      }
      if (DepartmentId != 0)
      {
        _db.DepartmentCourse.Add(new DepartmentCourse() { DepartmentId = DepartmentId, CourseId = course.CourseId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisCourse = _db.Courses
        .Include(course => course.JoinStuCou)
        .ThenInclude(join => join.Student)
        .Include(course => course.JoinDeptCou)
        .ThenInclude(join => join.Department)
        .FirstOrDefault(course => course.CourseId == id);
      return View(thisCourse);
    }

    public ActionResult Edit(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Focus");
      return View(thisCourse);
    }

    [HttpPost]
    public ActionResult Edit(Course course, int StudentId, int DepartmentId)
    {
      if (StudentId != 0)
      {
        _db.StudentCourse.Add(new StudentCourse() { StudentId = StudentId, CourseId = course.CourseId });
      }
      if (DepartmentId != 0)
      {
        _db.DepartmentCourse.Add(new DepartmentCourse() { DepartmentId = DepartmentId, CourseId = course.CourseId });
      }
      _db.Entry(course).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddStudent(int id)
    {
      var thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      return View(thisCourse);
    }

    [HttpPost]
    public ActionResult AddStudent(Course course, int StudentId)
    {
      if (StudentId != 0)
      {
        _db.StudentCourse.Add(new StudentCourse() { StudentId = StudentId, CourseId = course.CourseId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult AddDepartment(int id)
    {
      var thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Focus");
      return View(thisCourse);
    }

    [HttpPost]
    public ActionResult AddDepartment(Course course, int DepartmentId)
    {
      if (DepartmentId != 0)
      {
        _db.DepartmentCourse.Add(new DepartmentCourse() { CourseId = course.CourseId, DepartmentId = DepartmentId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      return View(thisCourse);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      _db.Courses.Remove(thisCourse);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteStudent(int joinId)
    {
      var joinEntry = _db.StudentCourse.FirstOrDefault(entry => entry.StudentCourseId == joinId);
      _db.StudentCourse.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteDepartment(int joinId)
    {
      var joinEntry = _db.DepartmentCourse.FirstOrDefault(entry => entry.DepartmentCourseId == joinId);
      _db.DepartmentCourse.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}