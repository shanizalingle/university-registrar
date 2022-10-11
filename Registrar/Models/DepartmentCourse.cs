namespace Registrar.Models
{
  public class DepartmentCourse
  {       
    public int DepartmentCourseId { get; set; }
    public int DepartmentId  { get; set; }
    public int CourseId { get; set; }
    public virtual Department Department { get; set; }
    public virtual Course Course { get; set; }
  }
}