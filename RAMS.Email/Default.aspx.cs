using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RAMS.Email
{
    /// <summary>
    /// Default page is displayed when default url is accessed or on project startup
    /// </summary>
    public partial class Default : System.Web.UI.Page
    {
        /// <summary>
        /// Page_Load method executes every time related page is requested
        /// </summary>
        /// <param name="sender">Sender object is the object that triggered the event</param>
        /// <param name="e">Event arguments are the arguments that sender object may use in come scenatios</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvEmails.DataSource = this.GetEmails();

                gvEmails.DataBind();
            }
        }

        /// <summary>
        /// GetEmails method calls stored procedure to fetch the emails and return the result as a DataSet
        /// </summary>
        /// <returns>DataSet with fetched emails</returns>
        private DataSet GetEmails()
        {
            using (SqlConnection sqlConnection = new SqlConnection(Global.connectionString))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("FetchEmails", sqlConnection);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                DataSet dataSet = new DataSet("Emails");

                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter())
                {
                    sqlDataAdapter.SelectCommand = sqlCommand;

                    sqlDataAdapter.Fill(dataSet);
                }

                return dataSet;
            }
        }
    }
}