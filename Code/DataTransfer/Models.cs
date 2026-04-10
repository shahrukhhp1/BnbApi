using System;
using System.Collections.Generic;
using System.Linq;

namespace BnbApi.DataTransfer
{
    public class Models
    {

    }

    public class FirstEntry 
    {
        public string Name { get; set; }
        public string FbId { get; set; }
        public string Location { get; set; }
        /// <summary>Form field "Guid". Unity may send the literal "null" from PlayerPrefs.</summary>
        public string Guid { get; set; }
        public int UId { get; set; }
        public int Score { get; set; }
        public int BallCount { get; set; }
        //public int[] stars { get; set; }
        //public string[] features { get; set; }
        public int LLimit { get; set; }
        public int TSpeed { get; set; }
        public int Version { get; set; }
        public int Stages { get; set; }
        public int Stars { get; set; }
    }
}
