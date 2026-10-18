using Microsoft.ML.Data;

namespace MnistApp.Core
{
    public class PrevisaoEmnist
    {
        [ColumnName("PredictedLabel")]
        public float PredictedLabel {get; set;}

        [ColumnName("Score")]
        public float[] Score {get; set;}
        
    }
}
