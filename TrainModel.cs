using Microsoft.ML;
using Microsoft.ML.Data;
using System;

class TrainModel
{
    public static void Main(string[] args)
    {
        var mlContext = new MLContext();

        // Ladda data
        IDataView dataView = mlContext.Data.LoadFromTextFile<ReviewData>("Data/reviews.csv", hasHeader: true, separatorChar: ',');

        Console.WriteLine("Schema:");
        foreach (var column in dataView.Schema)
        {
            Console.WriteLine($"Column: {column.Name}, Type: {column.Type}");
        }

        // Bygg en pipeline
        var pipeline = mlContext.Transforms.Text.NormalizeText("NormalizedReview", nameof(ReviewData.Review))
            .Append(mlContext.Transforms.Text.FeaturizeText("Features", "NormalizedReview"))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(nameof(ReviewData.Label))) 
            .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(nameof(ReviewData.Label), "Features"))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        // Träna modellen
        var model = pipeline.Fit(dataView);
        Console.WriteLine("Model trained successfully");

        // Spara modellen
        mlContext.Model.Save(model, dataView.Schema, "Models/reviewModel.zip");
        Console.WriteLine("Model is saved");
    }

    public class ReviewData
    {
        [LoadColumn(0)]
        public string? Review { get; set; }

        [LoadColumn(1)]
        public string? Label { get; set; }
    }
}
