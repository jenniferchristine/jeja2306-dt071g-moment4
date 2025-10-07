using Microsoft.ML;
using System;
using Microsoft.ML.Data;
using System.IO;

namespace ReviewClassifier.Services
{
    public static class ModelService
    {
        private static readonly string ModelPath = "Models/reviewModel.zip";
        private static PredictionEngine<ReviewInput, ReviewPrediction>? predEngine;

        static ModelService()
        {
            var mlContext = new MLContext();
            var model = mlContext.Model.Load(ModelPath, out var schema);

            // Försök att skapa prediction engine om modellen finns
            if (model != null)
            {
                predEngine = mlContext.Model.CreatePredictionEngine<ReviewInput, ReviewPrediction>(model);
            }
            else
            {
                Console.WriteLine("Model loading failed.");
            }

        }


        public static string ClassifyReview(string reviewText)
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

    public class ReviewInput
    {
        public string? Review { get; set; }
        public string? Label { get; set; }
    }

    public class ReviewPrediction
    {
        [ColumnName("PredictedLabel")]
        public string? Label { get; set; }
    }
}
