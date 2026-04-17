namespace LMS.Models.HelperModels
{
  public class SimpleDepartment
  {
    public string subject { get; set; } = null!;
    public string dname { get; set; } = null!;
    public List<SimpleCourse> courses { get; set; } = new();
  }
}
