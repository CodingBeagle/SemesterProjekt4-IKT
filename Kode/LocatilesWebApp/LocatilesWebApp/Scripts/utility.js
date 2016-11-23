window.onload = function() {
    console.log("onload called!");
    ResizeItemCol();
}

window.onresize = function onResizeHandler() {
    console.log("onresize called");
    ResizeItemCol();
}

function point() {
    this.coordx = 0;
    this.coordy = 0;
}

function drawPinIcon(canvas, x,y, r) {
    var ctx = canvas.getContext('2d');
    ctx.strokeStyle = "#FF0000";
    ctx.lineWidth = r*0.1 ;

    // Outer arc
    ctx.beginPath();
    ctx.arc(x, y, r, 0, 2 * Math.PI, true);
    ctx.closePath();
    ctx.fillStyle = "#3a3a3a";
    ctx.fill();
    ctx.strokeStyle = "#3a3a3a";
    ctx.stroke();

    // Inner circle
    ctx.beginPath();
    ctx.fillStyle = "#F00";
    ctx.arc(x, y, r * 0.7, 0, 2 * Math.PI, true);
    ctx.closePath();  
    ctx.fill();
    ctx.stroke();
}

function clearCanvas(canvas) {
    var ctx = canvas.getContext('2d');
    ctx.clearRect(0,0,canvas.width,canvas.height);
}

function searchItems(itemName) {

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
    clearCanvas(canvas);
    for (var i = 0; i < itemPlacements.length; i+=2) {
        var xCoordinate = document.getElementById(itemPlacements[i].id).innerHTML;
        var yCoordinate = document.getElementById(itemPlacements[i + 1].id).innerHTML;
        
        drawPinIcon(canvas, xCoordinate, yCoordinate, 18);
        console.log(xCoordinate);
        console.log(yCoordinate);
    }
}

function ResizeItemCol() {
    var floorplanHeight = $("#floorplan").height() + 'px';
    console.log("floorplan height=" + floorplanHeight);
    $(".scrollBox").css({ 'max-height':  floorplanHeight});
}