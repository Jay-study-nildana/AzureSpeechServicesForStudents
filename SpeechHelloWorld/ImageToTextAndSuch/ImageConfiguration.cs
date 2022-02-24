﻿using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToTextAndSuch
{
    public class ImageConfiguration
    {
        public ComputerVisionClient client { set; get; }
        public string endpoint { set; get; }
        public string key { set; get; }
                /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
        public void Authenticate()
        {
            client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
        }

        public ImageConfiguration()
        {
            endpoint = "https://ocrdemo1forstudents1.cognitiveservices.azure.com/";
            key = "b4391bc0409a4f76b9ac7362a863710d";
        }
    }
}
