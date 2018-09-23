using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BnbApi.DataTransfer;

namespace BnbApi.Services
{
    public class BusinessLogic
    {
        internal WorldScore GetTopTen(int? uid=0)
        {
            WorldScore ret = new WorldScore();
            ret.TopTen = new List<RankUser>();
            ret.UId = uid.Value;

            List<User> allData = new List<User>();
            using (bnbEntities ctx = new bnbEntities()) 
            {
                allData = ctx.User.OrderByDescending(x => x.RankScore).ToList();
                //ret.TopTen = 
            }

            var maxCount = (allData.Count < 7) ? allData.Count : 7;
            for (int i = 0; i < maxCount; i++)
            {
                var item = ConvertUserToRank(allData[i]);
                item.Rank = i + 1;
                ret.TopTen.Add(item);
            }

            if (uid.HasValue && uid > 0)
            {
                ret.CurrentUserList = new List<RankUser>();
                for (int i = 0; i < maxCount; i++)
                {
                    if (allData[i].UId == uid.Value)
                    {
                        if (i == 0)
                        { // for topper
                            var item = ConvertUserToRank(allData[i]);
                            item.Rank = i + 1;
                            item.IsCurrentUser = true;
                            ret.CurrentUserList.Add(item);

                            for (int x = i + 1; x < i + 4; x++)
                            {
                                if (x < maxCount)
                                {
                                    item = ConvertUserToRank(allData[x]);
                                    item.Rank = x + 1;
                                    ret.CurrentUserList.Add(item);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        }
                        else if (i == maxCount - 1)
                        {// for last
                            var item = ConvertUserToRank(allData[i]);
                            for (int x = i - 4; x < i; x++)
                            {
                                if (x < maxCount)
                                {
                                    item = ConvertUserToRank(allData[x]);
                                    item.Rank = x + 1;
                                    ret.CurrentUserList.Add(item);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            item = ConvertUserToRank(allData[i]);
                            item.Rank = i;
                            item.IsCurrentUser = true;
                            ret.CurrentUserList.Add(item);

                            break;
                        }
                        else if (i > 0)
                        {// for in bw
                            var item = ConvertUserToRank(allData[i - 1]);
                            item.Rank = i;
                            ret.CurrentUserList.Add(item);

                            item = ConvertUserToRank(allData[i]);
                            item.Rank = i + 1;
                            item.IsCurrentUser = true;
                            ret.CurrentUserList.Add(item);

                            for (int x = i + 1; x < i + 3; x++)
                            {
                                if (x < maxCount)
                                {
                                    item = ConvertUserToRank(allData[x]);
                                    item.Rank = x + 1;
                                    ret.CurrentUserList.Add(item);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        }

                    }
                }
            }

            //int maxStars = ret.TopTen[0]
            return ret;
        }

        public RankUser ConvertUserToRank(User cUser) 
        {
            RankUser cu = new RankUser();
            cu.Name = cUser.Name;
            cu.IsCurrentUser = true;
            cu.Score = cUser.Score;
            cu.UId = cUser.UId;
            cu.RankScore = cUser.RankScore.HasValue ? cUser.RankScore.Value : 0;
            cu.Stages = cUser.Stages.HasValue ? cUser.Stages.Value : 0;
            cu.Stars = cUser.Starts.HasValue ? cUser.Starts.Value : 0;
            if (cu.Stages > 0 && cu.Stars > 0) 
            {
                cu.Rating = (cu.Stars * 10) / (cu.Stages * 3);
            }
            cu.IsCurrentUser = false;
            return cu;
        }

        internal WorldScore WholeEntry(FirstEntry item)
        {

            if (item.UId == 0)
            {
                using (bnbEntities ctx = new bnbEntities())
                {
                    User chkUser = null;
                    if (!string.IsNullOrEmpty(item.FbId))
                    {
                        var fbCheck = ctx.User.Where(x => x.FbId == item.FbId).FirstOrDefault();
                        if (fbCheck != null)
                        {
                            chkUser = fbCheck;
                        }
                    }



                    if (chkUser == null)
                    {
                        var NameCheck = ctx.User.Where(x => x.Name == item.Name).FirstOrDefault();
                        if (NameCheck != null)
                            chkUser = NameCheck;


                        if (chkUser == null)
                        {
                            User newEnt = new User();
                            newEnt.Name = item.Name;
                            newEnt.Guid = item.Guid;
                            newEnt.BallCount = item.BallCount;
                            newEnt.LLimit = item.LLimit;
                            newEnt.Score = item.Score;
                            newEnt.TSpeed = item.TSpeed;
                            newEnt.Version = item.Version;
                            newEnt.Stages = item.Stages;
                            newEnt.Starts = item.Stars;
                            if (!string.IsNullOrEmpty(item.Location))
                            {
                                newEnt.Location = item.Location;
                            }
                            if (!string.IsNullOrEmpty(item.FbId))
                            {
                                newEnt.FbId = item.FbId;
                            }
                            newEnt.RankScore = CalculateRankScore(newEnt);
                            ctx.User.Add(newEnt);
                            ctx.SaveChanges();
                            item.UId = newEnt.UId;
                        }
                        else
                        {
                            return new WorldScore() { Msg = "Try Another Name" };
                        }
                    }
                    else
                    {
                        if (chkUser.Score < item.Score
                            && chkUser.Stages < item.Stages)
                        {

                        }
                    }

                  
                }
            }
            return GetTopTen(item.UId);
        }

        //public int CalculateRankScore(User u)
        //{
        //    int ret = 100;
        //    // stage min max 0 - 100
        //    // stage number * 100
        //    // stars x 100 / (stage x 3) => max 100
        //    // score x 10 / (stage x 20)
        //    if (u.Stages.HasValue) 
        //    {
        //        ret = ret * u.Stages.Value;
        //        if (u.Starts.HasValue)
        //            ret += (u.Starts.Value * 100) / (u.Stages.Value * 3);

        //        if (u.Score > 0)
        //            ret += (u.Score * 10) / (u.Stages.Value * 20);
        //    }
        //    return ret;
        //}

        internal WorldScore UpdateWholeEntry(FirstEntry item)
        {

            if (item.UId != 0)
            {
                using (bnbEntities ctx = new bnbEntities())
                {
                    User chkUser = ctx.User.Where(x => x.UId == item.UId).FirstOrDefault(); 

                    if (chkUser != null)
                    {
                        chkUser.BallCount = item.BallCount;
                        chkUser.LLimit = item.LLimit;
                        chkUser.Score = item.Score;
                        chkUser.TSpeed = item.TSpeed;
                        chkUser.Version = item.Version;
                        chkUser.Stages = item.Stages;
                        chkUser.Starts = item.Stars;
                        if (!string.IsNullOrEmpty(item.Location))
                        {
                            chkUser.Location = item.Location;
                        }
                        if (!string.IsNullOrEmpty(item.FbId))
                        {
                            chkUser.FbId = item.FbId;
                        }
                        chkUser.RankScore = CalculateRankScore(chkUser);
                        ctx.SaveChanges();
                    }
                }
            }
            return GetTopTen(item.UId);
        }


        public int CalculateRankScore(User u)
        {
            int int_ = 100;
            int? int_2 = u.Stages;
            if (int_2.HasValue)
            {
                int varKT0 = int_;
                int_2 = u.Stages;
                int_ = varKT0 * int_2.Value;
                int_2 = u.Starts;
                if (int_2.HasValue)
                {
                    int varOW0 = int_;
                    int_2 = u.Starts;
                    int varOK0 = int_2.Value * 100;
                    int_2 = u.Stages;
                    int_ = varOW0 + varOK0 / (int_2.Value * 3);
                }
                if (u.Score > 0)
                {
                    int varTJ0 = int_;
                    int varTG0 = u.Score * 10;
                    int_2 = u.Stages;
                    int_ = varTJ0 + varTG0 / (int_2.Value * 20);
                }
            }
            return int_;
        }
    }
}