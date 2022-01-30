using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using System.Net;
using TextToSpeech;

namespace SpeechHelloWorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextToSpeechController : ControllerBase
    {
        //make sure there are some audio files that you are trying to get
        //for example, you will see some audio files in the root folder 
        //put those names when you test this API endpoint
        [HttpGet]
        [Route("ReturnAudioFile")]
        public ActionResult<HttpResponseMessage> ReturnAudioFile(string fileName)
        {
            HttpResponseMessage result = null;

            var workingDirectory = System.IO.Directory.GetCurrentDirectory();
            //var full = Path.Combine(baseDir, dirFragment);
            var audioFileName = fileName;
            var tempFileName = Path.Combine(workingDirectory, audioFileName);

            var bytesOfFile = System.IO.File.ReadAllBytes(tempFileName);

            using (var readStream = new System.IO.FileStream(tempFileName, FileMode.Open))
            {
                var buffer = new byte[readStream.Length];
                using (var ms = new MemoryStream(buffer))
                {
                    readStream.CopyToAsync(ms).Wait();
                    return File(buffer, "audio/wav");
                }
            }

            return result;
        }

        [HttpGet]
        [Route("ReturnAudioStream")]
        public async Task<ActionResult<HttpResponseMessage>> ReturnAudioStreamAsync(string? textToConvert="I Love You")
        {
            var tempSpeechConfiguration = new SpeechConfiguration();
            var tempSpeechConfig = await tempSpeechConfiguration.GetSpeechConfig();

            using var synthesizer = new SpeechSynthesizer(tempSpeechConfig, null);
            var result = await synthesizer.SpeakTextAsync(textToConvert);
            using var stream = AudioDataStream.FromResult(result);

            //processing the stream into audio file
            //this can be skipped but as of now, I don't know how to return the file directly without saving.
            var workingDirectory = System.IO.Directory.GetCurrentDirectory();
            var audioFileName = "audiofileinternal" + DateTime.Now.Ticks.ToString() + ".wav";
            var tempFileName = Path.Combine(workingDirectory, audioFileName);
            await stream.SaveToWaveFileAsync(tempFileName);

            //returning the file
            using (var readStream = new FileStream(tempFileName, FileMode.Open))
            {
                var buffer = new byte[readStream.Length];
                using (var ms = new MemoryStream(buffer))
                {
                    readStream.CopyToAsync(ms).Wait();
                    return File(buffer, "audio/wav");
                }
            }
        }
    }
}
