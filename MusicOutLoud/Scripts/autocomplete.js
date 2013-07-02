
//var PlaylistArray = new Array();
//var PlaylistArrayID = new Array();
$(document).ready(function () {
    $('#tags').keypress(function () {
        //var availableTags = [
        //"ActionScript",
        //"AppleScript",
        //"Asp",
        //"BASIC",
        //"C",
        //"C++",
        //"Clojure",
        //"COBOL",
        //"ColdFusion",
        //"Erlang",
        //"Fortran",
        //"Groovy",
        //"Haskell",
        //"Java",
        //"JavaScript",
        //"Lisp",
        //"Perl",
        //"PHP",
        //"Python",
        //"Ruby",
        //"Scala",
        //"Scheme"
        //    ];
        var ArrayPl = new Array();
        var ArrayPlId = new Array();;
        $.ajax({
            type: "POST",
            url: "/Home/GetPlaylistsByUser",
            dataType: 'json',
            async: false,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    ArrayPl.push(data[i].Name);
                    ArrayPlId.push(data[i].Id);
                }
            },
            error: function(data) {
            alert("error");
            }
        });
        $("#tags").autocomplete({
            //source: PlaylistArray
            source: ArrayPl
        });


        //linkar para a pagina correcta
        $("#goBtn").click(function () {

            var PlaylistName = $("#tags").val();
            if (PlaylistName != "") {
                for (var i = 0; i < ArrayPl.length; ++i) {              
                    if (ArrayPl[i] == PlaylistName) {                       
                        var PlaylistId = ArrayPlId[i];
                        document.location.href = "/PlayList/PlayList/" + PlaylistId;
                    }
                }


            }
        });

    });
});