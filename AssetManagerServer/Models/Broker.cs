namespace AssetManagerServer.Models
{
    public class Broker
    {
        public Broker()
        {
            
        }

        public Broker(Broker broker)
        {
            Id = broker.Id;
            Name = broker.Name;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
    }
}