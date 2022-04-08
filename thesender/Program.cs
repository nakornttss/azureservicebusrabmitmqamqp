using System;
using Amqp;
using Amqp.Framing;
using System.Net;

var protocal = "amqp";              // use amqps for Azure Service Bus, use amqp for RabmitMQ in local
var hostname = "localhost:5672";    // use localhost:5672 for RabmitMQ in local
var username = WebUtility.UrlEncode("");   // Azure Service Bus is PolicyName
var password = WebUtility.UrlEncode("");   // Azure Service Bus is Primary Key or secondary key

// Connection string to connect Azure Service Bus or RabmitMQ
var connectionString = $"{protocal}://{username}:{password}@{hostname}/";  

// Make connection
var connection = new Connection(new Address(connectionString));

// Make session
var amqpSession = new Session(connection);

// set sender id
var senderSubscriptionId = "thedemo.amqp.sender";

// this is queue name
var queueName = "poc01";

// Make Link
var sender = new SenderLink(amqpSession, senderSubscriptionId, queueName);

for (var i = 0; i < 10; i++)
{
    // Create new message
    var message = new Message($"Hello Message Queue {i}");
    
    // Set message id
    message.Properties = new Properties() { MessageId = Guid.NewGuid().ToString() };

    // Add custom property to message
    message.ApplicationProperties = new ApplicationProperties();
    message.ApplicationProperties["Message.Type.Method"] = "Demo Property";

    // Send message to service bus
    sender.Send(message);
}
