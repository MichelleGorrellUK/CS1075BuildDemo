namespace ServerSide.DTOs
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Age { get; set; }
        public decimal Balance { get; set; }
    }
}
