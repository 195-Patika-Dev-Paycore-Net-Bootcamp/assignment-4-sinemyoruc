using Microsoft.ML.Data;

namespace SinemYoruc_HW4
{
    public class Container
    {
        public virtual long Id { get; set; }
        public virtual string ContainerName { get; set; }

        [LoadColumn(0)]
        public virtual double Latitude { get; set; }

        [LoadColumn(1)]
        public virtual double Longitude { get; set; }
        public virtual long VehicleId { get; set; }
    }
}
