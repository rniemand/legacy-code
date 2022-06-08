$(document).ready(function() {
    $('input#shortenUrl').keyup(function() {
         validateUrl($(this).val());
    });
    
    $('input#shortenUrl').blur(function() {
         validateUrl($(this).val());
    });

    $('button#buttonShorten').click(function () {
        shortenUrl($('input#shortenUrl').val());
    });
});


function validateUrl(url) {
    $('button#buttonShorten')
        .attr('disabled', '')
        .removeClass('btn-success')
        .addClass('btn-inverse');

    if (url.length === 0) {
        return;
    }

    if (/.{2,5}:\/\/.{2,}(:\d+|)(\/|).*$/i.test(url)) {
        $('button#buttonShorten')
            .removeAttr('disabled')
            .addClass('btn-success')
            .removeClass('btn-inverse');
    }
}

function shortenUrl(url) {
    $('div#shortenFlowDiv').hide();

    $('div#shortenMessageDiv')
        .html(
            $('<div />')
                .addClass('alert alert-success')
                .css('text-align', 'center')
                .append($('<i />').addClass('icon-spinner icon-spin icon-2x'))
                .append($('<span />').addClass('shortSpan').html('Shrinking your URL'))
        )
        .fadeIn(500);

    // Fire off the shorten command
    var postData = { url: url };
    $.post('/Home/ShortenUrl', postData, function (json) {
        $('div#shortenMessageDiv')
            .removeClass('alert-success')
            .addClass('alert alert-info')
            .css('text-align', 'center')
            .html('')
            .append($('<i />').addClass('icon-link icon-2x'))
            .append(
                $('<span />').addClass('shortSpan')
                .append($('<a />').attr('href', json.ShortUrlFull).html(json.ShortUrlFull))
            );

        $('div#qrCodeDiv').qrcode({ text: json.ShortUrlFull });
        $('div#qrCodeDiv')
            .append($('<div />').append(
                $('<button />')
                    .addClass('btn btn-primary startOver')
                    .html('Start Over')
                    .click(function () { window.location.href = '/'; })
            ))
            .fadeIn('slow');

        
        /*
        jQuery('#qrcodeCanvas').qrcode({
		text	: "http://jetienne.com"
	});	
        */

        console.log(json);

    }).fail(function() {
        alert("error");
    });

}
