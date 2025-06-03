using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ERP_Component_DAL.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
/*
public int AddPurch(Nerraj Aq)
{
    try
    {
        String ConnectionString = configuration.GetConnectionString("DefaultConnection");
        connection = new SqlConnection(ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = System.Data.CommandType.Text;

        cmd.CommandText = "insert into purchMain ([Descripion],[TotalAmount],[RaisedBy],[RequiredBy]) " + "OUTPUT INSERTED.PurchaseRequsitionID " +
                          "VALUES (@Descripion, @TotalAmount,@RaisedBy, @RequiredBy)";
        cmd.Parameters.AddWithValue("@Descripion", Aq.Descripion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@TotalAmount", Aq.TotalAmount);

        cmd.Parameters.AddWithValue("@RaisedBy", Aq.RaisedBy ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@RequiredBy", Aq.RequiredBy ?? (object)DBNull.Value);


        cmd.Connection = connection;
        connection.Open();
        int PurchaseRequsitionID = Convert.ToInt32(cmd.ExecuteScalar());


        return PurchaseRequsitionID;
    }
    catch (Exception ex)
    {
        throw ex;
    }
    finally
    {
        connection.Close();
    }

}
public bool AddItems(Nerraj Aq)
{
    try
    {
        String ConnectionString = configuration.GetConnectionString("DefaultConnection");
        connection = new SqlConnection(ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = System.Data.CommandType.Text;

        cmd.CommandText = "INSERT INTO purchItems ([PurchaseRequsitionID],[Itemname],[stockkeepingunit],[category],[specification],[unitofmeasure],[gst],[itemtype],[hsncode],[qty]) VALUES (@PurchaseRequsitionID,@Itemname,@stockkeepingunit,@category,@specification,@unitofmeasure,@gst,@itemtype,@hsncode,@qty)";
        cmd.Parameters.AddWithValue("@PurchaseRequsitionID", Aq.PurchaseRequsitionID);
        cmd.Parameters.AddWithValue("@Itemname", Aq.Itemname);
        cmd.Parameters.AddWithValue("@stockkeepingunit", Aq.stockkeepingunit);
        cmd.Parameters.AddWithValue("@category", Aq.category);
        cmd.Parameters.AddWithValue("@specification", Aq.specification);
        cmd.Parameters.AddWithValue("@unitofmeasure", Aq.unitofmeasure);
        cmd.Parameters.AddWithValue("@gst", Aq.gst);
        cmd.Parameters.AddWithValue("@itemtype", Aq.itemtype);
        cmd.Parameters.AddWithValue("@hsncode", Aq.hsncode);
        cmd.Parameters.AddWithValue("@qty", Aq.qty);

        cmd.Connection = connection;
        connection.Open();
        cmd.ExecuteScalar();
        connection.Close();

        return true;
    }
    catch (Exception ex)
    {
        throw ex;
    }
    finally
    {
        connection.Close();
    }
}
public bool updatePurchDetails(Nerraj Aq)
{
    try
    {
        String ConnectionString = configuration.GetConnectionString("DefaultConnection");
        connection = new SqlConnection(ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.CommandText = $"update purchMain Set Descripion='{Aq.Descripion}' , TotalAmount='{Aq.TotalAmount}', RaisedBy='{Aq.RaisedBy}' , RequiredBy='{Aq.RequiredBy}' where PurchaseRequsitionID = '{Aq.PurchaseRequsitionID}' ";



        cmd.Connection = connection;
        connection.Open();
        cmd.ExecuteScalar();
        connection.Close();



        return true;

    }
    catch (Exception ex)
    {
        throw ex;
    }
    finally
    {
        connection.Close();
    }
}
*/