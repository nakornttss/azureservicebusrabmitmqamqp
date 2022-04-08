using Amqp;
using Amqp.Framing;
using System.Xml;
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

// set receiver id
var receiverSubscriptionId = "thedemo.amqp.receiver";

// this is queue name
var queueName = "poc01";

// Make link
var consumer = new ReceiverLink(amqpSession, receiverSubscriptionId, $"{queueName}");

// Register handler to manage menage when get any new message
consumer.Start(5, OnMessageCallback);

// Wait until press some key
Console.Read();

// Handler for manage activity when getting the new message
static void OnMessageCallback(IReceiverLink receiver, Amqp.Message message)
{
    try
    {
        // Get custom property from message
        var messageType = message.ApplicationProperties["Message.Type.Method"];

        // Read message body
        var rawBody = message.Body;
        Console.WriteLine(rawBody.ToString() + " " + messageType);

        // Accept message once compled reading
        receiver.Accept(message);
    }
    catch (Exception ex)
    {
        // Reject message in case got any error
        receiver.Reject(message);
        Console.WriteLine(ex);
    }
}