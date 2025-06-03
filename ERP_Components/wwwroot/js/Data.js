function getMonthName(monthNumber) {
    // Array of month names
    const monthNames = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    // Ensure the month number is valid
    if (monthNumber < 1 || monthNumber > 12) {
        return "Invalid month number";
    }

    // Return the month name (adjust for zero-based indexing)
    return monthNames[monthNumber - 1];
}


$(function () {
    $('[data-toggle="tooltip"]').tooltip({
        trigger: "hover",
        position: {
            top: "-5px",
            left: "105%",
            using: function (position, feedback) {
                $(this).css(position);
                $("<div>")
                    .addClass("tooltip")
                    .addClass(feedback.vertical)
                    .addClass(feedback.horizontal)
                    .appendTo(this);
            }
        }
    });
    $(document).on('mousedown', "[aria-describedby]", function () {
        $("[aria-describedby]").tooltip('hide');
    });
    $('#ManageProductTable').on('draw.dt', function () {
        $('[data-toggle="tooltip"]').tooltip({
            trigger: "hover"
        });
    });

});


function showLeavePageModal(targetUrl, response) {
    const modal = document.getElementById('TabSwitchModal');
    $('#TabSwitchModal').modal('show');
    modal.dataset.targetUrl = targetUrl;
    if (response != '' || response != undefined) {
        modal.dataset.response = response;
    }
}

// Function to handle image upload
function uploadProfileImage() {
    var fileInput = document.getElementById('fileProfilePicture');

    // Validate if file is selected
    if (fileInput.files && fileInput.files[0]) {
        if (fileInput.files[0].size > 1048576) {
            Swal.fire({
                title: '<span style="color: black; font-size: 16px; margin-left: 5px; font-weight: 600;">' + 'File size exceeds 1MB limit. Please choose a smaller image.' + '</span>',
                icon: 'warning',
                toast: true,
                customClass: {
                    container: 'swal2-container-toaster',
                    popup: 'swal2-popup-toaster'
                },
                autoHeight: true,
                position: 'top',
                timer: 6000,
                showConfirmButton: false,
            });
            return false;
        }

        var fileName = fileInput.files[0].name;
        var fileExtension = fileName.split('.').pop().toLowerCase();

        if (fileExtension !== 'jpg' && fileExtension !== 'jpeg' && fileExtension !== 'png') {
            Swal.fire({
                title: '<span style="color: black; font-size: 16px; margin-left: 5px; font-weight: 600;">' + 'Only JPG, JPEG, and PNG files are allowed.' + '</span>',
                icon: 'warning',
                toast: true,
                customClass: {
                    container: 'swal2-container-toaster',
                    popup: 'swal2-popup-toaster'
                },
                autoHeight: true,
                position: 'top',
                timer: 6000,
                showConfirmButton: false,
            });
            return false;
        }
        let userId = ('191' && '191') || '';
        let userName = $('#UserName').text() || '';

        let obj = {};
        obj.MetaUserId = userId;
        obj.UserProfileFileName = (userId + userName + "UserProfileImage." + fileExtension).replace(/\s+/g, '');
        obj.Action = "Save_User_Profile_Image_Data"
        $.ajax({
            async: false,
            type: "POST",
            url: '/MangerController/ManageUserAndRole/UpdateUsersData',
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (JSON.parse(response) == "Success") {
                    var formData = new FormData();
                    formData.append('profileImage', fileInput.files[0]);
                    formData.append('profileImageFileName', obj.UserProfileFileName);
                    $.ajax({
                        type: "POST",
                        url: '/ManagerController/Admin/SaveUserProfileImage',
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            $('#imageUploadModalForProfile').modal('hide');
                            Swal.fire({
                                title: 'Profile Updated',
                                text: "Your profile image has been successfully updated.",
                                icon: 'success',
                                confirmButtonText: 'ok',
                            }).then((result) => {
                                if (result) {
                                    window.location.reload();
                                }
                            });
                        }
                    });

                }
            }
        })
    }
    else {
        let userId = ('191' && '191') || '';

        let obj = {};
        obj.MetaUserId = userId;
        obj.UserProfileFileName = "";
        obj.Action = "Save_User_Profile_Image_Data"
        $.ajax({
            async: false,
            type: "POST",
            url: '/ManagerController/ManageUserAndRole/UpdateUsersData',
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (JSON.parse(response) == "Success") {
                    var formData = new FormData();
                    formData.append('profileImage', fileInput.files[0]);
                    formData.append('profileImageFileName', obj.UserProfileFileName);
                    $.ajax({
                        type: "POST",
                        url: '/ManagerController/Admin/SaveUserProfileImage',
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            $('#imageUploadModalForProfile').modal('hide');
                            Swal.fire({
                                title: 'Profile Updated',
                                text: "Your profile image has been successfully updated.",
                                icon: 'success',
                                confirmButtonText: 'ok',
                            }).then((result) => {
                                if (result) {
                                    window.location.reload();
                                }
                            });
                        }
                    });
                }
            }
        })
    }
    return true;
}

function removeProfileImage() {
    $('#btnUploadImage').css('display', 'block');
    $('#imgUserProfile').attr("src", "");
    $('#userprofiledelteicon').css('display', 'none');
    document.getElementById('fileProfilePicture').value = '';
    document.querySelector('label[for="fileProfilePicture"]').textContent = 'Choose file';
}


function openImageUploadModal() {
    if (userProfileFileName != "" && userProfileFileName != null) {
        $('#userprofiledelteicon').css('display', 'block');
    } else {
        $('#userprofiledelteicon').css('display', 'none');
        $('#btnUploadImage').css('display', 'none');
        $('#fileProfilePicture').val('');
        var fileLabel = document.querySelector('.custom-file-label');
        fileLabel.innerHTML = "Choose File";
    }
    $('#imageUploadModalForProfile').modal('show');
    var currentImageSrc = document.getElementById('userImageForHeader').src;
    document.getElementById('imgUserProfile').src = currentImageSrc;
}

function closeUploadImagePopUp() {
    var fileLabel = document.querySelector('.custom-file-label');
    fileLabel.innerHTML = "Choose File";
    $('#imageUploadModalForProfile').modal('hide');
    $('#btnUploadImage').css('display', 'none');
}