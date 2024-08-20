// فتح نافذه منبثقة لاضافة برنامج تلفزيوني 
document.addEventListener("DOMContentLoaded", function () {
    var openPopupBtn = document.getElementById("openPopupBtn_Add");
    var myModal = document.getElementById("popUp_Add");

    openPopupBtn.addEventListener("click", function () {
        var modal = new bootstrap.Modal(myModal);
        modal.show();
    });
});



// PopupUpdateTVShow تنسيق اللغة من أجل 
document.addEventListener('DOMContentLoaded', function () {
    //  .btn-primary البحث عن جميع العناصر في الصفحة التي تحتوي على
    document.querySelectorAll('.btn-primary').forEach(button => {
        button.addEventListener('click', function () {
            var modalId = this.id.split('_')[1]; //  TVShowId اخراج 
            var languageInput = document.getElementById('languageInput_' + modalId);
            var languagesDropdown = document.getElementById('languagesDropdown_' + modalId);

            if (languageInput.value.trim() !== "") {
                var newOption = document.createElement('option');
                newOption.value = languageInput.value;
                newOption.text = languageInput.value;
                languagesDropdown.add(newOption);
                languageInput.value = ""; // تهيئة حقل الادخال
            }
        });
    });
});    



// تنسيق إضافة لغة 
document.getElementById('addLanguageBtn').addEventListener('click', function () {
    var languageInput = document.getElementById('languageInput');
    var languagesDropdown = document.getElementById('languagesDropdown');

    if (languageInput && languagesDropdown) {
        var inputValue = languageInput.value.trim(); // حذف المسافات

        if (languageInput.value.trim() !== "") {
            var newOption = document.createElement('option');
            newOption.value = languageInput.value;
            newOption.text = languageInput.value;
            languagesDropdown.add(newOption);
            languageInput.value = ""; // تهيئة حقل الادخال
        }
    }
});



// لازالة رسالة الخطأ عند الضغط على حقل الادخال
document.getElementById('languageInput').addEventListener('focus', function () {
    var languageError = document.getElementById('languageError');
    if (languageError) {
        languageError.textContent = ''; // مسح رسالة الخطأ عند النقر على حقل النص
    }
});



// إخفاء رسالة نجاح تسجيل الدخول بعد 4 ثوانٍ
setTimeout(function () {
    var successMessage = document.getElementById('successMessage');
    if (successMessage) {
        successMessage.style.display = 'none';
    }
}, 4000);

