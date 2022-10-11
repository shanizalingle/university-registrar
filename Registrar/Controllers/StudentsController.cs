using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Registrar.Controllers
{
  public class StudentsController : Controller
  {
    private readonly RegistrarContext _db;

    public StudentsController(RegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Student> model = _db.Students.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Title");
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Focus");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Student student, int CourseId, int DepartmentId)
    {
      _db.Students.Add(student);
      _db.SaveChanges();
      if (CourseId != 0)
      {
        _db.StudentCourse.Add(new StudentCourse() { CourseId = CourseId, StudentId = student.StudentId });
        _db.SaveChanges();
      }
      if (DepartmentId != 0)
      {
        _db.DepartmentStudent.Add(new DepartmentStudent() { DepartmentId = DepartmentId, StudentId = student.StudentId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisStudent = _db.Students
        .Include(student => student.JoinStuCou)
        .ThenInclude(join => join.Course)
        .Include(student => student.JoinDeptStu)
        .ThenInclude(join => join.Department)
        .FirstOrDefault(student => student.StudentId == id);
      return View(thisStudent);
    }

    public ActionResult Edit(int id)
    {
      Student thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Focus");
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Title");
      return View(thisStudent);
    }

    [HttpPost]
    public ActionResult Edit(Student student, int CourseId, int DepartmentId)
    {
      if (CourseId != 0)
      {
        _db.StudentCourse.Add(new StudentCourse() { CourseId = CourseId, StudentId = student.StudentId });
      }
      if (DepartmentId != 0)
      {
        _db.DepartmentStudent.Add(new DepartmentStudent() { DepartmentId = DepartmentId, StudentId = student.StudentId });
      }
      _db.Entry(student).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Student thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      return View(thisStudent);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Student thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      _db.Students.Remove(thisStudent);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddDepartment(int id)
    {
      var thisStudent = _db.Students.FirstOrDefault(student => student.StudentId == id);
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "Focus");
      return View(thisStudent);
    }

    [HttpPost]
    public ActionResult AddDepartment(Student student, int DepartmentId)
    {
      Console.WriteLine(student.StudentId);
      Console.WriteLine(DepartmentId);
      if (DepartmentId != 0)
      {
        _db.DepartmentStudent.Add(new DepartmentStudent() { StudentId = student.StudentId, DepartmentId = DepartmentId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteDepartment(int joinId)
    {
      var joinEntry = _db.DepartmentStudent.FirstOrDefault(entry => entry.DepartmentStudentId == joinId);
      _db.DepartmentStudent.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}