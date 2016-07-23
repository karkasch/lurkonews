$(document).ready(function () {
    $("#btn").click(function () {
        var url = $("#txt").val();
        if (url != null && url != "") {
            if (!url.endsWith("/"))
                url += "/";

            url = encodeURIComponent(url);
            url = url.replace("/", "%2F");
            url = url.replace(":", "%3A");

            window.location.href = "/read/" + url;
        }
    });

    //$("#cntnt").on("load", function () {
    //    var s = $(this).contents().find("body").html();

    //    s = s.replace("zak", "хуЙ!");

    //    $(this).contents().find("body").html(s);
    //});
});