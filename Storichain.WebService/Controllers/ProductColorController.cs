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
	public class ProductColorController : Controller
	{
		Biz_ProductColor biz = new Biz_ProductColor();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
				message += "product_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
				message += "sort_order is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetProductColor(	WebUtility.GetRequestByInt("product_idx"),
										WebUtility.GetRequestByInt("sort_order"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
				message += "product_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_color_name")))
				message += "product_color_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_color_rgb")))
				message += "product_color_rgb is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
				message += "sort_order is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_color_file_idx")))
				message += "product_color_file_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.AddProductColor(	WebUtility.GetRequestByInt("product_idx"),
											        WebUtility.GetRequest("product_color_name"),
											        WebUtility.GetRequest("product_color_rgb"),
											        WebUtility.GetRequestByInt("sort_order"),
											        WebUtility.GetRequestByInt("product_color_file_idx"),
											        DateTime.Now,
											        WebUtility.UserIdx());

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

		public ActionResult Modify()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
				message += "product_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_color_name")))
				message += "product_color_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_color_rgb")))
				message += "product_color_rgb is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
				message += "sort_order is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_color_file_idx")))
				message += "product_color_file_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.ModifyProductColor(	WebUtility.GetRequestByInt("product_idx"),
												    WebUtility.GetRequest("product_color_name"),
												    WebUtility.GetRequest("product_color_rgb"),
												    WebUtility.GetRequestByInt("sort_order"),
												    WebUtility.GetRequestByInt("product_color_file_idx"),
												    DateTime.Now,
												    WebUtility.UserIdx());

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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
				message += "product_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
				message += "sort_order is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveProductColor(	WebUtility.GetRequestByInt("product_idx"),
												WebUtility.GetRequestByInt("sort_order"));

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
