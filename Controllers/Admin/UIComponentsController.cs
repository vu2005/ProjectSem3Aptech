using Microsoft.AspNetCore.Mvc;

namespace CarInsuranceManage.Controllers.Admin
{
    public class UIComponentsController : Controller
    {
        public IActionResult Accordion()
        {
            return View("~/Views/Admin/UIComponents/Accordion.cshtml");  // Đường dẫn tuyệt đối đến view Accordion
        }

        public IActionResult Alert()
        {
            return View("~/Views/Admin/UIComponents/Alert.cshtml");  // Đường dẫn tuyệt đối đến view Alert
        }

        public IActionResult Badge()
        {
            return View("~/Views/Admin/UIComponents/Badge.cshtml");  // Đường dẫn tuyệt đối đến view Badge
        }

        public IActionResult Button()
        {
            return View("~/Views/Admin/UIComponents/Button.cshtml");  // Đường dẫn tuyệt đối đến view Button
        }

        public IActionResult Button_Group()
        {
            return View("~/Views/Admin/UIComponents/Button_Group.cshtml");  // Đường dẫn tuyệt đối đến view Button_Group
        }

        public IActionResult Cards()
        {
            return View("~/Views/Admin/UIComponents/Cards.cshtml");  // Đường dẫn tuyệt đối đến view Cards
        }

        public IActionResult Carousel()
        {
            return View("~/Views/Admin/UIComponents/Carousel.cshtml");  // Đường dẫn tuyệt đối đến view Carousel
        }

        public IActionResult Dropdown()
        {
            return View("~/Views/Admin/UIComponents/Dropdown.cshtml");  // Đường dẫn tuyệt đối đến view Dropdown
        }

        public IActionResult List_Group()
        {
            return View("~/Views/Admin/UIComponents/List_Group.cshtml");  // Đường dẫn tuyệt đối đến view List_Group
        }

        public IActionResult Modal()
        {
            return View("~/Views/Admin/UIComponents/Modal.cshtml");  // Đường dẫn tuyệt đối đến view Modal
        }

        public IActionResult Pagination()
        {
            return View("~/Views/Admin/UIComponents/Pagination.cshtml");  // Đường dẫn tuyệt đối đến view Pagination
        }

        public IActionResult Popover()
        {
            return View("~/Views/Admin/UIComponents/Popover.cshtml");  // Đường dẫn tuyệt đối đến view Popover
        }

        public IActionResult Progressbar()
        {
            return View("~/Views/Admin/UIComponents/Progressbar.cshtml");  // Đường dẫn tuyệt đối đến view Progressbar
        }

        public IActionResult Tab()
        {
            return View("~/Views/Admin/UIComponents/Tab.cshtml");  // Đường dẫn tuyệt đối đến view Tab
        }

        public IActionResult Typography()
        {
            return View("~/Views/Admin/UIComponents/Typography.cshtml");  // Đường dẫn tuyệt đối đến view Typography
        }
    }
}
