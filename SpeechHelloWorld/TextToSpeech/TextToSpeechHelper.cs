using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TextToSpeech2;
using TextToSpeech3;

namespace TextToSpeech
{
    public class TextToSpeechHelper
    {

        #region Less Than 10 minutes Audio
        //You will find the audio file in bin
        //something like this 
        //SpeechHelloWorld\SpeechHelloWorld\bin\Debug\net6.0
        public async Task CreateTestAudioFile()
        {
            var tempSpeechConfiguration = new SpeechConfiguration();
            var tempSpeechConfig = await tempSpeechConfiguration.GetSpeechConfig();
            var workingDirectory = Directory.GetCurrentDirectory();
            var audioFileName = "audiofile" + DateTime.Now.Ticks.ToString()+".wav";
            var tempFileName = Path.Combine(workingDirectory, audioFileName);
            using var audioConfig = AudioConfig.FromWavFileOutput(tempFileName);
            using var synthesizer = new SpeechSynthesizer(tempSpeechConfig, audioConfig);
            var tempTextToConvert = "I love you";
            await synthesizer.SpeakTextAsync(tempTextToConvert);
        }


        //this will return the audio stream of the audio file
        //you send the text you wish to conver
        //NOTE : this is NOT the long audio. This is for words that will result in audio less than 10 minutes
        //Useful for short term, live text to speech conversions
        //this also stores a file in the system
        //You will find the audio file in bin
        //something like this 
        //SpeechHelloWorld\SpeechHelloWorld\bin\Debug\net6.0
        public async Task<AudioDataStream> CreateAndReturnAudioStream(string? textToConvert = "I Love You")
        {
            var tempSpeechConfiguration = new SpeechConfiguration();
            var tempSpeechConfig = await tempSpeechConfiguration.GetSpeechConfig();

            using var synthesizer = new SpeechSynthesizer(tempSpeechConfig, null);
            var result = await synthesizer.SpeakTextAsync(textToConvert);
            using var stream = AudioDataStream.FromResult(result);

            //processing the stream into audio file
            var workingDirectory = System.IO.Directory.GetCurrentDirectory();
            var audioFileName = "audiofileinternal" + DateTime.Now.Ticks.ToString() + ".wav";
            var tempFileName = Path.Combine(workingDirectory, audioFileName);
            await stream.SaveToWaveFileAsync(tempFileName);

            //returning the stream
            return stream;
        }

        #endregion

        #region More Thank 10 minutes Long Audio API

        //Use CheckStatus to get the status. If the file generation is done, you will get a request ID
        //use the request ID and you will get the URL where you can download the file.
        public async Task<LongAudioStatusFile> GetAudioResult(string tempRequestID)
        {
            var longAudioStatusFile = new LongAudioStatusFile();
            var tempStuff = new ExtraStuff();
            string subscriptionKey = tempStuff.subscriptionKey;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string url = tempRequestID;
                var response = await client.GetAsync(url);
                var responseStringJSON = await client.GetStringAsync(url);

                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    var something = response.StatusCode;
                    var tempJSON = response.ToString();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    string jsonString = responseBody;
                    longAudioStatusFile = LongAudioStatusFile.FromJson(jsonString);
                }

