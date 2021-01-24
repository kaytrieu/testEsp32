using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mqtt.Client.AspNetCore.Services
{
    public class MqttClientService : IMqttClientService
    {
        private IMqttClient mqttClient;
        private IMqttClientOptions options;

        public MqttClientService(IMqttClientOptions options)
        {
            this.options = options;
            mqttClient = new MqttFactory().CreateMqttClient();
            ConfigureMqttClient();
        }

        private void ConfigureMqttClient()
        {
            mqttClient.ConnectedHandler = this;
            mqttClient.DisconnectedHandler = this;
            mqttClient.ApplicationMessageReceivedHandler = this;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
            Console.WriteLine($"+ Topic = {eventArgs.ApplicationMessage.Topic}");
            Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload)}");
            Console.WriteLine($"+ QoS = {eventArgs.ApplicationMessage.QualityOfServiceLevel}");
            Console.WriteLine($"+ Retain = {eventArgs.ApplicationMessage.Retain}");
            Console.WriteLine();

            return Task.Run(() => mqttClient.PublishAsync("hello/world"));
        }

        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            Console.WriteLine("### CONNECTED WITH SERVER ###");

            // Subscribe to a topic
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("my/topic").Build());

            Console.WriteLine("### SUBSCRIBED ###");
        }

        public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("### DISCONNECTED FROM SERVER ###");
            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {
                await mqttClient.ConnectAsync(options, CancellationToken.None); // Since 3.0.5 with CancellationToken
            }
            catch
            {
                System.Console.WriteLine("### RECONNECTING FAILED ###");
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
           await mqttClient.ConnectAsync(options);
            if (!mqttClient.IsConnected)
            {
                await mqttClient.ReconnectAsync();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                var disconnectOption = new MqttClientDisconnectOptions
                {
                    ReasonCode = MqttClientDisconnectReason.NormalDisconnection,
                    ReasonString = "NormalDiconnection"
                };
                await mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
            }
            await mqttClient.DisconnectAsync();
        }

        public async Task PublishAsync(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            await mqttClient.PublishAsync(message, CancellationToken.None); // Since 3.0.5 with CancellationToken

        }


    }
}
