using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;
using Newtonsoft.Json;

namespace Storichain.Controllers
{
	public class FollowGameController : Controller
	{
		Biz_FollowGame biz = new Biz_FollowGame();

		public ActionResult GetFollow() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            DataTable dt = null;

            if(WebUtility.GetRequestByInt("game_idx") == 0)
                dt = biz.GetFollowAll( WebUtility.UserIdx());
            else 
                dt = biz.GetFollow(WebUtility.UserIdx(),
                                    WebUtility.GetRequestByInt("game_idx"));

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Toggle() 
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_idx")))
                    message += "game_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int user_idx   = WebUtility.UserIdx();
                int game_idx   = WebUtility.GetRequestByInt("game_idx");

                bool isOK = biz.Toggle( user_idx, 
                                        game_idx, 
                                        DateTime.Now,
                                        DateTime.Now, 
                                        user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz.GetFollow(user_idx, game_idx);
                    
                    if(dt.Rows.Count > 0) 
                    {
                        DataTable dtG = biz.GetFollowChannel(user_idx, game_idx);

                        if(dt.Rows[0]["follow_yn"].ToString().Equals("Y")) 
                        {
                            foreach(DataRow drG in dtG.Rows) 
                            {
                                MongoDBCommon.FollowChannelYes(user_idx, drG["channel_idx"].ToInt());
                            }
                        }
                        else 
                        {
                            foreach(DataRow drG in dtG.Rows) 
                            {
                                MongoDBCommon.FollowChannelNo(user_idx, drG["channel_idx"].ToInt());
                            }
                        }
                    
                        json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                    }
                    
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                }
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ToggleAll() 
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!BizUtility.ValidCheck(WebUtility.GetRequest("follow_yn")))
                    message += "follow_yn is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int user_idx        = WebUtility.UserIdx();
                string follow_yn    = WebUtility.GetRequest("follow_yn");

                bool isOK = biz.ToggleAll(  user_idx, 
                                            follow_yn,
                                            DateTime.Now,
                                            DateTime.Now, 
                                            user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz.GetFollowAll(user_idx);
                    
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                }
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }
	}
}
