// See https://aka.ms/new-console-template for more information
using TextToSpeech;

Console.WriteLine("Hello, World!");

//lets test our audio helper
//this creates a file directly in the function
var tempTextToSpeechHelper = new TextToSpeechHelper();

# region test short audio

//await tempTextToSpeechHelper.CreateTestAudioFile();

//same as CreateTestAudioFile, but you can pass a string if you want
//await tempTextToSpeechHelper.CreateAndReturnAudioStream();

#endregion


# region test long audio

//test the long audio thing
//var StatusURL = await tempTextToSpeechHelper.LongAudioTestOne();

//Note, you can use teh responseAllRequests to scan for all requests and send a specific value to test.
//get all requests
var responseAllRequests = await tempTextToSpeechHelper.GetAllSynthesisRequests();

//now, we need to check the status
//var tempURLOfStatus = "https://centralindia.customvoice.api.speech.microsoft.com/api/texttospeech/v3.0/longaudiosynthesis/bf2fe991-82c9-4a19-8d50-something123";
//await tempTextToSpeechHelper.CheckStatus(tempURLOfStatus);

//var tempRequestIDURL = "https://centralindia.customvoice.api.speech.microsoft.com/api/texttospeech/v3.0/longaudiosynthesis/bf2fe991-82c9-4a19-8d50-something123/files";
//await tempTextToSpeechHelper.GetAudioResult(tempRequestIDURL);

#endregion


Console.WriteLine("Hello, World!");
