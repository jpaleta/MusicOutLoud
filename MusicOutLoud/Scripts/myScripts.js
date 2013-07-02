
//function checkSubmit()
//{
//    if (1 == 1)
//    {
//        alert("yo");
//        return true;
//    }
//    else
//    {
//                return false;
//    }
//}

//$("#login_submitBtn").click(function () {
//    alert("hello adp!");
//});


//$(document).ready(function () {
//    $("#login_form").validate({
//        rules: {
//            userId: "required"
//        },
//        messages: {
//            userId: "Please specify your name"
//        }
//    })
//});

$("#login_form").submit(function () {

    var isFormValid = true;

    var user = $("#userId").val();
    var pass = $("#passId").val();

    if ($.trim(user) === "") {
        $("#error_user").show();
        isFormValid = false;
    }
    else {
        $("#error_user").hide();
    }
    if ($.trim(pass) === "") {
        $("#error_pass").show();
        isFormValid = false;
    }
    else {
        $("#error_pass").hide();
    }

    //var isFormValid = true;
    //var user = $("#userId").val();
    //var pass = $("#passId").val();

    //if ($.trim(user) === "") {
    //    //alert($("#userId").val());
    //    if ($.trim(pass) !== "") {
    //        $("#error_pass").hide();
    //    }
    //    $("#error_user").show();
    //    isFormValid = false;
    //}

    //if ($.trim(pass) === "")
    //{
    //    if ($.trim(user) !== "") {
    //        $("#error_user").hide();
    //    }
    //    $("#error_pass").show();
    //    isFormValid = false;
    //}

    //if ($.trim(user) !== "" && $.trim(pass) !== "")
    //{
    //    //alert($("#userId").val());
    //    $("#error_user").hide();
    //    $("#error_pass").hide();
    //    isFormValid = true;
    //}

    return isFormValid;
});

$("#register_form").submit(function () {

    var isFormValid = true;

    var user = $("#userId").val();
    var name = $("#userName").val();
    var email = $("#userEmail").val();
    var pass = $("#userPass").val();
    var pass2 = $("#userPass2").val();

    if ($.trim(user) === "") {
        $("#error_user").show();
        isFormValid = false;
    }
    else {
        $("#error_user").hide();
    }

    if ($.trim(name) === "") {
        $("#error_name").show();
        isFormValid = false;
    }
    else {
        $("#error_name").hide();
    }

    if ($.trim(email) === "") {
        $("#error_email").show();
        isFormValid = false;
    }
    else {
        $("#error_email").hide();

        if (!isValidEmailAddress(email)) {
            $("#error_emailAt").show();
            isFormValid = false;
        }
        else
            $("#error_emailAt").hide();
    }

    if ($.trim(pass) === "") {
        $("#error_pass").show();
        isFormValid = false;
    }
    else {
        $("#error_pass").hide();

        if (pass.length < 6) {
            $("#error_passLen").show();
            isFormValid = false;
        }
        else {
            $("#error_passLen").hide();
        }
    }

    if ($.trim(pass2) === "") {
        $("#error_pass2").show();
        isFormValid = false;
    }
    else {
        $("#error_pass2").hide();
    }

    if ($.trim(pass) !== "" && $.trim(pass2) !== "" && $.trim(pass) === $.trim(pass2))  {
        $("#error_passEquals").hide();  
    }
    else {
        if ($.trim(pass) !== "" && $.trim(pass2) !== "") {
            $("#error_passEquals").show();
            isformValid = false;
        }
    }


    return isFormValid;
});

$("#list_form").submit(function () {
    var isFormValid = true;

    var listName = $("#listName").val();

    if ($.trim(listName) === "") {
        $("#error_list").show();
        isFormValid = false;
    }
    else {
        $("#error_list").hide();
    }
    //$.ajax({
    //    type: "POST",
    //    url: "/Home/SearchPlaylistByUser",
    //    dataType: 'json',
    //    data: { newPlayListName: $("#listName").val() },
    //    success: function (response) {
    //        alert(response.Success)
    //        if (response.Success == true) {
    //            $("#error_list2").hide();
    //            isFormValid = true;
    //            alert("True");
    //        }
    //        else {
    //            $("#error_list2").show();
    //            isFormValid = false;
    //            alert("FALSE");
    //        }
    //    },
    //    error: function (response) { alert("Connection Failed. Please Try Again e tambem tem o response de :" + response.Success); }
    //});

    return isFormValid;
});




function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    return pattern.test(emailAddress);
};


$(document).ready(function () {
    $('#createPlaylistbtn').click(function () {
        //var newPlayListName = $("#listName").val();
        //$.post('@Url.Action("SearchPlaylistByUser","Home")', { 'newPlayListName': $("#listName").val() }, function () {
        //    alert("algo");
        //});

        //$.get("@Url.Action('SearchPlaylistByUser')",
        //    { newPlayListName: $("#listName").val() },
        //       function (data) {
        //           alert(data);
        //       }
        //     );
        var aux = "1";
        $.ajax({
            type:    "POST", 
            url:     "/Home/SearchPlaylistByUser",
            dataType: 'json',
            async: false,
            data:    { newPlayListName: $("#listName").val() },
            success: function (response) {
                if (response.Success == true) {
                    $("#error_list").hide();
                    $("#error_list2").hide();
                }
                else {
                    $("#error_list").hide();
                    $("#error_list2").show();
                    aux = "0";
                }
            }

        });
        if (aux == "0")
            return false;

 
        //var params = { newPlayListName: $("#listName").val() };
        //$.ajax({
        //    url: "Home/SearchPlaylistByUser",
        //    type: "get",
        //    data: { newPlayListName: $("#listName").val() },
        //    success: function (response, textStatus, jqXHR) {
        //        if (response.IsExisting) {
        //            // User name is existing already, you can display a message to the user
        //            //$("#regTitle").html("Already Exists")
        //            alert("Exists");
        //        }
        //        else {
        //            // User name is not existing
        //            //$("#regTitle").html("Available")
        //            alert("Available");
        //        }
        //    },
        //    error: function (jqXHR, textStatus, errorThrown) {
        //        alert("error");
        //    },
        //    // callback handler that will be called on completion
        //    // which means, either on success or error
        //    complete: function () {
        //    }
        //});
    });
});
