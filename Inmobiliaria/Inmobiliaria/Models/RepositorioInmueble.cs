using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
	public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
	{
		public RepositorioInmueble(IConfiguration configuration) : base(configuration)
		{

		}

		public int Alta(Inmueble entidad, int propietarioId)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmueble (Direccion, Ambientes, Uso, Tipo, Precio, Estado, PropietarioId) " +
					"VALUES (@direccion, @ambientes, @uso, @tipo, @precio,@estado, @propietarioId);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", entidad.Direccion);
					command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
					command.Parameters.AddWithValue("@uso", entidad.Uso);
					command.Parameters.AddWithValue("@tipo", entidad.Tipo);
					command.Parameters.AddWithValue("@precio", entidad.Precio);
					command.Parameters.AddWithValue("@estado", entidad.Estado);
					command.Parameters.AddWithValue("@propietarioId", propietarioId);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					entidad.PropietarioId = propietarioId;
					entidad.Id = res;

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
				string sql = $"DELETE FROM Inmueble WHERE Id = {id}";
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

		public int Modificacion(Inmueble entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Inmueble SET " +
					"Direccion=@direccion, Ambientes=@ambientes, Uso=@uso, Tipo=@tipo, Precio=@precio, Estado=@estado " +
					"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@direccion", entidad.Direccion);
					command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
					command.Parameters.AddWithValue("@uso", entidad.Uso);
					command.Parameters.AddWithValue("@tipo", entidad.Tipo);
					command.Parameters.AddWithValue("@precio", entidad.Precio);
					command.Parameters.AddWithValue("@estado", entidad.Estado);
					command.Parameters.AddWithValue("@id", entidad.Id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT i.Id, i.Direccion, Ambientes, Uso, Tipo, Precio, Estado, PropietarioId," +
					" p.Nombre, p.Apellido" +
					" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Uso = reader.GetInt32(3),
							Tipo = reader.GetInt32(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetBoolean(6),
							PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
		public Inmueble ObtenerPorId(int id)
		{
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.Id, i.Direccion, Ambientes, Uso, Tipo, Precio,Estado, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id" +
					$" WHERE i.Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Uso = reader.GetInt32(3),
							Tipo = reader.GetInt32(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetBoolean(6),
							PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
					}
					connection.Close();
				}
			}
			return entidad;
		}

		public IList<Inmueble> BuscarPorPropietario(int idPropietario)
		{
			List<Inmueble> res = new List<Inmueble>();
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.Id, i.Direccion,Ambientes, Uso, Tipo, Precio, Estado, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id" +
					$" WHERE PropietarioId=@idPropietario";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@idPropietario", SqlDbType.Int).Value = idPropietario;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Uso = reader.GetInt32(3),
							Tipo = reader.GetInt32(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetBoolean(6),

							PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public int Alta(Inmueble p)
		{
			throw new NotImplementedException();
		}

		public IList<Inmueble> BuscarDisponibles()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT i.Id, i.Direccion, Ambientes, Uso, Tipo, Precio, Estado, PropietarioId," +
					" p.Nombre, p.Apellido" +
					" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id" +
					" WHERE Estado = 'true'";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Uso = reader.GetInt32(3),
							Tipo = reader.GetInt32(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetBoolean(6),
							PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

		public IList<Contrato> BuscarPorContrato(int idInmueble)
		{

			IList<Contrato> res = new List<Contrato>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT c.Id, c.FechaDesde, c.FechaHasta, c.InquilinoId, i.Nombre, i.Apellido, c.InmuebleId,e.direccion,e.propietarioId " +
					         $"FROM Contrato c INNER JOIN Inquilino i ON i.Id = c.InquilinoId" +
							  " INNER JOIN Inmueble e ON  e.Id = c.InmuebleId" +
							 $" WHERE InmuebleId IN(SELECT id FROM Inmueble WHERE Id =@idInmueble); ";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@idInmueble", SqlDbType.Int).Value = idInmueble;
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
						res.Add(entidad);
					}
					connection.Close();
				}
			}

			return res;
		}

		public IList<Inmueble> BuscarInmueblesDisponibles(DateTime inicio, DateTime fin)
		{
			IList<Inmueble> res = new List<Inmueble>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT i.Id, i.Direccion, Ambientes, Uso, Tipo, Precio, Estado, PropietarioId, p.Nombre, p.Apellido" +
					" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id " +
					"WHERE i.id NOT IN(SELECT InmuebleId " +
					"FROM Contrato WHERE (@inicio >= FechaDesde or @fin >=  FechaDesde)" +
								 " and(@inicio <= FechaHasta or @fin <= FechaHasta) ); ";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@inicio", SqlDbType.Date).Value = inicio;
					command.Parameters.Add("@fin", SqlDbType.Date).Value = fin;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Uso = reader.GetInt32(3),
							Tipo = reader.GetInt32(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetBoolean(6),
							PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
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
