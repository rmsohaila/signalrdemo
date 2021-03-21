"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub").build();

connection.on("sendToUser", (Heading, Content) => {
    var heading = document.createElement("h3");
    heading.textContent = Heading;

    var p = document.createElement("p");
    p.innerText = Content;

    var div = document.createElement("div");
    div.className = "col-3";
    div.appendChild(heading);
    div.appendChild(p);

    document.getElementById("articleList").appendChild(div);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});