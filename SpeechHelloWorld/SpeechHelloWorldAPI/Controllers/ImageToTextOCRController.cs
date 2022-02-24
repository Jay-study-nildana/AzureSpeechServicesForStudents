using ImageToTextAndSuch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace SpeechHelloWorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageToTextOCRController : ControllerBase
    {
        //just testing to see if the files are coming in and file upload in swagger
        [HttpPost]
        [Route("MultipleFileTest")]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }

        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> OnPostUploadSingleFileAsync(IFormFile file)
        {
            var response = "All done";

            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }
            //converting to Image. But only works on Windows.
            //using (var memoryStream = new MemoryStream())
            //{
            //    await file.CopyToAsync(memoryStream);
            //    using (var img = Image.FromStream(memoryStream))
            //    {
            //        // TODO: ResizeImage(img, 100, 100);
            //    }
            //}

            var workingDirectory = System.IO.Directory.GetCurrentDirectory();
            var tempFileName = Path.Combine(workingDirectory, file.FileName);
            var filePath = tempFileName;
            var formFile = file;

            //file gets temporarily stored at some  place like this. 
            //D:\Code2022\DotNetConceptsPrivate\AzureSpeechServicesForStudents\SpeechHelloWorld\SpeechHelloWorldAPI\SherlockHolmesTextPhoto.png

            using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }

            var tempImageToTextHelper = new ImageToTextHelper();
            response = await tempImageToTextHelper.SimpleImageToTextWithFile(filePath);

            return Ok(response);
        }
    }
}
