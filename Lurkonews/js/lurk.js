$(document).ready(function () {
    $("#btn").click(function () {
        var url = $("#txt").val();
        if (url != null && url != "") {
            url = url.trim();
            var re = new RegExp(/[а-яa-z0-9-_]+\.[a-z]{2,4}[/\?а-яa-z0-9-_=&]*/gi);

            if (!re.test(url))
                return;

            if (!url.endsWith("/"))
                url += "/";

            url = encodeURIComponent(url);
            url = url.replace("/", "%2F");
            url = url.replace(":", "%3A");

            window.location.href = "/read/" + url;
        }
    });
});