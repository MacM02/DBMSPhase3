using LMS.Models;
using LMS.Models.HelperModels;
using LMS.Models.LMSModels;

namespace LMS.Models.HelperModels
{
  public partial class Catalog
  {
    public List<SimpleDepartment> department { get; set; } = new();
  }
}
