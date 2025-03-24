// JavaScript source code
var anyPrint = {
    isPrinting: false,
    print: function (divId, documentTitle) {
        var aP = this;

        if (aP.isPrinting)
            return;

        aP.isPrinting = true;

        try {
            if ($('iframe[name="print_frame"]').length === 0) {
                var printElement = document.createElement('iframe');
                printElement.setAttribute('name', 'print_frame');
                printElement.setAttribute('width', 0);
                printElement.setAttribute('height', 0);
                printElement.setAttribute('frameborder', 0);
                printElement.setAttribute('id', 'frameToPrint');
                printElement.setAttribute('src', 'about:blank');
                document.body.appendChild(printElement);
            }

            $('link').each(function (i, item) {
                appendCssToFrame(item);
            });

            var logo = $('.logo').length < 1
                ? ""
                : '<div class="row"><div class="col-xs-12">' + $('.logo')[0].innerHTML + '</div></div>';
            var elementBody = logo + document.getElementById(divId).innerHTML;

            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!

            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            var todayString = dd + '-' + mm + '-' + yyyy;

            var originalTitle = document.title;
            document.title = documentTitle + ' ' + todayString;

            //var printDivCSS = '<link href="myprintstyle.css" rel="stylesheet" type="text/css">';
            //$("#frameToPrint").contents().find("#htmlToPrint").find('head').append(links);
            window.frames["print_frame"].document.body.innerHTML = elementBody;
            window.frames["print_frame"].document.childNodes[0].id = "htmlToPrint";
            $("#frameToPrint").contents().find("#htmlToPrint").find('[showOnPrint="hidden"]').remove();
            $("#frameToPrint").contents().find("#htmlToPrint").find('[showOnPrint="visible"]').removeClass('hidden');
            window.frames["print_frame"].document.title = documentTitle;
            window.frames["print_frame"].window.focus();

            setTimeout(function () {
                window.frames["print_frame"].window.print();
                document.title = originalTitle;
                aP.isPrinting = false;
            },
                300);
        } catch (e) {
            console.log(e);
            aP.isPrinting = false;
        }
    },
    appendCssToFrame: function (cssElement) {
        var linkElement = document.createElement('link');
        linkElement.setAttribute('rel', $(cssElement).attr('rel'));
        linkElement.setAttribute('type', 'text/css');
        linkElement.setAttribute('href', $(cssElement).attr('href'));

        window.frames["print_frame"].document.head.appendChild(linkElement);
        return linkElement.outerHTML;
    }
};