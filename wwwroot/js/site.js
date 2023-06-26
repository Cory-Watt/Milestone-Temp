$(function () {
    console.log("Page is ready");
});
$(document).on("mousedown", ".game-button", function (event) {
    event.preventDefault();
    switch (event.which) {
        case 1:
            var buttonNumber = $(this).val();
            console.log("Button Number " + buttonNumber + " was left clicked");
            doButtonUpdate(buttonNumber, "/minesweeper/ShowOneButton");
            break;
        case 2:
            alert('Middle mouse button is pressed');
            break;
        case 3:
            var buttonNumber = $(this).val();
            console.log("Button Number " + buttonNumber + " was right clicked");
            doButtonUpdate(buttonNumber, "/minesweeper/RightClickShowOneButton");
            break;
        default:
            alert('Nothing');

    }
});

$(document).bind("contextmenu", function (e) {
    e.preventDefault();
    console.log("Right click. Prevent context menu from showing.")
});

function doButtonUpdate(buttonNumber, urlString) {
    $.ajax({
        datatype: "json",
        method: 'POST',
        url: urlString,
        data: {
            "buttonNumber": buttonNumber
        },
        success: function (data) {

            console.log(data);
           // $("#" + buttonNumber).html(data);
           $("#" + buttonNumber).html(data.part1);
           $("#messageArea").html(data.part2);
        }
    });
};