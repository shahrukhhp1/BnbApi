using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BnbApi.DataTransfer
{
    public class RankUser
    {
        public int UId
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int Score
        {
            get;
            set;
        }
        public bool IsCurrentUser
        {
            get;
            set;
        }
        public int RankScore
        {
            get;
            set;
        }
        public string Location
        {
            get;
            set;
        }
        public int Rank
        {
            get;
            set;
        }
        public int Stars
        {
            get;
            set;
        }
        public double Rating
        {
            get;
            set;
        }
        public int Stages
        {
            get;
            set;
        }
    }

    public class WorldScore
    {
        public List<RankUser> TopTen { get; set; }
        public List<RankUser> CurrentUserList { get; set; }
        public string Msg { get; set; }
        public int UId { get; set; }
    }
}