using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace RAMS.Email
{
    /// <summary>
    /// EmailService class implements email services
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// SendEmail method allows to persist an email to database with status (Sent Status) of false (Not sent)
        /// </summary>
        /// <param name="address">An email address to where an email is going to be sent</param>
        /// <param name="subject">An email subject</param>
        /// <param name="body">An email body (Main content)</param>
        public static void SendEmail(string address, string subject, string body)
        {
            SqlConnection sqlConnection = new SqlConnection(Global.connectionString);

            SqlCommand sqlCommand = new SqlCommand("SendEmail", sqlConnection);

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@Address", address);
            sqlCommand.Parameters.AddWithValue("@Subject", subject);
            sqlCommand.Parameters.AddWithValue("@Body", body);

            sqlConnection.Open();

            sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();
        }
    }
}