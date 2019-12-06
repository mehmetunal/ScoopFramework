namespace ScoopFramework.Model
{
    public class ReturnValue
    {
        public object Id { get; set; }
        public bool Success { get; set; }
        public string Mesaj { get; set; }
        public object Data { get; set; }
        public int Count { get; set; }
        public string ReplayUrl { get; set; }
    }
}
