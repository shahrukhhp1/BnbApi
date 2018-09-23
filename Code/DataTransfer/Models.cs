using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public Guid Guid { get; set; }
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