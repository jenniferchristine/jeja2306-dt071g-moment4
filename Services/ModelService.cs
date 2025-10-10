using Microsoft.ML;
using System;
using Microsoft.ML.Data;
using System.IO;

namespace ReviewClassifier.Services
{
    public static class ModelService // modell för predictions
    {
        private static readonly string ModelPath = "Models/reviewModel.zip";
        private static PredictionEngine<ReviewInput, ReviewPrediction>? predEngine;

        static ModelService() // en statisk konstruktor som körs när klassen används första gången
        {
            var mlContext = new MLContext();
            var model = mlContext.Model.Load(ModelPath, out var schema);

            if (model != null) // försök att skapa prediction engine om modellen finns
            {
                predEngine = mlContext.Model.CreatePredictionEngine<ReviewInput, ReviewPrediction>(model); // om modellen laddar så skapas ett objekt för att göra predictions
            }
            else
            {
                Console.WriteLine("Model loading failed.");
            }

        }


        public static string ClassifyReview(string reviewText) // metod som tar en recension som parameter och returnerar en predictad label
        {
            try
            {
                var input = new ReviewInput { Review = reviewText };
                var prediction = predEngine.Predict(input);

                return prediction.Label ?? "Can't classify review";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during classification: " + ex.Message);
                return "Error during classification.";
            }
        }
    }

    public class ReviewInput // klass för input-data till modellen
    {
        public string? Review { get; set; }
        public string? Label { get; set; }
    }

    public class ReviewPrediction // klass för output-data eller resultat från modellen
    {
        [ColumnName("PredictedLabel")]
        public string? Label { get; set; }
    }
}
