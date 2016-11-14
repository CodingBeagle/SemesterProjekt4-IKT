window.onload = function onloadHandler()
{
    //document.getElementById('searchfield').value = "hey";
    //document.getElementById("searchButton").onclick = searchItems;
}

function point() {
    this.coordx = 0;
    this.coordy = 0;
}

function drawPinIcon(canvas, x,y, r) {
    var ctx = canvas.getContext('2d');
    ctx.strokeStyle = "#FF0000";
    ctx.lineWidth = r*0.1 ;
    var ycirclepos = y - Math.sqrt(r * r + r * r);

    // Outer arc
    ctx.beginPath();
    ctx.arc(x, ycirclepos, r, 1 / 4 * Math.PI, 3 / 4 * Math.PI, true);
    //Point
    ctx.lineTo(x, y);
    ctx.lineTo(x + Math.cos(1 / 4 * Math.PI) * r, ycirclepos + Math.cos(1 / 4 * Math.PI) * r);
    ctx.closePath();
    ctx.fillStyle = "#3a3a3a";
    ctx.fill();
    ctx.strokeStyle = "#3a3a3a";
    ctx.stroke();

    // Inner circle
    ctx.beginPath();
    //ctx.fillStyle = "#FD5F00";
    ctx.fillStyle = "#0FF";
    ctx.arc(x, ycirclepos, r * 0.7, 0, 2 * Math.PI, true);
    //var rectsize = r*1.4;
    //ctx.rect(x - rectsize/2, ycirclepos - rectsize/2, rectsize, rectsize);
    ctx.closePath();  
    ctx.fill();
    ctx.stroke();
}


function searchItems(itemName) {
    alert(itemName);

    FindAllItemPlacements(itemName);
}

function searchbarOnKeyUp(key) {
    if (key.keyCode === 13) //On enter key up
    {
        searchItems();
    }
}

function FindAllItemPlacements(itemName) {
    var itemPlacements = $("#" + itemName + "Placements").children("span");

    var canvas = document.getElementById('floorplan');

    for (var i = 0; i < itemPlacements.length; i++) {
        var xCoordinate = document.getElementById(itemPlacements[i].id).innerHTML;
        var yCoordinate = document.getElementById(itemPlacements[i + 1].id).innerHTML;

        drawPinIcon(canvas, xCoordinate, yCoordinate, 25);
        console.log(xCoordinate);
        console.log(yCoordinate);

        i++;
    }
}