using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkinCancerApp.Model.ImageModel
{
  public  class ImageResponseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("project")]
        public string Project { get; set; }

        [JsonProperty("iteration")]
        public string Iteration { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("predictions")]
        public List<Prediction> Predictions { get; set; }
    }
    public class Prediction
    {
        [JsonProperty("tagId")]
        public string TagId { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }

        [JsonProperty("probability")]
        public double Probability { get; set; }
    }
}
