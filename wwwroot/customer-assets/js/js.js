// Script để hiển thị form trả lời và thêm @username vào trong textarea khi người dùng nhấn "Reply"
document.querySelectorAll('.reply-btn').forEach(button => {
    button.addEventListener('click', function () {
        const commentId = this.getAttribute('data-comment-id');
        const username = this.getAttribute('data-username');  // Lấy username từ data-username
        const replyForm = document.getElementById('reply-form-' + commentId);
        const textarea = replyForm.querySelector('textarea');

        // Thêm @username vào textarea khi nhấn Reply
        textarea.value = '@' + username + ' ';

        // Hiển thị form trả lời
        replyForm.style.display = replyForm.style.display === 'none' ? 'block' : 'none';
    });
});