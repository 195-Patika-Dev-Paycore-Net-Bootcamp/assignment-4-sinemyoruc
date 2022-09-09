using Microsoft.ML.Data;

namespace SinemYoruc_HW4
{
    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedCustId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}
