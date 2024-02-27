using Dapper;

using rlqb_client.core;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace rlqb_client.utils
{
    internal class SqlUtil
    {
        private static List<T> selectAll<T>(string dbpath,string sql)
        {
            string connectionString = @"Data source="+dbpath+";Version=3;";
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();

                // 执行查询并映射到对象集合
                List<T> users = dbConnection.Query<T>(sql).AsList();
                return users;
            }
            return null;
        }

        private static T selectOne<T>(string dbpath, string sql)
        {
            string connectionString = @"Data source="+dbpath+";Version=3;";
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();

                // 执行查询并映射到对象集合
                T users = dbConnection.Query<T>(sql).FirstOrDefault();
                return users;
            }
           
        }

        public static List<ChatRoom> getChatRooms(string dbpath)
        {
            string sql = "SELECT a.rowid,a.ChatRoomName,a.UserNameList,b.NickName,b.Remark FROM \"main\".\"ChatRoom\" a left join \"main\".\"Contact\" b on a.ChatRoomName=b.Username";
            List<ChatRoom> chatRooms=   selectAll<ChatRoom>(dbpath, sql);
            foreach(ChatRoom chatRoom in chatRooms)
            {
                string[] users= chatRoom.UserNameList.Split("^G".ToCharArray());
                chatRoom.UserNameLists=users.ToList<string>();
            }
            return chatRooms;
        }


        public static List<User> geUsersAll(string dbpath)
        {
            string sql = "SELECT a.UserName,a.NickName,a.Remark,b.bigHeadImgUrl,a.rowid \"NAVICAT_ROWID\" FROM \"main\".\"Contact\" a left join \"main\".\"ContactHeadImgUrl\" b on a.UserName=b.usrName";
            List<User> chatRooms = selectAll<User>(dbpath, sql);
            return chatRooms;
        }

        public static User getUserByUserName(string dbpath, string username)
        {

            string sql = "SELECT a.UserName,a.NickName,a.Remark,b.bigHeadImgUrl,a.rowid \"NAVICAT_ROWID\" FROM \"main\".\"Contact\" a left join \"main\".\"ContactHeadImgUrl\" b on a.UserName=b.usrName where UserName='" + username + "'";
            User user = selectOne<User>(dbpath, sql);
            return user;


        }

        public static List<Message> getMessageByCreatetTime(string dbpath, long time)
        {
            string sql = "SELECT *,rowid \"NAVICAT_ROWID\" FROM \"main\".\"MSG\" WHERE \"CreateTime\" >= "+time+" and StrTalker  like '%@chatroom%' ORDER BY \"CreateTime\"";
            List<Message> messages=selectAll<Message>(dbpath, sql);
            return messages;
        }


    }
}
