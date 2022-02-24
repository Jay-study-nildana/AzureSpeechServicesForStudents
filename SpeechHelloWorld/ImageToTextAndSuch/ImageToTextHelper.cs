using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Text;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace ImageToTextAndSuch
{
    public class ImageToTextHelper
    {
        //I made a image by usingn the sherlock holmes chapter here
        //https://sherlock-holm.es/stories/html/cano.html#Chapter-1
        //I made a image by usingn the sherlock holmes chapter here
        //https://sherlock-holm.es/stories/html/cano.html#Chapter-1

        public async Task SimpleImageToTextTest()
        {
            //You will find the image file in bin
            //something like this 
            //SpeechHelloWorld\SpeechHelloWorld\bin\Debug\net6.0
            var workingDirectory = System.IO.Directory.GetCurrentDirectory();
            const string READ_TEXT_URL_IMAGE = "SherlockHolmesTextPhoto.png";

            //get the OCR client 
            ImageConfiguration imageConfiguration = new ImageConfiguration();
            imageConfiguration.Authenticate();

            var client = imageConfiguration.client;
            var tempFileName = Path.Combine(workingDirectory, READ_TEXT_URL_IMAGE);
            var urlFile = tempFileName;
            //I tried to use a local file, but, that does not seem to work
            //So, I have put a file on one of my own websites. I plan to keep this file on that location for as long as I can. But, feel free to use any other online file of your own
            urlFile = "https://jaysstoriesandnovelshome.files.wordpress.com/2022/02/sherlockholmestextphoto.png";

            // Read text from URL
            var textHeaders = await client.ReadAsync(urlFile);
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Extracting text from URL file {Path.GetFileName(urlFile)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            Console.WriteLine();
        }

        public async Task<string> SimpleImageToTextWithFile(string filePath)
        {
            var response =  new StringBuilder("");
            //You will find the image file in bin
            //something like this 
            //SpeechHelloWorld\SpeechHelloWorld\bin\Debug\net6.0
            //var workingDirectory = System.IO.Directory.GetCurrentDirectory();
            //const string READ_TEXT_URL_IMAGE = "SherlockHolmesTextPhoto.png";

            ////get the OCR client 
            ImageConfiguration imageConfiguration = new ImageConfiguration();
            imageConfiguration.Authenticate();

            var client = imageConfiguration.client;
            //var tempFileName = Path.Combine(workingDirectory, READ_TEXT_URL_IMAGE);
            //var tempFileName = filePath;
            var urlFile = filePath;
            //I tried to use a local file, but, that does not seem to work
            //So, I have put a file on one of my own websites. I plan to keep this file on that location for as long as I can. But, feel free to use any other online file of your own
            //urlFile = "https://jaysstoriesandnovelshome.files.wordpress.com/2022/02/sherlockholmestextphoto.png";

            // Read text from URL
            //Note : Computer Vision cannot open files stored locally on the computer directly
            //stream only or online files only
            //var textHeaders = await client.ReadAsync(urlFile);  //this wont work
            //client.ReadInStreamAsync(stream, language).Result;
            ReadInStreamHeaders textHeaders;
            var file = File.OpenRead(urlFile);  //get a stream
            using (var stream = file)
            {
                textHeaders = client.ReadInStreamAsync(stream).Result;
            }
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Extracting text from URL file {Path.GetFileName(urlFile)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Console.WriteLine(line.Text);
                    response.Append(line.Text);
                }
            }
            Console.WriteLine();
            return(response.ToString());
        }

        //Testing Adult Content.
        //D:\Code2022\DotNetConceptsPrivate\AzureSpeechServicesForStudents\SpeechHelloWorld\SpeechHelloWorld\bin\Debug\net6.0
        //in the above location I have put some racy and adult images for reference and testing only.
        //I request developers who are looking at my code not to get offended
        //All images are obtained from wikipedia
        public async Task<string> AdultContentWithFile(string filePath)
        {
            var response = new StringBuilder("");

            ////get the OCR client 
            ImageConfiguration imageConfiguration = new ImageConfiguration();
            imageConfiguration.Authenticate();

            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            var client = imageConfiguration.client;
            var urlFile = filePath;
            var imageUrl = urlFile;

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
            // Analyze the URL image 
            //ImageAnalysis results = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

            ImageAnalysis results;
            var file = File.OpenRead(urlFile);  //get a stream
            using (var stream = file)  
            {
                results = await client.AnalyzeImageInStreamAsync(stream, visualFeatures: features);
            }

            // Sunmarizes the image content.
            Console.WriteLine("Summary:");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
            }
            Console.WriteLine();

            // Display categories the image is divided into.
            Console.WriteLine("Categories:");
            foreach (var category in results.Categories)
            {
                Console.WriteLine($"{category.Name} with confidence {category.Score}");
            }
            Console.WriteLine();

            // Image tags and their confidence score
            Console.WriteLine("Tags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            Console.WriteLine();

            // Faces
            Console.WriteLine("Faces:");
            foreach (var face in results.Faces)
            {
                Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, " +
                  $"{face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, " +
                  $"{face.FaceRectangle.Top + face.FaceRectangle.Height}");
            }
            Console.WriteLine();

            // Adult or racy content, if any.
            Console.WriteLine("Adult:");
            Console.WriteLine($"Has adult content: {results.Adult.IsAdultContent} with confidence {results.Adult.AdultScore}");
            Console.WriteLine($"Has racy content: {results.Adult.IsRacyContent} with confidence {results.Adult.RacyScore}");
            Console.WriteLine($"Has gory content: {results.Adult.IsGoryContent} with confidence {results.Adult.GoreScore}");
            Console.WriteLine();

            return (response.ToString());
        }
    }
}
