using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Collections;
using Storichain.Models.Biz;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Net;
using System.Drawing;

namespace Storichain.Controllers
{
    public class BroadcastController : Controller
    {
        Biz_User biz_u                  = new Biz_User();
        Biz_Event biz_e                 = new Biz_Event();
        Biz_Supply biz_supply           = new Biz_Supply();
        Biz_Step biz_step               = new Biz_Step();
        Biz_CollectionEvent biz_coll    = new Biz_CollectionEvent();

        public ActionResult Ready() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequest("package_names")))
            //    message += "package_names is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            Biz_Channel biz_c = new Biz_Channel();
            DataTable dtChannel = biz_c.GetChannelByPackageNames(user_idx, WebUtility.GetRequest("package_names"));
            
            Biz_Code biz_code = new Biz_Code();
            DataTable dtCodeO = biz_code.GetCode(2);

            DataTable dtCode = new DataTable();
            DataRow drCode;
            dtCode.Columns.Add("topic_idx", typeof(int));
            dtCode.Columns.Add("topic_name", typeof(string));

            foreach(DataRow dr1 in dtCodeO.Rows) 
            {
                drCode = dtCode.NewRow();
                drCode["topic_idx"] = dr1["code_value"].ToInt();
                drCode["topic_name"] = dr1["code_name"];
                dtCode.Rows.Add(drCode);
            }

            DataTable dtUser = biz_u.GetUser(user_idx, 0);

            Dictionary<string, object> dic = new Dictionary<string,object>();
            dic.Add("channel", dtChannel);
            dic.Add("topic", dtCode);
            dic.Add("user", dtUser);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult RecordStart() 
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

            int user_idx = WebUtility.UserIdx();

            Biz_User biz_user   = new Biz_User(user_idx);
            WebResponseData result = UstreamHelper.Recording(biz_user.ChannelID, true);

            if(result.ResponseCode.ToInt() == 202)
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, result.ResponseCode, null);
            else
                json = DataTypeUtility.JSon("2000", Config.R_FAIL, result.ResponseCode, null);

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Start() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("topic_idx")))
                message += "topic_idx is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_idx")))
            //    message += "channel_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            if(WebUtility.GetConfig("BLOCK_USE_YN", "N").Equals("Y")) 
            {
                Biz_UserBlock biz_block = new Biz_UserBlock(user_idx);
                if(biz_block.Post_Block_YN.Equals("Y") || biz_block.User_Block_YN.Equals("Y"))
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "Logout", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }    
            }

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("supply_name")))
                message += "supply_name is null.\n";

            if(WebUtility.GetRequestByInt("topic_idx") == 1) 
            {
                    
            }
            else if(WebUtility.GetRequestByInt("topic_idx") == 2) 
            {
                
            }
            else if(WebUtility.GetRequestByInt("topic_idx") == 3) 
            {
                    
            }

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            string file_path1 = "event";
            string image_key1 = "";
            ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

            int file_idx = (WebUtility.GetRequestByInt("supply_file_idx") > 0)? WebUtility.GetRequestByInt("supply_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1);
            if(file_idx == 0)
                file_idx = 1791;

            DataTable dt1 = biz_e.GetEventByDataType(user_idx, 4);

            foreach(DataRow dr in dt1.Rows) 
            {
                int event_idx1 = dr["event_idx"].ToInt();

                Biz_User biz_user   = new Biz_User(user_idx);
                Biz_Event biz_ee    = new Biz_Event(event_idx1);
                Biz_Supply biz_s    = new Biz_Supply();
                biz_ee.ModifyOnAirStop(user_idx);
                WebResponseData rVideo  = UstreamHelper.Recording(biz_user.ChannelID, false);

                if(rVideo.ResponseCode.ToInt() >= 200 && rVideo.ResponseCode.ToInt() < 300)
                {
                    var resultVideo  = JsonConvert.DeserializeObject<dynamic>(rVideo.ResponseString);

                    if(resultVideo.Count == 0)
                    {
                        biz_e.ModifyUseYN(event_idx1, user_idx, "N", DateTime.Now, user_idx);
                        MongoDBCommon.RemovePost(event_idx1);
                        continue;
                    }

                    string video_id = resultVideo.video.id;

                    WebResponseData rVideoDetail = UstreamHelper.GetVideoDetail(video_id);

                    if(rVideoDetail.ResponseCode.ToInt() >= 200 && rVideoDetail.ResponseCode.ToInt() < 300)
                    {
                        var resultVideoDetail  = JsonConvert.DeserializeObject<dynamic>(rVideoDetail.ResponseString);

                        UstreamHelper.ModifyVideoDetail(video_id, biz_ee.Supply_Name, biz_ee.Supply_Desc);
                        UstreamHelper.ModifyVideoExpiration(video_id);
                    
                        string url      = resultVideoDetail.video.url;
                        float length    = DataTypeUtility.GetToFloat(resultVideoDetail.video.length);
                        string img_url  = resultVideoDetail.video.thumbnail["default"];

                        //string file_path11 = "event";
                        //string image_key11 = "";
                        //ArrayList list11 = ImageUtility.GetImageSizes(file_path11, ref image_key11); 

                        if(length > 0) 
                        {
                            try 
                            {
                                biz_s.ModifyUstreamData(event_idx1, 
                                                        video_id, 
                                                        url, 
                                                        length, 
                                                        3,
                                                        0,
                                                        //BizUtility.SaveFromUrl(img_url, file_path1, list1, image_key1),
                                                        DateTime.Now, 
                                                        user_idx);
                            }
                            catch(Exception ex) 
                            {
                                BizUtility.SendErrorLog(Request, ex);
                            }    
                        }
                        else 
                        {
                            biz_e.ModifyUseYN(event_idx1, user_idx, "N", DateTime.Now, user_idx);
                            MongoDBCommon.RemovePost(event_idx1);

                            DataTable dt2 = biz_ee.GetEvent(event_idx1);

                            if(dt2.Rows.Count > 0) 
                            {
                                if(!dt2.Rows[0]["video_id"].ToString().Equals(""))
                                {
                                    UstreamHelper.RemoveVideoDetail(dt2.Rows[0]["video_id"].ToString());
                                }
                            }
                        }
                    }
                }
            }

            int event_idx = biz_e.AddEventWithSupply(WebUtility.GetRequestByInt("topic_idx"), 
                                                    WebUtility.GetRequestByInt("channel_idx", 1), 
                                                    user_idx, 
                                                    WebUtility.GetRequest("publish_yn","Y"),
                                                    WebUtility.GetRequest("supply_name"),
                                                    WebUtility.GetRequest("supply_desc"),
                                                    WebUtility.GetRequest("supply_brand_name"),
                                                    WebUtility.GetRequest("supply_items"),
                                                    WebUtility.GetRequest("supply_location_name"),
                                                    WebUtility.GetRequestByFloat("supply_pos_x"),
                                                    WebUtility.GetRequestByFloat("supply_pos_y"),
                                                    WebUtility.GetRequest("supply_tel"),
                                                    WebUtility.GetRequestByInt("supply_price"),
                                                    WebUtility.GetRequestByInt("supply_price_origin"),
                                                    WebUtility.GetRequestByInt("supply_type_idx"),
                                                    WebUtility.GetRequest("supply_type_name"),
                                                    WebUtility.GetRequestByInt("supply_type2_idx"),
                                                    WebUtility.GetRequest("supply_type2_name"),
                                                    WebUtility.GetRequest("supply_start_date"),
                                                    WebUtility.GetRequest("supply_end_date"),
                                                    WebUtility.GetRequest("supply_date"),
                                                    WebUtility.GetRequest("supply_url"),
                                                    WebUtility.GetRequest("supply_web_title"),
                                                    WebUtility.GetRequestByInt("supply_time"),
                                                    WebUtility.GetRequestByInt("supply_count"),
                                                    WebUtility.GetRequest("supply_text"),
                                                    WebUtility.GetRequest("supply_tip"),
                                                    WebUtility.GetRequest("supply_origin"),
                                                    WebUtility.GetRequestByInt("owner_idx"),
                                                    "",
                                                    WebUtility.GetRequest("transaction_yn"),
                                                    WebUtility.GetRequest("supply_pic_name"),
                                                    WebUtility.GetRequestByInt("data_type_idx", 4),
                                                    file_idx,
                                                    WebUtility.GetDeviceTypeIdx(),
                                                    WebUtility.GetRequest("on_air_yn", "Y"),
                                                    WebUtility.GetRequest("temp_yn", "N"),
                                                    "N",
                                                    "",
                                                    DateTime.Now,
                                                    DateTime.Now,
                                                    WebUtility.GetRequestByInt("orientation_type_idx", 1),
                                                    WebUtility.GetRequestByInt("event_content_type_idx", 1),
                                                    WebUtility.GetRequestByInt("shop_product_idx", 0),
                                                    WebUtility.GetRequest("private_view_yn", "N"),
                                                    DateTime.Now,
                                                    user_idx
                                                    );

            if(WebUtility.GetRequestByInt("coll_idx") > 0) 
            {
                biz_coll.AddCollectionEvent(user_idx, WebUtility.GetRequestByInt("coll_idx"), event_idx, DateTime.Now, user_idx);
            }
            else if(WebUtility.GetRequestByInt("coll_idx") < 0)
            {
                biz_coll.RemoveCollectionEvent(event_idx);
            }

            DataTable dt = biz_e.GetList(event_idx, 0, 0, 0, 0, "", "", "");

            if(WebUtility.GetRequestByInt("coll_idx") > 0)  
            {
                dt.Columns.Add("coll_data", typeof(DataTable));
                dt.Columns.Add("coll_data_count", typeof(int));

                if(dt.Rows.Count > 0) 
                {
                    dt.Rows[0]["coll_data"]         = biz_coll.Get(user_idx, event_idx);
                    dt.Rows[0]["coll_data_count"]   = ((DataTable)dt.Rows[0]["coll_data"]).Rows.Count;
                }    
            }

            if(dt.Rows.Count > 0) 
            {
                Biz_User biz_user       = new Biz_User(user_idx);
                Biz_Notice biz_notice   = new Biz_Notice();

                //UstreamHelper.Recording(biz_user.ChannelID, true);

                if(WebUtility.GetRequest("push_yn", "Y").Equals("Y")) 
                {
                    if(WebUtility.GetConfig("NOTICE_YN").Equals("Y")) 
                    {
                        if(dt.Rows.Count > 0) 
                        {
                            biz_notice.SendNoticeDataTable( user_idx, 
                                                            null,
                                                            string.Format("[{0} 님의 라이브방송] {1}", biz_user.Nick_Name, WebUtility.GetRequest("supply_name")), 
                                                            "1",
                                                            "new_post", 
                                                            0, 
                                                            event_idx,
                                                            0,
                                                            0);
                        }
                    }
                }

                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            }
            else
            {
                json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ModifyCoverImage() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                string file_path1 = "event";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                Biz_Supply biz_s = new Biz_Supply();
                bool isOK = biz_s.ModifySupplyFile(WebUtility.GetRequestByInt("event_idx"), 
                                                  (WebUtility.GetRequestByInt("supply_file_idx") > 0)? WebUtility.GetRequestByInt("supply_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1));

                if(isOK) 
                {
                    Biz_Event biz_ee  = new Biz_Event();
                    DataTable dt      = BizUtility.GetImageData(biz_ee.GetEvent(WebUtility.GetRequestByInt("event_idx")));
                    MongoDBCommon.UpdateSupplyCoverImage(dt);

                    //json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);

                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Stop() 
        {
            string json = "";
            string message = "";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
            //    message += "event_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int event_idx = WebUtility.GetRequestByInt("event_idx");
            int user_idx = WebUtility.UserIdx();

            Biz_User biz_user   = new Biz_User(user_idx);
            Biz_Event biz_e     = new Biz_Event(event_idx);
            Biz_Supply biz_s    = new Biz_Supply();

            WebResponseData rVideo;

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
            {
                rVideo  = UstreamHelper.Recording(biz_user.ChannelID, false);

                if(rVideo.ResponseCode.ToInt() >= 200 && rVideo.ResponseCode.ToInt() < 300)
                {
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                json = DataTypeUtility.JSon("2000", Config.R_FAIL, "stop fail", null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if(biz_e.FileIdx == 1791)
            {
                biz_e.ModifyUseYN(event_idx, user_idx, "N", DateTime.Now, user_idx);
                MongoDBCommon.RemovePost(event_idx);

                DataTable dt1 = biz_e.GetEvent(event_idx);

                if(dt1.Rows.Count > 0) 
                {
                    if(!dt1.Rows[0]["video_id"].ToString().Equals(""))
                    {
                        UstreamHelper.RemoveVideoDetail(dt1.Rows[0]["video_id"].ToString());
                    }
                }

                json = DataTypeUtility.JSon("2000", "no cover image", "", null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            biz_e.ModifyOnAirStop(user_idx);
            rVideo  = UstreamHelper.Recording(biz_user.ChannelID, false);

            if(rVideo.ResponseCode.ToInt() >= 200 && rVideo.ResponseCode.ToInt() < 300)
            {
                var resultVideo  = JsonConvert.DeserializeObject<dynamic>(rVideo.ResponseString);

                if(resultVideo.Count == 0)
                {
                    biz_e.ModifyUseYN(event_idx, user_idx, "N", DateTime.Now, user_idx);
                    MongoDBCommon.RemovePost(event_idx);

                    json = DataTypeUtility.JSon("2000", Config.R_NONE_DATA, "", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                string video_id = resultVideo.video.id;

                WebResponseData rVideoDetail = UstreamHelper.GetVideoDetail(video_id);

                if(rVideoDetail.ResponseCode.ToInt() >= 200 && rVideoDetail.ResponseCode.ToInt() < 300)
                {
                    var resultVideoDetail  = JsonConvert.DeserializeObject<dynamic>(rVideoDetail.ResponseString);

                    UstreamHelper.ModifyVideoDetail(video_id, biz_e.Supply_Name, biz_e.Supply_Desc);
                    UstreamHelper.ModifyVideoExpiration(video_id);
                    
                    string url      = resultVideoDetail.video.url;
                    float length    = DataTypeUtility.GetToFloat(resultVideoDetail.video.length);
                    string img_url  = resultVideoDetail.video.thumbnail["default"];

                    string file_path1 = "event";
                    string image_key1 = "";
                    ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                    if(length > 0) 
                    {
                        try 
                        {
                            biz_s.ModifyUstreamData(event_idx, 
                                                    video_id, 
                                                    url, 
                                                    length, 
                                                    3,
                                                    0,
                                                    //BizUtility.SaveFromUrl(img_url, file_path1, list1, image_key1),
                                                    DateTime.Now, 
                                                    user_idx);
                        }
                        catch(Exception ex) 
                        {
                            BizUtility.SendErrorLog(Request, ex);
                            json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
                        }

                        json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", biz_e.GetList(event_idx, 0, 0, 0, 0, "", "", ""));
                        return Content(json, "application/json", System.Text.Encoding.UTF8);    
                    }
                    else 
                    {
                        biz_e.ModifyUseYN(event_idx, user_idx, "N", DateTime.Now, user_idx);
                        MongoDBCommon.RemovePost(event_idx);

                        DataTable dt1 = biz_e.GetEvent(event_idx);

                        if(dt1.Rows.Count > 0) 
                        {
                            if(!dt1.Rows[0]["video_id"].ToString().Equals(""))
                            {
                                UstreamHelper.RemoveVideoDetail(dt1.Rows[0]["video_id"].ToString());
                            }
                        }

                        json = DataTypeUtility.JSon("2000", "data length is zero", "", null);
                        return Content(json, "application/json", System.Text.Encoding.UTF8);
                    }
                }
            }

            json = DataTypeUtility.JSon("2000", Config.R_NONE_DATA, "", null);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult CreateChannelId() 
        {
            string json = "";
            string message = "";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            Biz_User biz_u = new Biz_User();
            DataTable dt = biz_u.GetAll();

            foreach(DataRow dr in dt.Rows) 
            {
                int user_idx = dr["user_idx"].ToInt();

                WebResponseData rChannel   = UstreamHelper.CreateChannel(user_idx);

                if(rChannel.ResponseCode.ToInt() >= 200 && rChannel.ResponseCode.ToInt() < 300)
                {
                    var resultChannel  = JsonConvert.DeserializeObject<dynamic>(rChannel.ResponseString);
                    string channel_id = resultChannel.channel.id;

                    WebResponseData rChannelKey = UstreamHelper.GetChannelKey(channel_id);
                    var resultChannelKey = JsonConvert.DeserializeObject<dynamic>(rChannelKey.ResponseString);
                    string channel_key = resultChannelKey.channel_key;

                    biz_u.ModifyChannelInfo(user_idx, channel_id, channel_key);
                }
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult AllowAutorecode() 
        {
            string json = "";
            string message = "";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            Biz_User biz_u = new Biz_User();
            DataTable dt = biz_u.GetAll();

            foreach(DataRow dr in dt.Rows) 
            {
                string channel_id = dr["channel_id"].ToValue();

                WebResponseData rChannel = UstreamHelper.Autorecord(channel_id);

                //if(rChannel.ResponseCode.ToInt() >= 200 && rChannel.ResponseCode.ToInt() < 300)
                //{
                //    string a = "";
                //}
                //else 
                //{
                //    string b = "";
                //}
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }



        



    }
}
