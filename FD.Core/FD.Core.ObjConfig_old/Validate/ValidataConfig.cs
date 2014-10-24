using System.Collections.Generic;


namespace SLComponse.Validate
{
    public class ValidataConfig
    {
        public ValidataConfig()
        {
            ColumnValidata = new List<ColumnValidataConfig>();
        }
        public List<ColumnValidataConfig> ColumnValidata { get; set; }
    }
}