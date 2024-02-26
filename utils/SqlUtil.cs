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
            string connectionString = @"Data source=D:\\MicroMsg.db_mir1.db;Version=3;";
            


            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
               


                dbConnection.Open();

                // 执行查询并映射到对象集合
                List<T> users = dbConnection.Query<T>(sql).AsList();

                return users;
            }
            return null;
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


    }
}
