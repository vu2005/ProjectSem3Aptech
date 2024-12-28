using Microsoft.AspNetCore.Mvc;

namespace CarInsuranceManage.Controllers.Admin
{
    public class TableController : Controller
    {
        public IActionResult Basic_Table()
        {
            return View("~/Views/Admin/Table/Basic_Table.cshtml");  // Đường dẫn tuyệt đối đến view Basic_Table
        }

        public IActionResult Data_Table()
        {
            return View("~/Views/Admin/Table/Data_Table.cshtml");  // Đường dẫn tuyệt đối đến view Data_Table
        }
    }
}
