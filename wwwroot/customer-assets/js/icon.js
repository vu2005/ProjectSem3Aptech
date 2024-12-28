document.addEventListener("DOMContentLoaded", function () {
  // Select elements
  const BtnShowPass = document.querySelector("#show-pass");
  const inputPass = document.querySelector("#input-pass");
  const BtnShowConfirmPass = document.querySelector("#show-confirm-pass");
  const inputConfirmPass = document.querySelector("#input-confirm-pass");

  // Function to toggle password visibility
  function togglePasswordVisibility(button, input) {
    if (button && input) {
      button.addEventListener("click", function () {
        // Toggle 'active' class on the button
        button.classList.toggle("active");

        // Toggle the input type between password and text
        input.type = input.type === "password" ? "text" : "password";
      });
    }
  }

  // Apply toggle function to both password fields
  togglePasswordVisibility(BtnShowPass, inputPass);
  togglePasswordVisibility(BtnShowConfirmPass, inputConfirmPass);
});
