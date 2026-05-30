using Microsoft.ML;
using Microsoft.ML.Data;

namespace GestionTareasAPI.MLModels
{
    public class DatoSentimiento
    {
        [LoadColumn(0)]
        public string Comentario { get; set; } = string.Empty;

        [LoadColumn(1), ColumnName("Label")]
        public bool Sentimiento { get; set; }
    }

    public class PrediccionSentimiento
    {
        [ColumnName("PredictedLabel")]
        public bool Prediccion { get; set; }
        public float Probabilidad { get; set; }
        public float Puntaje { get; set; }
    }

    public class SentimientoService
    {
        private readonly PredictionEngine<DatoSentimiento, PrediccionSentimiento> _predictionEngine;

        public SentimientoService()
        {
            var mlContext = new MLContext(seed: 0);

            // Busca el CSV relativo al ejecutable
            var rutaBase = AppDomain.CurrentDomain.BaseDirectory;
            var rutaCsv = Path.Combine(rutaBase, "MLModels", "datos_sentimiento.csv");

            // Si no existe en BaseDirectory, busca en el directorio del proyecto
            if (!File.Exists(rutaCsv))
            {
                var dir = Directory.GetCurrentDirectory();
                rutaCsv = Path.Combine(dir, "MLModels", "datos_sentimiento.csv");
            }

            var datos = mlContext.Data.LoadFromTextFile<DatoSentimiento>(
                path: rutaCsv,
                hasHeader: true,
                separatorChar: ',');

            var pipeline = mlContext.Transforms.Text
                .FeaturizeText("Features", nameof(DatoSentimiento.Comentario))
                .Append(mlContext.BinaryClassification.Trainers
                    .SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            var modelo = pipeline.Fit(datos);

            _predictionEngine = mlContext.Model
                .CreatePredictionEngine<DatoSentimiento, PrediccionSentimiento>(modelo);
        }

        public string AnalizarSentimiento(string comentario)
        {
            var resultado = _predictionEngine.Predict(new DatoSentimiento
            {
                Comentario = comentario
            });
            return resultado.Prediccion ? "Positivo" : "Negativo";
        }
    }
}
