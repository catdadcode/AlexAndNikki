// Functions

function preload(arrayOfImages)
{
    $(arrayOfImages).each(function ()
    {
        $('<img/>')[0].src = this;
        // Alternatively you could use:
        // (new Image()).src = this;
    });
}

function hideEverything()
{
    $('#banner img').hide();
    var total = 0;
    var elements = new Array();
    $('#banner').find('.bannerImage').each(function (elem)
    {
        $(this).attr('id', 'banImg' + elem);
        elements[elem] = $(this).attr('id');
        total++;
    });
    $('#banner')
    .data('elementsArray', elements)
    .data('totalElements', total);
}

function showEverything()
{
    var i = 0;
    var total = $('#banner').data('totalElements');
    var elements = $('#banner').data('elementsArray');
    function rndAnim()
    {
        if (i <= total)
        {
            $('#' + elements[i]).fadeIn(3000);
            i++;
            setTimeout(rndAnim, 700);
        }
        else
        {
            $('#AlexAndNikkiBanner').show('drop', { direction: 'up' }, 1000);
        }
    }
    rndAnim();
}

// DOM Ready
$(function ()
{
    //hideEverything();
    if ($.readCookie('visitedToday') == null)
    {
        hideEverything();
        $('#frame').hide();
    }
});

// Everything is Loaded
$(window).load(function () {
    if ($.readCookie('visitedToday') == null) {
        $('#frame').fadeIn('slow');
        showEverything();
        $.setCookie('visitedToday', 'true', { duration: 1 });
    }
});