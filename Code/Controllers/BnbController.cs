using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BnbApi.DataTransfer;


namespace BnbApi.Controllers
{
    public class BnbController : ApiController
    {
        Services.BusinessLogic bLayer = new Services.BusinessLogic();
        // GET api/bnb
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public DataTransfer<WorldScore> GetTopTen(int? uid=0)
        {
            DataTransfer<WorldScore> ret = new DataTransfer<WorldScore>();
            try
            {
                ret.Data = bLayer.GetTopTen(uid);
            }
            catch (Exception ex) 
            {
                ret.IsSuccess = false;
                ret.Errors = new string[] { ex.Message.ToString(), ex.StackTrace.ToString() };
            }
            return ret;
        }


        [HttpPost]
        public DataTransfer<WorldScore> WholeEntry([FromBody]FirstEntry item)
        {
            DataTransfer<WorldScore> ret = new DataTransfer<WorldScore>();
            try
            {
                ret.Data = bLayer.WholeEntry(item);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.Errors = new string[] { ex.Message.ToString() };
            }
            return ret;
        }

        //

        [HttpPost]
        public DataTransfer<WorldScore> UpdateEntry([FromBody]FirstEntry item)
        {
            DataTransfer<WorldScore> ret = new DataTransfer<WorldScore>();
            try
            {
                ret.Data = bLayer.UpdateWholeEntry(item);
            }
            catch (Exception ex)
            {
                ret.IsSuccess = false;
                ret.Errors = new string[] { ex.Message.ToString() };
            }
            return ret;
        }
    }
}
