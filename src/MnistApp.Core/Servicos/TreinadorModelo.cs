using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using MnistApp.Data;

namespace MnistApp.Core
{
    public class TreinarModelo
    {
        private readonly MLContext _mlContext;

        private DataViewSchema? _schemaTreino;

        public TreinarModelo()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public ITransformer Treinar(List<AmostraEmnist> treino)
        {
            // 1. Carregar a lista num IDataView
            IDataView dadosTreino = _mlContext.Data.LoadFromEnumerable(treino);
            _schemaTreino = dadosTreino.Schema;

            var pipeline = 
                _mlContext.Transforms.Conversion
                        .MapValueToKey
                            (
                                inputColumnName: "Label",
                                outputColumnName: "LabelKey"
                            )
                .Append(_mlContext.Transforms
                        .Concatenate
                            (
                                "Features",
                                "Pixels"
                            )
                        )
                .Append(_mlContext.Transforms
                        .NormalizeMinMax
                            (
                                "Features"
                            )
                        )
                .Append(_mlContext.MulticlassClassification.Trainers
                        .SdcaMaximumEntropy
                            (
                                labelColumnName:"LabelKey",
                                featureColumnName: "Features",
                                maximumNumberOfIterations: 100
                            )
                        )
                .Append(_mlContext.Transforms.Conversion
                        .MapKeyToValue
                            (
                                outputColumnName: "PredictedLabel",
                                inputColumnName: "PredictedLabel"
                            )
                        );
            
                return pipeline.Fit(dadosTreino);
        }

        public MulticlassClassificationMetrics Avaliar(ITransformer modelo, List<AmostraEmnist> teste)
        {
            IDataView dadosTeste = _mlContext.Data.LoadFromEnumerable(teste);
            IDataView previsoes = modelo.Transform(dadosTeste);

            return _mlContext.MulticlassClassification.Evaluate(
                data: previsoes,
                labelColumnName: "LabelKey",
                scoreColumnName: "Score",
                predictedLabelColumnName: "PredictedLabel");
        }

        public void Guardar(ITransformer modelo, string caminho)
        {
            if (_schemaTreino is null)
                throw new InvalidOperationException("Tens de chamar Treinar() antes de Guardar().");

            _mlContext.Model.Save(modelo, _schemaTreino, caminho);
            Console.WriteLine($"Modelo guardado em: {caminho}");
        }
    }
}
