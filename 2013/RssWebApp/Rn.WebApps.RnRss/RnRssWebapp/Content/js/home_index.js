$(document).ready(function() {
    $('i#getRssFeedIcon').click(function () {
        getRssFeed($('input#rssFeedUrl').val());
    });
});


function getRssFeed(feedUrl) {
    var $div = $('div#rssInfoDiv');

    var url = Rn.URL.append('RssFeed/GetFeedContents');
    var postData = {
        'feedUrl': feedUrl
    };

    $div.html(Rn.apps.FontAwesome.icons.spinner('Looking up feed information...'));

    $.post(url, postData, function (json) {
        var $table =
            $('<table />')
                .addClass('table table-striped table-bordered table-hover table-condensed')
                .append(
                    $('<thead />').append(
                        $('<tr />')
                            .append($('<th />').html('Name'))
                            .append($('<th />').html('Value'))
                    )
                )
                .append(
                    $('<tbody />')
                        .append($('<tr />')
                            .append($('<td />').html('Title'))
                            .append($('<td />').html(json.Channel.Title)))
                        .append($('<tr />')
                            .append($('<td />').html('Description'))
                            .append($('<td />').html(json.Channel.Description)))
                        .append($('<tr />')
                            .append($('<td />').html('Language'))
                            .append($('<td />').html(json.Channel.Language)))
                        .append($('<tr />')
                            .append($('<td />').html('LastBuildDate'))
                            .append($('<td />').html(json.Channel.LastBuildDate)))
                );

        console.log(json.Channel);

        $div.html($table);
    });

    //$.post()

}