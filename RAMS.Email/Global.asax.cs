using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace RAMS.Email
{
    /// <summary>
    /// Global class implements global configurations for this project
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        public const string connectionString = "Data Source=AT0MIC-PC;Initial Catalog=RAMSEmail;Integrated Security=False; User Id=sa;Password=sqlserver;";

        public static BackgroundWorker backgroundWorker = new BackgroundWorker();

        public static bool runBackgroundWorker = true; // Static variable that may be used to stop background worker

        /// <summary>
        /// Application_Start method executes as soon as application starts
        /// </summary>
        /// <param name="sender">Sender object is the object that triggered the event</param>
        /// <param name="e">Event arguments are the arguments that sender object may use in come scenatios</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            backgroundWorker.DoWork += new DoWorkEventHandler(DoWork); // Bind DoWork event to DoWork method

            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.WorkerSupportsCancellation = true;

            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkCompleted); // Bind RunWorkerCompleted event to WorkCompleted method

            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Application_End method executes once application is closed
        /// </summary>
        /// <param name="sender">Sender object is the object that triggered the event</param>
        /// <param name="e">Event arguments are the arguments that sender object may use in come scenatios</param>
        protected void Application_End(object sender, EventArgs e)
        {
            // If background worker is still running, stop background worker
            if (backgroundWorker != null)
            {
                backgroundWorker.CancelAsync();
            }
        }

        /// <summary>
        /// DoWork method performs a unit of work and it is triggered by DoWork event of background worker object
        /// </summary>
        /// <param name="sender">Sender object is the object that triggered the event</param>
        /// <param name="args">DoWork event arguments that sender object may use in come scenatios</param>
        private static void DoWork(object sender, DoWorkEventArgs args)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand sqlCommand = new SqlCommand("FetchUnsentEmails", sqlConnection); // Call "FetchUnsentEmails" store procedure to retrieve all unsent emails

            sqlCommand.CommandType = CommandType.StoredProcedure;

            DataSet dataSet = new DataSet();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            sqlDataAdapter.Fill(dataSet); // Fill data set with the data retrieved from store procedure

            if (dataSet.Tables.Count > 0)
            {
                // For each retrieved record call SendEmail method passing the parameters of this record
                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    SendEmail(Int32.Parse(dataRow[0].ToString()), dataRow[1].ToString(), dataRow[2].ToString(), dataRow[3].ToString());
                }
            }
        }

        /// <summary>
        /// WorkCompleted method performs a unit of work and it is triggered by RunWorkerCompleted event of background worker object
        /// </summary>
        /// <param name="sender">Sender object is the object that triggered the event</param>
        /// <param name="args">RunWorkerCompleted event arguments that sender object may use in come scenatios</param>
        private static void WorkCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;

            // Check if current instance of a background worker is still active (Application_End could stop the background worker)
            if (backgroundWorker != null)
            {
                Thread.Sleep(6000);

                if (runBackgroundWorker) // Check if user still wants background worker to run
                {
                    backgroundWorker.RunWorkerAsync(); // Restart background worker
                }
                else
                {
                    while (!runBackgroundWorker) // If user stopped the background worker, keep checking if background worker got restarted
                    {
                        Thread.Sleep(12000);
                    }

                    backgroundWorker.RunWorkerAsync(); // If background worker got restarted, start the background worker
                }
            }
        }

        /// <summary>
        /// SendEmail method is sending the email to specified email address
        /// </summary>
        /// <param name="emailId">Id of the email to be sent (Id is used to updated the status of the email)</param>
        /// <param name="address">Email address where the email is going to be sent</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body (Main content)</param>
        private static void SendEmail(int emailId, string address, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("toronto.coder@gmail.com", "Recruiting Agency");

            mailMessage.To.Add(new MailAddress(address));

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Send(mailMessage);

            UpdateStatus(emailId); // Update email status in the database
        }

        /// <summary>
        /// UpdateStatus method updates the status of the email after the email has been sent
        /// </summary>
        /// <param name="emailId">Id of the email to be updated</param>
        private static void UpdateStatus(int emailId)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand sqlCommand = new SqlCommand("UpdateStatus", sqlConnection);

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@EmailId", emailId);

            sqlConnection.Open();

            sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();
        }

    }
}