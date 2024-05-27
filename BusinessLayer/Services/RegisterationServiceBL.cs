using BusinessLayer.Helper;
using BusinessLayer.Interfaces;
using Confluent.Kafka;
using ModelLayer.EmailModel;
using ModelLayer.RegisterModel;
using Newtonsoft.Json;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class RegisterationServiceBL(IRegisterationRL registeration, IEmail email) : IRegisterationBL
    {
        private readonly IRegisterationRL _registeration = registeration;
        private readonly IEmail _email = email;

        public async Task<bool> UserRegister(RegisterUserModel registerModel)
        {
            var registrationDetailsForPublishing = new KafkaPublishingDetails(registerModel);
            var registrationDetailsJson = JsonConvert.SerializeObject(registrationDetailsForPublishing);
            var producerConfig = KafkaProducerConfiguration.GetProducerConfig();

            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                try
                {
                    await producer.ProduceAsync("Registration-topic", new Message<Null, string> { Value = registrationDetailsJson });
                    Console.WriteLine("Registration details published to Kafka topic.");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Failed to publish registration details to Kafka topic: {e.Error.Reason}");
                }
            }

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092", 
                GroupId = "my-consumer-group", 
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                consumer.Subscribe("Registration-topic"); 

                try
                {
                    var consumeResult = consumer.Consume(); 
                    if (registerModel.Email != null)
                        await _email.SendEmail(registerModel.Email, "Registeration Successful", $"Welcome to FundooNotes {registerModel.FirstName}");
                    Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                }
            }
            return await _registeration.RegisterUser(registerModel);
        }
    }
}
