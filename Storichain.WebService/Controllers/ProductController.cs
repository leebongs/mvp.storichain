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
	public class ProductController : Controller
	{
		Biz_Product biz = new Biz_Product();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
                message += "product_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetProduct(WebUtility.GetRequestByInt("product_idx"), 
                                            WebUtility.GetRequestByInt("me_user_idx", WebUtility.UserIdx()));

            dt.Columns.Add("product_color_data", typeof(DataTable));
            dt.Columns.Add("product_color_count", typeof(int));

            Biz_ProductColor biz_color = new Biz_ProductColor();

            foreach (DataRow dr in dt.Rows)
            {
                int product_idx             = dr["product_idx"].ToInt();
                dr["product_color_data"]    = biz_color.GetProductColor(product_idx, 0);;
                dr["product_color_count"]   = ((DataTable)dr["product_color_data"]).Rows.Count;
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetByEvent()
		{
			string json = "";
			string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetProductByEvent(WebUtility.GetRequestByInt("event_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetByStep(int event_idx, int step_idx)
		{
			string json = "";
			string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_idx")))
                message += "step_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetProductByStep(WebUtility.GetRequestByInt("event_idx"), WebUtility.GetRequestByInt("step_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}



        public ActionResult GetLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetLists(WebUtility.GetRequestByInt("user_idx"),
                                        WebUtility.GetRequestByInt("page", 1), 
                                        WebUtility.GetRequestByInt("page_rows", 25),
                                        out total_count,
                                        out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        private string GetSearchText(string searchText)
        {
            string[] strArr = searchText.Replace("+", " ").Split(' ');
            string temp = "";
            bool isFirst = true;

            foreach (string str in strArr)
            {
                if (str.Equals(""))
                    continue;

                if (isFirst)
                {
                    temp += string.Format("\"{0}*\"", str.Replace("'", "''"));
                    isFirst = false;
                }
                else
                {
                    temp += string.Format("OR \"{0}*\"", str.Replace("'", "''"));
                }
            }

            return temp;
        }

        public ActionResult SearchLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.SearchLists( WebUtility.GetRequestByInt("brand_idx"),
                                            WebUtility.GetRequestByInt("product_type_idx"),
                                            GetSearchText(WebUtility.GetRequest("product_name")),
                                            WebUtility.GetRequestByInt("page", 1), 
                                            WebUtility.GetRequestByInt("page_rows", 25),
                                            out total_count,
                                            out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult SelectListsByBrand()
        {
            int total_count;
            int page_count;

            DataTable dt = biz.SelectListsByBrand(WebUtility.GetRequestByInt("brand_idx"),
                                                WebUtility.GetRequestByInt("page", 1),
                                                WebUtility.GetRequestByInt("page_rows", 25),
                                                out total_count,
                                                out page_count);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt, dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_idx")))
				message += "brand_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_name")))
				message += "product_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_desc")))
				message += "product_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_size")))
				message += "product_size is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByFloat("product_price")))
				message += "product_price is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("skin_type_idx")))
				message += "skin_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_type_idx")))
				message += "product_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("gift_id")))
				message += "gift_id is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("publish_date")))
				message += "publish_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("cate_detail_name")))
				message += "cate_detail_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_file_idx")))
				message += "product_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("view_count")))
				message += "view_count is null.";

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
				int product_idx = biz.AddProduct(	WebUtility.GetRequestByInt("brand_idx"),
											        WebUtility.GetRequest("product_name"),
											        WebUtility.GetRequest("product_desc"),
											        WebUtility.GetRequest("product_size"),
											        WebUtility.GetRequestByFloat("product_price"),
                                                    WebUtility.GetRequest("product_tag"),
											        WebUtility.GetRequestByInt("skin_type_idx"),
											        WebUtility.GetRequestByInt("product_type_idx"),
											        WebUtility.GetRequest("gift_id"),
                                                    WebUtility.GetRequest("product_comment"),
											        WebUtility.GetRequestByDateTime("publish_date"),
											        WebUtility.GetRequest("cate_detail_name"),
											        WebUtility.GetRequestByInt("product_file_idx"),
											        WebUtility.GetRequestByInt("view_count"),
											        WebUtility.GetRequest("use_yn"),
                                                    WebUtility.GetRequestByDateTime("product_publish_date"),
                                                    WebUtility.GetRequest("product_remark"),
                                                    WebUtility.GetRequestByInt("product_emblem_idx"),
                                                    WebUtility.GetRequestByDateTime("create_date"),
											        WebUtility.GetRequestByInt("create_user_idx"));

				if(product_idx > 0)
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

        public ActionResult AddStepProduct()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_idx")))
				message += "step_idx is null.";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_idxs")))
				message += "product_idxs is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
                Biz_ProductStep biz_product_step = new Biz_ProductStep();

                foreach(string product_idx in WebUtility.GetRequest("product_idxs").Split(','))
                {
                    biz_product_step.AddProduct(	product_idx.ToInt(),
                                                    WebUtility.GetRequestByInt("event_idx"),
                                                    WebUtility.GetRequestByInt("step_idx"),
											        WebUtility.UserIdx());
                }

                Biz_Product biz_product = new Biz_Product();
                DataTable dtProduct = biz_product.GetProductByStep(WebUtility.GetRequestByInt("event_idx"), WebUtility.GetRequestByInt("step_idx"));
               
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dtProduct);
				
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_idx")))
				message += "brand_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_name")))
				message += "product_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_desc")))
				message += "product_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("product_size")))
				message += "product_size is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByFloat("product_price")))
				message += "product_price is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("skin_type_idx")))
				message += "skin_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_type_idx")))
				message += "product_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("gift_id")))
				message += "gift_id is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("publish_date")))
				message += "publish_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("cate_detail_name")))
				message += "cate_detail_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_file_idx")))
				message += "product_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("view_count")))
				message += "view_count is null.";

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
				bool isOK = biz.ModifyProduct(	WebUtility.GetRequestByInt("product_idx"),
												WebUtility.GetRequestByInt("brand_idx"),
												WebUtility.GetRequest("product_name"),
												WebUtility.GetRequest("product_desc"),
												WebUtility.GetRequest("product_size"),
												WebUtility.GetRequestByFloat("product_price"),
                                                WebUtility.GetRequest("product_tag"),
												WebUtility.GetRequestByInt("skin_type_idx"),
												WebUtility.GetRequestByInt("product_type_idx"),
												WebUtility.GetRequest("gift_id"),
                                                WebUtility.GetRequest("product_comment"),
												WebUtility.GetRequestByDateTime("publish_date"),
												WebUtility.GetRequest("cate_detail_name"),
												WebUtility.GetRequestByInt("product_file_idx"),
												WebUtility.GetRequestByInt("view_count"),
												WebUtility.GetRequest("use_yn"),
                                                WebUtility.GetRequestByDateTime("product_publish_date"),
                                                WebUtility.GetRequest("product_remark"),
                                                WebUtility.GetRequestByInt("product_emblem_idx"),
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

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
                message += "product_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isOK = biz.RemoveProduct(WebUtility.GetRequestByInt("product_idx"));

                if (isOK)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch (Exception ex)
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }




        public ActionResult RemoveStepProduct()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
				message += "product_idx is null.";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_idx")))
				message += "step_idx is null.";
            
			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
                Biz_ProductStep biz_step = new Biz_ProductStep();
                bool isOK = biz_step.RemoveProductStep( WebUtility.GetRequestByInt("product_idx"),
                                                        WebUtility.GetRequestByInt("event_idx"),
                                                        WebUtility.GetRequestByInt("step_idx"));

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


        public ActionResult ShareSNSProduct() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
                message += "product_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sns_type_idx")))
                message += "sns_type_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                Biz_UserSNSShare biz = new Biz_UserSNSShare();
                bool isOK = biz.AddUserSSNSShareProduct (   WebUtility.UserIdx(),
                                                            WebUtility.GetRequestByInt("product_idx"), 
                                                            WebUtility.GetRequestByInt("sns_type_idx"), 
                                                            DateTime.Now, 
                                                            WebUtility.UserIdx());

                if(isOK) 
                {
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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



//http://camptalk.me/camptest/Product/Get?product_idx=0
//http://camptalk.me/camptest/Product/Add?brand_idx=0&product_name=product_name&product_desc=product_desc&product_size=product_size&product_price=4&skin_type_idx=5&product_type_idx=6&gift_id=gift_id&publish_date=8&cate_detail_name=cate_detail_name&product_file_idx=10&view_count=11&use_yn=use_yn&create_date=13&create_user_idx=14&pid=pid&bid=bid&skind=skind&pkind=pkind&file_rname=file_rname&file_directory=file_directory&file_url=file_url
//http://camptalk.me/camptest/Product/Modify?product_idx=0&brand_idx=1&product_name=product_name&product_desc=product_desc&product_size=product_size&product_price=5&skin_type_idx=6&product_type_idx=7&gift_id=gift_id&publish_date=9&cate_detail_name=cate_detail_name&product_file_idx=11&view_count=12&use_yn=use_yn&update_date=14&update_user_idx=15&pid=pid&bid=bid&skind=skind&pkind=pkind&file_rname=file_rname&file_directory=file_directory&file_url=file_url
//http://camptalk.me/camptest/Product/Remove?product_idx=0

