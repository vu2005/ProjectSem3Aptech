document.getElementById("search-input").addEventListener("input", function () {
  var searchTerm = this.value.toLowerCase(); // Lấy giá trị tìm kiếm và chuyển thành chữ thường
  var menuItems = document.querySelectorAll("#menu li"); // Lấy tất cả các mục trong menu

  // Lặp qua tất cả các mục menu và kiểm tra xem tên mục có chứa từ khóa tìm kiếm không
  menuItems.forEach(function (item) {
    var menuText = item.textContent || item.innerText; // Lấy nội dung văn bản của mục menu
    if (menuText.toLowerCase().includes(searchTerm)) {
      item.style.display = ""; // Hiển thị mục nếu nó khớp với từ khóa
    } else {
      item.style.display = "none"; // Ẩn mục nếu không khớp
    }
  });
});
