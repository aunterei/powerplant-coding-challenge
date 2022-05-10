namespace powerplant_coding_challenge.Models
{
    public class PayloadModel
    {
        public double Load { get; set; }
        public AvailableFuels Fuels { get; set; }        
        public IEnumerable<Powerplant> Powerplants { get; set; }
    }
}
