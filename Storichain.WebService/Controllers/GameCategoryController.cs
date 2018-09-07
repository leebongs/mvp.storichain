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
	public class GameCategoryController : Controller
	{
		Biz_GameCategory biz = new Biz_GameCategory();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetGameCategory(WebUtility.GetRequestByInt("game_cate_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_cate_name")))
				message += "game_cate_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_cate_name_en")))
				message += "game_cate_name_en is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_cate_desc")))
				message += "game_cate_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_cate_file_idx")))
				message += "game_cate_file_idx is null.";

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
				int game_cate_idx = biz.AddGameCategory(	WebUtility.GetRequest("game_cate_name"),
											WebUtility.GetRequest("game_cate_name_en"),
											WebUtility.GetRequest("game_cate_desc"),
											WebUtility.GetRequestByInt("game_cate_file_idx"),
											WebUtility.GetRequest("use_yn"),
											WebUtility.GetRequestByDateTime("create_date"),
											WebUtility.GetRequestByInt("create_user_idx"));

				if(game_cate_idx > 0)
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_cate_idx")))
				message += "game_cate_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_cate_name")))
				message += "game_cate_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_cate_name_en")))
				message += "game_cate_name_en is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("game_cate_desc")))
				message += "game_cate_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_cate_file_idx")))
				message += "game_cate_file_idx is null.";

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
				bool isOK = biz.ModifyGameCategory(	WebUtility.GetRequestByInt("game_cate_idx"),
												WebUtility.GetRequest("game_cate_name"),
												WebUtility.GetRequest("game_cate_name_en"),
												WebUtility.GetRequest("game_cate_desc"),
												WebUtility.GetRequestByInt("game_cate_file_idx"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_cate_idx")))
				message += "game_cate_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveGameCategory(WebUtility.GetRequestByInt("game_cate_idx"));

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



//http://camptalk.me/camptest/GameCategory/Get?game_cate_idx=0
//http://camptalk.me/camptest/GameCategory/Add?game_cate_name=game_cate_name&game_cate_name_en=game_cate_name_en&game_cate_desc=game_cate_desc&game_cate_file_idx=3&use_yn=use_yn&create_date=5&create_user_idx=6
//http://camptalk.me/camptest/GameCategory/Modify?game_cate_idx=0&game_cate_name=game_cate_name&game_cate_name_en=game_cate_name_en&game_cate_desc=game_cate_desc&game_cate_file_idx=4&use_yn=use_yn&update_date=6&update_user_idx=7
//http://camptalk.me/camptest/GameCategory/Remove?game_cate_idx=0

