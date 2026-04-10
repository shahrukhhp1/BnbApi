using System;
using System.Collections.Generic;
using BnbApi.DataTransfer;
using BnbApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BnbApi.Controllers
{
    [ApiController]
    [Produces("application/xml")]
    [Route("api/[controller]/[action]")]
    public class BnbController : ControllerBase
    {
        private readonly BusinessLogic _businessLogic;

        public BnbController(BusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
        }

        // GET api/Bnb/Get — same surface as legacy Web API (api/{controller}/{action})
        [HttpGet]
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
                ret.Data = _businessLogic.GetTopTen(uid);
            }
            catch (Exception ex) 
            {
                ret.IsSuccess = false;
                ret.Errors = new string[] { ex.Message.ToString(), ex.StackTrace.ToString() };
            }
            return ret;
        }

        [HttpGet]
        public DataTransfer<GameVersion> GetVersion()
        {
            DataTransfer<GameVersion> ret = new DataTransfer<GameVersion>();
            try
            {
                ret.Data = _businessLogic.GetVersion();
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
                ret.Data = _businessLogic.WholeEntry(item);
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
                ret.Data = _businessLogic.UpdateWholeEntry(item);
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
