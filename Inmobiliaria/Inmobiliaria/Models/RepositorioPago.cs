using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioPago :  RepositorioBase, IRepositorioPago
    {

        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {


        }

		public int Alta(Pago entidad, int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Pago (NroPago,FechaPago,Importe, ContratoId) " +
					"VALUES (@nroPago,@fechaPago, @importe, @contratoId);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nroPago", entidad.NroPago);
					command.Parameters.AddWithValue("@fechaPago", entidad.FechaPago);
					command.Parameters.AddWithValue("@importe", entidad.Importe);
					command.Parameters.AddWithValue("@contratoId", id);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					entidad.IdPago = res;
					entidad.ContratoId = id;

					connection.Close();
				}
			}
			return res;
		}

		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Pago WHERE Id = {id}";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modificacion(Pago entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Pago SET " +
					"NroPago=nroPago,FechaPago=@fechaPago, Importe=@importe " +
					"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@nroPago",entidad.NroPago);
					command.Parameters.AddWithValue("@fechaPago", entidad.FechaPago);
					command.Parameters.AddWithValue("@importe", entidad.Importe);
					command.Parameters.AddWithValue("@id", entidad.IdPago);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Pago> ObtenerTodos()
		{
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT p.Id,p.NroPago p.FechaPago,p.Importe,p.ContratoId,c.InquilinoId,c.InmuebleId" +
					" FROM Pago p INNER JOIN Contrato c ON p.ContratoId = c.Id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago entidad = new Pago
						{
							IdPago = reader.GetInt32(0),
							NroPago = reader.GetInt32(1),
							FechaPago = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Contrato = new Contrato
							{
								IdContrato = reader.GetInt32(5),
								InquilinoId = reader.GetInt32(6),
							    InmuebleId = reader.GetInt32(7),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Pago ObtenerPorId(int id)
		{
			Pago entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT p.Id,p.NroPago, p.FechaPago, p.Importe,p.ContratoId,c.InquilinoId,c.InmuebleId" +
					$" FROM Pago p INNER JOIN Contrato c ON p.ContratoId = c.Id" +
					$" WHERE p.Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						 entidad = new Pago
						{
							IdPago = reader.GetInt32(0),
							NroPago=reader.GetInt32(1),
							FechaPago = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Contrato = new Contrato
							{
								IdContrato = reader.GetInt32(4),
								InquilinoId = reader.GetInt32(5),
								InmuebleId = reader.GetInt32(6),
							}
						};
					}
					connection.Close();
				}
			}
			return entidad;
		}

		public IList<Pago> BuscarPorContrato(int id)
		{
			List<Pago> res = new List<Pago>();
			Pago entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT p.Id,p.NroPago, p.FechaPago,p.Importe,p.ContratoId,c.InquilinoId,InmuebleId" +
					$" FROM Pago p INNER JOIN Contrato c ON p.ContratoId = c.Id" +
					$" WHERE ContratoId=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Pago
						{
							IdPago = reader.GetInt32(0),
							NroPago=reader.GetInt32(1),
							FechaPago = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Contrato = new Contrato
							{
								IdContrato = reader.GetInt32(4),
								InquilinoId = reader.GetInt32(5),
								InmuebleId = reader.GetInt32(6),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

        public int Alta(Pago p)
        {
            throw new NotImplementedException();
        }
    }
}
