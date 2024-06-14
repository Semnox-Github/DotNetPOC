using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
//using Semnox.Parafait;


namespace Semnox.Core.Utilities
{
    public class MessageUtils
    {
        DBUtils Utilities;
        ParafaitEnv env;

        public MessageUtils(DBUtils dbUtils)
            : this(dbUtils, new ParafaitEnv(dbUtils))
        {

        }
        public MessageUtils(DBUtils ParafaitUtilities, ParafaitEnv env)
        {
            Utilities = ParafaitUtilities;
            this.env = env;
        }

        public class messageClass
        {
            public int LanguageId;
            public int Messageno;
            public string Message;
        }
        
        List<messageClass> messageList = new List<messageClass>();
        private string getMessage(int MessageNo, int LanguageId, params object[] Params)
        {
            string message = findInList(MessageNo, LanguageId);
            if (message == null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Utilities.createConnection();
                cmd.CommandText = @"select isnull(tl.Message, m.Message)
                                                 from Messages m left outer join MessagesTranslated tl
                                                      on m.MessageId = tl.MessageId
                                                      and tl.LanguageId = @langId
                                                 where m.MessageNo = @messageNo";
                
                cmd.Parameters.AddWithValue("@messageNo", MessageNo);
                cmd.Parameters.AddWithValue("@langId", LanguageId);
                object o = cmd.ExecuteScalar();
                cmd.Connection.Close();

                if (o != null)
                {
                    message = o.ToString();
                    messageClass msg = new messageClass();
                    msg.Messageno = MessageNo;
                    msg.LanguageId = LanguageId;
                    msg.Message = message;
                    messageList.Add(msg);
                }
                else
                    return "Message not defined for Message No: " + MessageNo.ToString();
            }

            for (int i = 0; i < Params.Length; i++)
            {
                message = message.Replace("&" + (i + 1).ToString(), Params[i].ToString());
            }
            return message;
        }

        string findInList(int msgNo, int langId)
        {
            foreach (messageClass msgObject in messageList)
            {
                if (msgObject.Messageno == msgNo && msgObject.LanguageId == langId)
                    return msgObject.Message;
            }
            return null;
        }

        private string getMessage(string Message, int LanguageId, params object[] Params)
        {
            string message = findInList(Message, LanguageId);
            if (message == null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Utilities.createConnection();
                cmd.CommandText = @"select isnull(tl.Message, @message)
                                                 from Messages m left outer join MessagesTranslated tl
                                                      on m.MessageId = tl.MessageId
                                                      and tl.LanguageId = @langId
                                                 where m.Message = @message
                                                 and m.MessageNo >= 10000";

                cmd.Parameters.AddWithValue("@message", Message);
                cmd.Parameters.AddWithValue("@langId", LanguageId);
                object o = cmd.ExecuteScalar();
                cmd.Connection.Close();

                if (o != null)
                    message = o.ToString();
                else
                    message = Message;

                messageClass msg = new messageClass();
                msg.LanguageId = LanguageId;
                msg.Message = message;
                messageList.Add(msg);
            }

            for (int i = 0; i < Params.Length; i++)
            {
                message = message.Replace("&" + (i + 1).ToString(), Params[i].ToString());
            }
            return message;
        }

        string findInList(string msg, int langId)
        {
            foreach (messageClass msgObject in messageList)
            {
                if (string.Compare(msgObject.Message, msg, true) == 0  && msgObject.LanguageId == langId)
                    return msgObject.Message;
            }
            return null;
        }

        public string getMessage(int MessageNo, params object[] Params)
        {
            return getMessage(MessageNo, env.LanguageId, Params);
        }

        public string getMessageInLanguage(int MessageNo, int LanguageId, params object[] Params)
        {
            return getMessage(MessageNo, LanguageId, Params);
        }

        public string getMessage(string Message, params object[] Params)
        {
            return getMessage(Message, env.LanguageId, Params);
        }
    }
}
