using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Moto_Web.Models.VM
{
    public class ModelVM
    {
        public ModelVM()
        {
            ModelClass = new Model();
        }

        public int CompanyId { get; set; }
        public Model ModelClass { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}
