var connection = new signalR.HubConnectionBuilder().withUrl("/commentHub").build();
connection.on("ReceiveMessage", function (senderName, message) {
    var msg = senderName + ": " + message;
    var li = document.createElement("li");
    li.textContent = msg;
    $("#list").prepend(li);
});
connection.start();
$("#btnSend").on("click", function () {
    var message = $("#txtMessage").val();
    connection.invoke("SendMessage", message).catch(function (err) {
        console.error(err.toString());
        
            
        
    });
    location.reload();
});








