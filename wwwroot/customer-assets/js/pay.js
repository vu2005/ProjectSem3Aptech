function updatePrice() {
    // Lấy giá trị của các trường từ form
    const year = parseInt(document.getElementById('insurance_years').value) || 1; // Nếu người dùng không nhập năm, mặc định là 1
    const package = document.getElementById('insurance_package').value; // Gói bảo hiểm đã chọn

    // Gán mức phí theo từng gói bảo hiểm
    let pricePerYear = 0;
    switch (package) {
        case 'basic':
            pricePerYear = 50; // Gói Basic $50/năm
            break;
        case 'standard':
            pricePerYear = 70; // Gói Standard $70/năm
            break;
        case 'premium':
            pricePerYear = 90; // Gói Premium $90/năm
            break;
        default:
            pricePerYear = 0; // Mặc định nếu không chọn gói bảo hiểm
    }

    // Tính toán tổng giá bảo hiểm
    const totalPrice = pricePerYear * year;

    // Cập nhật hiển thị giá trên giao diện
    document.getElementById('insurance_price_display').innerText = `$${totalPrice.toFixed(2)}`; // Hiển thị giá
    document.getElementById('insurance_price').value = totalPrice.toFixed(2); // Cập nhật giá vào input ẩn
}

// Gọi hàm updatePrice khi người dùng thay đổi thông tin
document.addEventListener('DOMContentLoaded', function() {
    // Gọi hàm updatePrice ngay khi load trang để cập nhật giá mặc định
    updatePrice();

    // Thêm sự kiện để gọi hàm updatePrice mỗi khi có thay đổi trong form
    document.getElementById('insurance_package').addEventListener('change', updatePrice);
    document.getElementById('insurance_years').addEventListener('input', updatePrice);
});
