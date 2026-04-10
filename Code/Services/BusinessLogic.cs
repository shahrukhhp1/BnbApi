using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BnbApi.DataTransfer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BnbApi.Services
{
    public class BusinessLogic
    {
        private readonly BnbDbContext _db;
        private readonly IWebHostEnvironment _env;

        public BusinessLogic(BnbDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        internal WorldScore GetTopTen(int? uid=0)
        {

            WorldScore ret = new WorldScore();
            ret.TopTen = new List<RankUser>();
            ret.UId = uid.Value;

            List<User> allData = _db.User.OrderByDescending(x => x.RankScore).ToList();
                //ret.TopTen = 

            var maxCount = (allData.Count < 50) ? allData.Count : 50;
            for (int i = 0; i < maxCount; i++)
            {
                var item = ConvertUserToRank(allData[i]);
                item.Rank = i + 1;
                ret.TopTen.Add(item);
            }

            if (uid.HasValue && uid > 0)
            {
                maxCount = allData.Count;
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
            RankUser rankUser = new RankUser();
            rankUser.Name = cUser.Name;
            rankUser.IsCurrentUser = true;
            rankUser.Score = cUser.Score;
            rankUser.UId = cUser.UId;
            rankUser.Location = cUser.Location;
            RankUser varOO0 = rankUser;
            int? int_ = cUser.RankScore;
            int varOO1;
            if (!int_.HasValue)
            {
                varOO1 = 0;
            }
            else
            {
                int_ = cUser.RankScore;
                varOO1 = int_.Value;
            }
            varOO0.RankScore = varOO1;
            RankUser varMF0 = rankUser;
            int_ = cUser.Stages;
            int varMF1;
            if (!int_.HasValue)
            {
                varMF1 = 0;
            }
            else
            {
                int_ = cUser.Stages;
                varMF1 = int_.Value;
            }
            varMF0.Stages = varMF1;
            RankUser varBM0 = rankUser;
            int_ = cUser.Starts;
            int varBM1;
            if (!int_.HasValue)
            {
                varBM1 = 0;
            }
            else
            {
                int_ = cUser.Starts;
                varBM1 = int_.Value;
            }
            varBM0.Stars = varBM1;
            if (rankUser.Stages > 0 && rankUser.Stars > 0)
            {
                rankUser.Rating = (double)(rankUser.Stars * 10 / (rankUser.Stages * 3));
            }
            rankUser.IsCurrentUser = false;
            return rankUser;
        }

        internal WorldScore WholeEntry(FirstEntry item)
        {

            if (item.UId == 0)
            {
                    User chkUser = null;
                    if (!string.IsNullOrEmpty(item.FbId))
                    {
                        var fbCheck = _db.User.Where(x => x.FbId == item.FbId).FirstOrDefault();
                        if (fbCheck != null)
                        {
                            chkUser = fbCheck;
                        }
                    }



                    if (chkUser == null)
                    {
                        var NameCheck = _db.User.Where(x => x.Name == item.Name).FirstOrDefault();
                        if (NameCheck != null)
                            chkUser = NameCheck;


                        if (chkUser == null)
                        {
                            User newEnt = new User();
                            newEnt.Name = item.Name;
                            newEnt.Guid = ParseClientGuid(item.Guid);
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
                            _db.User.Add(newEnt);
                            _db.SaveChanges();
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
            return GetTopTen(item.UId);
        }

        internal WorldScore UpdateWholeEntry(FirstEntry item)
        {

            if (item.UId != 0)
            {
                    User chkUser = _db.User.Where(x => x.UId == item.UId).FirstOrDefault(); 

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
                        _db.SaveChanges();
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

        internal GameVersion GetVersion()
        {
            GameVersion outcome = new GameVersion();
            outcome = _db.GameVersions.OrderByDescending(x => x.Id).FirstOrDefault();
            return outcome;
        }

        private static Guid ParseClientGuid(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw) || string.Equals(raw, "null", StringComparison.OrdinalIgnoreCase))
                return Guid.NewGuid();
            return Guid.TryParse(raw, out var g) ? g : Guid.NewGuid();
        }

        public void FixRanks()
        {
                List<User> list = _db.User.ToList<User>();
                foreach (User current in list)
                {
                    int value = this.CalculateRankScore(current);
                    current.RankScore = new int?(value);
                    _db.SaveChanges();
                }
        }

        public string testc()
        {
            string outcome;
            try
            {
                string sqlPath = Path.Combine(_env.ContentRootPath, "sql.txt");
                string cmdText = File.ReadAllText(sqlPath);
                _db.Database.ExecuteSqlRaw(cmdText);
            }
            catch (Exception varVL0)
            {
                Exception exception = varVL0;
                outcome = exception.Message + " - " + exception.StackTrace;
                return outcome;
            }
            outcome = "success";
            return outcome;
        }
    }
}
