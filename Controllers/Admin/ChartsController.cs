using Microsoft.AspNetCore.Mvc;

namespace CarInsuranceManage.Controllers.Admin
{
    public class ChartsController : Controller
    {
        public IActionResult Morris()
        {
            return View("~/Views/Admin/Charts/Morris.cshtml");  // Đường dẫn tuyệt đối
        }

    }
}
