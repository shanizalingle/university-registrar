using System.Collections.Generic;
using System;

namespace Registrar.Models
{
  public class Student
  {
    public Student()
    {
      this.JoinStuCou = new HashSet<StudentCourse>();

      this.JoinDeptStu = new HashSet<DepartmentStudent>();
    }

    public int StudentId { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public virtual ICollection<StudentCourse> JoinStuCou { get; set; }
    public virtual ICollection<DepartmentStudent> JoinDeptStu { get; set; }
  }
}