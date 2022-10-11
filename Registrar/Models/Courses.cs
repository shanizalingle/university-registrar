using System.Collections.Generic;
using System;

namespace Registrar.Models
{
  public class Course
  {
    public Course()
    {
      this.JoinStuCou = new HashSet<StudentCourse>();

      this.JoinDeptCou = new HashSet<DepartmentCourse>();
    }

    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Number { get; set; }
    public virtual ICollection<StudentCourse> JoinStuCou { get;}
    public virtual ICollection<DepartmentCourse> JoinDeptCou { get; set; }

  }
}