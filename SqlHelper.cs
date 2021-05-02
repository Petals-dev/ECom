using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.SqlClient;

namespace ZohoIntegration
{
    public class SqlHelper
    {
        string ConnString = string.Empty;
        SqlConnection con = null;
        public SqlHelper()
        {
            ConnString = ConfigurationManager.AppSettings["ConnectionString"];
            con = new SqlConnection(ConnString);

            con.Open();
        }

        public void ImportData(ItemsModel model)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "dbo.SP_INSERT_ZOHO_ITEMS";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = con;

            cmd.Parameters.Add(new SqlParameter("@ZF02", model.ItemId));
            cmd.Parameters.Add(new SqlParameter("@ZF03", model.ItemType));
            cmd.Parameters.Add(new SqlParameter("@ZF04", model.Description));
            cmd.Parameters.Add(new SqlParameter("@ZF05", model.SKU));
            cmd.Parameters.Add(new SqlParameter("@ZF06", model.Unit));
            cmd.Parameters.Add(new SqlParameter("@ZF07", model.Brand));
            cmd.Parameters.Add(new SqlParameter("@ZF08", model.Category));
            cmd.Parameters.Add(new SqlParameter("@ZF09", model.SubCategory));
            cmd.Parameters.Add(new SqlParameter("@ZF10", model.SellingPrice));
            cmd.Parameters.Add(new SqlParameter("@ZF11", model.StockOnHand));
            cmd.Parameters.Add(new SqlParameter("@ZF12", model.AvailableStock));
            cmd.Parameters.Add(new SqlParameter("@ZF13", model.Status));
            cmd.Parameters.Add(new SqlParameter("@ZF14", ""));
           
            cmd.ExecuteNonQuery();
        }

        public void CloseConnection()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}