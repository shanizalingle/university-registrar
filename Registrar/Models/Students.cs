using System.Collections.Generic;
using System;

namespace Registrar.Models
{
  public class Student
  {
    public Student()
    {
      this.JoinEntities = new HashSet<StudentCourse>();
    }

    public int StudentId { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public virtual ICollection<StudentCourse> JoinEntities { get; set; }
  }
}