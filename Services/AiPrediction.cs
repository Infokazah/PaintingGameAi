using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PaintingGameMVVM;

namespace PaintGameMVVM.Services
{
    internal class AiPrediction
    {
        private readonly PredictionEngine<DivineModel.ModelInput, DivineModel.ModelOutput> engine;
        private string _currentTeme;
        private readonly List<string> Themes = new List<string>
        {
            "улитка",
            "бабочка",
            "кактус",
            "дом",
            "утка"
        };
        public AiPrediction()
        {    
            var mlContext = new MLContext();
            var mlModel = mlContext.Model.Load(Path.GetFullPath("DivineModel.zip"), out var _);
            engine = mlContext.Model.CreatePredictionEngine<DivineModel.ModelInput, DivineModel.ModelOutput>(mlModel);
            
        }

        public string CreatePrediction(byte[] image)
        {
            
            var result = engine.Predict(
                new DivineModel.ModelInput
                {
                    ImageSource = image
                });

                return result.PredictedLabel;
        }

        public string CreateNewTheme()
        {
            Random rnd = new Random();

            _currentTeme = Themes[rnd.Next(0, Themes.Count)];
            return _currentTeme;
        }
    }
}
