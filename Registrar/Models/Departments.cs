using System.Collections.Generic;
using System;

namespace Registrar.Models
{
  public class Department
  {
    public Department()
    {
      this.JoinDeptStu = new HashSet<DepartmentStudent>();
      this.JoinDeptCou = new HashSet<DepartmentCourse>();
    }

    public int DepartmentId { get; set; }
    public string Focus { get; set; }
    public virtual ICollection<DepartmentStudent> JoinDeptStu { get; set; }
    public virtual ICollection<DepartmentCourse> JoinDeptCou { get; set; }
  }
}