using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
	{
	
		public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {

        }
		public int ValidarDisponibilidad(DateTime fechaDesde,DateTime fechaHasta, int inmuebleId) 
		{
			var FechaDesde = fechaDesde.ToShortDateString();
			var FechaHasta= fechaHasta.ToShortDateString();
		
			IList<Contrato> res = new List<Contrato>();
			int disponibilidad = 0;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT InmuebleId " +
							" FROM Contrato WHERE (@fechaDesde >= FechaDesde or @fechaHasta >=  FechaDesde)" +
                                 " and(@fechaDesde <= FechaHasta or @fechaHasta <= FechaHasta) " +
								 "  and Id=@inmuebleId ";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@fechaDesde", SqlDbType.Date).Value = fechaDesde;
					command.Parameters.Add("@fechaHasta", SqlDbType.Date).Value = fechaHasta;
					command.Parameters.Add("@inmuebleId", SqlDbType.Int).Value = inmuebleId;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato entidad = new Contrato
						{
							InmuebleId = reader.GetInt32(0),

						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}

			if (res.Count() != 0) { disponibilidad = -1; }else { disponibilidad = 1; }

			return disponibilidad;
		}
		public int Alta(Contrato entidad)
		{
			int res = -1;

			DateTime ingreso = entidad.FechaDesde;
			DateTime salida = entidad.FechaHasta;

			int disponibilidad=ValidarDisponibilidad(ingreso,salida,entidad.InmuebleId);

			if (disponibilidad == 1)
			{

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					string sql = $"INSERT INTO Contrato (FechaDesde,FechaHasta,InquilinoId,InmuebleId) " +
						"VALUES (@fechaDesde, @fechaHasta, @inquilinoId, @inmuebleId);" +
						"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
					using (var command = new SqlCommand(sql, connection))
					{
						command.CommandType = CommandType.Text;
						command.Parameters.AddWithValue("@fechaDesde", entidad.FechaDesde);
						command.Parameters.AddWithValue("@fechaHasta", entidad.FechaHasta);
						command.Parameters.AddWithValue("@inquilinoId", entidad.InquilinoId);
						command.Parameters.AddWithValue("@inmuebleId", entidad.InmuebleId);
						connection.Open();
						res = Convert.ToInt32(command.ExecuteScalar());
						entidad.Id = res;
						connection.Close();
					}
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Contrato WHERE Id = {id}";
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

		public int Modificacion(Contrato entidad)
		{
			int res = -1;
			int disponibilidad = ValidarDisponibilidad(entidad.FechaDesde, entidad.FechaHasta, entidad.InmuebleId);

			if (disponibilidad == 1)
			{

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					string sql = "UPDATE Contrato SET " +
						"FechaDesde=@fechaDesde, FechaHasta=@fechaHasta, InquilinoId=@inquilinoId, InmuebleId=@inmuebleId " +
						"WHERE Id = @id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@fechaDesde", entidad.FechaDesde);
						command.Parameters.AddWithValue("@fechaHasta", entidad.FechaHasta);
						command.Parameters.AddWithValue("@inquilinoId", entidad.InquilinoId);
						command.Parameters.AddWithValue("@inmuebleId", entidad.InmuebleId);
						command.Parameters.AddWithValue("@id", entidad.Id);
						command.CommandType = CommandType.Text;
						connection.Open();
						res = command.ExecuteNonQuery();
						connection.Close();
					}
				}
			}
			return res;
		}

		public IList<Contrato> ObtenerTodos()
		{
			IList<Contrato> res = new List<Contrato>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT c.Id, c.FechaDesde, c.FechaHasta, c.InquilinoId, i.Nombre, i.Apellido, c.InmuebleId,e.direccion,e.propietarioId"+
							  " FROM Contrato c INNER JOIN Inquilino i ON i.Id = c.InquilinoId" +
							  " INNER JOIN Inmueble e ON  e.Id = c.InmuebleId";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato entidad = new Contrato
						{
							Id = reader.GetInt32(0),
							FechaDesde = reader.GetDateTime(1),
							FechaHasta = reader.GetDateTime(2),

							InquilinoId = reader.GetInt32(3),
							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(3),
								Nombre = reader.GetString(4),
								Apellido = reader.GetString(5),
							},

							InmuebleId = reader.GetInt32(6),
							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(6),
								Direccion= reader.GetString(7),
								PropietarioId = reader.GetInt32(8),
							}
						};
						
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Contrato ObtenerPorId(int id)
		{
			Contrato entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT c.Id, c.FechaDesde, c.FechaHasta, c.InquilinoId, i.Nombre, i.Apellido, c.InmuebleId,e.direccion,e.propietarioId" +
							  " FROM Contrato c INNER JOIN Inquilino i ON i.Id = c.InquilinoId" +
							  " INNER JOIN Inmueble e ON  e.Id = c.InmuebleId" +
							  " WHERE c.Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Contrato
						{
							Id= reader.GetInt32(0),
							FechaDesde = reader.GetDateTime(1),
						    FechaHasta = reader.GetDateTime(2),
							 
							InquilinoId = reader.GetInt32(3),
							Inquilino = new Inquilino
							{
								Id= reader.GetInt32(3),
								Nombre = reader.GetString(4),
								Apellido = reader.GetString(5),
							},

							InmuebleId = reader.GetInt32(6),
							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(6),
								Direccion = reader.GetString(7),
								PropietarioId = reader.GetInt32(8),
							}

						};
					}
					connection.Close();
				}
			}
			return entidad;
		}

		public IList<Contrato> ContratosVigentes(DateTime inicio, DateTime fin) 
		{
			IList<Contrato> res = new List<Contrato>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT c.Id, c.FechaDesde, c.FechaHasta, c.InquilinoId, i.Nombre, i.Apellido, c.InmuebleId,e.direccion,e.propietarioId" +
							  " FROM Contrato c INNER JOIN Inquilino i ON i.Id = c.InquilinoId" +
							  " INNER JOIN Inmueble e ON  e.Id = c.InmuebleId"+
							  " WHERE FechaDesde <= @fin AND FechaHasta >= @inicio;";
				
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@inicio", SqlDbType.Date).Value = inicio;
					command.Parameters.Add("@fin", SqlDbType.Date).Value = fin;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato entidad = new Contrato
						{
							Id = reader.GetInt32(0),
							FechaDesde = reader.GetDateTime(1),
							FechaHasta = reader.GetDateTime(2),

							InquilinoId = reader.GetInt32(3),
							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(3),
								Nombre = reader.GetString(4),
								Apellido = reader.GetString(5),
							},

							InmuebleId = reader.GetInt32(6),
							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(6),
								Direccion = reader.GetString(7),
								PropietarioId = reader.GetInt32(8),
							}
						};

						res.Add(entidad);
					}
					connection.Close();
				}
			}


			return res;
		}
		


	}
}
