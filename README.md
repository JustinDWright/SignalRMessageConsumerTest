# SignalRMessageConsumerTest
This is a test project to determine why SignalR HubContext is different when injected to a MassTransit IConsumer class.

The project's service will add a message to a rabbit message queue every 10 seconds.
In turn the consumer will pull the message and try and send the message to the test client via SignalR.
