using System;

namespace InsuranceSample.Domain.Models
{
    public class Client
    {
        public Guid ClientId { get; set; }
        public string SSN { get; set; }
        public string Name { get; set; }

        public Client()
        {
            ClientId = Guid.NewGuid();
        }
    }
}
