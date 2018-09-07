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
	public class GameController : Controller
	{
		Biz_Game biz = new Biz_Game();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            DataTable dt = biz.GetGame(WebUtility.GetRequestByInt("game_idx"));
			//DataTable dt = biz.GetGameByUser(WebUtility.UserIdx());
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetGameLists() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("me_user_idx")))
                message += "me_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int total_count;
            int page_count;

            DataTable dt = biz.GetGameLists(WebUtility.GetRequestByInt("me_user_idx"), 
                                            WebUtility.GetRequest("type_name", "game"), 
                                            WebUtility.GetRequestByInt("page", 1), 
                                            WebUtility.GetRequestByInt("page_rows", 10),
                                            out total_count,
                                            out page_count);

            dt.Columns.Add("event_data", typeof(DataTable));
            dt.Columns.Add("event_data_count", typeof(int));

            Biz_Event biz_e = new Biz_Event();

            foreach(DataRow drGame in dt.Rows) 
            {
                int game_idx1               = drGame["game_idx"].ToInt();
                drGame["event_data"]        = biz_e.GetListByGame(game_idx1, 0);
                drGame["event_data_count"]  = ((DataTable)drGame["event_data"]).Rows.Count;
            }

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_cate_idx")))
				message += "game_cate_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_name")))
				message += "game_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_name_en")))
				message += "game_name_en is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_desc")))
				message += "game_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("package_name")))
				message += "package_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("bundle_id")))
				message += "bundle_id is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("release_date")))
				message += "release_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_last_version")))
				message += "game_last_version is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_seller_name")))
				message += "game_seller_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_copy_right")))
				message += "game_copy_right is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByFloat("game_rate")))
				message += "game_rate is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_device_type_idx")))
				message += "game_device_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_file_idx")))
				message += "game_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_back_file_idx")))
				message += "game_back_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
				message += "use_yn is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("create_date")))
				message += "create_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("create_user_idx")))
				message += "create_user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				int game_idx = biz.AddGame(	WebUtility.GetRequestByInt("game_cate_idx"),
											WebUtility.GetRequest("game_name"),
											WebUtility.GetRequest("game_name_en"),
											WebUtility.GetRequest("game_desc"),
											WebUtility.GetRequest("package_name"),
											WebUtility.GetRequest("bundle_id"),
											WebUtility.GetRequestByDateTime("release_date"),
											WebUtility.GetRequest("game_last_version"),
											WebUtility.GetRequest("game_seller_name"),
											WebUtility.GetRequest("game_copy_right"),
											WebUtility.GetRequestByFloat("game_rate"),
											WebUtility.GetRequestByInt("game_device_type_idx"),
											WebUtility.GetRequestByInt("game_file_idx"),
											WebUtility.GetRequestByInt("game_back_file_idx"),
											WebUtility.GetRequest("use_yn"),
											WebUtility.GetRequestByDateTime("create_date"),
											WebUtility.GetRequestByInt("create_user_idx"));

				if(game_idx > 0)
					json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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

		public ActionResult Modify()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_idx")))
				message += "game_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_cate_idx")))
				message += "game_cate_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_name")))
				message += "game_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_name_en")))
				message += "game_name_en is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_desc")))
				message += "game_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("package_name")))
				message += "package_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("bundle_id")))
				message += "bundle_id is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("release_date")))
				message += "release_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_last_version")))
				message += "game_last_version is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_seller_name")))
				message += "game_seller_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_copy_right")))
				message += "game_copy_right is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByFloat("game_rate")))
				message += "game_rate is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_device_type_idx")))
				message += "game_device_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_file_idx")))
				message += "game_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_back_file_idx")))
				message += "game_back_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
				message += "use_yn is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("update_date")))
				message += "update_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("update_user_idx")))
				message += "update_user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.ModifyGame(	WebUtility.GetRequestByInt("game_idx"),
												WebUtility.GetRequestByInt("game_cate_idx"),
												WebUtility.GetRequest("game_name"),
												WebUtility.GetRequest("game_name_en"),
												WebUtility.GetRequest("game_desc"),
												WebUtility.GetRequest("package_name"),
												WebUtility.GetRequest("bundle_id"),
												WebUtility.GetRequestByDateTime("release_date"),
												WebUtility.GetRequest("game_last_version"),
												WebUtility.GetRequest("game_seller_name"),
												WebUtility.GetRequest("game_copy_right"),
												WebUtility.GetRequestByFloat("game_rate"),
												WebUtility.GetRequestByInt("game_device_type_idx"),
												WebUtility.GetRequestByInt("game_file_idx"),
												WebUtility.GetRequestByInt("game_back_file_idx"),
												WebUtility.GetRequest("use_yn"),
												WebUtility.GetRequestByDateTime("update_date"),
												WebUtility.GetRequestByInt("update_user_idx"));

				if(isOK)
					json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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

		public ActionResult Remove()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_idx")))
				message += "game_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveGame(WebUtility.GetRequestByInt("game_idx"));

				if(isOK)
					json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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



//http://camptalk.me/camptest/Game/Get?game_idx=0
//http://camptalk.me/camptest/Game/Add?game_cate_idx=0&game_name=game_name&game_name_en=game_name_en&game_desc=game_desc&package_name=package_name&bundle_id=bundle_id&release_date=6&game_last_version=game_last_version&game_seller_name=game_seller_name&game_copy_right=game_copy_right&game_rate=10&game_device_type_idx=11&game_file_idx=12&game_back_file_idx=13&use_yn=use_yn&create_date=15&create_user_idx=16
//http://camptalk.me/camptest/Game/Modify?game_idx=0&game_cate_idx=1&game_name=game_name&game_name_en=game_name_en&game_desc=game_desc&package_name=package_name&bundle_id=bundle_id&release_date=7&game_last_version=game_last_version&game_seller_name=game_seller_name&game_copy_right=game_copy_right&game_rate=11&game_device_type_idx=12&game_file_idx=13&game_back_file_idx=14&use_yn=use_yn&update_date=16&update_user_idx=17
//http://camptalk.me/camptest/Game/Remove?game_idx=0

