
const connection = new signalR.HubConnectionBuilder()
	.withUrl("http://localhost:5000/notificationHub",
		{
			skipNegotiation: true,
			transport: signalR.HttpTransportType.WebSockets
		})
	.configureLogging(signalR.LogLevel.Information)
	.build();

connection.on("Notify", function (message) {
	var li = document.createElement("li");
	li.textContent = message;
	document.getElementById("messagesList").appendChild(li);
});

connection.start().catch(function (err) {
	return console.error(err);
});

connection.onclose(function () {
	console.log("closing connection");
	timeoutConnection();
});

function timeoutConnection() {
	setTimeout(function () { startConnection(); }, 5000);
}

function startConnection() {
	connection.start().catch(err => timeoutConnection());
}