                var something2 = response.StatusCode;
            }
            return longAudioStatusFile;
        }

        //LongAudioTestOne will place a request in the azure text to speech long audio queue
        //this function will help you find out the status
        public async Task<LongAudioStatus> CheckStatus(string tempURLOfStatus)
        {
            var longAudioStatus = new LongAudioStatus();
            var tempStuff = new ExtraStuff();
            string subscriptionKey = tempStuff.subscriptionKey;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string url = tempURLOfStatus;
                //var response = client.PostAsync(url, content).Result;
                var response = await client.GetAsync(url);

                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    //APIHelper.PrintErrorMessage(response);
                    //return false;
                    var something = response.StatusCode;
                    var tempJSON = response.ToString();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    string jsonString = responseBody;
                    longAudioStatus = LongAudioStatus.FromJson(jsonString);
                }

                var something2 = response.StatusCode;
            }
            return longAudioStatus;
        }

        public async Task<AllSynthesisRequests> GetAllSynthesisRequests()
        {
            var allSynthesisRequests = new AllSynthesisRequests();
            var tempStuff = new ExtraStuff();
            string subscriptionKey = tempStuff.subscriptionKey;
            string url = @"https://centralindia.customvoice.api.speech.microsoft.com/api/texttospeech/v3.0/longaudiosynthesis";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                var response = await client.GetAsync(url);

                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    //APIHelper.PrintErrorMessage(response);
                    //return false;
                    var something = response.StatusCode;
                    var tempJSON = response.ToString();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    string jsonString = responseBody;
                    allSynthesisRequests = AllSynthesisRequests.FromJson(jsonString);

                }

                var something2 = response.StatusCode;
            }
            return allSynthesisRequests;
        }

        ////SpeechHelloWorld\SpeechHelloWorld\bin\Debug\net6.0
        //the above location is where you would put the samplefile.txt
        //or any other text file you wish to process.
        //This will place a request in the Long Audio Queue.
        //Then return a URL where status can be checked at a later point of time.
        //ideally, you would take the response URL and put it in a file or store it somewhere for later processing
        public async Task<string?> LongAudioTestOne()
        {
            var responseURLStatus = "Something Went Wrong";
            var tempStuff = new ExtraStuff();
            //path for the input text file
            var workingDirectory = Directory.GetCurrentDirectory();
            var inputFileName = "samplefile.txt";
            var tempFileName = Path.Combine(workingDirectory, inputFileName);

            string subscriptionKey = tempStuff.subscriptionKey;
            string url = @"https://centralindia.customvoice.api.speech.microsoft.com/api/texttospeech/v3.0/longaudiosynthesis";

            using (FileStream fstream = new FileStream(tempFileName, FileMode.Open))
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                content.Add(new StringContent("sample display name"), "displayname");
                content.Add(new StringContent("sample description"), "description");
                content.Add(new StringContent("en-GB"), "locale");
                content.Add(new StringContent("[{\"voiceName\": \"en-GB-LibbyNeural\"}]"), "voices");
                content.Add(new StringContent("riff-16khz-16bit-mono-pcm"), "outputformat");

                var scriptContent = new StreamContent(fstream);
                scriptContent.Headers.Add("Content-Disposition", $@"form-data; name=""script""; filename=""{inputFileName}""");
                scriptContent.Headers.Add("Content-Type", "text/plain");
                scriptContent.Headers.Add("Content-Length", $"{fstream.Length}");
                content.Add(scriptContent, "script", inputFileName);

                var response = await client.PostAsync(url, content);

                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    //APIHelper.PrintErrorMessage(response);
                    //return false;
                    var something = response.StatusCode;
                    string responseBody = await response.Content.ReadAsStringAsync();
                    string jsonString = responseBody;
                }
                else
                {
                    //this API does not have a JSON response.
                    var something = response.StatusCode;
                    string responseBody = await response.Content.ReadAsStringAsync();
                    string jsonString = responseBody;
                    //we get the URL of status in the response header
                    HttpHeaders headers = response.Headers;
                    IEnumerable<string> values;
                    if (response.Headers.TryGetValues("Location", out values))
                    {
                        //use the URL in output to get the request status.
                        string URLThatContainsOutputStatus = values.FirstOrDefault();
                        var allValues = values;
                        responseURLStatus = URLThatContainsOutputStatus;
                    }
                }
                var something2 = response.StatusCode;
            }
            return responseURLStatus;
        }

        public class ExtraStuff
        {
            public string subscriptionKey { set; get; }

            public ExtraStuff()
            {
                var temp = new SpeechConfiguration();
                subscriptionKey = temp.SpeechKey;
            }
        }

        #endregion
    }
}
