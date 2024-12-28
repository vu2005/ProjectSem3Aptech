using Microsoft.AspNetCore.Mvc;

namespace CarInsuranceManage.Controllers.Admin
{
    public class FormsController : Controller
    {
        public IActionResult Basic_Form()
        {
            return View("~/Views/Admin/Forms/Basic_Form.cshtml");  // Đường dẫn tuyệt đối đến view Basic_Form
        }

        public IActionResult Form_Validation()
        {
            return View("~/Views/Admin/Forms/Form_Validation.cshtml");  // Đường dẫn tuyệt đối đến view Form_Validation
        }

        public IActionResult Step_Form()
        {
            return View("~/Views/Admin/Forms/Step_Form.cshtml");  // Đường dẫn tuyệt đối đến view Step_Form
        }

        public IActionResult Editor()
        {
            return View("~/Views/Admin/Forms/Editor.cshtml");  // Đường dẫn tuyệt đối đến view Editor
        }

        public IActionResult Picker()
        {
            return View("~/Views/Admin/Forms/Picker.cshtml");  // Đường dẫn tuyệt đối đến view Picker
        }
    }
}
