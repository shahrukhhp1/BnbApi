//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BnbApi
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Feature = new HashSet<Feature>();
            this.Stage = new HashSet<Stage>();
        }
    
        public int UId { get; set; }
        public System.Guid Guid { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Version { get; set; }
        public int Score { get; set; }
        public Nullable<int> RankScore { get; set; }
        public Nullable<int> BallCount { get; set; }
        public Nullable<int> TSpeed { get; set; }
        public Nullable<int> LLimit { get; set; }
        public Nullable<int> Stages { get; set; }
        public Nullable<int> Starts { get; set; }
        public string FbId { get; set; }
    
        public virtual ICollection<Feature> Feature { get; set; }
        public virtual ICollection<Stage> Stage { get; set; }
    }
}
