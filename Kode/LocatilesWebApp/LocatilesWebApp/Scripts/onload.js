window.onload = function onloadHandler()
{
    alert(document.getElementById('searchfield').value + "hey");
    document.getElementById("searchButton").onclick = searchItems;
}

function point() {
    this.coordx = 0;
    this.coordy = 0;
}

function drawPinIcon(canvas, x,y) {
    var ctx = canvas.getContext('2d');
    ctx.strokeStyle = "#FF0000";
    ctx.beginPath();
    var r = 20;
    var ycirclepos = y - Math.sqrt(r * r + r * r)/2;
    ctx.arc(x, ycirclepos, r, 1 / 4 * Math.PI, 3 / 4 * Math.PI, true); // Outer circle
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(x - Math.cos(1 / 4 * Math.PI) * r, ycirclepos + Math.cos(1 / 4 * Math.PI) * r);
    ctx.lineTo(x, y);
    ctx.stroke();
}

function searchItems() {
    alert(document.getElementById('searchfield').value);
    var canvas = document.getElementById('floorplan');
    drawPinIcon(canvas, 80, 80);
}