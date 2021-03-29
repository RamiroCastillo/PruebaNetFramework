using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Test.Models;
using Test.Models.V1.Responses;

namespace Test.DataAccess.V1
{
    public interface IProduct
    {
        Response GetAllProducts();
    }

    public class Product: IProduct
    {
        private readonly string _connection;
        private readonly int _timeOut;

        public Product(string connectionString, int timeOut)
        {
            _connection = connectionString;
            _timeOut = timeOut;
        }
        public Response GetAllProducts()
        {
            try
            {
                StoreProcedure storeProcedure = new StoreProcedure("dbo.PRODUCT_GetAllProducts");                

                DataTable dataTable = storeProcedure.ReturnData(_connection, _timeOut);
                //Logger.Debug("StoreProcedure: {0} DataTable: {1}", SerializeJson.ToObject(storeProcedure), SerializeJson.ToObject(dataTable));

                if (storeProcedure.Error.Length <= 0)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Rows[0]["RESULTADO"].ToString().Equals("00"))
                        {
                            ProductResponse result = new ProductResponse();
                            result.Products.AddRange(from DataRow dataRow in dataTable.Rows
                                                                     let product = new SimpleProduct()
                                                                     {
                                                                         Code = dataRow["Code"].ToString(),
                                                                         Name = dataRow["Name"].ToString(),
                                                                         Description = dataRow["Description"].ToString(),
                                                                         Category = dataRow["Category"].ToString(),
                                                                         Cost = Convert.ToInt64(dataRow["Cost"]),
                                                                         Price = Convert.ToInt64(dataRow["Price"]),
                                                                         ImageUrl = dataRow["ImageUrl"].ToString(),

                                                                     }
                                                                     select product);
                            return Response.Success(result);
                        }
                        else
                        {
                            //Logger.Debug("Message: {0} DataTable: {1}", Response.CommentMenssage("AlreadyRegisteredEmail"), SerializeJson.ToObject(dataTable));
                            return Response.Error(dataTable, "AlreadyRegisteredEmail");
                        }
                    }
                    else
                    {
                        //Logger.Debug("Message: {0} DataTable: {1}", Response.CommentMenssage("Sql"), SerializeJson.ToObject(dataTable));
                        return Response.Error(dataTable, "Sql");
                    }
                }
                else
                {
                    //Logger.Error("Message: {0} StoreProcedure.Error: {1}", Response.CommentMenssage("Sql"), storeProcedure.Error);
                    return Response.Error(storeProcedure.Error, "Sql");
                }
            }
            catch (Exception ex)
            {
                //Logger.Error("Message: {0} Exception: {1}", ex.Message, SerializeJson.ToObject(ex));
                return Response.Error(ex, "Error");
            }
        }
    }
}
