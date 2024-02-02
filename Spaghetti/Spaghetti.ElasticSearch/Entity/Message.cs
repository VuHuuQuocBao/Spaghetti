namespace Spaghetti.ElasticSearch.Entity
{
    public class Message
    {
        public Guid message_id { get; set; }
        public int channel_id { get; set; }
        public string content { get; set; }
        public int author_id { get; set; }
        public int server_id { get; set; }

    }
}
