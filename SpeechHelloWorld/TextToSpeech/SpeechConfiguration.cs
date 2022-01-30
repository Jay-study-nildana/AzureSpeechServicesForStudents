using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToSpeech
{
    public class SpeechConfiguration
    {
        public SpeechConfig speechConfig { set; get; }

        public string SpeechKey { set; get; }
        public string SpeechRegion { set; get; }
        public string SpeechSynthesisLanguage { set; get; }
        public string SpeechSynthesisVoiceName { set; get; }

        public SpeechConfiguration()
        {
            SpeechKey = "9dabdc080b9844fea9fcffca8f611331";
            SpeechRegion = "eastus";
            SpeechSynthesisLanguage = "en-IN";
            SpeechSynthesisVoiceName = "en-IN-NeerjaNeural";
        }

        public async Task<SpeechConfig> GetSpeechConfig()
        {
            //build the speech config thing

            var config = SpeechConfig.FromSubscription(SpeechKey, SpeechRegion);
            //config.SpeechSynthesisLanguage = "en-IN";
            //config.SpeechSynthesisVoiceName = "en-IN-NeerjaNeural";
            config.SpeechSynthesisLanguage = SpeechSynthesisLanguage;
            config.SpeechSynthesisVoiceName = SpeechSynthesisVoiceName;
            return config;
        }
    }
}
