using MnistApp.Data;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace MnistApp.Core
{
    public class AmostraEmnist
    {
        [ColumnName("Features")]
        [VectorType(MnistImage.TotalPixeis)]
        public float[] Pixels {get; set;}
        
        [ColumnName("Label")]
        public float Label {get; set;}
    }
}
