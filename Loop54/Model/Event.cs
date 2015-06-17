
namespace Loop54.Model
{
    public class Event
    {
        public string Type { get; set; }

        public string String { get; set; }

        public Entity Entity { get; set; }

        public double Revenue { get; set; }

        public string OrderId{ get; set; }

        public int Quantity { get; set; }

        public Event()
        {
            Quantity = 1;
        }
    }
}
