using System.Collections.Generic;
using System;

namespace Registrar.Models
{
  public class Course
  {
    public Course()
    {
      this.JoinEntities = new HashSet<StudentCourse>();
    }

    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Number { get; set; }
    public virtual ICollection<StudentCourse> JoinEntities { get;}
  }
}