using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using System.IO;
using Storichain.Models.Biz;
using System.Text;

using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Storichain.Controllers
{
    public class TestController : Controller
    {
        //public ActionResult Push() 
        //{
        //    string json = "";

        //    string message = "";

        //    if (!BizUtility.ValidCheck(WebUtility.GetRequest("user_idxs")))
        //        message += "user_idxs is null.\n";

        //    if (!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    var mdsClient = new MDSClient("w3wp");
        //    mdsClient.Connect();

        //    Biz_Device biz = new Biz_Device();
        //    DataTable dt = biz.GetDeviceSimpleByUserIdx(WebUtility.GetRequest("user_idxs"));

        //    Biz_Event biz_e = new Biz_Event(WebUtility.GetRequestByInt("event_idx", 2718));
        //    DataTable dt1 = biz_e.GetList(WebUtility.GetRequestByInt("event_idx", 2718), 0, 0, 0, 0, "Y", "", "");

        //    string url_string = dt1.Rows[0].ImageUrl("supply", "supply_thumb");

        //    var messageData = new PushNotificationMessageData 
        //    {
        //        device_token        = null,
        //        device_type_idx     = -1,
        //        badge               = 0,
        //        body                = biz_e.Supply_Name,
        //        message             = biz_e.Supply_Desc,
        //        //body = biz_e.Supply_Name,
        //        //body                = "❤️트알못女에게 추천! 삼성 페이 라이프스타일❤️",
        //        //message             = biz_e.Supply_Desc,
        //        sound               = "default",
        //        biz_type            = "new_post",
        //        user_idx            = 1,
        //        //to_user_idx_data= DataTypeUtility.GetSplitDataTable(dtNoticeIDs, devideCount, i),
        //        to_user_idx_data    = dt,
        //        notice_idx          = 0,
        //        board_idx           = 0,
        //        custom_data         = "move/event_view?event_idx=" + WebUtility.GetRequestByInt("event_idx", 2718) + ",image_url=" + url_string

        //        //move/event_view?event_idx=2722,image_url=http://www.na.df.com/dfs.png
        //        //move/product_view?product_idx=4828
        //        //move/event_board_view?board_idx=298
        //        //move/board_notice_view?board_idx=55
        //        //move/channel_view?channel_idx=5
        //        //move/user_view?user_idx=18
        //    };

        //    try 
        //    {    
        //        var requestMessage = mdsClient.CreateMessage();
        //        requestMessage.DestinationApplicationName = WebUtility.GetConfig("PUSH_NOTICE_BOARD", "BeautyServices.NoticeBoard");
        //        requestMessage.MessageData = GeneralHelper.SerializeObject(messageData);
        //        requestMessage.Send();
        //    }
        //    catch(Exception ex) 
        //    {
        //        BizUtility.SendErrorLog(ex);
        //    }    
                    
        //    mdsClient.Disconnect();
            
        //    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, url_string, messageData.custom_data);
        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}

        //public ActionResult Pushtest()
        //{
        //    string json = "";

        //    var mdsClient = new MDSClient("w3wp");
        //    mdsClient.Connect();

        //    Biz_Device biz = new Biz_Device();
        //    DataTable dt = biz.GetDeviceSimple(WebUtility.GetRequestByInt("user_idx", 3152));

        //    Biz_Event biz_e = new Biz_Event(2718);
        //    DataTable dt1 = biz_e.GetList(2718, 0, 0, 0, 0, "Y", "", "");

        //    var messageData = new PushNotificationMessageData
        //    {
        //        device_token = null,
        //        device_type_idx = -1,
        //        badge = 0,
        //        //body                = biz_e.Supply_Name,
        //        //message             = biz_e.Supply_Desc,
        //        //body                = biz_e.Supply_Name,
        //        body = "❤️트알못女에게 추천! 삼성 페이 라이프스타일❤️",
        //        message = biz_e.Supply_Desc,
        //        sound = "default",
        //        biz_type = "new_post",
        //        user_idx = 1,
        //        //to_user_idx_data= DataTypeUtility.GetSplitDataTable(dtNoticeIDs, devideCount, i),
        //        to_user_idx_data = dt,
        //        notice_idx = 0,
        //        board_idx = 0,
        //        custom_data = "move/event_view?event_idx=2718,image_url=" + dt1.Rows[0].ImageUrl("supply", "supply")

        //        //move/event_view?event_idx=2722,image_url=http://www.na.df.com/dfs.png
        //        //move/product_view?product_idx=4828
        //        //move/event_board_view?board_idx=298
        //        //move/board_notice_view?board_idx=55
        //        //move/channel_view?channel_idx=5
        //        //move/user_view?user_idx=18
        //    };

        //    try
        //    {
        //        var requestMessage = mdsClient.CreateMessage();
        //        requestMessage.DestinationApplicationName = WebUtility.GetConfig("PUSH_NOTICE_BOARD", "BeautyServices.NoticeBoard");
        //        requestMessage.MessageData = GeneralHelper.SerializeObject(messageData);
        //        requestMessage.Send();
        //    }
        //    catch (Exception ex)
        //    {
        //        BizUtility.SendErrorLog(ex);
        //    }

        //    mdsClient.Disconnect();

        //    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}

        public ActionResult CurlGet() 
        {
            string json = "";
            string result = BizUtility.GetKakaoOldUserID("70047663");
            
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, result, null);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NoticeBoard() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("board_idx")))
                message += "board_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            Biz_Board biz_board = new Biz_Board();
            int board_idx = WebUtility.GetRequestByInt("board_idx");
            DataTable dt = biz_board.GetBoard(WebUtility.GetRequestByInt("board_idx"), 0, "Y");

            Biz_User biz_user;
            
            if(dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                Dictionary<string, object> d = new Dictionary<string, object>();

                int user_idx = WebUtility.GetConfig("JOIN_ADD_USER_IDXS", "4").ToInt();

                biz_user                = new Biz_User(user_idx);
                d["from_user_idx"]      = user_idx;
                d["nick_name"]          = biz_user.Nick_Name;
                d["user_file_idx"]      = biz_user.File_Idx;
                d["notice_message"]     = dr["board_subject"].ToValue();
                d["type_name"]          = (dr["board_type_idx"].ToInt() == 1)?"notice_board":"event_board";
                d["board_idx"]          = board_idx;
                d["board_subject"]      = dr["board_subject"].ToValue();
                d["board_url"]          = dr["board_url"].ToValue();

                if(dr["board_type_idx"].ToInt() == 1)
                {
                    d["type_name"]          = "notice_board";
                    d["board_date"]         = dr["create_date"].ToDateTime().ToString("yyyy.MM.dd");
                    d["notice_date"]        = dr["create_date"].ToDateTime();
                }
                else
                {
                    d["type_name"]          = "event_board";
                    d["board_date"]         = string.Format("{0} ~ {1}", dr["publish_start_date"].ToDateTime().ToString("yyyy.MM.dd"), dr["publish_end_date"].ToDateTime().ToString("yyyy.MM.dd"));
                    d["notice_date"]        = dr["publish_start_date"].ToDateTime();
                    d["publish_start_date"] = dr["publish_start_date"].ToDateTime();
                    d["publish_end_date"]   = dr["publish_end_date"].ToDateTime();
                }

                MongoDBCommon.SendFeedBankAtBoardData(d);

                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }
            
            json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Post() 
        {
            string json = "";

            int event_idx = 2718;

            Biz_Event biz_e = new Biz_Event(event_idx);
            DataTable dt2 = biz_e.GetList(event_idx, 0, 0, 0, 0, "Y", "", "");
            DataRow dr = dt2.Rows[0];

            int user_idx = 0;
            string message_name = dr.ItemValue("supply_name");
            string biz_type = "new_post";
            
            Biz_User biz_user = new Biz_User(user_idx);
            Biz_Notice biz_notice = new Biz_Notice();

            DataTable dt1 = biz_notice.SendPushQueue(user_idx, 
                                                    message_name, 
                                                    biz_type, 
                                                    event_idx);

            //if(WebUtility.GetConfig("PUSH_NOTIFICATION_YN", "N").Equals("Y"))
            //{
                Biz_PushQueue biz_push = new Biz_PushQueue();
                int queue_idx = biz_push.AddPushQueue(  biz_type,
                                                        message_name,
                                                        DateTime.Now,
                                                        event_idx,
                                                        0,
                                                        0,
                                                        "Y",
                                                        DateTime.Now,
                                                        user_idx);
            //}  
            
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt1);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Resign() 
        {
            string json = ResignKakao(467422);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }


        private string ResignKakao(int user_idx)
        {
            Biz_User bizU = new Biz_User();
            string user_id = bizU.GetUserId(user_idx);

            string json = "";

            if(!user_id.Equals(""))
            {
                Curl curl = new Curl();
                json = curl.PostParam("https://kapi.kakao.com/v1/user/unlink", 
                                        new Dictionary<string, string>() {{"target_id_type", "user_id"}, { "target_id", user_id} }, 
                                        new System.Collections.Specialized.NameValueCollection() {{"Authorization" , WebUtility.GetConfig("KAKAO_CONVERTER_API_HEADER")}});
            }

            return json;
        }        

        public ActionResult Image(string id)
        {

            BizUtility.SendErrorLog(new Exception("test"), this.Request.UrlReferrer.OriginalString); 

            var dir = Server.MapPath("/Content");
            var path = Path.Combine(dir, "bi.png"); //validate the path for security or use other means to generate the path.
            return base.File(path, "image/png");
        }

        public ActionResult Test()
        {
            return Content("", "application/json", System.Text.Encoding.UTF8);
        }



        public ActionResult RabbitPublish() 
        {
            string json     = "";
            string message  = "";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("comment_coupon_idx")))
            //    message += "comment_coupon_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                var factory = new ConnectionFactory() { HostName = WebUtility.GetConfig("RABBITMQ_HOST"), UserName = WebUtility.GetConfig("RABBITMQ_ID"), Password = WebUtility.GetConfig("RABBITMQ_HOST") };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel()) {
                    channel.QueueDeclare(queue: WebUtility.GetConfig("RABBITMQ_QUE_NAME"),
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    DataTable dt = new DataTable();
                    DataRow dr = null;
                    dt.Columns.Add("user_idx", typeof(int));
                    dt.Columns.Add("device_type_idx", typeof(int));
                    dt.Columns.Add("device_token", typeof(string));

                    for(int i = 0; i < 100000; i++)
                    {
                        dr = dt.NewRow();
                        dr["user_idx"] = i + 1;
                        dr["device_type_idx"] = ((i + 1) % 2) == 0?1:2;
                        dr["device_token"] = Guid.NewGuid().ToString();
                        dt.Rows.Add(dr);
                    }

                    var messageData = new PushNotificationMessageData
                    {
                        device_token = null,
                        device_type_idx = -1,
                        badge = 0,
                        body = "❤️트알못女에게 추천! 삼성 페이 라이프스타일❤️",
                        message = "",
                        sound = "default",
                        biz_type = "new_post",
                        user_idx = 1,
                        to_user_idx_data = dt,
                        notice_idx = 0,
                        board_idx = 0,
                        custom_data = "move/event_view?event_idx=2722,image_url=https://cdn-azr.ssumer.com/wp-content/uploads/2018/07/ssumer_macOS_HighSierra_191412.jpg"
                    };
                    
                    var messageString = JsonConvert.SerializeObject(messageData);
                    var body = Encoding.UTF8.GetBytes(messageString);

                    channel.BasicPublish(exchange: "",
                                         routingKey: WebUtility.GetConfig("RABBITMQ_QUE_NAME"),
                                         basicProperties: properties,
                                         body: body);
                }


                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "rabbit", null);

            }catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }


        public ActionResult RabbitSubscribe() 
        {
            string json     = "";
            string message  = "";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("comment_coupon_idx")))
            //    message += "comment_coupon_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                PushNotificationMessageData userData = null;

                var factory = new ConnectionFactory() { HostName = WebUtility.GetConfig("RABBITMQ_HOST"), UserName = WebUtility.GetConfig("RABBITMQ_ID"), Password = WebUtility.GetConfig("RABBITMQ_HOST") };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: WebUtility.GetConfig("RABBITMQ_QUE_NAME"),
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new EventingBasicConsumer(channel);
                    
                    consumer.Received += (model, ea) => {
                        var body    = ea.Body;
                        var messageString = Encoding.UTF8.GetString(body);

                        userData = JsonConvert.DeserializeObject<PushNotificationMessageData>(messageString);


                        //Console.WriteLine(" [x] Received {0}", message);
                        //int dots = message.Split('.').Length - 1;
                        //Thread.Sleep(dots * 1000);

                        //Console.WriteLine(" [x] Done");

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    channel.BasicConsume(queue: WebUtility.GetConfig("RABBITMQ_QUE_NAME"),
                                         autoAck: false,
                                         consumer: consumer);
                }

                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "rabbit", userData);

            }catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

    }
}
