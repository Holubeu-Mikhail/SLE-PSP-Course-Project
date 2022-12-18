namespace Common.Models
{
    public class ClientRequest
    {
        public double[][] Matrix { get; set; }
        
        public double[] Vector { get; set; }

        public bool IsLinear { get; set; }
    }
}
